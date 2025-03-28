using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class ModdedStatusEffectApplier : BraveBehaviour
    {
        public ModdedStatusEffectApplier()
        {
        }
        private void Start()
        {
            this.m_projectile = base.projectile;
            if (this.m_projectile != null && this.m_projectile.statusEffectsToApply != null)
            {
                if (appliesPlague) { m_projectile.statusEffectsToApply.Add(StatusEffectHelper.GeneratePlagueEffect(appliedPlagueDuration, 2, true, ExtendedColours.plaguePurple, true, ExtendedColours.plaguePurple)); }
                if (appliesCrying) { m_projectile.statusEffectsToApply.Add(StatusEffectHelper.GenerateCryingEfffect(appliedCryingDuration)) ; }
            }
        }

        public bool appliesPlague;
        public float appliedPlagueDuration = 100f;

        public bool appliesCrying;
        public float appliedCryingDuration = 20f;

        private Projectile m_projectile;
    }
}
