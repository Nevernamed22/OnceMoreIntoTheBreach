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
    public class M70 : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("M70", "m70");
            Game.Items.Rename("outdated_gun_mods:m70", "nn:m70");
            gun.gameObject.AddComponent<M70>();
            gun.SetShortDescription("Total Defence Doctrine");
            gun.SetLongDescription("A modification of the classic AK-47, adapted for space combat by a splinter group of rebels."+"\n\nThough their insurgency was put down by the Hegemony, these guns still find their way in circulation throughout the galaxy.");

            gun.SetGunSprites("m70", 8, false, 2);

            gun.SetAnimationFPS(gun.shootAnimation, 15);
            gun.SetAnimationFPS(gun.reloadAnimation, 10);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(15) as Gun, true, false);
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(15) as Gun).gunSwitchGroup;

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1f;
            gun.DefaultModule.cooldownTime = 0.14f;
            gun.DefaultModule.numberOfShotsInClip = 30;
            gun.SetBarrel(28, 10);
            gun.SetBaseMaxAmmo(550);
            gun.gunClass = GunClass.FULLAUTO;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(15) as Gun).muzzleFlashEffects;

            //BULLET STATS
            Projectile projectile = ProjectileSetupUtility.MakeProjectile(15, 5f);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.speed = 25f;

            flak = ProjectileSetupUtility.MakeProjectile(15, 5f);
            flak.AdditionalScaleMultiplier = 0.6f;

            FixedFlakBehaviour fixedFlak = projectile.gameObject.AddComponent<FixedFlakBehaviour>();
            fixedFlak.angleIsRelative = true;
            fixedFlak.postProcess = true;
            fixedFlak.AddProjectile(flak, 90f);
            fixedFlak.AddProjectile(flak, -90f);

            gun.AddShellCasing(1, 1, 0, 0);
            gun.AddClipDebris(0, 1, "clipdebris_m70");

            gun.quality = PickupObject.ItemQuality.B;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            ID = gun.PickupObjectId;
            gun.AddToSubShop(ItemBuilder.ShopType.Trorc);
        }
        public static Projectile flak;
        public static int ID;
        public override void PostProcessProjectile(Projectile projectile)
        {
            FixedFlakBehaviour fixedflak = projectile.GetComponent<FixedFlakBehaviour>();
            if (gun && gun.GunPlayerOwner() && fixedflak)
            {
                if (gun.GunPlayerOwner().PlayerHasActiveSynergy("People's Army"))
                {
                    fixedflak.AddProjectile(flak, 135f);
                    fixedflak.AddProjectile(flak, -135f);
                }
                if (gun.GunPlayerOwner().PlayerHasActiveSynergy("Shot in the Back"))
                {
                    fixedflak.OnFlakSpawn += OnFixedFlakSpawn;
                }
            }
            base.PostProcessProjectile(projectile);
        }
        private void OnFixedFlakSpawn(Projectile proj)
        {
            BounceProjModifier bounce = proj.gameObject.GetComponent<BounceProjModifier>();
            if (bounce) { bounce.numberOfBounces++; }
            else { proj.gameObject.AddComponent<BounceProjModifier>(); }
        }
    }

}
