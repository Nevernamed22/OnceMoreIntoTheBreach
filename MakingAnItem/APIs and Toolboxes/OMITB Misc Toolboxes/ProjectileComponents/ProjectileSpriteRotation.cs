using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    class ProjectileSpriteRotation : MonoBehaviour
    {
        private Projectile self;
        private void Start()
        {
            self = base.GetComponent<Projectile>();
        }
        public float RotPerFrame = 1f;
        private void FixedUpdate()
        {
            base.transform.Rotate(0, 0, RotPerFrame);
        }
    }
}
