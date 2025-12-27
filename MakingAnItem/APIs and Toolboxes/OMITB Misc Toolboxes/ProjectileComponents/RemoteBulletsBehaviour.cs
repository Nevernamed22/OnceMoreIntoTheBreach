using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class RemoteBulletsBehaviour : MonoBehaviour
    {
        public RemoteBulletsBehaviour()
        {
            this.trackingSpeed = Gungeon.Game.Items["remote_bullets"].GetComponent<GuidedBulletsPassiveItem>().trackingSpeed;
            this.trackingCurve = Gungeon.Game.Items["remote_bullets"].GetComponent<GuidedBulletsPassiveItem>().trackingCurve;
            this.trackingTime = Gungeon.Game.Items["remote_bullets"].GetComponent<GuidedBulletsPassiveItem>().trackingTime;
        }

        public void Start()
        {
            try
            {
                this.m_projectile = base.GetComponent<Projectile>();
                this.m_projectile.PreMoveModifiers += this.PreMoveProjectileModifier;
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
                ETGModConsole.Log(e.StackTrace);
            }
        }
        private void PreMoveProjectileModifier(Projectile p)
        {
            if (p && p.Owner is PlayerController)
            {
                BraveInput instanceForPlayer = BraveInput.GetInstanceForPlayer((p.Owner as PlayerController).PlayerIDX);
                if (instanceForPlayer == null) return;
                Vector2 vector = Vector2.zero;
                if (instanceForPlayer.IsKeyboardAndMouse(false))
                {
                    vector = (p.Owner as PlayerController).unadjustedAimPoint.XY() - p.specRigidbody.UnitCenter;
                }
                else
                {
                    if (instanceForPlayer.ActiveActions == null)
                    {
                        return;
                    }
                    vector = instanceForPlayer.ActiveActions.Aim.Vector;
                }
                float num = vector.ToAngle();
                float num2 = BraveMathCollege.Atan2Degrees(p.Direction);
                float num3 = 0f;
                if (p.ElapsedTime < this.trackingTime)
                {
                    num3 = this.trackingCurve.Evaluate(p.ElapsedTime / this.trackingTime) * this.trackingSpeed;
                }
                float num4 = Mathf.MoveTowardsAngle(num2, num, num3 * BraveTime.DeltaTime);
                Vector2 vector2 = Quaternion.Euler(0f, 0f, Mathf.DeltaAngle(num2, num4)) * p.Direction;
                if (p is HelixProjectile)
                {
                    HelixProjectile helixProjectile = p as HelixProjectile;
                    helixProjectile.AdjustRightVector(Mathf.DeltaAngle(num2, num4));
                }
                if (p.OverrideMotionModule != null)
                {
                    p.OverrideMotionModule.AdjustRightVector(Mathf.DeltaAngle(num2, num4));
                }
                p.Direction = vector2.normalized;
                if (p.shouldRotate)
                {
                    p.transform.eulerAngles = new Vector3(0f, 0f, p.Direction.ToAngle());
                }
            }
        }
        private Projectile m_projectile;
        public float trackingSpeed = 45f;
        public float trackingTime = 6f;
        [CurveRange(0f, 0f, 1f, 1f)]
        public AnimationCurve trackingCurve;
    }
}
