using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dungeonator;
using Alexandria.ItemAPI;
using SaveAPI;
using UnityEngine;

namespace NevernamedsItems
{
    class MagicMissile : PlayerItem
    {
        public static void Init()
        {
            PlayerItem item = ItemSetup.NewItem<MagicMissile>(
              "Magic Missile",
              "Dank Dungeon Walls",
              "An ancient art, kept sacred by a sect of Gunjurers deep within the Hollow's caverns." + "\n\nImbued with physical form in an attempt to preserve it for future generations." + "\n\nWorks best in the dark.",
              "magicmissile_icon") as PlayerItem;
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 540);
            item.consumable = false;
            item.quality = ItemQuality.C;
            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.PURCHASED_MAGICMISSILE, true);
            item.AddItemToDougMetaShop(20);

            ID = item.PickupObjectId;
        }
        public static int ID;
        public bool isGivingDarknessImmunity = false;
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnEnteredCombat += OnEnteredCombat;
            if (!isGivingDarknessImmunity)
            {
                CustomDarknessHandler.shouldBeLightOverride.SetOverride("MagicMissile", true);
                isGivingDarknessImmunity = true;
            }
        }
        private void OnEnteredCombat()
        {
            if (LastOwner != null && LastOwner.CurrentRoom != null)
            {
                if (LastOwner.CurrentRoom.IsDarkAndTerrifying)
                {
                    LastOwner.CurrentRoom.EndTerrifyingDarkRoom();
                }
            }
        }
        public override void OnDestroy()
        {
            if (LastOwner)
            {
                LastOwner.OnEnteredCombat += OnEnteredCombat;
            }
            if (isGivingDarknessImmunity)
            {
                CustomDarknessHandler.shouldBeLightOverride.RemoveOverride("MagicMissile");
                isGivingDarknessImmunity = false;
            }
            base.OnDestroy();
        }
        public override void OnPreDrop(PlayerController user)
        {
            base.OnPreDrop(user);
            user.OnEnteredCombat -= OnEnteredCombat;

            if (isGivingDarknessImmunity)
            {
                CustomDarknessHandler.shouldBeLightOverride.RemoveOverride("MagicMissile");
                isGivingDarknessImmunity = false;
            }
        }
        public override void DoEffect(PlayerController user)
        {
            SpawnMissile();
            if (user.PlayerHasActiveSynergy("Magic-er Missile"))
            {
                Invoke("SpawnMissile", 1f);
            }
        }
        private void SpawnMissile()
        {
            PlayerController user = LastOwner;
            Projectile projectile = ((Gun)ETGMod.Databases.Items[372]).DefaultModule.projectiles[0];
            GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, user.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (user.CurrentGun == null) ? 0f : user.CurrentGun.CurrentAngle), true);
            Projectile component = gameObject.GetComponent<Projectile>();
            if (component != null)
            {
                component.Owner = user;
                component.Shooter = user.specRigidbody;
                component.baseData.damage *= 2f;
                component.AdjustPlayerProjectileTint(ExtendedColours.lime, 1);
                user.DoPostProcessProjectile(component);
            }
        }
    }
}