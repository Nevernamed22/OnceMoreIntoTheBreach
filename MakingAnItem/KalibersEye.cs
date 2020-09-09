using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dungeonator;
using UnityEngine;
using ItemAPI;

namespace NevernamedsItems
{
    class KalibersEye : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Kaliber's Eye";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/kaliberseye_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<KalibersEye>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "They are mine.";
            string longDesc = "Makes the Gundead your own."+"\n\nDestroy them, but do not waste them.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE);

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.S;

            //YELLOW CHAMBER SYNERGY --> All Seeing
            List<string> mandatorySynergyItems = new List<string>() { "nn:kaliber's_eye", "yellow_chamber" };
            CustomSynergies.Add("All Seeing", mandatorySynergyItems);

            List<string> mandatorySynergyItems2 = new List<string>() { "nn:miners_bullets" };
            List<string> optionalSynergyItems2 = new List<string>() { "nn:kaliber's_eye", "bloody_eye", "rolling_eye", "eye_of_the_beholster" };
            CustomSynergies.Add("Eye of the Spider", mandatorySynergyItems2, optionalSynergyItems2);

            //NPC Pools
            item.AddToSubShop(ItemBuilder.ShopType.Cursula);
        }
        private void OnEnemyDamaged(float damage, bool fatal, HealthHaver enemy)
        {
            if (enemy.aiActor && fatal && !enemy.aiActor.IgnoreForRoomClear && !enemy.IsBoss && Owner.IsInCombat && enemy.aiActor.EnemyGuid != "249db525a9464e5282d02162c88e0357")
            {
                float procChance;
                if (Owner.HasPickupID(570)) procChance = 1f;
                else procChance = 0.5f;
                if (UnityEngine.Random.value < procChance)
                {
                    bool isJammed = false;
                    string enemyGuid = enemy.aiActor.EnemyGuid;
                    if (enemy.aiActor.IsBlackPhantom)
                    {
                        isJammed = true;
                    }
                    //ETGModConsole.Log("Jammed State: " + isJammed);
                    Vector2 nearbyPoint = (Owner.CenterPosition + new Vector2(3, 3));
                    IntVector2? intVector = Owner.CurrentRoom.GetNearestAvailableCell(nearbyPoint, new IntVector2?(IntVector2.One), new CellTypes?(CellTypes.FLOOR), false, null);
                    if (intVector.HasValue)
                    {
                        AIActor TargetActor = AIActor.Spawn(enemy.aiActor, intVector.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(intVector.Value), true, AIActor.AwakenAnimationType.Default, true);
                        PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(TargetActor.specRigidbody, null, false);
                        if (isJammed == true)
                        {
                            TargetActor.BecomeBlackPhantom();
                        }
                        TargetActor.ApplyEffect(GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultPermanentCharmEffect, 1f, null);
                        TargetActor.gameObject.AddComponent<KillOnRoomClear>();
                        TargetActor.IsHarmlessEnemy = true;
                        TargetActor.IgnoreForRoomClear = true;
                        if (TargetActor.gameObject.GetComponent<SpawnEnemyOnDeath>())
                        {
                            Destroy(TargetActor.gameObject.GetComponent<SpawnEnemyOnDeath>());
                        }
                    }
                }
            }
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