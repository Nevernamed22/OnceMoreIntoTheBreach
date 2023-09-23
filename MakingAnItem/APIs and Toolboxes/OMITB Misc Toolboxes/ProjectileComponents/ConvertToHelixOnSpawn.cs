using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.Misc;

namespace NevernamedsItems
{
    public class ConvertToHelixOnSpawn : MonoBehaviour
    {
        public ConvertToHelixOnSpawn()
        {
        }
        private void Start()
        {
            m_projectile = base.GetComponent<Projectile>();
            m_projectile.OverrideMotionModule = new HelixProjectileMotionModule()
            {
                ForceInvert = UnityEngine.Random.value <= 0.5f,
            };
            HelixProjectileMotionModule mot = m_projectile.OverrideMotionModule as HelixProjectileMotionModule;
            mot.helixAmplitude *= 0.5f;
        }
        private Projectile m_projectile;
    }
}
