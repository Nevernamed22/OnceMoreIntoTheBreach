﻿using System;
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
using Alexandria.Misc;

namespace NevernamedsItems
{

    public class DoomBoom : AdvancedGunBehavior
    {
        public static void Add()
        {

            Gun gun = ETGMod.Databases.Items.NewGun("Doom Boom", "doomboom");
            Game.Items.Rename("outdated_gun_mods:doom_boom", "nn:doom_boom");
            var behav = gun.gameObject.AddComponent<DoomBoom>();

            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(601) as Gun).gunSwitchGroup;


            gun.SetShortDescription("Momentum Mori");
            gun.SetLongDescription("Packs an explosive punch so powerful that it knocks enemies souls out of their bodies." + "\n\nIn Gundead culture, it is considered a high honour to be buried beneath funerary-grade heavy munitions such as this.");
            gun.SetupSprite(null, "doomboom_idle_001", 8);


            gun.SetAnimationFPS(gun.shootAnimation, 13);
            gun.SetAnimationFPS(gun.reloadAnimation, 10);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);

            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.2f;
            gun.DefaultModule.cooldownTime = 0.75f;
            gun.muzzleFlashEffects = VFXToolbox.CreateVFXPool("DoomBoom Muzzleflash",
               new List<string>()
               {
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/doomboom_muzzle_001",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/doomboom_muzzle_002",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/doomboom_muzzle_003",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/doomboom_muzzle_004",
                    "NevernamedsItems/Resources/MiscVFX/GunVFX/doomboom_muzzle_005",
               },
               13, //FPS
               new IntVector2(36, 41), //Dimensions
               tk2dBaseSprite.Anchor.MiddleLeft, //Anchor
               false, //Uses a Z height off the ground
               0, //The Z height, if used
               false,
              VFXAlignment.Fixed
                 );
            gun.DefaultModule.numberOfShotsInClip = 2;
            gun.barrelOffset.transform.localPosition = new Vector3(34f / 16f, 19f / 16f, 0f);
            gun.SetBaseMaxAmmo(50);
            gun.quality = PickupObject.ItemQuality.A;
            gun.gunClass = GunClass.EXPLOSIVE;

            //BULLET STATS
            Projectile projectile = ProjectileUtility.SetupProjectile(86);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage = 20f;
            projectile.baseData.speed *= 0.8f;
            projectile.hitEffects.overrideMidairDeathVFX = EasyVFXDatabase.BigWhitePoofVFX;
            projectile.hitEffects.alwaysUseMidair = true;
            projectile.gameObject.AddComponent<DoomExplosion>();

            vfx = VFXToolbox.CreateVFX("DoomBoom Explosion",
                 new List<string>()
                 {
                    "NevernamedsItems/Resources/MiscVFX/Explosions/doomboom_explosionvfx_001",
                    "NevernamedsItems/Resources/MiscVFX/Explosions/doomboom_explosionvfx_002",
                    "NevernamedsItems/Resources/MiscVFX/Explosions/doomboom_explosionvfx_003",
                    "NevernamedsItems/Resources/MiscVFX/Explosions/doomboom_explosionvfx_004",
                    "NevernamedsItems/Resources/MiscVFX/Explosions/doomboom_explosionvfx_005",
                    "NevernamedsItems/Resources/MiscVFX/Explosions/doomboom_explosionvfx_006",
                 },
                13, //FPS
                 new IntVector2(88, 82), //Dimensions
                 tk2dBaseSprite.Anchor.MiddleCenter, //Anchor
                 true, //Uses a Z height off the ground
                 0.4f //The Z height, if used
                   );

            projectile.AnimateProjectile(new List<string> {
                "doomboom_projectile_001",
                "doomboom_projectile_002",
                "doomboom_projectile_003",
                "doomboom_projectile_004",
                "doomboom_projectile_005",
                "doomboom_projectile_006",
                "doomboom_projectile_007",
                "doomboom_projectile_008",
            }, 8, tk2dSpriteAnimationClip.WrapMode.Loop,
            AnimateBullet.ConstructListOfSameValues(new IntVector2(16, 17), 8),
            AnimateBullet.ConstructListOfSameValues(false, 8),
            AnimateBullet.ConstructListOfSameValues(tk2dBaseSprite.Anchor.MiddleCenter, 8),
            AnimateBullet.ConstructListOfSameValues(true, 8),
            AnimateBullet.ConstructListOfSameValues(false, 8),
            AnimateBullet.ConstructListOfSameValues<Vector3?>(null, 8),
            AnimateBullet.ConstructListOfSameValues<IntVector2?>(new IntVector2(13, 13), 8),
            AnimateBullet.ConstructListOfSameValues<IntVector2?>(null, 8),
            AnimateBullet.ConstructListOfSameValues<Projectile>(null, 8), 0);

            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = "white";

