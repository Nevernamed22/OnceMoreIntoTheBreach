using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;
using SaveAPI;
using Alexandria.Misc;

namespace NevernamedsItems
{
    public class Makatov : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Makatov", "makatov");
            Game.Items.Rename("outdated_gun_mods:makatov", "nn:makatov");
            var behav = gun.gameObject.AddComponent<Makatov>();

            gun.SetShortDescription("A drink with the meal");
            gun.SetLongDescription("A gun grip hastily glued to the bottom of a molotov."+"\n\nSpits puffs of flame.");

            gun.SetupSprite(null, "makatov_idle_001", 8);

            gun.SetAnimationFPS(gun.shootAnimation, 12);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(59) as Gun, true, false);
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(125) as Gun).gunSwitchGroup;

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.2f;
            gun.DefaultModule.cooldownTime = 0.2f;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(42) as Gun).muzzleFlashEffects;

            gun.DefaultModule.numberOfShotsInClip = 20;
            gun.barrelOffset.transform.localPosition = new Vector3(28f / 16f, 17f / 16f, 0f);
            gun.SetBaseMaxAmmo(230);
            gun.gunClass = GunClass.FIRE;

            //BULLET STATS
            Projectile projectile = StandardisedProjectiles.flamethrower.InstantiateAndFakeprefab();
            projectile.baseData.damage = 4;
            gun.DefaultModule.projectiles[0] = projectile;

            projectile.pierceMinorBreakables = true;

            AdvancedFireOnReloadSynergyProcessor reloadfire = gun.gameObject.AddComponent<AdvancedFireOnReloadSynergyProcessor>();
            reloadfire.synergyToCheck = "Molotovs Revenge";
            reloadfire.angleVariance = 5f;
            reloadfire.numToFire = 1;
            reloadfire.projToFire = (PickupObjectDatabase.GetById(292) as Gun).DefaultModule.projectiles[0];

            gun.quality = PickupObject.ItemQuality.B;
            ETGMod.Databases.Items.Add(gun, false, "ANY");

            ID = gun.PickupObjectId;
        }
        public static int ID;       
    }
}