using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Alexandria.ItemAPI;
using Alexandria.Misc;
using UnityEngine;
using MonoMod.RuntimeDetour;
using System.Reflection;
using SaveAPI;

namespace NevernamedsItems
{
    public class RabbitsFoot : PassiveItem
    {
        public static void Init()
        {
            PassiveItem item = ItemSetup.NewItem<RabbitsFoot>(
              "Rabbits Foot",
              "Feels Lucky",
              "Imparts a strange luck upon your weapon.\n\nMany veteran gunslingers keep good luck charms such as this. Other notable good luck charms include coins, socks, underwear, and smell.",
              "rabbitsfoot_icon") as PassiveItem;
            item.quality = PickupObject.ItemQuality.B;
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Coolness, 1f, StatModifier.ModifyMethod.ADDITIVE);
            item.AddToSubShop(ItemBuilder.ShopType.Cursula);
            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.RAT_KILLED_BULLET, true);
            item.SetTag("lucky");
        }
        public override void Pickup(PlayerController player)
        {
            player.PostProcessProjectile += OnShoot;
            OMITBActions.ModifyChanceScalar += ModifyChanceScalar;
            base.Pickup(player);
        }
        public override void DisableEffect(PlayerController player)
        {
            if (player)
            {
                player.PostProcessProjectile -= OnShoot;
            }
            OMITBActions.ModifyChanceScalar -= ModifyChanceScalar;
            base.DisableEffect(player);
        }
        public void ModifyChanceScalar(ref float scalar, PlayerController player)
        {
            if (player == Owner)
            {
                scalar *= 2f;
            }
        }
        public void OnShoot(Projectile bullet, float f)
        {
            if (bullet && bullet.ProjectilePlayerOwner())
            {
                if (bullet.GetComponent<HomingModifier>() != null)
                {
                    HomingModifier homing = bullet.GetComponent<HomingModifier>();
                    homing.HomingRadius *= 2;
                    homing.AngularVelocity *= 2;
                }
                if (bullet.GetComponent<SpawnProjModifier>() != null)
                {
                    SpawnProjModifier comp = bullet.GetComponent<SpawnProjModifier>();
                    if (comp.spawnProjectilesInFlight)
                    {
                        comp.inFlightSpawnCooldown *= 0.5f;
                    }
                    if (comp.spawnProjectilesOnCollision)
                    {
                        comp.numberToSpawnOnCollison *= 2;
                    }
                }
                if (bullet.GetComponent<PierceProjModifier>() != null)
                {
                    PierceProjModifier comp = bullet.GetComponent<PierceProjModifier>();
                    comp.penetratesBreakables = true;
                    comp.penetration *= 2;
                }
            }
        }
    }
}


