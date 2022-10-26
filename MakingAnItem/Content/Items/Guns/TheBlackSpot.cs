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

namespace NevernamedsItems
{
    public class TheBlackSpot : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("The Black Spot", "theblackspot");
            Game.Items.Rename("outdated_gun_mods:the_black_spot", "nn:the_black_spot");
            gun.gameObject.AddComponent<TheBlackSpot>();
            gun.SetShortDescription("Put To Death");
            gun.SetLongDescription("This flintlock pistol is haunted by the souls of an entire pirate crew, put to death for mutiny."+"\n\nWhen you hold the barrel to your ear, you can hear the sea.");

            gun.SetupSprite(null, "theblackspot_idle_001", 8);

            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.SetAnimationFPS(gun.reloadAnimation, 12);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(169) as Gun).muzzleFlashEffects;
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(9) as Gun).gunSwitchGroup;

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.3f;
            gun.DefaultModule.cooldownTime = 0.30f;
            gun.DefaultModule.numberOfShotsInClip = 1;
            gun.barrelOffset.transform.localPosition = new Vector3(31f / 16f, 16f / 16f, 0f);
            gun.SetBaseMaxAmmo(50);
            gun.gunClass = GunClass.PISTOL;
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.MUSKETBALL;

            //BULLET STATS
            Projectile projectile = gun.DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.transform.parent = gun.barrelOffset;
            projectile.baseData.speed *= 2f;
            projectile.baseData.damage = 7f;
            projectile.baseData.range = 300;
            HomingModifier homingModifier = projectile.gameObject.AddComponent<HomingModifier>();
            homingModifier.HomingRadius = 100f;
            homingModifier.AngularVelocity = 4000f;
            BounceProjModifier bounce = projectile.gameObject.AddComponent<BounceProjModifier>();
            bounce.numberOfBounces = 100;
            PierceProjModifier pierce = projectile.gameObject.AddComponent<PierceProjModifier>();
            pierce.penetration = 50;
            EasyTrailBullet trail = projectile.gameObject.AddComponent<EasyTrailBullet>();
            trail.TrailPos = projectile.transform.position;
            trail.StartWidth = 0.43f;
            trail.EndWidth = 0f;
            trail.LifeTime = 1.5f;
            trail.BaseColor = Color.black;
            trail.EndColor = Color.black;
            projectile.SetProjectileSpriteRight("theblackspot_proj", 7, 7, false, tk2dBaseSprite.Anchor.MiddleCenter, 6, 6);
            projectile.gameObject.AddComponent<PierceDeadActors>();

            gun.quality = PickupObject.ItemQuality.A;
            ETGMod.Databases.Items.Add(gun, false, "ANY");

            ID = gun.PickupObjectId;
        }
        public static int ID;  
    }
}