using Alexandria.ItemAPI;
using Gungeon;
using System.Collections.Generic;
using UnityEngine;
using Alexandria.Misc;

namespace NevernamedsItems
{

    public class SquareBracket : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Square Bracket", "squarebracket");
            Game.Items.Rename("outdated_gun_mods:square_bracket", "nn:square_bracket");
            gun.gameObject.AddComponent<SquareBracket>();
            gun.SetShortDescription("Brace For Impact");
            gun.SetLongDescription("Fires square wave energy beams."+"\n\nA remnant of an ancient cult of technomages, versed in the dark arts of vector rotation.");

            gun.SetupSprite(null, "squarebracket_idle_001", 8);

            gun.SetAnimationFPS(gun.shootAnimation, 15);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(56) as Gun, true, false);
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(89) as Gun).gunSwitchGroup;

            //GUN STATS
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(89) as Gun).muzzleFlashEffects;
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0.8f;
            gun.DefaultModule.cooldownTime = 0.1f;
            gun.DefaultModule.numberOfShotsInClip = 5;
            gun.barrelOffset.transform.localPosition = new Vector3(23f / 16f, 11f / 16f, 0f);
            gun.SetBaseMaxAmmo(400);
            gun.gunClass = GunClass.PISTOL;

            //BULLET STATS
            Projectile projectile = gun.DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.speed *= 2f;
            projectile.baseData.damage = 7f;
            projectile.sprite.renderer.enabled = false;
            projectile.gameObject.AddComponent<SquareMotionHandler>();

            projectile.hitEffects.overrideMidairDeathVFX = EasyVFXDatabase.GreenLaserCircleVFX;
            projectile.hitEffects.alwaysUseMidair = true;

            List<string> BeamAnimPaths = new List<string>()
            {
                "NevernamedsItems/Resources/TrailSprites/smallgreentrail_001",
                "NevernamedsItems/Resources/TrailSprites/smallgreentrail_002",
                "NevernamedsItems/Resources/TrailSprites/smallgreentrail_003",
            };

            projectile.AddTrailToProjectile(
                "NevernamedsItems/Resources/TrailSprites/smallgreentrail_001",
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
            gun.DefaultModule.customAmmoType = "green blaster";

            gun.quality = PickupObject.ItemQuality.C;
            ETGMod.Databases.Items.Add(gun, false, "ANY");


            ID = gun.PickupObjectId;
        }
        public static int ID;
    }
}