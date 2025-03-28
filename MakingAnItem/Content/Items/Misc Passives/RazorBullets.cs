using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Alexandria.ItemAPI;
using UnityEngine;
using MonoMod.RuntimeDetour;
using System.Reflection;
using SaveAPI;

namespace NevernamedsItems
{
    public class RazorBullets : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<RazorBullets>(
            "Razor Bullets",
            "A Cut Above",
            "Standard ammunition, carved down to a razor sharp edge to increase their lacerating potential.",
            "razorbullets_icon");
            item.quality = PickupObject.ItemQuality.A;
            item.SetTag("bullet_modifier");
            //Unlock
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Damage, 1.1f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            Doug.AddToLootPool(item.PickupObjectId);

        }
        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {
            float procChance = 0.25f;
            procChance *= effectChanceScalar;
            if (UnityEngine.Random.value <= procChance)
            {
                sourceProjectile.AdjustPlayerProjectileTint(Color.white, 2);
                sourceProjectile.OnHitEnemy += HitEnemy;
                if (sourceProjectile.GetComponent<PierceProjModifier>() != null) { sourceProjectile.GetComponent<PierceProjModifier>().penetration++; }
                else { sourceProjectile.gameObject.AddComponent<PierceProjModifier>().penetration = 1; }
            }
        }
        private void HitEnemy(Projectile self, SpeculativeRigidbody enemy, bool fatal)
        {
            if (!fatal && enemy && enemy.gameActor)
            {
                enemy.gameActor.ApplyEffect(new GameActorExsanguinationEffect() { duration = 15f });
            }
        }
        private void PostProcessBeam(BeamController beam, SpeculativeRigidbody hitRigidBody, float tickrate)
        {
            float procChance = 0.25f;
            beam.AdjustPlayerBeamTint(Color.white, 1, 0f);
            GameActor gameActor = hitRigidBody.gameActor;
            if (!gameActor)
            {
                return;
            }
            if (UnityEngine.Random.value < BraveMathCollege.SliceProbability(procChance, tickrate))
            {
                hitRigidBody.gameActor.ApplyEffect(new GameActorExsanguinationEffect() { duration = 15f });
            }
        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.PostProcessProjectile -= this.PostProcessProjectile;
            player.PostProcessBeamTick -= this.PostProcessBeam;


            return debrisObject;
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += this.PostProcessProjectile;
            player.PostProcessBeamTick += this.PostProcessBeam;
        }
        public override void OnDestroy()
        {
            if (Owner)
            {
                Owner.PostProcessBeamTick -= this.PostProcessBeam;
                Owner.PostProcessProjectile -= this.PostProcessProjectile;
            }
            base.OnDestroy();
        }
    }
}
