using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NevernamedsItems
{
    class DirectionalBarrelPositionHandler : BraveBehaviour
    {
        private Gun sourceGun;
        private void Start()
        {
            sourceGun = base.GetComponent<Gun>();
        }
        public IntVector2[] barrelOffsets;
        private void Update()
        {
            if (sourceGun && sourceGun.usesDirectionalAnimator)
            {
                float num = sourceGun.gunAngle;
                if (sourceGun.CurrentOwner is PlayerController)
                {
                    PlayerController playerController = sourceGun.CurrentOwner as PlayerController;
                    num = BraveMathCollege.Atan2Degrees(playerController.unadjustedAimPoint.XY() - sourceGun.m_attachTransform.position.XY());
                }
                int num2 = BraveMathCollege.AngleToOctant(num + 90f);
                sourceGun.barrelOffset.localPosition = barrelOffsets[num2].ToVector3();
            }
        }
    }
}
