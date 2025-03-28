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
    public class TheGreyStaff : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("The Grey Staff", "thegreystaff");
            Game.Items.Rename("outdated_gun_mods:the_grey_staff", "nn:the_grey_staff");
            gun.gameObject.AddComponent<TheGreyStaff>();
            gun.SetShortDescription("You Shall Not Pass");
            gun.SetLongDescription("An ancient and powerful gun barrel from an age before the Gungeon."+"\n\nThough studied by Alben Smallbore, it is not of his craft.");

            gun.SetGunSprites("thegreystaff");

            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.SetAnimationFPS(gun.reloadAnimation, 12);

            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(145) as Gun).gunSwitchGroup;
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(56) as Gun, true, false);

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.3f;
            gun.DefaultModule.cooldownTime = 0.1f;
            gun.DefaultModule.angleVariance = 20;
            gun.DefaultModule.numberOfShotsInClip = 30;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(97) as Gun).muzzleFlashEffects;
            gun.barrelOffset.transform.localPosition = new Vector3(40f / 16f, 10f / 16f, 0f);
            gun.SetBaseMaxAmmo(777);
            gun.ammo = 777;
            gun.gunClass = GunClass.FULLAUTO;

            //BULLET STATS
            Projectile projectile = gun.DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            projectile.baseData.damage = 5.5f;
            projectile.baseData.speed = 11f;
            projectile.SetProjectileSprite("16x16_white_circle", 6, 6, true, tk2dBaseSprite.Anchor.MiddleCenter, 6, 6);
            projectile.hitEffects.overrideMidairDeathVFX = SharedVFX.WhiteCircleVFX;
            projectile.hitEffects.alwaysUseMidair = true;

            BounceProjModifier ricochet = projectile.gameObject.AddComponent<BounceProjModifier>();
            ricochet.numberOfBounces = 2;
            RemoteBulletsProjectileBehaviour remo = projectile.gameObject.AddComponent<RemoteBulletsProjectileBehaviour>();
            gun.DefaultModule.projectiles[0] = projectile;

            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = "white";           

            gun.quality = PickupObject.ItemQuality.A;
            ETGMod.Databases.Items.Add(gun, false, "ANY");

            ID = gun.PickupObjectId;
        }
        public static int ID;     
    }
}
