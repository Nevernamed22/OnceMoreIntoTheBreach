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

namespace NevernamedsItems
{
    public class TitanSlayer : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Titan Slayer", "titanslayer");
            Game.Items.Rename("outdated_gun_mods:titan_slayer", "nn:titan_slayer");
            var behav = gun.gameObject.AddComponent<TitanSlayer>();
            gun.SetShortDescription("A High Toll Indeed");
            gun.SetLongDescription("An elegant bow crafted to kill Titans on a distant world."+"\n\nDespite it's masterful craftsmanship, it seems its true power lies with its single, magic arrow...");

            gun.SetGunSprites("titanslayer");

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(56) as Gun, true, false);
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(8) as Gun).gunSwitchGroup;

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.doesScreenShake = true;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Charged;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;

            gun.reloadTime = 3f;
            gun.DefaultModule.cooldownTime = 1f;
            gun.DefaultModule.angleVariance = 0;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(8) as Gun).muzzleFlashEffects;

            gun.DefaultModule.numberOfShotsInClip = 1;
            gun.barrelOffset.transform.localPosition = new Vector3(19f / 16f, 15f / 16f, 0f);
            gun.SetBaseMaxAmmo(100);
            gun.ammo = 100;
            gun.gunClass = GunClass.CHARGE;

            gun.SetAnimationFPS(gun.chargeAnimation, 16);
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.chargeAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.chargeAnimation).loopStart = 5;

            gun.carryPixelOffset = new IntVector2(15, 0);
            gun.carryPixelUpOffset = new IntVector2(0, 8);
            gun.carryPixelDownOffset = new IntVector2(0, -8);

            //BULLET STATS
            Projectile projectile = gun.DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            projectile.SetProjectileSprite("titanslayer_proj", 21, 7, false, tk2dBaseSprite.Anchor.MiddleCenter, 17, 3);
            projectile.baseData.speed *= 2;
            projectile.baseData.range = float.MaxValue;
            projectile.baseData.damage = 33;
            projectile.BossDamageMultiplier = 1.4f;
            projectile.pierceMinorBreakables = true;
            NoCollideBehaviour nocol = projectile.gameObject.AddComponent<NoCollideBehaviour>();
            nocol.worksOnEnemies = false;
            nocol.worksOnProjectiles = false;
            projectile.hitEffects.enemy = (PickupObjectDatabase.GetById(535) as Gun).DefaultModule.chargeProjectiles[0].Projectile.hitEffects.enemy;
            BounceProjModifier bounce = projectile.gameObject.AddComponent<BounceProjModifier>();
            bounce.numberOfBounces = 100;
            projectile.gameObject.AddComponent<TitanSlayerArrow>();
            PierceProjModifier pierceProj = projectile.gameObject.AddComponent<PierceProjModifier>();
            pierceProj.penetration = 100;

            ProjectileModule.ChargeProjectile chargeProj = new ProjectileModule.ChargeProjectile
            {
                Projectile = projectile,
                ChargeTime = 0.5f,
            };
            gun.DefaultModule.chargeProjectiles = new List<ProjectileModule.ChargeProjectile> { chargeProj };

            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = CustomClipAmmoTypeToolbox.AddCustomAmmoType("Titan Slayer Arrow", "NevernamedsItems/Resources/CustomGunAmmoTypes/titanslayer_clipfull", "NevernamedsItems/Resources/CustomGunAmmoTypes/titanslayer_clipempty");

            gun.quality = PickupObject.ItemQuality.A;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            gun.SetTag("arrow_bolt_weapon");
            ID = gun.PickupObjectId;
        }
        public static int ID;
    }
    public class TitanSlayerArrow : MonoBehaviour
    {
        public Projectile self;
        public BounceProjModifier bounce;
        public SlowDownOverTimeModifier slow;
        float speedToRestoreTo;
        public void Start()
        {
            self = base.GetComponent<Projectile>();
            bounce = base.GetComponent<BounceProjModifier>();
            bounce.OnBounce += OnBounce;
        }
        public int phase = 1;
        public void OnBounce()
        {
            if (phase == 1)
            {
                speedToRestoreTo = self.baseData.speed;
                slow = self.gameObject.AddComponent<SlowDownOverTimeModifier>();
                slow.timeToSlowOver = 0.2f;
                slow.extendTimeByRangeStat = false;
                phase++;
            }
        }
        public IEnumerator RestoreSpeed()
        {
            if (self)
            {
                float dur = 0.2f;
                float el = 0;
                while (el <= dur)
                {
                    if (self)
                    {
                        self.baseData.speed = Mathf.Lerp(0, speedToRestoreTo * 0.7f, el / dur);
                        self.UpdateSpeed();
                    }
                    el += BraveTime.DeltaTime;
                    yield return null;
                }
            }
            yield break;
        }
        public float timeinPhase2 = 0;
        public void Update()
        {
            if (self && self.ProjectilePlayerOwner() && self.ProjectilePlayerOwner().CurrentGun && self.ProjectilePlayerOwner().CurrentGun.IsReloading)
            {
                if (phase == 2)
                {
                    timeinPhase2 += BraveTime.DeltaTime;
                    if (timeinPhase2 > 0.20f)
                    {
                        UnityEngine.Object.Destroy(slow);
                        phase++;
                        HomeInOnPlayerModifyer orAddComponent = self.gameObject.GetOrAddComponent<HomeInOnPlayerModifyer>();
                        orAddComponent.HomingRadius = 1000;
                        orAddComponent.AngularVelocity = 4000;
                        base.StartCoroutine(RestoreSpeed());
                        self.specRigidbody.CollideWithTileMap = false;
                        self.specRigidbody.Reinitialize();
                    }
                }
            }
            if (phase == 3 && self && self.ProjectilePlayerOwner() && !isDestroying)
            {
                if (Vector2.Distance(self.LastPosition, self.ProjectilePlayerOwner().specRigidbody.UnitCenter) < 2)
                {
                    isDestroying = true;
                    SpawnManager.SpawnVFX(SharedVFX.WhiteCircleVFX, self.LastPosition, Quaternion.identity);
                    if (self.ProjectilePlayerOwner().CurrentGun && self.ProjectilePlayerOwner().CurrentGun.GetComponent<TitanSlayer>() && self.ProjectilePlayerOwner().CurrentGun.IsReloading)
                    {
                        Gun gu = self.ProjectilePlayerOwner().CurrentGun;
                        gu.ForceImmediateReload(false);
                        GameUIRoot.Instance.ForceClearReload(self.ProjectilePlayerOwner().PlayerIDX);
                    }
                    UnityEngine.Object.Destroy(self.gameObject);
                }
            }
        }
        bool isDestroying = false;

    }
}
