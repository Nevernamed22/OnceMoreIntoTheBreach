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
    public class CarrionFormeThree : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("carrion_third_form", "carrionformthree");
            Game.Items.Rename("outdated_gun_mods:carrion_third_form", "nn:carrion_third_form");
            var behav = gun.gameObject.AddComponent<CarrionFormeThree>();
            behav.preventNormalFireAudio = true;
            gun.SetShortDescription("");
            gun.SetLongDescription("");

            gun.SetupSprite(null, "carrionformthree_idle_001", 8);

            gun.SetAnimationFPS(gun.shootAnimation, 13);
            gun.SetAnimationFPS(gun.idleAnimation, 13);
            gun.isAudioLoop = true;
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);

            //GUN STATS
            gun.doesScreenShake = true;
            gun.DefaultModule.ammoCost = 5;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Beam;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1f;
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.DefaultModule.cooldownTime = 0.001f;
            gun.DefaultModule.numberOfShotsInClip = 600;
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.BEAM;
            gun.barrelOffset.transform.localPosition = new Vector3(0.75f, 0.56f, 0f);
            gun.SetBaseMaxAmmo(600);
            gun.ammo = 600;

            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).loopStart = 0;

            //SUB TENTACLE
            #region subtentacle
            Projectile subTendrilProj = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0]);

            BasicBeamController subTendrilComp = subTendrilProj.GenerateBeamPrefab(
                "NevernamedsItems/Resources/BeamSprites/carrionsubtendril_mid_001",
                new Vector2(4, 2),
                new Vector2(0, 1),
                new List<string>() { "NevernamedsItems/Resources/BeamSprites/carrionsubtendril_mid_001" },
                13,
                //Impact
                null,
                -1,
                null,
                null,
                //End
                new List<string>() { "NevernamedsItems/Resources/BeamSprites/carrionsubtendril_end_001" },
                13,
                new Vector2(6, 2),
                new Vector2(0, 1),
                //Beginning
                new List<string>() { "NevernamedsItems/Resources/BeamSprites/carrionsubtendril_start_001" },
                13,
                new Vector2(7, 2),
                new Vector2(0, 1),
                //Other Variables
                false
                );

            subTendrilProj.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(subTendrilProj.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(subTendrilProj);
            subTendrilProj.baseData.damage = 10f;
            subTendrilProj.baseData.force *= 1f;
            subTendrilProj.baseData.range = 5.5f;
            subTendrilComp.ProjectileAndBeamMotionModule = new HelixProjectileMotionModule();
            subTendrilComp.boneType = BasicBeamController.BeamBoneType.Projectile;
            subTendrilComp.penetration = 1;
            subTendrilComp.homingRadius = 10;
            subTendrilComp.homingAngularVelocity = 1000;
            CarrionSubTendrilController subtendril = subTendrilProj.gameObject.AddComponent<CarrionSubTendrilController>();
            #endregion

            //MAIN TENTACLE
            #region maintentacle
            List<string> BeamAnimPaths = new List<string>()
            {
                "NevernamedsItems/Resources/BeamSprites/carrionformthree_mid_001",
                "NevernamedsItems/Resources/BeamSprites/carrionformthree_mid_002",
                "NevernamedsItems/Resources/BeamSprites/carrionformthree_mid_003",
                "NevernamedsItems/Resources/BeamSprites/carrionformthree_mid_004",
                "NevernamedsItems/Resources/BeamSprites/carrionformthree_mid_005",
            };
            List<string> BeamEndPaths = new List<string>()
            {
                "NevernamedsItems/Resources/BeamSprites/carrionformthree_end_001",
                "NevernamedsItems/Resources/BeamSprites/carrionformthree_end_002",
                "NevernamedsItems/Resources/BeamSprites/carrionformthree_end_003",
                "NevernamedsItems/Resources/BeamSprites/carrionformthree_end_004",
                "NevernamedsItems/Resources/BeamSprites/carrionformthree_end_005",
            };

            //BULLET STATS
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0]);

            BasicBeamController beamComp = projectile.GenerateBeamPrefab(
                "NevernamedsItems/Resources/BeamSprites/carrionformthree_mid_001",
                new Vector2(16, 5),
                new Vector2(0, 6),
                BeamAnimPaths,
                13,
                //Impact
                null,
                -1,
                null,
                null,
                //End
                BeamEndPaths,
                13,
                new Vector2(10, 5),
                new Vector2(0, 6),
                //Beginning
                null,
                -1,
                null,
                null,
                //Other Variables
                false
                );

            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            projectile.baseData.damage = 50f;
            projectile.baseData.force *= 1f;
            projectile.baseData.range = 16;
            projectile.baseData.speed *= 3;

            beamComp.boneType = BasicBeamController.BeamBoneType.Projectile;
            beamComp.startAudioEvent = "Play_WPN_demonhead_shot_01";
            beamComp.endAudioEvent = "Stop_WPN_All";
            beamComp.penetration = 5;
            beamComp.homingRadius = 10;
            beamComp.homingAngularVelocity = 500;
            beamComp.ProjectileScale *= 1.2f;

            CarrionMainTendrilController mainTendril = projectile.gameObject.AddComponent<CarrionMainTendrilController>();
            mainTendril.subTendrilPrefab = subTendrilProj.gameObject;

            projectile.gameObject.AddComponent<CarrionMovementTentacles>();

            gun.DefaultModule.projectiles[0] = projectile;
            #endregion

            gun.quality = PickupObject.ItemQuality.EXCLUDED; //S
            ETGMod.Databases.Items.Add(gun, null, "ANY");

            Carrion.CarrionForme3ID = gun.PickupObjectId;

        }
        public CarrionFormeThree()
        {

        }
    }
}