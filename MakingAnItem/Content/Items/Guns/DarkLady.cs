using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{

    public class DarkLady : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Dark Lady", "darklady");
            Game.Items.Rename("outdated_gun_mods:dark_lady", "nn:dark_lady");
            var behav = gun.gameObject.AddComponent<DarkLady>();
            gun.SetShortDescription("Two-Stage Mechanism");
            gun.SetLongDescription("Uncooperative. Half of the bullets it fires seem to have a mind of their own.\n\nAn infamous firearm.");

            gun.SetGunSprites("darklady", 8, false, 2);

          //  gun.SetAnimationFPS(gun.shootAnimation, 13);

            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(30) as Gun).gunSwitchGroup;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(30) as Gun).muzzleFlashEffects;

            for (int i = 0; i < 2; i++)
            {
                gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(15) as Gun, true, false);
            }

            //GUN STATS
            foreach (ProjectileModule mod in gun.Volley.projectiles)
            {
                mod.ammoCost = 1;
                mod.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
                mod.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
                mod.cooldownTime = 0.3f;
                mod.angleVariance = 7f;
                mod.numberOfShotsInClip = 6;

                Projectile projectile = ProjectileSetupUtility.MakeProjectile(15, 7f);
                mod.projectiles[0] = projectile;
                if (mod != gun.DefaultModule)
                {
                    mod.ammoCost = 0;
                    SelfReAimBehaviour reaim = projectile.gameObject.GetOrAddComponent<SelfReAimBehaviour>();
                    reaim.trigger = SelfReAimBehaviour.ReAimTrigger.IMMEDIATE;
                }
                else
                {
                    projectile.baseData.speed *= 1.3f;
                }
            }

            gun.reloadTime = 0.85f;
            gun.SetBarrel(31, 20);
            gun.SetBaseMaxAmmo(130);
            gun.gunClass = GunClass.PISTOL;

            //BULLET STATS
            gun.quality = PickupObject.ItemQuality.B;
            ETGMod.Databases.Items.Add(gun, false, "ANY");

            gun.AddClipSprites("redbullets");

            ID = gun.PickupObjectId;
        }
        public static int ID;
    }
}


