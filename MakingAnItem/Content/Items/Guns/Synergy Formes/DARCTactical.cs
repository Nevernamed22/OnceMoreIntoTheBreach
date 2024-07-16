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

namespace NevernamedsItems
{

    public class DARCTactical : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("DARC Tactical", "darctactical");
            Game.Items.Rename("outdated_gun_mods:darc_tactical", "nn:arc_tactical+darc_tactical");
            gun.gameObject.AddComponent<DARCTactical>();
            gun.SetShortDescription("Stormed Troopers");
            gun.SetLongDescription("This rapid firing weapon was designed by the ARC Private Security Company for dangerous field combat situations." + "\n\nIt has since become popular in completely different environments, namely the collections of tough guys who like having bigger guns than they need.");

            gun.SetGunSprites("darctactical", 8, true);

            gun.SetAnimationFPS(gun.shootAnimation, 30);
            gun.SetAnimationFPS(gun.reloadAnimation, 0);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(DARCPistol.ID) as Gun, true, false);
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(153) as Gun).gunSwitchGroup;

            //GUN STATS
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(41) as Gun).muzzleFlashEffects;
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0.5f;
            gun.DefaultModule.cooldownTime = 0.099f;
            gun.DefaultModule.numberOfShotsInClip = 32;
            gun.barrelOffset.transform.localPosition = new Vector3(42f / 16f, 8f / 16f, 0f);
            gun.SetBaseMaxAmmo(500);
            gun.gunClass = GunClass.FULLAUTO;
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = "DARC Bullets";

            //BULLET STATS
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage = 4.8f;
            projectile.baseData.force = 4f;

            gun.quality = PickupObject.ItemQuality.EXCLUDED;
            ETGMod.Databases.Items.Add(gun, false, "ANY");


            ID = gun.PickupObjectId;
        }
        public static int ID;
    }
}
