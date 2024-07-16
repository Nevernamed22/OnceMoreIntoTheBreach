using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;

namespace NevernamedsItems
{

    public class RocketPistol : GunBehaviour
    {


        public static void Add()
        {

            Gun gun = ETGMod.Databases.Items.NewGun("Rocket Pistol", "rocketpistol");
           
            Game.Items.Rename("outdated_gun_mods:rocket_pistol", "nn:rocket_pistol");
            gun.gameObject.AddComponent<RocketPistol>();
           
            gun.SetShortDescription("Hell... Maybe");
            gun.SetLongDescription("Made by a weak Gungeoneer whos atrophied muscles were incapable of holding the heavy Yari Launcher."+"\n\nWhile lacking in the sheer destructive potential that the Yari Launcher's rapid-fire provides, this Rocket Pistol can still do a lot of damage.");

            gun.SetGunSprites("rocketpistol");
            
            gun.SetAnimationFPS(gun.shootAnimation, 20);
            
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(16) as Gun, true, false);
            
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.1f;
            gun.DefaultModule.cooldownTime = 0.3f;
            gun.DefaultModule.numberOfShotsInClip = 6;
            gun.SetBaseMaxAmmo(600);
            gun.gunClass = GunClass.EXPLOSIVE;

            gun.quality = PickupObject.ItemQuality.B;
            ETGMod.Databases.Items.Add(gun, null, "ANY");

            ID = gun.PickupObjectId;
        }
        public static int ID;

        public RocketPistol()
        {

        }
    }
}
