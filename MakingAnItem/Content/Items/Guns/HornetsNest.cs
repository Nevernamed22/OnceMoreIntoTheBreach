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

namespace NevernamedsItems
{
    public class HornetsNest : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Hornets Nest", "hornetsnest");
            Game.Items.Rename("outdated_gun_mods:hornets_nest", "nn:hornets_nest");
            gun.gameObject.AddComponent<HornetsNest>();
            gun.SetShortDescription("Kicked into Action");
            gun.SetLongDescription("Years of gunsmiths attempting to upgrade the humble Bee Hive have finally been proven futile with the discovery of the once-mythical Hornet.");

            gun.SetGunSprites("hornetsnest", 8, false, 2);

            gun.SetAnimationFPS(gun.shootAnimation, 12);

            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(14) as Gun).gunSwitchGroup;

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(14) as Gun, true, false);

            gun.gunHandedness = GunHandedness.TwoHanded;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(14) as Gun).muzzleFlashEffects;

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1f;
            gun.DefaultModule.cooldownTime = 0.11f;
            gun.DefaultModule.numberOfShotsInClip = -1;
            gun.SetBarrel(18,7);
            gun.SetBaseMaxAmmo(300);
            gun.ammo = 300;
            gun.gunClass = GunClass.SILLY;

            //BULLET STATS
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(138) as SpawnProjectileOnDamagedItem).synergyProjectile);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            gun.quality = PickupObject.ItemQuality.A;
            ETGMod.Databases.Items.Add(gun, false, "ANY");

            ID = gun.PickupObjectId;
        }
        public static int ID;
    }
}
