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
    public class RepeatovolverInfinite : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Repeatovolver Infinite", "repeatovolverinf");
            Game.Items.Rename("outdated_gun_mods:repeatovolver_infinite", "nn:repeatovolver+ad_infinitum");
            gun.gameObject.AddComponent<DiamondGaxe>();
            gun.SetShortDescription("");
            gun.SetLongDescription("");

            gun.SetGunSprites("repeatovolverinf", 8, true, 2);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(56) as Gun, true, false);

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Burst;
            gun.DefaultModule.burstShotCount = 15;
            gun.DefaultModule.burstCooldownTime = 0.04f;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1f;
            gun.DefaultModule.cooldownTime = 1f;
            gun.DefaultModule.numberOfShotsInClip = 15;
            gun.SetBarrel(20, 9);

            gun.SetBaseMaxAmmo(1000);
            gun.InfiniteAmmo = true;
            gun.gunClass = GunClass.FULLAUTO;
            gun.gunHandedness = GunHandedness.OneHanded;

            //BULLET STATS
            Projectile projectile = ProjectileSetupUtility.MakeProjectile(56, 3);
            gun.DefaultModule.projectiles[0] = projectile;

            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(62) as Gun).muzzleFlashEffects;

            projectile.baseData.range *= 2f;
            projectile.transform.parent = gun.barrelOffset;
            projectile.SetProjectileSpriteRight("repeating_projectile", 9, 6, false, tk2dBaseSprite.Anchor.MiddleCenter, 9, 6);

            gun.AddShellCasing(1, 0, 0, 0);

            gun.quality = PickupObject.ItemQuality.EXCLUDED;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            gun.SetName("Repeatovolver");

            ID = gun.PickupObjectId;
        }
        public static int ID;
    }
}