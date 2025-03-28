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
    public class BubbleFist : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Bubble Fist", "bubblefist");
            Game.Items.Rename("outdated_gun_mods:bubble_fist", "nn:bubble_fist");
            gun.gameObject.AddComponent<BubbleFist>();
            gun.SetShortDescription("Aqua, Man!");
            gun.SetLongDescription("A peculiar spell that conjures seafoam from the users clenched fist.\n\nPerhaps a homebrew version of the flame hand?");

            gun.SetGunSprites("bubblefist", 8, false, 2);

            gun.SetAnimationFPS(gun.shootAnimation, 12);

            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(404) as Gun).gunSwitchGroup;

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Charged;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.2f;

            gun.gunClass = GunClass.CHARGE;
            gun.DefaultModule.cooldownTime = 1f;
            gun.DefaultModule.numberOfShotsInClip = 1;

            gun.SetBaseMaxAmmo(80);
            gun.quality = PickupObject.ItemQuality.C;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            gun.gunHandedness = GunHandedness.HiddenOneHanded;
            gun.SetBarrel(40, 21);

            Projectile projectile = ProjectileSetupUtility.MakeProjectile(86, 15f);
            projectile.gameObject.name = "BubbleFistCoreProjectile";
            projectile.sprite.renderer.enabled = false;
            projectile.baseData.speed *= 2f;
            projectile.baseData.range = 7f;

            PierceProjModifier pierce = projectile.gameObject.GetOrAddComponent<PierceProjModifier>();
            pierce.penetration = 100;
            pierce.penetratesBreakables = true;

            Projectile bubble = ProjectileSetupUtility.MakeProjectile(599, 6f);
            RandomProjectileStatsComponent rand = bubble.gameObject.AddComponent<RandomProjectileStatsComponent>();
            rand.randomScale = true;
            rand.highScalePercent = 100;
            rand.lowScalePercent = 70;

            SpawnProjModifier spawner = projectile.gameObject.GetOrAddComponent<SpawnProjModifier>();
            spawner.spawnProjectilesInFlight = true;
            spawner.numToSpawnInFlight = 1;
            spawner.fireRandomlyInAngle = true;
            spawner.inFlightSpawnAngle = 360f;
            spawner.inFlightSpawnCooldown = 0.01f;
            spawner.projectileToSpawnInFlight = bubble;
            spawner.usesComplexSpawnInFlight = true;
            spawner.PostprocessSpawnedProjectiles = true;

            spawner.spawnProjectilesOnCollision = true;
            spawner.projectileToSpawnOnCollision = bubble;
            spawner.numberToSpawnOnCollison = 3;
            spawner.spawnOnObjectCollisions = true;
            spawner.spawnCollisionProjectilesOnBounce = true;
            spawner.spawnProjecitlesOnDieInAir = true;
            spawner.collisionSpawnStyle = SpawnProjModifier.CollisionSpawnStyle.FLAK_BURST;
            spawner.randomRadialStartAngle = true;

            gun.DefaultModule.projectiles[0] = projectile;
            gun.DefaultModule.chargeProjectiles = new List<ProjectileModule.ChargeProjectile> { new ProjectileModule.ChargeProjectile
            {
                Projectile = projectile,
                ChargeTime = 0.4f,
            }};

            gun.AddClipSprites("bubblefist");
            gun.AddToSubShop(ItemBuilder.ShopType.Goopton);

            ID = gun.PickupObjectId;
        }
        public static int ID;
        public float reloadsynergyccooldown = 0f;
        public override void Update()
        {
            if (gun && gun.GunPlayerOwner() && gun.GunPlayerOwner().PlayerHasActiveSynergy("F#!^@in' Bubbles") && gun.IsReloading)
            {
                if (reloadsynergyccooldown > 0) { reloadsynergyccooldown -= BraveTime.DeltaTime; }
                else
                {
                    GameObject bubble = (PickupObjectDatabase.GetById(599) as Gun).DefaultModule.projectiles[2].InstantiateAndFireInDirection(gun.PrimaryHandAttachPoint.position, gun.CurrentAngle, 5, gun.GunPlayerOwner());
                    Projectile comp = bubble.GetComponent<Projectile>();
                    if (bubble && comp)
                    {
                        comp.Owner = gun.CurrentOwner;
                        comp.Shooter = gun.CurrentOwner.specRigidbody;
                        comp.ScaleByPlayerStats(gun.GunPlayerOwner());
                        gun.GunPlayerOwner().DoPostProcessProjectile(comp);
                    }
                    reloadsynergyccooldown = 0.1f;
                }
            }
            base.Update();
        }
        public override void PostProcessProjectile(Projectile projectile)
        {
            if (projectile && projectile.gameObject.name.Contains("BubbleFistCoreProjectile") && projectile.ProjectilePlayerOwner() && projectile.ProjectilePlayerOwner().PlayerHasActiveSynergy("Bubble Column"))
            {
                projectile.baseData.range = 100f;
            }
            base.PostProcessProjectile(projectile);
        }
    }
}

