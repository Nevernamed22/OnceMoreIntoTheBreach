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

namespace NevernamedsItems
{

    public class Arquebus : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Arquebus", "arquebus");
            Game.Items.Rename("outdated_gun_mods:arquebus", "nn:arquebus");
            gun.gameObject.AddComponent<Arquebus>();
            gun.SetShortDescription("Line and Sinker");
            gun.SetLongDescription("A classic muzzleloader, overstuffed with black powder from the depths of the Gungeon's labyrinthine mines."+"\n\nKicks up plenty of shrapnel on impact.");

            gun.SetGunSprites("arquebus");

            gun.SetAnimationFPS(gun.shootAnimation, 15);
            gun.SetAnimationFPS(gun.reloadAnimation, 6);

            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(9) as Gun).gunSwitchGroup;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(9) as Gun).muzzleFlashEffects;

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(9) as Gun, true, false);

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.32f;
            gun.DefaultModule.cooldownTime = 1f;
            gun.DefaultModule.numberOfShotsInClip = 1;
            gun.barrelOffset.transform.localPosition = new Vector3(65f / 16f, 31f / 16f, 0f);
            gun.SetBaseMaxAmmo(90);
            gun.gunClass = GunClass.RIFLE;

            //BULLET STATS
            Projectile projectile = gun.DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage = 40;
            BounceProjModifier bonce = projectile.GetComponent<BounceProjModifier>();
            bonce.numberOfBounces = 2;

            SpawnProjModifier spawnProjModifier = projectile.gameObject.AddComponent<SpawnProjModifier>();
            spawnProjModifier.SpawnedProjectilesInheritAppearance = false;
            spawnProjModifier.SpawnedProjectileScaleModifier = 0.5f;
            spawnProjModifier.SpawnedProjectilesInheritData = true;
            spawnProjModifier.spawnProjectilesOnCollision = true;
            spawnProjModifier.spawnProjecitlesOnDieInAir = false;
            spawnProjModifier.doOverrideObjectCollisionSpawnStyle = true;
            spawnProjModifier.randomRadialStartAngle = true;
            spawnProjModifier.startAngle = UnityEngine.Random.Range(0, 180);
            spawnProjModifier.numberToSpawnOnCollison = 5;

            Projectile tospn = (PickupObjectDatabase.GetById(531) as ComplexProjectileModifier).CollisionSpawnProjectile.InstantiateAndFakeprefab();
            tospn.baseData.damage = 10f;
            tospn.SetProjectileSprite("arquebusflak", 4, 4, false, tk2dBaseSprite.Anchor.MiddleCenter, 4, 4);

            spawnProjModifier.projectileToSpawnOnCollision = tospn;
            spawnProjModifier.collisionSpawnStyle = SpawnProjModifier.CollisionSpawnStyle.FLAK_BURST;

            spawnProjModifier.spawnCollisionProjectilesOnBounce = true;

            projectile.pierceMinorBreakables = true;

            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.MUSKETBALL;
            gun.TrimGunSprites();



            gun.quality = PickupObject.ItemQuality.B;
            ETGMod.Databases.Items.Add(gun, null, "ANY");

            ID = gun.PickupObjectId;
        }
        public static int ID;       
    }
}