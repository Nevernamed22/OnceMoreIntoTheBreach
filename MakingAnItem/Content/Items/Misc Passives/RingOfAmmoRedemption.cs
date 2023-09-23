using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    public class RingOfAmmoRedemption : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<RingOfAmmoRedemption>(
            "Ring of Ammo Redemption",
            "Thrown Guns Reload",
            "Killing an enemy with a thrown gun restores 10% of that gun's ammo." + "\n\nThis ring once belonged to Peale-4, an infamously loot-oriented Gungeoneer." + "\nHis ring is so hungry for loot that it encourages your guns to steal ammo directly from the dead.",
            "ringofammoredemption_icon");
            item.quality = PickupObject.ItemQuality.D; 
            RingOfAmmoRedemptionID = item.PickupObjectId;
        }
        public static int RingOfAmmoRedemptionID;
        private void PostProcessThrownGun(Projectile thrownGunProjectile)
        {
            thrownGunProjectile.OnHitEnemy += this.RestoreAmmo;
            //foreach (Component component in thrownGunProjectile.GetComponents<Component>())
            //{
               // ETGModConsole.Log(component.GetType().ToString());
            //}
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessThrownGun += this.PostProcessThrownGun;
        }
        private void RestoreAmmo(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
        {
            if (arg3)
            {
                float restorePercent = 0.10f;
                if ((arg1.Owner as PlayerController).PlayerHasActiveSynergy("Ammo Economy Inflation")) restorePercent = 0.20f;
                Gun gun = arg1.GetComponentInChildren<Gun>();
                if (gun)
                {
                    gun.GainAmmo(Mathf.FloorToInt(gun.AdjustedMaxAmmo * restorePercent));
                }
            }
        }
        public override DebrisObject Drop(PlayerController player)
        {
            player.PostProcessThrownGun -= this.PostProcessThrownGun;
            return base.Drop(player);
        }
        public override void OnDestroy()
        {
            if (Owner != null)
            {
            Owner.PostProcessThrownGun -= this.PostProcessThrownGun;
            }
            base.OnDestroy();
        }
    }
}
