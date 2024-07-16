using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.Misc;
using Dungeonator;

namespace NevernamedsItems
{
    public class LobbedProjectile : Projectile
    {
        private RoomHandler m_room;
        public override void Start()
        {
            AIActor nearestEnemy = ((Vector2)base.LastPosition).GetNearestEnemyToPosition(true, Dungeonator.RoomHandler.ActiveEnemyType.All, null, null);
            if (nearestEnemy != null) { SetDestination(nearestEnemy.Position); }

            m_canCollide = false;
            PierceProjModifier pierce = this.gameObject.GetOrAddComponent<PierceProjModifier>();
            pierce.penetratesBreakables = true;
            pierce.penetration = 999999;
            pierce.preventPenetrationOfActors = false;
            m_currentHeightSpeed = initialSpeed;
            base.Start();
            sprite.transform.localRotation = transform.rotation;
            transform.rotation = Quaternion.identity;
            m_originalHeightOffGround = sprite.HeightOffGround;
            specRigidbody.OnPreMovement += HandleHeight;

            

                /*    if (this.ProjectilePlayerOwner() && this.ProjectilePlayerOwner().CurrentGun && !BraveInput.GetInstanceForPlayer(this.ProjectilePlayerOwner().PlayerIDX).IsKeyboardAndMouse(false))
                    {
                        Vector2 start = this.ProjectilePlayerOwner().CurrentGun.barrelOffset.position;

                        Dungeon dungeon = GameManager.Instance.Dungeon;
                        Vector2 vector = start + this.ProjectilePlayerOwner().CurrentGun.CurrentAngle.DegreeToVector2().normalized * 20;
                        this.m_room = start.GetAbsoluteRoom();
                        bool flag = false;
                        Vector2 vector2 = start;
                        IntVector2 intVector = vector2.ToIntVector2(VectorConversions.Floor);
                        if (dungeon.data.CheckInBoundsAndValid(intVector))
                        {
                            flag = dungeon.data[intVector].isExitCell;
                        }
                        float num = vector.x - start.x;
                        float num2 = vector.y - start.y;
                        float num3 = Mathf.Sign(vector.x - start.x);
                        float num4 = Mathf.Sign(vector.y - start.y);
                        bool flag2 = num3 > 0f;
                        bool flag3 = num4 > 0f;
                        int num5 = 0;
                        while (Vector2.Distance(vector2, vector) > 0.1f && num5 < 10000)
                        {
                            num5++;
                            float num6 = Mathf.Abs((((!flag2) ? Mathf.Floor(vector2.x) : Mathf.Ceil(vector2.x)) - vector2.x) / num);
                            float num7 = Mathf.Abs((((!flag3) ? Mathf.Floor(vector2.y) : Mathf.Ceil(vector2.y)) - vector2.y) / num2);
                            int num8 = Mathf.FloorToInt(vector2.x);
                            int num9 = Mathf.FloorToInt(vector2.y);
                            IntVector2 intVector2 = new IntVector2(num8, num9);
                            bool flag4 = false;
                            if (!dungeon.data.CheckInBoundsAndValid(intVector2))
                            {
                                break;
                            }
                            CellData cellData = dungeon.data[intVector2];
                            if (cellData.nearestRoom != this.m_room || cellData.isExitCell != flag)
                            {
                                break;
                            }
                            if (cellData.type != CellType.WALL)
                            {
                                flag4 = true;
                            }
                            if (flag4)
                            {
                                intVector = intVector2;
                            }
                            if (num6 < num7)
                            {
                                num8++;
                                vector2.x += num6 * num + 0.1f * Mathf.Sign(num);
                                vector2.y += num6 * num2 + 0.1f * Mathf.Sign(num2);
                            }
                            else
                            {
                                num9++;
                                vector2.x += num7 * num + 0.1f * Mathf.Sign(num);
                                vector2.y += num7 * num2 + 0.1f * Mathf.Sign(num2);
                            }
                        }
                        Vector2 end = intVector.ToCenterVector2();
                        Vector2 found = Vector2.zero;
                        List<AIActor> activeEnemies = start.GetAbsoluteRoom().GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
                        if (activeEnemies != null)
                        {
                            for (int i = 0; i < activeEnemies.Count; i++)
                            {
                                if (found == Vector2.zero)
                                {
                                    AIActor aiactor = activeEnemies[i];
                                    if (aiactor && aiactor.healthHaver && !aiactor.healthHaver.IsDead)
                                    {
                                        Vector2 zero = Vector2.zero;
                                        if (BraveUtility.LineIntersectsAABB(start, end, aiactor.specRigidbody.HitboxPixelCollider.UnitBottomLeft, aiactor.specRigidbody.HitboxPixelCollider.UnitDimensions, out zero))
                                        {
                                            found = aiactor.specRigidbody.UnitCenter;
                                        }
                                    }
                                }
                            }
                        }

                        SetDestination(found);
                    }*/
            }

