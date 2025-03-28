using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.Misc;
using Alexandria.SoundAPI;
using SaveAPI;
using Dungeonator;

namespace NevernamedsItems
{
    public class Cornnon : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Cornnon", "cornnon");
            Game.Items.Rename("outdated_gun_mods:cornnon", "nn:cornnon");
            gun.gameObject.AddComponent<Cornnon>();
            gun.SetShortDescription("On the Cob");
            gun.SetLongDescription("Corn within the Gungeon possesses strange projectile properties, as visitors to the Gungeons Oubliette are no doubt aware.");

            gun.SetGunSprites("cornnon", 8, false, 2);

            gun.SetAnimationFPS(gun.shootAnimation, 15);
            gun.SetAnimationFPS(gun.reloadAnimation, 15);
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.reloadAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(56) as Gun, true, false);
            gun.AddCustomSwitchGroup("NN_Cornnon", "Play_ENM_pop_shot_01", "Play_ENM_Tarnisher_Bite_01");

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0.9f;
            gun.DefaultModule.cooldownTime = 0.02f;
            gun.DefaultModule.numberOfShotsInClip = 50;
            gun.DefaultModule.angleVariance = 360;
            gun.SetBarrel(17, 8);
            gun.SetBaseMaxAmmo(2000);
            gun.ammo = 2000;
            gun.gunClass = GunClass.FULLAUTO;

            //BULLET STATS
            Projectile projectile = ProjectileSetupUtility.MakeProjectile(56, 10f);
            projectile.gameObject.name = "Cornnon Projectile";
            projectile.SetProjectileSprite("cornnon_proj", 8, 6, false, tk2dBaseSprite.Anchor.MiddleCenter, 6, 4);
            projectile.baseData.UsesCustomAccelerationCurve = true;
            projectile.baseData.AccelerationCurve = AnimationCurve.Linear(0, 0.1f, 0.2f, 2f);
            projectile.baseData.speed = 10f;
            projectile.gameObject.AddComponent<CornTwister>();
            projectile.onDestroyEventName = "Play_ENM_pop_shot_01";
            projectile.gameObject.AddComponent<PierceDeadActors>();
            gun.DefaultModule.projectiles[0] = projectile;

            projectile.hitEffects.deathAny = VFXToolbox.CreateBlankVFXPool(Breakables.GenerateDebrisObject(Initialisation.VFXCollection, "cornnon_debris_002", AngularVelocityVariance: 200f, AngularVelocity: 10f).gameObject, true);
            projectile.hitEffects.HasProjectileDeathVFX = true;

            gun.gunHandedness = GunHandedness.AutoDetect;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(124) as Gun).muzzleFlashEffects;

            gun.AddClipSprites("cornnon");

            gun.quality = PickupObject.ItemQuality.C;
            ETGMod.Databases.Items.Add(gun, null, "ANY");

            ID = gun.PickupObjectId;
        }

        public override void PostProcessProjectile(Projectile projectile)
        {
            if (projectile && projectile.ProjectilePlayerOwner() && projectile.ProjectilePlayerOwner().PlayerHasActiveSynergy("Kernel Panic"))
            {
                projectile.OnHitEnemy += OnHit;
            }
            base.PostProcessProjectile(projectile);
        }
        private void OnHit(Projectile proj, SpeculativeRigidbody body, bool fatal)
        {
            if (fatal && proj)
            {
                for (int i = 0; i < 7; i++)
                {
                    Projectile spawned = gun.DefaultModule.projectiles[0].InstantiateAndFireInDirection(proj.SafeCenter, BraveUtility.RandomAngle()).GetComponent<Projectile>();
                    if (spawned)
                    {
                        spawned.Owner = proj.Owner;
                        spawned.Shooter = proj.Shooter;
                        BounceProjModifier bounce = spawned.gameObject.GetComponent<BounceProjModifier>();
                        if (bounce == null)
                        {
                            bounce = spawned.gameObject.AddComponent<BounceProjModifier>();
                            bounce.numberOfBounces = 1;
                        }
                        else
                        {
                            bounce.numberOfBounces++;
                        }
                        if (spawned.ProjectilePlayerOwner())
                        {
                            spawned.ScaleByPlayerStats(spawned.ProjectilePlayerOwner());
                        }
                    }
                }
            }
        }
        public static int ID;
        public class CornTwister : MonoBehaviour
        {
            private Projectile self;
            private void Start()
            {
                self = base.GetComponent<Projectile>();
            }
            float t = 0;
            private void Update()
            {
                t += BraveTime.DeltaTime;
                if (t > 0.1f)
                {
                    self.SendInDirection(self.Direction.Rotate(15f), false);
                    t = 0;
                }
            }
        }
    }
}

