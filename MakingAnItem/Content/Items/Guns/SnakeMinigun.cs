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
    public class SnakeMinigun : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Snake Minigun", "snakeminigun");
            Game.Items.Rename("outdated_gun_mods:snake_minigun", "nn:snake_minigun");
            gun.gameObject.AddComponent<SnakeMinigun>();
            gun.SetShortDescription("SSSSSSSSSSSSSSSSSS");
            gun.SetLongDescription("Among the finest examples of the respected, albeit niche, tradition of snakesmithing."+"\n\nContains more snakes by volume than any other device of its kind.");

            gun.SetGunSprites("snakeminigun", 8, false, 2);

            gun.SetAnimationFPS(gun.shootAnimation, 15);
            gun.SetAnimationFPS(gun.reloadAnimation, 15);
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.reloadAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(84) as Gun, true, false);
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(84) as Gun).gunSwitchGroup;

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1f;
            gun.DefaultModule.cooldownTime = 0.06f;
            gun.DefaultModule.numberOfShotsInClip = 15;
            gun.SetBarrel(43, 8);
            gun.SetBaseMaxAmmo(800);
            gun.gunClass = GunClass.POISON;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(89) as Gun).muzzleFlashEffects;

            //BULLET STATS
            Projectile projectile = StandardisedProjectiles.snake.InstantiateAndFakeprefab();
            projectile.baseData.damage = 4;
            gun.DefaultModule.projectiles[0] = projectile;

            //gun.AddShellCasing(1, 0, 0, 0, "shell_betsy");
            //gun.AddClipDebris(0, 1, "clipdebris_betsy");
            gun.AddClipSprites("snakeclip");

            gun.quality = PickupObject.ItemQuality.S;

            ETGMod.Databases.Items.Add(gun, null, "ANY");
            ID = gun.PickupObjectId;
        }
        public static int ID;
    }
}
