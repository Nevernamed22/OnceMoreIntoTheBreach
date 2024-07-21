using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class ProjectileSpinner : MonoBehaviour
    {
        public void Start()
        {

        }
        public void Update()
        {
            float z = base.transform.rotation.eulerAngles.z;
            base.transform.rotation = Quaternion.Euler(0f, 0f, z + (degreesPerSecond * BraveTime.DeltaTime));
        }
        public float degreesPerSecond = 180f;
    }
}
