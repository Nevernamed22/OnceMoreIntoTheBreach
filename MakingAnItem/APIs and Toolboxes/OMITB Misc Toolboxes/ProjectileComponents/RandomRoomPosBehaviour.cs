using Alexandria.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class RandomRoomPosBehaviour : MonoBehaviour
    {
        public RandomRoomPosBehaviour()
        {
        }
        private void Start()
        {
            this.m_projectile = base.GetComponent<Projectile>();
            if (this.m_projectile.GetAbsoluteRoom() != null)
            {
                IntVector2 newPosition = this.m_projectile.GetAbsoluteRoom().GetRandomVisibleClearSpot(3, 3);
                this.m_projectile.transform.position = newPosition.ToVector3();
                this.m_projectile.specRigidbody.Reinitialize();
            }
        }
        private Projectile m_projectile;
    }
}
