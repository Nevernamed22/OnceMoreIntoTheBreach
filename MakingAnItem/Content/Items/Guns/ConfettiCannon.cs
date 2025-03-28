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
using Alexandria.BreakableAPI;
using Alexandria.Assetbundle;

namespace NevernamedsItems
{
    public class ConfettiCannon : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Confetti Cannon", "confetticannon");
            Game.Items.Rename("outdated_gun_mods:confetti_cannon", "nn:confetti_cannon");
            var behav = gun.gameObject.AddComponent<ConfettiCannon>();
            behav.preventNormalFireAudio = true;
            behav.preventNormalReloadAudio = true;
            behav.overrideNormalFireAudio = "Play_OBJ_chest_surprise_01";
            gun.SetShortDescription("Congratulations!");
            gun.SetLongDescription("A tube containing a small gunpowder charge and a collection of paper confetti."+"\n\nOften used to celebrate gundead birthdays, and by nature of the weapon, gundead funerals.");

            gun.SetGunSprites("confetticannon");


            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.SetAnimationFPS(gun.reloadAnimation, 0);
            for (int i = 0; i < 12; i++)
            {
                gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
            }
            gun.doesScreenShake = false;
                gun.reloadTime = 0.8f;
                gun.barrelOffset.transform.localPosition = new Vector3(25f / 16f, 10f / 16f, 0f);
                gun.SetBaseMaxAmmo(100);
                gun.gunClass = GunClass.SHOTGUN;

            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(12) as Gun).muzzleFlashEffects;

            foreach (ProjectileModule mod in gun.Volley.projectiles)
            {
                //GUN STATS
                mod.ammoCost = mod == gun.DefaultModule ? 1 : 0;
                mod.shootStyle = ProjectileModule.ShootStyle.Automatic;
                mod.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
                mod.angleVariance = 12;
                mod.cooldownTime = 0.1f;
                mod.numberOfShotsInClip = 1;

                //BULLET STATS
                SetUpModuleProjectiles(mod);
            }
            gun.Volley.UsesShotgunStyleVelocityRandomizer = true;

            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = CustomClipAmmoTypeToolbox.AddCustomAmmoType("ConfettiCannon Ammo", "NevernamedsItems/Resources/CustomGunAmmoTypes/confetticannon_clipfull", "NevernamedsItems/Resources/CustomGunAmmoTypes/confetticannon_clipempty");


            gun.quality = PickupObject.ItemQuality.D;
            ETGMod.Databases.Items.Add(gun, false, "ANY");

            ID = gun.PickupObjectId;
        }
        public static int ID;
        private static void SetUpModuleProjectiles(ProjectileModule module)
        {
            if (redConfetti == null)
            {
                redConfetti = madeindivproj("red");
                orangeConfetti = madeindivproj("orange");
                yellowConfetti = madeindivproj("yellow");
                greenConfetti = madeindivproj("green");
                blueConfetti = madeindivproj("blue");
                purpleConfetti = madeindivproj("purple");
                pinkConfetti = madeindivproj("pink");
            }

            module.projectiles[0] = redConfetti;
            module.projectiles.Add(orangeConfetti);
            module.projectiles.Add(yellowConfetti);
            module.projectiles.Add(greenConfetti);
            module.projectiles.Add(blueConfetti);
            module.projectiles.Add(purpleConfetti);
            module.projectiles.Add(pinkConfetti);
        }
        public static Projectile redConfetti;
        public static Projectile orangeConfetti;
        public static Projectile yellowConfetti;
        public static Projectile greenConfetti;
        public static Projectile blueConfetti;
        public static Projectile purpleConfetti;
        public static Projectile pinkConfetti;
        private static Projectile madeindivproj(string colour)
        {
            Projectile projectile = (PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            projectile.baseData.damage = 3.5f;
            projectile.baseData.force *= 0.1f;

            projectile.EasyAnimate(new List<string>() {
                $"confetti_{colour}_001",
                $"confetti_{colour}_002",
                $"confetti_{colour}_003",
                $"confetti_{colour}_004",
                $"confetti_{colour}_005",
                $"confetti_{colour}_006",
                $"confetti_{colour}_007",
                $"confetti_{colour}_008",
                $"confetti_{colour}_009",
                $"confetti_{colour}_010",
            }, 10, new IntVector2(12, 8), 7, false, tk2dSpriteAnimationClip.WrapMode.Loop);

            SlowDownOverTimeModifier slowDown = projectile.gameObject.AddComponent<SlowDownOverTimeModifier>();
            slowDown.extendTimeByRangeStat = true;
            slowDown.activateDriftAfterstop = true;
            slowDown.doRandomTimeMultiplier = true;
            slowDown.killAfterCompleteStop = true;
            slowDown.timeTillKillAfterCompleteStop = 1f;
            slowDown.timeToSlowOver = 0.5f;
            slowDown.targetSpeed = projectile.baseData.speed * 0.1f;

            DriftModifier drift = projectile.gameObject.AddComponent<DriftModifier>();

            GameObject cashDebris = BreakableAPIToolbox.GenerateDebrisObject($"NevernamedsItems/Resources/Debris/confetti_debris_{colour}.png", true, 1, 1, 0, 0).gameObject;
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
            return projectile;
        }
    }
}
