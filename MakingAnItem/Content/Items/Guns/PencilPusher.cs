using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.Misc;
using SaveAPI;
using Dungeonator;
using Alexandria.Assetbundle;

namespace NevernamedsItems
{
    public class PencilPusher : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Pencil Pusher", "pencilpusher");
            Game.Items.Rename("outdated_gun_mods:pencil_pusher", "nn:pencil_pusher");
            gun.gameObject.AddComponent<PencilPusher>();
            gun.SetShortDescription("Official");
            gun.SetLongDescription("Launches pencils.\n\nThis ill-advised device is the result of inter-office warfare taken too far.");

            gun.SetGunSprites("pencilpusher", 8, false, 2);

            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.SetAnimationFPS(gun.reloadAnimation, 12);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(56) as Gun, true, false);
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(806) as Gun).gunSwitchGroup;

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0.7f;
            gun.DefaultModule.cooldownTime = 0.4f;
            gun.DefaultModule.numberOfShotsInClip = 1;
            gun.SetBarrel(28, 16);
            gun.SetBaseMaxAmmo(70);
            gun.gunClass = GunClass.RIFLE;

            //BULLET STATS
            Projectile projectile = ProjectileSetupUtility.MakeProjectile(56, 20f);
            projectile.baseData.speed *= 1.3f;

            GameObject penciltip = new GameObject("PencilTip");
            penciltip.transform.SetParent(projectile.transform);
            penciltip.transform.localPosition = new Vector3(21f / 16f, 3f / 16f, 0f);
            projectile.gameObject.GetOrAddComponent<PencilSketcher>();

            projectile.AnimateProjectileBundle("PencilPusherProjectile",
                  Initialisation.ProjectileCollection,
                  Initialisation.projectileAnimationCollection,
                  "PencilPusherProjectile",
                  MiscTools.DupeList(new IntVector2(22, 5), 5), //Pixel Sizes
                  MiscTools.DupeList(false, 5), //Lightened
                  MiscTools.DupeList(tk2dBaseSprite.Anchor.MiddleCenter, 5), //Anchors
                  MiscTools.DupeList(true, 5), //Anchors Change Colliders
                  MiscTools.DupeList(false, 5), //Fixes Scales
                  MiscTools.DupeList<Vector3?>(null, 5), //Manual Offsets
                  MiscTools.DupeList<IntVector2?>(new IntVector2(20, 3), 5), //Override colliders
                  MiscTools.DupeList<IntVector2?>(null, 5), //Override collider offsets
                  MiscTools.DupeList<Projectile>(null, 5)); // Override to copy from    

            projectile.hitEffects.deathTileMapHorizontal = VFXToolbox.CreateVFXPoolBundle("PencilPusherImpactHoriz", false, 0, persist: true);
            projectile.hitEffects.deathTileMapVertical = VFXToolbox.CreateVFXPoolBundle("PencilPusherImpactVert", false, 0, persist: true);

            VFXPool debrisImpact = new VFXPool()
            {
                type = VFXPoolType.All,
                effects = new List<VFXComplex>() {new VFXComplex()
                {
                  effects = new List<VFXObject>()
                  {
                      new VFXObject()
                      {
                          attached = true,
                          persistsOnDeath = true,
                          usesZHeight = false,
                          zHeight = 0f,
                          alignment = VFXAlignment.VelocityAligned,
                          destructible = false,
                          orphaned = true,
                          effect = Breakables.GenerateDebrisObject(Initialisation.GunDressingCollection, "pencilpusher_debris_001", true, 1, 1, 45, 20, null, 1, null, null, 1).gameObject,
                      },
                      new VFXObject()
                      {
                          attached = true,
                          persistsOnDeath = true,
                          usesZHeight = false,
                          zHeight = 0f,
                          alignment = VFXAlignment.VelocityAligned,
                          destructible = false,
                          orphaned = true,
                          effect = Breakables.GenerateDebrisObject(Initialisation.GunDressingCollection, "pencilpusher_debris_002", true, 1, 1, 45, 20, null, 1, null, null, 1).gameObject,
                      }
                }.ToArray(),
                }}.ToArray(),
            };
            projectile.hitEffects.HasProjectileDeathVFX = true;
            projectile.hitEffects.deathEnemy = debrisImpact;

            gun.DefaultModule.projectiles[0] = projectile;

            gun.carryPixelOffset = new IntVector2(8, -2);
            gun.carryPixelDownOffset = new IntVector2(-6, 0);
            gun.carryPixelUpOffset = new IntVector2(-6, 0);

            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(12) as Gun).muzzleFlashEffects;
            gun.gunHandedness = GunHandedness.AutoDetect;

            gun.AddClipSprites("pencilpusher");

            gun.quality = PickupObject.ItemQuality.B;
            ETGMod.Databases.Items.Add(gun, null, "ANY");

            gun.SetTag("arrow_bolt_weapon");
            ID = gun.PickupObjectId;


            pencilSketch = ProjectileSetupUtility.MakeProjectile(86, 0.5f);
            pencilSketch.objectImpactEventName = null;
            pencilSketch.enemyImpactEventName = null;
            pencilSketch.onDestroyEventName = null;
            pencilSketch.additionalStartEventName = null;
            pencilSketch.baseData.force *= 0.1f;
            pencilSketch.baseData.speed *= 0.0001f;
            pencilSketch.SetProjectileSprite("pencil_projectile", 4, 4, false);
            pencilSketch.hitEffects.enemy = null;
            pencilSketch.hitEffects.tileMapHorizontal = null;
            pencilSketch.hitEffects.tileMapVertical = null;
            pencilSketch.hitEffects.deathTileMapVertical = null;
            pencilSketch.hitEffects.deathTileMapHorizontal = null;
            pencilSketch.hitEffects.overrideMidairDeathVFX = null;
            pencilSketch.gameObject.GetOrAddComponent<DieWhenOwnerNotInRoom>();
        }
        public static int ID;
        public static Projectile pencilSketch;
        public class PencilSketcher : BraveBehaviour
        {
            float time = 0;
            private void Update()
            {
                if (base.projectile)
                {
                    if (time > 0.0001f)
                    {
                        Projectile sketch = ProjSpawnHelper.SpawnProjectileTowardsPoint(pencilSketch.gameObject, base.transform.Find("PencilTip").position, base.projectile.Direction, 0f, 0f).GetComponent<Projectile>();
                        sketch.Shooter = base.projectile.Shooter;
                        sketch.Owner = base.projectile.Owner;
                        if (base.projectile.ProjectilePlayerOwner())
                        {
                            sketch.ScaleByPlayerStats(base.projectile.ProjectilePlayerOwner());
                        }
                    }
                    else
                    {
                        time += BraveTime.DeltaTime;
                    }
                }
            }
        }
    }
}

