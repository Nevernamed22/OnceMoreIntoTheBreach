using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;
using SaveAPI;
using Alexandria.Misc;

namespace NevernamedsItems
{
    public class Rewarp : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Rewarp", "rewarp");
            Game.Items.Rename("outdated_gun_mods:rewarp", "nn:rewarp");
            var behav = gun.gameObject.AddComponent<Rewarp>();

            gun.SetShortDescription("Time Twister");
            gun.SetLongDescription("Summons bullets from an alternate timeline."+"\n\nThis was pulled through a tear in the curtain along with the Killithid incursion- though it is not of their craftmanship.");

            gun.SetupSprite(null, "rewarp_idle_001", 8);

            gun.SetAnimationFPS(gun.shootAnimation, 12);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(59) as Gun, true, false);
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(59) as Gun).gunSwitchGroup;

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 2f;
            gun.DefaultModule.cooldownTime = 0.8f;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(59) as Gun).muzzleFlashEffects;

            gun.DefaultModule.numberOfShotsInClip = 10;
            gun.barrelOffset.transform.localPosition = new Vector3(22f / 16f, 6f / 16f, 0f);
            gun.SetBaseMaxAmmo(100);
            gun.gunClass = GunClass.SILLY;

            //BULLET STATS
            Projectile projectile = gun.DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            projectile.gameObject.AddComponent<SideshooterHandler>();
            projectile.baseData.damage = 20;
            PierceProjModifier piercing = projectile.gameObject.AddComponent<PierceProjModifier>();
            piercing.penetration = 1;
            gun.DefaultModule.projectiles[0] = projectile;

            projectile.pierceMinorBreakables = true;          

            gun.quality = PickupObject.ItemQuality.B;
            ETGMod.Databases.Items.Add(gun, false, "ANY");

            ID = gun.PickupObjectId;

            subProjPrefab = DataCloners.CopyFields<TachyonProjectile>(Instantiate((PickupObjectDatabase.GetById(59) as Gun).DefaultModule.projectiles[0]));
            subProjPrefab.gameObject.MakeFakePrefab();
            subProjPrefab.pierceMinorBreakables = true;
            subProjPrefab.PenetratesInternalWalls = true;
        }
        public static Projectile subProjPrefab;
        public static int ID;
        public class SideshooterHandler : MonoBehaviour
        {
            private Projectile self;
            private void Start()
            {
                self = base.GetComponent<Projectile>();
            }
            private float lastCheckedDistance = 0;
            private void Update()
            {
                if (self)
                {
                    if (self.m_distanceElapsed >= lastCheckedDistance + 2)
                    {
                        Fire();
                        lastCheckedDistance += 2;
                    }
                }
            }
            private void Fire()
            {
                for (int i = 0; i < 2; i++)
                {
                    Projectile newproj = Rewarp.subProjPrefab.InstantiateAndFireInDirection(self.m_lastPosition, self.Direction.ToAngle() + (90f + (180 * i))).GetComponent<Projectile>();
                    newproj.Owner = self.Owner;
                    newproj.Shooter = self.Shooter;
                    if (self.ProjectilePlayerOwner())
                    {
                        newproj.ScaleByPlayerStats(self.ProjectilePlayerOwner());
                        self.ProjectilePlayerOwner().DoPostProcessProjectile(newproj);
                    }                    
                }
            }
        }
    }
}