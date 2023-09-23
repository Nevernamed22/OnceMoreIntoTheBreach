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
        private void Update()
        {
            base.transform.Rotate(0, 0, 1f);
        }
    }
}
