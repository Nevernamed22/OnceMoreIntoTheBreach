using Alexandria.ItemAPI;
using Gungeon;
using System.Collections.Generic;
using UnityEngine;
using Alexandria.Misc;

namespace NevernamedsItems
{

    public class LtBluesPhaser : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Lt. Blues Phaser", "ltbluesphaser");
            Game.Items.Rename("outdated_gun_mods:lt_blues_phaser", "nn:lt_blues_phaser");
            gun.gameObject.AddComponent<LtBluesPhaser>();
            gun.SetShortDescription("<><>");
            gun.SetLongDescription("The plasma blaster of a Colorscant lieutenant whose platoon was stranded within the Gungeon in a bygone age.");

            gun.SetupSprite(null, "ltbluesphaser_idle_001", 8);

            gun.SetAnimationFPS(gun.shootAnimation, 15);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(56) as Gun, true, false);
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(59) as Gun).gunSwitchGroup;

            //GUN STATS
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(59) as Gun).muzzleFlashEffects;
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Burst;
            gun.DefaultModule.burstCooldownTime = 0.1f;
            gun.DefaultModule.burstShotCount = 4;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1f;
            gun.DefaultModule.cooldownTime = 0.25f;
            gun.DefaultModule.numberOfShotsInClip = 8;
            gun.DefaultModule.angleVariance = 10f;
            gun.barrelOffset.transform.localPosition = new Vector3(23f / 16f, 6f / 16f, 0f);
            gun.SetBaseMaxAmmo(400);
            gun.gunClass = GunClass.FULLAUTO;

            //BULLET STATS
            Projectile projectile = gun.DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.speed *= 0.5f;
            projectile.baseData.damage = 6.5f;
            projectile.sprite.renderer.enabled = false;
            SquareMotionHandler motion = projectile.gameObject.AddComponent<SquareMotionHandler>();
            motion.angleChange = 45;
            motion.randomiseStart = true;
            projectile.gameObject.AddComponent<PierceProjModifier>();

            projectile.hitEffects.overrideMidairDeathVFX = EasyVFXDatabase.BlueLaserCircleVFX;
            projectile.hitEffects.alwaysUseMidair = true;

            List<string> BeamAnimPaths = new List<string>()
            {
                "NevernamedsItems/Resources/TrailSprites/smallbluetrail_001",
                "NevernamedsItems/Resources/TrailSprites/smallbluetrail_002",
                "NevernamedsItems/Resources/TrailSprites/smallbluetrail_003",
            };

            projectile.AddTrailToProjectile(
                "NevernamedsItems/Resources/TrailSprites/smallbluetrail_001",
                new Vector2(3, 2),
                new Vector2(1, 1),
                BeamAnimPaths, 20,
                BeamAnimPaths, 20,
                -1,
                0.0001f,
                -1,
                true
                );

            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = "pulse blue";

            gun.quality = PickupObject.ItemQuality.C;
            ETGMod.Databases.Items.Add(gun, false, "ANY");


            ID = gun.PickupObjectId;
        }
        public static int ID;
    }
}