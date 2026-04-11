using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Alexandria.ItemAPI;
using UnityEngine;
using MonoMod.RuntimeDetour;
using System.Reflection;
using Alexandria.Misc;

namespace NevernamedsItems
{
    public class EpimethianBullets : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<EpimethianBullets>(
             "Epimethian Bullets",
             "Afterthought",
             "Deals 50% more damage to enemies who have less than half their health." + "\n\nCreated by the mighty titan bullet Epimetheus.",
             "epimethianbullets_icon");
            item.quality = PickupObject.ItemQuality.B;
            item.SetTag("bullet_modifier");
            Doug.AddToLootPool(item.PickupObjectId);
        }

        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {
            sourceProjectile.Ext().ModifyDealtDamage += ModifyDealtDamage;
        }
        private void PostProcessBeam(BeamController beam)
        {
            if (beam && beam.projectile != null) { PostProcessProjectile(beam.projectile, 1f); }
        }
        private void ModifyDealtDamage(Projectile p, HealthHaver h, HealthHaver.ModifyDamageEventArgs args)
        {
            if (p != null && h != null && args != null)
            {
                if (h.currentHealth <= (h.GetMaxHealth() / 2f))
                {
                    args.ModifiedDamage *= 1.5f;
                }
            }
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += this.PostProcessProjectile;
            player.PostProcessBeam += this.PostProcessBeam;
        }
        public override void DisableEffect(PlayerController player)
        {
            base.DisableEffect(player);
            if (player)
            {
                player.PostProcessProjectile -= this.PostProcessProjectile;
                player.PostProcessBeam -= this.PostProcessBeam;
            }
        }
    }
}