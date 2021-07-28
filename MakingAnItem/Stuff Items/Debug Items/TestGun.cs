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

    public class TestGun : GunBehaviour
    {


        public static void Add()
        {
            
            Gun gun = ETGMod.Databases.Items.NewGun("Testinator", "testinator");
            
            Game.Items.Rename("outdated_gun_mods:testinator", "nn:testinator");
            gun.gameObject.AddComponent<TestGun>();
            
            gun.SetLongDescription("Made for fun. Probably broken.");
            
            gun.SetupSprite(null, "wailingmagnum_idle_001", 8);
            
            gun.SetAnimationFPS(gun.shootAnimation, 10);
   
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);

            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.1f;
            gun.DefaultModule.cooldownTime = 0.4f;
            gun.DefaultModule.numberOfShotsInClip = 500;
            gun.SetBaseMaxAmmo(500);

            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.speed *= 0.5f;
            BolaControlla bola = projectile.gameObject.AddComponent<BolaControlla>();
            bola.bolaPrefab = (PickupObjectDatabase.GetById(56) as Gun).DefaultModule.projectiles[0].gameObject;

            gun.quality = PickupObject.ItemQuality.EXCLUDED;
            ETGMod.Databases.Items.Add(gun, null, "ANY");

        }
        public static int vector = 1;
        public override void PostProcessProjectile(Projectile projectile)
        {
            base.PostProcessProjectile(projectile);
        }

        public TestGun()
        {

        }      
    }
}
