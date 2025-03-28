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
using Alexandria.Assetbundle;

namespace NevernamedsItems
{

    public class Monsoon : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Monsoon", "monsoon");
            Game.Items.Rename("outdated_gun_mods:monsoon", "nn:monsoon");
            gun.gameObject.AddComponent<Monsoon>();
            gun.SetShortDescription("Bless The Rains");
            gun.SetLongDescription("Lobs concentrated rainfall.\n\nAs the Gungeon is far from watertight, rainwater that falls upon the keep tends to pool in the Chambers below.");

            gun.SetGunSprites("monsoon", 8, false, 2);


            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(33) as Gun).gunSwitchGroup;
            gun.muzzleFlashEffects = VFXToolbox.CreateVFXPoolBundle("MonsoonMuzzle", false, 0, VFXAlignment.Fixed, 5, new Color32(255, 117, 117, 255));

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.23f;
            gun.DefaultModule.cooldownTime = 0.85f;
            gun.DefaultModule.numberOfShotsInClip = 5;
            gun.SetBarrel(21, 11);
            gun.SetBaseMaxAmmo(100);
            gun.ammo = 100;
            gun.gunClass = GunClass.PISTOL;
            gun.gunScreenShake = (PickupObjectDatabase.GetById(202) as Gun).gunScreenShake;

            LobbedProjectile proj = DataCloners.CopyFields<LobbedProjectile>(Instantiate((PickupObjectDatabase.GetById(33) as Gun).DefaultModule.projectiles[0]));
            proj.gameObject.MakeFakePrefab();
            proj.gameObject.AddComponent<BounceProjModifier>().numberOfBounces = 5;
            proj.visualHeight = 2f;
            proj.spawnCollisionProjectilesOnFloorBounce = true;
            proj.spawnCollisionGoopOnFloorBounce = true;
            proj.canHitAnythingEvenWhenNotGrounded = true;

            SpawnProjModifier flak = proj.gameObject.AddComponent<SpawnProjModifier>();
            flak.spawnProjectilesOnCollision = true;
            flak.spawnCollisionProjectilesOnBounce = true;
            flak.randomRadialStartAngle = true;
            flak.numberToSpawnOnCollison = 5;
            flak.collisionSpawnStyle = SpawnProjModifier.CollisionSpawnStyle.RADIAL;

            LobbedProjectile subproj = DataCloners.CopyFields<LobbedProjectile>(Instantiate((PickupObjectDatabase.GetById(33) as Gun).DefaultModule.projectiles[0]));
            subproj.gameObject.MakeFakePrefab();
            subproj.gameObject.AddComponent<BounceProjModifier>().numberOfBounces = 1;
            subproj.AdditionalScaleMultiplier = 0.5f;
            subproj.visualHeight = 1f;
            subproj.forcedDistance = 2;
            subproj.spawnCollisionGoopOnFloorBounce = true;
            subproj.canHitAnythingEvenWhenNotGrounded = true;

            GoopModifier subgooper = subproj.gameObject.AddComponent<GoopModifier>();
            subgooper.SpawnGoopOnCollision = true;
            subgooper.CollisionSpawnRadius = 0.5f;
            subgooper.goopDefinition = GoopUtility.WaterDef;

            flak.projectileToSpawnOnCollision = subproj;

            GoopModifier gooper = proj.gameObject.AddComponent<GoopModifier>();
            gooper.SpawnGoopOnCollision = true;
            gooper.CollisionSpawnRadius = 2f;
            gooper.goopDefinition = GoopUtility.WaterDef;

            gun.muzzleFlashEffects.effects[0].effects[0].attached = false;
            gun.DefaultModule.projectiles[0] = proj;
            gun.gunHandedness = GunHandedness.OneHanded;
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = "Tear";

            gun.AddShellCasing(1, 1, 0, 0, "shell_diamond");
            DebrisObject db = gun.shellCasing.GetComponent<DebrisObject>();
            db.DoesGoopOnRest = true;
            db.GoopRadius = 0.3f;
            db.AssignedGoop = GoopUtility.WaterDef;

            gun.quality = PickupObject.ItemQuality.B;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            ID = gun.PickupObjectId;

        }
        public static int ID;
    }
}