            ETGMod.Databases.Items.Add(gun, false, "ANY");
            ID = gun.PickupObjectId;
            gun.TrimGunSprites();
        }
        public static GameObject vfx;
        public static int ID;
        public class DoomExplosion : MonoBehaviour
        {
            public Projectile self;
            public void Start()
            {
                self = base.GetComponent<Projectile>();
                self.OnDestruction += OnDestroy;
                self.OnHitEnemy += OnHit;
            }
            public void OnHit(Projectile proj, SpeculativeRigidbody enemy, bool fatal)
            {
                if (fatal && enemy)
                {
                    DoGhost(enemy, proj.LastPosition);
                }
            }
            public void OnDestroy(Projectile fortunateson)
            {
                GameManager.Instance.StartCoroutine(Explode(self, self.LastPosition));
            }
            public void DoGhost(SpeculativeRigidbody enemy, Vector2 explosionPos)
            {
                GameObject gemy = StandardisedProjectiles.ghost.InstantiateAndFireInDirection(
                      enemy.UnitCenter,
                      (explosionPos - enemy.UnitCenter).ToAngle());
                if (self.Owner)
                {
                    Projectile proj = gemy.GetComponent<Projectile>();
                    proj.Owner = self.Owner;
                    proj.Shooter = self.Owner.specRigidbody;
                    if (self.ProjectilePlayerOwner())
                    {
                        self.ProjectilePlayerOwner().DoPostProcessProjectile(proj);
                        proj.ScaleByPlayerStats(self.ProjectilePlayerOwner());
                    }
                    proj.specRigidbody.RegisterGhostCollisionException(enemy);
                }
            }
            public IEnumerator Explode(Projectile source, Vector2 position)
            {
                float damageRadius = 2.5f;
                float bulletDeletionSqrRadius = damageRadius * damageRadius;

                GameObject gameObject;
                gameObject = SpawnManager.SpawnVFX(vfx, position, Quaternion.identity);

                GameObject gameObject2 = new GameObject("SoundSource");
                gameObject2.transform.position = position;
                AkSoundEngine.PostEvent("Play_WPN_grenade_blast_01", gameObject2);
                UnityEngine.Object.Destroy(gameObject2, 5f);

                yield return new WaitForSeconds(0.1f);
                List<HealthHaver> allHealth = StaticReferenceManager.AllHealthHavers;
                if (allHealth != null)
                {
                    for (int j = 0; j < allHealth.Count; j++)
                    {
                        HealthHaver healthHaver = allHealth[j];
                        if (healthHaver && healthHaver.aiActor && healthHaver.aiActor.HasBeenEngaged && healthHaver.aiActor.CompanionOwner == null && !healthHaver.isPlayerCharacter)
                        {

                            if (position.GetAbsoluteRoom() == allHealth[j].transform.position.GetAbsoluteRoom())
                            {
                                for (int k = 0; k < healthHaver.NumBodyRigidbodies; k++)
                                {
                                    SpeculativeRigidbody bodyRigidbody = healthHaver.GetBodyRigidbody(k);
                                    Vector2 vector = healthHaver.transform.position.XY();
                                    Vector2 vector2 = vector - position;
                                    float num;
                                    if (bodyRigidbody.HitboxPixelCollider != null)
                                    {
                                        vector = bodyRigidbody.HitboxPixelCollider.UnitCenter;
                                        vector2 = vector - position;
                                        num = BraveMathCollege.DistToRectangle(position, bodyRigidbody.HitboxPixelCollider.UnitBottomLeft, bodyRigidbody.HitboxPixelCollider.UnitDimensions);
                                    }
                                    else
                                    {
                                        vector = healthHaver.transform.position.XY();
                                        vector2 = vector - position;
                                        num = vector2.magnitude;
                                    }

                                    if (num < damageRadius)
                                    {
                                        string enemiesString = StringTableManager.GetEnemiesString("#EXPLOSION", -1);
                                        bool wasAlive = !healthHaver.IsDead;
                                        if (wasAlive)
                                        {
                                            healthHaver.ApplyDamage(25, vector2, enemiesString, CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
                                        }
                                        KnockbackDoer knockbackDoer = healthHaver.knockbackDoer;
                                        knockbackDoer.ApplyKnockback(vector2.normalized, 10, false);
                                        if (wasAlive && healthHaver.IsDead && healthHaver.specRigidbody)
                                        {
                                            DoGhost(healthHaver.specRigidbody, position);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                List<MinorBreakable> allBreakables = StaticReferenceManager.AllMinorBreakables;
                if (allBreakables != null)
                {
                    for (int l = 0; l < allBreakables.Count; l++)
                    {
                        MinorBreakable minorBreakable = allBreakables[l];
                        if (minorBreakable && !minorBreakable.resistsExplosions && !minorBreakable.OnlyBrokenByCode)
                        {
                            Vector2 vector3 = minorBreakable.CenterPoint - position;
                            if (vector3.sqrMagnitude < 9)
                            {
                                minorBreakable.Break(vector3.normalized);
                            }
                        }
                    }
                }
                if (GameManager.Instance.MainCameraController != null)
                {
                    GameManager.Instance.MainCameraController.DoScreenShake((PickupObjectDatabase.GetById(37) as Gun).gunScreenShake, new Vector2?(position), false);
                }
                for (int i = 0; i < StaticReferenceManager.AllDebris.Count; i++)
                {
                    Vector2 vector = StaticReferenceManager.AllDebris[i].transform.position.XY();
                    Vector2 vector2 = vector - position;
                    float sqrMagnitude = vector2.sqrMagnitude;
                    if (sqrMagnitude < 9)
                    {
                        float num2 = 1f - vector2.magnitude / 9;
                        StaticReferenceManager.AllDebris[i].ApplyVelocity(vector2.normalized * num2 * 10 * (1f + UnityEngine.Random.value / 5f));
                    }
                }
            }
        }
    }
}