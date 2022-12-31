using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.Misc;
using Alexandria.ItemAPI;
using SaveAPI;

namespace NevernamedsItems
{

    public class RazorRifle : AdvancedGunBehavior
    {

        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Razor Rifle", "razorrifle");
            Game.Items.Rename("outdated_gun_mods:razor_rifle", "nn:razor_rifle");
            gun.gameObject.AddComponent<RazorRifle>();
            gun.SetShortDescription("Close Shave");
            gun.SetLongDescription("One of the last products from Cut-Abuv(TM) Kitchenware and Personal Hygiene, before the company devolved into bankruptcy and legal litigation.");

            gun.SetupSprite(null, "razorrifle_idle_001", 8);

            gun.SetAnimationFPS(gun.shootAnimation, 17);
            gun.SetAnimationFPS(gun.reloadAnimation, 0);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(12) as Gun).gunSwitchGroup;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(169) as Gun).muzzleFlashEffects;

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0.7f;
            gun.DefaultModule.cooldownTime = 0.5f;
            gun.DefaultModule.numberOfShotsInClip = 10;
            gun.barrelOffset.transform.localPosition = new Vector3(27f / 16f, 10f / 16f, 0f);
            gun.SetBaseMaxAmmo(200);
            gun.ammo = 200;
            gun.gunClass = GunClass.FULLAUTO;

            //BULLET STATS
            Projectile projectile = gun.DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage = 7f;
            PierceProjModifier pierce = projectile.gameObject.AddComponent<PierceProjModifier>();
            pierce.penetration++;
            projectile.AnimateProjectile(new List<string> {
                "razorrifle_proj_001",
                "razorrifle_proj_002",
                "razorrifle_proj_003",
                "razorrifle_proj_004",
            }, 12, true, AnimateBullet.ConstructListOfSameValues(new IntVector2(10, 12), 4),
            AnimateBullet.ConstructListOfSameValues(true, 4),
            AnimateBullet.ConstructListOfSameValues(tk2dBaseSprite.Anchor.MiddleCenter, 4),
            AnimateBullet.ConstructListOfSameValues(true, 4),
            AnimateBullet.ConstructListOfSameValues(false, 4),
            AnimateBullet.ConstructListOfSameValues<Vector3?>(null, 4),
            AnimateBullet.ConstructListOfSameValues<IntVector2?>(null, 4),
            AnimateBullet.ConstructListOfSameValues<IntVector2?>(null, 4),
            AnimateBullet.ConstructListOfSameValues<Projectile>(null, 4));
            InherentExsanguinationEffect bleed = projectile.gameObject.AddComponent<InherentExsanguinationEffect>();
            bleed.duration = 10;

            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = CustomClipAmmoTypeToolbox.AddCustomAmmoType("RazorRifle Bullets", "NevernamedsItems/Resources/CustomGunAmmoTypes/razorrifle_clipfull", "NevernamedsItems/Resources/CustomGunAmmoTypes/razorrifle_clipempty");


            gun.quality = PickupObject.ItemQuality.B;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            ID = gun.PickupObjectId;
        }
        public static int ID;
    }
}

