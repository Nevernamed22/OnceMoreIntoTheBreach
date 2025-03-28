using Alexandria.ItemAPI;
using Gungeon;
using System.Collections.Generic;
using UnityEngine;

namespace NevernamedsItems
{

    public class ARCPistol : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("ARC Pistol", "arcpistol");
            Game.Items.Rename("outdated_gun_mods:arc_pistol", "nn:arc_pistol");
            gun.gameObject.AddComponent<ARCPistol>();
            gun.SetShortDescription("Shocked And Loaded");
            gun.SetLongDescription("Developed by the ARC Private Security company for easy manufacture and deployment, this electrotech blaster is the epittome of the ARC brand.");

            gun.SetGunSprites("arcpistol");

            gun.SetAnimationFPS(gun.shootAnimation, 15);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(56) as Gun, true, false);
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(153) as Gun).gunSwitchGroup;

            //GUN STATS
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(41) as Gun).muzzleFlashEffects;
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0.7f;
            gun.DefaultModule.cooldownTime = 0.15f;
            gun.DefaultModule.numberOfShotsInClip = 5;
            gun.barrelOffset.transform.localPosition = new Vector3(22f/16f, 11f / 16f, 0f);
            gun.SetBaseMaxAmmo(300);
            gun.gunClass = GunClass.PISTOL;

            //BULLET STATS
            Projectile projectile = ProjectileSetupUtility.MakeProjectile(56, 6);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.speed *= 5f;
            projectile.SetProjectileSprite("arc_proj", 8, 2, false, tk2dBaseSprite.Anchor.MiddleCenter, 8, 2);
            LightningProjectileComp lightning = projectile.gameObject.GetOrAddComponent<LightningProjectileComp>();
            lightning.targetEnemies = true;
            projectile.gameObject.AddComponent<PierceDeadActors>();

            projectile.hitEffects.overrideMidairDeathVFX = SharedVFX.ArcImpact;
            projectile.hitEffects.alwaysUseMidair = true;

            List<string> trailAnimpaths = new List<string>()
            {
                "NevernamedsItems/Resources/TrailSprites/arctrail_mid_001",
                "NevernamedsItems/Resources/TrailSprites/arctrail_mid_002",
                "NevernamedsItems/Resources/TrailSprites/arctrail_mid_003",
            };

            projectile.AddTrailToProjectile(
                "NevernamedsItems/Resources/TrailSprites/arctrail_mid_001",
                new Vector2(3, 2),
                new Vector2(1, 1),
                trailAnimpaths, 20,
                trailAnimpaths, 20,
                -1,
                0.0001f,
                -1,
                true
                );
            EmmisiveTrail emis = projectile.gameObject.GetOrAddComponent<EmmisiveTrail>();

            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = CustomClipAmmoTypeToolbox.AddCustomAmmoType("ARC Bullets", "NevernamedsItems/Resources/CustomGunAmmoTypes/arcweapon_clipfull", "NevernamedsItems/Resources/CustomGunAmmoTypes/arcweapon_clipempty");

            gun.quality = PickupObject.ItemQuality.C;
            ETGMod.Databases.Items.Add(gun, false, "ANY");


            ID = gun.PickupObjectId;
        }     
        public static int ID;
    }
}