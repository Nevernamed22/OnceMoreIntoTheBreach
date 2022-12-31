using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alexandria.Misc;
using Alexandria.ItemAPI;
using UnityEngine;

namespace NevernamedsItems
{
    public class StatusEffectBulletSynergy : MonoBehaviour
    {
        public StatusEffectBulletSynergy()
        {
            chance = 1;   
        }
        private void Start()
        {
            m_projectile = base.GetComponent<Projectile>();
            if (m_projectile && m_projectile.ProjectilePlayerOwner() && !string.IsNullOrEmpty(synergyToCheckFor) &&m_projectile.ProjectilePlayerOwner().PlayerHasActiveSynergy(synergyToCheckFor))
            {
                if (UnityEngine.Random.value <= chance)
                {
                    m_projectile.statusEffectsToApply.AddRange(StatusEffects);
                    m_projectile.AdjustPlayerProjectileTint(tint, 1);
                }
            }
        }
        
        private Projectile m_projectile;
        public List<GameActorEffect> StatusEffects = new List<GameActorEffect>();
        public Color tint;
        public float chance;
        public string synergyToCheckFor;
    }
}
