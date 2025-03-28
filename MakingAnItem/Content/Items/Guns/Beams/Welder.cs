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
using Alexandria.Assetbundle;
using Dungeonator;
using Alexandria.SoundAPI;

namespace NevernamedsItems
{
    public class Welder : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Welder", "welder");
            Game.Items.Rename("outdated_gun_mods:welder", "nn:welder");
            var behav = gun.gameObject.AddComponent<Welder>();
            gun.SetShortDescription("Big Weld");
            gun.SetLongDescription("Designed for competitive long range welding, before the sport was shut down by Hegemony regulations.");

            gun.SetGunSprites("welder", 8, false, 2);

            gun.SetAnimationFPS(gun.shootAnimation, 8);
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).loopStart = 1;

            gun.isAudioLoop = true;
            gun.gunClass = GunClass.BEAM;
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);

            gun.AddCustomSwitchGroup("nn:EMPTY", "", "");

            gun.DefaultModule.angleVariance = 0;
            gun.DefaultModule.ammoCost = 11;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Beam;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.DefaultModule.cooldownTime = 0.001f;
            gun.DefaultModule.numberOfShotsInClip = 400;

            Projectile Spark = ProjectileUtility.SetupProjectile(86);
            Spark.SetProjectileSprite("welderbeam_impact_003", 14, 14, true, tk2dBaseSprite.Anchor.MiddleCenter, 6, 6);
            ScaleChangeOverTimeModifier scl = Spark.gameObject.AddComponent<ScaleChangeOverTimeModifier>();
            scl.destroyAfterChange = true;
            Spark.gameObject.name = "Welding Spark Projectile";
            Spark.baseData.speed /= 2;
            scl.ScaleToChangeTo = 0.1f;
            scl.suppressDeathFXIfdestroyed = true;
            scl.timeToChangeOver = 0.25f;
            ProjectileSpriteRotation rot = Spark.gameObject.AddComponent<ProjectileSpriteRotation>();
            rot.RotPerFrame = 5;
            Spark.onDestroyEventName = "";
            Spark.objectImpactEventName = "";
            Shader glowshader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTiltedCutoutEmissive");
            tk2dSprite sproot = Spark.GetComponent<tk2dSprite>();
            if (sproot != null)
            {
                sproot.usesOverrideMaterial = true;
                sproot.renderer.material.shader = glowshader;
                sproot.renderer.material.EnableKeyword("BRIGHTNESS_CLAMP_ON");
                sproot.renderer.material.SetFloat("_EmissivePower", 100);
                sproot.renderer.material.SetFloat("_EmissiveColorPower", 100);
                sproot.renderer.material.SetColor("_EmissiveColor", new Color(1f, 161f / 255f, 0f));
            }

            Projectile projectile = ProjectileUtility.SetupProjectile(86);
            BasicBeamController beamComp = projectile.GenerateAnchoredBeamPrefabBundle(
                        "welderbeam_mid_001",
                        Initialisation.ProjectileCollection,
                        Initialisation.projectileAnimationCollection,
                        "WelderBeamMid",
                        new Vector2(20, 2),
                        new Vector2(0, -1),
                        muzzleAnimationName: "WelderBeamStart",
                        muzzleColliderDimensions: new Vector2(20, 2),
                        muzzleColliderOffsets: new Vector2(0, -1),
                        endAnimation: "WelderBeamEnd",
                        endColliderDimensions: new Vector2(20, 2),
                        endColliderOffsets: new Vector2(0, -1),
                        impactVFXAnimationName: "WelderBeamImpact",
                        impactVFXColliderDimensions: new Vector2(4, 4),
                        impactVFXColliderOffsets: new Vector2(-2, -2)
                        );
            EmmisiveBeams emission = projectile.gameObject.GetOrAddComponent<EmmisiveBeams>();
            emission.EmissivePower = 100;
            emission.EmissiveColorPower = 100;
            emission.EmissiveColor = new Color(1f, 161f / 255f, 0f);

            projectile.gameObject.name = "Welder Beam";
            projectile.baseData.damage = 15f;
            projectile.baseData.force = 20f;
            projectile.baseData.range = 7;
            projectile.baseData.speed = 60f;
            beamComp.boneType = BasicBeamController.BeamBoneType.Straight;
            beamComp.endAudioEvent = "Stop_WPN_demonhead_loop_01";
            beamComp.startAudioEvent = "Play_WPN_demonhead_shot_01";
            beamComp.penetration = 10;
            beamComp.PenetratesCover = true;

            BeamProjSpewModifier sparkler = projectile.gameObject.AddComponent<BeamProjSpewModifier>();
            sparkler.tickOnHit = true;
            sparkler.positionToSpawn = BeamProjSpewModifier.SpawnPosition.ENEMY_IMPACT;
            sparkler.accuracyVariance = 360;
            sparkler.bulletToSpew = Spark;

            gun.Volley.projectiles[0].projectiles[0] = projectile;


            //GUN STATS
            gun.gunScreenShake = (PickupObjectDatabase.GetById(60) as Gun).gunScreenShake;
            gun.reloadTime = 1.25f;
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.SetBarrel(30, 14);
            gun.SetBaseMaxAmmo(900);
            gun.ammo = 900;

            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = "Y-Beam Laser";


            gun.gunHandedness = GunHandedness.TwoHanded;

            gun.quality = PickupObject.ItemQuality.B;
            ETGMod.Databases.Items.Add(gun, false, "ANY");

        }
    }
}