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

namespace NevernamedsItems
{

    public class DiamondGun : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Diamond Gun", "diamondgun");
            Game.Items.Rename("outdated_gun_mods:diamond_gun", "nn:diamond_gun");
            gun.gameObject.AddComponent<DiamondGun>();
            gun.SetShortDescription("Diamonds Toniiiight");
            gun.SetLongDescription("Made out of shimmering crystal, this sidearm was mined in one piece from the rock of the Black Powder Mines.");

            Alexandria.Assetbundle.GunInt.SetupSprite(gun, Initialisation.gunCollection, "diamondgun_idle_001", 8, "diamondgun_ammonomicon_001");

            gun.SetAnimationFPS(gun.shootAnimation, 15);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(53) as Gun).muzzleFlashEffects;

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1f;
            gun.DefaultModule.cooldownTime = 0.2f;
            gun.DefaultModule.numberOfShotsInClip = 6;
            gun.barrelOffset.transform.localPosition = new Vector3(1.5f, 0.87f, 0f);
            gun.SetBaseMaxAmmo(200);
            gun.gunClass = GunClass.PISTOL;
            //BULLET STATS
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.transform.parent = gun.barrelOffset;
            projectile.baseData.damage *= 3f;
            projectile.hitEffects.overrideMidairDeathVFX = (PickupObjectDatabase.GetById(506) as Gun).DefaultModule.projectiles[0].hitEffects.overrideMidairDeathVFX;
            projectile.hitEffects.alwaysUseMidair = true;

            ParticleShitter particles = projectile.gameObject.GetOrAddComponent<ParticleShitter>();
            particles.particleType = GlobalSparksDoer.SparksType.SOLID_SPARKLES;
            particles.particlesPerSecond = 20;

            projectile.SetProjectileSprite("diamond_projectile", 11, 11, false, tk2dBaseSprite.Anchor.MiddleCenter, 10, 10);
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = CustomClipAmmoTypeToolbox.AddCustomAmmoType("Diamond Gun Bullets", "NevernamedsItems/Resources/CustomGunAmmoTypes/diamondgun_clipfull", "NevernamedsItems/Resources/CustomGunAmmoTypes/diamondgun_clipempty");
            gun.quality = PickupObject.ItemQuality.A;
            ETGMod.Databases.Items.Add(gun, null, "ANY");

            DiamondGunID = gun.PickupObjectId;
            sparkle = VFXToolbox.CreateVFXBundle("DiamondSparkle", new IntVector2(7, 7), tk2dBaseSprite.Anchor.MiddleCenter, true, 0.4f);
        }
        public static int DiamondGunID;
        public static GameObject sparkle;
        public override void PostProcessProjectile(Projectile projectile)
        {
            try
            {
                PlayerController player = projectile.Owner as PlayerController;
                if (player.PlayerHasActiveSynergy("Bane of Arthropods")) projectile.OnHitEnemy += this.killArthropods;
                if (player.PlayerHasActiveSynergy("Smite")) projectile.OnHitEnemy += this.killUndead;
                if (player.PlayerHasActiveSynergy("Fire Aspect")) projectile.OnHitEnemy += this.applyFire;
                if (player.PlayerHasActiveSynergy("Sharpness")) projectile.baseData.damage *= 1.5f;
                if (player.PlayerHasActiveSynergy("Knockback")) projectile.baseData.force *= 2f;
                base.PostProcessProjectile(projectile);
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
                ETGModConsole.Log(e.StackTrace);
            }

        }
        protected override void OnPickedUpByPlayer(PlayerController player)
        {
            player.GunChanged += this.OnChangedGun;
            base.OnPickedUpByPlayer(player);
        }
        private void OnChangedGun(Gun oldGun, Gun newGun, bool huh)
        {
            if (newGun == gun && (gun.CurrentOwner as PlayerController).PlayerHasActiveSynergy("Looting"))
            {

            }
        }
        public override void OnDropped()
        {
            base.OnDropped();
            PlayerController player = gun.CurrentOwner as PlayerController;
            player.GunChanged -= this.OnChangedGun;
        }
        private float sparkleAccum;
        protected override void Update()
        {
            if (gun && gun.sprite)
            {
                sparkleAccum += BraveTime.DeltaTime * 3;
                if (sparkleAccum > 1f)
                {
                    int num = Mathf.FloorToInt(sparkleAccum);
                    sparkleAccum %= 1f;
                    Vector2 minpos = gun.sprite.sprite.WorldBottomLeft;
                    Vector2 maxpos = gun.sprite.sprite.WorldTopRight;
                    for (int i = 0; i < num; i++)
                    {
                        GameObject sparkleinst = UnityEngine.Object.Instantiate(sparkle, new Vector2(UnityEngine.Random.Range(minpos.x, maxpos.x), UnityEngine.Random.Range(minpos.y, maxpos.y)), Quaternion.identity);
                        sparkleinst.GetComponent<tk2dBaseSprite>().HeightOffGround = 0.2f;
                    }
                }
            }
            base.Update();
        }

        GameActorFireEffect fireEffect = Gungeon.Game.Items["hot_lead"].GetComponent<BulletStatusEffectItem>().FireModifierEffect;
        private void applyFire(Projectile bullet, SpeculativeRigidbody enemy, bool fatal)
        {
          if (enemy && enemy.gameActor)  enemy.gameActor.ApplyEffect(this.fireEffect, 1f, null);
        }
        private void killArthropods(Projectile bullet, SpeculativeRigidbody enemy, bool fatal)
        {
            if (enemy && enemy.aiActor && !string.IsNullOrEmpty(enemy.aiActor.EnemyGuid))
            {
                if (arthropods.Contains(enemy.aiActor.EnemyGuid))
                {
                    enemy.aiActor.healthHaver.ApplyDamage(1E+07f, Vector2.zero, "BaneOfArthropods", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                }
            }
        }
        private void killUndead(Projectile bullet, SpeculativeRigidbody enemy, bool fatal)
        {
            if (enemy && enemy.aiActor && !string.IsNullOrEmpty(enemy.aiActor.EnemyGuid))
            {
            if (undead.Contains(enemy.aiActor.EnemyGuid))
            {
                enemy.aiActor.healthHaver.ApplyDamage(1E+07f, Vector2.zero, "Smite", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
            }

            }
        }
        public static List<string> undead = new List<string>()
        {
            EnemyGuidDatabase.Entries["spent"],
            EnemyGuidDatabase.Entries["gummy"],
            EnemyGuidDatabase.Entries["skullet"],
            EnemyGuidDatabase.Entries["skullmet"],
            EnemyGuidDatabase.Entries["shelleton"],
            EnemyGuidDatabase.Entries["gummy_spent"],
            EnemyGuidDatabase.Entries["skusket"],
            EnemyGuidDatabase.Entries["revolvenant"],
        };
        public static List<string> arthropods = new List<string>()
        {
            EnemyGuidDatabase.Entries["phaser_spider"],
            EnemyGuidDatabase.Entries["shotgrub"],
            EnemyGuidDatabase.Entries["creech"],
        };
        public DiamondGun()
        {

        }
    }
}