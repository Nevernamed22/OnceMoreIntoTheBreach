﻿using System;
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
    public class FlipFlop : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Flipflop", "flipflop");
            Game.Items.Rename("outdated_gun_mods:flipflop", "nn:flipflop");
            gun.gameObject.AddComponent<FlipFlop>();
            gun.SetShortDescription("");
            gun.SetLongDescription("");

            gun.SetupSprite(null, "flipflop_idle_001", 8);

            gun.SetAnimationFPS(gun.shootAnimation, 12);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(56) as Gun, true, false);

            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(5) as Gun).muzzleFlashEffects;
            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Burst;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.2f;
            gun.DefaultModule.burstShotCount = 4;
            gun.DefaultModule.burstCooldownTime = 0.1f;
            gun.DefaultModule.cooldownTime = 0.25f;
            gun.DefaultModule.numberOfShotsInClip = 8;
            gun.DefaultModule.angleVariance = 3.5f;
            gun.barrelOffset.transform.localPosition = new Vector3(2.68f, 0.87f, 0f);
            gun.SetBaseMaxAmmo(240);
            gun.ammo = 240;
            gun.gunClass = GunClass.RIFLE;

            //BULLET STATS
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage = 10f;
            projectile.baseData.speed *= 3f;
            projectile.baseData.range *= 10f;

            PierceDeadActors piercedead = projectile.gameObject.AddComponent<PierceDeadActors>();

            projectile.SetProjectileSprite("burstrifle_proj", 7, 4, true, tk2dBaseSprite.Anchor.MiddleCenter, 7, 4);
            EasyTrailBullet trail4 = projectile.gameObject.AddComponent<EasyTrailBullet>();
            trail4.TrailPos = projectile.transform.position;
            trail4.StartWidth = 0.25f;
            trail4.EndWidth = 0f;
            trail4.LifeTime = 0.1f;
            trail4.BaseColor = ExtendedColours.honeyYellow;
            trail4.EndColor = ExtendedColours.honeyYellow;

            projectile.hitEffects.alwaysUseMidair = true;
            projectile.hitEffects.overrideMidairDeathVFX = (PickupObjectDatabase.GetById(647) as Gun).DefaultModule.projectiles[0].hitEffects.tileMapVertical.effects[0].effects[0].effect;
            projectile.transform.parent = gun.barrelOffset;

            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = "Rifle";

            gun.quality = PickupObject.ItemQuality.A;
            ETGMod.Databases.Items.Add(gun, null, "ANY");

            gun.AddToSubShop(ItemBuilder.ShopType.Trorc);
            ID = gun.PickupObjectId;
        }
        public static int ID;
    }
}
