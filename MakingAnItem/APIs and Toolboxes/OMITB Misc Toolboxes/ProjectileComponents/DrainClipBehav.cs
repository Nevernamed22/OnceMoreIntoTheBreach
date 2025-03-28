using Alexandria.Misc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class DrainClipBehav : MonoBehaviour
    {
        private void Start()
        {
            this.m_projectile = base.GetComponent<Projectile>();
            if (m_projectile && m_projectile.PossibleSourceGun != null)
            {
                if (shotsToDrain < 0)
                {
                    m_projectile.PossibleSourceGun.MoveBulletsIntoClip(shotsToDrain * -1);
                }
                else if (shotsToDrain > 0)
                {
                m_projectile.PossibleSourceGun.LoseAmmo(shotsToDrain);
                ServiceWeapon wep = m_projectile.PossibleSourceGun.GetComponent<ServiceWeapon>();
                if (wep && m_projectile.PossibleSourceGun.ClipShotsRemaining == 0) { wep.Criticalise(true); }
                }
            }
        }
        private Projectile m_projectile;
        public int shotsToDrain = 1;
    }
}
