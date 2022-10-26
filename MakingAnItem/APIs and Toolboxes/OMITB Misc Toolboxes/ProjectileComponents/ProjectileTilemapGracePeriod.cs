using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.Misc;

namespace NevernamedsItems
{
    public class ProjectileTilemapGracePeriod : MonoBehaviour
    {
        public ProjectileTilemapGracePeriod()
        {
            period = 0.1f;
        }
        private void Start()
        {
            m_projectile = base.GetComponent<Projectile>();
            m_projectile.RenderTilePiercingForSeconds(period);
        }           
        private Projectile m_projectile;
        public float period;

    }
}
