using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.Misc;

namespace NevernamedsItems
{
    public class BulletSharkProjectileDoer : MonoBehaviour
    {
        public BulletSharkProjectileDoer()
        {

        }
        private void Awake()
        {
            m_projectile = base.GetComponent<Projectile>();
            playerOwner = m_projectile.ProjectilePlayerOwner();
        }
        private void Update()
        {
            if (timer < Delay) { timer += BraveTime.DeltaTime; }
            else
            {
                if (toSpawn)
                {
                    Projectile fired = toSpawn.InstantiateAndFireInDirection(m_projectile.LastPosition, FireAngle(), 10f, playerOwner).GetComponent<Projectile>();
                    if (playerOwner)
                    {
                        fired.Owner = playerOwner;
                        fired.Shooter = playerOwner.specRigidbody;
                        fired.ScaleByPlayerStats(playerOwner);
                        playerOwner.DoPostProcessProjectile(fired);
                    }
                    else
                    {
                        fired.Owner = m_projectile.Owner;
                        fired.Shooter = m_projectile.Shooter;
                    }
                    leftie = !leftie;
                }
                timer = 0;
            }
        }
        public float FireAngle()
        {
            float dir = m_projectile.Direction.ToAngle();
            dir += 180;
            dir += leftie ? 25 : -25;
            return dir;
        }
        private bool leftie;
        public float Delay = 0.1f;
        private float timer;
        private Projectile m_projectile;
        private PlayerController playerOwner;
        public Projectile toSpawn;

    }
}
