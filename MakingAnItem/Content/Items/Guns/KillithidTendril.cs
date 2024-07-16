using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.Assetbundle;

namespace NevernamedsItems
{
    public class KillithidTendril : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Killithid Tendril", "killithidtendril");
            Game.Items.Rename("outdated_gun_mods:killithid_tendril", "nn:killithid_tendril");
      var behav=      gun.gameObject.AddComponent<KillithidTendril>();
            behav.preventNormalFireAudio = true;
            behav.preventNormalReloadAudio = true;
            behav.overrideNormalFireAudio = "Play_ENM_squidface_cast_01";
            behav.overrideNormalReloadAudio = "Play_ENM_squidface_chant_01";
            gun.SetShortDescription("Wiggle Wiggle");
            gun.SetLongDescription("A tadpole of the Killithid species, capable of opening up portals to it's home dimension."+"\n\nWhile currently dormant, one day it will become active and burrow into the head of a sapient humanoid, eat their brain, and turn them into another Killithid.");

            gun.SetGunSprites("killithidtendril");

            gun.SetAnimationFPS(gun.shootAnimation, 10);

            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(35) as Gun).gunSwitchGroup;
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Ordered;
            gun.reloadTime = 1f;
            gun.DefaultModule.cooldownTime = 0.4f;
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.DefaultModule.numberOfShotsInClip = 200;
            gun.barrelOffset.transform.localPosition = new Vector3(0.93f, 0.56f, 0f);
            gun.SetBaseMaxAmmo(200);
            gun.ammo = 200;
            gun.gunClass = GunClass.SILLY;
            Projectile subproj = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            subproj.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(subproj.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(subproj);
            subproj.baseData.damage = 8f;
            subproj.SetProjectileSpriteRight("enemystyleproj", 10, 10, true, tk2dBaseSprite.Anchor.MiddleCenter, 8, 8);
            subProjectile = subproj;


            //BULLET STATS
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            projectile.baseData.damage = 15f;
            projectile.baseData.speed = 0;
            RandomRoomPosBehaviour roompos = projectile.gameObject.AddComponent<RandomRoomPosBehaviour>();
            BulletLifeTimer lifespan = projectile.gameObject.AddComponent<BulletLifeTimer>();
            lifespan.secondsTillDeath = 15f;
            PierceProjModifier piercing = projectile.gameObject.AddComponent<PierceProjModifier>();
            piercing.penetration = 100;
            piercing.penetratesBreakables = true;
            SpawnProjModifier firing = projectile.gameObject.AddComponent<SpawnProjModifier>();
            firing.InFlightSourceTransform = projectile.transform;
            firing.projectileToSpawnInFlight = subProjectile;
            firing.PostprocessSpawnedProjectiles = true;
            firing.spawnProjectilesInFlight = true;
            firing.spawnProjecitlesOnDieInAir = false;
            firing.spawnOnObjectCollisions = false;
            firing.inFlightAimAtEnemies = true;
            firing.usesComplexSpawnInFlight = true;
            firing.numToSpawnInFlight = 1;
            firing.inFlightSpawnCooldown = 1;
            projectile.baseData.force *= 1.2f;
            projectile.hitEffects.alwaysUseMidair = true;
            projectile.hitEffects.overrideMidairDeathVFX = EasyVFXDatabase.YellowLaserCircleVFX;
            projectile.AnimateProjectileBundle("KillithidTendrilProjectile",
                   Initialisation.ProjectileCollection,
                   Initialisation.projectileAnimationCollection,
                   "KillithidTendrilProjectile",
                   new List<IntVector2> {
                        new IntVector2(16, 16),
                        new IntVector2(20, 20),
                        new IntVector2(24, 24),
                        new IntVector2(20, 20),
                   }, //Pixel Sizes
                   MiscTools.DupeList(true, 4), //Lightened
                   MiscTools.DupeList(tk2dBaseSprite.Anchor.MiddleCenter, 4), //Anchors
                   MiscTools.DupeList(true, 4), //Anchors Change Colliders
                   MiscTools.DupeList(false, 4), //Fixes Scales
                   MiscTools.DupeList<Vector3?>(null, 4), //Manual Offsets
                   MiscTools.DupeList<IntVector2?>(new IntVector2(16, 16), 4), //Override colliders
                   MiscTools.DupeList<IntVector2?>(null, 4), //Override collider offsets
                   MiscTools.DupeList<Projectile>(null, 4)); // Override to copy from    

            gun.DefaultModule.projectiles[0] = projectile;

            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = CustomClipAmmoTypeToolbox.AddCustomAmmoType("KillithidTendril Bullets", "NevernamedsItems/Resources/CustomGunAmmoTypes/killithidtendril_clipfull", "NevernamedsItems/Resources/CustomGunAmmoTypes/killithidtendril_clipempty");

            gun.quality = PickupObject.ItemQuality.B;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            KillithidTendrilID = gun.PickupObjectId;
        }
        public static Projectile subProjectile;
        public static int KillithidTendrilID;
        public KillithidTendril()
        {

        }
    }
}