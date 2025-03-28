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
    public class MoltenHeat : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Molten Heat", "moltenheat");
            Game.Items.Rename("outdated_gun_mods:molten_heat", "nn:molten_heat");
            var behav = gun.gameObject.AddComponent<MoltenHeat>();
            behav.preventNormalFireAudio = true;
            gun.SetShortDescription("Packed");
            gun.SetLongDescription("This normal handgun was accidentally dipped into the molten steel of the forge, partially melting it in the process.");

            gun.SetGunSprites("moltenheat", 8, false, 2);


            gun.isAudioLoop = true;
            gun.gunClass = GunClass.BEAM;
            int iterator = 1;
            for (int i = 0; i < 4; i++)
            {
                gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
            }
            foreach (ProjectileModule mod in gun.Volley.projectiles)
            {
                mod.ammoCost = 10;
                if (mod != gun.DefaultModule) { mod.ammoCost = 0; }

                mod.angleVariance = 0;
                mod.shootStyle = ProjectileModule.ShootStyle.Beam;
                mod.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
                mod.cooldownTime = 0.001f;
                mod.numberOfShotsInClip = -1;
                mod.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
                mod.customAmmoType = "red_beam";

                Projectile projectile = ProjectileUtility.SetupProjectile(658);

                projectile.baseData.damage = 15f;
                projectile.baseData.force = 15f;
                projectile.baseData.range = 100;
                

                switch (iterator)
                {
                    case 1: projectile.baseData.speed = 90f; break;
                    case 2: projectile.baseData.speed = 30f; break;
                    case 3: projectile.baseData.speed = 10f; break;
                    case 4: projectile.baseData.speed = 5f; break;
                }
                projectile.gameObject.GetComponent<BasicBeamController>().homingAngularVelocity = 0;
                projectile.gameObject.GetComponent<BasicBeamController>().endAudioEvent = "Stop_WPN_demonhead_loop_01";
                projectile.gameObject.GetComponent<BasicBeamController>().startAudioEvent = "Play_WPN_demonhead_shot_01";

                iterator++;
                mod.projectiles[0] = projectile;
            }

            //GUN STATS
            gun.doesScreenShake = false;
            gun.reloadTime = 1f;
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.SetBarrel(19, 12);
            gun.SetBaseMaxAmmo(500);
            gun.ammo = 500;

            gun.gunHandedness = GunHandedness.OneHanded;

            gun.quality = PickupObject.ItemQuality.B;
            ETGMod.Databases.Items.Add(gun, false, "ANY");

        }
    }
}

