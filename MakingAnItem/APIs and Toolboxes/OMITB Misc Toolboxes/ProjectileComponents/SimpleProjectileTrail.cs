using Alexandria.ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class SimpleProjectileTrail : MonoBehaviour
    {
        public bool addSmoke;
        //public bool addFire;
        public static GameObject smokePre = (PickupObjectDatabase.GetById(39) as Gun).DefaultModule.projectiles[0].transform.Find("VFX_Rocket_Exhaust").gameObject;
        public static GameObject firePre = (PickupObjectDatabase.GetById(39) as Gun).DefaultModule.projectiles[0].transform.Find("VFX_Rocket_Exhaust_Fire").gameObject;
        public void Start()
        {
            self = base.GetComponent<Projectile>();

            if (addSmoke)
            {
                GameObject smoke = UnityEngine.Object.Instantiate( smokePre);
                smoke.transform.position = self.specRigidbody.UnitCenterLeft;
                smoke.transform.SetParent(self.transform);
                smoke.GetComponent<ParticleKiller>().ForceInit();
            }
            
        }
        public Projectile self;
    }
}
