﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dungeonator;
using UnityEngine;
using ItemAPI;
using System.Collections;

namespace NevernamedsItems
{
    class Voodoollets : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Voodoollets";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/voodoollets_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<Voodoollets>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Doll-ay, Oh, Oh, We Come Doll-ay!";
            string longDesc = "Whenever any enemy suffers damage, another shall be wounded in kind." + "\n\nA relic left behind by a strange cult of voodoo worshippers, who sought to open a portal to Bullet Heaven." + "\nThey vanished without a trace. Perhaps what awaited them was not the heaven they had hoped." + "\n\nKaliba Eléison";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.A;
            item.AddToSubShop(ItemBuilder.ShopType.Cursula);
        }

        //SYNERGY WITH SPARE KEY --> "Spare Keybullet Kin"
        private void OnEnemyDamaged(float damage, bool fatal, HealthHaver enemy)
        {
            try
            {
                if (enemy.aiActor && enemy)
                {
                    if (fatal && Owner.HasPickupID(71))
                    {
                        if (UnityEngine.Random.value < 0.1f)
                        {
                            SpawnObjectPlayerItem bigbombPrefab = PickupObjectDatabase.GetById(71).GetComponent<SpawnObjectPlayerItem>();
                            GameObject bigbombObject = bigbombPrefab.objectToSpawn.gameObject;

                            GameObject bigbombObject2 = UnityEngine.Object.Instantiate<GameObject>(bigbombObject, enemy.sprite.WorldBottomCenter, Quaternion.identity);
                            tk2dBaseSprite bombsprite = bigbombObject2.GetComponent<tk2dBaseSprite>();
                            if (bombsprite)
                            {
                                bombsprite.PlaceAtPositionByAnchor(enemy.sprite.WorldBottomCenter, tk2dBaseSprite.Anchor.MiddleCenter);
                            }
                        }
                    }
                    float outcomeDamage = damage;
                    if (Owner.HasPickupID(442)) outcomeDamage *= 2;
                    RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
                    List<AIActor> enemiesInRoom = new List<AIActor>();

                    if (absoluteRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear) != null)
                    {
                        foreach (AIActor m_Enemy in absoluteRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear))
                        {
                            enemiesInRoom.Add(m_Enemy);
                        }
                    }
                    AIActor randomActiveEnemy = null;
                    AIActor randomActiveOTHEREnemy = null;
                    if (enemiesInRoom.Count > 1)
                    {
                        enemiesInRoom.Remove(enemy.aiActor);
                        randomActiveEnemy = BraveUtility.RandomElement(enemiesInRoom);
                        if (enemiesInRoom.Count > 2 && (Owner.HasPickupID(276) || Owner.HasPickupID(149) || Owner.HasPickupID(482) || Owner.HasPickupID(506) || Owner.HasPickupID(172) || Owner.HasPickupID(198) || Owner.HasPickupID(Gungeon.Game.Items["nn:spectre_bullets"].PickupObjectId)))
                        {
                            enemiesInRoom.Remove(randomActiveEnemy);
                            randomActiveOTHEREnemy = BraveUtility.RandomElement(enemiesInRoom);
                        }
                    }

                    if (randomActiveEnemy != null && randomActiveEnemy != enemy.aiActor && randomActiveEnemy.healthHaver && randomActiveEnemy.healthHaver.IsAlive && randomActiveEnemy.healthHaver.IsVulnerable)
                    {
                        if (OnCooldownVoodoo == false)
                        {
                            OnCooldownVoodoo = true;
                            
                            if (Owner.HasPickupID(527) && UnityEngine.Random.value < 0.25f) randomActiveEnemy.gameActor.ApplyEffect(this.charmEffect, 1f, null);
                            randomActiveEnemy.healthHaver.ApplyDamage(outcomeDamage, Vector2.zero, "Voodoo Magic", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                            
                            if ((Owner.HasPickupID(276) || Owner.HasPickupID(149) || Owner.HasPickupID(482) || Owner.HasPickupID(506) || Owner.HasPickupID(172) || Owner.HasPickupID(198) || Owner.HasPickupID(Gungeon.Game.Items["nn:spectre_bullets"].PickupObjectId)) && randomActiveOTHEREnemy != null && randomActiveOTHEREnemy != enemy.aiActor && randomActiveOTHEREnemy.healthHaver && randomActiveOTHEREnemy.healthHaver.IsAlive && randomActiveOTHEREnemy.healthHaver.IsVulnerable)
                            {
                                if (Owner.HasPickupID(527) && UnityEngine.Random.value < 0.25f) randomActiveOTHEREnemy.gameActor.ApplyEffect(this.charmEffect, 1f, null);
                                randomActiveOTHEREnemy.healthHaver.ApplyDamage(outcomeDamage, Vector2.zero, "Voodoo Magic", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                            }
                            Invoke("Cooldown", 0.01f);
                        }
                    }

                }
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
                ETGModConsole.Log(e.StackTrace);
                ETGModConsole.Log("IF YOU SEE THIS PLEASE REPORT IT TO NEVERNAMED (WITH SCREENSHOTS)");
            }
        }
        GameActorCharmEffect charmEffect = Gungeon.Game.Items["charming_rounds"].GetComponent<BulletStatusEffectItem>().CharmModifierEffect;

        public static bool OnCooldownVoodoo;
        private void Cooldown()
        {
            OnCooldownVoodoo = false;
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnAnyEnemyReceivedDamage += this.OnEnemyDamaged;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.OnAnyEnemyReceivedDamage -= this.OnEnemyDamaged;
            return debrisObject;
        }
        protected override void OnDestroy()
        {
            Owner.OnAnyEnemyReceivedDamage -= this.OnEnemyDamaged;
            base.OnDestroy();
        }
    }
}
