using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Alexandria.ItemAPI;
using UnityEngine;
using MonoMod.RuntimeDetour;
using System.Reflection;
using SaveAPI;

namespace NevernamedsItems
{
    public class JammedBullets : PassiveItem
    {
        public static void Init()
        {
            PassiveItem item = ItemSetup.NewItem<JammedBullets>(
              "Jammed Bullets",
              "Screaming",
              "Increases firepower. They don't stand a chance.\n\nThe ancient metal of these bullets carries a dull otherworldly hue. You never want to let them go.",
              "jammedbullets_icon") as PassiveItem;
            item.quality = PickupObject.ItemQuality.S;
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Damage, 1.75f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 2.5f, StatModifier.ModifyMethod.ADDITIVE);
            item.AddToSubShop(ItemBuilder.ShopType.Cursula);
            Doug.AddToLootPool(item.PickupObjectId);
            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.ALLJAMMED_BEATEN_ADVANCEDDRAGUN, true);
        }
        public override void Pickup(PlayerController player)
        {
            player.PostProcessProjectile += OnShoot;
            player.PostProcessBeam += OnBeam;
            base.Pickup(player);
        }
        public override void DisableEffect(PlayerController player)
        {
            if (player)
            {
                player.PostProcessProjectile -= OnShoot;
                player.PostProcessBeam -= OnBeam;
            }
            base.DisableEffect(player);
        }
        public void OnBeam(BeamController beam)
        {
            if (beam && beam.projectile)
            {
                OnShoot(beam.projectile, 1);
            }
        }
        public void OnShoot(Projectile bullet, float f)
        {
            bullet.ignoreDamageCaps = true;
            if (bullet.sprite != null)
            {
                bullet.sprite.usesOverrideMaterial = true;
                bullet.sprite.renderer.material.SetFloat("_BlackBullet", 1f);
                bullet.sprite.renderer.material.SetFloat("_EmissivePower", -40f);
            }
        }

    }
}


