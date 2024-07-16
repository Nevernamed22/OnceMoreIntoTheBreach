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

namespace NevernamedsItems
{
    public class ParticleBeam : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Particle Beam", "particlebeam");
            Game.Items.Rename("outdated_gun_mods:particle_beam", "nn:particle_beam");
            var behav = gun.gameObject.AddComponent<ParticleBeam>();
            behav.preventNormalFireAudio = true;
            gun.SetShortDescription("Stonefaced");
            gun.SetLongDescription("An invisible laser which tears chunks from intersecting walls in the form of superheated shrapnel."+"\n\nFavoured by Gargoyle Hunters.");

            gun.SetGunSprites("particlebeam");

            gun.isAudioLoop = true;
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);

            //GUN STATS
            gun.doesScreenShake = false;
            gun.DefaultModule.ammoCost = 15;
            gun.DefaultModule.angleVariance = 0;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Beam;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1f;
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.DefaultModule.cooldownTime = 0.001f;
            gun.DefaultModule.numberOfShotsInClip = -1;
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.BEAM;
            gun.barrelOffset.transform.localPosition = new Vector3(32f / 16f, 7f / 16f, 0f);
            gun.SetBaseMaxAmmo(1000);
            gun.ammo = 1000;
            gun.gunClass = GunClass.BEAM;

            List<string> BeamAnimPaths = new List<string>()
            {
                "NevernamedsItems/Resources/BeamSprites/redbeam_seg_001",
                "NevernamedsItems/Resources/BeamSprites/redbeam_seg_002",
                "NevernamedsItems/Resources/BeamSprites/redbeam_seg_003",
                "NevernamedsItems/Resources/BeamSprites/redbeam_seg_004"
            };
            List<string> ImpactAnimPaths = new List<string>()
            {
                "NevernamedsItems/Resources/BeamSprites/redbeam_impact_001",
                "NevernamedsItems/Resources/BeamSprites/redbeam_impact_002",
                "NevernamedsItems/Resources/BeamSprites/redbeam_impact_003",
                "NevernamedsItems/Resources/BeamSprites/redbeam_impact_004",
            };

            Projectile projectile = (PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            BasicBeamController beamComp = projectile.GenerateBeamPrefab(
                "NevernamedsItems/Resources/BeamSprites/redbeam_seg_001", new Vector2(18, 2), new Vector2(0, 8), BeamAnimPaths, 8,
                ImpactAnimPaths, 13, new Vector2(4, 4), new Vector2(7, 7));
            projectile.baseData.damage *= 4;
            projectile.baseData.range *= 2;
            projectile.baseData.speed =  400;

            projectile.sprite.renderer.enabled = false;

            beamComp.boneType = BasicBeamController.BeamBoneType.Straight;
            beamComp.interpolateStretchedBones = false;
            beamComp.ContinueBeamArtToWall = true;

            beamComp.penetration += 10;

            beamComp.boneType = BasicBeamController.BeamBoneType.Projectile;
            beamComp.startAudioEvent = "Play_WPN_radiationlaser_shot_01";
            beamComp.endAudioEvent = "Stop_WPN_All";

            Projectile spewProj =(PickupObjectDatabase.GetById(83) as Gun).DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            spewProj.baseData.damage = 2;
            RandomProjectileStatsComponent randomStats = spewProj.gameObject.AddComponent<RandomProjectileStatsComponent>();
            randomStats.randomScale = true;
            randomStats.randomSpeed = true;
            spewProj.gameObject.AddComponent<ProjectileTilemapGracePeriod>();

            ScaleChangeOverTimeModifier scaler = spewProj.gameObject.GetOrAddComponent<ScaleChangeOverTimeModifier>();
            scaler.destroyAfterChange = true;
            scaler.timeToChangeOver = 0.3f;
            scaler.ScaleToChangeTo = 0.01f;
            scaler.suppressDeathFXIfdestroyed = true;
            
            
            BeamProjSpewModifier spew = projectile.gameObject.AddComponent<BeamProjSpewModifier>();
            spew.bulletToSpew = spewProj;
            spew.accuracyVariance = 10;
            spew.angleFromAim = 180;

            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = "Y-Beam Laser";

            gun.DefaultModule.projectiles[0] = projectile;

            gun.quality = PickupObject.ItemQuality.B;
            ETGMod.Databases.Items.Add(gun, false, "ANY");

        }
    }
}