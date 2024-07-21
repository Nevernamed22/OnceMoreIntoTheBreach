using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using MonoMod;
using Alexandria.ItemAPI;
using UnityEngine;
using System.Reflection;

namespace NevernamedsItems
{

    public class DiscGunSuperDiscForme : GunBehaviour
    {
        public static int DiscGunSuperDiscSynergyFormeID;

        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Super Disc Gun", "discgunsuper");
            Game.Items.Rename("outdated_gun_mods:super_disc_gun", "nn:disc_gun+super_disc");
            gun.gameObject.AddComponent<DiscGunSuperDiscForme>();
            gun.SetShortDescription("Badder Choices");
            gun.SetLongDescription("Fires a shit-ton of discs. If you're reading this, you're a hacker.");

            gun.SetGunSprites("discgunsuper", 8, true);

            gun.SetAnimationFPS(gun.shootAnimation, 14);
            gun.SetAnimationFPS(gun.reloadAnimation, 14);
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.reloadAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;

            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(393) as Gun).muzzleFlashEffects;
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(12) as Gun).gunSwitchGroup;
            gun.SetBarrel(30, 17);

            for (int i = 0; i < 5; i++)
            {
                gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
            }

            foreach (ProjectileModule mod in gun.Volley.projectiles)
            {
                mod.ammoCost = 1;
                mod.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
                mod.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
                mod.cooldownTime = 0.25f;
                mod.numberOfShotsInClip = 10;
                mod.angleVariance = 20f;

                Projectile projectile = ProjectileSetupUtility.MakeProjectile(DiscGun.DiscGunID, 20f);
                mod.projectiles[0] = projectile;
                
                if (mod != gun.DefaultModule) { mod.ammoCost = 0; }
            }

            gun.reloadTime = 1f;
            gun.SetBaseMaxAmmo(300);
            gun.AddClipSprites("discgun");

            gun.quality = PickupObject.ItemQuality.EXCLUDED;

            ETGMod.Databases.Items.Add(gun, null, "ANY");
            gun.SetName("Disc Gun");

            DiscGunSuperDiscSynergyFormeID = gun.PickupObjectId;
        }
    }
}