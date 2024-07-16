using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Alexandria.ItemAPI;
using UnityEngine;
using MonoMod.RuntimeDetour;
using System.Reflection;

namespace NevernamedsItems
{
    public class Ammolite : PassiveItem
    {
        public static void Init()
        {
            PassiveItem item = ItemSetup.NewItem<Ammolite>(
              "Ammolite",
              "Gemmed Shells",
              "Refilling a gun with ammo permanently increases it's damage by 5%." + "\n\nThese beautiful opal-like gemstones are found in strange abundance within Gunymede's crust.",
              "ammolite_icon") as PassiveItem;           
            item.quality = PickupObject.ItemQuality.B;
        }
        Hook ammoPickupHook = new Hook(
                typeof(AmmoPickup).GetMethod("Pickup", BindingFlags.Instance | BindingFlags.Public),
                typeof(Ammolite).GetMethod("ammoPickupHookMethod")
            );
        public static void ammoPickupHookMethod(Action<AmmoPickup, PlayerController> orig, AmmoPickup self, PlayerController player)
        {
            orig(self, player);
            //ETGModConsole.Log("Ammo pickup was triggered");
            if (player.HasPickupID(Gungeon.Game.Items["nn:ammolite"].PickupObjectId))
            {
                if (canGiveDMG)
                {
                    if (player.CurrentGun.GetComponent<AmmoliteModifier>())
                    {
                        AmmoliteModifier ammoliteMod = player.CurrentGun.GetComponent<AmmoliteModifier>();
                        ammoliteMod.TimesPickedUpAmmo += 1;                 
                    }
                    else
                    {
                        player.CurrentGun.gameObject.AddComponent<AmmoliteModifier>();
                    }
                    canGiveDMG = false;
                }
                else
                {
                    canGiveDMG = true;
                }
            }
        }
        static bool canGiveDMG = false;
        public class AmmoliteModifier : GunBehaviour
        {
            public int TimesPickedUpAmmo = 1;
            public override void PostProcessProjectile(Projectile projectile)
            {
                PlayerController playerController = this.gun.CurrentOwner as PlayerController;
                float damagemod = (0.05f * TimesPickedUpAmmo) + 1;
                projectile.baseData.damage *= damagemod;
                base.PostProcessProjectile(projectile);
            }
        }
    }
}

