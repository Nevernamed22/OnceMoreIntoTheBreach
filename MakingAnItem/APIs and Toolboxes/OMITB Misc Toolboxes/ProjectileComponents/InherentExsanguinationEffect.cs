using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class InherentExsanguinationEffect : MonoBehaviour
    {
        public InherentExsanguinationEffect()
        {
        }
        private void Start()
        {
            this.m_projectile = base.GetComponent<Projectile>();
            m_projectile.OnHitEnemy += HitEnemy;
        }
        private void HitEnemy(Projectile self, SpeculativeRigidbody enemy, bool fatal)
        {
            if (!fatal && enemy && enemy.gameActor)
            {
                enemy.gameActor.ApplyEffect(new GameActorExsanguinationEffect() { duration = duration });
            }
        }
        private Projectile m_projectile;
        public float duration;
    }
}
