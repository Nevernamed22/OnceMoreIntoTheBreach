using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using ItemAPI;
using MonoMod.RuntimeDetour;
using System.Reflection;

namespace NevernamedsItems
{
    public class DeskFan : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Desk Fan", "deskfan");
            Game.Items.Rename("outdated_gun_mods:desk_fan", "nn:desk_fan");
            gun.gameObject.AddComponent<DeskFan>();
            gun.SetShortDescription("Night Shift");
            gun.SetLongDescription("Pushes enemies away, and does slight damage." + "\n\nHides great and terrible secrets... maybe.");

            gun.SetupSprite(null, "deskfan_idle_001", 8);

            gun.SetAnimationFPS(gun.shootAnimation, 24);
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(520) as Gun).gunSwitchGroup;

            for (int i = 0; i < 4; i++)
            {
                gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
            }

            //GUN STATS
            gun.reloadTime = 0.8f;
            gun.barrelOffset.transform.localPosition = new Vector3(0.62f, 0.62f, 0f);
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.SetBaseMaxAmmo(700);
            gun.ammo = 700;
            gun.doesScreenShake = false;


            foreach (ProjectileModule mod in gun.Volley.projectiles)
            {
                mod.angleVariance = 45;
                mod.cooldownTime = 0.11f;
                mod.numberOfShotsInClip = 30;
                mod.ammoCost = 1;
                mod.shootStyle = ProjectileModule.ShootStyle.Automatic;
                mod.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;

                //BULLET STATS
                Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(mod.projectiles[0]);
                projectile.gameObject.SetActive(false);
                FakePrefab.MarkAsFakePrefab(projectile.gameObject);
                UnityEngine.Object.DontDestroyOnLoad(projectile);
                mod.projectiles[0] = projectile;
                //projectile.knockbackDoer.knockbackMultiplier *= 1.5f;
                projectile.baseData.damage *= 0.07f;
                projectile.baseData.speed *= 1f;
                projectile.baseData.force *= 1.5f;
                
                projectile.sprite.renderer.enabled = false;
                projectile.baseData.range *= 1f;
                if (mod != gun.DefaultModule) { mod.ammoCost = 0; }

                projectile.transform.parent = gun.barrelOffset;
            }

            gun.quality = PickupObject.ItemQuality.D;
            ETGMod.Databases.Items.Add(gun, null, "ANY");

            DeskFanID = gun.PickupObjectId;
        }
        public static int DeskFanID;
        public override Projectile OnPreFireProjectileModifier(Gun gun, Projectile projectile, ProjectileModule mod)
        {
            if (gun.CurrentOwner is PlayerController)
            {
                if ((gun.CurrentOwner as PlayerController).PlayerHasActiveSynergy("Fresh Air") && (UnityEngine.Random.value < 0.05))
                {
                    return ((Gun)ETGMod.Databases.Items["balloon_gun"]).DefaultModule.projectiles[0];
                }
                else
                {
                    return projectile;
                }
            }
            else
            {
                return projectile;
            }
        }
        public DeskFan()
        {

        }
    }
}