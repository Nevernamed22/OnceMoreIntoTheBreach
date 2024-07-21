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
using SaveAPI;
using Alexandria.Misc;

namespace NevernamedsItems
{

    public class Scythe : AdvancedGunBehavior
    {
        public static void Add()
        {

            Gun gun = ETGMod.Databases.Items.NewGun("Scythe", "scythe");
            Game.Items.Rename("outdated_gun_mods:scythe", "nn:scythe");
            var behav = gun.gameObject.AddComponent<Scythe>();

            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(417) as Gun).gunSwitchGroup;


            gun.SetShortDescription("Reap");
            gun.SetLongDescription("An old fashioned scythe without a gun attached. Rends the souls from enemies."+"\n\nFavoured by reapers and wheat farmers everywhere except the gungeon.");
            gun.SetGunSprites("scythe");


            gun.SetAnimationFPS(gun.shootAnimation, 13);
            gun.SetAnimationFPS(gun.reloadAnimation, 0);

            gun.AddPassiveStatModifier(PlayerStats.StatType.Curse, 2f, StatModifier.ModifyMethod.ADDITIVE);

            gun.gunScreenShake = (PickupObjectDatabase.GetById(417) as Gun).gunScreenShake;
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);


            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.15f;
            gun.DefaultModule.cooldownTime = 0.75f;
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.DefaultModule.numberOfShotsInClip = 6;

            gun.SetBarrel(47, 57);


            gun.barrelOffset.transform.localPosition = new Vector3(49f / 16f, 58f / 16f, 0f);
            gun.SetBaseMaxAmmo(100);
            gun.quality = PickupObject.ItemQuality.B;
            gun.gunClass = GunClass.SILLY;

            //BULLET STATS
            Projectile projectile = ProjectileUtility.SetupProjectile(86);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage = 20f;
            projectile.sprite.renderer.enabled = false;


            SoulRendSlashModifier slash = projectile.gameObject.AddComponent<SoulRendSlashModifier>();
            slash.DestroyBaseAfterFirstSlash = true;
            slash.slashParameters = ScriptableObject.CreateInstance<SlashData>();
            slash.slashParameters.soundEvent = null;
            slash.slashParameters.projInteractMode = SlashDoer.ProjInteractMode.IGNORE;
            slash.SlashDamageUsesBaseProjectileDamage = true;
            slash.slashParameters.doVFX = false;
            slash.slashParameters.doHitVFX = true;
            slash.slashParameters.slashRange = 4.5f;
            slash.slashParameters.slashDegrees = 45f;
            slash.slashParameters.playerKnockbackForce = 40f;

            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = "white";

            ETGMod.Databases.Items.Add(gun, false, "ANY");
            ID = gun.PickupObjectId;
        }
        public static int ID;
        public class SoulRendSlashModifier : ProjectileSlashingBehaviour
        {
            public override void SlashHitTarget(GameActor target, bool fatal)
            {
                if (fatal && base.GetComponent<Projectile>() && base.GetComponent<Projectile>().Owner && base.GetComponent<Projectile>().Owner is PlayerController)
                {
                    PlayerController ac = base.GetComponent<Projectile>().Owner as PlayerController;

                    GameObject gemy = StandardisedProjectiles.ghost.InstantiateAndFireInDirection(
                      target.specRigidbody.UnitCenter,
                      ac.CurrentGun.CurrentAngle);
                    Projectile proj = gemy.GetComponent<Projectile>();
                    proj.Owner = ac;
                    proj.Shooter = ac.specRigidbody;
                    proj.specRigidbody.RegisterGhostCollisionException(target.specRigidbody);
                    ac.DoPostProcessProjectile(proj);
                    proj.ScaleByPlayerStats(ac);
                }
                base.SlashHitTarget(target, fatal);
            }
        }
    }
}