using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class NoCollideBehaviour : MonoBehaviour
    {
        public NoCollideBehaviour()
        {
            worksOnProjectiles = false;
            worksOnEnemies = true;
        }

        public void Start()
        {
            try
            {
                this.m_projectile = base.GetComponent<Projectile>();
                this.m_projectile.specRigidbody.OnPreRigidbodyCollision += this.HandlePreCollision;
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
                ETGModConsole.Log(e.StackTrace);
            }
        }

        private void HandlePreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
        {
            try
            {
                if (otherRigidbody)
                {
                    if (otherRigidbody.aiActor != null && otherRigidbody.healthHaver != null)
                    {
                        if (worksOnEnemies) PhysicsEngine.SkipCollision = true;
                    }
                    else if (otherRigidbody.projectile != null && otherRigidbody.projectile.collidesWithProjectiles)
                    {
                        if (worksOnProjectiles) PhysicsEngine.SkipCollision = true;
                    }
                    else { PhysicsEngine.SkipCollision = true; }
                }
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
                ETGModConsole.Log(e.StackTrace);
            }
        }
        private Projectile m_projectile;

        public bool worksOnEnemies = false;
        public bool worksOnProjectiles = false;
    }
}
