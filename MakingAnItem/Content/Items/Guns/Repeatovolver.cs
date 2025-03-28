﻿using System;
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
using SaveAPI;

namespace NevernamedsItems
{

    public class Repeatovolver : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Repeatovolver", "repeatovolver");
            Game.Items.Rename("outdated_gun_mods:repeatovolver", "nn:repeatovolver");
            gun.gameObject.AddComponent<Repeatovolver>();
            gun.SetShortDescription("Rolls Off The Tongue");
            gun.SetLongDescription("A revolver modified to spew forth it's entire extended cylinder with a single pull of the trigger." + "\n\nNames such as 'Repeating Revolver', 'Revolverpeater' and 'Rerererererevolver' were floated, but eventually 'Repeatovolver' won out.");

            Alexandria.Assetbundle.GunInt.SetupSprite(gun, Initialisation.gunCollection, "repeatovolver_idle_001", 8, "repeatovolver_ammonomicon_001");

            gun.SetAnimationFPS(gun.shootAnimation, 15);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(56) as Gun, true, false);

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Burst;
            gun.DefaultModule.burstShotCount = 10;
            gun.DefaultModule.burstCooldownTime = 0.04f;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1f;
            gun.DefaultModule.cooldownTime = 1f;
            gun.DefaultModule.numberOfShotsInClip = 10;
            gun.barrelOffset.transform.localPosition = new Vector3(1.31f, 0.62f, 0f);
            gun.SetBaseMaxAmmo(1000);
            gun.gunClass = GunClass.FULLAUTO;
            //BULLET STATS
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;

            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(62) as Gun).muzzleFlashEffects;
            gun.gunHandedness = GunHandedness.OneHanded;
            projectile.baseData.range *= 2f;
            projectile.transform.parent = gun.barrelOffset;
            projectile.SetProjectileSpriteRight("repeating_projectile", 9, 6, false, tk2dBaseSprite.Anchor.MiddleCenter, 9, 6);

            gun.AddShellCasing(1, 0, 0, 0);

            gun.quality = PickupObject.ItemQuality.B;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            gun.SetName("Repeatovolver");

            RepeatovolverID = gun.PickupObjectId;
            gun.SetupUnlockOnCustomFlag(CustomDungeonFlags.PURCHASED_REPEATOVOLVER, true);
        }
        public static int RepeatovolverID;
        public override void Update()
        {
            if (gun && gun.GunPlayerOwner())
            {
                PlayerController player = gun.GunPlayerOwner();
                if (player.PlayerHasActiveSynergy("Ad Infinitum") && !gun.InfiniteAmmo)
                {
                    gun.InfiniteAmmo = true;
                }
                else if (!player.PlayerHasActiveSynergy("Ad Infinitum") && gun.InfiniteAmmo)
                {
                    gun.InfiniteAmmo = false;
                }
            }
        }
        public override void PostProcessProjectile(Projectile projectile)
        {
            if (projectile && projectile.ProjectilePlayerOwner() && base.gun && gun.CurrentOwner && gun.IsCurrentGun())
            {
                if (projectile.ProjectilePlayerOwner().PlayerHasActiveSynergy("Repeating Yourself"))
                {
                    projectile.OnHitEnemy += OnHit;
                }

                bool isFirstShot = (gun.LastShotIndex == 0);
                bool isLastShot = (gun.LastShotIndex == gun.ClipCapacity - 1);
                if (isFirstShot && projectile.ProjectilePlayerOwner().PlayerHasActiveSynergy("Ad Nauseum"))
                {
                    projectile.AdjustPlayerProjectileTint(ExtendedColours.plaguePurple, 2);
                    projectile.statusEffectsToApply.Add(StaticStatusEffects.StandardPlagueEffect);
                }
                if (isLastShot && projectile.ProjectilePlayerOwner().PlayerHasActiveSynergy("Ad Nauseum"))
                {
                    projectile.AdjustPlayerProjectileTint(ExtendedColours.poisonGreen, 2);
                    projectile.statusEffectsToApply.Add(StaticStatusEffects.irradiatedLeadEffect);
                }
            }
            base.PostProcessProjectile(projectile);
        }
        public void OnHit(Projectile proj, SpeculativeRigidbody bod, bool fatal)
        {
            if (fatal)
            {
                int missingShots = gun.ClipCapacity - gun.ClipShotsRemaining;
                if (missingShots > 0) gun.MoveBulletsIntoClip(missingShots);
            }
        }
        public Repeatovolver()
        {

        }
    }
}

