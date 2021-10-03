using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using MonoMod;
using ItemAPI;
using UnityEngine;

namespace NevernamedsItems
{

    public class PaintballGun : GunBehaviour
    {


        public static void Add()
        {          
            Gun gun = ETGMod.Databases.Items.NewGun("Paintball Gun", "paintballgun");
            
            Game.Items.Rename("outdated_gun_mods:paintball_gun", "nn:paintball_gun");
            gun.gameObject.AddComponent<PaintballGun>();          
            gun.SetShortDescription("The Colours, Duke!");
            gun.SetLongDescription("Small rubbery pellets loaded with lethal old-school lead paint."+"\n\nBrought to the Gungeon by an amateur artist who wished to flee his debts.");
         
            gun.SetupSprite(null, "paintballgun_idle_001", 8);      
            gun.SetAnimationFPS(gun.shootAnimation, 14);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.1f;
            gun.DefaultModule.cooldownTime = 0.1f;
            gun.DefaultModule.numberOfShotsInClip = 10;
            gun.SetBaseMaxAmmo(300);
            //gun.DefaultModule.positionOffset = new Vector3(1f, 0f, 0f);
            gun.gunClass = GunClass.PISTOL;
            //BULLET STATS
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage *= 1.5f;
            RandomiseProjectileColourComponent paintballController = projectile.gameObject.AddComponent<RandomiseProjectileColourComponent>();
            paintballController.ApplyColourToHitEnemies = true;

            gun.quality = PickupObject.ItemQuality.C;

            gun.encounterTrackable.EncounterGuid = "this is the Paintball Gun";
            ETGMod.Databases.Items.Add(gun, null, "ANY");

        }
        public override void OnPostFired(PlayerController player, Gun gun)
        {
            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_WPN_smileyrevolver_shot_01", gameObject);
        }
        public PaintballGun()
        {

        }
    }  
}
