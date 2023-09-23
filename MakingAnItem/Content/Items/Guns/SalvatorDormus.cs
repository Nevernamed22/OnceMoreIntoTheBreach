
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

    public class SalvatorDormus : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Salvator Dormus", "salvatordormus");
            Game.Items.Rename("outdated_gun_mods:salvator_dormus", "nn:salvator_dormus");
            gun.gameObject.AddComponent<SalvatorDormus>();
            gun.SetShortDescription("Type Match");
            gun.SetLongDescription("Increases it's own stats based on what other types of gun are in it's owner's possession" + "\n\nOne of the earliest models of semiautomatic pistol ever invented, it's ancestral promenance grants it more power the more of it's descendants are held.");
            gun.SetupSprite(null, "salvatordormus_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.SetAnimationFPS(gun.reloadAnimation, 0);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1f;
            gun.DefaultModule.cooldownTime = 0.12f;
            gun.DefaultModule.numberOfShotsInClip = 7;
            gun.barrelOffset.transform.localPosition = new Vector3(25f / 16f, 14f / 16f, 0f);
            gun.SetBaseMaxAmmo(440);
            gun.gunClass = GunClass.PISTOL;

            Projectile projectile = gun.DefaultModule.projectiles[0].gameObject.InstantiateAndFakeprefab().GetComponent<Projectile>();
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.hitEffects.overrideMidairDeathVFX = (PickupObjectDatabase.GetById(178) as Gun).GetComponent<FireOnReloadSynergyProcessor>().DirectedBurstSettings.ProjectileInterface.SpecifiedProjectile.hitEffects.tileMapHorizontal.effects[0].effects[0].effect;
            projectile.hitEffects.alwaysUseMidair = true;
            projectile.baseData.damage = 7f;

            gun.quality = PickupObject.ItemQuality.B;
            ETGMod.Databases.Items.Add(gun, false, "ANY");

            ID = gun.PickupObjectId;
        }
        public static int ID;

        public int lastGunCount;
        public override void OnSwitchedToThisGun()
        {
            UpdateGunStats();
            base.OnSwitchedToThisGun();
        }
        protected override void OnPickedUpByPlayer(PlayerController player)
        {
            base.OnPickedUpByPlayer(player);
            UpdateGunStats();
        }

        public override void OnSwitchedAwayFromThisGun()
        {
            UpdateGunStats();
            base.OnSwitchedAwayFromThisGun();
        }
        public void UpdateGunStats()
        {
            gun.RemoveCurrentGunStatModifier(PlayerStats.StatType.RateOfFire);
            gun.RemoveCurrentGunStatModifier(PlayerStats.StatType.ReloadSpeed);
            gun.RemoveCurrentGunStatModifier(PlayerStats.StatType.Damage);
            gun.RemoveCurrentGunStatModifier(PlayerStats.StatType.ProjectileSpeed);
            gun.RemoveCurrentGunStatModifier(PlayerStats.StatType.AdditionalClipCapacityMultiplier);
            if (gun.GunPlayerOwner())
            {
                gun.AddCurrentGunStatModifier(PlayerStats.StatType.RateOfFire, 1 + (0.05f * gun.GunPlayerOwner().inventory.AllGuns.FindAll((Gun x) => x.DefaultModule != null && x.DefaultModule.shootStyle == ProjectileModule.ShootStyle.Automatic).Count), StatModifier.ModifyMethod.MULTIPLICATIVE);
                gun.AddCurrentGunStatModifier(PlayerStats.StatType.ReloadSpeed, Mathf.Max(0, 1 - (0.05f * gun.GunPlayerOwner().inventory.AllGuns.FindAll((Gun x) => x.DefaultModule != null && x.DefaultModule.shootStyle == ProjectileModule.ShootStyle.SemiAutomatic).Count)), StatModifier.ModifyMethod.MULTIPLICATIVE);
                gun.AddCurrentGunStatModifier(PlayerStats.StatType.Damage, 1 + (0.05f * gun.GunPlayerOwner().inventory.AllGuns.FindAll((Gun x) => x.DefaultModule != null && x.DefaultModule.shootStyle == ProjectileModule.ShootStyle.Charged).Count), StatModifier.ModifyMethod.MULTIPLICATIVE);
                gun.AddCurrentGunStatModifier(PlayerStats.StatType.ProjectileSpeed, 1 + (0.05f * gun.GunPlayerOwner().inventory.AllGuns.FindAll((Gun x) => x.DefaultModule != null && x.DefaultModule.shootStyle == ProjectileModule.ShootStyle.Burst).Count), StatModifier.ModifyMethod.MULTIPLICATIVE);
                gun.AddCurrentGunStatModifier(PlayerStats.StatType.AdditionalClipCapacityMultiplier, 1 + (0.05f * gun.GunPlayerOwner().inventory.AllGuns.FindAll((Gun x) => x.DefaultModule != null && x.DefaultModule.shootStyle == ProjectileModule.ShootStyle.Beam).Count), StatModifier.ModifyMethod.MULTIPLICATIVE);
                gun.GunPlayerOwner().stats.RecalculateStats(gun.GunPlayerOwner());
            }
        }
        protected override void NonCurrentGunUpdate()
        {
            if (gun && gun.GunPlayerOwner() && gun.GunPlayerOwner().inventory.AllGuns.Count != lastGunCount)
            {
                UpdateGunStats();
                lastGunCount = gun.GunPlayerOwner().inventory.AllGuns.Count;
            }
            base.NonCurrentGunUpdate();
        }
    }
}