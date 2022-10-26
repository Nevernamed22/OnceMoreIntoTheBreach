using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.Misc;

namespace NevernamedsItems
{
    public class ProjectileShootbackMod : MonoBehaviour
    {
        public ProjectileShootbackMod()
        {

        }
        private void Awake()
        {
            m_projectile = base.GetComponent<Projectile>();
            m_projectile.OnDestruction += this.HandleShootBack;
        }
        private void HandleShootBack(Projectile promj)
        {
            if (m_projectile)
            {
                GameObject obj = prefabToFire.InstantiateAndFireInDirection(m_projectile.LastPosition, m_projectile.Direction.ToAngle() + 180f, 0, null);
                Projectile proj = obj.GetComponent<Projectile>();
                if (proj && m_projectile.ProjectilePlayerOwner())
                {
                    proj.Owner = m_projectile.ProjectilePlayerOwner();
                    proj.Shooter = m_projectile.ProjectilePlayerOwner().specRigidbody;
                    proj.ScaleByPlayerStats(m_projectile.ProjectilePlayerOwner());
                    m_projectile.ProjectilePlayerOwner().DoPostProcessProjectile(proj);
                    proj.RenderTilePiercingForSeconds(0.2f);
                }
            }
        }
        private Projectile m_projectile;
        public Projectile prefabToFire;

    }
}
