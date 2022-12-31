using Alexandria.Misc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class SquareMotionHandler : MonoBehaviour
    {
        public SquareMotionHandler()
        {
            horizontalLimiter = 3f;
            verticalLimiter = 2f;
            stage = 0;
            angleChange = 90f;
            randomiseStart = false;
            startLeft = true;
        }
        private Projectile self;

        public float horizontalLimiter;
        public float verticalLimiter;
        private float curDistance;
        private float lastDistance;
        private float curTargetDistance;
        public float angleChange;

        public bool randomiseStart;
        public bool startLeft;

        private int stage;

        private void Start()
        {
            if (!startLeft) { stage = -1; }
            if (randomiseStart) { stage = UnityEngine.Random.value <= 0.5f ? -1 : 0; }
            this.self = base.GetComponent<Projectile>();
        }
        private void Update()
        {
            if (self && self.m_distanceElapsed > lastDistance)
            {
                curDistance += self.m_distanceElapsed - lastDistance;
                lastDistance = self.m_distanceElapsed;
            }
            if (curDistance >= curTargetDistance)
            {
                ProgressStage();
                curDistance = 0;
            }
        }
        private void ProgressStage()
        {
            switch (stage)
            {
                case -1:
                    curTargetDistance = verticalLimiter * 0.5f;
                    self.SendInDirection(self.Direction.Rotate(angleChange), false);
                    stage = 3;
                    break;
                case 0:
                    curTargetDistance = verticalLimiter * 0.5f ;
                    self.SendInDirection(self.Direction.Rotate(-angleChange), false);
                    stage = 1;
                    break;
                case 1:
                    curTargetDistance = horizontalLimiter;
                    self.SendInDirection(self.Direction.Rotate(angleChange), false);
                    stage = 2;
                    break;
                case 2:
                    curTargetDistance = verticalLimiter;
                    self.SendInDirection(self.Direction.Rotate(angleChange), false);
                    stage = 3;
                    break;
                case 3:
                    curTargetDistance = horizontalLimiter;
                    self.SendInDirection(self.Direction.Rotate(-angleChange), false);
                    stage = 4;
                    break;
                case 4:
                    curTargetDistance = verticalLimiter;
                    self.SendInDirection(self.Direction.Rotate(-angleChange), false);
                    stage = 1;
                    break;

            }
        }
    }
}
