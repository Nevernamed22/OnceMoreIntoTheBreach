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
    public class RapidRiposte : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Rapid Riposte", "rapidriposte2");
            Game.Items.Rename("outdated_gun_mods:rapid_riposte", "nn:rapid_riposte");
            gun.gameObject.AddComponent<RapidRiposte>();
            gun.SetShortDescription("Top Tier Gunsmanship");
            gun.SetLongDescription("Fires with the utmost precision, parrying projectiles back at their owners." + "\n\nAn old rapier, modified for gunslinging.");

            gun.SetGunSprites("rapidriposte2", 10, false, 2);

            gun.SetAnimationFPS(gun.shootAnimation, 10);

            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(417) as Gun).gunSwitchGroup;
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(56) as Gun, true, false);

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1f;
            gun.DefaultModule.cooldownTime = 0.1f;
            gun.DefaultModule.angleVariance = 1;
            gun.DefaultModule.numberOfShotsInClip = 5;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(97) as Gun).muzzleFlashEffects;
            gun.SetBarrel(40, 35);
            gun.SetBaseMaxAmmo(130);
            gun.ammo = 130;
            gun.gunClass = GunClass.RIFLE;


            Midair1 = VFXToolbox.CreateVFXBundle("rapidriposte_midair_01", true, 1f, 100, 10, new Color32(255, 255, 255, 255));
            Midair1.GetComponent<tk2dSprite>().HeightOffGround = 1f;

            Midair2 = VFXToolbox.CreateVFXBundle("rapidriposte_midair_02", true, 1f, 100, 10, new Color32(255, 255, 255, 255));
            Midair2.GetComponent<tk2dSprite>().HeightOffGround = 1f;

            Midair3 = VFXToolbox.CreateVFXBundle("rapidriposte_midair_03", true, 1f, 100, 10, new Color32(255, 255, 255, 255));
            Midair3.GetComponent<tk2dSprite>().HeightOffGround = 1f;

            for (int i = 0; i < 3; i++)
            {
                Projectile projectile = gun.DefaultModule.projectiles[0].InstantiateAndFakeprefab();
                projectile.baseData.damage *= 3f;
                projectile.baseData.force *= 3f;
                projectile.baseData.speed *= (i == 2) ? 3f : 2f;
                projectile.pierceMinorBreakables = true;
                AdvancedMirrorProjectileModifier mirror = projectile.gameObject.GetOrAddComponent<AdvancedMirrorProjectileModifier>();
                mirror.projectileSurvives = true;
                mirror.maxMirrors = 1;
                mirror.postProcessReflectedBullets = true;
                mirror.tintsBullets = false;
                projectile.hitEffects.alwaysUseMidair = true;
                projectile.hitEffects.midairInheritsRotation = true;
                projectile.hitEffects.midairInheritsVelocity = true;
                projectile.objectImpactEventName = "star";
                switch (i)
                {
                    case 0:
                        projectile.SetProjectileSprite("rapidriposte_proj1_001", 16, 55, true, tk2dBaseSprite.Anchor.MiddleLeft, 11, 49);
                        projectile.gameObject.name = "RapidRiposte_Projectile_1";
                        projectile.hitEffects.overrideMidairDeathVFX = Midair1;
                        Projectile1 = projectile;
                        break;
                    case 1:
                        projectile.SetProjectileSprite("rapidriposte_proj2_001", 33, 28, true, tk2dBaseSprite.Anchor.MiddleLeft, 30, 24);
                        projectile.gameObject.name = "RapidRiposte_Projectile_2";
                        Projectile2 = projectile;
                        projectile.hitEffects.overrideMidairDeathVFX = Midair2;
                        break;
                    case 2:
                        projectile.SetProjectileSprite("rapidriposte_proj3_001", 36, 20, true, tk2dBaseSprite.Anchor.MiddleLeft, 31, 12);
                        projectile.gameObject.name = "RapidRiposte_Projectile_3";
                        Projectile3 = projectile;
                        projectile.hitEffects.overrideMidairDeathVFX = Midair3;
                        break;
                }
            }






            gun.DefaultModule.projectiles[0] = Projectile1.GetComponent<Projectile>();


            Muzzle1 = VFXToolbox.CreateVFXPoolBundle("rapidriposte_muzzle_01", false, 0, VFXAlignment.Fixed, 5, new Color32(255, 255, 255, 255), orphaned: true);
            Muzzle2 = VFXToolbox.CreateVFXPoolBundle("rapidriposte_muzzle_02", false, 0, VFXAlignment.Fixed, 5, new Color32(255, 255, 255, 255), orphaned: true);
            Muzzle3 = VFXToolbox.CreateVFXPoolBundle("rapidriposte_muzzle_03", false, 0, VFXAlignment.Fixed, 5, new Color32(255, 255, 255, 255), orphaned: true);
            gun.muzzleFlashEffects = Muzzle1;

            gun.quality = PickupObject.ItemQuality.A;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            ID = gun.PickupObjectId;

            ShootAnim1 = gun.UpdateAnimation("fire", Initialisation.gunCollection2, true);
            ShootAnim2 = gun.UpdateAnimation("fire2", Initialisation.gunCollection2, true);
            ShootAnim3 = gun.UpdateAnimation("fire3", Initialisation.gunCollection2, true);

            AnimMuzzles = new Dictionary<string, VFXPool>()
            {
                { ShootAnim1, Muzzle1 },
                { ShootAnim2, Muzzle2 },
                { ShootAnim3, Muzzle3 },
            };
        }
        public static int ID;
        //Fire Animations
        public static string ShootAnim1;
        public static string ShootAnim2;
        public static string ShootAnim3;
        //Muzzle Flashes
        public static VFXPool Muzzle1;
        public static VFXPool Muzzle2;
        public static VFXPool Muzzle3;
        //Projectiles
        public static Projectile Projectile1;
        public static Projectile Projectile2;
        public static Projectile Projectile3;
        //Projectile Midairs
        public static GameObject Midair1;
        public static GameObject Midair2;
        public static GameObject Midair3;


        string lastShootAnimation = "fire";
        public static Dictionary<string, VFXPool> AnimMuzzles;
        public override void PostProcessProjectile(Projectile projectile)
        {
            if (projectile.GetComponent<AdvancedMirrorProjectileModifier>())
            {
                if (projectile.Owner is PlayerController && (projectile.Owner as PlayerController).PlayerHasActiveSynergy("Wouldn't You Agree?"))
                {
                    projectile.GetComponent<AdvancedMirrorProjectileModifier>().RapidRiposteWeebshitSynergy = true;
                }
            }
            base.PostProcessProjectile(projectile);
        }
        public override Projectile OnPreFireProjectileModifier(Gun gun, Projectile projectile, ProjectileModule mod)
        {
            if (projectile.gameObject.name.StartsWith("RapidRiposte_Projectile"))
            {
                if (lastShootAnimation == ShootAnim1) { return Projectile1; }
                else if (lastShootAnimation == ShootAnim2) { return Projectile2; }
                else if (lastShootAnimation == ShootAnim3) { return Projectile3; }
            }
            return base.OnPreFireProjectileModifier(gun, projectile, mod);
        }
        public override void OnPostFired(PlayerController player, Gun gun)
        {
            List<string> anims = new List<string>() { ShootAnim1, ShootAnim2, ShootAnim3 };
            anims.Remove(lastShootAnimation);
            lastShootAnimation = BraveUtility.RandomElement(anims);
            gun.shootAnimation = lastShootAnimation;
            gun.muzzleFlashEffects = AnimMuzzles[lastShootAnimation];
            base.OnPostFired(player, gun);
        }
        public RapidRiposte()
        {

        }
    }
}
