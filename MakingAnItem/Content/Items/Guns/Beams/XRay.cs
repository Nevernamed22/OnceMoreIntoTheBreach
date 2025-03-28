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
    public class XRay : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("X-Ray", "xray");
            Game.Items.Rename("outdated_gun_mods:xray", "nn:x_ray");
            var behav = gun.gameObject.AddComponent<XRay>();
            behav.preventNormalFireAudio = true;
            gun.SetShortDescription("Ionizing");
            gun.SetLongDescription("A portable medical imaging device brought to the Gungeon by the Medecins Sans Diplome. Hasn't seen much field use, as diagnosis is a very small part of their standard operating proceedure.");

            gun.SetGunSprites("xray", 2, false, 2);


            gun.isAudioLoop = true;
            gun.gunClass = GunClass.BEAM;
            int iterator = 0;
            for (int i = 0; i < 2; i++)
            {
                gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
            }
            foreach (ProjectileModule mod in gun.Volley.projectiles)
            {
                mod.ammoCost = 10;
                if (mod != gun.DefaultModule) { mod.ammoCost = 0; }
                mod.shootStyle = ProjectileModule.ShootStyle.Beam;
                mod.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
                mod.cooldownTime = 0.001f;
                mod.numberOfShotsInClip = -1;
                mod.ammoType = GameUIAmmoType.AmmoType.BEAM;

                Projectile projectile = ProjectileUtility.SetupProjectile(86);

                string x = iterator == 0 ? "black" : "white";
                BasicBeamController beamComp = projectile.GenerateAnchoredBeamPrefabBundle(
                        $"xraybeam{x}_mid_001",
                        Initialisation.ProjectileCollection,
                        Initialisation.projectileAnimationCollection,
                        $"xraybeam{x}_mid",
                        new Vector2(16, 4),
                        new Vector2(0, -2),
                        impactVFXAnimationName: "XRayImpact",
                        impactVFXColliderDimensions: new Vector2(4, 4),
                        impactVFXColliderOffsets: new Vector2(-2, -2)
                        );
                EmmisiveBeams emission = projectile.gameObject.GetOrAddComponent<EmmisiveBeams>();
                emission.EmissivePower = 50;
                emission.EmissiveColorPower = 5;

                projectile.baseData.damage = 15f;
                projectile.baseData.force = 15f;
                projectile.baseData.range = 50;
                projectile.baseData.speed = 50f;
                beamComp.boneType = BasicBeamController.BeamBoneType.Projectile;
                projectile.gameObject.name = $"XRay Beam {x}";

                BeamAutoAddMotionModule helix = projectile.gameObject.AddComponent<BeamAutoAddMotionModule>();
                helix.Helix = true;
                beamComp.penetration = 2;
                beamComp.PenetratesCover = true;
                if (iterator == 0)
                {
                    beamComp.endAudioEvent = "Stop_WPN_All";
                    beamComp.startAudioEvent = "Play_WPN_moonscraperLaser_shot_01";
                    helix.invertHelix = true;
                }
                iterator++;

                mod.projectiles[0] = projectile;
            }

            //GUN STATS
            gun.doesScreenShake = false;
            gun.reloadTime = 1f;
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.SetBarrel(20, 5);
            gun.SetBaseMaxAmmo(500);
            gun.ammo = 500;

            gun.gunHandedness = GunHandedness.HiddenOneHanded;

            gun.quality = PickupObject.ItemQuality.B;
            ETGMod.Databases.Items.Add(gun, false, "ANY");

        }
    }
}

