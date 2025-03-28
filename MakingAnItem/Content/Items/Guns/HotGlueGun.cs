﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.Assetbundle;

namespace NevernamedsItems
{
    public class HotGlueGun : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Hot Glue Gun", "hotgluegun");
            Game.Items.Rename("outdated_gun_mods:hot_glue_gun", "nn:hot_glue_gun");
            gun.gameObject.AddComponent<HotGlueGun>();
            gun.SetShortDescription("Ow!.. Heat was Hot!");
            gun.SetLongDescription("Glues your foes in place, and has a chance to set them ablaze."+"\n\nPioneered by amateur guncrafters, and generally considered one of the most painful methods of adhesive application.");

            Alexandria.Assetbundle.GunInt.SetupSprite(gun, Initialisation.gunCollection, "hotgluegun_idle_001", 8, "hotgluegun_ammonomicon_001");

            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(199) as Gun).gunSwitchGroup;

            gun.SetAnimationFPS(gun.shootAnimation, 12);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0.7f;
            gun.DefaultModule.cooldownTime = 0.4f;
            gun.DefaultModule.numberOfShotsInClip = 5;
            gun.barrelOffset.transform.localPosition = new Vector3(0.75f, 0.31f, 0f);
            gun.SetBaseMaxAmmo(150);
            gun.ammo = 150;
            gun.gunClass = GunClass.FIRE;
            //BULLET STATS
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            projectile.baseData.damage *= 1.6f;
            projectile.baseData.speed *= 0.5f;
            projectile.baseData.range *= 0.5f;

            ApplyLockdownBulletBehaviour applylockdown = projectile.gameObject.AddComponent<ApplyLockdownBulletBehaviour>();
            applylockdown.useSpecialBulletTint = false;
            applylockdown.duration = 5f;
            applylockdown.enemyTintColour = ExtendedColours.paleYellow;
            applylockdown.TintEnemy = true;
            applylockdown.TintCorpse = true;
            applylockdown.corpseTintColour = ExtendedColours.paleYellow;

            ExtremelySimpleStatusEffectBulletBehaviour burning = projectile.gameObject.AddComponent<ExtremelySimpleStatusEffectBulletBehaviour>();
            burning.onHitProcChance = 0.07f;
            burning.onFiredProcChance = 1f;
            burning.usesFireEffect = true;
            burning.fireEffect = StaticStatusEffects.hotLeadEffect;

            projectile.AnimateProjectileBundle("GlueGunProjectile",
                   Initialisation.ProjectileCollection,
                   Initialisation.projectileAnimationCollection,
                   "GlueGunProjectile",
                   new List<IntVector2> {
                        new IntVector2(5, 5), //1
                        new IntVector2(4, 6), //2            
                        new IntVector2(5, 5), //3
                        new IntVector2(6, 4), //4
                   }, //Pixel Sizes
                   MiscTools.DupeList(false, 4), //Lightened
                   MiscTools.DupeList(tk2dBaseSprite.Anchor.MiddleCenter, 4), //Anchors
                   MiscTools.DupeList(true, 4), //Anchors Change Colliders
                   MiscTools.DupeList(false, 4), //Fixes Scales
                   MiscTools.DupeList<Vector3?>(null, 4), //Manual Offsets
                   MiscTools.DupeList<IntVector2?>(null, 4), //Override colliders
                   MiscTools.DupeList<IntVector2?>(null, 4), //Override collider offsets
                   MiscTools.DupeList<Projectile>(null, 4)); // Override to copy from    
        
            gun.DefaultModule.projectiles[0] = projectile;
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = CustomClipAmmoTypeToolbox.AddCustomAmmoType("HotGlueGun Bullets", "NevernamedsItems/Resources/CustomGunAmmoTypes/hotgluegun_clipfull", "NevernamedsItems/Resources/CustomGunAmmoTypes/hotgluegun_clipempty");
            gun.quality = PickupObject.ItemQuality.B;
            ETGMod.Databases.Items.Add(gun, false, "ANY");
            HotGlueGunID = gun.PickupObjectId;

        }
        public static int HotGlueGunID;
        public override void PostProcessProjectile(Projectile projectile)
        {
            if (projectile.Owner is PlayerController)
            {
                PlayerController player = projectile.Owner as PlayerController;
                if (player.PlayerHasActiveSynergy("Heat Stress"))
                {
                    ExtremelySimpleStatusEffectBulletBehaviour burning = projectile.gameObject.GetComponent<ExtremelySimpleStatusEffectBulletBehaviour>();
                    if (burning != null)
                    {
                        burning.onHitProcChance *= 3f;
                    }
                }               
                if (player.PlayerHasActiveSynergy("Stick In The Mud"))
                {
                    ApplyLockdownBulletBehaviour lockingdown = projectile.gameObject.GetComponent<ApplyLockdownBulletBehaviour>();
                    if (lockingdown != null)
                    {
                        lockingdown.duration *= 2f;
                    }
                }
            }
        }
        public HotGlueGun()
        {

        }
    }
}
