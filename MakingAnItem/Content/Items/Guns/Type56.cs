using Alexandria.ItemAPI;
using Alexandria.Misc;
using Gungeon;
using UnityEngine;

namespace NevernamedsItems
{

    public class Type56 : AdvancedGunBehavior
    {

        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Type 56", "type56");
            Game.Items.Rename("outdated_gun_mods:type_56", "nn:type_56");
            gun.gameObject.AddComponent<Type56>();
            gun.SetShortDescription("State Standard");
            gun.SetLongDescription("Cheap imperial hardware from the fringes of Hegemony space. Its brittle ammunition is prone to fragmentation.");

            gun.SetupSprite(null, "type56_idle_001", 8);

            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.SetAnimationFPS(gun.reloadAnimation, 0);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(15) as Gun, true, false);
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(15) as Gun).gunSwitchGroup;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(15) as Gun).muzzleFlashEffects;

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0.7f;
            gun.DefaultModule.cooldownTime = 0.1f;
            gun.DefaultModule.numberOfShotsInClip = 20;
            gun.barrelOffset.transform.localPosition = new Vector3(28f / 16f, 5f / 16f, 0f);
            gun.SetBaseMaxAmmo(400);
            gun.ammo = 400;
            gun.gunClass = GunClass.FULLAUTO;

            //BULLET STATS
            Projectile projectile = gun.DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            projectile.baseData.damage = 5;
            gun.DefaultModule.projectiles[0] = projectile;
            SpawnProjModifier spawnProjModifier = projectile.gameObject.AddComponent<SpawnProjModifier>();
            spawnProjModifier.SpawnedProjectilesInheritAppearance = true;
            spawnProjModifier.SpawnedProjectileScaleModifier = 0.5f;
            spawnProjModifier.SpawnedProjectilesInheritData = true;
            spawnProjModifier.spawnProjectilesOnCollision = true;
            spawnProjModifier.spawnProjecitlesOnDieInAir = true;
            spawnProjModifier.doOverrideObjectCollisionSpawnStyle = true;
            spawnProjModifier.randomRadialStartAngle = true;
            spawnProjModifier.startAngle = Random.Range(0, 180);          
            spawnProjModifier.numberToSpawnOnCollison = 4;
            spawnProjModifier.projectileToSpawnOnCollision = (PickupObjectDatabase.GetById(531) as ComplexProjectileModifier).CollisionSpawnProjectile;
            spawnProjModifier.collisionSpawnStyle = SpawnProjModifier.CollisionSpawnStyle.FLAK_BURST;


            gun.quality = PickupObject.ItemQuality.B;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            ID = gun.PickupObjectId;

            gun.SetTag("kalashnikov");
        }
        public static int ID;
    }
}

