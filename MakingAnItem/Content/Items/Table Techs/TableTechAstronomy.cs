using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Reflection;
using MonoMod.RuntimeDetour;
using Alexandria.ItemAPI;
using Alexandria.Misc;

namespace NevernamedsItems
{
    public class TableTechAstronomy : TableFlipItem
    {
        public static void Init()
        {
            var item = ItemSetup.NewItem<TableTechAstronomy>(
                "Table Tech Astronomy",
               "The Flip Beyond",
               "Flipped tables conjure a simulacrum of the heavens" + "\n\n Among secretive flipping circles, there is a cryptic proverb; A table flipped properly will never come back down.",
               "tabletechastronomy_icon");
            item.quality = PickupObject.ItemQuality.B;
            ID = item.PickupObjectId;
            item.SetTag("table_tech");

            SpaceFog = PickupObjectDatabase.GetById(597).gameObject.GetComponent<GunParticleSystemController>().TargetSystem;

            Mercury = (PickupObjectDatabase.GetById(597) as Gun).DefaultModule.projectiles[0];
            Venus = (PickupObjectDatabase.GetById(597) as Gun).DefaultModule.projectiles[1];
            Earth = (PickupObjectDatabase.GetById(597) as Gun).DefaultModule.projectiles[2];
            Mars = (PickupObjectDatabase.GetById(597) as Gun).DefaultModule.projectiles[3];
            Jupiter = (PickupObjectDatabase.GetById(597) as Gun).DefaultModule.projectiles[4];
            Saturn = (PickupObjectDatabase.GetById(597) as Gun).DefaultModule.projectiles[5];
            Uranus = (PickupObjectDatabase.GetById(597) as Gun).DefaultModule.projectiles[6];
            Neptune = (PickupObjectDatabase.GetById(597) as Gun).DefaultModule.projectiles[7];
        }
        public static int ID;
        public override void Pickup(PlayerController player)
        {
            player.OnTableFlipped += SpawnPlanets;
            base.Pickup(player);
        }
        public override void DisableEffect(PlayerController player)
        {
            if (player)
            {
                player.OnTableFlipped -= SpawnPlanets;
            }
            base.DisableEffect(player);
        }
        public static ParticleSystem SpaceFog;

        public static Projectile Mercury;
        public static Projectile Venus;
        public static Projectile Earth;
        public static Projectile Mars;
        public static Projectile Jupiter;
        public static Projectile Saturn;
        public static Projectile Uranus;
        public static Projectile Neptune;
        private void SpawnPlanets(FlippableCover obj)
        {
            if (GameManager.Options.ShaderQuality == GameOptions.GenericHighMedLowOption.HIGH || GameManager.Options.ShaderQuality == GameOptions.GenericHighMedLowOption.MEDIUM)
            {
                ParticleSystem.EmitParams emitParams = default(ParticleSystem.EmitParams);
                emitParams.position = obj.transform.position;
                SpaceFog.Emit(emitParams, 5);
            };
            InitPlanet(Mercury, obj.transform.position, 2, obj.specRigidbody);
            InitPlanet(Venus, obj.transform.position, 3f, obj.specRigidbody);
            InitPlanet(Earth, obj.transform.position, 4, obj.specRigidbody);
            InitPlanet(Mars, obj.transform.position, 5f, obj.specRigidbody);
            InitPlanet(Jupiter, obj.transform.position, 7, obj.specRigidbody);
            InitPlanet(Saturn, obj.transform.position, 9, obj.specRigidbody);
            InitPlanet(Uranus, obj.transform.position, 11, obj.specRigidbody);
            InitPlanet(Neptune, obj.transform.position, 13, obj.specRigidbody);
        }
        public void InitPlanet(Projectile proj, Vector2 v, float radius, SpeculativeRigidbody cover)
        {
            GameObject orbital = proj.InstantiateAndFireInDirection(v, 0);
            Projectile component = orbital.GetComponent<Projectile>();
            if (component != null)
            {
                component.Owner = Owner;
                component.Shooter = Owner.specRigidbody;
                NoCollideBehaviour nocol = orbital.AddComponent<NoCollideBehaviour>();
                nocol.worksOnEnemies = false;
                component.specRigidbody.CollideWithTileMap = false;
                component.pierceMinorBreakables = true;

                

                component.ScaleByPlayerStats(Owner);
                Owner.DoPostProcessProjectile(component);

                component.baseData.speed *= UnityEngine.Random.Range(0.5f, 1.5f);
                component.UpdateSpeed();

                OrbitProjectileMotionModule orbitProjectileMotionModule = new OrbitProjectileMotionModule();

                orbitProjectileMotionModule.lifespan = 50;
                orbitProjectileMotionModule.MinRadius = 0.1f;
                orbitProjectileMotionModule.MaxRadius = 0.1f;
                orbitProjectileMotionModule.usesAlternateOrbitTarget = true;
                orbitProjectileMotionModule.OrbitGroup = -6;
                orbitProjectileMotionModule.alternateOrbitTarget = cover;
                component.OverrideMotionModule = orbitProjectileMotionModule;

                StartCoroutine(LerpToMaxRadius(component, radius));
            }
        }
        private IEnumerator LerpToMaxRadius(Projectile proj, float radius)
        {
            if (!proj || proj.OverrideMotionModule == null) yield break;
            if (proj.OverrideMotionModule is OrbitProjectileMotionModule)
            {
                OrbitProjectileMotionModule motionMod = proj.OverrideMotionModule as OrbitProjectileMotionModule;

                float elapsed = 0f;
                float duration = 1f;
                while (elapsed < duration)
                {
                    elapsed += proj.LocalDeltaTime;
                    float t = elapsed / duration;
                    float currentRadius = Mathf.Lerp(0.1f, radius, t);

                    motionMod.m_radius = currentRadius;
                    yield return null;
                }
            }
            yield break;
        }
    }
}