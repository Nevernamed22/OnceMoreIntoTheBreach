using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;
using MonoMod.RuntimeDetour;
using System.Reflection;
using SaveAPI;
using Alexandria.Assetbundle;

namespace NevernamedsItems
{
    public class C7A2 : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("C7A2", "cza2");
            Game.Items.Rename("outdated_gun_mods:c7a2", "nn:c7a2");
            var behav = gun.gameObject.AddComponent<C7A2>();
            gun.SetShortDescription("O'");
            gun.SetLongDescription("A standard issue military rifle from a Pre-Hegemony civilisation.");

            gun.SetGunSprites("cza2");

            gun.SetAnimationFPS(gun.shootAnimation, 16);
            gun.SetAnimationFPS(gun.reloadAnimation, 0);
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(96) as Gun).gunSwitchGroup;

            gun.gunHandedness = GunHandedness.TwoHanded;
            for (int i = 0; i < 3; i++)
            {
                gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
            }
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(96) as Gun).muzzleFlashEffects;

            //GUN STATS
            gun.reloadTime = 2f;
            gun.barrelOffset.transform.localPosition = new Vector3(46f/16f, 8f / 16f, 0f);
            gun.SetBaseMaxAmmo(650);
            gun.ammo = 650;
            gun.gunClass = GunClass.FULLAUTO;

            //GUN STATS
            int j = 0;
            foreach (ProjectileModule mod in gun.Volley.projectiles)
            {

                mod.ammoCost = 1;
                mod.shootStyle = ProjectileModule.ShootStyle.Automatic;
                mod.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
                mod.cooldownTime = 0.19f;
                mod.angleVariance = 0f;
                mod.numberOfShotsInClip = 20;

                Projectile projectile = ProjectileSetupUtility.MakeProjectile(86, 2.5f, 700, 30);
                projectile.baseData.force = 4;
                projectile.hitEffects = (PickupObjectDatabase.GetById(15) as Gun).DefaultModule.projectiles[0].hitEffects;
                mod.projectiles[0] = projectile;

                switch (j)
                {
                    case 0: 
                        mod.positionOffset = new Vector2(0, -0.3f);
                        mod.ammoCost = 0;
                        break;
                    case 1:
                        mod.ammoCost = 1;
                        break;
                    case 2:
                        mod.positionOffset = new Vector2(0, 0.3f);
                        mod.ammoCost = 0;
                        break;
                }
                j++;
            }





            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.SMALL_BULLET;
            gun.AddShellCasing(18, 9);


            gun.quality = PickupObject.ItemQuality.C;
            ETGMod.Databases.Items.Add(gun, null, "ANY");

            gun.AddToSubShop(ItemBuilder.ShopType.Trorc);
            ID = gun.PickupObjectId;
        }
        public static int ID;
    }
}