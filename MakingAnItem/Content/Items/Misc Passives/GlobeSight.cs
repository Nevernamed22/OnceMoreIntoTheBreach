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
using Alexandria.Assetbundle;

namespace NevernamedsItems
{
    public class GlobeSight : TableFlipItem
    {
        public static void Init()
        {
            var item = ItemSetup.NewItem<GlobeSight>(
               "Globe Sight",
               "The World Goes Round",
               "Releases orbital momentum from inanimate objects.\n\nPeering through this sight makes your head spin.",
               "globesight_icon");
            item.quality = PickupObject.ItemQuality.C;
            ID = item.PickupObjectId;

            dummy = new GameObject("Orbit Center Dummy");
            SpeculativeRigidbody body = dummy.GetOrAddComponent<SpeculativeRigidbody>();
            body.PixelColliders = new List<PixelCollider>();
            body.PixelColliders.Add(new PixelCollider()
            {
                ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
                CollisionLayer = CollisionLayer.Pickup,
                ManualWidth = 1,
                ManualHeight = 1,
                ManualOffsetX = 0,
                ManualOffsetY = 0,
                Enabled = true,
                IsTrigger = false,
            });
            body.CollideWithOthers = false;
            dummy.MakeFakePrefab();

            Orbital = ProjectileUtility.SetupProjectile(86);
            Orbital.gameObject.name = "Globesight Orbital Projectile";
            Orbital.baseData.damage = 8f;
            Orbital.baseData.force = 0.1f;
            NoCollideBehaviour nocol = Orbital.gameObject.AddComponent<NoCollideBehaviour>();
            nocol.worksOnEnemies = false;
            Orbital.baseData.range = 100000f;
            Orbital.specRigidbody.CollideWithTileMap = false;
            Orbital.pierceMinorBreakables = true;
            Orbital.hitEffects.overrideMidairDeathVFX = SharedVFX.SmoothLightBlueLaserCircleVFX;
            Orbital.hitEffects.alwaysUseMidair = true;
            BulletLifeTimer lifespan = Orbital.gameObject.AddComponent<BulletLifeTimer>();
            lifespan.secondsTillDeath = 15f;
            Orbital.AnimateProjectileBundle("GlobesightProjectile",
                    Initialisation.ProjectileCollection,
                    Initialisation.projectileAnimationCollection,
                    "GlobesightProjectile",
                    MiscTools.DupeList(new IntVector2(7, 7), 5), //Pixel Sizes
                    MiscTools.DupeList(false, 5), //Lightened
                    MiscTools.DupeList(tk2dBaseSprite.Anchor.MiddleCenter, 5), //Anchors
                    MiscTools.DupeList(true,5), //Anchors Change Colliders
                    MiscTools.DupeList(false, 5), //Fixes Scales
                    MiscTools.DupeList<Vector3?>(null, 5), //Manual Offsets
                    MiscTools.DupeList<IntVector2?>(new IntVector2(7, 7), 5), //Override colliders
                    MiscTools.DupeList<IntVector2?>(null, 5), //Override collider offsets
                    MiscTools.DupeList<Projectile>(null, 5)); // Override to copy from
        }
        public static GameObject dummy;
        public static Projectile Orbital;
        public static int ID;
        public override void Pickup(PlayerController player)
        {
            OMITBActions.MinorBreakableBroken += SpawnOrbital;
            base.Pickup(player);
        }
        public override void DisableEffect(PlayerController player)
        {
            if (player)
            {
                OMITBActions.MinorBreakableBroken -= SpawnOrbital;
            }
            base.DisableEffect(player);
        }

        private void SpawnOrbital(MinorBreakable obj)
        {
            Vector2 pos = obj.transform.position;
            if (obj.specRigidbody) { pos = obj.specRigidbody.UnitCenter; }

            GameObject dummyCenter = UnityEngine.Object.Instantiate(dummy, pos, Quaternion.identity);

            GameObject orbital = Orbital.InstantiateAndFireInDirection(pos, 0);
            Projectile component = orbital.GetComponent<Projectile>();
            if (component != null)
            {
                component.Owner = Owner;
                component.Shooter = Owner.specRigidbody;

                component.ScaleByPlayerStats(Owner);
                Owner.DoPostProcessProjectile(component);

                component.baseData.speed *= UnityEngine.Random.Range(0.8f, 1.2f);
                component.UpdateSpeed();

                OrbitProjectileMotionModule orbitProjectileMotionModule = new OrbitProjectileMotionModule();

                orbitProjectileMotionModule.lifespan = 15;
                orbitProjectileMotionModule.MinRadius = 0.1f;
                orbitProjectileMotionModule.MaxRadius = 0.1f;
                orbitProjectileMotionModule.usesAlternateOrbitTarget = true;
                orbitProjectileMotionModule.OrbitGroup = -7;
                orbitProjectileMotionModule.alternateOrbitTarget = dummyCenter.GetComponent<SpeculativeRigidbody>();
                component.OverrideMotionModule = orbitProjectileMotionModule;

                StartCoroutine(LerpToMaxRadius(component, 3.5f));
            }
            UnityEngine.Object.Destroy(dummyCenter, 20);
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