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
    }
    public class StatusEffectBulletMod : MonoBehaviour
    {
        public StatusEffectBulletMod()
        {

        }
        private void Start()
        {
            self = base.GetComponent<Projectile>();
            foreach (StatusData data in datasToApply)
            {
                if (UnityEngine.Random.value <= data.applyChance)
                {
                    if (data.applyTint) self.AdjustPlayerProjectileTint(data.effectTint, 1);
                    self.statusEffectsToApply.Add(data.effect);
                }
            }
        }

        private Projectile self;
        public List<StatusData> datasToApply = new List<StatusData>();
        public class StatusData
        {
            public GameActorEffect effect;
            public float applyChance;
            public Color effectTint;
            public bool applyTint = false;
        }
    }
}
