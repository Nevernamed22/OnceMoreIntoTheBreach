using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;
using MonoMod.RuntimeDetour;
using System.Reflection;
using Alexandria.Assetbundle;

namespace NevernamedsItems
{
    public class AM0SpreadForme : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("AM-0 Spread Forme", "am0spreadforme");
            Game.Items.Rename("outdated_gun_mods:am0_spread_forme", "nn:am0+spreadshot");
            gun.gameObject.AddComponent<AM0SpreadForme>();
            gun.SetShortDescription("Fires Ammunition");
            gun.SetLongDescription("" + "\n\nThis gun is comically stuffed with whole ammo boxes.");
            gun.SetGunSprites("am0spreadforme", 8, true);

            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(519) as Gun).gunSwitchGroup;

            for (int i = 0; i < 3; i++)
            {
                gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
            }

            //GUN STATS
            gun.reloadTime = 0.8f;
            gun.barrelOffset.transform.localPosition = new Vector3(2.43f, 0.75f, 0f);
            gun.SetBaseMaxAmmo(500);
            gun.ammo = 500;

            

            foreach (ProjectileModule mod in gun.Volley.projectiles)
            {
                mod.angleVariance = 35;
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
                projectile.baseData.damage *= 0.55f;
                projectile.baseData.speed *= 0.7f;
                projectile.baseData.range *= 2f;
                if (mod != gun.DefaultModule) { mod.ammoCost = 0; }

                projectile.AnimateProjectileBundle("AM0SpreadProjectile",
                   Initialisation.ProjectileCollection,
                   Initialisation.projectileAnimationCollection,
                   "AM0SpreadProjectile",
                   new List<IntVector2> {
                        new IntVector2(11, 14), //1
                        new IntVector2(13, 16), //2            All frames are 13x16 except select ones that are 11-14
                        new IntVector2(13, 16), //3
                        new IntVector2(13, 16),//4
                        new IntVector2(11, 14),//5
                        new IntVector2(13, 16),//6
                        new IntVector2(13, 16),//7
                        new IntVector2(13, 16),//8
                        new IntVector2(11, 14),//9
                        new IntVector2(13, 16),//10
                        new IntVector2(13, 16),//11
                        new IntVector2(13, 16),//12
                        new IntVector2(11, 14),//13
                        new IntVector2(13, 16),//14
                        new IntVector2(13, 16),//15
                        new IntVector2(13, 16),//16
                   }, //Pixel Sizes
                   MiscTools.DupeList(false, 16), //Lightened
                   MiscTools.DupeList(tk2dBaseSprite.Anchor.MiddleCenter, 16), //Anchors
                   MiscTools.DupeList(true, 16), //Anchors Change Colliders
                   MiscTools.DupeList(false, 16), //Fixes Scales
                   MiscTools.DupeList<Vector3?>(null, 16), //Manual Offsets
                   MiscTools.DupeList<IntVector2?>(null, 16), //Override colliders
                   MiscTools.DupeList<IntVector2?>(null, 16), //Override collider offsets
                   MiscTools.DupeList<Projectile>(null, 16)); // Override to copy from                  
            }

            gun.quality = PickupObject.ItemQuality.EXCLUDED;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            gun.SetName("AM-0");

            AM0SpreadFormeID = gun.PickupObjectId;
        }
        public static int AM0SpreadFormeID;
        public AM0SpreadForme()
        {

        }
    }
}