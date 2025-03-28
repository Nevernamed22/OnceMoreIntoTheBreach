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
        public bool shootBackOnDestruction = false;
        public bool shootBackOnTimer = false;
        public float timebetweenShootbacks = 0.1f;
        private void Awake()
        {
            m_projectile = base.GetComponent<Projectile>();
            m_projectile.OnDestruction += this.HandleShootBack;
        }
        private void HandleShootBack(Projectile promj)
        {
            if (m_projectile && shootBackOnDestruction)
            {
                Shootback();
            }
        }
        private float t = 0;
       private void Update()
        {
            if (shootBackOnTimer)
            {
                t += BraveTime.DeltaTime;
                if (t >= timebetweenShootbacks)
                {
                    if (m_projectile) { Shootback(); }
                    t = 0;
                }
            }
        }
        private void Shootback()
        {
            GameObject obj = prefabToFire.InstantiateAndFireInDirection(m_projectile.LastPosition, m_projectile.Direction.ToAngle() + 180f, 0, null);
            Projectile proj = obj.GetComponent<Projectile>();
            if (proj && m_projectile.Owner)
            {
                proj.Owner = m_projectile.Owner;
                proj.Shooter = m_projectile.Owner.specRigidbody;
                if (m_projectile.ProjectilePlayerOwner())
                {
                    proj.ScaleByPlayerStats(m_projectile.ProjectilePlayerOwner());
                    m_projectile.ProjectilePlayerOwner().DoPostProcessProjectile(proj);
                }
                proj.RenderTilePiercingForSeconds(0.2f);
            }
        }
        private Projectile m_projectile;
        public Projectile prefabToFire;

    }
}
