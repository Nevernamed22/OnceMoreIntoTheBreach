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

    public class Wex : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Wex", "wex");
            Game.Items.Rename("outdated_gun_mods:wex", "nn:wex");
            gun.gameObject.AddComponent<Wex>();
            gun.SetShortDescription("Flammensoldat");
            gun.SetLongDescription("The wechselapparat is a reliable flamethrower made out of left over car parts and scrap metal. Famous for looking (and nominally behaving) like a pool floatie.");

            gun.SetGunSprites("wex", 8, false, 2);

            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.reloadAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
            gun.gunSwitchGroup = "NN_WPN_Flamethrower";

            gun.SetAnimationFPS(gun.shootAnimation, 15);
            gun.SetAnimationFPS(gun.reloadAnimation, 13);
            gun.SetAnimationFPS(gun.idleAnimation, 5);

            for (int i = 0; i < 2; i++)
            {
                gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(181) as Gun, true, false);
            }

            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(83) as Gun).muzzleFlashEffects;

            //GUN STATS
            foreach (ProjectileModule mod in gun.Volley.projectiles)
            {

                mod.ammoCost = 1;
                mod.shootStyle = ProjectileModule.ShootStyle.Automatic;
                mod.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
                mod.cooldownTime = 0.05f;
                mod.angleVariance = 0f;
                mod.numberOfShotsInClip = 70;

                ImprovedHelixProjectile projectile = DataCloners.CopyFields<ImprovedHelixProjectile>(StandardisedProjectiles.flamethrower.InstantiateAndFakeprefab());
                projectile.helixAmplitude = 2;

                projectile.GetComponent<ParticleShitter>().particlesPerSecond = 10;
                mod.projectiles[0] = projectile;

                if (mod != gun.DefaultModule)
                {
                    projectile.startInverted = true;
                    mod.ammoCost = 0;
                }
            }

            gun.SetBaseMaxAmmo(1000);
            gun.ammo = 1000;
            gun.gunClass = GunClass.FIRE;
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = "Y-Beam Laser";

            gun.carryPixelOffset = new IntVector2(-4, 3);
            gun.carryPixelDownOffset = new IntVector2(-2, 4);
            gun.carryPixelUpOffset = new IntVector2(8, -5);

            gun.reloadTime = 1f;
            gun.SetBarrel(43, 21);

            gun.quality = PickupObject.ItemQuality.B;
            ETGMod.Databases.Items.Add(gun, null, "ANY");


            ID = gun.PickupObjectId;
        }
        
        public static int ID;
    }
}