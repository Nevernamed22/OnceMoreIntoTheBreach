using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;
using MonoMod.RuntimeDetour;
using System.Reflection;
using SaveAPI;
using Alexandria.Assetbundle;

namespace NevernamedsItems
{
    public class ScrapStrap : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Scrap Strap", "scrapstrap");
            Game.Items.Rename("outdated_gun_mods:scrap_strap", "nn:scrap_strap");
            var behav = gun.gameObject.AddComponent<ScrapStrap>();
            gun.SetShortDescription("Seen Better Days");
            gun.SetLongDescription("This handgun has seemingly been repaired at least four separate times, but never by an experienced gunsmith. Jamming irregular pieces of metal down the barrel seems to work just fine.\n\nSo fine, in fact, that you can repurpose the weapons spent shells as ammunition for other guns!");

            gun.SetGunSprites("scrapstrap");

            gun.SetAnimationFPS(gun.reloadAnimation, 0);

            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(510) as Gun).gunSwitchGroup;

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(1) as Gun).muzzleFlashEffects;

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 2f;
            gun.DefaultModule.cooldownTime = 0.02f;
            gun.DefaultModule.numberOfShotsInClip = 23;
            gun.barrelOffset.transform.localPosition = new Vector3(16f/16f, 10f / 16f, 0f);
            gun.SetBaseMaxAmmo(500);
            gun.ammo = 500;
            gun.gunClass = GunClass.PISTOL;

            //BULLET STATS
            Projectile projectile = (PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0].gameObject.InstantiateAndFakeprefab().GetComponent<Projectile>();
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage = 4.5f;

            gun.quality = PickupObject.ItemQuality.D;
            ETGMod.Databases.Items.Add(gun, null, "ANY");

            ID = gun.PickupObjectId;
            gun.SetupUnlockOnCustomStat(CustomTrackedStats.BEGGAR_TOTAL_DONATIONS, 14, DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN);
        }
        public static int ID;
        public override void OnPostFired(PlayerController player, Gun gun)
        {
            List<Gun> guns = new List<Gun>(player.inventory.AllGuns);
            guns.RemoveAll(x => x == gun || !x.CanGainAmmo || x.InfiniteAmmo || x.CurrentAmmo >= x.maxAmmo);
            if (guns.Count > 0) { BraveUtility.RandomElement(guns).GainAmmo(1); }
            base.OnPostFired(player, gun);
        }
    }
}

