using System;
using System.Collections.Generic;
using System.Linq;
using Gungeon;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;
using MonoMod.RuntimeDetour;
using System.Reflection;
using SaveAPI;
using Alexandria.Misc;

namespace NevernamedsItems
{
    public class NeutroniumAmmolet : BlankModificationItem
    {
        public static void Init()
        {
            BlankModificationItem item = ItemSetup.NewItem<NeutroniumAmmolet>(
            "Neutronium Ammolet",
            "Blanks Unravel",
            "An impossible element with no protons, created in the mantle of a neutron star." + "\n\nCrushes nearby spacetime when agitated by a blank.",
            "neutroniumammolet_icon") as BlankModificationItem;
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.AdditionalBlanksPerFloor, 1f, StatModifier.ModifyMethod.ADDITIVE);
            item.quality = PickupObject.ItemQuality.A;
            ID = item.PickupObjectId;
            item.AddToSubShop(ItemBuilder.ShopType.OldRed);

            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.PURCHASED_NEUTRONIUMAMMOLET, true);
            item.AddItemToDougMetaShop(60);
            item.SetTag("ammolet");
        }
        private static int ID;
        public override void Pickup(PlayerController player)
        {
            player.GetExtComp().OnBlankModificationItemProcessed += OnBlankModTriggered;
            base.Pickup(player);
        }
        public override void DisableEffect(PlayerController player)
        {
            player.GetExtComp().OnBlankModificationItemProcessed -= OnBlankModTriggered;
            base.DisableEffect(player);
        }
        private void OnBlankModTriggered(PlayerController user, SilencerInstance blank, Vector2 pos, BlankModificationItem item)
        {
            if (item is NeutroniumAmmolet)
            {
                Projectile projectile2 = ((Gun)ETGMod.Databases.Items["black_hole_gun"]).DefaultModule.projectiles[0];
                GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, pos, Quaternion.identity, true);
                Projectile component = gameObject.GetComponent<Projectile>();
                if (component != null)
                {
                    component.Owner = user;
                    component.Shooter = user.specRigidbody;
                    component.baseData.speed = 0f;
                    component.baseData.range *= 100;
                    BulletLifeTimer timer = component.gameObject.AddComponent<BulletLifeTimer>();
                    timer.secondsTillDeath = 7f;
                    user.DoPostProcessProjectile(component);
                }
            }
        }
    }

}