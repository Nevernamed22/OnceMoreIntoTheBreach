﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dungeonator;
using Alexandria.ItemAPI;
using UnityEngine;
using Alexandria.Assetbundle;

namespace NevernamedsItems
{
    class HoneyPot : PlayerItem
    {
        public static void Init()
        {
            PlayerItem item = ItemSetup.NewItem<HoneyPot>(
           "Honey Pot",
           "A Little Something",
           "A handy, throwable pot of sticky honey." + "\n\nSome Gundead tell whispers of buzzing coming from the Oubliette...",
           "honeypot_icon") as PlayerItem;

            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 250);
            item.consumable = false;
            item.quality = ItemQuality.D;

            PotProjectile = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0]);
            PotProjectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(PotProjectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(PotProjectile);
            PotProjectile.baseData.damage *= 0f;
            PotProjectile.baseData.speed *= 0.5f;
            PotProjectile.baseData.range *= 0.5f;
            PotProjectile.collidesWithEnemies = false;
            PotProjectile.collidesWithPlayer = false;
            PotProjectile.collidesWithProjectiles = false;
            PotProjectile.pierceMinorBreakables = true;
            PierceProjModifier keepComponent = PotProjectile.gameObject.GetOrAddComponent<PierceProjModifier>();
            keepComponent.penetratesBreakables = true;
            keepComponent.penetration += 100;
            BounceProjModifier Bouncing = PotProjectile.gameObject.GetOrAddComponent<BounceProjModifier>();
            Bouncing.numberOfBounces += 100;

            PotProjectile.AnimateProjectileBundle("HoneyPotProjectile",
                    Initialisation.ProjectileCollection,
                    Initialisation.projectileAnimationCollection,
                    "HoneyPotProjectile",
                    new List<IntVector2> { new IntVector2(19, 16), new IntVector2(16, 19), new IntVector2(19, 16), new IntVector2(16, 19) }, //Pixel Sizes
                    MiscTools.DupeList(false, 4), //Lightened
                    MiscTools.DupeList(tk2dBaseSprite.Anchor.MiddleCenter, 4), //Anchors
                    MiscTools.DupeList(true, 4), //Anchors Change Colliders
                    MiscTools.DupeList(false, 4), //Fixes Scales
                    MiscTools.DupeList<Vector3?>(null, 4), //Manual Offsets
                    MiscTools.DupeList<IntVector2?>(new IntVector2(14, 14), 4), //Override colliders
                    MiscTools.DupeList<IntVector2?>(null, 4), //Override collider offsets
                    MiscTools.DupeList<Projectile>(null, 4)); // Override to copy from             
        }
        private static Projectile PotProjectile;
        public override void DoEffect(PlayerController user)
        {
            GameObject gameObject = SpawnManager.SpawnProjectile(PotProjectile.gameObject, user.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (user.CurrentGun == null) ? 0f : user.CurrentGun.CurrentAngle), true);
            Projectile component = gameObject.GetComponent<Projectile>();
            if (component != null)
            {
                component.Owner = user;
                component.Shooter = user.specRigidbody;
                component.OnDestruction += DoHoleSpawn;
            }
        }
        private void DoHoleSpawn(Projectile projectile)
        {
            if (projectile.Owner is PlayerController)
            {
                float radius = 7f;
                if ((projectile.Owner as PlayerController).PlayerHasActiveSynergy("Honey, I'm Home!")) radius = 10f;
                DeadlyDeadlyGoopManager goop = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(EasyGoopDefinitions.HoneyGoop);
                goop.TimedAddGoopCircle(projectile.specRigidbody.UnitCenter, radius, 0.75f, true);
            }
        }
    }
}
