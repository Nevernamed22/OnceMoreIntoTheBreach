using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.Misc;

namespace NevernamedsItems
{
    public class Betsy : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Betsy", "betsy");
            Game.Items.Rename("outdated_gun_mods:betsy", "nn:betsy");
            gun.gameObject.AddComponent<Betsy>();
            gun.SetShortDescription("Heavens");
            gun.SetLongDescription("A gorgeous exemplar of guncraft, this undeniable Edwin original is the only one of its kind in the whole galaxy.");

            gun.SetGunSprites("betsy", 8, false, 2);

            gun.SetAnimationFPS(gun.shootAnimation, 15);
            gun.SetAnimationFPS(gun.reloadAnimation, 15);
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.reloadAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(56) as Gun, true, false);
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(53) as Gun).gunSwitchGroup;

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1f;
            gun.DefaultModule.cooldownTime = 0.1f;
            gun.DefaultModule.numberOfShotsInClip = 15;
            gun.SetBarrel(30, 12);
            gun.SetBaseMaxAmmo(200);
            gun.gunClass = GunClass.EXPLOSIVE;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(477) as Gun).muzzleFlashEffects;

            gun.carryPixelOffset = new IntVector2(5, -5);
            gun.carryPixelDownOffset = new IntVector2(3, 2);
            gun.carryPixelUpOffset = new IntVector2(5, 4);

            //BULLET STATS
            Projectile projectile = ProjectileSetupUtility.MakeProjectile(56, 10f);
            projectile.baseData.speed *= 1.6f;
            projectile.baseData.UsesCustomAccelerationCurve = true;
            projectile.baseData.AccelerationCurve = AnimationCurve.Linear(0, 0f, 1f, 1f);

            ExplosiveModifier explode = projectile.gameObject.AddComponent<ExplosiveModifier>();
            explode.doExplosion = true;
            explode.explosionData = DataCloners.CopyExplosionData(StaticExplosionDatas.explosiveRoundsExplosion);
            explode.explosionData.damage = 10f;
            explode.explosionData.damageRadius = 1.5f;
            explode.explosionData.pushRadius = 1.5f;

            projectile.SetProjectileSprite("betsy_proj", 9, 5, false, tk2dBaseSprite.Anchor.MiddleLeft);

            CustomVFXTrail trail = projectile.gameObject.AddComponent<CustomVFXTrail>();
            trail.timeBetweenSpawns = 0.02f;
            trail.anchor = CustomVFXTrail.Anchor.ChildTransform;
            GameObject spawnPoint = new GameObject("CustomVFXSpawnpoint");
            spawnPoint.transform.SetParent(projectile.transform);
            //spawnPoint.transform.localPosition = new Vector3(2f / 16f, 0f);
            trail.VFX = new VFXPool
            {
                type = VFXPoolType.All,
                effects = new VFXComplex[] { new VFXComplex() {
                    effects = new VFXObject[] {
                        new VFXObject()
                        {
                        effect = SharedVFX.BulletSmokeTrail,
                        attached = false
                        },
                        new VFXObject()
                        {
                        effect = SharedVFX.BulletSparkTrail,
                        attached = true
                        }
                    } } }
            };

            gun.DefaultModule.projectiles[0] = projectile;

            gun.AddShellCasing(1, 0, 0, 0, "shell_betsy");
            gun.AddClipDebris(0, 1, "clipdebris_betsy");
            gun.AddClipSprites("betsy");

            gun.quality = PickupObject.ItemQuality.A;

            ETGMod.Databases.Items.Add(gun, null, "ANY");
        }
    }
}
