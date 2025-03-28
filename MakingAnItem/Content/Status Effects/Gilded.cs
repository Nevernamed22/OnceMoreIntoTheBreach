using Alexandria.ItemAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class GameActorGildedEffect : GameActorEffect
    {
        public GameActorGildedEffect()
        {
            this.TintColor = ExtendedColours.gildedBulletsGold;
            this.DeathTintColor = ExtendedColours.gildedBulletsGold;
            this.AppliesTint = true;
            this.AppliesDeathTint = true;
            this.AffectsPlayers = false;
            this.AffectsEnemies = true;
            this.effectIdentifier = "gilded";
            OverheadVFX = SharedVFX.GildedOverhead;
            sparkleAccum = 0;

        }
        private float sparkleAccum;
        private float cachedHealth = 0f;
        public override void OnEffectApplied(GameActor actor, RuntimeGameActorEffectData effectData, float partialAmount = 1)
        {
            if (actor && actor.healthHaver != null)
            {
                cachedHealth = actor.healthHaver.currentHealth;
                actor.healthHaver.OnHealthChanged += Hurt;
            }
            base.OnEffectApplied(actor, effectData, partialAmount);
        }
        private int currencyToSpawnNextTick = 0;
        public void Hurt(float currentHealth, float maxHealth)
        {
            float damageTaken = cachedHealth - currentHealth;
            if (damageTaken > 0f)
            {
                currencyToSpawnNextTick = Mathf.FloorToInt(damageTaken / 3.5f);
                currencyToSpawnNextTick = Math.Max(currencyToSpawnNextTick, 0);
            }
            cachedHealth = currentHealth;
        }
        public override void EffectTick(GameActor actor, RuntimeGameActorEffectData effectData)
        {
            sparkleAccum += BraveTime.DeltaTime * 3;
            if (sparkleAccum > 1f)
            {
                int num = Mathf.FloorToInt(sparkleAccum);
                sparkleAccum %= 1f;
                Vector2 minpos = actor.sprite.WorldBottomLeft;
                Vector2 maxpos = actor.sprite.WorldTopRight;
                for (int i = 0; i < num; i++)
                {
                    GameObject piss = UnityEngine.Object.Instantiate(SharedVFX.GoldenSparkle, new Vector2(UnityEngine.Random.Range(minpos.x, maxpos.x), UnityEngine.Random.Range(minpos.y, maxpos.y)), Quaternion.identity);
                    piss.transform.parent = actor.transform;
                    piss.GetComponent<tk2dBaseSprite>().HeightOffGround = 0.2f;
                    actor.sprite.AttachRenderer(piss.GetComponent<tk2dBaseSprite>());
                }
            }
            if (currencyToSpawnNextTick > 0)
            {
                LootEngine.SpawnCurrency(actor.CenterPosition, currencyToSpawnNextTick);
                currencyToSpawnNextTick = 0;
            }

            if (actor.aiAnimator)
            {
                actor.aiAnimator.FpsScale = (!actor.IsFalling) ? 0.5f : 1f;
            }
            if (actor.aiShooter)
            {
                actor.aiShooter.AimTimeScale = 0.5f;
            }
            if (actor.behaviorSpeculator)
            {
                actor.behaviorSpeculator.CooldownScale = 0.5f;
            }
            if (actor.bulletBank)
            {
                actor.bulletBank.TimeScale = 0.5f;
            }
            base.EffectTick(actor, effectData);
        }
        public override void OnEffectRemoved(GameActor actor, RuntimeGameActorEffectData effectData)
        {
            if (actor.aiAnimator)
            {
                actor.aiAnimator.FpsScale = 1f;
            }
            if (actor.aiShooter)
            {
                actor.aiShooter.AimTimeScale = 1f;
            }
            if (actor.behaviorSpeculator)
            {
                actor.behaviorSpeculator.CooldownScale = 1f;
            }
            if (actor.bulletBank)
            {
                actor.bulletBank.TimeScale = 1f;
            }
            if (actor && actor.healthHaver != null)
            {
                actor.healthHaver.OnHealthChanged -= Hurt;
            }
            base.OnEffectRemoved(actor, effectData);
        }

    }
}
