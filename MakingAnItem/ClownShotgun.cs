using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using ItemAPI;

namespace NevernamedsItems
{

    public class ClownShotgun : GunBehaviour
    {


        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Clown Shotgun", "clownshotgun");
            Game.Items.Rename("outdated_gun_mods:clown_shotgun", "nn:clown_shotgun");
            gun.gameObject.AddComponent<ClownShotgun>();
            gun.SetShortDescription("Honk Honk");
            gun.SetLongDescription("Filled to an excessive degree with bouncing shells. Once belonged to a real, genuine, pure-bred clown."+"\n\nAn essential tool in any clown's arsenal, along with a cute little car, a hula hoop, and space lubricant.");

            gun.SetupSprite(null, "clownshotgun_idle_001", 8);

            gun.SetAnimationFPS(gun.shootAnimation, 13);
            gun.SetAnimationFPS(gun.idleAnimation, 5);

            for (int i = 0; i < 9; i++)
            {
                gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
            }

            //GUN STATS
            foreach (ProjectileModule mod in gun.Volley.projectiles)
            {
                mod.ammoCost = 1;
                mod.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
                mod.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
                mod.cooldownTime = 0.8f;
                mod.angleVariance = 40f;
                mod.numberOfShotsInClip = 40;
                Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(mod.projectiles[0]);
                mod.projectiles[0] = projectile;
                projectile.gameObject.SetActive(false);
                FakePrefab.MarkAsFakePrefab(projectile.gameObject);
                UnityEngine.Object.DontDestroyOnLoad(projectile);
                projectile.baseData.range *= 10;
                BounceProjModifier Bouncing = projectile.gameObject.GetOrAddComponent<BounceProjModifier>();
                Bouncing.numberOfBounces = 1;
                if (mod != gun.DefaultModule) { mod.ammoCost = 0; }
                projectile.transform.parent = gun.barrelOffset;
            }

            gun.reloadTime = 1.5f;
            gun.barrelOffset.transform.localPosition = new Vector3(1.68f, 0.62f, 0f);
            gun.SetBaseMaxAmmo(400);

            //BULLET STATS

            // Here we just set the quality of the gun and the "EncounterGuid", which is used by Gungeon to identify the gun.
            gun.quality = PickupObject.ItemQuality.C;
            gun.encounterTrackable.EncounterGuid = "this is the Clown Shotgun";
            ETGMod.Databases.Items.Add(gun, null, "ANY");
        }
        private void Update()
        {
            PlayerController player = gun.CurrentOwner as PlayerController;
            if (player.PlayerHasActiveSynergy("Clown Town"))
            {
                if (gun.DefaultModule.angleVariance == 40f)
                {
                    foreach (ProjectileModule mod in gun.Volley.projectiles)
                    {
                        mod.angleVariance = 10f;
                    }
                }
            }
            else
            {
                if (gun.DefaultModule.angleVariance == 10f)
                {
                    foreach (ProjectileModule mod in gun.Volley.projectiles)
                    {
                        mod.angleVariance = 40f;
                    }
                }
            }
        }
        public ClownShotgun()
        {

        }
    }
}
