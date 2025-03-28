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
using Alexandria.Assetbundle;
using Alexandria.VisualAPI;

namespace NevernamedsItems
{

    public class Pallbearer : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Pallbearer", "pallbearer");
            Game.Items.Rename("outdated_gun_mods:pallbearer", "nn:pallbearer");
            var behav = gun.gameObject.AddComponent<Pallbearer>();
            gun.SetShortDescription("One In The Chamber");
            gun.SetLongDescription("An ornate coffin pulled from the deepest crypts of the Gungeon's Hollow catacombs.\n\nThe spirits inside are broiling with rage.");

            gun.SetGunSprites("pallbearer", 8, false, 2);

            gun.SetAnimationFPS(gun.shootAnimation, 13);
            gun.SetAnimationFPS(gun.idleAnimation, 5);


            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.reloadAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;

            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(45) as Gun).gunSwitchGroup;
            gun.muzzleFlashEffects = SharedVFX.DoomBoomMuzzle;

            for (int i = 0; i < 3; i++)
            {
                gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
            }

            //GUN STATS
            int it = 1;
            foreach (ProjectileModule mod in gun.Volley.projectiles)
            {
                mod.ammoCost = 1;
                mod.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
                mod.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
                mod.cooldownTime = 1f;
                mod.angleVariance = 17f;
                mod.numberOfShotsInClip = 2;

                Projectile projectile = ProjectileUtility.SetupProjectile(86);
                mod.projectiles[0] = projectile;

                SlowDownOverTimeModifier Slow = projectile.gameObject.AddComponent<SlowDownOverTimeModifier>();
                Slow.timeToSlowOver = 0.5f;
                Slow.doRandomTimeMultiplier = true;
                Slow.extendTimeByRangeStat = true;
                Slow.killAfterCompleteStop = true;
                Slow.targetSpeed = 0.1f;
                Slow.timeTillKillAfterCompleteStop = 0.25f * it;

                projectile.AnimateProjectileBundle("PallbearerProj",
                    Initialisation.ProjectileCollection,
                    Initialisation.projectileAnimationCollection,
                    "PallbearerProj",
                    MiscTools.DupeList(new IntVector2(13, 13), 3), //Pixel Sizes
                    MiscTools.DupeList(true, 3), //Lightened
                    MiscTools.DupeList(tk2dBaseSprite.Anchor.MiddleCenter, 3), //Anchors
                    MiscTools.DupeList(true, 3), //Anchors Change Colliders
                    MiscTools.DupeList(false, 3), //Fixes Scales
                    MiscTools.DupeList<Vector3?>(null, 3), //Manual Offsets
                    MiscTools.DupeList<IntVector2?>(null, 3), //Override colliders
                    MiscTools.DupeList<IntVector2?>(null, 3), //Override collider offsets
                    MiscTools.DupeList<Projectile>(null, 3));

                SpawnProjModifier spawner = projectile.gameObject.AddComponent<SpawnProjModifier>();
                spawner.spawnProjecitlesOnDieInAir = true;
                spawner.spawnProjectilesInFlight = false;
                spawner.spawnProjectilesOnCollision = true;
                spawner.spawnOnObjectCollisions = true;
                spawner.spawnCollisionProjectilesOnBounce = true;
                //spawner.randomRadialStartAngle = true;

                Projectile Ghost = ProjectileUtility.SetupProjectile(56);
                Ghost.baseData.damage = 10;
                Ghost.SetProjectileSprite("pallbearer_ghost", 17, 11, true, tk2dBaseSprite.Anchor.MiddleCenter, 15, 9);
                HomingModifier homing = Ghost.gameObject.AddComponent<HomingModifier>();
                homing.HomingRadius = 100;
                homing.HomingRadius = 600f;

                ImprovedAfterImage afterImage = Ghost.gameObject.AddComponent<ImprovedAfterImage>();
                afterImage.spawnShadows = true;
                afterImage.shadowLifetime = 0.1f;
                afterImage.shadowTimeDelay = 0.01f;
                afterImage.dashColor = Color.white;
                afterImage.name = "Gun Trail";
                afterImage.maxEmission = 10f;

                Ghost.pierceMinorBreakables = true;

                ExplosiveModifier expl = Ghost.gameObject.GetOrAddComponent<ExplosiveModifier>();
                expl.doExplosion = true;
                expl.explosionData = DataCloners.CopyExplosionData(StaticExplosionDatas.explosiveRoundsExplosion);
                expl.explosionData.damageRadius = 2;
                expl.explosionData.damage = 15;
                expl.explosionData.effect = SharedVFX.KillDevilExplosion;
                expl.explosionData.pushRadius = 0.2f;

                spawner.projectileToSpawnOnCollision = Ghost;
                spawner.collisionSpawnStyle = SpawnProjModifier.CollisionSpawnStyle.FLAK_BURST;
                spawner.PostprocessSpawnedProjectiles = true;
                spawner.numberToSpawnOnCollison = 1;

                if (mod != gun.DefaultModule) { mod.ammoCost = 0; }
                it++;
            }

            gun.reloadTime = 1.8f;
            gun.SetBarrel(32, 15);
            gun.SetBaseMaxAmmo(30);
            gun.gunClass = GunClass.EXPLOSIVE;

            //BULLET STATS
            gun.quality = PickupObject.ItemQuality.B;
            ETGMod.Databases.Items.Add(gun, false, "ANY");
            gun.Volley.UsesShotgunStyleVelocityRandomizer = true;

            gun.AddClipSprites("smallghost");


            ID = gun.PickupObjectId;
        }
        public static int ID;
    }
}
