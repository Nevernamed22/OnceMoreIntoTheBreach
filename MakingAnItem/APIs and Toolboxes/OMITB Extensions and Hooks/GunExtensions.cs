using Alexandria.ItemAPI;
using Alexandria.Misc;
using Alexandria.SoundAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
   public static class GunExtensions
    {
        public static void AddCustomSwitchGroup(this Gun gun, string name, string fire, string reload)
        {
            gun.gunSwitchGroup = name;
            SoundManager.AddCustomSwitchData("WPN_Guns", gun.gunSwitchGroup, "Play_WPN_Gun_Reload_01", reload);
            SoundManager.AddCustomSwitchData("WPN_Guns", gun.gunSwitchGroup, "Play_WPN_Gun_Shot_01", fire);
        }
        public static float NormaliseProbabilityAcrossFirerate(this Gun gun, float activationsPerSecond, float minActivationChance, bool accountForVolley = false, float overrideBossroomActivPS = -1f)
        {
            float num = activationsPerSecond;
            if (gun && gun.GunPlayerOwner())
            {
                float num2 = 1f / gun.DefaultModule.cooldownTime;
                if (accountForVolley && gun.Volley != null && gun.Volley.UsesShotgunStyleVelocityRandomizer)
                {
                    num2 *= (float)gun.Volley.projectiles.Count;
                }
                num = Mathf.Clamp01(activationsPerSecond / num2);
                if (overrideBossroomActivPS != -1f && gun.GunPlayerOwner().CurrentRoom != null && gun.GunPlayerOwner().CurrentRoom.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.BOSS)
                {
                    num = Mathf.Clamp01(overrideBossroomActivPS / num2);
                }
                num = Mathf.Max(minActivationChance, num);
            }
            return num;
        }

        public static void AddExtraPermanentModules(this Gun g, PlayerController p, float cooldownModifier = 1, int numModules = 1, bool threesixtyspread = false)
        {
            ProjectileVolleyData vol = g.Volley.CopyVolley(0.7f, 1, 1, 1, 0.5f, 1, cooldownModifier, 1, 1, numModules, threesixtyspread);
            g.Volley = vol;
        }
        public static void PermanentlyRandomiseStats(this Gun g, PlayerController p)
        {
            g.reloadTime *= UnityEngine.Random.Range(0.5f, 2f);
            if (!g.InfiniteAmmo) g.SetBaseMaxAmmo(Mathf.Max(1, Mathf.CeilToInt(g.maxAmmo * UnityEngine.Random.Range(0.5f, 2f))));

            float damageModifier = UnityEngine.Random.Range(0.5f, 2f);
            float rangeModifier = UnityEngine.Random.Range(0.5f, 2f);
            float speedModifier = UnityEngine.Random.Range(0.5f, 2f);
            float forceModifier = UnityEngine.Random.Range(0.5f, 2f);
            float accuracyModifier = UnityEngine.Random.Range(0.5f, 2f);
            float burstCooldownModifier = UnityEngine.Random.Range(0.5f, 2f);
            float cooldownModifier = UnityEngine.Random.Range(0.5f, 2f);
            float clipSizeMultiplier = UnityEngine.Random.Range(0.5f, 2f);
            float bulletSizeMultiplier = UnityEngine.Random.Range(0.5f, 2f);

            ProjectileVolleyData vol = g.Volley.CopyVolley(damageModifier, rangeModifier, speedModifier, forceModifier, accuracyModifier, burstCooldownModifier, cooldownModifier, clipSizeMultiplier, bulletSizeMultiplier);
            g.Volley = vol;
        }
        public static ProjectileModule CopyModule(this ProjectileModule original, float damageModifier, float rangeModifier, float speedModifier, float forceModifier, float accuracyModifier, float burstCooldownModifier, float cooldownModifier, float clipSizeMultiplier, float bulletSizeMultiplier, bool clampVariance, bool noCost)
        {
            ProjectileModule newModule = new ProjectileModule();

            //Clone Clonable Module Parts
            newModule.shootStyle = original.shootStyle;
            newModule.ammoType = original.ammoType;
            newModule.customAmmoType = original.customAmmoType;
            newModule.sequenceStyle = original.sequenceStyle;
            newModule.orderedGroupCounts = original.orderedGroupCounts;
            newModule.maxChargeTime = original.maxChargeTime;
            newModule.triggerCooldownForAnyChargeAmount = original.triggerCooldownForAnyChargeAmount;
            newModule.isFinalVolley = original.isFinalVolley;
            newModule.usesOptionalFinalProjectile = original.usesOptionalFinalProjectile;
            newModule.numberOfFinalProjectiles = original.numberOfFinalProjectiles;
            newModule.finalAmmoType = original.finalAmmoType;
            newModule.finalCustomAmmoType = original.finalCustomAmmoType;
            newModule.angleFromAim = original.angleFromAim;
            newModule.alternateAngle = original.alternateAngle;
            newModule.positionOffset = original.positionOffset;
            newModule.mirror = original.mirror;
            newModule.inverted = original.inverted;
            newModule.ammoCost = noCost ? 0 : original.ammoCost;
            newModule.burstShotCount = original.burstShotCount;
            newModule.ignoredForReloadPurposes = original.ignoredForReloadPurposes;
            newModule.preventFiringDuringCharge = original.preventFiringDuringCharge;
            newModule.isExternalAddedModule = original.isExternalAddedModule;
            newModule.IsDuctTapeModule = original.IsDuctTapeModule;

            //Copy Altered Module Parts
            newModule.angleVariance = original.angleVariance * accuracyModifier;
            if (clampVariance) { newModule.angleVariance = Mathf.Max(newModule.angleVariance, 10f); }
            newModule.burstCooldownTime = original.burstCooldownTime * burstCooldownModifier;
            newModule.cooldownTime = original.cooldownTime * cooldownModifier;
            if (original.numberOfShotsInClip > 1) { newModule.numberOfShotsInClip = Mathf.CeilToInt((float)original.numberOfShotsInClip * clipSizeMultiplier); }

            //Projectile
            foreach (Projectile proj in original.projectiles)
            {
                newModule.projectiles.Add(CopyProjectile(proj, damageModifier, rangeModifier, speedModifier, forceModifier, bulletSizeMultiplier));
            }

            //Charged Projectiles
            foreach (ProjectileModule.ChargeProjectile origChargeModule in original.chargeProjectiles)
            {
                ProjectileModule.ChargeProjectile chargeModule = new ProjectileModule.ChargeProjectile();
                chargeModule.ChargeTime = origChargeModule.ChargeTime;
                chargeModule.UsedProperties = origChargeModule.UsedProperties;
                chargeModule.AmmoCost = origChargeModule.AmmoCost;
                chargeModule.VfxPool = origChargeModule.VfxPool;
                chargeModule.LightIntensity = origChargeModule.LightIntensity;
                chargeModule.ScreenShake = origChargeModule.ScreenShake;
                chargeModule.OverrideShootAnimation = origChargeModule.OverrideShootAnimation;
                chargeModule.OverrideMuzzleFlashVfxPool = origChargeModule.OverrideMuzzleFlashVfxPool;
                chargeModule.MegaReflection = origChargeModule.MegaReflection;
                chargeModule.AdditionalWwiseEvent = origChargeModule.AdditionalWwiseEvent;

                if (origChargeModule.Projectile)
                {
                    chargeModule.Projectile = CopyProjectile(origChargeModule.Projectile, damageModifier, rangeModifier, speedModifier, forceModifier, bulletSizeMultiplier);
                }
                newModule.chargeProjectiles.Add(chargeModule);
            }

            if (original.finalProjectile != null)
            {
                newModule.finalProjectile = CopyProjectile(original.finalProjectile, damageModifier, rangeModifier, speedModifier, forceModifier, bulletSizeMultiplier);
            }
            if (original.finalVolley != null)
            {
                newModule.finalVolley = CopyVolley(original.finalVolley, damageModifier, rangeModifier, speedModifier, forceModifier, accuracyModifier, burstCooldownModifier, cooldownModifier, clipSizeMultiplier, bulletSizeMultiplier);
            }
            return newModule;
        }
        private static ProjectileVolleyData CopyVolley(this ProjectileVolleyData original, float damageModifier, float rangeModifier, float speedModifier, float forceModifier, float accuracyModifier, float burstCooldownModifier, float cooldownModifier, float clipSizeMultiplier, float bulletSizeMultiplier, int Modules = 1, bool threeSixtyspread = false)
        {
            ProjectileVolleyData volley = new ProjectileVolleyData();
            volley.projectiles = new List<ProjectileModule>();
            volley.UsesBeamRotationLimiter = original.UsesBeamRotationLimiter;
            volley.BeamRotationDegreesPerSecond = original.BeamRotationDegreesPerSecond;
            volley.ModulesAreTiers = original.ModulesAreTiers;
            volley.UsesShotgunStyleVelocityRandomizer = original.UsesShotgunStyleVelocityRandomizer;
            volley.DecreaseFinalSpeedPercentMin = original.DecreaseFinalSpeedPercentMin;
            volley.IncreaseFinalSpeedPercentMax = original.IncreaseFinalSpeedPercentMax;
            foreach (ProjectileModule origModule in original.projectiles)
            {
                for (int i = 0; i < Modules; i++)
                {
                    ProjectileModule copiedModule = origModule.CopyModule(damageModifier, rangeModifier, speedModifier, forceModifier, accuracyModifier, burstCooldownModifier, cooldownModifier, clipSizeMultiplier, bulletSizeMultiplier, i > 0, i > 0);
                    if (threeSixtyspread) { copiedModule.angleVariance = 360f; }
                    volley.projectiles.Add(copiedModule);
                }
            }
            return volley;
        }
        private static Projectile CopyProjectile(Projectile original, float damageModifier, float rangeModifier, float speedModifier, float forceModifier, float bulletSizeMultiplier)
        {
            Projectile copy = original.InstantiateAndFakeprefab();
            copy.gameObject.SetActive(false);

            copy.baseData.damage *= damageModifier;
            copy.baseData.range *= rangeModifier;
            copy.baseData.speed *= speedModifier;
            copy.baseData.force *= forceModifier;
            copy.AdditionalScaleMultiplier = bulletSizeMultiplier;

            return copy;
        }
    }
}
