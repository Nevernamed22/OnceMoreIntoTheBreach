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
using Dungeonator;
using Alexandria.Misc;

namespace NevernamedsItems
{
    public class TotemOfGundying : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Totem of Gundying", "totemofgundying");
            Game.Items.Rename("outdated_gun_mods:totem_of_gundying", "nn:totem_of_gundying");
            gun.gameObject.AddComponent<TotemOfGundying>();
            gun.SetShortDescription("Postmortal");
            gun.SetLongDescription("An ancient relic of a lost religion, holding this totem-shaped gun as one breathes their last breath allows them to return to the firefight, fit and ready for action.");

            gun.SetupSprite(null, "totemofgundying_idle_001", 8);

            gun.SetAnimationFPS(gun.shootAnimation, 9);
            gun.SetAnimationFPS(gun.reloadAnimation, 9);
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.reloadAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(89) as Gun).muzzleFlashEffects;
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(145) as Gun).gunSwitchGroup;

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0.8f;
            gun.DefaultModule.cooldownTime = 0.30f;
            gun.DefaultModule.numberOfShotsInClip = 4;
            gun.barrelOffset.transform.localPosition = new Vector3(34f / 16f, 25f / 16f, 0f);
            gun.SetBaseMaxAmmo(120);
            gun.gunClass = GunClass.PISTOL;

            //BULLET STATS
            Projectile projectile = gun.DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.transform.parent = gun.barrelOffset;
            projectile.baseData.damage = 9f;
            projectile.baseData.speed *= 1.5f;
            SlowDownOverTimeModifier slowdown = projectile.gameObject.AddComponent<SlowDownOverTimeModifier>();
            slowdown.extendTimeByRangeStat = true;
            slowdown.doRandomTimeMultiplier = true;
            slowdown.killAfterCompleteStop = true;
            slowdown.timeTillKillAfterCompleteStop = 0.5f;
            slowdown.timeToSlowOver = 0.5f;
            projectile.hitEffects = (PickupObjectDatabase.GetById(89) as Gun).DefaultModule.projectiles[0].hitEffects;
            projectile.gameObject.AddComponent<PierceProjModifier>().penetration = 1;
            projectile.gameObject.AddComponent<BounceProjModifier>().numberOfBounces = 1;

            projectile.SetProjectileSpriteRight($"totemofgundying_proj_001", 9, 9, false, tk2dBaseSprite.Anchor.MiddleCenter, 8, 8);
            projectile.AnimateProjectile(new List<string> {
                $"totemofgundying_proj_001",
                $"totemofgundying_proj_002",
            }, 12, tk2dSpriteAnimationClip.WrapMode.Loop,
            AnimateBullet.ConstructListOfSameValues(new IntVector2(9, 9), 2),
            AnimateBullet.ConstructListOfSameValues(true, 2),
            AnimateBullet.ConstructListOfSameValues(tk2dBaseSprite.Anchor.MiddleCenter, 2),
            AnimateBullet.ConstructListOfSameValues(true, 2),
            AnimateBullet.ConstructListOfSameValues(false, 2),
            AnimateBullet.ConstructListOfSameValues<Vector3?>(null, 2),
            AnimateBullet.ConstructListOfSameValues<IntVector2?>(null, 2),
            AnimateBullet.ConstructListOfSameValues<IntVector2?>(null, 2),
            AnimateBullet.ConstructListOfSameValues<Projectile>(null, 2), 0);

            gun.quality = PickupObject.ItemQuality.EXCLUDED;
            ETGMod.Databases.Items.Add(gun, false, "ANY");

            ID = gun.PickupObjectId;
        }
        public static int ID;
        protected override void OnPickedUpByPlayer(PlayerController player)
        {
            player.healthHaver.OnPreDeath += OwnerTookDamage;
            base.OnPickedUpByPlayer(player);
        }
        private void OwnerTookDamage(Vector2 dir)
        {
            if (Owner && Owner is PlayerController)
            {
                // ETGModConsole.Log($"Current gun: {((PlayerController)Owner).CurrentGun.DisplayName}");
                if (((PlayerController)Owner).healthHaver.GetCurrentHealth() < 0.5f && (((PlayerController)Owner).CurrentGun != null && ((PlayerController)Owner).CurrentGun.PickupObjectId == TotemOfGundying.ID) || (((PlayerController)Owner).CurrentSecondaryGun != null && ((PlayerController)Owner).CurrentSecondaryGun.PickupObjectId == TotemOfGundying.ID))
                {
                    if (((PlayerController)Owner).ForceZeroHealthState) { ((PlayerController)Owner).healthHaver.Armor += 6; }
                        ((PlayerController)Owner).healthHaver.FullHeal();
                    ((PlayerController)Owner).healthHaver.OnPreDeath -= OwnerTookDamage;
                    ((PlayerController)Owner).stats.RecalculateStats(((PlayerController)Owner));
                    ((PlayerController)Owner).ClearDeadFlags();
                    for (int i = 0; i < 20; i++)
                    {
                        GameObject toFire = (PickupObjectDatabase.GetById(TotemOfGundying.ID) as Gun).DefaultModule.projectiles[0].projectile.gameObject;
                        GameObject obj = UnityEngine.Object.Instantiate<GameObject>(toFire, ((PlayerController)Owner).CenterPosition, Quaternion.Euler(new Vector3(0f, 0f, UnityEngine.Random.Range(0, 360))));
                        Projectile proj = obj.GetComponent<Projectile>();
                        if (proj)
                        {
                            proj.Owner = Owner;
                            proj.Shooter = Owner.specRigidbody;
                            proj.baseData.damage *= ((PlayerController)Owner).stats.GetStatValue(PlayerStats.StatType.Damage);
                            proj.baseData.speed *= ((PlayerController)Owner).stats.GetStatValue(PlayerStats.StatType.ProjectileSpeed);
                            proj.baseData.range *= ((PlayerController)Owner).stats.GetStatValue(PlayerStats.StatType.RangeMultiplier);
                            proj.baseData.force *= ((PlayerController)Owner).stats.GetStatValue(PlayerStats.StatType.KnockbackMultiplier);
                            proj.BossDamageMultiplier *= ((PlayerController)Owner).stats.GetStatValue(PlayerStats.StatType.DamageToBosses);
                            proj.UpdateSpeed();
                            ((PlayerController)Owner).DoPostProcessProjectile(proj);
                        }
                    }
                    for (int i = ((PlayerController)Owner).inventory.AllGuns.Count; i >= 0; i--)
                    {
                        if (((PlayerController)Owner).inventory.AllGuns[i].PickupObjectId == TotemOfGundying.ID)
                        {
                            //((PlayerController)Owner).inventory.AllGuns[i].GetComponent<AdvancedDualWieldSynergyProcessor>().DisableEffect();
                            ((PlayerController)Owner).RemoveItemFromInventory(((PlayerController)Owner).inventory.AllGuns[i]);
                        }
                    }
                }

            }
        }
        protected override void OnPostDroppedByPlayer(PlayerController player)
        {
            ((PlayerController)Owner).healthHaver.OnPreDeath -= OwnerTookDamage;
            base.OnPostDroppedByPlayer(player);
        }
    }

}