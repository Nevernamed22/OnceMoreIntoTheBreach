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

    public class Borz : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Borz", "borz");
            Game.Items.Rename("outdated_gun_mods:borz", "nn:borz");
            var behav = gun.gameObject.AddComponent<Borz>();
            gun.SetShortDescription("Krasniy Wolf");
            gun.SetLongDescription("A homemade insurgency weapon- slapped together and barely able to function. Flipping the gun upside down with a dodge roll seems to boost it's effectiveness." + "\n\nUsed for a short time by a brief splinter-state of the Hegemony of Man.");

            gun.SetGunSprites("borz");

            gun.SetAnimationFPS(gun.shootAnimation, 10);
            gun.SetAnimationFPS(gun.reloadAnimation, 0);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(56) as Gun, true, false);
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(2) as Gun).muzzleFlashEffects;
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(2) as Gun).gunSwitchGroup;

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1f;
            gun.DefaultModule.cooldownTime = 0.2f;
            gun.DefaultModule.angleVariance = 8f;
            gun.DefaultModule.numberOfShotsInClip = 60;
            gun.barrelOffset.transform.localPosition = new Vector3(18f / 16f, 12f / 16f, 0f);
            gun.SetBaseMaxAmmo(600);
            gun.gunClass = GunClass.FULLAUTO;

            //BULLET STATS
            Projectile projectile = gun.DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            projectile.baseData.speed *= 1.5f;
            projectile.SetProjectileSprite("spacersfancy_proj", 9, 4, true, tk2dBaseSprite.Anchor.MiddleCenter, 9, 4);
            projectile.hitEffects = (PickupObjectDatabase.GetById(15) as Gun).DefaultModule.projectiles[0].hitEffects;
            gun.DefaultModule.projectiles[0] = projectile;

            gun.quality = PickupObject.ItemQuality.C;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            gun.AddToSubShop(ItemBuilder.ShopType.Trorc);
            ID = gun.PickupObjectId;
            gun.SetupUnlockOnCustomFlag(CustomDungeonFlags.PURCHASED_BORZ, true);
            gun.AddItemToTrorcMetaShop(20);
        }
        public static int ID;
        protected override void OnPickedUpByPlayer(PlayerController player)
        {
            player.OnRollStarted += OnRolled;
            base.OnPickedUpByPlayer(player);
        }
        protected override void OnPostDroppedByPlayer(PlayerController player)
        {
            player.OnRollStarted -= OnRolled;
            base.OnPostDroppedByPlayer(player);
        }
        public override void OnDestroy()
        {
            if (gun && gun.GunPlayerOwner()) { gun.GunPlayerOwner().OnRollStarted -= OnRolled; }
            base.OnDestroy();
        }
        private void OnRolled(PlayerController player, Vector2 vec)
        {
            timeRemaining += 3.5f;
            timeRemaining = Mathf.Min(timeRemaining, 3.5f);
        }
        public float timeRemaining = 0;
        protected override void Update()
        {
            if (timeRemaining > 0 && Owner && Owner is PlayerController && gun.IsCurrentGun())
            {
                timeRemaining -= BraveTime.DeltaTime;
                gun.RemoveCurrentGunStatModifier(PlayerStats.StatType.RateOfFire);
                float amt = Mathf.Lerp(1f, gun.GunPlayerOwner().PlayerHasActiveSynergy("Back in Grozny") ? 4f : 3f, timeRemaining / 3.5f);
                //ETGModConsole.Log(amt);
                gun.AddCurrentGunStatModifier(PlayerStats.StatType.RateOfFire, amt, StatModifier.ModifyMethod.MULTIPLICATIVE);
                (Owner as PlayerController).stats.RecalculateStats((Owner as PlayerController));
            }
            base.Update();
        }
    }
}