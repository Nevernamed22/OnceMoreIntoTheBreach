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

    public class Claymore : AdvancedGunBehavior
    {
        public static void Add()
        {

            Gun gun = ETGMod.Databases.Items.NewGun("Claymore", "claymore");
            Game.Items.Rename("outdated_gun_mods:claymore", "nn:claymore");
            var behav = gun.gameObject.AddComponent<Claymore>();

            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(417) as Gun).gunSwitchGroup;


            gun.SetShortDescription("Bladeborn");
            gun.SetLongDescription("Slices and detonates foes"+"\n\nFrom the extensive and ostentatious sword collection of King Charthur of the knights of the octagonal table.");
            gun.SetupSprite(null, "claymore_idle_001", 8);


            gun.SetAnimationFPS(gun.shootAnimation, 13);
            gun.SetAnimationFPS(gun.reloadAnimation, 0);

            gun.AddPassiveStatModifier(PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE);

            gun.gunScreenShake = (PickupObjectDatabase.GetById(417) as Gun).gunScreenShake;
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);


            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.10f;
            gun.DefaultModule.cooldownTime = 0.6f;
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.DefaultModule.numberOfShotsInClip = 5;
            gun.barrelOffset.transform.localPosition = new Vector3(47f / 16f, 48f / 16f, 0f);
            gun.SetBaseMaxAmmo(80);
            gun.quality = PickupObject.ItemQuality.B;
            gun.gunClass = GunClass.SILLY;

            //BULLET STATS
            Projectile projectile = ProjectileUtility.SetupProjectile(86);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage = 5f;
            projectile.sprite.renderer.enabled = false;


            ExplosiveSlashModifier slash = projectile.gameObject.AddComponent<ExplosiveSlashModifier>();
            slash.DestroyBaseAfterFirstSlash = true;
            slash.slashParameters = ScriptableObject.CreateInstance<SlashData>();
            slash.slashParameters.soundEvent = null;
            slash.slashParameters.projInteractMode = SlashDoer.ProjInteractMode.DESTROY;
            slash.SlashDamageUsesBaseProjectileDamage = true;
            slash.slashParameters.doVFX = false;
            slash.slashParameters.doHitVFX = true;
            slash.slashParameters.slashRange = 3.5f;
            slash.slashParameters.playerKnockbackForce = 40f;
            slash.explosionData = StaticExplosionDatas.explosiveRoundsExplosion;

            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = CustomClipAmmoTypeToolbox.AddCustomAmmoType("Claymore Bullets", "NevernamedsItems/Resources/CustomGunAmmoTypes/claymore_clipfull", "NevernamedsItems/Resources/CustomGunAmmoTypes/claymore_clipempty");


            ETGMod.Databases.Items.Add(gun, false, "ANY");
            ID = gun.PickupObjectId;

            gun.TrimGunSprites();

            gun.SetupUnlockOnCustomFlag(CustomDungeonFlags.ADVDRAGUN_KILLED_BULLET, true);
        }
        public static int ID;
        public class ExplosiveSlashModifier : ProjectileSlashingBehaviour
        {
            public ExplosionData explosionData;
            public override void SlashHitTarget(GameActor target, bool fatal)
            {
                for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
                {
                    PlayerController playerController = GameManager.Instance.AllPlayers[i];
                    if (playerController && playerController.specRigidbody)
                    {
                        this.explosionData.ignoreList.Add(playerController.specRigidbody);
                    }
                }
                Exploder.Explode(target.CenterPosition, explosionData, Vector2.zero);
                base.SlashHitTarget(target, fatal);
            }
        }
    }
}