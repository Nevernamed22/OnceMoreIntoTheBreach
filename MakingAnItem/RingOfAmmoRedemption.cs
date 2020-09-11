using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using ItemAPI;

namespace NevernamedsItems
{
    public class RingOfAmmoRedemption : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Ring of Ammo Redemption";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/pearlbracelet_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<RingOfAmmoRedemption>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Thrown Guns Afflict";
            string longDesc = "Thrown guns afflict a whole host of status effects, and return to their owner." + "\n\nPearls aren't really proper gemstones, but the people who make these are Wizards, not Geologists.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.EXCLUDED; //D
        }
        private void PostProcessThrownGun(Projectile thrownGunProjectile)
        {
            thrownGunProjectile.OnHitEnemy += this.RestoreAmmo;
            foreach (Component component in thrownGunProjectile.GetComponents<Component>())
            {
                ETGModConsole.Log(component.GetType().ToString());
            }
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
                Gun gun = arg1.GetComponent<Gun>();
                if (gun)
                {
                    gun.GainAmmo(Mathf.FloorToInt(gun.AdjustedMaxAmmo * 0.5f));
                }
                else
                {
                    //ETGModConsole.Log("Could not find gun component.");
                }
            }
            else
            {
                ETGModConsole.Log("arg3 unsatisfied");
            }
        }
        public override DebrisObject Drop(PlayerController player)
        {
            player.PostProcessThrownGun -= this.PostProcessThrownGun;
            return base.Drop(player);
        }
        protected override void OnDestroy()
        {
            Owner.PostProcessThrownGun -= this.PostProcessThrownGun;
            base.OnDestroy();
        }
    }
}
