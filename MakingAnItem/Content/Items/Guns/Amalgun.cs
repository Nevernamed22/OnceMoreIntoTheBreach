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
using SaveAPI;

namespace NevernamedsItems
{

    public class Amalgun : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Amalgun", "amalgun");
            Game.Items.Rename("outdated_gun_mods:amalgun", "nn:amalgun");
            gun.gameObject.AddComponent<Amalgun>();
            gun.SetShortDescription("The Ultimate Gun");
            gun.SetLongDescription("A collection of seemingly random guns duct taped together." + "\n\nHas a penchant for jamming in firing mode, reload to clear the jam.");

            gun.SetGunSprites("amalgun");

            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(124) as Gun).gunSwitchGroup;

            gun.SetAnimationFPS(gun.shootAnimation, 16);

            for (int i = 0; i < 10; i++) { gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(122) as Gun, true, false); }
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(87) as Gun, true, false);
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(30) as Gun, true, false);
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(94) as Gun, true, false);
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(15) as Gun, true, false);


            bool initedOneBlunderbuss = false;
            foreach (ProjectileModule mod in gun.Volley.projectiles)
            {
                if (mod.shootStyle == ProjectileModule.ShootStyle.Charged)
                {
                    if (!initedOneBlunderbuss) { initedOneBlunderbuss = true; }
                    else { mod.ammoCost = 0; }
                }
            }

            AdvancedHoveringGunSynergyProcessor hoveringGun = gun.gameObject.AddComponent<AdvancedHoveringGunSynergyProcessor>();
            hoveringGun.RequiredSynergy = "The Sum Of It's Parts";
            hoveringGun.requiresTargetGunInInventory = true;
            hoveringGun.fireDelayBasedOnGun = true;
            hoveringGun.BeamFireDuration = 1.2f;
            hoveringGun.Trigger = AdvancedHoveringGunSynergyProcessor.TriggerStyle.CONSTANT;
            hoveringGun.PositionType = HoveringGunController.HoverPosition.CIRCULATE;
            hoveringGun.IDsToSpawn = new int[] { 122, 87, 30, 94, 15 };

            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(124) as Gun).muzzleFlashEffects;
            gun.reloadTime = 1.21f;
            gun.barrelOffset.transform.localPosition = new Vector3(27f / 16f, 14f / 16f, 0f);
            gun.SetBaseMaxAmmo(700);
            gun.gunClass = GunClass.FULLAUTO;
            gun.quality = PickupObject.ItemQuality.A;
            ETGMod.Databases.Items.Add(gun, null, "ANY");


            ID = gun.PickupObjectId;
        }
        public static int ID;
    }
}
