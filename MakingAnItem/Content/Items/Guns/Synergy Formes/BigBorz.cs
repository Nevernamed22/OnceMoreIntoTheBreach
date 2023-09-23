using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;
using SaveAPI;
using Alexandria.Misc;

namespace NevernamedsItems
{

    public class BigBorz : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Big Borz", "bigborz");
            Game.Items.Rename("outdated_gun_mods:big_borz", "nn:borz+big_borz");
            var behav = gun.gameObject.AddComponent<BigBorz>();
            gun.SetShortDescription("");
            gun.SetLongDescription("");

            gun.SetupSprite(null, "bigborz_idle_001", 8);

            gun.SetAnimationFPS(gun.shootAnimation, 10);
            gun.SetAnimationFPS(gun.reloadAnimation, 0);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(2) as Gun).muzzleFlashEffects;
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(2) as Gun).gunSwitchGroup;

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1f;
            gun.DefaultModule.cooldownTime = 0.2f;
            gun.DefaultModule.angleVariance = 5f;
            gun.DefaultModule.numberOfShotsInClip = 70;
            gun.barrelOffset.transform.localPosition = new Vector3(26f / 16f, 15f / 16f, 0f);
            gun.SetBaseMaxAmmo(700);
            gun.gunClass = GunClass.FULLAUTO;

            //BULLET STATS
            Projectile projectile = gun.DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            projectile.baseData.speed *= 1.5f;
            projectile.baseData.damage = 6f;
            projectile.SetProjectileSpriteRight("12x12_yellowenemy_projectile", 12, 12, true, tk2dBaseSprite.Anchor.MiddleCenter, 11, 11);
            projectile.hitEffects = (PickupObjectDatabase.GetById(15) as Gun).DefaultModule.projectiles[0].hitEffects;
            gun.DefaultModule.projectiles[0] = projectile;

            gun.quality = PickupObject.ItemQuality.EXCLUDED;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            ID = gun.PickupObjectId;
        }
        public static int ID;        
    }
}