using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class ExplodeOnBulletIntersection : MonoBehaviour
    {
        public ExplosionData explosionData;
        private void Start()
        {
            this.m_projectile = base.GetComponent<Projectile>();
            this.m_projectile.collidesWithProjectiles = true;
            this.m_projectile.UpdateCollisionMask();
            SpeculativeRigidbody specRigidbody = this.m_projectile.specRigidbody;
            specRigidbody.OnPreRigidbodyCollision += this.HandlePreCollision;
        }
        private void HandlePreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
        {
            if (otherRigidbody != null && otherRigidbody.projectile != null && timesExploded < 3)
            {
                if (otherRigidbody.projectile.Owner is AIActor)
                {
                    if (otherRigidbody.projectile != lastCollidedProjectile)
                    {
                        if (explosionData != null) { Exploder.Explode(m_projectile.specRigidbody.UnitCenter, explosionData, Vector2.zero, null, true); }
                        lastCollidedProjectile = otherRigidbody.projectile;
                        timesExploded++;
                    }
                }
                PhysicsEngine.SkipCollision = true;
            }
        }
        private int timesExploded = 0;
        private Projectile m_projectile;
        private Projectile lastCollidedProjectile;
    }
}
