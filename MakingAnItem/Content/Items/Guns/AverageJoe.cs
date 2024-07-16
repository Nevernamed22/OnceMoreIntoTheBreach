using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.Misc;
using Alexandria.ItemAPI;
using SaveAPI;

namespace NevernamedsItems
{

    public class AverageJoe : AdvancedGunBehavior
    {

        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Average Joe", "averagejoe");
            Game.Items.Rename("outdated_gun_mods:average_joe", "nn:average_joe");
            gun.gameObject.AddComponent<AverageJoe>();
            gun.SetShortDescription("Everyman");
            gun.SetLongDescription("Shots from this gun deal damage equal to the average damage per bullet of each gun in the owner's arsenal."+ "\n\nA boring gun for the boring everyman- flagrantly and irresponsibly having been aesthetically modified with a red sash.");

            gun.SetGunSprites("averagejoe");

            gun.SetAnimationFPS(gun.shootAnimation, 17);
            gun.SetAnimationFPS(gun.reloadAnimation, 0);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(56) as Gun, true, false);
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(54) as Gun).gunSwitchGroup;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(184) as Gun).muzzleFlashEffects;

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0.7f;
            gun.DefaultModule.cooldownTime = 0.4f;
            gun.DefaultModule.numberOfShotsInClip = 10;
            gun.barrelOffset.transform.localPosition = new Vector3(33f / 16f, 16f / 16f, 0f);
            gun.SetBaseMaxAmmo(300);
            gun.ammo = 300;
            gun.gunClass = GunClass.FULLAUTO;

            //BULLET STATS
            Projectile projectile = gun.DefaultModule.projectiles[0].InstantiateAndFakeprefab();          
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.SetProjectileSprite("boltcaster_proj", 23, 3, true, tk2dBaseSprite.Anchor.MiddleCenter, 13, 3);
            projectile.gameObject.AddComponent<DamageAverageBehaviour>();

            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = "Risk Rifle Bullets";

            gun.quality = PickupObject.ItemQuality.C;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            ID = gun.PickupObjectId;
        }
        public static int ID;       
    }
}

