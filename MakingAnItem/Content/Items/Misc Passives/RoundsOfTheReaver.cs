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
using Alexandria.Misc;

namespace NevernamedsItems
{
    public class RoundsOfTheReaver : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<RoundsOfTheReaver>(
            "Rounds Of The Reaver",
            "Razed",
            "Separates enemies from their eternal soul." + "\n\nThese bullets were forged in the flesh of a great beast, trapped outside of time within an aimless void...",
            "roundsofthereaver_icon");           
            item.quality = PickupObject.ItemQuality.A;
            item.SetTag("bullet_modifier");
            Doug.AddToLootPool(item.PickupObjectId);
        }

        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {

            float procChance = 0.15f;
            procChance *= effectChanceScalar;
            bool DoFirstShotOverrideSynergy = (Owner.CurrentGun.LastShotIndex == 0) && Owner.PlayerHasActiveSynergy("Added Effect - Reave");
            try
            {
                if (UnityEngine.Random.value <= procChance || DoFirstShotOverrideSynergy)
                {
                    sourceProjectile.AdjustPlayerProjectileTint(ExtendedColours.reaverAqua, 2);
                    sourceProjectile.OnHitEnemy += OnHitEnemy;
                }
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
            }
        }
        private void OnHitEnemy(Projectile bullet, SpeculativeRigidbody body, bool fatal)
        {
            if (bullet && body && body.healthHaver && !fatal && body.sprite)
            {
                Reave(body, bullet.ProjectilePlayerOwner(), bullet.Direction.ToAngle());
            }
        }
        public static void Reave(SpeculativeRigidbody enemy, PlayerController owner, float angle)
        {
            GameObject prefab = (PickupObjectDatabase.GetById(761) as Gun).DefaultModule.projectiles[0].gameObject;
            GameObject spawned = SpawnManager.SpawnProjectile(prefab, enemy.sprite.WorldCenter, Quaternion.Euler(0f, 0f, angle), true);
            if (spawned.GetComponent<Projectile>())
            {
                Projectile spawnedProj = spawned.GetComponent<Projectile>();
                spawnedProj.Owner = owner;
                spawnedProj.Shooter = owner.specRigidbody;
                spawnedProj.baseData.damage = 0;
            }
        }
        private void PostProcessBeam(BeamController beam, SpeculativeRigidbody hitRigidBody, float tickrate)
        {
            float procChance = 0.15f;
            beam.AdjustPlayerBeamTint(ExtendedColours.reaverAqua, 1, 0f);
            GameActor gameActor = hitRigidBody.gameActor;
            if (!gameActor || !hitRigidBody.healthHaver || !hitRigidBody.sprite)
            {
                return;
            }
            if (UnityEngine.Random.value < BraveMathCollege.SliceProbability(procChance, tickrate))
            {
                Reave(hitRigidBody, beam.projectile.ProjectilePlayerOwner(), beam.Direction.ToAngle());
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
            if (player)
            {
                player.PostProcessProjectile -= this.PostProcessProjectile;
                player.PostProcessBeamTick -= this.PostProcessBeam;
            }
            base.DisableEffect(player);
        }       
    }
}
