using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Alexandria.ItemAPI;
using Gungeon;
using UnityEngine;

namespace NevernamedsItems
{
    public class ExpandingBullets : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<ExpandingBullets>(
            "Expanding Bullets",
            "Dummy!",
            "Designed to expand upon impact with a target, these bullets are too dim to understand where they are- continually expanding and contracting in the air.",
            "expandingbullets_icon");
            item.quality = PickupObject.ItemQuality.C;
            item.SetTag("bullet_modifier");
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.RangeMultiplier, 1.3f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ID = item.PickupObjectId;
            Doug.AddToLootPool(item.PickupObjectId);
        }
        public static int ID;
        public override void Pickup(PlayerController player)
        {
            player.PostProcessProjectile += OnPost;
            base.Pickup(player);
        }
        public override void DisableEffect(PlayerController player)
        {        
            if (player) player.PostProcessProjectile -= OnPost;
            base.DisableEffect(player);
        }
        private void OnPost(Projectile bullet, float ch)
        {
            if (bullet.gameObject.GetComponent<OscillatingProjectileModifier>())
            {
                OscillatingProjectileModifier osc = bullet.gameObject.GetComponent<OscillatingProjectileModifier>();
                osc.multiplyDamage = true;
                osc.multiplyScale = true;
                osc.multiplySpeed = true;
                osc.maxDamageMult += 0.8f;
                osc.minDamageMult = Mathf.Max(0f, osc.minDamageMult - 0.2f);
            }
            else
            {
                OscillatingProjectileModifier osc = bullet.gameObject.AddComponent<OscillatingProjectileModifier>();
                osc.multiplyDamage = true;
                osc.multiplyScale = true;
                osc.multiplySpeed = true;
                osc.maxDamageMult = 1.8f;
                osc.minDamageMult = 0.8f;
            }        
        }
    }
}
