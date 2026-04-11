using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    public class CleansingRounds : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<CleansingRounds>(
             "Cleansing Rounds",
             "Undo What Is Done",
             "These holy shells are capable of saving the Gundead from eternal Jamnation.",
             "cleansingrounds_improved");

            item.quality = PickupObject.ItemQuality.B;
            item.SetTag("bullet_modifier");
            Doug.AddToLootPool(item.PickupObjectId);
        }

        public float ChanceToUnjam
        {
            get
            {
                return (Owner != null && Owner.PlayerHasActiveSynergy("Holy Smackerel")) ? 0.5f : 0.1f;
            }
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += this.PostProcessProjectile;
            player.PostProcessBeamTick += this.PostProcessBeam;
        }
        public override void DisableEffect(PlayerController player)
        {
            base.DisableEffect(player);
            if (player)
            {
                player.PostProcessProjectile -= this.PostProcessProjectile;
                player.PostProcessBeamTick -= this.PostProcessBeam;
            }
        }
        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {
            sourceProjectile.OnHitEnemy += OnHitEnemy;
        }
        private void PostProcessBeam(BeamController beam, SpeculativeRigidbody hitRigidBody, float tickrate)
        {
            float procChance = ChanceToUnjam;
            AIActor actor = hitRigidBody.aiActor;
            if (!actor) return;
            if (actor.IsBlackPhantom && UnityEngine.Random.value < BraveMathCollege.SliceProbability(procChance, tickrate))
            {
                actor.UnbecomeBlackPhantom();
            }
        }
        private void OnHitEnemy(Projectile projectile, SpeculativeRigidbody rigidbody, bool fatal)
        {
            if (projectile != null && rigidbody != null && rigidbody.aiActor != null && !fatal && rigidbody.healthHaver != null && !rigidbody.healthHaver.IsDead && rigidbody.aiActor.IsBlackPhantom)
            {
                if (UnityEngine.Random.value <= ChanceToUnjam)
                {
                    rigidbody.aiActor.UnbecomeBlackPhantom();
                }
            }
        }
    }
}
