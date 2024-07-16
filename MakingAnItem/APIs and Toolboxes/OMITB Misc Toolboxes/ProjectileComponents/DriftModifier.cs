using Alexandria.Misc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class DriftModifier : MonoBehaviour
    {
        public DriftModifier()
        {
            DriftTimer = 1;
            maxDriftReaims = 100;
            diesAfterMaxDrifts = false;
            startInactive = false;
        }
        private void Start()
        {
            this.m_projectile = base.GetComponent<Projectile>();
            timer = DriftTimer;
            active = !startInactive;
        }
        private Projectile m_projectile;
        public float DriftTimer;
        private float timer;
        public int maxDriftReaims;
        public bool diesAfterMaxDrifts;
        private int timesReAimed;
        public bool startInactive;
        private bool active;
        public float degreesOfVariance = -1;
        public void Activate() { active = true;}
        private void Update()
        {
            if (active)
            {
                if (timer > 0) { timer -= BraveTime.DeltaTime; }
                else
                {
                    if (timesReAimed < maxDriftReaims)
                    {
                        if (degreesOfVariance != -1)
                        {
                            m_projectile.SendInDirection((m_projectile.Direction.ToAngle() + UnityEngine.Random.Range(-degreesOfVariance, degreesOfVariance)).DegreeToVector2(), false, false);
                        }
                        else { m_projectile.SendInRandomDirection(); }
                        timesReAimed++;
                    }
                    else if (diesAfterMaxDrifts) { m_projectile.DieInAir(); }
                    timer = DriftTimer;
                }
            }
        }  
    }
}
