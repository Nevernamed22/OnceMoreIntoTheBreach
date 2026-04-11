using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;
using SaveAPI;
using Alexandria.Misc;

namespace NevernamedsItems
{
    public class BloodthirstyBullets : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<BloodthirstyBullets>(
             "Bloodthirsty Bullets",
             "Born in the Fray",
             "Bullets either deal massive bonus damage to enemies, or enjam them.\n\nThese bullets were designed, cast, and shaped in the middle of combat to train them for the battlefield.",
             "bloodthirstybullets_improved");
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE);
            item.quality = PickupObject.ItemQuality.B;
            item.SetTag("bullet_modifier");
            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.ALLJAMMED_BEATEN_MINES, true);
            BloodthirstyBulletsID = item.PickupObjectId;
            Doug.AddToLootPool(item.PickupObjectId);
        }
        public static int BloodthirstyBulletsID;
        public static readonly float chanceToJam = 0.4f;
        public static readonly float synergy_chanceToJam = 0.2f;
        public static readonly float damageMultiplier = 6f;
        public static readonly float synergy_damageMultiplier = 12f;
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += this.PostProcessProjectile;
            player.PostProcessBeam += this.PostProcessBeam;
            player.PostProcessBeamTick += this.PostProcessBeamTick;
        }
        private void ModifyDealtDamage(Projectile p, HealthHaver h, HealthHaver.ModifyDamageEventArgs args)
        {
            if (p != null && h != null && args != null && h.aiActor && !h.aiActor.IsBlackPhantom)
            {
                args.ModifiedDamage *= (p.ProjectilePlayerOwner() && p.ProjectilePlayerOwner().PlayerHasActiveSynergy("[todo: Add funny synergy name]")) ? synergy_damageMultiplier : damageMultiplier;
            }
        }
        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {
            if (sourceProjectile == null) return;

            PlayerController player = sourceProjectile.ProjectilePlayerOwner();

            if (AllJammedState.AllJammedActive == true)
            {
                sourceProjectile.baseData.damage *= (player != null && player.PlayerHasActiveSynergy("[todo: Add funny synergy name]")) ? 1.9f : 1.45f;
            }
            else
            {
                if (UnityEngine.Random.value <= (player != null && player.PlayerHasActiveSynergy("[todo: Add funny synergy name]") ? synergy_chanceToJam : chanceToJam))
                {
                    sourceProjectile.OnHitEnemy += OnHitEnemy;
                }
                else
                {
                    sourceProjectile.Ext().ModifyDealtDamage += ModifyDealtDamage;
                }
            }
        }

        private void OnHitEnemy(Projectile p, SpeculativeRigidbody en, bool fatal)
        {
            if (en && en.aiActor && !en.aiActor.IsBlackPhantom && en.aiActor.healthHaver && !en.aiActor.healthHaver.IsDead)
            {
                en.aiActor.BecomeBlackPhantom();
            }
        }
        private void PostProcessBeam(BeamController sourceBeam)
        {
            if (sourceBeam == null || sourceBeam.projectile == null) { return; }

            Projectile sourceProjectile = sourceBeam.projectile;
            PlayerController player = sourceProjectile.ProjectilePlayerOwner();

            if (AllJammedState.AllJammedActive == true)
            {
                sourceProjectile.baseData.damage *= (player != null && player.PlayerHasActiveSynergy("[todo: Add funny synergy name]")) ? 1.9f : 1.45f;
            }
            else
            {
                sourceProjectile.Ext().ModifyDealtDamage += ModifyDealtDamage;
            }
        }
        private void PostProcessBeamTick(BeamController beam, SpeculativeRigidbody hitRigidBody, float tickrate)
        {
            if (AllJammedState.AllJammedActive == true || beam == null || beam.projectile == null || hitRigidBody == null) return;

            PlayerController player = beam.projectile.ProjectilePlayerOwner();

            float procChance = (player != null && player.PlayerHasActiveSynergy("[todo: Add funny synergy name]") ? synergy_chanceToJam : chanceToJam);

            GameActor gameActor = hitRigidBody.gameActor;
            
            if (!gameActor)
            {
                return;
            }
            if (UnityEngine.Random.value < BraveMathCollege.SliceProbability(procChance, tickrate))
            {
                if (gameActor is AIActor && !(gameActor as AIActor).IsBlackPhantom) { (gameActor as AIActor).BecomeBlackPhantom(); }   
            }
        }

        public override void DisableEffect(PlayerController player)
        {
            if (player)
            {
                player.PostProcessProjectile -= this.PostProcessProjectile;
                player.PostProcessBeam -= this.PostProcessBeam;
                player.PostProcessBeamTick -= this.PostProcessBeamTick;
            }
            base.DisableEffect(player);
        }
    }
}
