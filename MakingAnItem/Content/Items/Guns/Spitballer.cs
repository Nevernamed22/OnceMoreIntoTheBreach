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
using SaveAPI;
using Alexandria.Misc;
using Alexandria.Assetbundle;

namespace NevernamedsItems
{

    public class Spitballer : AdvancedGunBehavior
    {
        public static void Add()
        {

            Gun gun = ETGMod.Databases.Items.NewGun("Spitballer", "spitballer");
            Game.Items.Rename("outdated_gun_mods:spitballer", "nn:spitballer");
            var behav = gun.gameObject.AddComponent<Spitballer>();
            behav.preventNormalFireAudio = true;
            behav.preventNormalReloadAudio = true;

            gun.SetShortDescription("Spoot");
            gun.SetLongDescription("A rolled piece of paper designed by cruel schoolchildren to launch balls of spit and more paper at their classmates.");
            gun.SetGunSprites("spitballer");

            gun.SetAnimationFPS(gun.shootAnimation, 10);
            gun.SetAnimationFPS(gun.reloadAnimation, 0);
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).frames[0].eventAudio = "spit_fire";
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).frames[0].triggerEvent = true;

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);

            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0.5f;
            gun.DefaultModule.cooldownTime = 0.3f;
            gun.DefaultModule.numberOfShotsInClip = 3;
            gun.quality = PickupObject.ItemQuality.D;
            gun.gunClass = GunClass.SHITTY;
            gun.SetBaseMaxAmmo(1000);
            gun.carryPixelOffset = new IntVector2(1, 3);
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(33) as Gun).muzzleFlashEffects;

            gun.barrelOffset.transform.localPosition = new Vector3(12f / 16f, 3f / 16f, 0f);


            //BULLET STATS
            Projectile projectile = ProjectileUtility.SetupProjectile(86);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.gameObject.name = "spitball";
            projectile.baseData.damage = 5.1f;
            projectile.baseData.speed *= 0.8f;
            projectile.hitEffects.overrideMidairDeathVFX = VFXToolbox.CreateVFXBundle("SpitballerImpact", new IntVector2(10, 8), tk2dBaseSprite.Anchor.MiddleCenter, true, 0.2f);
            projectile.hitEffects.alwaysUseMidair = true;

            projectile.SetProjectileSprite("spitballer_proj", 4, 4, false, tk2dBaseSprite.Anchor.MiddleCenter, 4, 4);

            GoopModifier goop = projectile.gameObject.AddComponent<GoopModifier>();
            goop.SpawnGoopOnCollision = true;
            goop.CollisionSpawnRadius = 0.5f;
            goop.goopDefinition = EasyGoopDefinitions.WaterGoop;


            plagueball = ProjectileUtility.SetupProjectile(86);
            plagueball.baseData.damage = 6.1f;
            plagueball.baseData.speed *= 0.8f;
            plagueball.hitEffects.overrideMidairDeathVFX = VFXToolbox.CreateVFXBundle("PlagueSplash_Small", new IntVector2(10, 8), tk2dBaseSprite.Anchor.MiddleCenter, true, 0.2f);
            plagueball.hitEffects.alwaysUseMidair = true;
            plagueball.SetProjectileSprite("plagueball_proj", 4, 4, false, tk2dBaseSprite.Anchor.MiddleCenter, 4, 4);
            GoopModifier plaguegoop = plagueball.gameObject.AddComponent<GoopModifier>();
            plaguegoop.SpawnGoopOnCollision = true;
            plaguegoop.CollisionSpawnRadius = 0.5f;
            plaguegoop.goopDefinition = EasyGoopDefinitions.PlagueGoop;

            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = "white";

            ETGMod.Databases.Items.Add(gun, false, "ANY");
            ID = gun.PickupObjectId;
            gun.TrimGunSprites();

            gun.SetupUnlockOnCustomStat(CustomTrackedStats.BEGGAR_TOTAL_DONATIONS, 4, DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN);

        }
        public static int ID;
        public static Projectile plagueball;
        public override Projectile OnPreFireProjectileModifier(Gun gun, Projectile projectile, ProjectileModule mod)
        {
            if (projectile && projectile.name.Contains("spitball") && gun && gun.GunPlayerOwner() && gun.GunPlayerOwner().PlayerHasActiveSynergy("Superspreader"))
            {
                return plagueball;
            }
            return base.OnPreFireProjectileModifier(gun, projectile, mod);
        }
        public override void PostProcessProjectile(Projectile projectile)
        {      
            if (projectile && projectile.name.Contains("spitfire_proj") && gun && gun.GunPlayerOwner() && gun.GunPlayerOwner().PlayerHasActiveSynergy("Superspreader"))
            {
                BulletSharkProjectileDoer sharker = projectile.GetComponent<BulletSharkProjectileDoer>();
                if (sharker) { sharker.toSpawn = plagueball; }
            }
            base.PostProcessProjectile(projectile);
        }
    }
}