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
using Alexandria.Misc;

namespace NevernamedsItems
{

    public class MusketRifle : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Musket Rifle", "musketrifle");
            Game.Items.Rename("outdated_gun_mods:musket_rifle", "nn:musket_rifle");
            gun.gameObject.AddComponent<MusketRifle>();
            gun.SetShortDescription("Civil");
            gun.SetLongDescription("An antique musket rifle. Thoroughly inefficient, but charged with a sense of bloodthirsty ancient optimism about it's potential.");

            gun.SetupSprite(null, "musketrifle_idle_001", 8);

            gun.SetAnimationFPS(gun.shootAnimation, 15);
            gun.SetAnimationFPS(gun.reloadAnimation, 6);

            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(9) as Gun).gunSwitchGroup;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(9) as Gun).muzzleFlashEffects;

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(9) as Gun, true, false);

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 2f;
            gun.DefaultModule.cooldownTime = 1f;
            gun.DefaultModule.numberOfShotsInClip = 1;
            gun.barrelOffset.transform.localPosition = new Vector3(79f / 16f, 24f / 16f, 0f);
            gun.SetBaseMaxAmmo(100);
            gun.gunClass = GunClass.RIFLE;

            //BULLET STATS
            Projectile projectile = gun.DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage = 34;
            projectile.baseData.speed *= 2;
            BounceProjModifier bonce = projectile.GetComponent<BounceProjModifier>();
            bonce.TrackEnemyChance = 1;
            bonce.bouncesTrackEnemies = true;
            bonce.bounceTrackRadius = 5f;
            projectile.pierceMinorBreakables = true;

            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.MUSKETBALL;
            gun.TrimGunSprites();

            

            gun.quality = PickupObject.ItemQuality.D;
            ETGMod.Databases.Items.Add(gun, null, "ANY");

           ID = gun.PickupObjectId;
        }
        public static int ID;
        public override void OnPostFired(PlayerController player, Gun gun)
        {
            if (player && player.PlayerHasActiveSynergy("Flash In The Pan"))
            {
              GameObject smoke =  StandardisedProjectiles.smoke.InstantiateAndFireInDirection(gun.barrelOffset.position, gun.CurrentAngle, 0, player);
                smoke.GetComponent<Projectile>().AssignToPlayer(player, true);
            }
            base.OnPostFired(player, gun);
        }
    }
}