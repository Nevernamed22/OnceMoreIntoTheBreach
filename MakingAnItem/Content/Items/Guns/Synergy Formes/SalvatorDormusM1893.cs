
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

    public class SalvatorDormusM1893 : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Salvator Dormus M1893", "salvatordormusm1893");
            Game.Items.Rename("outdated_gun_mods:salvator_dormus_m1893", "nn:salvator_dormus+m1893");
            gun.gameObject.AddComponent<SalvatorDormusM1893>();
            gun.SetShortDescription("Type Match");
            gun.SetLongDescription("Increases it's own stats based on what other types of gun are in it's owner's possession" + "\n\nOne of the earliest models of semiautomatic pistol ever invented, it's ancestral promenance grants it more power the more of it's descendants are held.");
         
            gun.SetGunSprites("salvatordormusm1893", 8, true);

            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.SetAnimationFPS(gun.reloadAnimation, 0);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1f;
            gun.DefaultModule.cooldownTime = 0.1f;
            gun.DefaultModule.numberOfShotsInClip = 30;
            gun.barrelOffset.transform.localPosition = new Vector3(49f / 16f, 12f / 16f, 0f);
            gun.SetBaseMaxAmmo(550);
            gun.gunClass = GunClass.PISTOL;


            Projectile projectile = gun.DefaultModule.projectiles[0].gameObject.InstantiateAndFakeprefab().GetComponent<Projectile>();
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.hitEffects.overrideMidairDeathVFX = (PickupObjectDatabase.GetById(178) as Gun).GetComponent<FireOnReloadSynergyProcessor>().DirectedBurstSettings.ProjectileInterface.SpecifiedProjectile.hitEffects.tileMapHorizontal.effects[0].effects[0].effect;
            projectile.hitEffects.alwaysUseMidair = true;

            gun.quality = PickupObject.ItemQuality.EXCLUDED;
            ETGMod.Databases.Items.Add(gun, false, "ANY");
            gun.SetName("Salvator Dormus");

            ID = gun.PickupObjectId;
        }
        public static int ID;
    }
}