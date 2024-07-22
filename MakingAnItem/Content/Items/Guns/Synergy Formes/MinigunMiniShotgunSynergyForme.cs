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

    public class MinigunMiniShotgunSynergyForme : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Mini Shotgun", "minishotgun2");
            Game.Items.Rename("outdated_gun_mods:mini_shotgun", "nn:mini_gun+mini_shotgun");
            gun.gameObject.AddComponent<MinigunMiniShotgunSynergyForme>();
            gun.SetShortDescription("Tiny Toys");
            gun.SetLongDescription("This shotgun is the size of my self confidence." );
          
            gun.SetGunSprites("minishotgun2", 8, true, 2);

            gun.SetAnimationFPS(gun.shootAnimation, 16);
            gun.SetAnimationFPS(gun.reloadAnimation, 16);
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.reloadAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(43) as Gun).gunSwitchGroup;

            for (int i = 0; i < 3; i++)
            {
                gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
            }

            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(79) as Gun).muzzleFlashEffects;
            gun.SetBarrel(13, 6);
            //GUN STATS
            foreach (ProjectileModule mod in gun.Volley.projectiles)
            {
                mod.ammoCost = 1;
                mod.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
                mod.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
                mod.cooldownTime = 0.4f;
                mod.angleVariance = 10f;
                mod.numberOfShotsInClip = 4;
                Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(mod.projectiles[0]);
                mod.projectiles[0] = projectile;
                projectile.gameObject.SetActive(false);
                FakePrefab.MarkAsFakePrefab(projectile.gameObject);
                UnityEngine.Object.DontDestroyOnLoad(projectile);
                projectile.baseData.damage = 7f;
                projectile.AdditionalScaleMultiplier *= 0.5f;
                if (mod != gun.DefaultModule) { mod.ammoCost = 0; }
            }
            gun.reloadTime = 1.1f;
            gun.SetBaseMaxAmmo(200);
            gun.quality = PickupObject.ItemQuality.EXCLUDED;
            ETGMod.Databases.Items.Add(gun, null, "ANY");

            gun.SetName("Mini Gun");

            gun.AddShellCasing(1, 6, 0, 0, "shell_tiny");

            gun.AddClipSprites("minishotgun");
            MiniShotgunID = gun.PickupObjectId;
        }
        public static int MiniShotgunID;
        public MinigunMiniShotgunSynergyForme()
        {

        }
    }
}