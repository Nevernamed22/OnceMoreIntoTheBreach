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

namespace NevernamedsItems
{

    public class DARCCannon : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("DARC Cannon", "darccannon");
            Game.Items.Rename("outdated_gun_mods:darc_cannon", "nn:arc_cannon+darc_cannon");
            gun.gameObject.AddComponent<DARCCannon>();
            gun.SetShortDescription("All Lightning");
            gun.SetLongDescription("The ARC Cannon was commissioned from the ARC Private Security Company by the Sultan of a distant planet, who now spends his free time striking ships from the sky and declaring himself a god.");

            gun.SetGunSprites("darccannon", 8, true);

            gun.SetAnimationFPS(gun.shootAnimation, 15);
            gun.SetAnimationFPS(gun.reloadAnimation, 0);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(DARCPistol.ID) as Gun, true, false);
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(153) as Gun).gunSwitchGroup;

            //GUN STATS
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(228) as Gun).muzzleFlashEffects;
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 3f;
            gun.DefaultModule.cooldownTime = 0.9f;
            gun.DefaultModule.numberOfShotsInClip = 2;
            gun.barrelOffset.transform.localPosition = new Vector3(45f / 16f, 16f / 16f, 0f);
            gun.SetBaseMaxAmmo(40);
            gun.gunClass = GunClass.RIFLE;
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = "DARC Bullets";

            //BULLET STATS
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage = 24f;
            LightningProjectileComp lightning = projectile.gameObject.GetComponent<LightningProjectileComp>();

            UnityEngine.Object.Destroy(projectile.GetComponent<TrailController>());
            List<string> BeamAnimPaths = new List<string>()
            {
                "NevernamedsItems/Resources/TrailSprites/bigdarctrail_mid_001",
                "NevernamedsItems/Resources/TrailSprites/bigdarctrail_mid_002",
                "NevernamedsItems/Resources/TrailSprites/bigdarctrail_mid_003",
            };

            projectile.AddTrailToProjectile(
                "NevernamedsItems/Resources/TrailSprites/bigdarctrail_mid_001",
                new Vector2(8, 7),
                new Vector2(1, 1),
                BeamAnimPaths, 20,
                BeamAnimPaths, 20,
                -1,
                0.0001f,
                -1,
                true
                );

            ExplosiveModifier explode = projectile.gameObject.AddComponent<ExplosiveModifier>();
            explode.explosionData = new ExplosionData()
            {
                breakSecretWalls = false,
                //effect = (PickupObjectDatabase.GetById(36) as Gun).DefaultModule.chargeProjectiles[1].Projectile.projectile.hitEffects.overrideEarlyDeathVfx,
                effect = SharedVFX.ArcExplosionRed,
                doDamage = true,
                damageRadius = 3,
                damageToPlayer = 0,
                damage = 48,
                debrisForce = 20,
                doExplosionRing = true,
                doDestroyProjectiles = true,
                doForce = true,
                doScreenShake = true,
                playDefaultSFX = true,
                force = 20,
            };

            Projectile subLightning = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(DARCPistol.ID) as Gun).DefaultModule.projectiles[0]);
            subLightning.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(subLightning.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(subLightning);
            subLightning.baseData.damage = 6f;


            LightningProjectileComp lightning2 = subLightning.gameObject.GetComponent<LightningProjectileComp>();
            subLightning.projectile.gameObject.AddComponent<PierceProjModifier>();
            lightning2.targetEnemies = false;

            SpawnProjModifier flakLightning = projectile.gameObject.AddComponent<SpawnProjModifier>();
            flakLightning.numberToSpawnOnCollison = 7;
            flakLightning.numToSpawnInFlight = 0;
            flakLightning.PostprocessSpawnedProjectiles = true;
            flakLightning.projectileToSpawnOnCollision = subLightning;
            flakLightning.randomRadialStartAngle = true;
            flakLightning.spawnCollisionProjectilesOnBounce = true;
            flakLightning.spawnOnObjectCollisions = true;
            flakLightning.spawnProjecitlesOnDieInAir = true;
            flakLightning.spawnProjectilesOnCollision = true;
            flakLightning.spawnProjectilesInFlight = false;
            flakLightning.alignToSurfaceNormal = true;

            gun.quality = PickupObject.ItemQuality.EXCLUDED;
            ETGMod.Databases.Items.Add(gun, false, "ANY");


            ID = gun.PickupObjectId;
        }

        public static int ID;
    }
}