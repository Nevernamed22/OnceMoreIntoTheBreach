using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;
using System.Reflection;

namespace NevernamedsItems
{
    class GoodMimic : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Good Mimic", "goodmimic");
            Game.Items.Rename("outdated_gun_mods:good_mimic", "nn:good_mimic");
            gun.gameObject.AddComponent<GoodMimic>();
            gun.SetShortDescription("All Grown Up");
            gun.SetLongDescription("Unlike most, this mimic thirsts for adventure rather than blood." + "\n\nBest to be polite though, just in case.");
            gun.SetupSprite(null, "goodmimic_idle_001", 8);

            gun.SetAnimationFPS(gun.shootAnimation, 10);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);

            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.1f;
            gun.gunHandedness = GunHandedness.AutoDetect;
            gun.DefaultModule.cooldownTime = 0.1f;
            gun.DefaultModule.numberOfShotsInClip = 6;
            gun.SetBaseMaxAmmo(300);
            // Here we just set the quality of the gun and the "EncounterGuid", which is used by Gungeon to identify the gun.
            gun.quality = PickupObject.ItemQuality.EXCLUDED; //C

            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage *= 1f;
            projectile.baseData.speed *= 1f;
            projectile.baseData.range *= 2f;

            gun.barrelOffset.transform.localPosition = new Vector3(1.0f, 0.37f, 0f);

            ETGMod.Databases.Items.Add(gun, null, "ANY");
            GoodMimicID = gun.PickupObjectId;
        }

        protected override void Update()
        {
            base.Update();
            if (this.gun.CurrentOwner == null)
            {
                this.TransformToTargetGunSpecial(PickupObjectDatabase.GetById(GoodMimicID) as Gun);
            }
        }

        protected override void OnPickedUpByPlayer(PlayerController player)
        {
            player.OnAnyEnemyReceivedDamage += this.ProjectileHitEnemy;
            base.OnPickedUpByPlayer(player);
        }
        protected override void OnPostDroppedByPlayer(PlayerController player)
        {
            player.OnAnyEnemyReceivedDamage -= this.ProjectileHitEnemy;
            base.OnPostDroppedByPlayer(player);
        }
        bool canChangeToBossWeapon = true;
        public void ProjectileHitEnemy(float damage, bool fatal, HealthHaver enemy)
        {
            CustomEnemyTagsSystem tags = enemy.gameObject.GetComponent<CustomEnemyTagsSystem>();
            if (tags != null && tags.ignoreForGoodMimic == true) { return; }
            if (enemy != null && enemy.aiActor != null && ((fatal && !enemy.IsBoss) || (enemy.IsBoss && canChangeToBossWeapon)))
            {

                int GunID = -1;
                if (enemy.aiActor.aiShooter != null && enemy.aiActor.aiShooter.CurrentGun != null)
                {
                    if (SpecialOverrideGuns.ContainsKey(enemy.aiActor.EnemyGuid)) { GunID = SpecialOverrideGuns[enemy.aiActor.EnemyGuid]; }
                    else { GunID = enemy.aiActor.aiShooter.CurrentGun.PickupObjectId; }
                }
                else if (Entries.ContainsKey(enemy.aiActor.EnemyGuid))
                {
                    GunID = Entries[enemy.aiActor.EnemyGuid];
                }
                if (GunID > 0)
                {
                    if (enemy.IsBoss)
                    {
                        canChangeToBossWeapon = false;
                        Invoke("ResetBossWeaponCooldown", 2.5f);
                    }
                    TransformToTargetGunSpecial((PickupObjectDatabase.GetById(GunID) as Gun));
                }
            }
        }
        private void ResetBossWeaponCooldown() { canChangeToBossWeapon = true; }
        public void TransformToTargetGunSpecial(Gun targetGun)
        {
            int clipShotsRemaining = this.gun.ClipShotsRemaining;
            if (((VFXPool)m_currentlyPlayingChargeVFX_info.GetValue(this.gun)) != null)
            {
                ((VFXPool)m_currentlyPlayingChargeVFX_info.GetValue(this.gun)).DestroyAll();
                m_currentlyPlayingChargeVFX_info.SetValue(this.gun, null);
            }
            ProjectileVolleyData volley = this.gun.Volley;
            this.gun.RawSourceVolley = targetGun.RawSourceVolley;
            this.gun.singleModule = targetGun.singleModule;
            this.gun.modifiedVolley = null;
            if (targetGun.sprite)
            {
                this.gun.DefaultSpriteID = targetGun.sprite.spriteId;
                this.gun.GetSprite().SetSprite(targetGun.sprite.Collection, this.gun.DefaultSpriteID);
                if (base.spriteAnimator && targetGun.spriteAnimator)
                {
                    base.spriteAnimator.Library = targetGun.spriteAnimator.Library;
                }
                tk2dSpriteDefinition.AttachPoint[] attachPoints = this.gun.GetSprite().Collection.GetAttachPoints(this.gun.DefaultSpriteID);
                tk2dSpriteDefinition.AttachPoint attachPoint;
                if (attachPoints != null)
                {
                    attachPoint = Array.Find(attachPoints, (tk2dSpriteDefinition.AttachPoint a) => a.name == "PrimaryHand");
                }
                else
                {
                    attachPoint = null;
                }
                tk2dSpriteDefinition.AttachPoint attachPoint2 = attachPoint;
                if (attachPoint2 != null)
                {
                    m_defaultLocalPosition_info.SetValue(this.gun, -attachPoint2.position);
                }
            }
            if (targetGun.GetBaseMaxAmmo() != this.gun.GetBaseMaxAmmo() && targetGun.GetBaseMaxAmmo() > 0)
            {
                int num = (!this.gun.InfiniteAmmo) ? this.gun.AdjustedMaxAmmo : this.gun.GetBaseMaxAmmo();
                this.gun.SetBaseMaxAmmo(targetGun.GetBaseMaxAmmo());
                if (this.gun.AdjustedMaxAmmo > 0 && num > 0 && this.gun.ammo > 0 && !this.gun.InfiniteAmmo)
                {
                    this.gun.ammo = Mathf.FloorToInt((float)this.gun.ammo / (float)num * (float)this.gun.AdjustedMaxAmmo);
                    this.gun.ammo = Mathf.Min(this.gun.ammo, this.gun.AdjustedMaxAmmo);
                }
                else
                {
                    this.gun.ammo = Mathf.Min(this.gun.ammo, this.gun.GetBaseMaxAmmo());
                }
            }
            this.gun.gunSwitchGroup = targetGun.gunSwitchGroup;
            this.gun.isAudioLoop = targetGun.isAudioLoop;
            this.gun.gunClass = targetGun.gunClass;
            if (!string.IsNullOrEmpty(this.gun.gunSwitchGroup))
            {
                AkSoundEngine.SetSwitch("WPN_Guns", this.gun.gunSwitchGroup, base.gameObject);
            }
            this.gun.currentGunDamageTypeModifiers = targetGun.currentGunDamageTypeModifiers;
            this.gun.carryPixelOffset = targetGun.carryPixelOffset;
            this.gun.carryPixelUpOffset = targetGun.carryPixelUpOffset;
            this.gun.carryPixelDownOffset = targetGun.carryPixelDownOffset;
            this.gun.leftFacingPixelOffset = targetGun.leftFacingPixelOffset;
            this.gun.UsesPerCharacterCarryPixelOffsets = targetGun.UsesPerCharacterCarryPixelOffsets;
            this.gun.PerCharacterPixelOffsets = targetGun.PerCharacterPixelOffsets;
            this.gun.gunPosition = targetGun.gunPosition;
            this.gun.forceFlat = targetGun.forceFlat;
            if (targetGun.GainsRateOfFireAsContinueAttack != this.gun.GainsRateOfFireAsContinueAttack)
            {
                this.gun.GainsRateOfFireAsContinueAttack = targetGun.GainsRateOfFireAsContinueAttack;
                this.gun.RateOfFireMultiplierAdditionPerSecond = targetGun.RateOfFireMultiplierAdditionPerSecond;
            }
            if (this.gun.barrelOffset && targetGun.barrelOffset)
            {
                this.gun.barrelOffset.localPosition = targetGun.barrelOffset.localPosition;
                m_originalBarrelOffsetPosition_info.SetValue(this.gun, targetGun.barrelOffset.localPosition);
            }
            if (this.gun.muzzleOffset && targetGun.muzzleOffset)
            {
                this.gun.muzzleOffset.localPosition = targetGun.muzzleOffset.localPosition;
                m_originalMuzzleOffsetPosition_info.SetValue(this.gun, targetGun.muzzleOffset.localPosition);
            }
            if (this.gun.chargeOffset && targetGun.chargeOffset)
            {
                this.gun.chargeOffset.localPosition = targetGun.chargeOffset.localPosition;
                m_originalChargeOffsetPosition_info.SetValue(this.gun, targetGun.chargeOffset.localPosition);
            }
            this.gun.reloadTime = targetGun.reloadTime;
            this.gun.blankDuringReload = targetGun.blankDuringReload;
            this.gun.blankReloadRadius = targetGun.blankReloadRadius;
            this.gun.reflectDuringReload = targetGun.reflectDuringReload;
            this.gun.blankKnockbackPower = targetGun.blankKnockbackPower;
            this.gun.blankDamageToEnemies = targetGun.blankDamageToEnemies;
            this.gun.blankDamageScalingOnEmptyClip = targetGun.blankDamageScalingOnEmptyClip;
            this.gun.doesScreenShake = targetGun.doesScreenShake;
            this.gun.gunScreenShake = targetGun.gunScreenShake;
            this.gun.directionlessScreenShake = targetGun.directionlessScreenShake;
            this.gun.AppliesHoming = targetGun.AppliesHoming;
            this.gun.AppliedHomingAngularVelocity = targetGun.AppliedHomingAngularVelocity;
            this.gun.AppliedHomingDetectRadius = targetGun.AppliedHomingDetectRadius;
            this.gun.GoopReloadsFree = targetGun.GoopReloadsFree;
            this.gun.gunHandedness = targetGun.gunHandedness;
            m_cachedGunHandedness_info.SetValue(this.gun, null);
            this.gun.shootAnimation = targetGun.shootAnimation;
            this.gun.usesContinuousFireAnimation = targetGun.usesContinuousFireAnimation;
            this.gun.reloadAnimation = targetGun.reloadAnimation;
            this.gun.emptyReloadAnimation = targetGun.emptyReloadAnimation;
            this.gun.idleAnimation = targetGun.idleAnimation;
            this.gun.chargeAnimation = targetGun.chargeAnimation;
            this.gun.dischargeAnimation = targetGun.dischargeAnimation;
            this.gun.emptyAnimation = targetGun.emptyAnimation;
            this.gun.introAnimation = targetGun.introAnimation;
            this.gun.finalShootAnimation = targetGun.finalShootAnimation;
            this.gun.enemyPreFireAnimation = targetGun.enemyPreFireAnimation;
            this.gun.dodgeAnimation = targetGun.dodgeAnimation;
            this.gun.muzzleFlashEffects = targetGun.muzzleFlashEffects;
            this.gun.usesContinuousMuzzleFlash = targetGun.usesContinuousMuzzleFlash;
            this.gun.finalMuzzleFlashEffects = targetGun.finalMuzzleFlashEffects;
            this.gun.reloadEffects = targetGun.reloadEffects;
            this.gun.emptyReloadEffects = targetGun.emptyReloadEffects;
            this.gun.activeReloadSuccessEffects = targetGun.activeReloadSuccessEffects;
            this.gun.activeReloadFailedEffects = targetGun.activeReloadFailedEffects;
            this.gun.shellCasing = targetGun.shellCasing;
            this.gun.shellsToLaunchOnFire = targetGun.shellsToLaunchOnFire;
            this.gun.shellCasingOnFireFrameDelay = targetGun.shellCasingOnFireFrameDelay;
            this.gun.shellsToLaunchOnReload = targetGun.shellsToLaunchOnReload;
            this.gun.reloadShellLaunchFrame = targetGun.reloadShellLaunchFrame;
            this.gun.clipObject = targetGun.clipObject;
            this.gun.clipsToLaunchOnReload = targetGun.clipsToLaunchOnReload;
            this.gun.reloadClipLaunchFrame = targetGun.reloadClipLaunchFrame;
            this.gun.IsTrickGun = targetGun.IsTrickGun;
            this.gun.TrickGunAlternatesHandedness = targetGun.TrickGunAlternatesHandedness;
            this.gun.alternateVolley = targetGun.alternateVolley;
            this.gun.alternateShootAnimation = targetGun.alternateShootAnimation;
            this.gun.alternateReloadAnimation = targetGun.alternateReloadAnimation;
            this.gun.alternateIdleAnimation = targetGun.alternateIdleAnimation;
            this.gun.alternateSwitchGroup = targetGun.alternateSwitchGroup;
            this.gun.rampBullets = targetGun.rampBullets;
            this.gun.rampStartHeight = targetGun.rampStartHeight;
            this.gun.rampTime = targetGun.rampTime;
            this.gun.usesDirectionalAnimator = targetGun.usesDirectionalAnimator;
            this.gun.usesDirectionalIdleAnimations = targetGun.usesDirectionalIdleAnimations;
            Component[] targetGunComponents = targetGun.GetComponents<Component>();
            Component[] thisGunComponents = this.GetComponents<Component>();
            foreach (Component targetGunComponent in targetGunComponents)
            {
                if (this.gun.GetComponent(targetGunComponent.GetType()) == null)
                {
                    Component comp = this.gun.gameObject.AddComponent(targetGunComponent.GetType());
                    comp.SetFields(targetGunComponent, includeProperties: true, includeFields: true);
                }
            }
            foreach (Component thisGunComponent in thisGunComponents)
            {
                if (thisGunComponent != this && targetGun.GetComponent(thisGunComponent.GetType()) == null)
                {
                    Destroy(thisGunComponent);
                }
            }
            if (base.aiAnimator)
            {
                Destroy(base.aiAnimator);
                base.aiAnimator = null;
            }
            if (targetGun.aiAnimator)
            {
                AIAnimator aianimator = base.gameObject.AddComponent<AIAnimator>();
                AIAnimator aiAnimator = targetGun.aiAnimator;
                aianimator.facingType = aiAnimator.facingType;
                aianimator.DirectionParent = aiAnimator.DirectionParent;
                aianimator.faceSouthWhenStopped = aiAnimator.faceSouthWhenStopped;
                aianimator.faceTargetWhenStopped = aiAnimator.faceTargetWhenStopped;
                aianimator.directionalType = aiAnimator.directionalType;
                aianimator.RotationQuantizeTo = aiAnimator.RotationQuantizeTo;
                aianimator.RotationOffset = aiAnimator.RotationOffset;
                aianimator.ForceKillVfxOnPreDeath = aiAnimator.ForceKillVfxOnPreDeath;
                aianimator.SuppressAnimatorFallback = aiAnimator.SuppressAnimatorFallback;
                aianimator.IsBodySprite = aiAnimator.IsBodySprite;
                aianimator.IdleAnimation = aiAnimator.IdleAnimation;
                aianimator.MoveAnimation = aiAnimator.MoveAnimation;
                aianimator.FlightAnimation = aiAnimator.FlightAnimation;
                aianimator.HitAnimation = aiAnimator.HitAnimation;
                aianimator.OtherAnimations = aiAnimator.OtherAnimations;
                aianimator.OtherVFX = aiAnimator.OtherVFX;
                aianimator.OtherScreenShake = aiAnimator.OtherScreenShake;
                aianimator.IdleFidgetAnimations = aiAnimator.IdleFidgetAnimations;
                base.aiAnimator = aianimator;
            }
            MultiTemporaryOrbitalSynergyProcessor component = targetGun.GetComponent<MultiTemporaryOrbitalSynergyProcessor>();
            MultiTemporaryOrbitalSynergyProcessor component2 = base.GetComponent<MultiTemporaryOrbitalSynergyProcessor>();
            if (!component && component2)
            {
                Destroy(component2);
            }
            else if (component && !component2)
            {
                MultiTemporaryOrbitalSynergyProcessor multiTemporaryOrbitalSynergyProcessor = base.gameObject.AddComponent<MultiTemporaryOrbitalSynergyProcessor>();
                multiTemporaryOrbitalSynergyProcessor.RequiredSynergy = component.RequiredSynergy;
                multiTemporaryOrbitalSynergyProcessor.OrbitalPrefab = component.OrbitalPrefab;
            }
            if (this.gun.RawSourceVolley != null)
            {
                for (int i = 0; i < this.gun.RawSourceVolley.projectiles.Count; i++)
                {
                    this.gun.RawSourceVolley.projectiles[i].ResetRuntimeData();
                }
            }
            else
            {
                this.gun.singleModule.ResetRuntimeData();
            }
            if (volley != null)
            {
                this.gun.RawSourceVolley = DuctTapeItem.TransferDuctTapeModules(volley, this.gun.RawSourceVolley, this.gun);
            }
            if (this.gun.CurrentOwner is PlayerController)
            {
                PlayerController playerController = this.gun.CurrentOwner as PlayerController;
                if (playerController.stats != null)
                {
                    playerController.stats.RecalculateStats(playerController, false, false);
                }
            }
            if (base.gameObject.activeSelf)
            {
                base.StartCoroutine((IEnumerator)HandleFrameDelayedTransformation_info.Invoke(this.gun, new object[0]));
            }
            this.gun.DidTransformGunThisFrame = true;
        }

        private static FieldInfo m_currentlyPlayingChargeVFX_info = typeof(Gun).GetField("m_currentlyPlayingChargeVFX", BindingFlags.NonPublic | BindingFlags.Instance);
        private static FieldInfo m_defaultLocalPosition_info = typeof(Gun).GetField("m_defaultLocalPosition", BindingFlags.NonPublic | BindingFlags.Instance);
        private static FieldInfo m_originalBarrelOffsetPosition_info = typeof(Gun).GetField("m_originalBarrelOffsetPosition", BindingFlags.NonPublic | BindingFlags.Instance);
        private static FieldInfo m_originalMuzzleOffsetPosition_info = typeof(Gun).GetField("m_originalMuzzleOffsetPosition", BindingFlags.NonPublic | BindingFlags.Instance);
        private static FieldInfo m_originalChargeOffsetPosition_info = typeof(Gun).GetField("m_originalChargeOffsetPosition", BindingFlags.NonPublic | BindingFlags.Instance);
        private static FieldInfo m_cachedGunHandedness_info = typeof(Gun).GetField("m_cachedGunHandedness", BindingFlags.NonPublic | BindingFlags.Instance);
        private static MethodInfo HandleFrameDelayedTransformation_info = typeof(Gun).GetMethod("HandleFrameDelayedTransformation", BindingFlags.NonPublic | BindingFlags.Instance);
        public static int GoodMimicID;

        public static Dictionary<string, int> Entries { get; set; } = new Dictionary<string, int>()
        {
             {GUIDs.Shelleton, 464}, //Shelleton --> Shellegun
             {GUIDs.Gunzookie, 599},
             {GUIDs.Gunzockie, 599},
             {GUIDs.Gigi, 445}, //Gigi --> Scrambler
             {GUIDs.Parrot, 445}, //Parrot --> Scrambler
             {GUIDs.Skusket, 45}, //Skusket --> Skull Spitter
             {GUIDs.Black_Skusket, 45}, //Black Skusket --> Skull Spitter
             {GUIDs.Skusket_Head, 45}, //Skusket Head --> Skull Spitter
             {GUIDs.Muzzle_Wisp, 125}, //Muzzle Wisp --> Flame Hand
             {GUIDs.Muzzle_Flare, 698}, //Muzzle Flare --> Flame Hand+Maximize Spell
             {GUIDs.Pinhead, 19}, //Pinhead --> Grenade Launcher
             {GUIDs.Mountain_Cube, 130}, //Mountain Cube --> Glacier
             {GUIDs.Shambling_Round, 23}, //Shambing Round --> Dungeon Eagle
             {GUIDs.Fuselier, 332}, //Fuselier --> Lil' Bomber
             {GUIDs.Bullet_Shark, Gunshark.GunsharkID}, //Bullet Shark --> Gunshark
             {GUIDs.Bookllet, Bookllet.BooklletID}, //Bookllet --> Bookllet
             {GUIDs.Blue_Bookllet, Bookllet.BooklletID},
             {GUIDs.Green_Bookllet, Bookllet.BooklletID},
             {GUIDs.Angry_Necronomicon, Bookllet.BooklletID},
             {GUIDs.Bombshee, 3}, // Bombshee --> Screecher
             {GUIDs.Bullet_King, 551}, // Bullet King --> Crown of Guns
             {GUIDs.Gatling_Gull, 84}, // Gatling Gull --> Vulcan Cannon
             {GUIDs.Treadnaught, 486}, // Treadnaught --> Treadnaught Barrel
             {GUIDs.Cannonbalrog, 37}, // Cannonbalrog --> Serious Cannon
             {GUIDs.Dragun, 146}, // Dragun --> Dragunfire
             {GUIDs.Advanced_Dragun, 670}, // Advanced Dragun --> High Dragunfire
             {GUIDs.Helicopter_Agunim, 707}, // Advanced Dragun --> Vulcan Cannon
 
             {GUIDs.Tazie, StunGun.StunGunID}, // Tazie --> Stun Gun
             {GUIDs.Gun_Nut, BulletBlade.BulletBladeID}, //Gun Nut --> Bullet Blade
             {GUIDs.Spectral_Gun_Nut, BulletBladeGhostForme.GhostBladeID }, //Gun Nut --> Bullet Blade
             {GUIDs.Confirmed, 761}, //Confirmed --> High Kaliber
             {GUIDs.Fungun, FungoCannon.FungoCannonID}, //Fungun --> Fungo Cannon
             {GUIDs.Spogre, FungoCannon.FungoCannonID}, //Spogre --> Fungo Cannon
             {GUIDs.Keybullet_Kin, Rekeyter.RekeyterID}, //Keybullet Kin --> Rekeyter  
             {GUIDs.Lore_Gunjurer, Lorebook.LorebookID}, //Lore Gunjurer --> Lorebook  
             {GUIDs.Blizzbulon, Icicle.IcicleID}, //Blizzbulon --> Icicle
             {GUIDs.Phaser_Spider, PhaserSpiderling.PhaserSpiderlingID}, //Phaser Spider --> Phaser Spiderling
             {GUIDs.Lead_Maiden, MaidenRifle.MaidenRifleID}, //Lead Maiden --> Maiden Rifle
             {GUIDs.Bullat, Bullatterer.BullattererID}, //Bullat --> Bullatterer
             {GUIDs.Spirat, Bullatterer.BullattererID}, //Spirat --> Bullatterer
             {GUIDs.Grenat, Bullatterer.BullattererID}, //Grenat --> Bullatterer
             {GUIDs.Shotgat, Bullatterer.BullattererID}, //Shotgat --> Bullatterer
             {GUIDs.King_Bullat, KingBullatterer.KingBullattererID}, //King Bullat --> Bullatterer+King Bullatterer
             {GUIDs.Bullat_Gargoyle, KingBullatterer.KingBullattererID}, //Gargoyle --> Bullatterer+King Bullatterer
             {GUIDs.Nitra, DynamiteLauncher.DynamiteLauncherID}, //Nitra --> Dynamite launcher
             {GUIDs.Misfire_Beast, Beastclaw.BeastclawID}, //Misfire Beast --> Beastclaw


             {GUIDs.Expand.Com4nd0_Boss, HeavyAssaultRifle.HeavyAssaultRifleID}, //Com4nd0 Boss --> Heavy Assault Rifle
             {GUIDs.Expand.Parasitic_Abomination, 333}, //Parasitic Abomination --> Mutation
             {GUIDs.Expand.Cronenberg, 333}, //Cronenberg --> Mutation
             {GUIDs.Expand.Aggressive_Cronenberg, 333}, //Cronenberg --> Mutation
 
 
             {GUIDs.Great_Bullet_Shark, GunsharkMegasharkSynergyForme.GunsharkMegasharkSynergyFormeID}, //Great Bullet Shark --> Gunshark+Megashark
             
 
        };
        public static Dictionary<string, int> SpecialOverrideGuns { get; set; } = new Dictionary<string, int>()
        {
             {GUIDs.Shroomer, ShroomedGun.ShroomedGunID}, //Shroomer --> ShroomedGun             
        };
    }
}