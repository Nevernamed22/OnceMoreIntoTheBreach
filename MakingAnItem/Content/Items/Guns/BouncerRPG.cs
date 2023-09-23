using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.Misc;
using Alexandria.ItemAPI;
using SaveAPI;

namespace NevernamedsItems
{

    public class BouncerRPG : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Bouncer RPG", "bouncerrpg");
            Game.Items.Rename("outdated_gun_mods:bouncer_rpg", "nn:bouncer_rpg");
            gun.gameObject.AddComponent<BouncerRPG>();
            gun.SetShortDescription("Kaboing");
            gun.SetLongDescription("The payload of this standard rocket propelled grenade has been coated in rubber, allowing it to ricochet once before detonating.");

            gun.SetupSprite(null, "bouncerrpg_idle_001", 8);

            gun.SetAnimationFPS(gun.shootAnimation, 12);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(56) as Gun, true, false);
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(39) as Gun).gunSwitchGroup;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(39) as Gun).muzzleFlashEffects;
            gun.doesScreenShake = false;

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 2.5f;
            gun.DefaultModule.cooldownTime = 1f;
            gun.DefaultModule.numberOfShotsInClip = 2;
            gun.barrelOffset.transform.localPosition = new Vector3(26f / 16f, 12f / 16f, 0f);
            gun.SetBaseMaxAmmo(35);
            gun.ammo = 35;
            gun.gunClass = GunClass.EXPLOSIVE;

            //BULLET STATS
            Projectile proj = gun.DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            SimpleProjectileTrail trail = proj.gameObject.AddComponent<SimpleProjectileTrail>();
            trail.addSmoke = true;
            //trail.addFire = true;

            proj.baseData.UsesCustomAccelerationCurve = true;
            proj.baseData.AccelerationCurve = (PickupObjectDatabase.GetById(39) as Gun).DefaultModule.projectiles[0].baseData.AccelerationCurve;

            BounceProjModifier bounce = proj.gameObject.AddComponent<BounceProjModifier>();
            bounce.numberOfBounces = 1;
            StatModifyOnBounce bouncestat = proj.gameObject.AddComponent<StatModifyOnBounce>();
            bouncestat.mods.Add(new StatModifyOnBounce.Modifiers() { stattype = StatModifyOnBounce.ProjectileStatType.SPEED, amount = 2 });
            bouncestat.mods.Add(new StatModifyOnBounce.Modifiers() { stattype = StatModifyOnBounce.ProjectileStatType.DAMAGE, amount = 2 });

            ExplosiveModifier explosion = proj.gameObject.AddComponent<ExplosiveModifier>();
            explosion.doExplosion = true;
            explosion.explosionData = (PickupObjectDatabase.GetById(39) as Gun).DefaultModule.projectiles[0].GetComponent<ExplosiveModifier>().explosionData;

            

            proj.SetProjectileSpriteRight("bouncerrpg_proj", 10, 9, false, tk2dBaseSprite.Anchor.MiddleCenter, 9, 8);
            gun.DefaultModule.projectiles[0] = proj;


            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = CustomClipAmmoTypeToolbox.AddCustomAmmoType("BouncerRPG Ammo", "NevernamedsItems/Resources/CustomGunAmmoTypes/bouncerrpg_clipfull", "NevernamedsItems/Resources/CustomGunAmmoTypes/bouncerrpg_clipempty");


            gun.quality = PickupObject.ItemQuality.B;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            ID = gun.PickupObjectId;
        }
        public static int ID;
    }
}

