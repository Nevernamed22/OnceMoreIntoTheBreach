using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    public class BootLeg : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<BootLeg>(
            "Legboot",
            "Knock It Off",
            "From a peculiar and less graphically appealing alternate dimension where dodge rolling through bullets or into enemies restores ammo." + "\n\nThis boot is as long as your entire leg!",
            "legboot_icon");
            item.quality = PickupObject.ItemQuality.A;
        }


        private void onDodgeRolledOverBullet(Projectile bullet)
        {
            if (Owner.CurrentGun != null) Owner.CurrentGun.GainAmmo(1);
        }
        private void onDodgeRolledIntoEnemy(PlayerController player, AIActor enemy)
        {
            if (Owner.CurrentGun != null) Owner.CurrentGun.GainAmmo(5);
        }

        public override void Pickup(PlayerController player)
        {
            player.OnDodgedProjectile += this.onDodgeRolledOverBullet;
            player.OnRolledIntoEnemy += this.onDodgeRolledIntoEnemy;
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject result = base.Drop(player);
            player.OnDodgedProjectile -= this.onDodgeRolledOverBullet;
            player.OnRolledIntoEnemy -= this.onDodgeRolledIntoEnemy;
            return result;
        }
        public override void OnDestroy()
        {
            if (Owner)
            {
                Owner.OnDodgedProjectile -= this.onDodgeRolledOverBullet;
                Owner.OnRolledIntoEnemy -= this.onDodgeRolledIntoEnemy;
            }
            base.OnDestroy();
        }
    }
}