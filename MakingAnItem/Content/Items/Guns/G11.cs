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
    public class G11 : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("G11", "g11");
            Game.Items.Rename("outdated_gun_mods:g11", "nn:g11");
            var behav = gun.gameObject.AddComponent<G11>();

            gun.SetShortDescription("Kechler and Hock");
            gun.SetLongDescription("Revolutionary recoil mitigation mechanisms cause the first two shots in each magazine of this clockwork marvel to fire with increased power."+"\n\nAn old Hegemony prototype. Somehow, never caught on.");

            gun.SetupSprite(null, "g11_idle_001", 8);

            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.SetAnimationFPS(gun.reloadAnimation, 0);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(56) as Gun, true, false);

            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(83) as Gun).gunSwitchGroup;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(49) as Gun).muzzleFlashEffects;

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.burstShotCount = 2;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Burst;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.34f;
            gun.DefaultModule.cooldownTime = 0.25f;
            gun.DefaultModule.burstCooldownTime = 0.1f;
            gun.DefaultModule.numberOfShotsInClip = 12;
            gun.barrelOffset.transform.localPosition = new Vector3(33f / 16f, 10f / 16f, 0f);
            gun.SetBaseMaxAmmo(400);
            gun.gunClass = GunClass.FULLAUTO;

            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = "Rifle";

            //BULLET STATS
            Projectile projectile = (PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            projectile.baseData.speed *= 1.2f;
            projectile.baseData.damage = 7.5f;

            gun.DefaultModule.projectiles[0] = (PickupObjectDatabase.GetById(49) as Gun).DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            

            gun.DefaultModule.usesOptionalFinalProjectile = true;
            gun.DefaultModule.numberOfFinalProjectiles = 10;
            gun.DefaultModule.finalProjectile = projectile;

            gun.carryPixelOffset = new IntVector2(10, 0);
            gun.carryPixelDownOffset = new IntVector2(-2, -10);

            gun.quality = PickupObject.ItemQuality.B;
            ETGMod.Databases.Items.Add(gun, false, "ANY");
            ID = gun.PickupObjectId;
        }
        public static int ID;
    }
}
