using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.Misc;
using Alexandria.SoundAPI;
using SaveAPI;
using Dungeonator;
using Alexandria.Assetbundle;

namespace NevernamedsItems
{
    public class Dulcannon : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Dulcannon", "dulcannon");
            Game.Items.Rename("outdated_gun_mods:dulcannon", "nn:dulcannon");
            gun.gameObject.AddComponent<Dulcannon>();
            gun.SetShortDescription("At Wits End");
            gun.SetLongDescription("Summons a single bullet from each held gun upon reloading.\n\nDiscerning the true origins of this strange cannon have driven Gungeonologists mad with frustration for centuries.");

            gun.SetGunSprites("dulcannon", 8, false, 2);

            gun.SetAnimationFPS(gun.shootAnimation, 12);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(56) as Gun, true, false);
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(37) as Gun).gunSwitchGroup;

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.2f;
            gun.DefaultModule.cooldownTime = 1f;
            gun.DefaultModule.numberOfShotsInClip = 5;
            gun.DefaultModule.angleVariance = 7;
            gun.SetBarrel(37, 14);
            gun.SetBaseMaxAmmo(100);
            gun.ammo = 100;
            gun.gunClass = GunClass.SILLY;

            //BULLET STATS
            Projectile projectile = ProjectileSetupUtility.MakeProjectile(56, 25f);
            projectile.gameObject.name = "Dulcannon Projectile";
            projectile.AnimateProjectileBundle("DulcannonProjectile",
                    Initialisation.ProjectileCollection,
                    Initialisation.projectileAnimationCollection,
                    "DulcannonProjectile",
                    MiscTools.DupeList(new IntVector2(21, 10), 4), //Pixel Sizes
                    MiscTools.DupeList(true, 4), //Lightened
                    MiscTools.DupeList(tk2dBaseSprite.Anchor.MiddleCenter, 4), //Anchors
                    MiscTools.DupeList(true, 4), //Anchors Change Colliders
                    MiscTools.DupeList(false, 4), //Fixes Scales
                    MiscTools.DupeList<Vector3?>(null, 4), //Manual Offsets
                    MiscTools.DupeList<IntVector2?>(new IntVector2(21, 10), 4), //Override colliders
                    MiscTools.DupeList<IntVector2?>(null, 4), //Override collider offsets
                    MiscTools.DupeList<Projectile>(null, 4)); // Override to copy from
            projectile.baseData.speed = 20f;

            projectile.hitEffects.enemy = VFXToolbox.CreateBlankVFXPool(SharedVFX.SmoothLightBlueLaserCircleVFX);
            projectile.hitEffects.tileMapVertical = VFXToolbox.CreateBlankVFXPool(SharedVFX.BigDustCloud);
            projectile.hitEffects.tileMapHorizontal = VFXToolbox.CreateBlankVFXPool(SharedVFX.BigDustCloud);
            projectile.hitEffects.overrideMidairDeathVFX = SharedVFX.BigDustCloud;
            projectile.objectImpactEventName = "anvil";

            PierceProjModifier pierce = projectile.gameObject.AddComponent<PierceProjModifier>();
            pierce.penetration = 1;
            gun.DefaultModule.projectiles[0] = projectile;

            gun.gunHandedness = GunHandedness.AutoDetect;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(37) as Gun).muzzleFlashEffects;

            gun.AddClipSprites("dulcannon");

            gun.quality = PickupObject.ItemQuality.A;
            ETGMod.Databases.Items.Add(gun, null, "ANY");

            ID = gun.PickupObjectId;
        }
        public static int ID;
        public override void OnReloadedPlayer(PlayerController owner, Gun gun)
        {
            if (gun && owner && gun.ClipShotsRemaining == 0)
            {
                List<Projectile> ToSpawn = new List<Projectile>();
                foreach (Gun g in owner.inventory.AllGuns)
                {
                    if (g != null && g.DefaultModule != null && g.DefaultModule.GetCurrentProjectile() != null && g.DefaultModule.GetCurrentProjectile().GetComponent<BeamController>() == null && g.PickupObjectId != ID)
                    {
                        ToSpawn.Add(g.DefaultModule.GetCurrentProjectile());
                        if (g.DefaultModule.shootStyle == ProjectileModule.ShootStyle.Charged)
                        {
                            g.ammo = Math.Max(0, g.ammo - g.DefaultModule.ammoCost);
                        }
                        else
                        {
                            g.DecrementAmmoCost(g.DefaultModule);
                        }
                    }
                }
                float inc = 360f / (float)ToSpawn.Count;
                float iter = 0;
                foreach (Projectile proj in ToSpawn)
                {
                    StartOrbital(owner, proj, owner.CenterPosition, 4, inc * iter);
                    iter++;
                }
            }
            base.OnReloadedPlayer(owner, gun);
        }
        public void StartOrbital(PlayerController player, Projectile proj, Vector2 v, float radius, float angular)
        {
            GameObject orbital = proj.InstantiateAndFireInDirection(v, 0);
            Projectile component = orbital.GetComponent<Projectile>();
            if (component != null)
            {
                component.Owner = player;
                component.Shooter = player.specRigidbody;
                NoCollideBehaviour nocol = orbital.AddComponent<NoCollideBehaviour>();
                nocol.worksOnEnemies = false;
                component.specRigidbody.CollideWithTileMap = false;
                component.pierceMinorBreakables = true;

                component.ScaleByPlayerStats(player);
                player.DoPostProcessProjectile(component);

                component.baseData.speed = 20;
                component.baseData.range = float.MaxValue;
                component.UpdateSpeed();

                OrbitProjectileMotionModule orbitProjectileMotionModule = new OrbitProjectileMotionModule();

                orbitProjectileMotionModule.lifespan = 50;
                orbitProjectileMotionModule.MinRadius = 0.1f;
                orbitProjectileMotionModule.MaxRadius = 0.1f;
                orbitProjectileMotionModule.OrbitGroup = -66;
                component.OverrideMotionModule = orbitProjectileMotionModule;

                component.transform.localRotation = Quaternion.Euler(0f, 0f, component.transform.localRotation.z + angular);

                BulletLifeTimer timer = component.gameObject.AddComponent<BulletLifeTimer>();
                timer.secondsTillDeath = 6;

                component.StartCoroutine(LerpToMaxRadius(component, radius));
            }
        }
        private IEnumerator LerpToMaxRadius(Projectile proj, float radius)
        {
            if (!proj || proj.OverrideMotionModule == null) yield break;
            if (proj.OverrideMotionModule is OrbitProjectileMotionModule)
            {
                OrbitProjectileMotionModule motionMod = proj.OverrideMotionModule as OrbitProjectileMotionModule;

                float elapsed = 0f;
                float duration = 1f;
                while (elapsed < duration)
                {
                    elapsed += proj.LocalDeltaTime;
                    float t = elapsed / duration;
                    float currentRadius = Mathf.Lerp(0.1f, radius, t);

                    motionMod.m_radius = currentRadius;
                    yield return null;
                }
            }
            yield break;
        }
    }
}

