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
using Dungeonator;
using Alexandria.Assetbundle;

namespace NevernamedsItems
{
    public class Pinaka : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Pinaka", "pinaka");
            Game.Items.Rename("outdated_gun_mods:pinaka", "nn:pinaka");
            var behav = gun.gameObject.AddComponent<Pinaka>();
            gun.SetShortDescription("Restored");
            gun.SetLongDescription("An ancient and powerful divine weapon, broken and cast aside to win the hand of a Goddess in marriage."+"\n\nThough much of its power was lost when it was splintered in two, the weapon has slowly healed deep within the Gungeons chambers, awaiting a new master.");

            gun.SetGunSprites("pinaka", 8, false, 2);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(12) as Gun, true, false);
            gun.SetAnimationFPS(gun.chargeAnimation, 16);
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.chargeAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.chargeAnimation).loopStart = 6;
            gun.SetAnimationFPS(gun.reloadAnimation, 10);
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.reloadAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            gun.SetAnimationFPS(gun.shootAnimation, 12);

            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(8) as Gun).gunSwitchGroup;

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Charged;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1f;
            gun.DefaultModule.cooldownTime = 1f;
            gun.DefaultModule.angleVariance = 0;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(210) as Gun).muzzleFlashEffects;
            gun.DefaultModule.numberOfShotsInClip = 1;
            gun.SetBarrel(30, 25);
            gun.SetBaseMaxAmmo(30);
            gun.ammo = 30;

            gun.carryPixelOffset = new IntVector2(10, 0);

            gun.gunClass = GunClass.CHARGE;

            //BULLET STATS
            Projectile projectile = ProjectileSetupUtility.MakeProjectile(56, 20f); 
            projectile.baseData.speed = 35f;
            projectile.baseData.range = 1000f;

            projectile.AnimateProjectileBundle("PinakaProjectile",
                   Initialisation.ProjectileCollection,
                   Initialisation.projectileAnimationCollection,
                   "PinakaProjectile",
                   MiscTools.DupeList(new IntVector2(23, 5), 3), //Pixel Sizes
                   MiscTools.DupeList(true, 3), //Lightened
                   MiscTools.DupeList(tk2dBaseSprite.Anchor.MiddleCenter, 3), //Anchors
                   MiscTools.DupeList(true, 3), //Anchors Change Colliders
                   MiscTools.DupeList(false, 3), //Fixes Scales
                   MiscTools.DupeList<Vector3?>(null, 3), //Manual Offsets
                   MiscTools.DupeList<IntVector2?>(null, 3), //Override colliders
                   MiscTools.DupeList<IntVector2?>(null, 3), //Override collider offsets
                   MiscTools.DupeList<Projectile>(null, 3)); // Override to copy from  

            PierceProjModifier pierce = projectile.gameObject.GetOrAddComponent<PierceProjModifier>();
            pierce.penetration++;
            pierce.penetratesBreakables = true;

            projectile.hitEffects.overrideMidairDeathVFX = (PickupObjectDatabase.GetById(8) as Gun).DefaultModule.chargeProjectiles[1].Projectile.hitEffects.enemy.effects[0].effects[0].effect;
            projectile.hitEffects.alwaysUseMidair = true;

            projectile.gameObject.GetOrAddComponent<PinakaRailgun>();

            ProjectileModule.ChargeProjectile chargeProj = new ProjectileModule.ChargeProjectile
            {
                Projectile = projectile,
                ChargeTime = 0.5f,
            };
            gun.DefaultModule.chargeProjectiles = new List<ProjectileModule.ChargeProjectile> { chargeProj };

            gun.AddClipSprites("pinaka");

            gun.quality = PickupObject.ItemQuality.S;
            ETGMod.Databases.Items.Add(gun, false, "ANY");
            gun.SetTag("arrow_bolt_weapon");
            ID = gun.PickupObjectId;
        }
        public static int ID;
        public class PinakaRailgun : MonoBehaviour
        {
            public Projectile self;
            private void Start()
            {
                self = base.GetComponent<Projectile>();
                if (self)
                {
                    self.OnDestruction += OnDestroy;
                }
            }
            private void OnDestroy(Projectile me)
            {
                Vector2 pos = self.SafeCenter;
                RoomHandler room = self.GetAbsoluteRoom();
                if (room != null)
                {
                    List<AIActor> activeEnemies = room.GetActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear);
                    if (activeEnemies != null)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            if (activeEnemies.Count > 0)
                            {
                                AIActor Target = GetNearestActor(activeEnemies, pos);
                                if (Target != null)
                                {
                                    activeEnemies.Remove(Target);

                                    GameObject inst = Bejeweler.railgun.InstantiateAndFireTowardsPosition(pos, Target.CenterPosition, 0, 0);
                                    Projectile instproj = inst.GetComponent<Projectile>();
                                    if (instproj)
                                    {
                                        instproj.Owner = me.Owner;
                                        instproj.Shooter = me.Shooter;
                                        if (me.ProjectilePlayerOwner())
                                        {
                                            instproj.ScaleByPlayerStats(me.ProjectilePlayerOwner());
                                            me.ProjectilePlayerOwner().DoPostProcessProjectile(instproj);
                                        }
                                        instproj.RenderTilePiercingForSeconds(0.1f);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            private AIActor GetNearestActor(List<AIActor> enemies, Vector2 pos)
            {
                AIActor curClosests = null;
                float curDist = float.MaxValue;
                foreach (AIActor enemy in enemies)
                {
                    float x = Vector2.Distance(enemy.Position, pos);
                    if (x < curDist)
                    {
                        curClosests = enemy;
                        curDist = x;
                    }
                }
                return curClosests;
            }
        }
    }
}