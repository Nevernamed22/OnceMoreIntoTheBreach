using Alexandria.ItemAPI;
using Alexandria.Misc;
using Gungeon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class ServiceWeaponShatter : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Shatter", "serviceweaponshatter");
            Game.Items.Rename("outdated_gun_mods:shatter", "nn:service_weapon+shatter");
            gun.gameObject.AddComponent<ServiceWeaponShatter>();
            gun.SetShortDescription("");
            gun.SetLongDescription("");

            gun.SetGunSprites("serviceweaponshatter", 8, true);

            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.SetAnimationFPS(gun.reloadAnimation, 12);

            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(47) as Gun).gunSwitchGroup;

            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(ServiceWeapon.ID) as Gun).muzzleFlashEffects;

            gun.gunScreenShake= (PickupObjectDatabase.GetById(51) as Gun).gunScreenShake;

            for (int i = 0; i < 5; i++) { gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(56) as Gun, true, false); }

            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.reloadAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.reloadAnimation).loopStart = 3;

            gun.reloadTime = 1.8f;
            gun.barrelOffset.transform.localPosition = new Vector3(26f / 16f, 16f / 16f, 0f);
            gun.SetBaseMaxAmmo(6);
            gun.gunClass = GunClass.SHOTGUN;
            foreach (ProjectileModule mod in gun.Volley.projectiles)
            {
                //GUN STATS
                mod.ammoCost = 0;
                mod.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
                mod.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
                mod.cooldownTime = 0.7f;
                mod.angleVariance = 19;
                mod.numberOfShotsInClip = -1;

                //BULLET STATS
                Projectile projectile = mod.projectiles[0].InstantiateAndFakeprefab();
                mod.projectiles[0] = projectile;
                projectile.baseData.damage = 20f;
                projectile.baseData.speed *= 2;
                projectile.baseData.force *= 2;
                projectile.baseData.range = 8f;
                projectile.SetProjectileSprite("serviceweapon_proj", 11, 6, true, tk2dBaseSprite.Anchor.MiddleCenter, 10, 5);
                projectile.hitEffects.overrideMidairDeathVFX = (PickupObjectDatabase.GetById(98) as Gun).DefaultModule.projectiles[0].hitEffects.overrideMidairDeathVFX;
                projectile.hitEffects.alwaysUseMidair = true;

                mod.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
                mod.customAmmoType = "Service Weapon Bullets";
            }
            gun.Volley.UsesShotgunStyleVelocityRandomizer = true;
            gun.DefaultModule.ammoCost = 1;


            gun.quality = PickupObject.ItemQuality.EXCLUDED;
            ETGMod.Databases.Items.Add(gun, null, "ANY");

            ID = gun.PickupObjectId;
        }
        public static int ID;
    }
}
