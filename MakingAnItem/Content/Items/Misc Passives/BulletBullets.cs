using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    public class BulletBullets : IntersectEnemyBulletsItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<BulletBullets>(
             "Bullet Bullets",
             "Score!",
             "When your bullets pass through enemy bullets, a single bullet will be reloaded into your clip." + "\n\nStrategic aiming can allow a gungeoneer to avoid the irritation of reloading.",
             "bulletbullets_icon");
            item.quality = PickupObject.ItemQuality.D;
            item.SetTag("bullet_modifier");
        }       
        private Projectile lastProjectile = null;
        public override void DoIntersectionEffect(Projectile playerBullet, Projectile enemyBullet)
        {
            if (enemyBullet != lastProjectile)
            {
                Owner.CurrentGun.MoveBulletsIntoClip(1);
                lastProjectile = enemyBullet;
            }
            else if (Owner.HasPickupID(375))
            {
                Owner.CurrentGun.MoveBulletsIntoClip(1);
                lastProjectile = enemyBullet;
            }
            base.DoIntersectionEffect(playerBullet, enemyBullet);
        }
    }

}

