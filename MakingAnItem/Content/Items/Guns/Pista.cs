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
using Dungeonator;
using Alexandria.Misc;
using Alexandria.VisualAPI;

namespace NevernamedsItems
{

    public class Pista : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Pista", "pista");
            Game.Items.Rename("outdated_gun_mods:pista", "nn:pista");
            gun.gameObject.AddComponent<Pista>();
            gun.SetShortDescription("Yeeeeehaw!");
            gun.SetLongDescription("Six tiny spirits inhabit this gun, gleefully riding it's bullets into battle, and re-aiming them towards the nearest target when the owner signals them via reloading." + "\n\nThis gun smells vaguely Italian.");

            Alexandria.Assetbundle.GunInt.SetupSprite(gun, Initialisation.gunCollection, "pista_idle_001", 8, "pista_ammonomicon_001");

            gun.SetAnimationFPS(gun.shootAnimation, 15);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(38) as Gun).gunSwitchGroup;

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1f;
            gun.DefaultModule.cooldownTime = 0.15f;
            gun.DefaultModule.numberOfShotsInClip = 6;
            gun.SetBarrel(16, 13);
            gun.SetBaseMaxAmmo(200);
            gun.gunClass = GunClass.PISTOL;

            //BULLET STATS
            Projectile projectile = gun.DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.transform.parent = gun.barrelOffset;
            projectile.baseData.speed *= 0.65f;
            projectile.baseData.range *= 2f;
            projectile.baseData.damage = 10f;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(38) as Gun).muzzleFlashEffects;

            SelfReAimBehaviour reaim = projectile.gameObject.GetOrAddComponent<SelfReAimBehaviour>();
            reaim.maxReloadReAims = 1;
            reaim.trigger = SelfReAimBehaviour.ReAimTrigger.RELOAD;
            reaim.VFX = (PickupObjectDatabase.GetById(178) as Gun).GetComponent<FireOnReloadSynergyProcessor>().DirectedBurstSettings.ProjectileInterface.SpecifiedProjectile.hitEffects.tileMapHorizontal.effects[0].effects[0].effect;
            reaim.sounds = new List<string>() { "Play_BOSS_Punchout_Punch_Hit_01", "Play_ENM_Hurt" };

            gun.AddClipSprites("pista");

            gun.AddShellCasing(0, 0, 6, 1, "shell_turquoise");

            gun.quality = PickupObject.ItemQuality.B;
            ETGMod.Databases.Items.Add(gun, false, "ANY");

            PistaID = gun.PickupObjectId;
        }
        public static int PistaID;
        public override void PostProcessProjectile(Projectile projectile)
        {
            if (projectile.gameObject.GetComponent<SelfReAimBehaviour>())
            {
                if (projectile.ProjectilePlayerOwner() && projectile.ProjectilePlayerOwner().PlayerHasActiveSynergy("Pistols Requiem"))
                {
                    projectile.gameObject.GetComponent<SelfReAimBehaviour>().maxReloadReAims = 100;
                }
                projectile.gameObject.GetComponent<SelfReAimBehaviour>().OnReAim += OnReAim;
            }
            base.PostProcessProjectile(projectile);
        }
        private float frictionTimer;
        public override void Update()
        {
            if (frictionTimer >= 0) frictionTimer -= BraveTime.DeltaTime;
            base.Update();
        }
        public void ReAimEffects(Projectile ReAimed)
        {
            if (frictionTimer < 0)
            {
                StickyFrictionManager.Instance.RegisterCustomStickyFriction(0.25f, 0f, true, false);
                frictionTimer = 0.3f;
            }
            if (ReAimed.ProjectilePlayerOwner() && ReAimed.ProjectilePlayerOwner().PlayerHasActiveSynergy("Six Bullets"))
            {
                ReAimed.baseData.speed *= 2f;
                ReAimed.UpdateSpeed();
                ReAimed.baseData.damage *= 1.25f;

                ImprovedAfterImage afterImage = ReAimed.gameObject.AddComponent<ImprovedAfterImage>();
                afterImage.spawnShadows = true;
                afterImage.shadowLifetime = (UnityEngine.Random.Range(0.1f, 0.2f));
                afterImage.shadowTimeDelay = 0.001f;
                afterImage.dashColor = new Color(1, 0.8f, 0.55f, 0.3f);
                afterImage.name = "Gun Trail";
            }
        }
        public static void OnReAim(Projectile ReAimed)
        {
            if (ReAimed && ReAimed.PossibleSourceGun && ReAimed.PossibleSourceGun.GetComponent<Pista>()) { ReAimed.PossibleSourceGun.GetComponent<Pista>().ReAimEffects(ReAimed); }
        }
    }
}