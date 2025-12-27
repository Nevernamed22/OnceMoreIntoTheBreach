using Alexandria.Misc;
using Dungeonator;
using FullInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class BeamGunShooterOrigin : BraveBehaviour
    {
        public bool PreventBeamContinuation = false;
        public float heightOffset;
        public AIActor owner = null;
        private float angleAim = -1f;
        public Projectile beam;
        public void UpdatePosition(Vector2 pos, float angle = -1f)
        {
            base.transform.position = pos;
            angleAim = angle;
        }
        private BasicBeamController m_laserBeam;
        private bool isFiring = false;
        protected IEnumerator FireBeam(Projectile projectile)
        {
            isFiring = true;

            GameObject beamObject = UnityEngine.Object.Instantiate<GameObject>(projectile.gameObject);
            m_laserBeam = beamObject.GetComponent<BasicBeamController>();
            m_laserBeam.Owner = base.aiActor;
            m_laserBeam.HitsPlayers = (projectile.collidesWithPlayer || (base.aiActor && base.aiActor.CanTargetPlayers));
            m_laserBeam.HitsEnemies = (projectile.collidesWithEnemies || (base.aiActor && base.aiActor.CanTargetEnemies));
            m_laserBeam.HeightOffset = heightOffset;
            m_laserBeam.ContinueBeamArtToWall = !PreventBeamContinuation;

            bool firstFrame = true;
            while (this.m_laserBeam != null && isFiring)
            {
                float clampedAngle = BraveMathCollege.ClampAngle360(angleAim);
                Vector2 dirVec = new Vector3(Mathf.Cos(clampedAngle * 0.0174532924f), Mathf.Sin(clampedAngle * 0.0174532924f)) * 10f;
                this.m_laserBeam.Origin = this.transform.position;
                this.m_laserBeam.Direction = dirVec;
                if (firstFrame)
                {
                    yield return null;
                    firstFrame = false;
                }
                else
                {
                    yield return null;
                    while (Time.timeScale == 0f)
                    {
                        yield return null;
                    }
                }
            }
            if (!isFiring && m_laserBeam != null)
            {
                m_laserBeam.CeaseAttack();
            }

            this.m_laserBeam = null;
            yield break;
        }
        public void StartBeam()
        {
            base.StartCoroutine(FireBeam(beam));
        }
        public void KillBeam()
        {
            base.StartCoroutine(WindDown());
        }
        private IEnumerator WindDown()
        {
            isFiring = false;
            yield return null;
            UnityEngine.Object.Destroy(this.gameObject);
            yield break;
        }
    }
    public class CustomShootBeamGunBehaviour : BasicAttackBehavior
    {
        public float shootTime;
        public float shootTimeVariance;

        private bool isFiring = false;
        public float FiringTime = 5f;
        private float m_firingTime = 0f;
        private BeamGunShooterOrigin BeamOrigin = null;
        public Projectile beamPrefab;
        public void BeginAttack()
        {
            m_firingTime = 0f;
            CreateAndFireBeam();
            isFiring = true;
        }
        public void EndAttack()
        {
            m_firingTime = 0f;
            KillBeam();
            isFiring = false;
        }
        public void CreateAndFireBeam()
        {
            if (BeamOrigin != null) { KillBeam(); }
            GameObject beamOrig = new GameObject("Beam_Origin");
            beamOrig.transform.position = m_aiShooter.CurrentGun.barrelOffset.position;

            BeamGunShooterOrigin comp = beamOrig.AddComponent<BeamGunShooterOrigin>();
            comp.owner = this.m_aiActor;
            comp.beam = beamPrefab;
            comp.heightOffset = 0;

            comp.StartBeam();

            BeamOrigin = comp;
        }
        public void KillBeam()
        {
            if (BeamOrigin != null)
            {
                BeamOrigin.KillBeam();
            }
        }
        public override void Start()
        {
            base.Start();

            Gun gun = PickupObjectDatabase.GetById(this.m_aiShooter.equippedGunId) as Gun;
            if (gun && !string.IsNullOrEmpty(gun.enemyPreFireAnimation))
            {
                tk2dSpriteAnimationClip clipByName = gun.spriteAnimator.GetClipByName(gun.enemyPreFireAnimation);
                this.m_preFireTime = clipByName.BaseClipLength;
            }
        }
        public override void Upkeep()
        {
            base.Upkeep();
            base.DecrementTimer(ref this.m_nextShotTimer, false);
            base.DecrementTimer(ref this.m_reloadTimer, false);

            m_timeSinceLastShot += this.m_deltaTime;

            if (this.m_aiShooter && this.m_aiShooter.CurrentGun && BeamOrigin != null)
            {
                BeamOrigin.UpdatePosition(m_aiShooter.CurrentGun.barrelOffset.position, this.m_aiShooter.CurrentGun.CurrentAngle);
            }

            if (currentForcedAimingAngle != -1 && isFiring)
            {
                this.m_aiShooter.AimInDirection(BraveMathCollege.DegreesToVector(currentForcedAimingAngle, 1f));
            }
            else if (m_behaviorSpeculator.TargetRigidbody == null && !isFiring)
            {
                this.m_aiShooter.AimInDirection(BraveMathCollege.DegreesToVector(this.m_aiAnimator.FacingDirection, 1f));
            }
        }
        public override BehaviorResult Update()
        {
            //All the crap to figure out if we should proceed with the behaviour
            BehaviorResult behaviorResult = base.Update();
            if (behaviorResult != BehaviorResult.Continue)
            {
                return behaviorResult;
            }
            if (!this.IsReady())
            {
                return BehaviorResult.Continue;
            }
            if (this.m_behaviorSpeculator.TargetRigidbody == null)
            {
                return BehaviorResult.Continue;
            }
            bool Outranged = this.Range > 0f && this.m_aiActor.DistanceToTarget > this.Range;
            bool doesntHaveLineOfSight = this.LineOfSight && !this.m_aiActor.HasLineOfSightToTarget;
            if (this.m_aiActor.TargetRigidbody == null || Outranged || doesntHaveLineOfSight)
            {
                return BehaviorResult.Continue;
            }


            if (this.PreventTargetSwitching)
            {
                this.m_aiActor.SuppressTargetSwitch = true;
            }
            this.m_updateEveryFrame = true;

            BeginAttack();

            return (!this.StopDuringAttack) ? BehaviorResult.RunContinuousInClass : BehaviorResult.RunContinuous;
        }
        private float currentForcedAimingAngle = -1f;
        public override ContinuousBehaviorResult ContinuousUpdate()
        {
            base.ContinuousUpdate();

            if (!isFiring || BeamOrigin == null) { return ContinuousBehaviorResult.Finished; }

            bool Outranged = this.Range > 0f && this.m_aiActor.DistanceToTarget > this.Range;
            bool doesntHaveLineOfSight = this.LineOfSight && !this.m_aiActor.HasLineOfSightToTarget;
            if (this.m_aiActor.TargetRigidbody == null || Outranged || doesntHaveLineOfSight)
            {
                EndAttack();
            }

            m_firingTime += base.m_deltaTime;


            if (m_firingTime >= FiringTime)
            {
                EndAttack();
            }
            else
            {
                currentForcedAimingAngle = BeamOrigin.transform.position.CalculateVectorBetween(this.m_aiActor.TargetRigidbody.UnitCenter).ToAngle();
            }

            return ContinuousBehaviorResult.Continue;
        }


        public bool LineOfSight = true;

    
        public bool StopDuringAttack;

       
        public bool PreventTargetSwitching;

        // Token: 0x04003B68 RID: 15208
        [InspectorCategory("Visuals")]
        public string OverrideAnimation;

        // Token: 0x04003B69 RID: 15209
        [InspectorCategory("Visuals")]
        public string OverrideDirectionalAnimation;

        // Token: 0x04003B6A RID: 15210
        [InspectorShowIf("IsComplexBullet")]
        [InspectorCategory("Visuals")]
        public bool HideGun;

        // Token: 0x04003B6B RID: 15211
        [InspectorCategory("Visuals")]
        public bool UseLaserSight;

        // Token: 0x04003B6C RID: 15212
        [InspectorShowIf("UseLaserSight")]
        [InspectorCategory("Visuals")]
        public bool UseGreenLaser;

        // Token: 0x04003B6D RID: 15213
        [InspectorShowIf("UseLaserSight")]
        [InspectorCategory("Visuals")]
        public float PreFireLaserTime = -1f;

        // Token: 0x04003B6E RID: 15214
        [InspectorCategory("Visuals")]
        public bool AimAtFacingDirectionWhenSafe;

        // Token: 0x04003B6F RID: 15215
        private ShootGunBehavior.State m_state;

        // Token: 0x04003B70 RID: 15216
        private LaserSightController m_laserSight;

        // Token: 0x04003B71 RID: 15217
        private float m_remainingAmmo;

        // Token: 0x04003B72 RID: 15218
        private float m_reloadTimer;

        // Token: 0x04003B73 RID: 15219
        private float m_prefireLaserTimer;

        // Token: 0x04003B74 RID: 15220
        private float m_nextShotTimer;

        // Token: 0x04003B75 RID: 15221
        private float m_preFireTime;

        // Token: 0x04003B76 RID: 15222
        private float m_timeSinceLastShot;

        // Token: 0x02000D4F RID: 3407
        private enum State
        {
            // Token: 0x04003B78 RID: 15224
            Idle,
            // Token: 0x04003B79 RID: 15225
            PreFireLaser,
            // Token: 0x04003B7A RID: 15226
            PreFire,
            // Token: 0x04003B7B RID: 15227
            Firing,
            // Token: 0x04003B7C RID: 15228
            WaitingForNextShot
        }
    }
}
