using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dungeonator;
using Alexandria.ItemAPI;
using UnityEngine;

namespace NevernamedsItems
{
    class FinishedBullet : PlayerItem
    {
        public static void Init()
        {
            PlayerItem item = ItemSetup.NewItem<FinishedBullet>(
              "Finished Bullet",
              "Let's Finish This",
              "A single bullet from the legendary 'Finished Gun'." + "\n\nEven without the Gun to fire it, a good throwing arm and plenty of resolve can achieve wonderful results.",
              "finishedbullet_icon") as PlayerItem;
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 240);
            item.AddPassiveStatModifier( PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE);
            item.consumable = false;
            item.quality = ItemQuality.D;
        }
        public override void DoEffect(PlayerController user)
        {
            Projectile projectile = ((Gun)ETGMod.Databases.Items[762]).DefaultModule.finalProjectile;
            GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, user.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (user.CurrentGun == null) ? 0f : user.CurrentGun.CurrentAngle), true);
            Projectile component = gameObject.GetComponent<Projectile>();
            if (component != null)
            {
                component.Owner = user;
                component.Shooter = user.specRigidbody;
                component.baseData.damage = 1f;
            }
        }
    }
}
