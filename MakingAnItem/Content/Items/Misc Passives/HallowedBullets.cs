using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;
using Dungeonator;
using SaveAPI;

namespace NevernamedsItems
{
    public class HallowedBullets : IntersectEnemyBulletsItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<HallowedBullets>(
             "Hallowed Bullets",
             "Bullet Holeys",
             "These bullets are so blessed that they will preach to other projectiles mid-air. Usually this does nothing, but it does give Jammed projectiles a chance at redemption.",
             "hallowedbullets_improved");
            item.quality = PickupObject.ItemQuality.D;
            item.SetTag("bullet_modifier");
            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.ALLJAMMED_BEATEN_KEEP, true);
            Doug.AddToLootPool(item.PickupObjectId);
        }
        public void onFiredGun(Projectile bullet, float eventchancescaler)
        {           
            bullet.BlackPhantomDamageMultiplier *= 1.1f;              
        }        
        public override void Pickup(PlayerController player)
        {
            player.PostProcessProjectile += this.onFiredGun;
            base.Pickup(player);
        }
        public override void DisableEffect(PlayerController player)
        {
            if (player) player.PostProcessProjectile -= this.onFiredGun;
            base.DisableEffect(player);
        }

        public override void DoIntersectionEffect(Projectile playerBullet, Projectile enemyBullet)
        {
            if (enemyBullet.IsBlackBullet)
            {
                enemyBullet.ReturnFromBlackBullet();
            }
            base.DoIntersectionEffect(playerBullet, enemyBullet);
        }

    }
}