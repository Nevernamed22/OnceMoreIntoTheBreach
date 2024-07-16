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
    public class DublDuck : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Dub'l Duck", "dublduck");
            Game.Items.Rename("outdated_gun_mods:dub'l_duck", "nn:dubl_duck");
            var behav = gun.gameObject.AddComponent<DublDuck>();

            gun.SetShortDescription("Quackpot");
            gun.SetLongDescription("An existing bolt action pistol model with a customised grip, shamelessly shipped to the Gungeon by it's own creator in an attempt to raise his own notoriety.");

            gun.SetGunSprites("dublduck");

            gun.SetAnimationFPS(gun.shootAnimation, 12);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(49) as Gun, true, false);

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0.9f;
            gun.DefaultModule.cooldownTime = 0.1f;
            gun.DefaultModule.numberOfShotsInClip = 2;
            gun.barrelOffset.transform.localPosition = new Vector3(29f / 16f, 12f / 16f, 0f);
            gun.SetBaseMaxAmmo(100);
            gun.gunClass = GunClass.RIFLE;

            //BULLET STATS
            Projectile projectile = gun.DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage = 15f;
            ProjectileShootbackMod shootback = projectile.gameObject.AddComponent<ProjectileShootbackMod>();
            shootback.prefabToFire = ((Gun)PickupObjectDatabase.GetById(27)).DefaultModule.finalProjectile;

            projectile.pierceMinorBreakables = true;

            gun.quality = PickupObject.ItemQuality.C;
            ETGMod.Databases.Items.Add(gun, false, "ANY");

            ID = gun.PickupObjectId;
        }
        public static int ID;     
    }
}
