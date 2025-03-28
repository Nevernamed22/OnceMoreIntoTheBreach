using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    class OscillatingProjectileModifier : BraveBehaviour
    {
        public float oscillationTime = 0.5f;

        public bool multiplyScale = false;
        public float minScaleMult = 1f;
        public float maxScaleMult = 3f;

        public bool multiplyDamage = false;
        public float minDamageMult = 1;
        public float maxDamageMult = 2;

        public bool multiplySpeed = false;
        public float minSpeedMult = 1;
        public float maxSpeedMult = 0.2f;

        public bool multiplyRange = false;
        public float minRangeMult = 0.8f;
        public float maxRangeMult = 1.2f;

        private float lastMult = 1;
        private float damageMult = 1;
        private float latspeed = 1;
        private float lastRange = 1;


        private float elapsedTime = 0f;
        private void Update()
        {
            var pingPongValue = Mathf.PingPong(elapsedTime, oscillationTime);
            var scaleMultiplier = Mathf.SmoothStep(minScaleMult, maxScaleMult, pingPongValue);
            var damageMultiplier = Mathf.SmoothStep(minDamageMult, maxDamageMult, pingPongValue);
            var speedMultiplier = Mathf.SmoothStep(minSpeedMult, maxSpeedMult, pingPongValue);
            var rangeMultiplier = Mathf.SmoothStep(minRangeMult, maxRangeMult, pingPongValue);
            elapsedTime += BraveTime.DeltaTime;

            if (multiplyScale) { projectile.RuntimeUpdateScale(scaleMultiplier / lastMult); }
            if (multiplyDamage) { projectile.baseData.damage *= damageMultiplier / damageMult; }
            if (multiplySpeed)
            {
                projectile.baseData.speed *= speedMultiplier / latspeed;
                projectile.UpdateSpeed();
            }
            if (multiplyRange) { projectile.baseData.range *= rangeMultiplier / lastRange; }

            lastMult = scaleMultiplier;
            damageMult = damageMultiplier;
            latspeed = speedMultiplier;
            lastRange = rangeMultiplier;
        }
    }
}
