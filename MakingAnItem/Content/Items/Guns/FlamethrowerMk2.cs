using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.Misc ;

namespace NevernamedsItems
{

    public class FlamethrowerMk2 : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Flamethrower Mk 2", "flamethrowermk2");
            Game.Items.Rename("outdated_gun_mods:flamethrower_mk_2", "nn:flamethrower_mk_2");
            gun.gameObject.AddComponent<FlamethrowerMk2>();
            gun.SetShortDescription("I Fear No Man");
            gun.SetLongDescription("Spews an ignited gasoline vapor."+"\n\nThe favoured weapon of Lucinda Burns as part of her 'char-grill' fighting technique.");

            gun.SetGunSprites("flamethrowermk2");

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(153) as Gun).gunSwitchGroup;
            gun.doesScreenShake = false;
            //GUN STATS
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(83) as Gun).muzzleFlashEffects;
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.2f;
            gun.DefaultModule.cooldownTime = 0.025f;
            gun.DefaultModule.numberOfShotsInClip = 130;
            gun.barrelOffset.transform.localPosition = new Vector3(40f / 16f, 11f / 16f, 0f);
            gun.SetBaseMaxAmmo(1300);
            gun.ammo = 1300;
            gun.gunClass = GunClass.FIRE;
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = "Y-Beam Laser";
            gun.DefaultModule.angleVariance = 10;
            //BULLET STATS
            Projectile projectile = StandardisedProjectiles.flamethrower.InstantiateAndFakeprefab();
            projectile.GetComponent<ParticleShitter>().particlesPerSecond = 20;
            gun.DefaultModule.projectiles[0] = projectile;

            gun.quality = PickupObject.ItemQuality.B;
            ETGMod.Databases.Items.Add(gun, null, "ANY");


            ID = gun.PickupObjectId;
        }
        public static int ID;
    }
}
