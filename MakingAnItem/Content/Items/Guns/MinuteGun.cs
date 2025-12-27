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
    public class MinuteGunAmmoHandler : CustomAmmoDisplay
    {
        private Gun _gun;
        private MinuteGun MinuteGun;
        private PlayerController _owner;

        private void Start()
        {
            _gun = base.GetComponent<Gun>();
            MinuteGun = base.GetComponent<MinuteGun>();
            _owner = this._gun.CurrentOwner as PlayerController;
        }

        public override bool DoCustomAmmoDisplay(GameUIAmmoController uic)
        {
            if (!this._owner)
                return false; // return false to just run the default vanilla ammo display logic

            // add a ui sprite, display a custom variable, and colorize the vanilla ammo display
            string final = MinuteGun.displayTimer;
            if (MinuteGun.displayTimer == "00:00")
            {
                final = $"[color #ff0000]{final}[/color]";
            }
            uic.GunAmmoCountLabel.Text = final;
            return true; // return true to let the game know you have a custom stuff you want to display
        }
    }
    public class MinuteGun : AdvancedGunBehavior, GunShootNegater
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Minute Gun", "minutegun");
            Game.Items.Rename("outdated_gun_mods:minute_gun", "nn:minute_gun");
            gun.gameObject.AddComponent<MinuteGun>();
            gun.SetShortDescription("You Have 60 Seconds");
            gun.SetLongDescription("Usable for only 60 seconds each floor." + "\n\nA tiny adventurer managed to bring this all the way from his home in the swamps to the Gungeon's Entrance before his time was up.");

            Alexandria.Assetbundle.GunInt.SetupSprite(gun, Initialisation.gunCollection, "minutegun_idle_001", 8, "minutegun_ammonomicon_001");

            gun.SetAnimationFPS(gun.shootAnimation, 16);

            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(86) as Gun).gunSwitchGroup;
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(56) as Gun, true, false);
            gun.gameObject.AddComponent<MinuteGunAmmoHandler>();
            //GUN STATS
            gun.DefaultModule.ammoCost = 0;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1f;
            gun.DefaultModule.cooldownTime = 0.1f;
            gun.DefaultModule.angleVariance = 5;
            gun.DefaultModule.numberOfShotsInClip = 7;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(84) as Gun).muzzleFlashEffects;
            gun.barrelOffset.transform.localPosition = new Vector3(1.75f, 0.87f, 0f);
            gun.InfiniteAmmo = true;
            gun.gunClass = GunClass.PISTOL;
            //BULLET STATS
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            projectile.SetProjectileSprite("minutegun_projectile", 16, 9, true, tk2dBaseSprite.Anchor.MiddleCenter, 15, 8);
            projectile.hitEffects.overrideMidairDeathVFX = SharedVFX.WhiteCircleVFX;
            projectile.hitEffects.alwaysUseMidair = true;
            projectile.baseData.damage = 15;
            gun.DefaultModule.projectiles[0] = projectile;
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = CustomClipAmmoTypeToolbox.AddCustomAmmoType("Minute Gun Bullets", "NevernamedsItems/Resources/CustomGunAmmoTypes/minutegun_clipfull", "NevernamedsItems/Resources/CustomGunAmmoTypes/minutegun_clipempty");
            gun.quality = PickupObject.ItemQuality.C;
            ETGMod.Databases.Items.Add(gun, null, "ANY");

            ID = gun.PickupObjectId;
        }
        public static int ID;
        public string displayTimer;
        public float timer;
        public override void PostProcessProjectile(Projectile projectile)
        {
            if (gun.GunPlayerOwner() && gun.GunPlayerOwner().PlayerHasActiveSynergy("Nick Of Time")) projectile.baseData.damage *= (3 - (0.05f * timer));
            base.PostProcessProjectile(projectile);
        }
        public float GetTimerMaximum(PlayerController player)
        {
            return 60f * player.stats.GetStatValue(PlayerStats.StatType.AmmoCapacityMultiplier);
        }
        protected override void OnPickedUpByPlayer(PlayerController player)
        {
            if (!everPickedUpByPlayer) { timer = GetTimerMaximum(player); }
            GameManager.Instance.OnNewLevelFullyLoaded += this.OnNewLevel;
            base.OnPickedUpByPlayer(player);
        }
        protected override void OnPostDroppedByPlayer(PlayerController player)
        {
            GameManager.Instance.OnNewLevelFullyLoaded -= this.OnNewLevel;
            base.OnPostDroppedByPlayer(player);
        }
        public override void OnDestroy()
        {
            GameManager.Instance.OnNewLevelFullyLoaded -= this.OnNewLevel;
            base.OnDestroy();
        }
        private void OnNewLevel()
        {
            if (gun.GunPlayerOwner()) { timer = GetTimerMaximum(gun.GunPlayerOwner()); }
        }
        protected override void Update()
        {
            if (timer >= 0)
            {
                if (gun && gun.CurrentOwner && gun.CurrentOwner.CurrentGun.PickupObjectId == MinuteGun.ID)
                {
                    timer -= BraveTime.DeltaTime;
                    int initTime = Mathf.CeilToInt( timer);
                    int numMinutes = 0;
                    while (initTime >= 60)
                    {
                        numMinutes++;
                        initTime -= 60;
                    }
                    if (numMinutes <= 0 && initTime <= 0)
                    {
                        displayTimer = $"00:00";
                    }
                    else
                    {
                        string seconds = initTime.ToString();
                        string minutes = numMinutes.ToString();
                        if (numMinutes < 10) { minutes = "0" + minutes; }
                        if (initTime < 10) { seconds = "0" + seconds; }
                        displayTimer = $"{numMinutes}:{seconds}";
                    }            
                }
            }
            base.Update();
        }

        public bool ShouldBeNegated()
        {
            return timer <= 0f;
        }
    }
}
