using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;
using SaveAPI;
using Alexandria.Misc;

namespace NevernamedsItems
{
    public class SnakePistol : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Snake Pistol", "snakepistol");
            Game.Items.Rename("outdated_gun_mods:snake_pistol", "nn:snake_pistol");
            var behav = gun.gameObject.AddComponent<SnakePistol>();

            gun.SetShortDescription("Hiss Bang");
            gun.SetLongDescription("This starkly cylindrical sidearm hails from a dimension of permanent combat."+"\n\nContains dehydrated snakes.");

            gun.SetGunSprites("snakepistol");

            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.SetAnimationFPS(gun.reloadAnimation, 0);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(56) as Gun, true, false);
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(150) as Gun).gunSwitchGroup;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(150) as Gun).muzzleFlashEffects;

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0.8f;
            gun.DefaultModule.cooldownTime = 0.1f;
            gun.DefaultModule.numberOfShotsInClip = 1;
            gun.barrelOffset.transform.localPosition = new Vector3(18f / 16f, 11f / 16f, 0f);
            gun.SetBaseMaxAmmo(100);
            gun.gunClass = GunClass.POISON;

            //BULLET STATS
            Projectile projectile = StandardisedProjectiles.snake.InstantiateAndFakeprefab();        
            gun.DefaultModule.projectiles[0] = projectile;

            gun.AddClipSprites("snakeclip");

            gun.quality = PickupObject.ItemQuality.D;
            ETGMod.Databases.Items.Add(gun, false, "ANY");

            ID = gun.PickupObjectId;

            AdvancedHoveringGunSynergyProcessor SerpentsReach = gun.gameObject.AddComponent<AdvancedHoveringGunSynergyProcessor>();
            SerpentsReach.RequiredSynergy = "Serpents Reach";
            SerpentsReach.requiresTargetGunInInventory = true;
            SerpentsReach.FireType = HoveringGunController.FireType.ON_COOLDOWN;
            SerpentsReach.Trigger = AdvancedHoveringGunSynergyProcessor.TriggerStyle.CONSTANT;
        }
        public static int ID;
    }
}
