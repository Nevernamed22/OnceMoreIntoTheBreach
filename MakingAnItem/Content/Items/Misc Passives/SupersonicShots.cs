using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;
using System.Collections;
using SaveAPI;

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
            "supersonicshot_icon");           
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.ProjectileSpeed, 10, StatModifier.ModifyMethod.MULTIPLICATIVE);
            item.quality = PickupObject.ItemQuality.A;
            item.SetTag("bullet_modifier");
            item.AddToSubShop(ItemBuilder.ShopType.Cursula);
            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.BEATEN_HOLLOW_BOSS_TURBO_MODE, true);
        }
        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {
            sourceProjectile.AdjustPlayerProjectileTint(Color.blue, 1, 0f);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.PostProcessProjectile -= this.PostProcessProjectile;
            return debrisObject;
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += this.PostProcessProjectile;
        }
        public override void OnDestroy()
        {
            if (Owner)
            {
                Owner.PostProcessProjectile -= this.PostProcessProjectile;
            }
            base.OnDestroy();
        }       
    }
}

