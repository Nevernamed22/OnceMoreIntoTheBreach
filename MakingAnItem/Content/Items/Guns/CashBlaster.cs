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
using BreakAbleAPI;

namespace NevernamedsItems
{
    public class CashBlaster : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Cash Blaster", "cashblaster");
            Game.Items.Rename("outdated_gun_mods:cash_blaster", "nn:cash_blaster");
            var behav = gun.gameObject.AddComponent<CashBlaster>();
            behav.preventNormalFireAudio = true;
            behav.preventNormalReloadAudio = true;
            gun.SetShortDescription("Money Guney");
            gun.SetLongDescription("A tacky device designed to fire paper currency."+"\n\nUnfortunately, paper currency is not legal tender in the Gungeon.");

            gun.SetupSprite(null, "cashblaster_idle_001", 8);

            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.SetAnimationFPS(gun.reloadAnimation, 0);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
            gun.doesScreenShake = false;

            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(12) as Gun).muzzleFlashEffects;

            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).frames[0].eventAudio = "Play_OBJ_book_drop_01";
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).frames[0].triggerEvent = true;

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0.9f;
            gun.DefaultModule.angleVariance = 12;
            gun.DefaultModule.cooldownTime = 0.1f;
            gun.DefaultModule.numberOfShotsInClip = 20;
            gun.barrelOffset.transform.localPosition = new Vector3(14f / 16f, 9f / 16f, 0f);
            gun.SetBaseMaxAmmo(300);
            gun.gunClass = GunClass.FULLAUTO;

            //BULLET STATS
            Projectile projectile = gun.DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            gun.DefaultModule.projectiles[0] = projectile;

            projectile.baseData.damage = 4f;

            projectile.EasyAnimate(new List<string>() { 
                "cashproj_001",
                "cashproj_002",
                "cashproj_003",
                "cashproj_004",
                "cashproj_005",
                "cashproj_006",
                "cashproj_007",
                "cashproj_008",
                "cashproj_009",
                "cashproj_010",
            }, 10, new IntVector2(12, 8), 7, false, true);

            SlowDownOverTimeModifier slowDown = projectile.gameObject.AddComponent<SlowDownOverTimeModifier>();
            slowDown.extendTimeByRangeStat = true;
            slowDown.activateDriftAfterstop = true;
            slowDown.doRandomTimeMultiplier = true;
            slowDown.killAfterCompleteStop = true;
            slowDown.timeTillKillAfterCompleteStop = 1f;
            slowDown.timeToSlowOver = 0.5f;
            slowDown.targetSpeed = projectile.baseData.speed * 0.1f;

            DriftModifier drift = projectile.gameObject.AddComponent<DriftModifier>();

            GameObject cashDebris = BreakableAPIToolbox.GenerateDebrisObject($"NevernamedsItems/Resources/Debris/cashdebris.png", true, 1, 1, 0, 0).gameObject;
            projectile.hitEffects = new ProjectileImpactVFXPool()
            {
                suppressHitEffectsIfOffscreen = false,
                suppressMidairDeathVfx = false,
                overrideMidairZHeight = -1,
                overrideEarlyDeathVfx = null,
                overrideMidairDeathVFX = cashDebris,
                midairInheritsVelocity = true,
                midairInheritsFlip = true,
                midairInheritsRotation = true,
                alwaysUseMidair = true,
                CenterDeathVFXOnProjectile = false,
                HasProjectileDeathVFX = false
            };

            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = CustomClipAmmoTypeToolbox.AddCustomAmmoType("Cashblaster Bullets", "NevernamedsItems/Resources/CustomGunAmmoTypes/cashblaster_clipfull", "NevernamedsItems/Resources/CustomGunAmmoTypes/cashblaster_clipempty");


            gun.quality = PickupObject.ItemQuality.C;
            ETGMod.Databases.Items.Add(gun, false, "ANY");

            ID = gun.PickupObjectId;
        }
        public static int ID;
    }
}
