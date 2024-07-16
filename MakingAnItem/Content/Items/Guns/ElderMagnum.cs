using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.Assetbundle;

namespace NevernamedsItems
{

    public class ElderMagnum : AdvancedGunBehavior
    {
        public static void Add()
        {

            Gun gun = ETGMod.Databases.Items.NewGun("Elder Magnum", "eldermagnum2");

            Game.Items.Rename("outdated_gun_mods:elder_magnum", "nn:elder_magnum");
            var comp = gun.gameObject.AddComponent<ElderMagnum>();

            gun.SetShortDescription("Guncestral");
            gun.SetLongDescription("An ancient firearm, left to age in some safe over hundreds of years." + "\n\nWhoever owned this gun has probably been slinging since before your great grandpappy was born.");

            Alexandria.Assetbundle.GunInt.SetupSprite(gun, Initialisation.gunCollection, "eldermagnum2_idle_001", 8, "eldermagnum2_ammonomicon_001");


            gun.SetAnimationFPS(gun.shootAnimation, 14);
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(198) as Gun).gunSwitchGroup;
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(80) as Gun, true, false);

            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.3f;
            gun.DefaultModule.cooldownTime = 0.1f;
            gun.DefaultModule.numberOfShotsInClip = 7;
            gun.barrelOffset.transform.localPosition = new Vector3(1.5f, 0.81f, 0f);
            gun.InfiniteAmmo = true;
            gun.gunClass = GunClass.SHITTY;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            projectile.hitEffects.overrideMidairDeathVFX = EasyVFXDatabase.RedLaserCircleVFX;
            projectile.hitEffects.alwaysUseMidair = true;

            projectile.SetProjectileCollisionRight("eldermagnum_projectile", Initialisation.ProjectileCollection, 5, 5, true, tk2dBaseSprite.Anchor.MiddleCenter, 4, 4);
            gun.DefaultModule.projectiles[0] = projectile;

            gun.AddShellCasing(1, 0, 0, 0, "shell_red");
            gun.shellCasing.gameObject.AddComponent<ClipBurner>();

            gun.quality = PickupObject.ItemQuality.EXCLUDED;
            ETGMod.Databases.Items.Add(gun, false, "ANY");

        }
    }
    public class ClipBurner : BraveBehaviour
    {
        private void Start()
        {
            base.sprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitCutoutUberPhantom");
            base.sprite.renderer.material.SetFloat("_PhantomGradientScale", 0.75f);
            base.sprite.renderer.material.SetFloat("_PhantomContrastPower", 1.3f);
            base.sprite.renderer.material.SetFloat("_ApplyFade", 0.3f);
            base.sprite.usesOverrideMaterial = true;
            base.debris.OnGrounded += OnGround;
        }
        public void OnGround(DebrisObject self)
        {
            base.StartCoroutine(BurnBlackPhantomCorpse());
        }
        private IEnumerator BurnBlackPhantomCorpse()
        {
            Material targetMaterial = base.sprite.renderer.material;
            float ela = 0f;
            float dura = 3f;
            while (ela < dura)
            {
                ela += BraveTime.DeltaTime;
                float t = ela / dura;
                targetMaterial.SetFloat("_BurnAmount", t);
                yield return null;
            }
            doParticles = false;
            yield break;
        }
        private float particleCounter = 0;
        bool doParticles = true;
        private void Update()
        {
            if (doParticles)
            {
                particleCounter += BraveTime.DeltaTime * 2;
                if (particleCounter > 1f)
                {
                    int num = Mathf.FloorToInt(particleCounter);
                    particleCounter %= 1f;


                    GlobalSparksDoer.DoRandomParticleBurst(num, base.sprite.WorldBottomLeft.ToVector3ZisY(0f), base.sprite.WorldTopRight.ToVector3ZisY(0f), Vector3.up, 90f, 0.5f, null, null, null, GlobalSparksDoer.SparksType.BLACK_PHANTOM_SMOKE);
                }
            }
        }
    }
}