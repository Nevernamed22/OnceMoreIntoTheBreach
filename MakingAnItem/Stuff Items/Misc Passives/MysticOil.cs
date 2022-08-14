using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using ItemAPI;
using SaveAPI;

namespace NevernamedsItems
{
    public class MysticOil : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Sanctified Oil";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/mysticoil_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<MysticOil>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Soyled It";
            string longDesc = "Oil supposedly used to shine the glittering barrels and gleaming chambers of Bullet Heaven, though the existence of the place is but a mere rumour.\n\n" + "Works best on Automatic weapons.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.RateOfFire, 100f, StatModifier.ModifyMethod.ADDITIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Damage, 0.2f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.AmmoCapacityMultiplier, 5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.ReloadSpeed, 0.01f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.ChargeAmountMultiplier, 100f, StatModifier.ModifyMethod.MULTIPLICATIVE);

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.A;
            item.AddToSubShop(ItemBuilder.ShopType.Goopton);
            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.PURCHASED_SANCTIFIEDOIL, true);
            item.AddItemToGooptonMetaShop(27);
        }
        public override void Pickup(PlayerController player)
        {
            bool pickemmed = m_pickedUpThisRun;
            player.PostProcessProjectile += this.DoEffect;
            base.Pickup(player);
            if (!pickemmed)
            {
                for (int i = 0; i < Owner.inventory.AllGuns.Count; i++)
                {
                    if (Owner.inventory.AllGuns[i] != null && Owner.inventory.AllGuns[i].CanGainAmmo)
                    {
                        Owner.inventory.AllGuns[i].GainAmmo(Owner.inventory.AllGuns[i].AdjustedMaxAmmo);
                        Owner.inventory.AllGuns[i].ForceImmediateReload(false);
                    }
                }
            }
        }
        public override DebrisObject Drop(PlayerController player)
        {
            player.PostProcessProjectile -= this.DoEffect;
            DebrisObject debrisObject = base.Drop(player);
            return debrisObject;
        }
        public override void OnDestroy()
        {
            if (Owner)
            {
                Owner.PostProcessProjectile -= this.DoEffect;
            }
            base.OnDestroy();
        }
        private void DoEffect(Projectile projectile, float effectChanceScalar)
        {
            if (Owner.CurrentGun != null)
            {
                Invoke("GiveBulletBack", 0.1f);
            }
        }
        private void GiveBulletBack()
        {
            Owner.CurrentGun.MoveBulletsIntoClip(1);
        }
    }
}
