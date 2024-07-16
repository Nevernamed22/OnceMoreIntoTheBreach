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
    public class BouncerUzi : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Bouncer Uzi", "bounceruzi");
            Game.Items.Rename("outdated_gun_mods:bouncer_uzi", "nn:bouncer_uzi");
            gun.gameObject.AddComponent<BouncerUzi>();
            gun.SetShortDescription("Let's Bounce");
            gun.SetLongDescription("A standard machine pistol the bullets of which have been wrapped in rubber- though that doesnt make them less deadly."+"\n\nLegends tell of a mythical troupe of Gundead who used these whimsical barkers, but nobody has seen hide nor hair of them in many years.");

            gun.SetGunSprites("bounceruzi");

            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.SetAnimationFPS(gun.reloadAnimation, 0);

            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(43) as Gun).gunSwitchGroup;
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(43) as Gun, true, false);

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.2f;
            gun.DefaultModule.cooldownTime = 0.07f;
            gun.DefaultModule.angleVariance = 15;
            gun.DefaultModule.numberOfShotsInClip = 30;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(43) as Gun).muzzleFlashEffects;
            gun.barrelOffset.transform.localPosition = new Vector3(11f/16f, 10f / 16f, 0f);
            gun.SetBaseMaxAmmo(700);
            gun.ammo = 700;
            gun.gunClass = GunClass.FULLAUTO;

            //BULLET STATS
            Projectile projectile = gun.DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            projectile.baseData.damage = 4f;
            projectile.SetProjectileSprite("bounceruzi_proj", 5, 5, true, tk2dBaseSprite.Anchor.MiddleCenter, 4, 4);

            BounceProjModifier ricochet = projectile.gameObject.AddComponent<BounceProjModifier>();
            ricochet.numberOfBounces = 1;

            VFXPool impact = VFXToolbox.CreateVFXPool("BouncerUzi Impact",
                 new List<string>()
                 {
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/bounceruzi_impact_001",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/bounceruzi_impact_002",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/bounceruzi_impact_003",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/bounceruzi_impact_004",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/bounceruzi_impact_005",
                 },
                12, //FPS
                 new IntVector2(13, 13), //Dimensions
                 tk2dBaseSprite.Anchor.MiddleCenter, //Anchor
                 false, //Uses a Z height off the ground
                 0 //The Z height, if used
                   );

            projectile.hitEffects.enemy = impact;
            projectile.hitEffects.tileMapHorizontal = impact;
            projectile.hitEffects.tileMapVertical = impact;

            gun.muzzleFlashEffects = VFXToolbox.CreateVFXPool("BouncerUzi Muzzleflash",
                new List<string>()
                {
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/bounceruzi_muzzle_001",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/bounceruzi_muzzle_002",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/bounceruzi_muzzle_003",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/bounceruzi_muzzle_004",
                },
                13, //FPS
                new IntVector2(13, 11), //Dimensions
                tk2dBaseSprite.Anchor.MiddleLeft, //Anchor
                false, //Uses a Z height off the ground
                0, //The Z height, if used
                false,
               VFXAlignment.Fixed
                  );

            gun.DefaultModule.projectiles[0] = projectile;


            AdvancedVolleyModificationSynergyProcessor advVolley = gun.gameObject.AddComponent<AdvancedVolleyModificationSynergyProcessor>();
            AdvancedVolleyModificationSynergyData data = ScriptableObject.CreateInstance<AdvancedVolleyModificationSynergyData>();
            ProjectileModule cloned = ProjectileModule.CreateClone(gun.DefaultModule, false);
            cloned.angleFromAim += 90;
            cloned.ammoCost = 0;
            ProjectileModule cloned2 = ProjectileModule.CreateClone(gun.DefaultModule, false);
            cloned2.angleFromAim -= 90;
            cloned2.ammoCost = 0;
            data.AddsModules = true;
            data.ModulesToAdd = new List<ProjectileModule>() { cloned, cloned2 }.ToArray();
            data.RequiredSynergy = "Bounce To The Rhythm";
            advVolley.synergies.Add(data);


            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = "RiotGun Bullets";

            gun.quality = PickupObject.ItemQuality.C;
            ETGMod.Databases.Items.Add(gun, false, "ANY");

            ID = gun.PickupObjectId;
        }
        public static int ID;
        public override void PostProcessProjectile(Projectile projectile)
        {
            if (projectile && projectile.ProjectilePlayerOwner() && projectile.ProjectilePlayerOwner().PlayerHasActiveSynergy("Purple Gungeon Eater") && projectile.GetComponent<BounceProjModifier>())
            {
                projectile.GetComponent<BounceProjModifier>().OnBounceContext += OnBounceContext;
            }
            base.PostProcessProjectile(projectile);
        }
        public void OnBounceContext(BounceProjModifier bounce, SpeculativeRigidbody body)
        {
            Exploder.DoDistortionWave(bounce.GetComponent<Projectile>().sprite.WorldCenter, 0.5f, 0.04f, 2, 0.5f);
            Exploder.DoRadialDamage(4, bounce.GetComponent<Projectile>().sprite.WorldCenter, 3, false, true, false);
        }
    }
}
