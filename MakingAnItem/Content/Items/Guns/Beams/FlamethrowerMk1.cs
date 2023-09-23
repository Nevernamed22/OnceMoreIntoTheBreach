using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.Misc ;

namespace NevernamedsItems
{
    public class FlamethrowerMk1 : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Flamethrower Mk 1", "flamethrowermk1");
            Game.Items.Rename("outdated_gun_mods:flamethrower_mk_1", "nn:flamethrower_mk_1");
            var behav = gun.gameObject.AddComponent<FlamethrowerMk1>();
            gun.SetShortDescription("Bearing Down On Me");
            gun.SetLongDescription("A crudely modified Mega-Douser filled with gasoline."+"\n\nThe very first experiment of flamesmith Lucinda \"Third Degree\" Burns.");

            gun.SetupSprite(null, "flamethrowermk1_idle_001", 8);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(10) as Gun).gunSwitchGroup;

            //GUN STATS
            gun.isAudioLoop = true;
            gun.doesScreenShake = false;
            gun.DefaultModule.ammoCost = 10;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Beam;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1f;
            gun.muzzleFlashEffects = VFXToolbox.CreateVFXPool("Flamethrower Mk1 Muzzleflash",
                new List<string>()
                {
                    "NevernamedsItems/Resources/BeamSprites/liquidfirebeam_impact_001",
                "NevernamedsItems/Resources/BeamSprites/liquidfirebeam_impact_002",
                "NevernamedsItems/Resources/BeamSprites/liquidfirebeam_impact_003",
                "NevernamedsItems/Resources/BeamSprites/liquidfirebeam_impact_004",
                "NevernamedsItems/Resources/BeamSprites/liquidfirebeam_impact_005",
                "NevernamedsItems/Resources/BeamSprites/liquidfirebeam_impact_006",
                "NevernamedsItems/Resources/BeamSprites/liquidfirebeam_impact_007",
                "NevernamedsItems/Resources/BeamSprites/liquidfirebeam_impact_008",
                },
                17, //FPS
                new IntVector2(23, 23), //Dimensions
                tk2dBaseSprite.Anchor.MiddleLeft, //Anchor
                false, //Uses a Z height off the ground
                0, //The Z height, if used
                false,
               VFXAlignment.Fixed,
               -1,
               null,
                    tk2dSpriteAnimationClip.WrapMode.Loop
                  ) ;
            gun.usesContinuousMuzzleFlash = true;
            gun.DefaultModule.cooldownTime = 0.1f;
            gun.DefaultModule.angleVariance = 0f;
            gun.DefaultModule.numberOfShotsInClip = -1;
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.BEAM;
            gun.barrelOffset.transform.localPosition = new Vector3(18f / 16f, 8f / 16f, 0f);
            gun.SetBaseMaxAmmo(900);
            gun.ammo = 900;
            gun.gunClass = GunClass.FIRE;

            List<string> BeamAnimPaths = new List<string>()
            {
                "NevernamedsItems/Resources/BeamSprites/liquidfirebeam_mid_001",
                "NevernamedsItems/Resources/BeamSprites/liquidfirebeam_mid_002",
                "NevernamedsItems/Resources/BeamSprites/liquidfirebeam_mid_003",
                "NevernamedsItems/Resources/BeamSprites/liquidfirebeam_mid_004",
                "NevernamedsItems/Resources/BeamSprites/liquidfirebeam_mid_005",
            };

            List<string> BeamImpactPaths = new List<string>()
            {
                "NevernamedsItems/Resources/BeamSprites/liquidfirebeam_impact_001",
                "NevernamedsItems/Resources/BeamSprites/liquidfirebeam_impact_002",
                "NevernamedsItems/Resources/BeamSprites/liquidfirebeam_impact_003",
                "NevernamedsItems/Resources/BeamSprites/liquidfirebeam_impact_004",
                "NevernamedsItems/Resources/BeamSprites/liquidfirebeam_impact_005",
                "NevernamedsItems/Resources/BeamSprites/liquidfirebeam_impact_006",
                "NevernamedsItems/Resources/BeamSprites/liquidfirebeam_impact_007",
                "NevernamedsItems/Resources/BeamSprites/liquidfirebeam_impact_008",
            };

            //BULLET STATS
            Projectile projectile = (PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0].InstantiateAndFakeprefab();

            BasicBeamController beamComp = projectile.GenerateBeamPrefab(
                "NevernamedsItems/Resources/BeamSprites/liquidfirebeam_mid_001",
                new Vector2(16, 4),
                new Vector2(0, 6),
                BeamAnimPaths,
                13,
                //Impact
                BeamImpactPaths,
                17,
                new Vector2(11, 11),
                new Vector2(6, 6),
                //End
                null,
                -1,
                null,
                null,
                //Beginning
                null,
                -1,
                null,
                null
                );

            projectile.baseData.damage = 15f;
            projectile.baseData.force =40f;
            projectile.baseData.range *= 200;
            projectile.baseData.speed = 20;
           


            beamComp.boneType = BasicBeamController.BeamBoneType.Projectile;
            beamComp.interpolateStretchedBones = true;
            beamComp.collisionSeparation = true;
            beamComp.TileType = BasicBeamController.BeamTileType.Flowing;
            beamComp.endType = BasicBeamController.BeamEndType.Persist;

            projectile.AppliesFire = true;
            projectile.fireEffect = StaticStatusEffects.hotLeadEffect;
            beamComp.statusEffectChance = 1f;
            beamComp.TimeToStatus = 0.1f;

            GoopModifier gooper = projectile.gameObject.AddComponent<GoopModifier>();
            gooper.goopDefinition = GoopUtility.FireDef;
            gooper.SpawnGoopOnCollision = true;
            gooper.CollisionSpawnRadius = 1;

            ParticleShitter particles = projectile.gameObject.GetOrAddComponent<ParticleShitter>();
            particles.particleType = GlobalSparksDoer.SparksType.STRAIGHT_UP_FIRE;

            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = "Y-Beam Laser";

            gun.DefaultModule.projectiles[0] = projectile;

            gun.quality = PickupObject.ItemQuality.C;
            ETGMod.Databases.Items.Add(gun, false, "ANY");

        }
    }
}