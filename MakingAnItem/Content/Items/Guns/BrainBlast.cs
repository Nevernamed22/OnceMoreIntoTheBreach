using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;
using MonoMod.RuntimeDetour;
using System.Reflection;
using SaveAPI;
using Alexandria.Assetbundle;
using Alexandria.Misc;

namespace NevernamedsItems
{
    public class BrainBlast : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Brain Blast", "brainblast");
            Game.Items.Rename("outdated_gun_mods:brain_blast", "nn:brain_blast");
            var behav = gun.gameObject.AddComponent<BrainBlast>();
            behav.preventNormalFireAudio = true;
            behav.preventNormalReloadAudio = true;
            gun.SetShortDescription("Mind Power");
            gun.SetLongDescription("The electrical impulses that power this floating brain appear to have been exponentially magnified by the Gungeons magics.\n\nDid it always exist, or did it spontaneously manifest in a chest one day? Who knows.");

            gun.doesScreenShake = false;

            gun.SetGunSprites("brainblast");
            gun.preventRotation = true;

            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(89) as Gun).gunSwitchGroup;

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);

            gun.gunHandedness = GunHandedness.NoHanded;

            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(36) as Gun).muzzleFlashEffects;

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 2f;
            gun.DefaultModule.cooldownTime = 0.06f;
            gun.DefaultModule.angleVariance = 360f;
            gun.DefaultModule.numberOfShotsInClip = -1;
            gun.barrelOffset.transform.localPosition = new Vector3(19f / 16f, 8f / 16f, 0f);
            gun.SetBaseMaxAmmo(2500);
            gun.ammo = 2500;
            gun.gunClass = GunClass.FULLAUTO;

            gun.shootAnimation = gun.idleAnimation;

            //BULLET STATS
            Projectile projectile = ProjectileUtility.SetupProjectile(86);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage = 1f;
            projectile.baseData.range *= 10f;
            projectile.baseData.force *= 0.05f;
            projectile.sprite.renderer.enabled = false;
            projectile.pierceMinorBreakables = true;
            projectile.PenetratesInternalWalls = true;

            RemoteBulletsProjectileBehaviour remote = projectile.gameObject.GetOrAddComponent<RemoteBulletsProjectileBehaviour>();
            ComplexProjectileModifier shockRounds = PickupObjectDatabase.GetById(298) as ComplexProjectileModifier;
            ChainLightningModifier shockroundsshocky = projectile.gameObject.GetOrAddComponent<ChainLightningModifier>();
            shockroundsshocky.LinkVFXPrefab = shockRounds.ChainLightningVFX;
            shockroundsshocky.damageTypes = shockRounds.ChainLightningDamageTypes;
            shockroundsshocky.maximumLinkDistance = 20;
            shockroundsshocky.damagePerHit = 2.5f;
            shockroundsshocky.damageCooldown = shockRounds.ChainLightningDamageCooldown;
            if (shockRounds.ChainLightningDispersalParticles != null)
            {
                shockroundsshocky.UsesDispersalParticles = true;
                shockroundsshocky.DispersalParticleSystemPrefab = shockRounds.ChainLightningDispersalParticles;
                shockroundsshocky.DispersalDensity = shockRounds.ChainLightningDispersalDensity;
                shockroundsshocky.DispersalMinCoherency = shockRounds.ChainLightningDispersalMinCoherence;
                shockroundsshocky.DispersalMaxCoherency = shockRounds.ChainLightningDispersalMaxCoherence;
            }
            else shockroundsshocky.UsesDispersalParticles = false;

            projectile.hitEffects.alwaysUseMidair = true;
            projectile.hitEffects.overrideMidairDeathVFX = (PickupObjectDatabase.GetById(18) as Gun).DefaultModule.projectiles[0].hitEffects.overrideMidairDeathVFX;

            projectile.transform.parent = gun.barrelOffset;
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.BEAM;

            gun.muzzleFlashEffects = VFXToolbox.CreateVFXPoolBundle("BrainBlastMuzzle", true, 0.4f, VFXAlignment.Fixed);
            gun.carryPixelOffset = new IntVector2(5, 20);

            gun.quality = PickupObject.ItemQuality.B;
            ETGMod.Databases.Items.Add(gun, null, "ANY");

            gun.AddToSubShop(ItemBuilder.ShopType.Goopton);
            ID = gun.PickupObjectId;
        }
        public static int ID;
        public bool isAudioLooping = false;

        protected override void Update()
        {
            if (gun)
            {
                if (gun.IsFiring && !isAudioLooping && gun.CurrentOwner) { AkSoundEngine.PostEvent("Play_ElectricSoundLoop", base.gameObject); isAudioLooping = true; }
                if (!gun.IsFiring && isAudioLooping) { AkSoundEngine.PostEvent("Stop_ElectricSoundLoop", base.gameObject); isAudioLooping = false; }
            }
            base.Update();
        }

        public override void PostProcessProjectile(Projectile projectile)
        {
            if (gun && gun.GunPlayerOwner() && gun.GunPlayerOwner().PlayerHasActiveSynergy("Dear God That Pig Can Do Algebra"))
            {
                foreach(PassiveItem item in gun.GunPlayerOwner().passiveItems)
                {
                    if ((item is CompanionItem) && (item as CompanionItem).ExtantCompanion && ((item as CompanionItem).CompanionGuid == "fe51c83b41ce4a46b42f54ab5f31e6d0" || (item as CompanionItem).CompanionGuid == "86237c6482754cd29819c239403a2de7"))
                    {
                        Projectile fired = base.gun.DefaultModule.projectiles[0].InstantiateAndFireInDirection((item as CompanionItem).ExtantCompanion.GetComponent<tk2dSprite>().WorldCenter, BraveUtility.RandomAngle(), 0f, null).GetComponent<Projectile>();
                        fired.Owner = gun.GunPlayerOwner();
                        fired.Shooter = gun.GunPlayerOwner().specRigidbody;
                        fired.ScaleByPlayerStats(gun.GunPlayerOwner());
                        gun.GunPlayerOwner().DoPostProcessProjectile(fired); 
                    }
                }
            }
            base.PostProcessProjectile(projectile);
        }
        public override void OnSwitchedAwayFromThisGun()
        {
            AkSoundEngine.PostEvent("Stop_ElectricSoundLoop", base.gameObject); isAudioLooping = false;
            base.OnSwitchedAwayFromThisGun();
        }
        public override void OnSwitchedToThisGun()
        {
            AkSoundEngine.PostEvent("Stop_ElectricSoundLoop", base.gameObject); isAudioLooping = false;
            base.OnSwitchedToThisGun();
        }
    }
}