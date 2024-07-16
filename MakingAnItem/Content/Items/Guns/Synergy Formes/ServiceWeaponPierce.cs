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
using Alexandria.Misc;
using Alexandria.VisualAPI;

namespace NevernamedsItems
{

    public class ServiceWeaponPierce : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Pierce", "serviceweaponpierce");
            Game.Items.Rename("outdated_gun_mods:pierce", "nn:service_weapon+pierce");
            gun.gameObject.AddComponent<ServiceWeaponPierce>();
            gun.SetShortDescription("");
            gun.SetLongDescription("");

            gun.SetGunSprites("serviceweaponpierce", 8, true);

            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.SetAnimationFPS(gun.reloadAnimation, 12);
            gun.SetAnimationFPS(gun.chargeAnimation, 7);

            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(124) as Gun).gunSwitchGroup;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(362) as Gun).muzzleFlashEffects;

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(56) as Gun, true, false);

            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.reloadAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.reloadAnimation).loopStart = 3;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.chargeAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.chargeAnimation).loopStart = 7;

            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).frames[0].eventAudio = "Play_wpn_chargelaser_shot_01";
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).frames[0].triggerEvent = true;

            //GUN STATS
            gun.usesContinuousFireAnimation = true;
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Charged;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1f;
            gun.DefaultModule.cooldownTime = 0.1f;
            gun.DefaultModule.numberOfShotsInClip = -1;
            gun.DefaultModule.angleVariance = 0;
            gun.barrelOffset.transform.localPosition = new Vector3(33f / 16f, 16f / 16f, 0f);
            gun.SetBaseMaxAmmo(2);
            gun.gunClass = GunClass.CHARGE;

            //BULLET STATS
            Projectile projectile = gun.DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage = 70;
            projectile.baseData.speed *= 4;
                projectile.SetProjectileSprite("serviceweaponpierce_proj", 24, 13, true, tk2dBaseSprite.Anchor.MiddleCenter, 20, 10);
            projectile.hitEffects = (PickupObjectDatabase.GetById(328) as Gun).DefaultModule.chargeProjectiles[0].Projectile.hitEffects;

            ImprovedAfterImage afterImage = projectile.gameObject.AddComponent<ImprovedAfterImage>();
            afterImage.spawnShadows = true;
            afterImage.shadowLifetime = (UnityEngine.Random.Range(0.1f, 0.2f));
            afterImage.shadowTimeDelay = 0.001f;
            afterImage.dashColor = new Color(1, 0.8f, 0.55f, 0.3f);
            afterImage.name = "Gun Trail";

            PierceProjModifier piercing = projectile.gameObject.AddComponent<PierceProjModifier>();
            piercing.penetration = 1;

            projectile.PenetratesInternalWalls = true;

            //projectile.gameObject.AddComponent<DestroyInternalWallsBehav>();

            ProjectileModule.ChargeProjectile chargeProj = new ProjectileModule.ChargeProjectile
            {
                Projectile = projectile,
                ChargeTime = 1f,
            };
            gun.DefaultModule.chargeProjectiles = new List<ProjectileModule.ChargeProjectile> { chargeProj };

            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = "Service Weapon Bullets";

            gun.TrimGunSprites();

            gun.quality = PickupObject.ItemQuality.EXCLUDED;
            ETGMod.Databases.Items.Add(gun, null, "ANY");

            ID = gun.PickupObjectId;
        }
        public static int ID;
    }
}