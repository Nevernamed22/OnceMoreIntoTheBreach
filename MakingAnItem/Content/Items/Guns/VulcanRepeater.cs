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

namespace NevernamedsItems
{
    public class VulcanRepeater : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Vulcan Repeater", "vulcanrepeater");
            Game.Items.Rename("outdated_gun_mods:vulcan_repeater", "nn:vulcan_repeater");
            gun.gameObject.AddComponent<VulcanRepeater>();
            gun.SetShortDescription("Glocram Rises");
            gun.SetLongDescription("Fires powerful explosive bolts which have a chance to split in two for double the power."+"\n\nForged with the soul of an impossible beast, by warriors of a forgotten age.");
           
            gun.SetGunSprites("vulcanrepeater");

            gun.SetAnimationFPS(gun.shootAnimation, 8);
            gun.SetAnimationFPS(gun.reloadAnimation, 0);
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(12) as Gun).gunSwitchGroup;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(12) as Gun).muzzleFlashEffects;

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(56) as Gun, true, false);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0.8f;
            gun.gunClass = GunClass.PISTOL;
            gun.DefaultModule.cooldownTime = 0.5f;
            gun.DefaultModule.numberOfShotsInClip = 1;
            gun.SetBaseMaxAmmo(50);
            gun.quality = PickupObject.ItemQuality.A;

            ETGMod.Databases.Items.Add(gun, false, "ANY");

            gun.barrelOffset.transform.localPosition = new Vector3(40f / 16f, 14f / 16f, 0f);
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = CustomClipAmmoTypeToolbox.AddCustomAmmoType("Vulcan Repeater Bolts", "NevernamedsItems/Resources/CustomGunAmmoTypes/vulcanrepeater_clipfull", "NevernamedsItems/Resources/CustomGunAmmoTypes/vulcanrepeater_clipempty");

            Projectile projectile = gun.DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage = 22f;
            projectile.baseData.speed *= 1.2f;
            ExplosiveModifier explode = projectile.gameObject.AddComponent<ExplosiveModifier>();
            explode.doExplosion = true;
            explode.explosionData = StaticExplosionDatas.explosiveRoundsExplosion;
            ProjectileSplitController split = projectile.gameObject.AddComponent<ProjectileSplitController>();
            split.amtToSplitTo = 2;
            split.chanceToSplit = 0.45f;
            split.distanceBasedSplit = true;
            split.distanceTillSplit = 5f;
            split.dmgMultAfterSplit = 0.5f;
            projectile.SetProjectileSprite("vulcanrepeater_proj", 16, 5, false, tk2dBaseSprite.Anchor.MiddleCenter, 14, 3);

            ID = gun.PickupObjectId;
            gun.SetTag("arrow_bolt_weapon");

           
        }
        public static int ID;      
    }
}

