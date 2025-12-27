using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;
using System.Collections;
using SaveAPI;
using Alexandria.VisualAPI;

namespace NevernamedsItems
{
    public class SupersonicShots : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<SupersonicShots>(
            "Supersonic Shots",
            "Nyoom",
            "Makes your bullets travel at supersonic speeds." + "\n\nBrought to the Gungeon by the infamous speedster Tonic.",
            "supersonicshots_improved");           
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.ProjectileSpeed, 10, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Damage, 1.2f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            item.quality = PickupObject.ItemQuality.A;
            item.SetTag("bullet_modifier");
            item.AddToSubShop(ItemBuilder.ShopType.Cursula);
            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.BEATEN_HOLLOW_BOSS_TURBO_MODE, true);
            Doug.AddToLootPool(item.PickupObjectId);
        }
        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {
            sourceProjectile.AdjustPlayerProjectileTint(Color.blue, 1, 0f);
            ImprovedAfterImage afterImage = sourceProjectile.gameObject.AddComponent<ImprovedAfterImage>();
            afterImage.spawnShadows = true;
            afterImage.shadowLifetime = (UnityEngine.Random.Range(0.1f, 0.2f));
            afterImage.shadowTimeDelay = 0.0005f;
            afterImage.dashColor = Color.blue;
            afterImage.name = "Gun Trail";
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += this.PostProcessProjectile;
        }
        public override void DisableEffect(PlayerController player)
        {
            if (player)
            {
                player.PostProcessProjectile -= this.PostProcessProjectile;
            }
            base.DisableEffect(player);
        }
     
    }
}

