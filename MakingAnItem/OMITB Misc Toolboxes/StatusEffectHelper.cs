using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    class StatusEffectHelper
    {
        public static GameActorFireEffect GenerateFireEffect()
        {
            return new GameActorFireEffect();
        }
        public static GameActorSpeedEffect GenerateLockdown(float duration = 4f)
        {
            GameActorSpeedEffect lockdownToApply = new GameActorSpeedEffect
            {
                duration = duration,
                TintColor = Color.grey,
                DeathTintColor = Color.grey,
                effectIdentifier = "Lockdown",
                AppliesTint = true,
                AppliesDeathTint = true,
                resistanceType = EffectResistanceType.None,
                SpeedMultiplier = 0f,

                //Eh
                OverheadVFX = LockdownStatusEffect.lockdownVFXObject,
                AffectsEnemies = true,
                AffectsPlayers = false,
                AppliesOutlineTint = false,
                OutlineTintColor = Color.grey,
                PlaysVFXOnActor = false,
            };
            return lockdownToApply;
        }
        public static GameActorCharmEffect GenerateCharmEffect(float duration)
        {
            GameActorCharmEffect charmEffect = new GameActorCharmEffect
            {
                duration = duration,
                TintColor = StaticStatusEffects.charmingRoundsEffect.TintColor,
                AppliesDeathTint = StaticStatusEffects.charmingRoundsEffect.AppliesDeathTint,
                AppliesTint = StaticStatusEffects.charmingRoundsEffect.AppliesTint,
                effectIdentifier = StaticStatusEffects.charmingRoundsEffect.effectIdentifier,
                DeathTintColor = StaticStatusEffects.charmingRoundsEffect.DeathTintColor,
                OverheadVFX = StaticStatusEffects.charmingRoundsEffect.OverheadVFX,
                AffectsEnemies = StaticStatusEffects.charmingRoundsEffect.AffectsEnemies,
                AppliesOutlineTint = StaticStatusEffects.charmingRoundsEffect.AppliesOutlineTint,
                AffectsPlayers = StaticStatusEffects.charmingRoundsEffect.AffectsPlayers,
                maxStackedDuration = StaticStatusEffects.charmingRoundsEffect.maxStackedDuration,
                OutlineTintColor = StaticStatusEffects.charmingRoundsEffect.OutlineTintColor,
                PlaysVFXOnActor = StaticStatusEffects.charmingRoundsEffect.PlaysVFXOnActor,
                resistanceType = StaticStatusEffects.charmingRoundsEffect.resistanceType,
                stackMode = StaticStatusEffects.charmingRoundsEffect.stackMode,
            };
            return charmEffect;
        }
        public static GameActorPlagueEffect GeneratePlagueEffect(float duration, float dps, bool tintEnemy, Color bodyTint, bool tintCorpse, Color corpseTint)
        {
            GameActorPlagueEffect commonPlague = new GameActorPlagueEffect
            {
                duration = 10,
                effectIdentifier = "Plague",
                resistanceType = EffectResistanceType.None,
                DamagePerSecondToEnemies = 2f,
                ignitesGoops = false,
                OverheadVFX = PlagueStatusEffectSetup.PlagueOverheadVFX,
                AffectsEnemies = true,
                AffectsPlayers = false,
                AppliesOutlineTint = false,
                PlaysVFXOnActor = false,
                AppliesTint = tintEnemy,
                AppliesDeathTint = tintCorpse,
                TintColor = bodyTint,
                DeathTintColor = corpseTint,
            };
            return commonPlague;
        }
        public static GameActorSizeEffect GenerateSizeEffect(float duration, Vector2 targetScale)
        {
            GameActorSizeEffect commonPlague = new GameActorSizeEffect
            {
                duration = duration,
                newScaleMultiplier = targetScale,
                effectIdentifier = "Shrink",
                resistanceType = EffectResistanceType.None,
                OverheadVFX = null,
                AffectsEnemies = true,
                AffectsPlayers = false,
                AppliesOutlineTint = false,
                PlaysVFXOnActor = false,
                AppliesTint = false,
                AppliesDeathTint = false,
                TintColor = Color.red,
                DeathTintColor = Color.red,
            };
            return commonPlague;
        }
    }
    public class StatusEffectBulletMod : MonoBehaviour
    {
        public StatusEffectBulletMod()
        {
            pickRandom = false;
        }
        private void Start()
        {
            self = base.GetComponent<Projectile>();
            if (pickRandom)
            {
                List<StatusData> validStatuses = new List<StatusData>();
                foreach (StatusData data in datasToApply)
                {
                    if (UnityEngine.Random.value <= data.applyChance)
                    {
                        validStatuses.Add(data);
                    }
                }
                if (validStatuses.Count() > 0)
                {
                    StatusData selectedStatus = BraveUtility.RandomElement(validStatuses);
                    if (selectedStatus.applyTint) self.AdjustPlayerProjectileTint(selectedStatus.effectTint, 1);
                    self.statusEffectsToApply.Add(selectedStatus.effect);
                }
            }
            else
            {
                foreach (StatusData data in datasToApply)
                {
                    if (UnityEngine.Random.value <= data.applyChance)
                    {
                        if (data.applyTint) self.AdjustPlayerProjectileTint(data.effectTint, 1);
                        self.statusEffectsToApply.Add(data.effect);
                    }
                }
            }
        }

        private Projectile self;
        public List<StatusData> datasToApply = new List<StatusData>();
        public bool pickRandom;
        public class StatusData
        {
            public GameActorEffect effect;
            public float applyChance;
            public Color effectTint;
            public bool applyTint = false;
        }
    }
}
