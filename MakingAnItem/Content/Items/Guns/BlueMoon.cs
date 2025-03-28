using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.Misc;
using Alexandria.ItemAPI;
using SaveAPI;
using Alexandria.Assetbundle;

namespace NevernamedsItems
{

    public class BlueMoon : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Blue Moon", "bluemoon");
            Game.Items.Rename("outdated_gun_mods:blue_moon", "nn:blue_moon");
            gun.gameObject.AddComponent<BlueMoon>();
            gun.SetShortDescription("Strikes Once");
            gun.SetLongDescription("Created by the Queen of Sniperion, moon of Gunymede, for a mysterious planetside suitor. He could not have her heart, but he may have her gun.");

            gun.SetGunSprites("bluemoon", 8, false, 2);

            gun.SetAnimationFPS(gun.shootAnimation, 12);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(385) as Gun).gunSwitchGroup;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(45) as Gun).muzzleFlashEffects;

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 2f;
            gun.DefaultModule.cooldownTime = 0.7f;
            gun.DefaultModule.numberOfShotsInClip = 3;
            gun.SetBarrel(32, 7);
            gun.SetBaseMaxAmmo(50);
            gun.ammo = 50;
            gun.gunClass = GunClass.FULLAUTO;

            //BULLET STATS
            Projectile projectile = ProjectileSetupUtility.MakeProjectile(86, 20f);

            projectile.AnimateProjectileBundle("BlueMoonProjectile",
                   Initialisation.ProjectileCollection,
                   Initialisation.projectileAnimationCollection,
                   "BlueMoonProjectile",
                   MiscTools.DupeList(new IntVector2(32, 32), 4), //Pixel Sizes
                   MiscTools.DupeList(true, 4), //Lightened
                   MiscTools.DupeList(tk2dBaseSprite.Anchor.MiddleCenter, 4), //Anchors
                   MiscTools.DupeList(true, 4), //Anchors Change Colliders
                   MiscTools.DupeList(false, 4), //Fixes Scales
                   MiscTools.DupeList<Vector3?>(null, 4), //Manual Offsets
                   MiscTools.DupeList<IntVector2?>(new IntVector2(20, 20), 4), //Override colliders
                   MiscTools.DupeList<IntVector2?>(null, 4), //Override collider offsets
                   MiscTools.DupeList<Projectile>(null, 4)); // Override to copy from  
            projectile.hitEffects.deathAny = VFXToolbox.CreateBlankVFXPool(SharedVFX.BigWhitePoofVFX);
            projectile.hitEffects.HasProjectileDeathVFX = true;
            projectile.hitEffects.overrideMidairDeathVFX = SharedVFX.SmoothLightBlueLaserCircleVFX;
            projectile.hitEffects.alwaysUseMidair = true;
            projectile.pierceMinorBreakables = true;
            projectile.gameObject.AddComponent<BlueMoonBullet>();

            gun.DefaultModule.projectiles[0] = projectile;
            gun.gunHandedness = GunHandedness.HiddenOneHanded;
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.NAIL;

            gun.quality = PickupObject.ItemQuality.B;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            ID = gun.PickupObjectId;

            Wobbler = ProjectileSetupUtility.MakeProjectile(86, 5.5f, -1, 0.001f);
            OscillatingProjectileModifier osc = Wobbler.gameObject.AddComponent<OscillatingProjectileModifier>();
            osc.multiplySpeed = true;
            osc.multiplyScale = true;
            osc.multiplyDamage = true;
            Wobbler.SetProjectileSprite("blue_enemystyle_projectile", 10, 10, true, tk2dBaseSprite.Anchor.MiddleCenter, 8, 8);

            DriftModifier drift = Wobbler.gameObject.AddComponent<DriftModifier>();
            drift.diesAfterMaxDrifts = true;
            drift.maxDriftReaims = 10;
            drift.startInactive = true;
            SpriteSparkler particles = Wobbler.gameObject.GetOrAddComponent<SpriteSparkler>();
            particles.doVFX = true;
            particles.VFX = SharedVFX.BlueSparkle;
            particles.particlesPerSecond = 1f;


            Wobbler.hitEffects.overrideMidairDeathVFX = SharedVFX.SmoothLightBlueLaserCircleVFX;
            Wobbler.hitEffects.alwaysUseMidair = true;
        }
        public static int ID;
        public static Projectile Wobbler;
        public class BlueMoonBullet : BraveBehaviour
        {
            List<Projectile> spawned = new List<Projectile>();
            float time = 0;
            private void Update()
            {
                if (base.projectile)
                {
                    if (time > 0.02f)
                    {
                        Projectile sketch = Wobbler.InstantiateAndFireInDirection(base.projectile.SafeCenter, BraveUtility.RandomAngle()).GetComponent<Projectile>();
                        sketch.Shooter = base.projectile.Shooter;
                        sketch.Owner = base.projectile.Owner;
                        if (base.projectile.ProjectilePlayerOwner())
                        {
                            sketch.ScaleByPlayerStats(base.projectile.ProjectilePlayerOwner());
                        }
                        spawned.Add(sketch);
                        time = 0;
                    }
                    else
                    {
                        time += BraveTime.DeltaTime;
                    }
                }
            }
            private void Start()
            {
                base.projectile.OnDestruction += OnDest;
            }
            public void OnDest(Projectile pr)
            {
                for (int i = 0; i < 20; i++)
                {
                    GameObject sparkleinst = UnityEngine.Object.Instantiate(SharedVFX.BlueSparkle, base.projectile.SafeCenter, Quaternion.identity);
                    SimpleMover orAddComponent = sparkleinst.GetOrAddComponent<SimpleMover>();
                    orAddComponent.velocity = BraveUtility.RandomAngle().DegreeToVector2() * UnityEngine.Random.Range(5, 10) * 0.4f;
                    orAddComponent.acceleration = orAddComponent.velocity / 1.3f * -1f;
                }
                GameManager.Instance.StartCoroutine(HandleRelease(spawned));
            }
            public static IEnumerator HandleRelease(List<Projectile> spawned)
            {
                yield return new WaitForSeconds(4);
                for (int i = spawned.Count - 1; i >= 0; i--)
                {
                    if (spawned[i] != null)
                    {
                        spawned[i].baseData.speed = 10f;
                        SlowDownOverTimeModifier slow = spawned[i].gameObject.AddComponent<SlowDownOverTimeModifier>();
                        slow.targetSpeed = 2f;
                        slow.doRandomTimeMultiplier = true;
                        slow.activateDriftAfterstop = true;
                    }
                }
                yield break;
            }
        }
    }
}
