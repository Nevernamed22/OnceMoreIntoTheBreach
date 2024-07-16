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

    public class Borchardt : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Borchardt", "borchardt");
            Game.Items.Rename("outdated_gun_mods:borchardt", "nn:borchardt");
            var behav = gun.gameObject.AddComponent<Borchardt>();
            gun.SetShortDescription("Preluger");
            gun.SetLongDescription("Landing a shot on an enemy has a chance to force a shotgun blast from the meagre barrel of this weapon. Exactly why this works isn't entirely known, but most people don't care too much.");

            gun.SetGunSprites("borchardt");

            gun.SetAnimationFPS(gun.shootAnimation, 10);
            gun.SetAnimationFPS(gun.reloadAnimation, 0);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(22) as Gun, true, false);
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(22) as Gun).muzzleFlashEffects;
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(22) as Gun).gunSwitchGroup;

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.2f;
            gun.DefaultModule.cooldownTime = 0.22f;
            gun.DefaultModule.angleVariance = 8f;
            gun.DefaultModule.numberOfShotsInClip = 12;
            gun.barrelOffset.transform.localPosition = new Vector3(32f / 16f, 12f / 16f, 0f);
            gun.SetBaseMaxAmmo(400);
            gun.gunClass = GunClass.PISTOL;

            //BULLET STATS
            Projectile projectile = gun.DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            projectile.baseData.damage = 10f;
            gun.DefaultModule.projectiles[0] = projectile;

            gun.quality = PickupObject.ItemQuality.B;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            gun.AddToSubShop(ItemBuilder.ShopType.Trorc);
            ID = gun.PickupObjectId;
            gun.SetupUnlockOnCustomFlag(CustomDungeonFlags.PURCHASED_BORCHARDT, true);
            gun.AddItemToTrorcMetaShop(20);
        }
        public static int ID;
        public override void PostProcessProjectile(Projectile projectile)
        {
            if (projectile && projectile.ProjectilePlayerOwner()) { projectile.OnHitEnemy += OnHit; }
            base.PostProcessProjectile(projectile);
        }
        private void OnHit(Projectile proj, SpeculativeRigidbody body, bool fatal)
        {
            if (UnityEngine.Random.value <= 0.1f && gun.GunPlayerOwner())
            {
                for (int i = 0; i < 6; i++)
                {
                    Projectile newproj = gun.DefaultModule.projectiles[0].InstantiateAndFireInDirection(gun.barrelOffset.position, gun.CurrentAngle, 20f, gun.GunPlayerOwner()).GetComponent<Projectile>();
                    newproj.ScaleByPlayerStats(gun.GunPlayerOwner());
                    newproj.Owner = gun.GunPlayerOwner();
                    newproj.Shooter = gun.GunPlayerOwner().specRigidbody;
                    gun.GunPlayerOwner().DoPostProcessProjectile(newproj);
                    newproj.baseData.speed *= UnityEngine.Random.Range(1f, 0.8f);
                }
            }
        }
    }
}