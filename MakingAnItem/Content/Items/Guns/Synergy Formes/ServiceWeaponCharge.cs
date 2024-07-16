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

    public class ServiceWeaponCharge : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Charge", "serviceweaponcharge");
            Game.Items.Rename("outdated_gun_mods:charge", "nn:service_weapon+charge");
            gun.gameObject.AddComponent<ServiceWeaponCharge>();
            gun.SetShortDescription("");
            gun.SetLongDescription("");

            gun.SetGunSprites("serviceweaponcharge", 8, true);

            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.SetAnimationFPS(gun.reloadAnimation, 12);
            gun.SetAnimationFPS(gun.chargeAnimation, 4);

            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(124) as Gun).gunSwitchGroup;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(362) as Gun).muzzleFlashEffects;

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(56) as Gun, true, false);

            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.reloadAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.reloadAnimation).loopStart = 2;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.chargeAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.chargeAnimation).loopStart = 10;

            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).frames[0].eventAudio = "Play_wpn_chargelaser_shot_01";
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).frames[0].triggerEvent = true;

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Charged;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1f;
            gun.DefaultModule.cooldownTime = 0.1f;
            gun.DefaultModule.numberOfShotsInClip = -1;
            gun.DefaultModule.angleVariance = 0;
            gun.barrelOffset.transform.localPosition = new Vector3(33f / 16f, 16f / 16f, 0f);
            gun.SetBaseMaxAmmo(3);
            gun.gunClass = GunClass.CHARGE;

            //BULLET STATS
            Projectile projectile = (PickupObjectDatabase.GetById(56) as Gun).DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage = 10;
            projectile.baseData.speed *= 1.2f;
            projectile.SetProjectileSprite("serviceweapon_proj", 11, 6, true, tk2dBaseSprite.Anchor.MiddleCenter, 10, 5);
            projectile.hitEffects.overrideMidairDeathVFX = (PickupObjectDatabase.GetById(178) as Gun).GetComponent<FireOnReloadSynergyProcessor>().DirectedBurstSettings.ProjectileInterface.SpecifiedProjectile.hitEffects.tileMapHorizontal.effects[0].effects[0].effect;
            projectile.hitEffects.alwaysUseMidair = true;
            ImplosionBehaviour implosion = projectile.gameObject.AddComponent<ImplosionBehaviour>();
            implosion.waitTime = 0.2f;
            implosion.Suck = true;
            implosion.explosionData = new ExplosionData()
            {
                effect = StaticExplosionDatas.explosiveRoundsExplosion.effect,
                ignoreList = StaticExplosionDatas.explosiveRoundsExplosion.ignoreList,
                ss = StaticExplosionDatas.explosiveRoundsExplosion.ss,
                damageRadius = 5f,
                damageToPlayer = 0f,
                doDamage = true,
                damage = 45,
                doDestroyProjectiles = false,
                doForce = true,
                debrisForce = 30f,
                preventPlayerForce = true,
                explosionDelay = 0.1f,
                usesComprehensiveDelay = false,
                doScreenShake = true,
                playDefaultSFX = true,
            };

            Projectile projectile2 = (PickupObjectDatabase.GetById(56) as Gun).DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            projectile2.baseData.damage = 10;
            projectile2.baseData.speed *= 1.4f;
            projectile2.SetProjectileSprite("serviceweapon_proj", 11, 6, true, tk2dBaseSprite.Anchor.MiddleCenter, 10, 5);
            projectile2.AdditionalScaleMultiplier = 1.1f;
            projectile2.hitEffects.overrideMidairDeathVFX = (PickupObjectDatabase.GetById(178) as Gun).GetComponent<FireOnReloadSynergyProcessor>().DirectedBurstSettings.ProjectileInterface.SpecifiedProjectile.hitEffects.tileMapHorizontal.effects[0].effects[0].effect;
            projectile2.hitEffects.alwaysUseMidair = true;
            projectile2.gameObject.AddComponent<DrainClipBehav>().shotsToDrain = 1;

            ImplosionBehaviour implosion2 = projectile2.gameObject.AddComponent<ImplosionBehaviour>();
            implosion2.waitTime = 0.6f;
            implosion2.Suck = true;
            implosion2.explosionData = new ExplosionData()
            {
                effect = StaticExplosionDatas.genericLargeExplosion.effect,
                ignoreList = StaticExplosionDatas.genericLargeExplosion.ignoreList,
                ss = StaticExplosionDatas.genericLargeExplosion.ss,
                damageRadius = 5f,
                damageToPlayer = 0f,
                doDamage = true,
                damage = 90,
                doDestroyProjectiles = false,
                doForce = true,
                debrisForce = 30f,
                preventPlayerForce = true,
                explosionDelay = 0.1f,
                usesComprehensiveDelay = false,
                doScreenShake = true,
                playDefaultSFX = true,
            };

            Projectile projectile3 = (PickupObjectDatabase.GetById(56) as Gun).DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            projectile3.baseData.damage = 10;
            projectile3.baseData.speed *= 1.6f;
                            projectile3.SetProjectileSprite("serviceweapon_proj", 11, 6, true, tk2dBaseSprite.Anchor.MiddleCenter, 10, 5);
            projectile3.AdditionalScaleMultiplier = 1.2f;
            projectile3.hitEffects.overrideMidairDeathVFX = (PickupObjectDatabase.GetById(178) as Gun).GetComponent<FireOnReloadSynergyProcessor>().DirectedBurstSettings.ProjectileInterface.SpecifiedProjectile.hitEffects.tileMapHorizontal.effects[0].effects[0].effect;
            projectile3.hitEffects.alwaysUseMidair = true;

            projectile3.gameObject.AddComponent<DrainClipBehav>().shotsToDrain = 2;

            ImplosionBehaviour implosion3 = projectile3.gameObject.AddComponent<ImplosionBehaviour>();
            implosion3.waitTime = 1f;
            implosion3.Suck = true;
            implosion3.vfx = EasyVFXDatabase.HighPriestImplosionRing;
            implosion3.explosionData = new ExplosionData()
            {
                effect = StaticExplosionDatas.genericLargeExplosion.effect,
                ignoreList = StaticExplosionDatas.genericLargeExplosion.ignoreList,
                ss = StaticExplosionDatas.genericLargeExplosion.ss,
                damageRadius = 5f,
                damageToPlayer = 0f,
                doDamage = true,
                damage = 135,
                doDestroyProjectiles = true,
                doForce = true,
                debrisForce = 30f,
                preventPlayerForce = true,
                explosionDelay = 0.1f,
                usesComprehensiveDelay = false,
                doScreenShake = true,
                playDefaultSFX = true,
            };

        ProjectileModule.ChargeProjectile chargeProj = new ProjectileModule.ChargeProjectile
            {
                Projectile = projectile,
                ChargeTime = 0.8f,
            };
            ProjectileModule.ChargeProjectile chargeProj2 = new ProjectileModule.ChargeProjectile
            {
                Projectile = projectile2,
                ChargeTime = 1.6f,
                //AmmoCost = 2,
            };
            ProjectileModule.ChargeProjectile chargeProj3 = new ProjectileModule.ChargeProjectile
            {
                Projectile = projectile3,
                ChargeTime = 2.4f,
               // AmmoCost = 3,
            };
            gun.DefaultModule.chargeProjectiles = new List<ProjectileModule.ChargeProjectile> { chargeProj, chargeProj2, chargeProj3 };

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