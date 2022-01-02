﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;
using Gungeon;
using MonoMod;
using UnityEngine;
using ItemAPI;

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

            gun.SetupSprite(null, "arcpistol_idle_001", 8);

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
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.speed *= 5f;
            projectile.baseData.damage = 6f;
            projectile.SetProjectileSpriteRight("arc_proj", 8, 2, false, tk2dBaseSprite.Anchor.MiddleCenter, 8, 2);
            LightningProjectileComp lightning = projectile.gameObject.GetOrAddComponent<LightningProjectileComp>();
            projectile.gameObject.AddComponent<PierceDeadActors>();

            projectile.hitEffects.overrideMidairDeathVFX = VFXToolbox.CreateVFX("ARC Impact",
                 new List<string>()
                 {
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/electricImpact1",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/electricImpact2",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/electricImpact3",
                 },
                10, //FPS
                 new IntVector2(44, 43), //Dimensions
                 tk2dBaseSprite.Anchor.MiddleCenter, //Anchor
                 false, //Uses a Z height off the ground
                 0 //The Z height, if used
                   );
            projectile.hitEffects.alwaysUseMidair = true;

            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = CustomClipAmmoTypeToolbox.AddCustomAmmoType("ARC Bullets", "NevernamedsItems/Resources/CustomGunAmmoTypes/arcweapon_clipfull", "NevernamedsItems/Resources/CustomGunAmmoTypes/arcweapon_clipempty");

            gun.quality = PickupObject.ItemQuality.C;
            ETGMod.Databases.Items.Add(gun, null, "ANY");


            ID = gun.PickupObjectId;
        }     
        public static int ID;
    }
}