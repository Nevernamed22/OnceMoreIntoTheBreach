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
using Alexandria.Misc;
using Alexandria.Assetbundle;

namespace NevernamedsItems
{
    public class AgarGun : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Agar Gun", "agargun");
            Game.Items.Rename("outdated_gun_mods:agar_gun", "nn:agar_gun");
            gun.gameObject.AddComponent<AgarGun>();
            gun.SetShortDescription("Feeda Me");
            gun.SetLongDescription("An old historical remnant reinvigorated for bio warfare."+"\n\nThe final shot launches a hungry macrobacteria, who gobbles up agar to fuel its own destructive power.");

            gun.SetGunSprites("agargun");

            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.SetAnimationFPS(gun.reloadAnimation, 0);
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(33) as Gun).gunSwitchGroup;
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);

            //GUN STATS
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(33) as Gun).muzzleFlashEffects;
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.15f;
            gun.DefaultModule.cooldownTime = 0.15f;
            gun.DefaultModule.numberOfShotsInClip = 15;
            gun.barrelOffset.transform.localPosition = new Vector3(48f/16f, 8f / 16, 0f);
            gun.SetBaseMaxAmmo(470);
            gun.ammo = 470;
            gun.gunClass = GunClass.FULLAUTO;

            //BULLET STATS
            #region MainBullet
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage = 5f;
            BounceProjModifier bounce = projectile.gameObject.GetOrAddComponent<BounceProjModifier>();
            bounce.numberOfBounces = 2;
            projectile.gameObject.AddComponent<AgarGunSmallProj>();

            projectile.hitEffects.overrideMidairDeathVFX = (PickupObjectDatabase.GetById(33) as Gun).DefaultModule.projectiles[0].hitEffects.overrideMidairDeathVFX;
            projectile.hitEffects.alwaysUseMidair = true;

            //ANIMATE BULLET
            projectile.AnimateProjectileBundle("AgarGunProjectile",
                    Initialisation.ProjectileCollection,
                    Initialisation.projectileAnimationCollection,
                    "AgarGunProjectile",
                    MiscTools.DupeList(new IntVector2(12, 11), 4), //Pixel Sizes
                    MiscTools.DupeList(true, 4), //Lightened
                    MiscTools.DupeList(tk2dBaseSprite.Anchor.MiddleCenter, 4), //Anchors
                    MiscTools.DupeList(true, 4), //Anchors Change Colliders
                    MiscTools.DupeList(false, 4), //Fixes Scales
                    MiscTools.DupeList<Vector3?>(null, 4), //Manual Offsets
                    MiscTools.DupeList<IntVector2?>(null, 4), //Override colliders
                    MiscTools.DupeList<IntVector2?>(null, 4), //Override collider offsets
                    MiscTools.DupeList<Projectile>(null, 4)); // Override to copy from                   
            #endregion
            #region SubProjectile
            Projectile subProj = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(56) as Gun).DefaultModule.projectiles[0]);
            subProj.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(subProj.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(subProj);
            subProj.baseData.damage = 10f;
            subProj.baseData.speed *= 0.7f;
            subProj.baseData.range *= 2f;
            subProj.gameObject.AddComponent<PierceProjModifier>();
            subProj.hitEffects.overrideMidairDeathVFX = (PickupObjectDatabase.GetById(755) as Gun).DefaultModule.projectiles[0].hitEffects.overrideMidairDeathVFX;
            subProj.hitEffects.alwaysUseMidair = true;

            subProj.gameObject.AddComponent<AgarGunBigProj>();

            //ANIMATE BULLET
            subProj.AnimateProjectileBundle("AgarGunAgarProjectile",
                    Initialisation.ProjectileCollection,
                    Initialisation.projectileAnimationCollection,
                    "AgarGunAgarProjectile",
                    MiscTools.DupeList(new IntVector2(17, 12), 4), //Pixel Sizes
                    MiscTools.DupeList(true, 4), //Lightened
                    MiscTools.DupeList(tk2dBaseSprite.Anchor.MiddleCenter, 4), //Anchors
                    MiscTools.DupeList(true, 4), //Anchors Change Colliders
                    MiscTools.DupeList(false, 4), //Fixes Scales
                    MiscTools.DupeList<Vector3?>(null, 4), //Manual Offsets
                    MiscTools.DupeList<IntVector2?>(null, 4), //Override colliders
                    MiscTools.DupeList<IntVector2?>(null, 4), //Override collider offsets
                    MiscTools.DupeList<Projectile>(null, 4)); // Override to copy from             
            #endregion

            gun.DefaultModule.finalProjectile = subProj;
            gun.DefaultModule.usesOptionalFinalProjectile = true;
            gun.DefaultModule.numberOfFinalProjectiles = 1;

            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = CustomClipAmmoTypeToolbox.AddCustomAmmoType("Agar Gun Ammo", "NevernamedsItems/Resources/CustomGunAmmoTypes/agargun_clipfull", "NevernamedsItems/Resources/CustomGunAmmoTypes/agargun_clipempty");
            gun.DefaultModule.finalAmmoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.finalCustomAmmoType = CustomClipAmmoTypeToolbox.AddCustomAmmoType("Agar Gun Glob", "NevernamedsItems/Resources/CustomGunAmmoTypes/agargun_final_clipfull", "NevernamedsItems/Resources/CustomGunAmmoTypes/agargun_final_clipempty");

            gun.quality = PickupObject.ItemQuality.C;
            ETGMod.Databases.Items.Add(gun, null, "ANY");

            gun.AddToSubShop(ItemBuilder.ShopType.Goopton);
            ID = gun.PickupObjectId;
        }
        public static int ID;
    }
    public class AgarGunSmallProj : MonoBehaviour
    {
        private Projectile self;
        private PlayerController owner;
        private float initialSpeed;
        private void Start()
        {
            this.self = base.GetComponent<Projectile>();
            owner = self.ProjectilePlayerOwner();
            initialSpeed = self.baseData.speed;

            StartCoroutine(DoSpeedChange());
        }
        private IEnumerator DoSpeedChange()
        {
            float realTime = 1f;
            realTime *= UnityEngine.Random.Range(0.5f, 1f);
            realTime *= self.ProjectilePlayerOwner().stats.GetStatValue(PlayerStats.StatType.RangeMultiplier);

            float elapsed = 0f;
            while (elapsed < realTime)
            {
                elapsed += BraveTime.DeltaTime;
                float t = Mathf.Clamp01(elapsed / realTime);
                float speedMod = Mathf.Lerp(initialSpeed, 0, t);

                self.baseData.speed = speedMod;
                self.UpdateSpeed();

                if (!self) break;
                yield return null;
            }
            BulletLifeTimer timer = self.gameObject.AddComponent<BulletLifeTimer>();
            timer.secondsTillDeath = 7f;
        }
    }
    public class AgarGunBigProj : MonoBehaviour
    {
        private Projectile m_projectile;
        private void Start()
        {
            m_projectile = base.GetComponent<Projectile>();
        }
        private void Update()
        {
            Vector2 vector = this.m_projectile.transform.position.XY();
            for (int i = 0; i < StaticReferenceManager.AllProjectiles.Count; i++)
            {
                Projectile projectile = StaticReferenceManager.AllProjectiles[i];
                if (projectile && projectile.Owner == m_projectile.Owner && projectile.GetComponent<AgarGunSmallProj>())
                {
                    float sqrMagnitude = (projectile.transform.position.XY() - vector).sqrMagnitude;
                    if (sqrMagnitude < 2)
                    {
                        this.AbsorbBullet(projectile);
                    }
                }
            }
        }
        private void AbsorbBullet(Projectile target)
        {
            m_projectile.baseData.damage *= 1.1f;
            m_projectile.RuntimeUpdateScale(1.1f);
            target.DieInAir();
        }
    }
}
