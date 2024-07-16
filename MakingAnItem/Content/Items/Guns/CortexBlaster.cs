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
using Dungeonator;

namespace NevernamedsItems
{
    public class CortexBlaster : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Cortex Blaster", "cortexblaster");
            Game.Items.Rename("outdated_gun_mods:cortex_blaster", "nn:cortex_blaster");
            var behav = gun.gameObject.AddComponent<CortexBlaster>();
            gun.SetShortDescription("Crash Bang-De-Shoot");
            gun.SetLongDescription("The signature weapon of a mad, sad scientist. It's inner workings are complete nonsense to everyone save it's creator."+"\n\nCharges up to fire swirling charges of plasma.");

            Alexandria.Assetbundle.GunInt.SetupSprite(gun, Initialisation.gunCollection, "cortexblaster_idle_001", 8, "cortexblaster_ammonomicon_001");

            gun.SetAnimationFPS(gun.shootAnimation, 15);
            gun.SetAnimationFPS(gun.chargeAnimation, 10);
            gun.SetAnimationFPS(gun.reloadAnimation, 15);

            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(13) as Gun).gunSwitchGroup;
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(56) as Gun, true, false);

            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.chargeAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.chargeAnimation).loopStart = 3;

            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.reloadAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.reloadAnimation).loopStart = 2;

            //GUN STATS
            gun.gunScreenShake = (PickupObjectDatabase.GetById(23) as Gun).gunScreenShake;
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Charged;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.DefaultModule.cooldownTime = 0.2f;
            gun.DefaultModule.numberOfShotsInClip = 10;
            gun.DefaultModule.angleVariance = 0;

            gun.reloadTime = 1f;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(334) as Gun).muzzleFlashEffects;
            gun.barrelOffset.transform.localPosition = new Vector3(31f / 16f, 13f / 16f, 0f);
            gun.gunClass = GunClass.CHARGE;
            gun.SetBaseMaxAmmo(220);
            gun.ammo = 220;

            //BULLET STATS
            Projectile projectile = gun.DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            projectile.SetProjectileSprite("cortexblaster_proj", 17, 7, false, tk2dBaseSprite.Anchor.MiddleCenter, 11, 3);
            projectile.hitEffects.alwaysUseMidair = true;
            projectile.baseData.damage = 10;
            projectile.baseData.force *= 1.2f;
            projectile.baseData.speed *= 0.9f;
            projectile.hitEffects.overrideMidairDeathVFX = EasyVFXDatabase.PurpleLaserCircleVFX;

            List<string> BeamAnimPaths = new List<string>()
            {
                "NevernamedsItems/Resources/TrailSprites/cortexblaster_trail_001",
                "NevernamedsItems/Resources/TrailSprites/cortexblaster_trail_002",
                "NevernamedsItems/Resources/TrailSprites/cortexblaster_trail_003",
            };

            //Wigglers
            ImprovedHelixProjectile leftWiggle = DataCloners.CopyFields<ImprovedHelixProjectile>(Instantiate((PickupObjectDatabase.GetById(56) as Gun).DefaultModule.projectiles[0]));
            leftWiggle.SpawnShadowBulletsOnSpawn = false;
            leftWiggle.gameObject.MakeFakePrefab();
            leftWiggle.SetProjectileSprite("cortexblaster_proj", 17, 7, false, tk2dBaseSprite.Anchor.MiddleCenter, 11, 3);
            leftWiggle.hitEffects.alwaysUseMidair = true;
            leftWiggle.hitEffects.overrideMidairDeathVFX = EasyVFXDatabase.PurpleLaserCircleVFX;
            leftWiggle.baseData.damage = 11;
            leftWiggle.baseData.force *= 1.2f;
            leftWiggle.pierceMinorBreakables = true;
            leftWiggle.baseData.speed *= 0.9f;
            leftWiggle.gameObject.AddComponent<PierceProjModifier>();
            leftWiggle.AddTrailToProjectile(
                "NevernamedsItems/Resources/TrailSprites/cortexblaster_trail_001",
                new Vector2(6, 4),
                new Vector2(0, 1),
                BeamAnimPaths, 20,
                BeamAnimPaths, 20,
                -1,
                0.0001f,
                -1,
                true
                );
            leftWiggle.gameObject.GetOrAddComponent<EmmisiveTrail>();
            StatusEffectBulletSynergy synergyFreezeLeft = leftWiggle.gameObject.GetOrAddComponent<StatusEffectBulletSynergy>();
            synergyFreezeLeft.tint = ExtendedColours.freezeBlue;
            synergyFreezeLeft.StatusEffects.AddRange(new List<GameActorEffect>() { StaticStatusEffects.frostBulletsEffect });
            synergyFreezeLeft.synergyToCheckFor = "It's About Rime";

            ImprovedHelixProjectile rightWiggle = DataCloners.CopyFields<ImprovedHelixProjectile>(Instantiate((PickupObjectDatabase.GetById(56) as Gun).DefaultModule.projectiles[0]));
            rightWiggle.SpawnShadowBulletsOnSpawn = false;
            rightWiggle.gameObject.MakeFakePrefab();
            rightWiggle.SetProjectileSprite("cortexblaster_proj", 17, 7, false, tk2dBaseSprite.Anchor.MiddleCenter, 11, 3);
            rightWiggle.hitEffects.alwaysUseMidair = true;
            rightWiggle.hitEffects.overrideMidairDeathVFX = EasyVFXDatabase.PurpleLaserCircleVFX;
            rightWiggle.startInverted = true;
            rightWiggle.baseData.damage = 11;
            rightWiggle.baseData.force *= 1.2f;
            rightWiggle.baseData.speed *= 0.9f;
            rightWiggle.pierceMinorBreakables = true;
            rightWiggle.gameObject.AddComponent<PierceProjModifier>();
            rightWiggle.AddTrailToProjectile(
                "NevernamedsItems/Resources/TrailSprites/cortexblaster_trail_001",
                new Vector2(6, 4),
                new Vector2(0, 1),
                BeamAnimPaths, 20,
                BeamAnimPaths, 20,
                -1,
                0.0001f,
                -1,
                true
                );
            rightWiggle.gameObject.GetOrAddComponent<EmmisiveTrail>();
            StatusEffectBulletSynergy synergyFreezeRight = rightWiggle.gameObject.GetOrAddComponent<StatusEffectBulletSynergy>();
            synergyFreezeRight.tint = ExtendedColours.freezeBlue;
            synergyFreezeRight.StatusEffects.AddRange(new List<GameActorEffect>() { StaticStatusEffects.frostBulletsEffect });
            synergyFreezeRight.synergyToCheckFor = "It's About Rime";

            //CHARGE BULLET STATS
            Projectile chargeprojectile = (PickupObjectDatabase.GetById(56) as Gun).DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            chargeprojectile.SetProjectileSprite("cortexblaster_proj", 17, 7, false, tk2dBaseSprite.Anchor.MiddleCenter, 11, 3);
            chargeprojectile.hitEffects.alwaysUseMidair = true;
            chargeprojectile.hitEffects.overrideMidairDeathVFX = EasyVFXDatabase.PurpleLaserCircleVFX;
            chargeprojectile.baseData.damage = 11;
            chargeprojectile.baseData.force *= 1.2f;
            chargeprojectile.baseData.speed *= 0.9f;
            chargeprojectile.gameObject.AddComponent<PierceProjModifier>();
            chargeprojectile.gameObject.AddComponent<BounceProjModifier>();
            chargeprojectile.gameObject.name = "CortexBlaster_Main_ChargeProj";

            SneakyShotgunComponent sneakshot = chargeprojectile.gameObject.AddComponent<SneakyShotgunComponent>();
            sneakshot.eraseSource = false;
            sneakshot.angleVariance = 0;
            sneakshot.doVelocityRandomiser = false;
            sneakshot.useComplexPrefabs = true;
            sneakshot.complexPrefabs.Add(leftWiggle);
            sneakshot.complexPrefabs.Add(rightWiggle);


            chargeprojectile.AddTrailToProjectile(
                "NevernamedsItems/Resources/TrailSprites/cortexblaster_trail_001",
                new Vector2(6, 4),
                new Vector2(0, 1),
                BeamAnimPaths, 20,
                BeamAnimPaths, 20,
                -1,
                0.0001f,
                -1,
                true
                );
            chargeprojectile.gameObject.GetOrAddComponent<EmmisiveTrail>();

            ProjectileModule.ChargeProjectile chargeProj1 = new ProjectileModule.ChargeProjectile
            {
                Projectile = projectile,
                ChargeTime = 0f,
                VfxPool = null,

            };
            ProjectileModule.ChargeProjectile chargeProj2 = new ProjectileModule.ChargeProjectile
            {
                Projectile = chargeprojectile,
                ChargeTime = 0.5f,
            };
            gun.DefaultModule.chargeProjectiles = new List<ProjectileModule.ChargeProjectile> { chargeProj1, chargeProj2 };





            gun.quality = PickupObject.ItemQuality.B;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            ID = gun.PickupObjectId;
        }
        public static int ID;
        public override void PostProcessProjectile(Projectile projectile)
        {
            if (gun && gun.GunPlayerOwner() && gun.GunPlayerOwner().PlayerHasActiveSynergy("Hidden Gem"))
            {
                if (projectile.gameObject.name.Contains("CortexBlaster_Main_ChargeProj"))
                {
                    BounceProjModifier bonce = projectile.gameObject.GetComponent<BounceProjModifier>();
                    if (bonce) { bonce.OnBounceContext += BounceProjContext; }
                }
            }
            base.PostProcessProjectile(projectile);
        }
        public void BounceProjContext(BounceProjModifier mod, SpeculativeRigidbody spec)
        {
            if (mod && mod.projectile)
            {
                mod.projectile.baseData.damage *= 1.4f;
                HomingModifier homing = mod.projectile.gameObject.GetOrAddComponent<HomingModifier>();
                homing.AngularVelocity = 360f;
                homing.HomingRadius = 5f;
            }
        }
    }
}