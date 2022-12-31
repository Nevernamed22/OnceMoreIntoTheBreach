using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.Misc;
using Alexandria.ItemAPI;
using SaveAPI;

namespace NevernamedsItems
{

    public class Smoker : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Smoker", "smoker");
            Game.Items.Rename("outdated_gun_mods:smoker", "nn:smoker");
            gun.gameObject.AddComponent<Smoker>();
            gun.SetShortDescription("");
            gun.SetLongDescription("");

            gun.SetupSprite(null, "smoker_idle_001", 8);

            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.SetAnimationFPS(gun.reloadAnimation, 12);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(56) as Gun, true, false);
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(520) as Gun).gunSwitchGroup;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(26) as Gun).muzzleFlashEffects;
            gun.doesScreenShake = false;
            gun.usesContinuousFireAnimation = true;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0.7f;
            gun.DefaultModule.cooldownTime = 0.1f;
            gun.DefaultModule.numberOfShotsInClip = 20;
            gun.barrelOffset.transform.localPosition = new Vector3(25f / 16f, 14f / 16f, 0f);
            gun.SetBaseMaxAmmo(300);
            gun.ammo = 300;
            gun.gunClass = GunClass.FULLAUTO;

            //BULLET STATS
            Projectile projectile = gun.DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.AnimateProjectile(new List<string> {
                "smokeproj_001",
                "smokeproj_002",
                "smokeproj_003",
                "smokeproj_004",
                "smokeproj_005",
                "smokeproj_006",
                "smokeproj_007",
                "smokeproj_008",
                "smokeproj_009",
                "smokeproj_010",
                "smokeproj_011",
                "smokeproj_012",
            }, 12, true, AnimateBullet.ConstructListOfSameValues(new IntVector2(48, 48), 12),
            AnimateBullet.ConstructListOfSameValues(false, 12),
            AnimateBullet.ConstructListOfSameValues(tk2dBaseSprite.Anchor.MiddleCenter, 12),
            AnimateBullet.ConstructListOfSameValues(true, 12),
            AnimateBullet.ConstructListOfSameValues(false, 12),
            AnimateBullet.ConstructListOfSameValues<Vector3?>(null, 12),
            AnimateBullet.ConstructListOfSameValues<IntVector2?>(new IntVector2(26, 26), 12),
            AnimateBullet.ConstructListOfSameValues<IntVector2?>(null, 12),
            AnimateBullet.ConstructListOfSameValues<Projectile>(null, 12));
            projectile.sprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitBlendUber");
            projectile.sprite.renderer.material.SetFloat("_VertexColor", 1f);
            projectile.sprite.color = projectile.sprite.color.WithAlpha(0.65f);
            projectile.sprite.usesOverrideMaterial = true;
            projectile.baseData.speed *= 0.65f;
            projectile.baseData.force = 0;
            projectile.baseData.damage = 1;
            PierceProjModifier pierce = projectile.gameObject.GetOrAddComponent<PierceProjModifier>();
            pierce.penetration += 100;
            pierce.penetratesBreakables = true;
            BounceProjModifier bounce = projectile.gameObject.GetOrAddComponent<BounceProjModifier>();
            bounce.numberOfBounces = 5;
            SlowDownOverTimeModifier slowDown = projectile.gameObject.GetOrAddComponent<SlowDownOverTimeModifier>();
            slowDown.extendTimeByRangeStat = true;
            slowDown.killAfterCompleteStop = true;
            slowDown.targetSpeed = 0;
            slowDown.timeToSlowOver = 1f;
            slowDown.timeTillKillAfterCompleteStop = 20f;
            slowDown.doRandomTimeMultiplier = true;
            projectile.gameObject.GetOrAddComponent<DieWhenOwnerNotInRoom>();

            VFXPool SmokePoof = VFXToolbox.CreateVFXPool("SmokePoof",
                new List<string>()
                {
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/smokeimpact_vfx_001",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/smokeimpact_vfx_002",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/smokeimpact_vfx_003",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/smokeimpact_vfx_004",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/smokeimpact_vfx_005",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/smokeimpact_vfx_006",
                },
                10, //FPS
                new IntVector2(48, 48), //Dimensions
                tk2dBaseSprite.Anchor.MiddleLeft, //Anchor
                false, //Uses a Z height off the ground
                0 //The Z height, if used
                  );
            tk2dBaseSprite smokePoofSprite = SmokePoof.effects[0].effects[0].effect.GetComponent<tk2dBaseSprite>();
            smokePoofSprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitBlendUber");
            smokePoofSprite.renderer.material.SetFloat("_VertexColor", 1f);
            smokePoofSprite.color = projectile.sprite.color.WithAlpha(0.65f);
            smokePoofSprite.usesOverrideMaterial = true;

            projectile.hitEffects.tileMapVertical = SmokePoof;
            projectile.hitEffects.tileMapHorizontal = SmokePoof;
            projectile.hitEffects.deathAny = SmokePoof;
            projectile.hitEffects.overrideMidairDeathVFX = SmokePoof.effects[0].effects[0].effect;
            VFXPool smallSmokePoof =  VFXToolbox.CreateVFXPool("SmokePoof Small",
                new List<string>()
                {
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/smokeenemyimpact_vfx_001",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/smokeenemyimpact_vfx_002",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/smokeenemyimpact_vfx_003",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/smokeenemyimpact_vfx_004",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/smokeenemyimpact_vfx_005",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/smokeenemyimpact_vfx_006",
                },
                10, //FPS
                new IntVector2(24, 24), //Dimensions
                tk2dBaseSprite.Anchor.MiddleLeft, //Anchor
                false, //Uses a Z height off the ground
                0 //The Z height, if used
                  );
            tk2dBaseSprite smallSmokePoofSprite = smallSmokePoof.effects[0].effects[0].effect.GetComponent<tk2dBaseSprite>();
            smallSmokePoofSprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitBlendUber");
            smallSmokePoofSprite.renderer.material.SetFloat("_VertexColor", 1f);
            smallSmokePoofSprite.color = projectile.sprite.color.WithAlpha(0.65f);
            smallSmokePoofSprite.usesOverrideMaterial = true;
            projectile.hitEffects.enemy = smallSmokePoof;

            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = CustomClipAmmoTypeToolbox.AddCustomAmmoType("Smoker Ammo", "NevernamedsItems/Resources/CustomGunAmmoTypes/smoker_clipfull", "NevernamedsItems/Resources/CustomGunAmmoTypes/smoker_clipempty");


            gun.quality = PickupObject.ItemQuality.C;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            ID = gun.PickupObjectId;
        }
        public static int ID;
    }
}

