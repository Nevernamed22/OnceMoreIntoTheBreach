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
    public class Clamshell : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Clamshell", "clamshell");
            Game.Items.Rename("outdated_gun_mods:clamshell", "nn:clamshell");
            gun.gameObject.AddComponent<Clamshell>();
            gun.SetShortDescription("Greefer");
            gun.SetLongDescription("Fires highly volatile shells."+"While it may initially appear to be one large gun, this weapon is in fact a colony of many small guns working together."); 

            gun.SetGunSprites("clamshell", 8, false, 2);

            gun.SetAnimationFPS(gun.shootAnimation, 8);
            gun.SetAnimationFPS(gun.reloadAnimation, 8);

            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(39) as Gun).muzzleFlashEffects;
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(39) as Gun).gunSwitchGroup;

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.1f;

            gun.gunClass = GunClass.EXPLOSIVE;
            gun.DefaultModule.cooldownTime = 1f;
            gun.DefaultModule.numberOfShotsInClip = 2;

            gun.SetBaseMaxAmmo(50);
            gun.quality = PickupObject.ItemQuality.C;
            ETGMod.Databases.Items.Add(gun, null, "ANY");

            gun.SetBarrel(27, 12);

            Projectile projectile = ProjectileSetupUtility.MakeProjectile(56, 10f);
            projectile.baseData.UsesCustomAccelerationCurve = true;
            projectile.baseData.AccelerationCurve = (PickupObjectDatabase.GetById(39) as Gun).DefaultModule.projectiles[0].baseData.AccelerationCurve;
            projectile.SetProjectileSprite("clamshell_proj", 8, 8, false, tk2dBaseSprite.Anchor.MiddleCenter, 6, 6);

            ExplosiveModifier explode = projectile.gameObject.GetOrAddComponent<ExplosiveModifier>();
            explode.doExplosion = true;
            explode.explosionData = DataCloners.CopyExplosionData(StaticExplosionDatas.explosiveRoundsExplosion);
            explode.explosionData.damage = 20f;

            Projectile bubble = ProjectileSetupUtility.MakeProjectile(599, 7f);
            RandomProjectileStatsComponent rand = bubble.gameObject.AddComponent<RandomProjectileStatsComponent>();
            rand.randomScale = true;
            rand.highScalePercent = 100;
            rand.lowScalePercent = 40;

            SpawnProjModifier spawner = projectile.gameObject.GetOrAddComponent<SpawnProjModifier>();
            spawner.spawnProjectilesInFlight = true;
            spawner.numToSpawnInFlight = 1;
            spawner.fireRandomlyInAngle = true;
            spawner.inFlightSpawnAngle = 360f;
            spawner.inFlightSpawnCooldown = 0.1f;
            spawner.projectileToSpawnInFlight = bubble;
            spawner.usesComplexSpawnInFlight = true;

            spawner.spawnProjectilesOnCollision = true;
            spawner.projectileToSpawnOnCollision = bubble;
            spawner.numberToSpawnOnCollison = 8;
            spawner.spawnOnObjectCollisions = true;
            spawner.spawnCollisionProjectilesOnBounce = true;
            spawner.spawnProjecitlesOnDieInAir = true;
            spawner.collisionSpawnStyle = SpawnProjModifier.CollisionSpawnStyle.RADIAL;
            spawner.randomRadialStartAngle = true;

            gun.DefaultModule.projectiles[0] = projectile;

            gun.AddClipSprites("minigun");
            gun.carryPixelOffset = new IntVector2(2, 3);

            ID = gun.PickupObjectId;
        }
        public static int ID;
    }
}

