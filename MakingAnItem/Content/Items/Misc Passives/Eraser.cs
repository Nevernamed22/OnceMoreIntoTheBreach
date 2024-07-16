using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Alexandria.ItemAPI;
using UnityEngine;
using MonoMod.RuntimeDetour;
using System.Reflection;
using Alexandria.EnemyAPI;

namespace NevernamedsItems
{
    public class Eraser : PassiveItem
    {
        public static void Init()
        {
            PassiveItem item = ItemSetup.NewItem<Eraser>(
              "Eraser",
              "Now I Only Want You Gone",
              "Chance to erase enemy bullets." + "\n\nDesigned to remove mistakes, including that mistake of walking into that bullet that you were just about to make!",
              "eraser_icon") as PassiveItem;          
            item.quality = PickupObject.ItemQuality.B;
        }

        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {
            float chance = 0.25f * effectChanceScalar;
           if (UnityEngine.Random.value <= chance)
            {
                sourceProjectile.AdjustPlayerProjectileTint(ExtendedColours.pink, 2);
                sourceProjectile.OnHitEnemy += this.OnHit;
            }
        }
        private void OnHit(Projectile proj, SpeculativeRigidbody enemy, bool fatal)
        {
            if (enemy && enemy.gameActor && enemy.healthHaver)
            {
                float chance = 1;
                if (enemy.healthHaver.IsBoss) chance = 0.33f;
                enemy.gameActor.DeleteOwnedBullets(chance);
            }
        }
        private void PostProcessBeam(BeamController beam, SpeculativeRigidbody enemy, float tickrate)
        {
            float procChance = 0.25f;
            GameActor gameActor = enemy.gameActor;
            if (!gameActor)
            {
                return;
            }
            if (UnityEngine.Random.value < BraveMathCollege.SliceProbability(procChance, tickrate))
            {
                if (enemy && enemy.gameActor && enemy.healthHaver)
                {
                    float chance = 1;
                    if (enemy.healthHaver.IsBoss) chance = 0.33f;
                    enemy.gameActor.DeleteOwnedBullets(chance);
                }
            }
        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.PostProcessProjectile -= this.PostProcessProjectile;
            player.PostProcessBeamTick -= this.PostProcessBeam;


            return debrisObject;
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += this.PostProcessProjectile;
            player.PostProcessBeamTick += this.PostProcessBeam;
        }
        public override void OnDestroy()
        {
            if (Owner)
            {
                Owner.PostProcessBeamTick -= this.PostProcessBeam;
                Owner.PostProcessProjectile -= this.PostProcessProjectile;
            }
            base.OnDestroy();
        }
    }
}