        public virtual void HandleHeight(SpeculativeRigidbody body)
        {
            m_currentHeightSpeed += speedCurve.Evaluate(m_elapsedBounceTime) * 60 * LocalDeltaTime * (Owner is PlayerController ? (Owner as PlayerController).stats.GetStatValue(PlayerStats.StatType.ProjectileSpeed) : 1f);
            m_elapsedBounceTime += LocalDeltaTime * (Owner is PlayerController ? (Owner as PlayerController).stats.GetStatValue(PlayerStats.StatType.ProjectileSpeed) : 1f);
            m_currentHeight += (m_currentHeightSpeed * LocalDeltaTime) * (Owner is PlayerController ? (Owner as PlayerController).stats.GetStatValue(PlayerStats.StatType.ProjectileSpeed) : 1f);
            m_currentHeight = Mathf.Max(m_currentHeight, 0f);
            sprite.transform.localPosition = new Vector3(sprite.transform.localPosition.x, Mathf.Max(m_currentHeight, 0f));
            sprite.HeightOffGround = m_originalHeightOffGround + m_currentHeight;
            sprite.UpdateZDepth();
            if (transform.rotation.eulerAngles.z != 0f)
            {
                Quaternion rotation = sprite.transform.rotation;
                rotation.eulerAngles = new Vector3(sprite.transform.rotation.eulerAngles.x, sprite.transform.rotation.eulerAngles.y, sprite.transform.rotation.eulerAngles.z + transform.rotation.eulerAngles.z);
                sprite.transform.rotation = rotation;
                transform.rotation = Quaternion.identity;
            }
            if (projectile.shouldRotate && projectile.angularVelocity == 0)
            {
                projectile.sprite.transform.rotation = Quaternion.Euler(0f, 0f, (projectile.sprite.transform.position - lastSpritePosition).XY().ToAngle());
            }
            lastSpritePosition = projectile.sprite.transform.position;
            if (m_currentHeight <= 0f && m_currentHeightSpeed < 0f && !m_isDestroying)
            {
                StartCoroutine(HandleDestruction());
                m_isDestroying = true;
            }
        }

        public override void OnPreCollision(SpeculativeRigidbody body, PixelCollider collider, SpeculativeRigidbody collision, PixelCollider collisionCollider)
        {
            if (!m_canCollide && collision.GetComponentInParent<DungeonDoorController>() == null && (collision.GetComponent<MajorBreakable>() == null || !collision.GetComponent<MajorBreakable>().IsSecretDoor))
            {
                PhysicsEngine.SkipCollision = true;
                return;
            }
            base.OnPreCollision(body, collider, collision, collisionCollider);
        }

        public void EnsureSimulatedTime()
        {
            if (simulatedTime == null)
            {
                float? time = SimulateTime();
                simulatedTime = time;
            }
        }

