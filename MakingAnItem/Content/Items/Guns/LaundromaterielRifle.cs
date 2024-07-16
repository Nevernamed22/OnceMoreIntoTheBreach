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
using MonoMod.RuntimeDetour;
using System.Reflection;
using Alexandria.BreakableAPI;
using Dungeonator;
using Alexandria.Assetbundle;

namespace NevernamedsItems
{
    public class LaundromaterielRifle : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Laundromateriel Rifle", "laundromaterielrifle");
            Game.Items.Rename("outdated_gun_mods:laundromateriel_rifle", "nn:laundromateriel_rifle");
            gun.gameObject.AddComponent<LaundromaterielRifle>();
            gun.SetShortDescription("Washed Up");
            gun.SetLongDescription("This washing machine was stolen from the long lost communal washroom in the Breach- for use as a weapon within the Gungeon." + "\n\n Contains several prized artefacts, such as Winchesters lucky shorts.");

            gun.SetGunSprites("laundromaterielrifle");

            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(404) as Gun).gunSwitchGroup;
            gun.reloadTime = 0.8f;
            gun.SetBaseMaxAmmo(130);
            gun.ammo = 130;
            gun.gunHandedness = GunHandedness.HiddenOneHanded;
            gun.barrelOffset.transform.localPosition = new Vector3(18f / 16f, 6f / 16f, 0f);
            gun.gunClass = GunClass.SHOTGUN;
            gun.muzzleFlashEffects = VFXToolbox.CreateVFXPool("LaundromaterielRifle Muzzleflash",
                new List<string>()
                {
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/watersplash_muzzleflash_001",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/watersplash_muzzleflash_002",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/watersplash_muzzleflash_003",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/watersplash_muzzleflash_004",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/watersplash_muzzleflash_005",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/watersplash_muzzleflash_006",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/watersplash_muzzleflash_007",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/watersplash_muzzleflash_008",
                },
                17, //FPS
                new IntVector2(60, 60), //Dimensions
                tk2dBaseSprite.Anchor.MiddleLeft, //Anchor
                false, //Uses a Z height off the ground
                0, //The Z height, if used
                false,
               VFXAlignment.Fixed
                  );
            CustomClipAmmoTypeToolbox.AddCustomAmmoType("Laundromateriel Bullets", "NevernamedsItems/Resources/CustomGunAmmoTypes/laundromaterielrifle_clipfull", "NevernamedsItems/Resources/CustomGunAmmoTypes/laundromaterielrifle_clipempty");
            for (int i = 0; i < 4; i++)
            {
                gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
            }
            
            foreach (ProjectileModule mod in gun.Volley.projectiles)
            {
                mod.ammoCost = mod != gun.DefaultModule ? 0 : 1;

                mod.shootStyle = ProjectileModule.ShootStyle.Automatic;
                mod.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
                mod.cooldownTime = 0.34f;
                mod.angleVariance = 40f;
                mod.numberOfShotsInClip = 6;

                //BULLET STATS
                mod.projectiles[0] = MakeProj("yellowshirt", "yellowshirt", "LaundromaterielYellowShirt");
                mod.projectiles.Add(MakeProj("yellowshirt", "kinshirt", "LaundromaterielYellowShirt"));
                mod.projectiles.Add(MakeProj("pinkshirt", "pinkshirt", "LaundromaterielPinkShirt"));
                mod.projectiles.Add(MakeProj("redshirt", "redshirt", "LaundromaterielRedShirt"));
                mod.projectiles.Add(MakeProj("greenshirt", "greenshirt", "LaundromaterielGreenShirt"));
                mod.projectiles.Add(MakeProj("bluepants", "bluepants", "LaundromaterielBluePants"));
                mod.projectiles.Add(MakeProj("greypants", "greypants", "LaundromaterielGreyPants"));
                mod.projectiles.Add(MakeProj("whitepants", "whitepants", "LaundromaterielWhitePants"));
                mod.projectiles.Add(MakeProj("pinkshirt", "pinkpants", "LaundromaterielPinkShirt"));
            }
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = "Laundromateriel Bullets";
            gun.quality = PickupObject.ItemQuality.C;

            ETGMod.Databases.Items.Add(gun, false, "ANY");
            ID = gun.PickupObjectId;
        }
        public override void OnPostFired(PlayerController player, Gun gun)
        {
            if (player && player.CurrentRoom != null && player.CurrentRoom.HasActiveEnemies(RoomHandler.ActiveEnemyType.All) && player.PlayerHasActiveSynergy("Bloodwash"))
            {
                List<AIActor> toCheck = player.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
                for (int i = 0; i < toCheck.Count; i++)
                {
                    AIActor aiactor = toCheck[i];
                    if (aiactor)
                    {
                        if (((Vector2)aiactor.Position).PositionBetweenRelativeValidAngles(gun.barrelOffset.position, gun.CurrentAngle, 4, 60))
                        {
                            aiactor.ApplyEffect(new GameActorExsanguinationEffect() { duration = 3f });
                        }
                    }
                }
            }
            base.OnPostFired(player, gun);
        }
        private static Projectile MakeProj(string projName, string debrisname, string animationName)
        {
            Projectile projectile = ((Gun)PickupObjectDatabase.GetById(86)).DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            projectile.name = projName;
            projectile.baseData.speed *= 0.7f;
            projectile.baseData.range *= 2f;
            AnimateIndiv(projectile, animationName);

            GameObject shirt = BreakableAPIToolbox.GenerateDebrisObject($"NevernamedsItems/Resources/Debris/{debrisname}_debris.png", true, 1, 1, 45, 20).gameObject;
            shirt.name = debrisname;
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
        private static void AnimateIndiv(Projectile projectile, string animName)
        {
            projectile.AnimateProjectileBundle(animName,
                   Initialisation.ProjectileCollection,
                   Initialisation.projectileAnimationCollection,
                   animName,
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
