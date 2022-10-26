﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using ItemAPI;

namespace NevernamedsItems
{

    public class DARCShotgun : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("DARC Shotgun", "darcshotgun");
            Game.Items.Rename("outdated_gun_mods:darc_shotgun", "nn:arc_shotgun+darc_shotgun");
            var behav = gun.gameObject.AddComponent<DARCShotgun>();
            gun.SetShortDescription("Crack The Heavens");
            gun.SetLongDescription("Part of a short-lived campaign from the ARC Private Security Company to advertise their weaponry to the public as valuable home defence tools." + "\n\nAfter the fourth lawsuit from a disgruntled customer blasting their own legs off, ARC pulled the campaign.");

            gun.SetupSprite(null, "darcshotgun_idle_001", 8);

            gun.SetAnimationFPS(gun.shootAnimation, 13);
            gun.SetAnimationFPS(gun.idleAnimation, 5);
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(153) as Gun).gunSwitchGroup;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(41) as Gun).muzzleFlashEffects;

            for (int i = 0; i < 6; i++)
            {
                gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(DARCPistol.ID) as Gun, true, false);
            }

            //GUN STATS
            foreach (ProjectileModule mod in gun.Volley.projectiles)
            {
                mod.ammoCost = 1;
                mod.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
                mod.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
                mod.cooldownTime = 0.54f;
                mod.angleVariance = 15f;
                mod.numberOfShotsInClip = 10;

                Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(mod.projectiles[0]);
                mod.projectiles[0] = projectile;
                projectile.gameObject.SetActive(false);
                FakePrefab.MarkAsFakePrefab(projectile.gameObject);
                UnityEngine.Object.DontDestroyOnLoad(projectile);
                projectile.baseData.damage = 6f;

                if (mod != gun.DefaultModule) { mod.ammoCost = 0; }
            }

            gun.reloadTime = 1.8f;
            gun.barrelOffset.transform.localPosition = new Vector3(40f / 16f, 10f / 16f, 0f);
            gun.SetBaseMaxAmmo(150);
            gun.gunClass = GunClass.SHOTGUN;

            //BULLET STATS
            gun.quality = PickupObject.ItemQuality.EXCLUDED;
            ETGMod.Databases.Items.Add(gun, false, "ANY");
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = "DARC Bullets";
            gun.Volley.UsesShotgunStyleVelocityRandomizer = true;

            ID = gun.PickupObjectId;
        }
        public static int ID;
    }
}
