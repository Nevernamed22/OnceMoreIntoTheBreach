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
using SaveAPI;
using Alexandria.Assetbundle;

namespace NevernamedsItems
{
    public class BusterGun : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Buster Gun", "bustergun");
            Game.Items.Rename("outdated_gun_mods:buster_gun", "nn:buster_gun");
            var behav = gun.gameObject.AddComponent<BusterGun>();
            gun.SetShortDescription("Strife");
            gun.SetLongDescription("Fires powerful blasts that ripple the air and stop the Gundead in their tracks."+"\n\nThe true origin of this weapon is unknown, but some Gungeonologists speculate it to be a Pre-Gungeon artefact.");

            gun.SetGunSprites("bustergun", 8, false, 2);

            gun.SetAnimationFPS(gun.shootAnimation, 16);
            gun.SetAnimationFPS(gun.reloadAnimation, 5);
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.reloadAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;

            gun.doesScreenShake = true;
            gun.gunScreenShake = StaticExplosionDatas.genericLargeExplosion.ss;
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(593) as Gun).gunSwitchGroup;

            gun.gunHandedness = GunHandedness.OneHanded;

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(56) as Gun, true, false);

            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(180) as Gun).muzzleFlashEffects;

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1f;
            gun.DefaultModule.cooldownTime = 1.5f;
            gun.DefaultModule.numberOfShotsInClip = 3;

            gun.SetBarrel(63, 15);
            gun.SetBaseMaxAmmo(50);
            gun.gunClass = GunClass.EXPLOSIVE;

            //BULLET STATS
            Projectile projectile = ProjectileSetupUtility.MakeProjectile(56, 50f);
            projectile.baseData.speed *= 2f;
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.SetProjectileSprite("bustergun_proj", 29, 10, true, tk2dBaseSprite.Anchor.MiddleCenter, 25, 8);
            GameObject BusterGunImpact = VFXToolbox.CreateVFXBundle("BusterGunImpact", false, 0, 10, 10, new Color32(255, 255, 255, 255));
            VFXPool impactPool = VFXToolbox.CreateBlankVFXPool(BusterGunImpact);


            projectile.hitEffects.tileMapVertical = impactPool;
            projectile.hitEffects.tileMapHorizontal = impactPool;
            projectile.hitEffects.enemy = impactPool;
            projectile.hitEffects.overrideMidairDeathVFX = BusterGunImpact;
            projectile.pierceMinorBreakables = true;
            projectile.objectImpactEventName = "plasmarifle";

            DistortionWaveDamager dist = projectile.gameObject.AddComponent<DistortionWaveDamager>();
            dist.lockDownDuration = 5f;
            dist.audioEvent = "Play_WPN_distortion_split_01";
            dist.Range = 5f;

            gun.carryPixelOffset = new IntVector2(1, 4);

            gun.AddShellCasing(1, 1, 0, 0, "shell_bustergun");     
            GameObject smokepoint = new GameObject("Smokepoint");
            smokepoint.transform.SetParent(gun.shellCasing.transform);
            smokepoint.transform.localPosition = new Vector3(7f / 16f, 3f / 16f, 0f);
            SpriteSparkler smoker = gun.shellCasing.AddComponent<SpriteSparkler>();
            smoker.childTarget = "Smokepoint";
            smoker.doVFX = true;
            smoker.particlesPerSecond = 3f;
            smoker.lifetime = 5f;
            smoker.randomise = true;
            smoker.VFX = VFXToolbox.CreateVFXBundle("SmallSmokePuff", false, 0);

            gun.reloadEffects = VFXToolbox.CreateBlankVFXPool(VFXToolbox.CreateVFXBundle("SmokePlume", false, 0), false);
            if (gun.reloadOffset == null)
            {
                GameObject rl = new GameObject("reloadOffset");
                rl.transform.SetParent(gun.transform);
                gun.reloadOffset = rl.transform;
            }
            gun.reloadOffset.transform.localPosition = new Vector3(36f / 16f, 18f / 16f, 0f);

            gun.AddClipSprites("bustergun");

            gun.quality = PickupObject.ItemQuality.B;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
        }
        public static int ID;
    }
}