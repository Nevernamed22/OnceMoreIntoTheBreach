using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alexandria.Misc;
using Alexandria.ItemAPI;
using UnityEngine;

namespace NevernamedsItems
{
    public class DamageAverageBehaviour : MonoBehaviour
    {
        public DamageAverageBehaviour()
        {
        }
        private void Start()
        {
            this.m_projectile = base.GetComponent<Projectile>();
            if (m_projectile && m_projectile.Owner && m_projectile.Owner is PlayerController) Owner = m_projectile.Owner as PlayerController;
            if (Owner)
            {
                List<float> ownerDamageAverage = new List<float>();
                foreach(Gun gun in Owner.inventory.AllGuns)
                {
                    if (gun != null && gun.DefaultModule != null && gun.DefaultModule.projectiles != null && gun.DefaultModule.projectiles[0] != null)
                    {
                        if (gun.DefaultModule.projectiles[0].GetComponent<DamageAverageBehaviour>() == null)
                        {
                            ownerDamageAverage.Add(gun.DefaultModule.projectiles[0].baseData.damage);
                       }
                    }
                }
                m_projectile.baseData.damage = ownerDamageAverage.ToArray().Average() * Owner.stats.GetStatValue(PlayerStats.StatType.Damage);
            }
        }
        private Projectile m_projectile;
        private PlayerController Owner;
    }
}
