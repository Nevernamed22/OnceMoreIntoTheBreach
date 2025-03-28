﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;
using MonoMod.RuntimeDetour;
using System.Reflection;

namespace NevernamedsItems
{
    public class BulletBoots : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<BulletBoots>(
            "Bullet Boots",
            "Run and Gun",
            "Fires bullets in the direction you're running." + "\n\nThe best offence is running directly at the enemy completely unguarded.",
            "bulletboots_icon");
            item.AddPassiveStatModifier( PlayerStats.StatType.MovementSpeed, 1, StatModifier.ModifyMethod.ADDITIVE);
            item.CanBeDropped = true;
            item.quality = PickupObject.ItemQuality.D;

            Projectile projectile2 = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(56) as Gun).DefaultModule.projectiles[0]);
            projectile2.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile2.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile2);
            projectile2.baseData.speed *= 1.2f;
            projectile2.baseData.range *= 0.5f;
            projectile2.SetProjectileSprite("bulletboots_projectile", 19, 9, true, tk2dBaseSprite.Anchor.MiddleCenter, 17, 7);
            BulletBootsProjectile = projectile2;

            BulletBootsID = item.PickupObjectId;
        }
        public static Projectile BulletBootsProjectile;
        public static int BulletBootsID;
        bool onCooldown = false;
        float cooldownSeconds = 0.25f;
        public override void Update()
        {
            if (Owner != null && Owner.IsInCombat)
            {
                if (Owner.specRigidbody.Velocity != Vector2.zero)
                {
                    if (!onCooldown)
                    {
                        onCooldown = true;

                        float dir = Owner.LastCommandedDirection.ToAngle();


                        GameObject gameObject = SpawnManager.SpawnProjectile(BulletBootsProjectile.gameObject, base.Owner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, dir), true);
                        Projectile component = gameObject.GetComponent<Projectile>();
                        if (component != null)
                        {
                            component.Owner = Owner;
                            component.Shooter = Owner.specRigidbody;
                            component.TreatedAsNonProjectileForChallenge = true;
                            component.baseData.damage *= Owner.stats.GetStatValue(PlayerStats.StatType.Damage);
                            component.baseData.speed *= Owner.stats.GetStatValue(PlayerStats.StatType.ProjectileSpeed);
                            component.baseData.force *= Owner.stats.GetStatValue(PlayerStats.StatType.KnockbackMultiplier);
                            component.UpdateSpeed();
                            Owner.DoPostProcessProjectile(component);
                        }

                        Invoke("ResetCooldown", cooldownSeconds);

                    }
                }
            }
            base.Update();
        }
        private void ResetCooldown()
        {
            onCooldown = false;
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            return base.Drop(player);
        }
    }

}