using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;
using Dungeonator;
using SaveAPI;
using Alexandria.Assetbundle;
using Alexandria.Misc;

namespace NevernamedsItems
{
    public class HookGun : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Hook Gun", "hookgun");
            Game.Items.Rename("outdated_gun_mods:hook_gun", "nn:hook_gun");
            gun.gameObject.AddComponent<HookGun>();
            gun.SetShortDescription("Gadzooks");
            gun.SetLongDescription("An old device, though immaculately maintained."+"\n\nThe Executioners of the Gungeon Proper know its true purpose.");

            gun.SetGunSprites("hookgun", 8, false, 2);

            gun.SetAnimationFPS(gun.shootAnimation, 10);
            gun.SetAnimationFPS(gun.reloadAnimation, 10);
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.reloadAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;

            gun.AddPassiveStatModifier(PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE);

            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(124) as Gun).gunSwitchGroup;
            for (int i = 0; i < 2; i++)
            {
                gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
            }

            //GUN STATS
            gun.reloadTime = 1f;
            gun.SetBarrel(32, 8);
            gun.muzzleFlashEffects.type = VFXPoolType.None;

            gun.SetBaseMaxAmmo(400);
            gun.ammo = 400;
            gun.gunClass = GunClass.SILLY;

            foreach (ProjectileModule mod in gun.Volley.projectiles)
            {
                mod.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
                mod.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
                mod.cooldownTime = 0.25f;
                mod.numberOfShotsInClip = 10;
                mod.angleVariance = 0f;

                if (mod != gun.DefaultModule)
                {
                    //Hooks

                    Projectile projectile = DataCloners.CopyFields<TachyonProjectile>(Instantiate(mod.projectiles[0]));
                    projectile.gameObject.MakeFakePrefab();

                    projectile.baseData.damage = 10f;

                    projectile.specRigidbody.CollideWithTileMap = false;
                    projectile.m_ignoreTileCollisionsTimer = 1f;
                    projectile.pierceMinorBreakables = true;


                    SlowDownOverTimeModifier slowdown = projectile.gameObject.AddComponent<SlowDownOverTimeModifier>();
                    slowdown.doRandomTimeMultiplier = true;
                    slowdown.extendTimeByRangeStat = false;
                    slowdown.killAfterCompleteStop = false;
                    slowdown.targetSpeed = 0;
                    slowdown.timeToSlowOver = 1f;

                    projectile.gameObject.AddComponent<FakeBoomerangController>();

                    mod.ammoCost = 0;

                    projectile.AnimateProjectileBundle("HookGunProj",
                    Initialisation.ProjectileCollection,
                    Initialisation.projectileAnimationCollection,
                    "HookGunProj",
                    MiscTools.DupeList(new IntVector2(15, 15), 4), //Pixel Sizes
                    MiscTools.DupeList(true, 4), //Lightened
                    MiscTools.DupeList(tk2dBaseSprite.Anchor.MiddleCenter, 4), //Anchors
                    MiscTools.DupeList(true, 4), //Anchors Change Colliders
                    MiscTools.DupeList(false, 4), //Fixes Scales
                    MiscTools.DupeList<Vector3?>(null, 4), //Manual Offsets
                    MiscTools.DupeList<IntVector2?>(new IntVector2(13, 13), 4), //Override colliders
                    MiscTools.DupeList<IntVector2?>(null, 4), //Override collider offsets
                    MiscTools.DupeList<Projectile>(null, 4)); // Override to copy from

                    projectile.hitEffects.overrideMidairDeathVFX = (PickupObjectDatabase.GetById(417) as Gun).DefaultModule.projectiles[0].hitEffects.overrideMidairDeathVFX;
                    projectile.hitEffects.alwaysUseMidair = true;

                    mod.projectiles[0] = projectile;
                }
                else
                {
                    //Melee

                    Projectile projectile = ProjectileSetupUtility.MakeProjectile(86, 20f);

                    ModdedStatusEffectSlashBehaviour slash = projectile.gameObject.AddComponent<ModdedStatusEffectSlashBehaviour>();
                    slash.DestroyBaseAfterFirstSlash = true;
                    slash.slashParameters = ScriptableObject.CreateInstance<SlashData>();
                    slash.slashParameters.soundEvent = null;
                    slash.slashParameters.projInteractMode = SlashDoer.ProjInteractMode.IGNORE;
                    slash.SlashDamageUsesBaseProjectileDamage = true;
                    slash.slashParameters.doVFX = false;
                    slash.slashParameters.doHitVFX = true;
                    slash.slashParameters.slashRange = 1.8125f;
                    slash.slashParameters.slashDegrees = 10f;
                    slash.slashParameters.playerKnockbackForce = 0f;
                    slash.appliesExsanguination = true;

                    projectile.baseData.damage = 7f;

                    mod.ammoCost = 1;
                    mod.projectiles[0] = projectile;
                }
            }

            AdvancedVolleyModificationSynergyProcessor advVolley = gun.gameObject.AddComponent<AdvancedVolleyModificationSynergyProcessor>();
            AdvancedVolleyModificationSynergyData data = ScriptableObject.CreateInstance<AdvancedVolleyModificationSynergyData>();
            ProjectileModule cloned = ProjectileModule.CreateClone(gun.DefaultModule, false);
            cloned.projectiles[0] = ProjectileSetupUtility.MakeProjectile(86, 10f);
            cloned.projectiles[0].m_ignoreTileCollisionsTimer = 1f;
            cloned.projectiles[0].pierceMinorBreakables = true;
            SlowDownOverTimeModifier slowdown2 = cloned.projectiles[0].gameObject.AddComponent<SlowDownOverTimeModifier>();
            slowdown2.doRandomTimeMultiplier = true;
            slowdown2.extendTimeByRangeStat = false;
            slowdown2.killAfterCompleteStop = false;
            slowdown2.targetSpeed = 0;
            slowdown2.timeToSlowOver = 1f;
            cloned.projectiles[0].gameObject.AddComponent<FakeBoomerangController>();
            cloned.projectiles[0].AnimateProjectileBundle("HookGunProj",
            Initialisation.ProjectileCollection,
            Initialisation.projectileAnimationCollection,
            "HookGunProj",
            MiscTools.DupeList(new IntVector2(15, 15), 4), //Pixel Sizes
            MiscTools.DupeList(true, 4), //Lightened
            MiscTools.DupeList(tk2dBaseSprite.Anchor.MiddleCenter, 4), //Anchors
            MiscTools.DupeList(true, 4), //Anchors Change Colliders
            MiscTools.DupeList(false, 4), //Fixes Scales
            MiscTools.DupeList<Vector3?>(null, 4), //Manual Offsets
            MiscTools.DupeList<IntVector2?>(new IntVector2(13, 13), 4), //Override colliders
            MiscTools.DupeList<IntVector2?>(null, 4), //Override collider offsets
            MiscTools.DupeList<Projectile>(null, 4)); // Override to copy from
            cloned.projectiles[0].hitEffects.overrideMidairDeathVFX = (PickupObjectDatabase.GetById(417) as Gun).DefaultModule.projectiles[0].hitEffects.overrideMidairDeathVFX;
            cloned.projectiles[0].hitEffects.alwaysUseMidair = true;
            cloned.ammoCost = 0;
            data.AddsModules = true;
            data.ModulesToAdd = new List<ProjectileModule>() { cloned }.ToArray();
            data.RequiredSynergy = "Kalibers Hooks";
            advVolley.synergies.Add(data);

            gun.AddClipDebris(0, 1, "clipdebris_hookgun");
            gun.AddClipSprites("hookgun");

            gun.quality = PickupObject.ItemQuality.C;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            gun.AddToSubShop(ItemBuilder.ShopType.Cursula);

            ID = gun.PickupObjectId;
        }
        public static int ID;
        public class FakeBoomerangController : BraveBehaviour
        {
            private float cachedSpeed = 0f;
            private void Start()
            {
                cachedSpeed = base.projectile.baseData.speed;
                base.projectile.baseData.range = 100f;
                base.projectile.GetComponent<SlowDownOverTimeModifier>().OnCompleteStop += OnStop;
            }
            private void OnStop(Projectile self)
            {
                base.projectile.GetComponent<SlowDownOverTimeModifier>().OnCompleteStop -= OnStop;
                UnityEngine.Object.Destroy(base.projectile.GetComponent<SlowDownOverTimeModifier>());

                base.projectile.SendInDirection((base.projectile.Direction.ToAngle() + 180f).DegreeToVector2(), true);

                SlowDownOverTimeModifier speedUp = base.projectile.gameObject.AddComponent<SlowDownOverTimeModifier>();
                speedUp.doRandomTimeMultiplier = false;
                speedUp.extendTimeByRangeStat = false;
                speedUp.killAfterCompleteStop = false;
                speedUp.targetSpeed = cachedSpeed;
                speedUp.timeToSlowOver = 1f;
            }
        }
    }
}