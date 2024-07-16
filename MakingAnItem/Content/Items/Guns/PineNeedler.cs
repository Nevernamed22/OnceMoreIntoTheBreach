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
using Alexandria.BreakableAPI;

namespace NevernamedsItems
{

    public class PineNeedler : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Pine Needler", "pineneedler");
            Game.Items.Rename("outdated_gun_mods:pine_needler", "nn:pine_needler");
            gun.gameObject.AddComponent<PineNeedler>();
            gun.SetShortDescription("Wilding");
            gun.SetLongDescription("A miniature pine tree grown in an undiscovered, verdant chamber. Sometimes drops cones."+"\n\nThe words \"No Pine, No Gine\" are crudely carved into the bark.");

            gun.SetGunSprites("pineneedler");
            gun.SetAnimationFPS(gun.shootAnimation, 15);
            gun.SetAnimationFPS(gun.reloadAnimation, 10);

            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(124) as Gun).gunSwitchGroup;
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(56) as Gun, true, false);

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.1f;
            gun.DefaultModule.cooldownTime = 0.17f;
            gun.DefaultModule.numberOfShotsInClip = 20;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(339) as Gun).muzzleFlashEffects;
            gun.barrelOffset.transform.localPosition = new Vector3(32f/16f, 12f/16f, 0f);
            gun.SetBaseMaxAmmo(250);
            gun.gunClass = GunClass.EXPLOSIVE;

            //BULLET STATS
            Projectile projectile = gun.DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.speed *= 2f;
            projectile.SetProjectileSprite("pineneedler_proj", 8, 1, false, tk2dBaseSprite.Anchor.MiddleCenter, 5, 1);

            AdvancedFireOnReloadSynergyProcessor reloadfire = gun.gameObject.AddComponent<AdvancedFireOnReloadSynergyProcessor>();
            reloadfire.synergyToCheck = "Leafy Greens";
            reloadfire.angleVariance = 20f;
            reloadfire.numToFire = 7;
            reloadfire.projToFire = (PickupObjectDatabase.GetById(339) as Gun).Volley.projectiles[1].projectiles[0];

            GameObject shirt = BreakableAPIToolbox.GenerateDebrisObject($"NevernamedsItems/Resources/Debris/pineneedler_proj.png", true, 1, 1, 45, 20).gameObject;
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
                                    effect = ((Gun)PickupObjectDatabase.GetById(124)).DefaultModule.projectiles[0].hitEffects.enemy.effects[0].effects[0].effect,
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
                tileMapHorizontal = ((Gun)PickupObjectDatabase.GetById(124)).DefaultModule.projectiles[0].hitEffects.tileMapHorizontal,
                tileMapVertical = ((Gun)PickupObjectDatabase.GetById(124)).DefaultModule.projectiles[0].hitEffects.tileMapVertical,
            };

            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = CustomClipAmmoTypeToolbox.AddCustomAmmoType("Pine Needles", "NevernamedsItems/Resources/CustomGunAmmoTypes/pineneedler_clipfull", "NevernamedsItems/Resources/CustomGunAmmoTypes/pineneedler_clipempty");

            gun.quality = PickupObject.ItemQuality.B;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
             
            ID = gun.PickupObjectId;
        }
        public static int ID;

        public override void PostProcessProjectile(Projectile projectile)
        {
            if (gun && gun.GunPlayerOwner() && gun.GunPlayerOwner().PlayerHasActiveSynergy("Creatures of the Wood"))
            {
                AdvancedTransmogrifyBehaviour transmog = projectile.gameObject.GetOrAddComponent<AdvancedTransmogrifyBehaviour>();
                transmog.TransmogDataList.Add(new AdvancedTransmogrifyBehaviour.TransmogData()
                {
                    identifier = "Pine Needler",
                    TargetGuids = new List<string>() { "4254a93fc3c84c0dbe0a8f0dddf48a5a" },
                    maintainHPPercent = false,
                    TransmogChance = 0.1f
                });
            }
            base.PostProcessProjectile(projectile);
        }

        public int cones = 0;
        public override void OnReloadPressed(PlayerController player, Gun gun, bool manual)
        {
        cones = 0;
            base.OnReloadPressed(player, gun, manual);
        }
        public override Projectile OnPreFireProjectileModifier(Gun gun, Projectile projectile, ProjectileModule module)
        {
            if (cones < 3 && UnityEngine.Random.value <= 0.1f)
            {
                cones++;
                return (PickupObjectDatabase.GetById(339) as Gun).DefaultModule.projectiles[0];
            }
            return base.OnPreFireProjectileModifier(gun, projectile, module);
        }
    }
}