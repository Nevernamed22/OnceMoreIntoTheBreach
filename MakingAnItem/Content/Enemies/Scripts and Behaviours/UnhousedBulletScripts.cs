using Brave.BulletScript;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public static class BulletScripttools
    {
        public static float EaseOut(float t)
        {
            t = Mathf.Clamp01(t);
            return 1f - (1f - t) * (1f - t);
        }
        public static Vector2 ArcPoint(Vector2 start, Vector2 end, float curveHeight, float time)
        {
            time = Mathf.Clamp01(time);
            Vector2 linear = Vector2.Lerp(start, end, time);
            Vector2 direction = end - start;
            Vector2 normal = new Vector2(-direction.y, direction.x).normalized;
            float height = Mathf.Sin(time * Mathf.PI) * curveHeight;
            return linear + normal * height;
        }
        public static float ClosenessToEitherEdgeOfRange(float rangeMax, float rangeMin, float value)
        {
            float highestNumber = Mathf.Max(rangeMax, rangeMin);
            float lowestNumber = Mathf.Min(rangeMax, rangeMin);
            float difference = highestNumber - lowestNumber;
            float halfDifference = difference * 0.5f;

            float fin = (value - halfDifference) / halfDifference;
            return fin < 0 ? fin * -1f : fin;
        }
    }
    public class OMITB_ControlledBullet : Bullet
    {
        public OMITB_ControlledBullet(Vector2 offset, int setupDelay, int setupTime) : base(null, false, false, false)
        {
            this.m_offset = offset;
            this.m_setupDelay = setupDelay;
            this.m_setupTime = setupTime;
        }
        

        public override IEnumerator Top()
        {
            base.ManualControl = true;
            this.m_offset = this.m_offset.Rotate(this.Direction);
            for (int i = 0; i < 360; i++)
            {
                if (i > this.m_setupDelay && i < this.m_setupDelay + this.m_setupTime)
                {
                    base.Position += this.m_offset / (float)this.m_setupTime;
                }
                base.Position += BraveMathCollege.DegreesToVector(this.Direction, this.Speed / 60f);
                yield return base.Wait(1);
            }
            base.Vanish(false);
            yield break;
        }
        private Vector2 m_offset;
        private int m_setupDelay;
        private int m_setupTime;
    }
    public class OMITBBigBouncyShotgun : Script
    {
        public override IEnumerator Top()
        {
            for (int j = 0; j < 5; j++)
            {
                float angle = -60 + (30 * j);
                base.Fire(new Direction(angle, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), new Bullet("laser", false, false, false));

            }
            yield return base.Wait(2.5f);
            for (int i = -10; i <= 10; i++)
            {
                base.Fire(new Direction((float)(i * 6), DirectionType.Aim, -1f), new Speed(5f, SpeedType.Absolute), new Bullet("bouncer", false, false, false));
            }
            yield break;
        }
    }
    public class ScrappedDeaconAttack : Script
    {
        public static float baseSpeed = 5f;
        public static float bonusSpeedMax = 5f;
        public static float bonusSpeedMin = 0f;
        public static float maxRotFrames = 1.5f;
        public static float minRotFrames = 0f;
        public static float minWaitTime = 20f;
        public static float maxWaitTime = 200f;
        public override IEnumerator Top()
        {
            float st = -45;
            float end = 45;
            float aimDirection = base.GetAimDirection(1f, 5f);

            for (int i = 0; i <= 21; i++)
            {

                float final = Mathf.SmoothStep(st, end, (float)i / 20f);
                float finalreverse = Mathf.SmoothStep(end, st, (float)i / 20f);

                float foo = i + 1;
                if (foo > 10) { foo -= 10; }
                else { foo = 10 - (i - 1); }



                float rotFrames = Mathf.Lerp(minRotFrames, maxRotFrames, EaseOut(foo / 10f));
                float waitTime = Mathf.Lerp(minWaitTime, maxWaitTime, EaseOut(foo / 10f));

                float startAngle = aimDirection + final;
                float endAngle = aimDirection + finalreverse;
                base.Fire(new Direction(final, DirectionType.Aim, -1f), new Speed(baseSpeed + Mathf.Lerp(bonusSpeedMin, bonusSpeedMax, EaseOut(foo / 10f)), SpeedType.Absolute), new ScrappedDeaconAttack.CurveShot(endAngle, startAngle, rotFrames, waitTime));
            }
            return null;
        }
        float EaseOut(float t)
        {
            t = Mathf.Clamp01(t);
            return 1f - (1f - t) * (1f - t);
        }
        public class CurveShot : Bullet
        {
            public CurveShot(float desiredAngle, float st, float rotationFrames, float waittime) : base("default", false, false, false)
            {
                this.startAngle = st;
                this.desiredAngle = desiredAngle;
                waitTime = waittime;
                rotFrames = rotationFrames;
            }

            public override IEnumerator Top()
            {
                this.Direction = startAngle;
                yield return base.Wait(waitTime);

                for (int i = 0; i < 200; i++)
                {
                    this.Direction = Mathf.MoveTowardsAngle(this.Direction, desiredAngle, rotFrames);
                    UpdateVelocity();

                    yield return base.Wait(1);
                }

                yield break;
            }


            private float startAngle;
            private float desiredAngle;
            private float rotFrames;
            private float waitTime;
        }

    }
}
