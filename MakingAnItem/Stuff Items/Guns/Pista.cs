using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;
using Gungeon;
using MonoMod;
using UnityEngine;
using ItemAPI;
using Dungeonator;

namespace NevernamedsItems
{

    public class Pista : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Pista", "pista");
            Game.Items.Rename("outdated_gun_mods:pista", "nn:pista");
            gun.gameObject.AddComponent<Pista>();
            gun.SetShortDescription("Yeeeeehaw!");
            gun.SetLongDescription("Six tiny spirits inhabit this gun, gleefully riding it's bullets into battle, and re-aiming them towards the nearest target when the owner signals them via reloading."+"\n\nThis gun smells vaguely Italian.");

            gun.SetupSprite(null, "pista_idle_001", 8);

            gun.SetAnimationFPS(gun.shootAnimation, 15);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1f;
            gun.DefaultModule.cooldownTime = 0.15f;
            gun.DefaultModule.numberOfShotsInClip = 6;
            gun.barrelOffset.transform.localPosition = new Vector3(0.81f, 0.62f, 0f);
            gun.SetBaseMaxAmmo(200);
            gun.gunClass = GunClass.PISTOL;
            //BULLET STATS
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.transform.parent = gun.barrelOffset;
            projectile.baseData.speed *= 0.65f;
            projectile.baseData.range *= 2f;
            projectile.baseData.damage *= 1.6f;

            gun.quality = PickupObject.ItemQuality.C;
            gun.encounterTrackable.EncounterGuid = "this is the Pista";
            ETGMod.Databases.Items.Add(gun, null, "ANY");

            PistaID = gun.PickupObjectId;
        }
        public static int PistaID;
        public override void PostProcessProjectile(Projectile projectile)
        {
            ActiveBullets.Add(projectile);
            base.PostProcessProjectile(projectile);
        }
        public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
        {
            if (ActiveBullets.Count > 0)
            {
                foreach (Projectile bullet in ActiveBullets)
                {
                    if (bullet)
                    {
                        bullet.ReAimToNearestEnemy();
                        if (!player.PlayerHasActiveSynergy("Pistols Requiem")) BulletsToRemoveFromActiveBullets.Add(bullet);
                    }
                }
                foreach (Projectile bullet in BulletsToRemoveFromActiveBullets)
                {
                    ActiveBullets.Remove(bullet);
                }
                BulletsToRemoveFromActiveBullets.Clear();
            }
            base.OnReloadPressed(player, gun, bSOMETHING);
        }
        public static List<Projectile> ActiveBullets = new List<Projectile>() { };
        public static List<Projectile> BulletsToRemoveFromActiveBullets = new List<Projectile>() { };
        public Pista()
        {

        }
    }
}