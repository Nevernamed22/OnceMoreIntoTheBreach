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

namespace NevernamedsItems
{

    public class Pandephonium : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Pandephonium", "pandephonium");
            Game.Items.Rename("outdated_gun_mods:pandephonium", "nn:pandephonium");
            var behav = gun.gameObject.AddComponent<Pandephonium>();

            gun.SetShortDescription("");
            gun.SetLongDescription("");

            gun.SetupSprite(null, "pandephonium_idle_001", 8);

            gun.SetAnimationFPS(gun.shootAnimation, 15);

            gun.barrelOffset.transform.localPosition = new Vector3(3.37f, 0.93f, 0f);

            for (int i = 0; i < 9; i++)
            {
                gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
            }

            Projectile twentyDamageProjectile = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0]);
            twentyDamageProjectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(twentyDamageProjectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(twentyDamageProjectile);
            twentyDamageProjectile.baseData.damage = 3.5f;
            twentyDamageProjectile.baseData.speed *= 1f;
            twentyDamageProjectile.AdditionalScaleMultiplier *= 0.5f;

            int i2 = 1;
            foreach (ProjectileModule mod in gun.Volley.projectiles)
            {
                mod.ammoCost = 1;
                mod.numberOfShotsInClip = 3;
                mod.cooldownTime = 0.5f;
                mod.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
                mod.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;


                if (i2 < 5)
                {
                    float offsetStart = 0.2f;
                    offsetStart *= i2;
                    mod.positionOffset = new Vector2(0, -offsetStart);
                }
                else if (i2 > 5)
                {
                    float offsetStart = 0.2f;
                    offsetStart *= (i2 - 5);
                    mod.positionOffset = new Vector2(0, offsetStart);
                }
                if (mod != gun.DefaultModule) mod.ammoCost = 0;
                mod.angleVariance = 0f;
                mod.projectiles[0] = twentyDamageProjectile;
                i2++;
            }
            gun.reloadTime = 1f;
            gun.SetBaseMaxAmmo(200);

            gun.quality = PickupObject.ItemQuality.B;
            ETGMod.Databases.Items.Add(gun, null, "ANY");

            PandephoniumID = gun.PickupObjectId;
        }
        public static int PandephoniumID;
        public Pandephonium()
        {

        }
    }

}