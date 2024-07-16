using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;
using SaveAPI;
using Alexandria.Misc;

namespace NevernamedsItems
{
    public class AlternatingFire : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Alternating Fire", "alternatingfire");
            Game.Items.Rename("outdated_gun_mods:alternating_fire", "nn:alternating_fire");
            var behav = gun.gameObject.AddComponent<AlternatingFire>();

            gun.SetShortDescription("Worth the Risk");
            gun.SetLongDescription("Alternates every two seconds between smaller, punchier bullets and larger, more powerful projectiles. \n\nThis gun exists simultaneously in both the elemental planes of light and darkness, and is constantly phasing between the two.");

            gun.SetGunSprites("alternatingfire");

            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.SetAnimationFPS(gun.reloadAnimation, 7);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(47) as Gun).gunSwitchGroup;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(13) as Gun).muzzleFlashEffects;

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.1f;
            gun.DefaultModule.cooldownTime = 0.085f;
            gun.DefaultModule.numberOfShotsInClip = 35;
            gun.barrelOffset.transform.localPosition = new Vector3(34f / 16f, 16f / 16f, 0f);
            gun.SetBaseMaxAmmo(800);
            gun.gunClass = GunClass.FULLAUTO;

            //BULLET STATS
            Projectile projectile = (PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            projectile.gameObject.name = "alternatingfire_proj";
            projectile.SetProjectileSprite("alternatingfire_proj", 7, 7, true, tk2dBaseSprite.Anchor.MiddleCenter, 6, 6);
            projectile.hitEffects.alwaysUseMidair = true;
            projectile.hitEffects.overrideMidairDeathVFX = EasyVFXDatabase.WhiteCircleVFX;
            projectile.baseData.damage = 6;

            gun.DefaultModule.projectiles[0] = projectile;
            

            alternateProj = (PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            alternateProj.SetProjectileSprite("alternatingfire_proj2", 7, 7, true, tk2dBaseSprite.Anchor.MiddleCenter, 6, 6);
            alternateProj.hitEffects.alwaysUseMidair = true;
            alternateProj.hitEffects.overrideMidairDeathVFX = EasyVFXDatabase.WhiteCircleVFX;
            alternateProj.baseData.damage = 3;
            alternateProj.baseData.speed *= 2;
            alternateProj.baseData.force *= 2;


            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = CustomClipAmmoTypeToolbox.AddCustomAmmoType("AlternatingFireClip", "NevernamedsItems/Resources/CustomGunAmmoTypes/alternatingfire_clipfull", "NevernamedsItems/Resources/CustomGunAmmoTypes/alternatingfire_clipempty");


            gun.quality = PickupObject.ItemQuality.B;
            ETGMod.Databases.Items.Add(gun, false, "ANY");

            ID = gun.PickupObjectId;
        }
        protected override void Update()
        {
            if (timer >= 2f)
            {
                alternate = !alternate;
                timer = 0;
            }
            else { timer += BraveTime.DeltaTime; }
            base.Update();
        }
        public float timer;
        public bool alternate = false;
        public override Projectile OnPreFireProjectileModifier(Gun gun, Projectile projectile, ProjectileModule mod)
        {
            if (alternate && projectile.gameObject.name.Contains("alternatingfire")) { return alternateProj; }
            return base.OnPreFireProjectileModifier(gun, projectile, mod);
        }
        public static Projectile alternateProj;
        public static int ID;
    }
}
