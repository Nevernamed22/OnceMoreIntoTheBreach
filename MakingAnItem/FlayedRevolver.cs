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

    public class FlayedRevolver : AdvancedGunBehavior
    {


        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Flayed Revolver", "flayedrevolver");
            Game.Items.Rename("outdated_gun_mods:flayed_revolver", "nn:flayed_revolver");
            gun.gameObject.AddComponent<FlayedRevolver>();
            gun.SetShortDescription("Sinister Bells");
            gun.SetLongDescription("The favoured weapon of the cruel Mine Flayer, Planar lord of rings.\n\n" + "Reloading a full clip allows the bearer to slip beyond the curtain, if only briefly.");
            gun.SetupSprite(null, "flayedrevolver_idle_001", 13);
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(35) as Gun).gunSwitchGroup;

            gun.SetAnimationFPS(gun.shootAnimation, 24);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(35) as Gun, true, false);

            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.1f;
            gun.DefaultModule.cooldownTime = 0.1f;
            gun.barrelOffset.transform.localPosition = new Vector3(1.43f, 0.81f, 0f);
            gun.DefaultModule.numberOfShotsInClip = 6;
            gun.SetBaseMaxAmmo(250);

            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage *= 1.10f;
            projectile.baseData.speed *= 1f;
            projectile.baseData.range *= 1f;

            gun.quality = PickupObject.ItemQuality.B;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            FlayedRevolverID = gun.PickupObjectId;
        }
        public static int FlayedRevolverID;
        public override void OnReloadPressedSafe(PlayerController player, Gun gun, bool manualReload)
        {
            if ((gun.ClipCapacity == gun.ClipShotsRemaining) || (gun.CurrentAmmo == gun.ClipShotsRemaining))
            {
                if (gun.CurrentAmmo >= 5)
                {
                    gun.CurrentAmmo -= 5;
                    TeleportPlayerToCursorPosition.StartTeleport(player);
                }
            }
        }

        public FlayedRevolver()
        {

        }
    }
}
