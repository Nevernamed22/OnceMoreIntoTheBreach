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
    public class Gunion : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Gunion", "gunion");
            Game.Items.Rename("outdated_gun_mods:gunion", "nn:gunion");
            gun.gameObject.AddComponent<Gunion>();
            gun.SetShortDescription("Allium Cepa");
            gun.SetLongDescription("A gunion (Allium cepa L., from Latin cepa meaning \"gunion\"), also known as the bulb gunion or common gunion, is a vegetable that is the most widely cultivated species of the genus Allium.");

            gun.SetGunSprites("gunion", 8, false, 2);

            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.SetAnimationFPS(gun.reloadAnimation, 12);

            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(33) as Gun).muzzleFlashEffects;
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(124) as Gun).gunSwitchGroup;

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(56) as Gun, true, false);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0.4f;

            gun.gunClass = GunClass.PISTOL;
            gun.DefaultModule.cooldownTime = 0.4f;
            gun.DefaultModule.numberOfShotsInClip = 3;

            gun.SetBaseMaxAmmo(100);
            gun.quality = PickupObject.ItemQuality.C;
            ETGMod.Databases.Items.Add(gun, null, "ANY");

            gun.SetBarrel(18, 12);

            Projectile projectile = ProjectileSetupUtility.MakeProjectile(56, 7.8f);
            projectile.SetProjectileSprite("gunion_proj", 7, 4, false);
            projectile.hitEffects = (PickupObjectDatabase.GetById(33) as Gun).DefaultModule.projectiles[0].hitEffects;

            ModdedStatusEffectApplier statusEffects = projectile.gameObject.AddComponent<ModdedStatusEffectApplier>();
            statusEffects.appliesCrying = true;

            gun.DefaultModule.projectiles[0] = projectile;

            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = "poison_blob";
            //gun.AddClipSprites("minigun");
            gun.AddClipDebris(3, 1, "clipdebris_gunion");

            ID = gun.PickupObjectId;
        }
        public static int ID;
    }
}

