using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;
using System.Collections;
using System.Reflection;
using MonoMod.RuntimeDetour;
using Alexandria.Misc;

namespace NevernamedsItems
{
    class BlackGuonStone : AdvancedPlayerOrbitalItem
    {
        public static void Init()
        {
            AdvancedPlayerOrbitalItem item = ItemSetup.NewItem<BlackGuonStone>(
            "Black Guon Stone",
            "No Bullets Can Escape",
            "Chance to crush enemy bullets into a single point of infinite density." + "\n\nThis ancient stone, though appearing arcane, is entirely based on scientific principles. Batteries are included.",
            "blackguonstone_icon") as AdvancedPlayerOrbitalItem;
            item.quality = PickupObject.ItemQuality.B;

            item.OrbitalPrefab = ItemSetup.CreateOrbitalObject("Black Guon Stone", "blackguonstone_orbital", new IntVector2(8,8), new IntVector2(-4, -4)).GetComponent<PlayerOrbital>();

            item.SetTag("guon_stone");

            item.HasAdvancedUpgradeSynergy = true;
            item.AdvancedUpgradeSynergy = "Blacker Guon Stone";
            item.AdvancedUpgradeOrbitalPrefab = ItemSetup.CreateOrbitalObject("Blacker Guon Stone", "blackguonstone_synergy", new IntVector2(12, 12), new IntVector2(-6, -6), perfectOrbitalFactor: 10);
        }
        public override void OnOrbitalCreated(GameObject orbital)
        {
            SpeculativeRigidbody orbBody = orbital.GetComponent<SpeculativeRigidbody>();
            if (orbBody) orbBody.OnPreRigidbodyCollision += this.OnGuonHit;
            base.OnOrbitalCreated(orbital);
        }
        private void OnGuonHit(SpeculativeRigidbody myRigidbody, PixelCollider myCollider, SpeculativeRigidbody other, PixelCollider otherCollider)
        {
            if (UnityEngine.Random.value <= 0.25f)
            {
                Projectile component = other.GetComponent<Projectile>();
                if (component != null && component.ProjectilePlayerOwner() == null && Owner)
                {
                    Projectile blackHolePrefab = ((Gun)PickupObjectDatabase.GetById(169)).DefaultModule.projectiles[0];
                    float angle = (myRigidbody.UnitCenter - Owner.specRigidbody.UnitCenter).ToAngle();
                    GameObject instanceObj = blackHolePrefab.InstantiateAndFireInDirection(myRigidbody.UnitCenter, angle);
                    Projectile instanceBlackHole = instanceObj.GetComponent<Projectile>();
                    if (instanceBlackHole != null)
                    {
                        instanceBlackHole.Owner = base.Owner;
                        instanceBlackHole.Shooter = base.Owner.specRigidbody;
                        if (!Owner.PlayerHasActiveSynergy("Blacker Guon Stone"))
                        {
                            instanceBlackHole.RuntimeUpdateScale(0.5f);
                            instanceBlackHole.baseData.damage *= 0.5f;
                        }
                        instanceBlackHole.baseData.damage *= Owner.stats.GetStatValue(PlayerStats.StatType.Damage);
                        Owner.DoPostProcessProjectile(instanceBlackHole);

                        SlowDownOverTimeModifier slowdown = instanceBlackHole.gameObject.GetOrAddComponent<SlowDownOverTimeModifier>();
                        slowdown.timeToSlowOver = 0.5f;
                        slowdown.timeTillKillAfterCompleteStop = Owner.PlayerHasActiveSynergy("Schwarzschild Radius") ? 1 : 0.5f;
                        slowdown.killAfterCompleteStop = true;
                        slowdown.extendTimeByRangeStat = false;
                    }
                }
            }
        }
    }
}