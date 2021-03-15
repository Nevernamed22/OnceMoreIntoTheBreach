using System;
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
    public class HeatRay : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Heat Ray", "heatray");
            Game.Items.Rename("outdated_gun_mods:heat_ray", "nn:heat_ray");
       var behav =      gun.gameObject.AddComponent<HeatRay>();
            behav.overrideNormalFireAudio = "Play_ENM_shelleton_beam_01";
            behav.preventNormalFireAudio = true;
            gun.SetShortDescription("Set to Defrost");
            gun.SetLongDescription("An old weaponised heating coil, will burn enemies it's focused on for long enough.");

            gun.SetupSprite(null, "heatray_idle_001", 8);

            gun.SetAnimationFPS(gun.shootAnimation, 8);
            gun.isAudioLoop = true;
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);

            //GUN STATS
            gun.doesScreenShake = false;
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Beam;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1f;
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.DefaultModule.cooldownTime = 0.001f;
            gun.DefaultModule.numberOfShotsInClip = 1000;
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.BEAM;
            gun.barrelOffset.transform.localPosition = new Vector3(1.06f, 0.31f, 0f);
            gun.SetBaseMaxAmmo(1000);
            gun.ammo = 1000;

            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;

            List<string> BeamAnimPaths = new List<string>()
            {
                "NevernamedsItems/Resources/BeamSprites/heatray_seg_001",
                "NevernamedsItems/Resources/BeamSprites/heatray_seg_002",
                "NevernamedsItems/Resources/BeamSprites/heatray_seg_003",
                "NevernamedsItems/Resources/BeamSprites/heatray_seg_004",
                "NevernamedsItems/Resources/BeamSprites/heatray_seg_005",
                "NevernamedsItems/Resources/BeamSprites/heatray_seg_006",
                "NevernamedsItems/Resources/BeamSprites/heatray_seg_007",
                "NevernamedsItems/Resources/BeamSprites/heatray_seg_008",
            };

            //BULLET STATS
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0]);
            BasicBeamController beamComp = projectile.GenerateBeamPrefab(
                "NevernamedsItems/Resources/BeamSprites/heatray_seg_001", new Vector2(10, 2), new Vector2(0, 3), BeamAnimPaths, 16);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            projectile.baseData.damage *= 0.8f;
            projectile.baseData.force *= 0.1f;
            projectile.baseData.range *= 2;
            projectile.baseData.speed *= 0.7f;
            beamComp.boneType = BasicBeamController.BeamBoneType.Projectile;
            beamComp.interpolateStretchedBones = false;

            projectile.AppliesFire = true;
            projectile.fireEffect = StaticStatusEffects.hotLeadEffect;
            beamComp.statusEffectChance = 1;
            beamComp.TimeToStatus = 1;

            gun.DefaultModule.projectiles[0] = projectile;

            gun.quality = PickupObject.ItemQuality.EXCLUDED; //D
            ETGMod.Databases.Items.Add(gun, null, "ANY");

        }       
        public HeatRay()
        {

        }
    }
}

