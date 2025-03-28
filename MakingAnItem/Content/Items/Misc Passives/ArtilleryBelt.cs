﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.Misc;

namespace NevernamedsItems
{
    public class ArtilleryBelt : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<ArtilleryBelt>(
            "Artillery Belt",
            "From The Hip",
            "Takes pot-shots at your foes." + "\n\nA relic of Alben Smallbore's research on garmentosapience.",
            "artillerybelt_icon");
            item.quality = PickupObject.ItemQuality.B;
        }
        private float timer;
        public override void Update()
        {
            if (Owner)
            {
                if (timer > 0)
                {
                    timer -= BraveTime.DeltaTime;
                }
                if (timer <= 0)
                {
                    if (UnityEngine.Random.value <= 0.1f && Owner.IsInCombat) Fire();
                    timer = 0.1f;
                }
            }
            base.Update();
        }
        private void Fire()
        {
            if (Owner.specRigidbody != null && Owner.specRigidbody.UnitCenter.GetNearestEnemyToPosition() != null)
            {
                AkSoundEngine.PostEvent("Play_WPN_smileyrevolver_shot_01", gameObject);
                GameObject spawnedProj = ProjSpawnHelper.SpawnProjectileTowardsPoint((PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0].gameObject, Owner.specRigidbody.UnitCenter, Owner.specRigidbody.UnitCenter.GetNearestEnemyToPosition().Position, 0, 20, Owner);
                Projectile spawnedProjectileComp = spawnedProj.GetComponent<Projectile>();
                spawnedProjectileComp.Owner = Owner;
                spawnedProjectileComp.Shooter = Owner.specRigidbody;
                spawnedProjectileComp.baseData.damage *= Owner.stats.GetStatValue(PlayerStats.StatType.Damage);
                spawnedProjectileComp.baseData.speed *= Owner.stats.GetStatValue(PlayerStats.StatType.ProjectileSpeed);
                spawnedProjectileComp.baseData.range *= Owner.stats.GetStatValue(PlayerStats.StatType.RangeMultiplier);
                spawnedProjectileComp.baseData.force *= Owner.stats.GetStatValue(PlayerStats.StatType.KnockbackMultiplier);
                Owner.DoPostProcessProjectile(spawnedProjectileComp);
                spawnedProjectileComp.ApplyCompanionModifierToBullet(Owner);
            }
        }
        public override void Pickup(PlayerController player)
        {
            timer = 0.1f;
            base.Pickup(player);
        }
    }

}
