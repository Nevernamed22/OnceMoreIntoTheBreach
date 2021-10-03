﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using ItemAPI;
using MonoMod.RuntimeDetour;
using System.Reflection;

namespace NevernamedsItems
{
    public class BigShot : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Big Shot", "bigshot");
            Game.Items.Rename("outdated_gun_mods:big_shot", "nn:big_shot");
            gun.gameObject.AddComponent<BigShot>();
            gun.SetShortDescription("Now's Your Chance!");
            gun.SetLongDescription("The sign4ture weap0n of a [ONCE IN A LIFETIME OPPORTUNITY] that came to the Gungeon [ON AN ALL EXPENSES PAID HOLIDAY] seeking $$fr33$$! KROMER." + "\n\nYou're fill3d with [Hyperlink Blocked.].");

            gun.SetupSprite(null, "bigshot_idle_001", 8);

            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(519) as Gun).gunSwitchGroup;
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1f;
            gun.DefaultModule.cooldownTime = 0.2f;
            gun.DefaultModule.numberOfShotsInClip = 5;
            gun.barrelOffset.transform.localPosition = new Vector3(3.06f, 1.18f, 0f);
            gun.SetBaseMaxAmmo(100);
            gun.ammo = 100;
            gun.gunClass = GunClass.SILLY;

            //BULLET STATS
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage = 19.99f;
            projectile.baseData.speed *= 0.7f;
            projectile.baseData.range *= 2f;
            //ANIMATE BULLET
            projectile.AnimateProjectile(new List<string> {
                "bigshot_orangeproj_001",
                "bigshot_orangeproj_002",
                "bigshot_orangeproj_003",
            }, 10, true, new List<IntVector2> {
                 new IntVector2(17, 16), //1
                  new IntVector2(17, 16), //2
                   new IntVector2(17, 16), //3
            },
            AnimateBullet.ConstructListOfSameValues(true, 3),
            AnimateBullet.ConstructListOfSameValues(tk2dBaseSprite.Anchor.MiddleCenter, 3),
            AnimateBullet.ConstructListOfSameValues(true, 3),
            AnimateBullet.ConstructListOfSameValues(false, 3),
            AnimateBullet.ConstructListOfSameValues<Vector3?>(null, 3),
            AnimateBullet.ConstructListOfSameValues<IntVector2?>(null, 3),
            AnimateBullet.ConstructListOfSameValues<IntVector2?>(null, 3),
            AnimateBullet.ConstructListOfSameValues<Projectile>(null, 3));
            projectile.gameObject.AddComponent<BigShotProjectileComp>();
            projectile.DestroyMode = Projectile.ProjectileDestroyMode.BecomeDebris;
            projectile.SetProjectileSpriteRight("bigshot_orangeproj_001", 17, 16, true, tk2dBaseSprite.Anchor.MiddleCenter, 17, 16);

            Projectile projectile2 = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0]);
            projectile2.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile2.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile2);
            projectile2.baseData.damage = 19.99f;
            projectile2.baseData.speed *= 0.7f;
            projectile2.baseData.range *= 2f;

            //ANIMATE BULLET
            projectile2.AnimateProjectile(new List<string> {
                "bigshot_pinkproj_001",
                "bigshot_pinkproj_002",
                "bigshot_pinkproj_003",
            }, 16, true, new List<IntVector2> {
                new IntVector2(17, 15), //1
                new IntVector2(17, 15), //2
                new IntVector2(17, 15), //3
            }, AnimateBullet.ConstructListOfSameValues(true, 3),
            AnimateBullet.ConstructListOfSameValues(tk2dBaseSprite.Anchor.MiddleCenter, 3),
            AnimateBullet.ConstructListOfSameValues(true, 3),
            AnimateBullet.ConstructListOfSameValues(false, 3),
            AnimateBullet.ConstructListOfSameValues<Vector3?>(null, 3),
            AnimateBullet.ConstructListOfSameValues<IntVector2?>(null, 3),
            AnimateBullet.ConstructListOfSameValues<IntVector2?>(null, 3),
            AnimateBullet.ConstructListOfSameValues<Projectile>(null, 3));
            projectile2.SetProjectileSpriteRight("bigshot_pinkproj_001", 17, 15, true, tk2dBaseSprite.Anchor.MiddleCenter, 17, 15);
            projectile2.DestroyMode = Projectile.ProjectileDestroyMode.BecomeDebris;
            projectile2.gameObject.AddComponent<BigShotProjectileComp>();
            gun.DefaultModule.projectiles.Add(projectile2);


            gun.quality = PickupObject.ItemQuality.B;
            ETGMod.Databases.Items.Add(gun, null, "ANY");

            BigShotID = gun.PickupObjectId;
        }
        public static int BigShotID;
        public override void PostProcessProjectile(Projectile projectile)
        {
            if (projectile && projectile.ProjectilePlayerOwner())
            {
                if (projectile.ProjectilePlayerOwner().PlayerHasActiveSynergy("Hyperlink Blocked"))
                {
                    if (UnityEngine.Random.value <= 0.35f)
                    {
                        HungryProjectileModifier hungry = projectile.gameObject.AddComponent<HungryProjectileModifier>();
                        hungry.HungryRadius = 1.5f;
                        projectile.AdjustPlayerProjectileTint(ExtendedColours.purple, 1);
                    }
                }
                if (projectile.ProjectilePlayerOwner().PlayerHasActiveSynergy("BIGGEST SHOT"))
                {
                    projectile.RuntimeUpdateScale(2);
                    projectile.baseData.damage *= 1.25f;
                }
            }
            base.PostProcessProjectile(projectile);
        }
        public bool hasRequestedHeal;
        protected override void Update()
        {
            if (!hasRequestedHeal && gun && gun.GunPlayerOwner())
            {
                if (Input.GetKey(KeyCode.F1))
                {
                    AkSoundEngine.PostEvent("Play_OBJ_heart_heal_01", gun.GunPlayerOwner().gameObject);
                    gun.GunPlayerOwner().PlayEffectOnActor((PickupObjectDatabase.GetById(73).GetComponent<HealthPickup>().healVFX), Vector3.zero, true, false, false);
                    if (gun.GunPlayerOwner().characterIdentity == PlayableCharacters.Robot)
                    {
                        gun.GunPlayerOwner().healthHaver.Armor += 1;
                    }
                    else
                    {
                        gun.GunPlayerOwner().healthHaver.ApplyHealing(1);
                    }
                    hasRequestedHeal = true;
                }
            }
            base.Update();
        }
        public BigShot()
        {

        }
    }
    public class BigShotProjectileComp : MonoBehaviour
    {
        public BigShotProjectileComp()
        {

        }
        private List<string> dialogue = new List<string>()
        {
            "NOW'S YOUR CHANCE TO BE A BIG SHOT",
            "LOW, LOW, PRICE!",
            "BIGGER AND BETTER THAN EVER",
            "All Alone On A Late Night?",
            "BIG SHOT!!!!!",
            "Hyperlink Blocked.",
            "FREE KROMER",
            "WELL HAVE I GOT A DEAL FOR YOU!!",
            "$VALUES$",
            "$$DEALS$",
            "$'CHEAP'",
            "$$49.998",
            "Press F1 for Help"
        };
        private void OnHitEnemy(Projectile bullet, SpeculativeRigidbody enemy, bool fatal)
        {
            if (bullet && enemy)
            {
                if (UnityEngine.Random.value <= 0.05f)
                {
                    VFXToolbox.DoStringSquirt(BraveUtility.RandomElement(dialogue), enemy.Position.GetPixelVector2(), ExtendedColours.honeyYellow);
                    if (bullet.ProjectilePlayerOwner() && bullet.ProjectilePlayerOwner().PlayerHasActiveSynergy("De4l 0f 4 Lif3tim3"))
                    {
                        LootEngine.SpawnCurrency(enemy.Position.GetPixelVector2(), UnityEngine.Random.Range(2, 5));
                    }
                }

            }
        }
        private void Start()
        {
            self = base.GetComponent<Projectile>();
            if (self)
            {
                self.OnHitEnemy += OnHitEnemy;
                int random = UnityEngine.Random.Range(1, 7);
                switch (random)
                {
                    case 1:
                        PierceProjModifier piercing = self.gameObject.GetOrAddComponent<PierceProjModifier>();
                        piercing.penetration += 2;
                        break;
                    case 2:
                        BounceProjModifier bouncing = self.gameObject.GetOrAddComponent<BounceProjModifier>();
                        bouncing.numberOfBounces += 2;
                        break;
                    case 3:
                        RemoteBulletsProjectileBehaviour remote = self.gameObject.GetOrAddComponent<RemoteBulletsProjectileBehaviour>();
                        break;
                    case 4:
                        AngryBulletsProjectileBehaviour angry = self.gameObject.GetOrAddComponent<AngryBulletsProjectileBehaviour>();
                        break;
                    case 5:
                        OrbitalBulletsBehaviour orbital = self.gameObject.gameObject.GetOrAddComponent<OrbitalBulletsBehaviour>();
                        break;
                    case 6:
                        HomingModifier homing = self.gameObject.gameObject.AddComponent<HomingModifier>(); homing.HomingRadius = 100f;
                        break;
                }
            }
        }
        private Projectile self;
    }
}
