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
using SaveAPI;

namespace NevernamedsItems
{
    public class FractalGun : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Fractal Gun", "fractalgun");
            Game.Items.Rename("outdated_gun_mods:fractal_gun", "nn:fractal_gun");
            var behav = gun.gameObject.AddComponent<FractalGun>();
            behav.preventNormalFireAudio = true;
            behav.overrideNormalFireAudio = "Play_WPN_beretta_shot_01";
            gun.SetShortDescription("Spray and Spray");
            gun.SetLongDescription("This gun repeats infinitely along the Nth dimension." + "\n\nThis gun repeats infinitely along the Nth dimension." + "\n\nThis gun repeats infinitely along the Nth dimension.");

            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).frames[2].eventAudio = "Play_WPN_beretta_shot_01";
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).frames[2].triggerEvent = true;

            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).frames[3].eventAudio = "Play_WPN_minigun_shot_01";
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).frames[3].triggerEvent = true;

            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).frames[3].eventAudio = "Play_WPN_minigun_shot_01";
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).frames[3].triggerEvent = true;

            Alexandria.Assetbundle.GunInt.SetupSprite(gun, Initialisation.gunCollection, "fractalgun_idle_001", 8, "fractalgun_ammonomicon_001");

            gun.SetAnimationFPS(gun.shootAnimation, 12);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0.76f;
            gun.DefaultModule.cooldownTime = 0.3f;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(124) as Gun).muzzleFlashEffects;
            gun.DefaultModule.numberOfShotsInClip = 3;
            gun.DefaultModule.angleVariance = 12;
            gun.barrelOffset.transform.localPosition = new Vector3(21f/16f, 13f/16f, 0f);
            gun.SetBaseMaxAmmo(170);
            gun.gunClass = GunClass.PISTOL;

            //BULLET STATS
            Projectile projectile = gun.DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            projectile.baseData.damage = 7.5f;
            projectile.baseData.speed *= 0.7f;
            projectile.baseData.range *= 10f;

            ProjectileSplitController splitting = projectile.gameObject.AddComponent<ProjectileSplitController>();
            splitting.distanceTillSplit = 4f;
            splitting.amtToSplitTo = 3;
            splitting.maxRecursionAmount = 3;
            splitting.distanceBasedSplit = true;

            gun.DefaultModule.projectiles[0] = projectile;

            gun.quality = PickupObject.ItemQuality.B;
            ETGMod.Databases.Items.Add(gun, null, "ANY");

            ID = gun.PickupObjectId;
        }
        public static int ID;
    }
}
