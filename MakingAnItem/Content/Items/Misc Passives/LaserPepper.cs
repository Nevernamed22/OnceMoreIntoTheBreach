﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using MonoMod.RuntimeDetour;
using System.Reflection;
using Dungeonator;
using Alexandria.Misc;

namespace NevernamedsItems
{
    public class LaserPepper : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Laser Pepper";
            string resourceName = "NevernamedsItems/Resources/NeoItemSprites/laserpepper_icon";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<LaserPepper>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Burns the Tongue";
            string longDesc = "A delectable condemnation of the scientific drive to create spicier and spicier peppers. \n\nThey were so preoccupied with whether they could, they didn't stop to think if they should.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            item.CanBeDropped = true;
            item.quality = PickupObject.ItemQuality.B;
        }
        private float timer;
        public override void Update()
        {
            if (Owner)
            {
                if (timer <= 0)
                {
                    timer = UnityEngine.Random.Range(1f, 6f);
                    if (Owner.CenterPosition.GetNearestEnemyToPosition() != null && (Vector2.Distance(Owner.CenterPosition.GetNearestEnemyToPosition().Position, Owner.CenterPosition) <= 5))
                    {
                        AIActor enemy = Owner.CenterPosition.GetNearestEnemyToPosition();
                        UnityEngine.Object.Instantiate<GameObject>(LaserKnife.laserSlashVFX, enemy.sprite.WorldCenter, Quaternion.identity);
                        if (enemy && (!enemy.healthHaver || !enemy.healthHaver.IsBoss))
                        {
                            GameManager.Instance.Dungeon.StartCoroutine(LaserKnife.HandleEnemyDeath(enemy, Owner.CenterPosition.GetVectorToNearestEnemy()));
                        }
                        else if (enemy && enemy.healthHaver && enemy.healthHaver.IsBoss)
                        {
                            enemy.healthHaver.ApplyDamage(100, Vector2.zero, "Laser Pepper", CoreDamageTypes.None, DamageCategory.Unstoppable, true, null, true);
                        }
                        AkSoundEngine.PostEvent("Play_WPN_bountyhunterarm_shot_03", Owner.gameObject);
                    }
                }
                else
                {
                    timer -= BraveTime.DeltaTime;
                }
            }
            base.Update();
        }
    }
}
