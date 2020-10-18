using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using ItemAPI;
using UnityEngine;
using MonoMod.RuntimeDetour;
using System.Reflection;

namespace NevernamedsItems
{
    public class Ammolite : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Ammolite";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/ammolite_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<Ammolite>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Gemmed Shells";
            string longDesc = "Refilling a gun with ammo permanently increases it's damage by 5%."+"\n\nThese beautiful opal-like gemstones are found in strange abundance within Gunymede's crust.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item


            //Set the rarity of the item
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

