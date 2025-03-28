﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.Misc;
using MonoMod.RuntimeDetour;
using System.Reflection;
using Alexandria.BreakableAPI;
using Dungeonator;
using Alexandria.Assetbundle;

namespace NevernamedsItems
{
    public class Bloodwash : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Bloodwash", "laundromaterielriflebloodwash");
            Game.Items.Rename("outdated_gun_mods:bloodwash", "nn:laundromateriel_rifle+bloodwash");
            gun.gameObject.AddComponent<LaundromaterielRifle>();
            gun.SetShortDescription("Washed Up");
            gun.SetLongDescription("This washing machine was stolen from the long lost communal washroom in the Breach- for use as a weapon within the Gungeon." + "\n\n Contains several prized artefacts, such as Winchesters lucky shorts.");

            gun.SetGunSprites("laundromaterielriflebloodwash", 8, true);

            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(404) as Gun).gunSwitchGroup;
            gun.reloadTime = 0.8f;
            gun.SetBaseMaxAmmo(130);
            gun.ammo = 130;
            gun.gunHandedness = GunHandedness.HiddenOneHanded;
            gun.barrelOffset.transform.localPosition = new Vector3(18f / 16f, 6f / 16f, 0f);
            gun.gunClass = GunClass.SHOTGUN;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(479) as Gun).muzzleFlashEffects;

            for (int i = 0; i < 5; i++)
            {
                gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
            }

            foreach (ProjectileModule mod in gun.Volley.projectiles)
            {
                mod.ammoCost = mod != gun.DefaultModule ? 0 : 1;

                mod.shootStyle = ProjectileModule.ShootStyle.Automatic;
                mod.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
                mod.cooldownTime = 0.34f;
                mod.angleVariance = 30f;
                mod.numberOfShotsInClip = 6;

                //BULLET STATS
                mod.projectiles[0] = MakeProj("redshirt", "bloodpants1");
                mod.projectiles.Add(MakeProj("redshirt", "bloodpants2"));
                mod.projectiles.Add(MakeProj("redshirt", "bloodshirt1"));
                mod.projectiles.Add(MakeProj("redshirt", "bloodshirt2"));

            }
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = "Laundromateriel Bullets";


            gun.quality = PickupObject.ItemQuality.EXCLUDED;

            ETGMod.Databases.Items.Add(gun, false, "ANY");
            ID = gun.PickupObjectId;
            gun.SetName("Laundromateriel Rifle");
        }

        private static Projectile MakeProj(string projName, string debrisname)
        {
            Projectile projectile = ((Gun)PickupObjectDatabase.GetById(86)).DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            projectile.name = projName;
            projectile.baseData.speed *= 0.7f;
            projectile.baseData.range *= 2f;
            AnimateIndiv(projectile, projName);

            GameObject shirt = BreakableAPIToolbox.GenerateDebrisObject($"NevernamedsItems/Resources/Debris/{debrisname}_debris.png", true, 1, 1, 45, 20).gameObject;
            shirt.name = debrisname;
            shirt.GetComponent<DebrisObject>().DoesGoopOnRest = true;
            shirt.GetComponent<DebrisObject>().GoopRadius = 1;
            shirt.GetComponent<DebrisObject>().AssignedGoop = EasyGoopDefinitions.BlobulonGoopDef;

            projectile.hitEffects = new ProjectileImpactVFXPool()
            {
                suppressHitEffectsIfOffscreen = false,
                suppressMidairDeathVfx = false,
                overrideMidairZHeight = -1,
                overrideEarlyDeathVfx = null,
                overrideMidairDeathVFX = shirt,
                midairInheritsVelocity = false,
                midairInheritsFlip = false,
                midairInheritsRotation = false,
                alwaysUseMidair = false,
                CenterDeathVFXOnProjectile = false,
                HasProjectileDeathVFX = true,
                enemy = new VFXPool()
                {
                    type = VFXPoolType.Single,
                    effects = new VFXComplex[]
                    {
                        new VFXComplex()
                        {
                            effects = new VFXObject[]
                            {
                                new VFXObject()
                                {
                                    alignment = VFXAlignment.Fixed,
                                    attached = true,
                                    destructible = false,
                                    orphaned = true,
                                    persistsOnDeath = false,
                                    usesZHeight = false,
                                    zHeight = 0,
                                    effect = ((Gun)PickupObjectDatabase.GetById(150)).DefaultModule.projectiles[0].hitEffects.enemy.effects[0].effects[0].effect,
                                }
                            }
                        }
                    }
                },
                deathEnemy = new VFXPool()
                {
                    type = VFXPoolType.Single,
                    effects = new VFXComplex[]
                    {
                        new VFXComplex()
                        {
                            effects = new VFXObject[]
                            {
                                new VFXObject()
                                {
                                    alignment = VFXAlignment.Fixed,
                                    attached = true,
                                    destructible = false,
                                    orphaned = true,
                                    persistsOnDeath = false,
                                    usesZHeight = false,
                                    zHeight = 0,
                                    effect = shirt,
                                }
                            }
                        }
                    }
                },
                deathTileMapHorizontal = new VFXPool()
                {
                    type = VFXPoolType.Single,
                    effects = new VFXComplex[]
                    {
                        new VFXComplex()
                        {
                            effects = new VFXObject[]
                            {
                                new VFXObject()
                                {
                                    alignment = VFXAlignment.Fixed,
                                    attached = true,
                                    destructible = false,
                                    orphaned = true,
                                    persistsOnDeath = false,
                                    usesZHeight = false,
                                    zHeight = 0,
                                    effect = shirt,
                                }
                            }
                        }
                    }
                },
                deathTileMapVertical = new VFXPool()
                {
                    type = VFXPoolType.Single,
                    effects = new VFXComplex[]
                    {
                        new VFXComplex()
                        {
                            effects = new VFXObject[]
                            {
                                new VFXObject()
                                {
                                    alignment = VFXAlignment.Fixed,
                                    attached = true,
                                    destructible = false,
                                    orphaned = true,
                                    persistsOnDeath = false,
                                    usesZHeight = false,
                                    zHeight = 0,
                                    effect = shirt,
                                }
                            }
                        }
                    }
                },
                deathAny = new VFXPool()
                {
                    type = VFXPoolType.None,
                    effects = new VFXComplex[]
                    {
                        new VFXComplex()
                        {
                            effects = new VFXObject[]
                            {
                                new VFXObject()
                                {
                                    alignment = VFXAlignment.Fixed,
                                    attached = false,
                                    destructible = false,
                                    orphaned = true,
                                    persistsOnDeath = false,
                                    usesZHeight = false,
                                    zHeight = 0,
                                    effect = shirt,
                                }
                            }
                        }
                    }
                },
                tileMapHorizontal = ((Gun)PickupObjectDatabase.GetById(28)).DefaultModule.projectiles[0].hitEffects.tileMapHorizontal,
                tileMapVertical = ((Gun)PickupObjectDatabase.GetById(28)).DefaultModule.projectiles[0].hitEffects.tileMapVertical,
            };
            return projectile;
        }
        private static void AnimateIndiv(Projectile projectile, string projName)
        {
            projectile.AnimateProjectileBundle("LaundromaterielRedShirt",
                   Initialisation.ProjectileCollection,
                   Initialisation.projectileAnimationCollection,
                   "LaundromaterielRedShirt",
                   MiscTools.DupeList(new IntVector2(12, 12), 8), //Pixel Sizes
                   MiscTools.DupeList(false, 8), //Lightened
                   MiscTools.DupeList(tk2dBaseSprite.Anchor.MiddleCenter, 8), //Anchors
                   MiscTools.DupeList(true, 8), //Anchors Change Colliders
                   MiscTools.DupeList(false, 8), //Fixes Scales
                   MiscTools.DupeList<Vector3?>(null, 8), //Manual Offsets
                   MiscTools.DupeList<IntVector2?>(null, 8), //Override colliders
                   MiscTools.DupeList<IntVector2?>(null, 8), //Override collider offsets
                   MiscTools.DupeList<Projectile>(null, 8)); // Override to copy from    
        }
        public static int ID;
    }
}
