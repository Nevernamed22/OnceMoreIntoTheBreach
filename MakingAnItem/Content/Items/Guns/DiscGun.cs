using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using MonoMod;
using Alexandria.ItemAPI;
using UnityEngine;
using System.Reflection;
using Alexandria.Assetbundle;

namespace NevernamedsItems
{

    public class DiscGun : AdvancedGunBehavior
    {
        public static int DiscGunID;

        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Disc Gun", "discgun2");
            Game.Items.Rename("outdated_gun_mods:disc_gun", "nn:disc_gun");
            var behav = gun.gameObject.AddComponent<DiscGun>();
            gun.SetShortDescription("Bad Choices");
            gun.SetLongDescription("Fires sharp, bouncing discs." + "\n\nCapable of hurting it's bearer, because someone thought that would be funny.");

            Alexandria.Assetbundle.GunInt.SetupSprite(gun, Initialisation.gunCollection, "discgun2_idle_001", 8, "discgun2_ammonomicon_001");

            gun.SetAnimationFPS(gun.shootAnimation, 14);
            gun.SetAnimationFPS(gun.reloadAnimation, 14);

            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(26) as Gun).muzzleFlashEffects;
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(12) as Gun).gunSwitchGroup;


            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1f;
            gun.DefaultModule.cooldownTime = 0.25f;
            gun.DefaultModule.numberOfShotsInClip = 10;
            gun.SetBaseMaxAmmo(300);
            gun.SetBarrel(19, 11);
            gun.gunClass = GunClass.PISTOL;

            //BULLET STATS
            Projectile projectile = ProjectileSetupUtility.MakeProjectile(86, 20f);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.range *= 20f;
            projectile.objectImpactEventName = "saw";
            projectile.baseData.speed *= 0.4f;

            CustomVFXTrail trail = projectile.gameObject.AddComponent<CustomVFXTrail>();
            trail.timeBetweenSpawns = 0.15f;
            trail.anchor = CustomVFXTrail.Anchor.Center;
            trail.VFX = VFXToolbox.CreateBlankVFXPool(VFXToolbox.CreateVFXBundle("DiscTrail", false, 0), true);
            trail.heightOffset = -1f;

            GameObject discDebris = Breakables.GenerateDebrisObject(Initialisation.GunDressingCollection, "disc_debris", true, 1, 1, 45, 20, null, 1, null, null, 1).gameObject;

            VFXPool plinkPool = (PickupObjectDatabase.GetById(97) as Gun).DefaultModule.projectiles[0].hitEffects.tileMapVertical;
            VFXPool debrisPool = VFXToolbox.CreateBlankVFXPool(discDebris, true);

            projectile.hitEffects.enemy = projectile.hitEffects.enemy = (PickupObjectDatabase.GetById(369) as Gun).DefaultModule.chargeProjectiles[0].Projectile.hitEffects.tileMapVertical;

            projectile.hitEffects.deathTileMapVertical = debrisPool;
            projectile.hitEffects.deathTileMapHorizontal = debrisPool;
            projectile.hitEffects.deathEnemy = debrisPool;
            projectile.hitEffects.deathAny = plinkPool;
            projectile.hitEffects.overrideMidairDeathVFX = discDebris;

            projectile.hitEffects.suppressHitEffectsIfOffscreen = false;
            projectile.hitEffects.suppressMidairDeathVfx = false;
            projectile.hitEffects.overrideMidairZHeight = -1;
            projectile.hitEffects.overrideEarlyDeathVfx = null;
            projectile.hitEffects.overrideMidairDeathVFX = discDebris;
            projectile.hitEffects.midairInheritsVelocity = false;
            projectile.hitEffects.midairInheritsFlip = false;
            projectile.hitEffects.midairInheritsRotation = false;
            projectile.hitEffects.alwaysUseMidair = false;
            projectile.hitEffects.CenterDeathVFXOnProjectile = false;
            projectile.hitEffects.HasProjectileDeathVFX = true;
            projectile.hitEffects.tileMapHorizontal = ((Gun)PickupObjectDatabase.GetById(97)).DefaultModule.projectiles[0].hitEffects.tileMapHorizontal;
            projectile.hitEffects.tileMapVertical = ((Gun)PickupObjectDatabase.GetById(97)).DefaultModule.projectiles[0].hitEffects.tileMapVertical;

            projectile.AnimateProjectileBundle("DiscProjectile",
                   Initialisation.ProjectileCollection,
                   Initialisation.projectileAnimationCollection,
                   "DiscProjectile",
                   MiscTools.DupeList(new IntVector2(15, 15), 8), //Pixel Sizes
                   MiscTools.DupeList(false, 8), //Lightened
                   MiscTools.DupeList(tk2dBaseSprite.Anchor.MiddleCenter, 8), //Anchors
                   MiscTools.DupeList(true, 8), //Anchors Change Colliders
                   MiscTools.DupeList(false, 8), //Fixes Scales
                   MiscTools.DupeList<Vector3?>(null, 8), //Manual Offsets
                   MiscTools.DupeList<IntVector2?>(new IntVector2(9, 9), 8), //Override colliders
                   MiscTools.DupeList<IntVector2?>(null, 8), //Override collider offsets
                   MiscTools.DupeList<Projectile>(null, 8)); // Override to copy from    

            SelfHarmBulletBehaviour SuicidalTendancies = projectile.gameObject.AddComponent<SelfHarmBulletBehaviour>();

            PierceProjModifier Piercing = projectile.gameObject.GetOrAddComponent<PierceProjModifier>();
            Piercing.penetratesBreakables = true;
            Piercing.penetration += 10;
            BounceProjModifier Bouncing = projectile.gameObject.GetOrAddComponent<BounceProjModifier>();
            Bouncing.numberOfBounces = 10;


            gun.AddClipSprites("discgun");
            gun.quality = PickupObject.ItemQuality.D;

            ETGMod.Databases.Items.Add(gun, null, "ANY");

            gun.AddToGunslingKingTable();

            DiscGunID = gun.PickupObjectId;
        }
        public override void PostProcessProjectile(Projectile projectile)
        {
            base.PostProcessProjectile(projectile);
            PlayerController playerController = projectile.Owner as PlayerController;
            if (playerController.PlayerHasActiveSynergy("Even Worse Choices")) projectile.baseData.damage *= 2f;
            if (playerController.PlayerHasActiveSynergy("Discworld"))
            {
                HomingModifier homing = projectile.gameObject.AddComponent<HomingModifier>();
                homing.AngularVelocity = 50f;
                homing.HomingRadius = 50f;
            }
        }
    }
    public class SelfHarmBulletBehaviour : MonoBehaviour
    {
        public SelfHarmBulletBehaviour()
        {
        }

        private void Awake()
        {
            try
            {
                //ETGModConsole.Log("Awake is being called");
                this.m_projectile = base.GetComponent<Projectile>();
                canDealDamage = false;
                Invoke("HandleCooldown", 1f);
                PlayerController playerController = this.m_projectile.Owner as PlayerController;
                //if (playerController.PlayerHasActiveSynergy("Even Worse Choices")) this.m_projectile.baseData.damage *= 2f;
                this.m_projectile.allowSelfShooting = true;
                this.m_projectile.collidesWithEnemies = true;
                this.m_projectile.collidesWithPlayer = true;
                this.m_projectile.SetNewShooter(this.m_projectile.Shooter);
                this.m_projectile.UpdateCollisionMask();
                this.m_projectile.specRigidbody.OnPreRigidbodyCollision += this.OnHitSelf;
                //ETGModConsole.Log("Awake finished");

            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
                ETGModConsole.Log(e.StackTrace);
            }
        }
        private Projectile m_projectile;
        private bool canDealDamage = true;
        private void OnHitSelf(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
        {
            try
            {
                //ETGModConsole.Log("OnHitSelf Triggered");
                bool isProtectedByDiscworld = false;
                float selfHarmDamageAmount = 0.5f;
                FieldInfo field = typeof(Projectile).GetField("m_hasPierced", BindingFlags.Instance | BindingFlags.NonPublic);
                field.SetValue(myRigidbody.projectile, false);
                PlayerController playerController = this.m_projectile.Owner as PlayerController;
                if (playerController.PlayerHasActiveSynergy("Even Worse Choices")) selfHarmDamageAmount = 1.0f;
                if (playerController.PlayerHasActiveSynergy("Discworld"))
                {
                    PlayableCharacters characterIdentity = playerController.characterIdentity;
                    if (playerController.ForceZeroHealthState)
                    {
                        if (playerController.healthHaver.Armor <= 1)
                        {
                            isProtectedByDiscworld = true;
                        }
                    }
                    else
                    {
                        if (playerController.healthHaver.Armor == 0 || playerController.healthHaver.NextDamageIgnoresArmor)
                        {
                            if (playerController.healthHaver.GetCurrentHealth() == 0.5f)
                            {
                                isProtectedByDiscworld = true;

                            }
                            else if (playerController.healthHaver.GetCurrentHealth() == 1f && playerController.PlayerHasActiveSynergy("Even Worse Choices"))
                            {
                                isProtectedByDiscworld = true;
                            }
                        }
                        else if (playerController.healthHaver.NextShotKills)
                        {
                            isProtectedByDiscworld = true;
                        }
                    }
                }
                PlayerController component = otherRigidbody.GetComponent<PlayerController>();
                if (component && !component.IsGhost)
                {
                    if (this.m_projectile.PossibleSourceGun)
                    {
                        if (canDealDamage && !component.IsDodgeRolling && !isProtectedByDiscworld)
                        {
                            component.healthHaver.ApplyDamage(selfHarmDamageAmount, Vector2.zero, "Disc Gun", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                            canDealDamage = false;
                            Invoke("HandleCooldown", 1f);
                        }
                        PhysicsEngine.SkipCollision = true;
                    }
                }

            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
                ETGModConsole.Log(e.StackTrace);
            }

        }
        private void HandleCooldown()
        {
            canDealDamage = true;
        }
        private void Update()
        {

        }
    }
}