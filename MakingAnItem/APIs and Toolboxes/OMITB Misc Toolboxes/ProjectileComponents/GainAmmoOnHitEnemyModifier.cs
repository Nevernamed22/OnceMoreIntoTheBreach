using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.Misc;

namespace NevernamedsItems
{
    public class GainAmmoOnHitEnemyModifier : MonoBehaviour
    {
        public bool requireKill = false;
        public int ammoToGain = 1;
        private void Awake()
        {
            m_projectile = base.GetComponent<Projectile>();
            m_projectile.OnHitEnemy += this.OnHit;
        }
        private void OnHit(Projectile obj, SpeculativeRigidbody enemy, bool fatal)
        {
            if (fatal || !requireKill)
            {
                if (obj.PossibleSourceGun != null && obj.PossibleSourceGun.CanGainAmmo)
                {
                    obj.PossibleSourceGun.GainAmmo(ammoToGain);
                }
            }
        }
        private Projectile m_projectile;
    }
}
