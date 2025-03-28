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

namespace NevernamedsItems
{
    public class TriBeam : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Tri-Beam", "tribeam");
            Game.Items.Rename("outdated_gun_mods:tribeam", "nn:tri_beam");
            var behav = gun.gameObject.AddComponent<TriBeam>();
            behav.preventNormalFireAudio = true;
            gun.SetShortDescription("Revolutionary Tech");
            gun.SetLongDescription("This seemingly unremarkable tri-core energy weapon actually marks an astounding leap in the furthering of gunsmithing technology.");

            gun.SetGunSprites("tribeam");

            gun.SetAnimationFPS(gun.shootAnimation, 8);
            gun.isAudioLoop = true;
            gun.gunClass = GunClass.BEAM;
            int iterator = 0;
            for (int i = 0; i < 3; i++)
            {
                gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
            }
            foreach (ProjectileModule mod in gun.Volley.projectiles)
            {
                if (iterator == 1) mod.angleFromAim = 30;
                if (iterator == 2) mod.angleFromAim = -30;
                mod.ammoCost = 10;
                if (mod != gun.DefaultModule) { mod.ammoCost = 0; }
                mod.shootStyle = ProjectileModule.ShootStyle.Beam;
                mod.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
                mod.cooldownTime = 0.001f;
                mod.numberOfShotsInClip = 1000;
                mod.ammoType = GameUIAmmoType.AmmoType.BEAM;

                Projectile projectile = ProjectileUtility.SetupProjectile(86);

                BasicBeamController beamComp = projectile.GenerateAnchoredBeamPrefabBundle(
                    "tribeam_mid_001",
                    Initialisation.ProjectileCollection,
                    Initialisation.projectileAnimationCollection,
                    "TriBeam_Mid",
                    new Vector2(10, 2), 
                    new Vector2(0, -2),
                    muzzleAnimationName: "TriBeam_Start",
                    muzzleColliderDimensions: new Vector2(10, 2),
                    muzzleColliderOffsets: new Vector2(0, -2),
                    impactVFXAnimationName: "LaserBeam_Impact_Blue",
                    impactVFXColliderDimensions: new Vector2(4, 4),
                    impactVFXColliderOffsets: new Vector2(-2, -2)
                    );
                EmmisiveBeams emission = projectile.gameObject.GetOrAddComponent<EmmisiveBeams>();
                emission.EmissivePower = 5;
                emission.EmissiveColorPower = 5;

                projectile.baseData.damage = 18f;
                projectile.baseData.force *= 1f;
                projectile.baseData.range *= 5;
                projectile.baseData.speed *= 10f;
                beamComp.boneType = BasicBeamController.BeamBoneType.Straight;
                if (iterator == 0)
                {
                    beamComp.endAudioEvent = "Stop_WPN_All";
                    beamComp.startAudioEvent = "Play_WPN_moonscraperLaser_shot_01";
                }
                iterator++;

                mod.projectiles[0] = projectile;
            }

            //GUN STATS
            gun.doesScreenShake = false;
            gun.reloadTime = 1f;
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.SetBarrel(16, 10);
            gun.SetBaseMaxAmmo(1000);
            gun.ammo = 1000;

            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).loopStart = 1;

            gun.quality = PickupObject.ItemQuality.C;
            ETGMod.Databases.Items.Add(gun, false, "ANY");

        }
    }
}

