using Alexandria.ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class ApplyLockdown
    {
        public static void ApplyDirectLockdown(GameActor target, float duration, Color tintColour, Color deathTintColour, EffectResistanceType resistanceType, string identifier, bool tintsEnemy, bool tintsCorpse)
        {
            GameActorSpeedEffect lockdownToApply = new GameActorSpeedEffect
            {
                duration = duration,
                TintColor = tintColour,
                DeathTintColor = deathTintColour,
                effectIdentifier = identifier,
                AppliesTint = tintsEnemy,
                AppliesDeathTint = tintsCorpse,
                resistanceType = resistanceType,
                SpeedMultiplier = 0f,

                //Eh
                OverheadVFX = SharedVFX.LockdownOverhead,
                AffectsEnemies = true,
                AffectsPlayers = false,
                AppliesOutlineTint = false,
                OutlineTintColor = tintColour,
                PlaysVFXOnActor = false,
            };
            if (target && target.aiActor && target.healthHaver && target.healthHaver.IsAlive)
            {
                target.ApplyEffect(lockdownToApply, 1f, null);
            }
        }
    }
    public class ApplyLockdownBulletBehaviour : MonoBehaviour
    {
        public ApplyLockdownBulletBehaviour()
        {
            this.bulletTintColour = Color.grey;
            this.useSpecialBulletTint = false;
            this.TintEnemy = true;
            this.TintCorpse = false;
            this.enemyTintColour = Color.grey;
            this.duration = 1;
            this.procChance = 1;
        }
        private void Start()
        {
            this.m_projectile = base.GetComponent<Projectile>();
            if (useSpecialBulletTint)
            {
                m_projectile.AdjustPlayerProjectileTint(bulletTintColour, 2);
            }
            m_projectile.OnHitEnemy += this.OnHitEnemy;
        }
        private void OnHitEnemy(Projectile bullet, SpeculativeRigidbody enemy, bool fatal)
        {
            if (UnityEngine.Random.value <= procChance)
            {
                ApplyLockdown.ApplyDirectLockdown(enemy.gameActor, this.duration, this.enemyTintColour, this.corpseTintColour, EffectResistanceType.None, "Lockdown", this.TintEnemy, this.TintCorpse);
            }
        }
        private Projectile m_projectile;
        public Color bulletTintColour;
        public Color enemyTintColour;
        public Color corpseTintColour;
        public float duration;
        public bool useSpecialBulletTint;
        public bool TintEnemy;
        public bool TintCorpse;
        public int procChance;
    }
}