        public void SetDestination(Vector2 destination, Vector2? overrideStart = null)
        {
            EnsureSimulatedTime();
            if (simulatedTime != null)
            {
                if (m_originalSpeed == null) m_originalSpeed = baseData.speed;
                baseData.speed = m_originalSpeed.GetValueOrDefault() * Vector2.Distance(overrideStart ?? transform.position.XY(), destination) / (simulatedTime.GetValueOrDefault() * Time.timeScale);
            }
        }

        public float? SimulateTime()
        {
            if (speedCurve.keys[speedCurve.length - 1].value >= 0f)
            {
                return null;
            }
            float time = 0f;
            float speed = initialSpeed;
            float height = 0f;
            float actualSpeed = 1f;
            float distance = 0f;
            bool breakNextFrame = false;
            while (true)
            {
                time += GameManager.INVARIANT_DELTA_TIME * (Owner is PlayerController ? (Owner as PlayerController).stats.GetStatValue(PlayerStats.StatType.ProjectileSpeed) : 1f);
                speed += speedCurve.Evaluate(time) * 60 * GameManager.INVARIANT_DELTA_TIME * (Owner is PlayerController ? (Owner as PlayerController).stats.GetStatValue(PlayerStats.StatType.ProjectileSpeed) : 1f);
                height += speed * GameManager.INVARIANT_DELTA_TIME * (Owner is PlayerController ? (Owner as PlayerController).stats.GetStatValue(PlayerStats.StatType.ProjectileSpeed) : 1f);
                distance += actualSpeed * GameManager.INVARIANT_DELTA_TIME;
                if (breakNextFrame)
                {
                    break;
                }
                if (height <= 0f && speed < 0f)
                {
                    breakNextFrame = true;
                }
            }
            return time;
        }

        public override void Move()
        {
            if (angularVelocity != 0f)
            {
                sprite.transform.RotateAround(sprite.transform.position.XY(), Vector3.forward, angularVelocity * LocalDeltaTime);
            }
            float original = angularVelocity;
            angularVelocity = 0f;
            base.Move();
            angularVelocity += original;
        }

        public override void OnRigidbodyCollision(CollisionData rigidbodyCollision)
        {
            base.OnRigidbodyCollision(rigidbodyCollision);
            m_hasPierced = false;
        }

        protected IEnumerator HandleDestruction()
        {
            m_canCollide = true;
            yield return null;
            var cachedSpeed = baseData.speed;
            while (!ShouldBeDestroyed)
            {
                baseData.speed = 0f;
                UpdateSpeed();
                OnFloorLinger();
                yield return null;
            }
            baseData.speed = cachedSpeed;
            UpdateSpeed();
            if (GetComponent<BounceProjModifier>() != null && GetComponent<BounceProjModifier>().numberOfBounces > 0 && IsAffectedByBounce)
            {
                m_canCollide = false;
                m_elapsedBounceTime = 0f;
                m_currentHeightSpeed = initialSpeed;
                m_currentHeight = 0f;
                GetComponent<BounceProjModifier>().numberOfBounces--;
                m_isDestroying = false;
                DoBounceReset();
            }
            else
            {
                DieInAir(false, true, true, false);
            }
            yield break;
        }

        public virtual bool IsAffectedByBounce => true;
        public virtual bool ShouldBeDestroyed => true;
        public virtual void DoBounceReset()
        {
        }
        public virtual void OnFloorLinger()
        {
        }
        protected float? simulatedTime;
        public AnimationCurve speedCurve;
        public float initialSpeed;
        public float flySpeedMultiplier;
        public Vector2 destinationOffset;
        protected float m_elapsedBounceTime;
        protected bool m_isDestroying;
        protected float m_currentHeightSpeed;
        protected float m_currentHeight;
        protected float m_originalHeightOffGround;
        protected bool m_canCollide;
        protected float? m_originalSpeed;
        private Vector3 lastSpritePosition;
    }
}