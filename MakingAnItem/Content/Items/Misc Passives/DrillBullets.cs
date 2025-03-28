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
    public class DrillBullets : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<DrillBullets>(
            "Drill Bullets",
            "Drrrrrrrrr",
            "Bullets increase in damage the more they pierce!" + "\n\nKilling people is probably the most legal thing you can do with these.",
            "drillbullets_improved");
            item.quality = PickupObject.ItemQuality.A;
            item.SetTag("bullet_modifier");
            Doug.AddToLootPool(item.PickupObjectId);
        }

        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {
            PierceProjModifier piercing = sourceProjectile.gameObject.GetOrAddComponent<PierceProjModifier>();
            piercing.penetration += 5;
            piercing.penetratesBreakables = true;
            MaintainDamageOnPierce maintenance = sourceProjectile.gameObject.GetOrAddComponent<MaintainDamageOnPierce>();
            maintenance.damageMultOnPierce = 1.25f;
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