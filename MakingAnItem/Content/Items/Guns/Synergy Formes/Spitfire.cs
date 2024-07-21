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
using Alexandria.BreakableAPI;
using Alexandria.Misc;

namespace NevernamedsItems
{
    public class Spitfire : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Spitballer", "spitfire");
            Game.Items.Rename("outdated_gun_mods:spitballer", "nn:spitballer+spitfire");
            gun.gameObject.AddComponent<Spitfire>();
            gun.SetShortDescription("");
            gun.SetLongDescription("");

            gun.SetGunSprites("spitfire", 8, true);

            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).frames[0].eventAudio = "spit_fire";
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).frames[0].triggerEvent = true;


            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(56) as Gun, true, false);
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(33) as Gun).muzzleFlashEffects;

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.0f;
            gun.DefaultModule.cooldownTime = 0.21f;
            gun.DefaultModule.numberOfShotsInClip = 10;
            gun.barrelOffset.transform.localPosition = new Vector3(28f / 16f, 14f/16f, 0f);
            gun.SetBaseMaxAmmo(2000);
            gun.ammo = 2000;
            gun.gunClass = GunClass.FULLAUTO;

            //BULLET STATS
            Projectile projectile = gun.DefaultModule.projectiles[0].gameObject.InstantiateAndFakeprefab().GetComponent<Projectile>();
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.gameObject.name = "spitfire_proj";
            projectile.baseData.damage = 8f;
           
            projectile.SetProjectileSprite("spitfire_proj", 10, 2, false, tk2dBaseSprite.Anchor.MiddleCenter, 10, 2);

            BulletSharkProjectileDoer sharker = projectile.gameObject.AddComponent<BulletSharkProjectileDoer>();

            Projectile spitball = ProjectileUtility.SetupProjectile(86);
            spitball.baseData.damage = 5.1f;
            spitball.baseData.speed *= 0.8f;
            spitball.hitEffects.overrideMidairDeathVFX = VFXToolbox.CreateVFXBundle("SpitballerImpact", new IntVector2(10, 8), tk2dBaseSprite.Anchor.MiddleCenter, true, 0.2f);
            spitball.hitEffects.alwaysUseMidair = true;
            spitball.SetProjectileSprite("spitballer_proj", 4, 4, false, tk2dBaseSprite.Anchor.MiddleCenter, 4, 4);
            GoopModifier goop = spitball.gameObject.AddComponent<GoopModifier>();
            goop.SpawnGoopOnCollision = true;
            goop.CollisionSpawnRadius = 0.5f;
            goop.goopDefinition = EasyGoopDefinitions.WaterGoop;

            sharker.toSpawn = spitball;

            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = "white";
            gun.quality = PickupObject.ItemQuality.EXCLUDED;
            ETGMod.Databases.Items.Add(gun, false, "ANY");
            gun.SetName("Spitballer");

            ID = gun.PickupObjectId;
        }
        public static int ID;
    }
}
