using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using Alexandria.Misc;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    public class BlueGun : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Blue Gun", "bluegun");
            Game.Items.Rename("outdated_gun_mods:blue_gun", "nn:blue_gun");
            var behav = gun.gameObject.AddComponent<BlueGun>();
            behav.preventNormalFireAudio = true;
            gun.SetShortDescription("Why So Blue?");
            gun.SetLongDescription("Blue Guns like these are often used by Hegemony Soldiers to simulate combat situations without live weapons. This has, as per the usual, been convoluted within the Gungeon."+"\n\nCan be reloaded with blanks to refill ammo.");

            gun.SetGunSprites("bluegun");

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);

            //GUN STATS
            gun.doesScreenShake = false;
            gun.DefaultModule.ammoCost = 8;
            gun.DefaultModule.angleVariance = 0;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Beam;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1f;
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.DefaultModule.cooldownTime = 0.001f;
            gun.DefaultModule.numberOfShotsInClip = -1;
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.BEAM;
            gun.barrelOffset.transform.localPosition = new Vector3(28f / 16f, 8f / 16f, 0f);
            gun.SetBaseMaxAmmo(300);
            gun.ammo = 300;
            gun.gunClass = GunClass.BEAM;

            List<string> BeamAnimPaths = new List<string>()
            {
                "NevernamedsItems/Resources/BeamSprites/tribeam_mid_001",
                "NevernamedsItems/Resources/BeamSprites/tribeam_mid_002",
            };

            List<string> BeamImpactPaths = new List<string>()
            {
                "NevernamedsItems/Resources/BeamSprites/bluebeam_impact_001",
                "NevernamedsItems/Resources/BeamSprites/bluebeam_impact_002",
                "NevernamedsItems/Resources/BeamSprites/bluebeam_impact_003",
                "NevernamedsItems/Resources/BeamSprites/bluebeam_impact_004",
            };

            //BULLET STATS
            Projectile projectile = (PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0].InstantiateAndFakeprefab();

            BasicBeamController beamComp = projectile.GenerateBeamPrefab(
                "NevernamedsItems/Resources/BeamSprites/tribeam_mid_001",
                new Vector2(10, 2),
                    new Vector2(0, 4),
                BeamAnimPaths,
                13,
                //Impact
                BeamImpactPaths,
                13,
                new Vector2(4, 4),
                    new Vector2(7, 7),
                //End
                null,
                -1,
                null,
                null,
                //Beginning
                null,
                -1,
                null,
                null,
                //Other Variables
                10
                );


            projectile.baseData.damage = 10f;
            projectile.baseData.force *= 1f;
            projectile.baseData.range *= 200;
            projectile.baseData.speed *= 5;

            beamComp.boneType = BasicBeamController.BeamBoneType.Projectile;
            beamComp.startAudioEvent = "Play_WPN_radiationlaser_shot_01";
            beamComp.endAudioEvent = "Stop_WPN_All";
            //beamComp.interpolateStretchedBones = false;
            BeamBlankModifier bemblank = projectile.gameObject.AddComponent<BeamBlankModifier>();
            bemblank.chancePerTick = 0.5f;
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.BEAM;

            gun.DefaultModule.projectiles[0] = projectile;

            gun.quality = PickupObject.ItemQuality.A;
            ETGMod.Databases.Items.Add(gun, false, "ANY");

        }
        public override void OnReloadPressedSafe(PlayerController player, Gun gun, bool manualReload)
        {
            if (player.Blanks > 0)
            {
                player.Blanks -= 1;
                AkSoundEngine.PostEvent("Play_OBJ_ammo_pickup_01", player.gameObject);
                player.PlayEffectOnActor(PickupObjectDatabase.GetById(78).GetComponent<AmmoPickup>().pickupVFX, Vector3.zero, true, false, false);
                gun.GainAmmo(150);
            }
            base.OnReloadPressedSafe(player, gun, manualReload);
        }

    }
}