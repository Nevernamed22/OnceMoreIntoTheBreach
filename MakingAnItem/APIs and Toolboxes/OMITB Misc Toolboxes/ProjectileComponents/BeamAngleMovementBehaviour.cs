using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.Misc;

namespace NevernamedsItems
{
    public class BeamAngleMovementBehaviour : MonoBehaviour
    {
        public BeamAngleMovementBehaviour()
        {
            completed = false;
        }
        private void Awake()
        {
            m_projectile = base.GetComponent<Projectile>();
            beam = m_projectile.GetComponent<BeamController>();
        }

        private void LateUpdate()
        {
            if (targets.Count > 0 && !completed)
            {
                timeSinceLastTarget += BraveTime.DeltaTime;

                float desiredAngle = Mathf.Lerp(
                    0, 
                    targets[currentTargetIndex].rotationAmount,
                  timeSinceLastTarget  / targets[currentTargetIndex].timeToChangeOver);
                beam.Direction.Rotate(desiredAngle);

                //Once time is completed, transition to next target, or loop/complete if on the final target.
                if (timeSinceLastTarget <= targets[currentTargetIndex].timeToChangeOver)
                {
                    timeSinceLastTarget = 0;
                    if ((targets.Count - 1) == currentTargetIndex)
                    {
                        if (loopAfterTargetCompletion) currentTargetIndex = 0;
                        else completed = true;
                    }
                    else currentTargetIndex++;
                }
            }
        }


        public List<TargetBeamRotation> targets = new List<TargetBeamRotation>();
        private Projectile m_projectile;
        private BeamController beam;
        public bool loopAfterTargetCompletion = true;
        private float timeSinceLastTarget = 0;
        private int currentTargetIndex = 0;
        private bool completed;
        public class TargetBeamRotation
        {
            public float rotationAmount;
            public float timeToChangeOver;
            public bool holdAfter = false;
        }
    }
}
