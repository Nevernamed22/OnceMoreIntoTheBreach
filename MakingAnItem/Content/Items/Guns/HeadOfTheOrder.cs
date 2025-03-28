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
    public class HeadOfTheOrder : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Head of the Order", "headoftheorder");
            Game.Items.Rename("outdated_gun_mods:head_of_the_order", "nn:head_of_the_order");
           var behav =  gun.gameObject.AddComponent<HeadOfTheOrder>();
            behav.preventNormalFireAudio = true;
            behav.overrideNormalFireAudio = "Play_ENM_highpriest_blast_01";
            gun.SetShortDescription("Guns, Immortal");
            gun.SetLongDescription("Though torn from the rest of it's corporeal form, the immortal soul of the High Priest remains bound to this weapon.");
            gun.AddPassiveStatModifier(PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE);

            Alexandria.Assetbundle.GunInt.SetupSprite(gun, Initialisation.gunCollection, "headoftheorder_idle_001", 8, "headoftheorder_ammonomicon_001");

            gun.SetAnimationFPS(gun.shootAnimation, 10);

            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(79) as Gun).gunSwitchGroup;
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Ordered;
            gun.reloadTime = 2f;
            gun.DefaultModule.cooldownTime = 0.65f;
            gun.DefaultModule.numberOfShotsInClip = 6;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(79) as Gun).muzzleFlashEffects;
            gun.barrelOffset.transform.localPosition = new Vector3(1.62f, 0.75f, 0f);
            gun.SetBaseMaxAmmo(100);
            gun.ammo = 100;
            gun.gunClass = GunClass.PISTOL;
            //BULLET STATS
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            projectile.baseData.damage *= 6f;
            projectile.baseData.force *= 2f;
            projectile.baseData.speed *= 2f;
            projectile.pierceMinorBreakables = true;
            HomingModifier homing = projectile.gameObject.GetOrAddComponent<HomingModifier>();
            homing.HomingRadius = 20f;
            homing.AngularVelocity = 400;

            projectile.AnimateProjectileBundle("HeadOfTheOrderProjectile",
                   Initialisation.ProjectileCollection,
                   Initialisation.projectileAnimationCollection,
                   "HeadOfTheOrderProjectile",
                   MiscTools.DupeList(new IntVector2(24, 20), 10), //Pixel Sizes
                   MiscTools.DupeList(true, 10), //Lightened
                   MiscTools.DupeList(tk2dBaseSprite.Anchor.MiddleCenter, 10), //Anchors
                   MiscTools.DupeList(true, 10), //Anchors Change Colliders
                   MiscTools.DupeList(false, 10), //Fixes Scales
                   MiscTools.DupeList<Vector3?>(null, 10), //Manual Offsets
                   MiscTools.DupeList<IntVector2?>(new IntVector2(8, 8), 10), //Override colliders
                   MiscTools.DupeList<IntVector2?>(null, 10), //Override collider offsets
                   MiscTools.DupeList<Projectile>(null, 10)); // Override to copy from    

            gun.DefaultModule.projectiles[0] = projectile;

            gun.quality = PickupObject.ItemQuality.S;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            HeadOfTheOrderID = gun.PickupObjectId;
        }
        public static int HeadOfTheOrderID;
        protected override void Update()
        {
            if (gun.CurrentOwner && gun.CurrentOwner is PlayerController)
            {
                PlayerController player = gun.CurrentOwner as PlayerController;
                if (player.PlayerHasActiveSynergy("Non Desistas Non Exieris") && gun.reloadTime == 2f)
                {
                    gun.reloadTime = 1f;
                    gun.SetBaseMaxAmmo(200);
                    gun.DefaultModule.cooldownTime = 0.4f;
                }
                else if (!player.PlayerHasActiveSynergy("Non Desistas Non Exieris") && gun.reloadTime == 1f)
                {
                    gun.reloadTime = 2f;
                    gun.SetBaseMaxAmmo(100);
                    gun.DefaultModule.cooldownTime = 0.65f;
                }
                player.stats.RecalculateStats(player, true, false);
            }
            base.Update();
        }
        public override void OnSwitchedAwayFromThisGun()
        {
            if (gun.CurrentOwner && gun.CurrentOwner is PlayerController)
            {
                RemoveFlight(gun.CurrentOwner as PlayerController);
            }
            base.OnSwitchedAwayFromThisGun();
        }
        public override void OnSwitchedToThisGun()
        {
            if (gun.CurrentOwner && gun.CurrentOwner is PlayerController)
            {
                GiveFlight(gun.CurrentOwner as PlayerController);
            }
            base.OnSwitchedToThisGun();
        }
        protected override void OnPickedUpByPlayer(PlayerController player)
        {
            GiveFlight(player);
            base.OnPickedUpByPlayer(player);
        }
        protected override void OnPostDroppedByPlayer(PlayerController player)
        {
            RemoveFlight(player);
            base.OnPostDroppedByPlayer(player);
        }
        public override void OnDestroy()
        {
            if (gun && gun.CurrentOwner && gun.CurrentOwner is PlayerController)
            {
                RemoveFlight(gun.CurrentOwner as PlayerController);
            }
            base.OnDestroy();
        }
        private void GiveFlight(PlayerController playerController)
        {
           playerController.SetIsFlying(true, "HeadOfTheOrder", true, false);
            playerController.AdditionalCanDodgeRollWhileFlying.AddOverride("HeadOfTheOrder", null);
        }
        private void RemoveFlight(PlayerController playerController)
        {
            playerController.SetIsFlying(false, "HeadOfTheOrder", true, false);
            playerController.AdditionalCanDodgeRollWhileFlying.RemoveOverride("HeadOfTheOrder");
        }
        public HeadOfTheOrder()
        {

        }
    }
}