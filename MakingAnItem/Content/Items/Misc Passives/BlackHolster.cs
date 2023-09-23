using System;
using System.Collections.Generic;
using System.Linq;
using Gungeon;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    public class BlackHolster : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<BlackHolster>(
            "Black Holster",
            "Unload",
            "Chance to unholster the full power of the void upon reloading." + "\n\nOnce sat on the hip of a gunslinger that was neither man nor beast, nor even flesh, but given form by the void itself.",
            "blackholster_icon");
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE);
            item.quality = PickupObject.ItemQuality.A;
        }
        private void HandleGunReloaded(PlayerController player, Gun playerGun)
        {
            if (playerGun.ClipShotsRemaining == 0 && UnityEngine.Random.value > 0.85f)
            {
                Projectile projectile2 = ((Gun)ETGMod.Databases.Items["black_hole_gun"]).DefaultModule.projectiles[0];
                GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, base.Owner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle), true);
                Projectile component = gameObject.GetComponent<Projectile>();
                if (component != null)
                {
                    component.Owner = base.Owner;
                    component.Shooter = base.Owner.specRigidbody;
                    component.baseData.speed = 5f;
                    component.baseData.damage = 3f;
                }
            }
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnReloadedGun += this.HandleGunReloaded;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.OnReloadedGun -= this.HandleGunReloaded;
            return debrisObject;
        }
        public override void OnDestroy()
        {
            if (Owner)
            {
                Owner.OnReloadedGun -= this.HandleGunReloaded;
            }
            base.OnDestroy();
        }
    }

}