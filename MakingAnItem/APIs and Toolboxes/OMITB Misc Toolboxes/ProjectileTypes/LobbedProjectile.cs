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
        public static Vector2 GetRelativeAim(PlayerController player)
        {
            BraveInput instanceForPlayer = BraveInput.GetInstanceForPlayer(player.PlayerIDX);
            Vector2 a = Vector2.zero;
            if (instanceForPlayer != null)
            {
                if (instanceForPlayer.IsKeyboardAndMouse(false))
                {
                    a = player.unadjustedAimPoint.XY() - player.CenterPosition;
                }
                else
                {
                    bool flag4 = instanceForPlayer.ActiveActions == null;
                    if (flag4)
                    {
                        return a;
                    }
                    a = instanceForPlayer.ActiveActions.Aim.Vector;
                }
            }
            return a;
        }
        public override void Start()
        {
            canCollide = false;
            pierceMinorBreakables = true;
            PierceProjModifier pierce = this.gameObject.GetOrAddComponent<PierceProjModifier>();
            pierce.penetratesBreakables = true;
            pierce.BeastModeLevel = PierceProjModifier.BeastModeStatus.BEAST_MODE_LEVEL_ONE;
            pierce.preventPenetrationOfActors = false;
            base.Start();
            sprite.transform.localRotation = transform.rotation;
            transform.rotation = Quaternion.identity;
            originalHeightOffGround = sprite.HeightOffGround;
            OnPostUpdate += HandleHeight;
            localLastPosition = transform.position;
            baseData.range = 999999f;
            if (destinationDist < 0f)
            {
                if (forcedDistance > 0f)
                {
                    destinationDist = forcedDistance;
                }
                else
                {
                    var own = Owner;
                    var shoot = Shooter;
                    if (own is PlayerController p && own.specRigidbody == shoot)
                    {
                        SetPlayerDestination(p);
                    }
                    else if (shoot != null && shoot.aiActor != null && shoot.aiActor.TargetRigidbody != null)
                    {
                        SetDestination(shoot.aiActor.TargetRigidbody.UnitCenter);
                    }
                    else if (own is PlayerController p2)
                    {
                        SetPlayerDestination(p2);
                    }
                }
            }
            if (destinationDist < 0f)
            {
                destinationDist = 0.0625f;
            }
        }

        public void SetPlayerDestination(PlayerController play)
        {
            var inp = BraveInput.GetInstanceForPlayer(play.PlayerIDX);
            if (inp != null)
            {
                if (inp.IsKeyboardAndMouse() && !LobbedProjectileMotion.CONTROLLER_LOB_DEBUG)
                {
                    SetDestination(play.CenterPosition + GetRelativeAim(play));
                }
                else
                {
                    PhysicsEngine.Instance.Raycast(specRigidbody.UnitCenter, GetRelativeAim(play).normalized, 1000f, out var res, true, true, int.MaxValue, CollisionLayer.Projectile, false, x => !(x.minorBreakable == null && x.majorBreakable == null));
                    if (res.SpeculativeRigidbody == null)
                    {
                        RaycastResult.Pool.Free(ref res);
                        PhysicsEngine.Instance.Raycast(specRigidbody.UnitCenter, GetRelativeAim(play).normalized, 1000f, out res, true, true, int.MaxValue, CollisionLayer.Projectile, false, x => !(x.minorBreakable == null && (x.majorBreakable == null || (!x.majorBreakable.m_isBroken && (x.GetComponent<FlippableCover>() == null || !x.GetComponent<FlippableCover>().m_flipped)))));
                    }
                    if (res.SpeculativeRigidbody == null)
                    {
                        RaycastResult.Pool.Free(ref res);
                        PhysicsEngine.Instance.Raycast(specRigidbody.UnitCenter, GetRelativeAim(play).normalized, 1000f, out res, true, true, int.MaxValue, CollisionLayer.Projectile, false, x => !((x.majorBreakable == null || (!x.majorBreakable.m_isBroken && (x.GetComponent<FlippableCover>() == null || !x.GetComponent<FlippableCover>().m_flipped))) && (x.minorBreakable == null || !x.minorBreakable.IsBroken)));
                    }
                    if (res.SpeculativeRigidbody != null)
                    {
                        SetDestination(res.SpeculativeRigidbody.UnitCenter + res.SpeculativeRigidbody.Velocity * baseData.speed * timeToLandWithNormalShotSpeed / 23);
                    }
                    else
                    {
                        SetDestination(res.Contact);
                        destinationDist = Mathf.Max(destinationDist - 0.25f, 0.0625f);
                    }
                }
            }
        }

        public void HandleHeight(Projectile proj)
        {
            proj.specRigidbody.Velocity *= destinationDist / 23f / timeToLandWithNormalShotSpeed;
            if (sprite != null)
            {
                var currentHeight = Mathf.Max(localDistanceElapsed / destinationDist * 4 * visualHeight * (1 - localDistanceElapsed / destinationDist), 0f);
                sprite.transform.localPosition = new Vector3(sprite.transform.localPosition.x, Mathf.Max(currentHeight, 0f));
                sprite.HeightOffGround = originalHeightOffGround + currentHeight;
                if (transform.rotation.eulerAngles.z != 0f)
                {
                    Quaternion rotation = sprite.transform.rotation;
                    rotation.eulerAngles = new Vector3(sprite.transform.rotation.eulerAngles.x, sprite.transform.rotation.eulerAngles.y, sprite.transform.rotation.eulerAngles.z + transform.rotation.eulerAngles.z);
                    sprite.transform.rotation = rotation;
                    transform.rotation = Quaternion.identity;
                }
                if (shouldRotate && angularVelocity == 0)
                {
                    sprite.transform.rotation = Quaternion.Euler(0f, 0f, (sprite.transform.position - lastSpritePosition).XY().ToAngle());
                }
                lastSpritePosition = sprite.transform.position;
            }
            localDistanceElapsed += Vector3.Distance(localLastPosition, transform.position);
            localLastPosition = transform.position;
            if (localDistanceElapsed >= destinationDist && !isDestroying)
            {
                localDistanceElapsed = 0f;
                StartCoroutine(HandleDestruction());
                isDestroying = true;
            }
        }

        public override void OnPreCollision(SpeculativeRigidbody body, PixelCollider collider, SpeculativeRigidbody collision, PixelCollider collisionCollider)
        {
            if (!canHitAnythingEvenWhenNotGrounded && !canCollide && collision.GetComponentInParent<DungeonDoorController>() == null && (collision.GetComponent<MajorBreakable>() == null || !collision.GetComponent<MajorBreakable>().IsSecretDoor))
            {
                PhysicsEngine.SkipCollision = true;
                return;
            }
            base.OnPreCollision(body, collider, collision, collisionCollider);
        }

        public void SetDestination(Vector2 destination, Vector2? overrideStart = null)
        {
            var lastdd = destinationDist;
            destinationDist = Mathf.Max(Vector2.Distance(transform.position.XY(), destination), 0.0625f);
            if (lastdd >= 0f)
            {
                localDistanceElapsed *= destinationDist / lastdd;
            }
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
            canCollide = true;
            yield return null;
            while (!ShouldBeDestroyed)
            {
                isLingering = true;
                OnFloorLinger();
                yield return null;
            }
            isLingering = false;
            if (IsAffectedByBounce && GetComponent<BounceProjModifier>() != null && GetComponent<BounceProjModifier>().numberOfBounces > 0)
            {
                canCollide = false;
                GetComponent<BounceProjModifier>().numberOfBounces--;
                isDestroying = false;

                SpawnProjModifier spawner = GetComponent<SpawnProjModifier>();
                if (spawner && spawnCollisionProjectilesOnFloorBounce)
                {
                    spawner.SpawnCollisionProjectiles(this.m_transform.position.XY(), base.specRigidbody.Velocity.normalized, null, false);
                }

                GoopModifier gooper = GetComponent<GoopModifier>();
                if (gooper && spawnCollisionGoopOnFloorBounce)
                {
                    DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(gooper.goopDefinition).TimedAddGoopCircle(SafeCenter, gooper.CollisionSpawnRadius, 0.5f, false);
                }

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

        public bool spawnCollisionProjectilesOnFloorBounce = false;
        public bool spawnCollisionGoopOnFloorBounce = false;
        public float forcedDistance = -1f;
        public float timeToLandWithNormalShotSpeed = 0.5f;
        public float visualHeight = 5f;
        public bool canHitAnythingEvenWhenNotGrounded;
        protected Vector3 lastSpritePosition;
        protected float destinationDist = -1f;
        protected bool isDestroying;
        protected float originalHeightOffGround;
        protected bool canCollide;
        protected float localDistanceElapsed;
        protected Vector3 localLastPosition;
        protected bool isLingering = false;
    }
    public class LobbedProjectileMotion : BraveBehaviour
    {
        public void Start()
        {
            canCollide = false;
            projectile.pierceMinorBreakables = true;
            PierceProjModifier pierce = this.gameObject.GetOrAddComponent<PierceProjModifier>();
            pierce.penetratesBreakables = true;
            pierce.BeastModeLevel = PierceProjModifier.BeastModeStatus.BEAST_MODE_LEVEL_ONE; //this applies infinite penetration for some reason
            pierce.preventPenetrationOfActors = false;
            transform.rotation = Quaternion.identity;
            if (projectile.sprite != null)
            {
                projectile.sprite.transform.localRotation = transform.rotation;
                lastSpritePosition = projectile.sprite.transform.position;
                originalHeightOffGround = projectile.sprite.HeightOffGround;
            }
            specRigidbody.OnPreRigidbodyCollision += OnPreCollision;
            specRigidbody.OnRigidbodyCollision += OnRigidbodyCollision;
            projectile.OnPostUpdate += HandleHeight;
            projectile.angularVelocity = 0f;
            localLastPosition = transform.position;
            projectile.baseData.range = 999999f;
            if (destinationDist < 0f)
            {
                if (forcedDistance > 0f)
                {
                    destinationDist = forcedDistance;
                }
                else
                {
                    var own = projectile.Owner;
                    var shoot = projectile.Shooter;
                    if (own is PlayerController p && own.specRigidbody == shoot)
                    {
                        SetPlayerDestination(p);
                    }
                    else if (shoot != null && shoot.aiActor != null && shoot.aiActor.TargetRigidbody != null)
                    {
                        SetDestination(shoot.aiActor.TargetRigidbody.UnitCenter);
                    }
                    else if (own is PlayerController p2)
                    {
                        SetPlayerDestination(p2);
                    }
                }
            }
            if (destinationDist < 0f)
            {
                destinationDist = 0.0625f;
            }
        }

        public static bool CONTROLLER_LOB_DEBUG = false;

        public void SetPlayerDestination(PlayerController play)
        {
            var inp = BraveInput.GetInstanceForPlayer(play.PlayerIDX);
            if (inp != null)
            {
                if (inp.IsKeyboardAndMouse() && !CONTROLLER_LOB_DEBUG)
                {
                    SetDestination(play.CenterPosition + LobbedProjectile.GetRelativeAim(play));
                }
                else
                {
                    PhysicsEngine.Instance.Raycast(specRigidbody.UnitCenter, LobbedProjectile.GetRelativeAim(play).normalized, 1000f, out var res, true, true, int.MaxValue, CollisionLayer.Projectile, false, x => !(x.minorBreakable == null && x.majorBreakable == null));
                    if (res.SpeculativeRigidbody == null)
                    {
                        RaycastResult.Pool.Free(ref res);
                        PhysicsEngine.Instance.Raycast(specRigidbody.UnitCenter, LobbedProjectile.GetRelativeAim(play).normalized, 1000f, out res, true, true, int.MaxValue, CollisionLayer.Projectile, false, x => !(x.minorBreakable == null && (x.majorBreakable == null || (!x.majorBreakable.m_isBroken && (x.GetComponent<FlippableCover>() == null || !x.GetComponent<FlippableCover>().m_flipped)))));
                    }
                    if (res.SpeculativeRigidbody == null)
                    {
                        RaycastResult.Pool.Free(ref res);
                        PhysicsEngine.Instance.Raycast(specRigidbody.UnitCenter, LobbedProjectile.GetRelativeAim(play).normalized, 1000f, out res, true, true, int.MaxValue, CollisionLayer.Projectile, false, x => !((x.majorBreakable == null || (!x.majorBreakable.m_isBroken && (x.GetComponent<FlippableCover>() == null || !x.GetComponent<FlippableCover>().m_flipped))) && (x.minorBreakable == null || !x.minorBreakable.IsBroken)));
                    }
                    if (res.SpeculativeRigidbody != null)
                    {
                        SetDestination(res.SpeculativeRigidbody.UnitCenter + res.SpeculativeRigidbody.Velocity * projectile.baseData.speed * timeToLandWithNormalShotSpeed / 23);
                    }
                    else
                    {
                        SetDestination(res.Contact);
                        destinationDist = Mathf.Max(destinationDist - 0.25f, 0.0625f);
                    }
                }
            }
        }

        public void HandleHeight(Projectile proj)
        {
            proj.specRigidbody.Velocity *= destinationDist / 23f / timeToLandWithNormalShotSpeed;
            if (projectile.sprite != null)
            {
                var currentHeight = Mathf.Max(localDistanceElapsed / destinationDist * 4 * visualHeight * (1 - localDistanceElapsed / destinationDist), 0f);
                projectile.sprite.transform.localPosition = new Vector3(projectile.sprite.transform.localPosition.x, Mathf.Max(currentHeight, 0f));
                projectile.sprite.HeightOffGround = originalHeightOffGround + currentHeight;
                if (transform.rotation.eulerAngles.z != 0f)
                {
                    Quaternion rotation = projectile.sprite.transform.rotation;
                    rotation.eulerAngles = new Vector3(projectile.sprite.transform.rotation.eulerAngles.x, projectile.sprite.transform.rotation.eulerAngles.y, projectile.sprite.transform.rotation.eulerAngles.z + transform.rotation.eulerAngles.z);
                    projectile.sprite.transform.rotation = rotation;
                    transform.rotation = Quaternion.identity;
                }
                if (projectile.shouldRotate && projectile.angularVelocity == 0)
                {
                    projectile.sprite.transform.rotation = Quaternion.Euler(0f, 0f, (projectile.sprite.transform.position - lastSpritePosition).XY().ToAngle());
                }
                lastSpritePosition = projectile.sprite.transform.position;
            }
            localDistanceElapsed += Vector3.Distance(localLastPosition, transform.position);
            localLastPosition = transform.position;
            if (localDistanceElapsed >= destinationDist && !isDestroying)
            {
                localDistanceElapsed = 0f;
                StartCoroutine(HandleDestruction());
                isDestroying = true;
            }
        }

        public void OnPreCollision(SpeculativeRigidbody body, PixelCollider collider, SpeculativeRigidbody collision, PixelCollider collisionCollider)
        {
            if (!canHitAnythingEvenWhenNotGrounded && !canCollide && collision.GetComponentInParent<DungeonDoorController>() == null && (collision.GetComponent<MajorBreakable>() == null || !collision.GetComponent<MajorBreakable>().IsSecretDoor))
            {
                PhysicsEngine.SkipCollision = true;
                return;
            }
        }

        public void SetDestination(Vector2 destination)
        {
            var lastdd = destinationDist;
            destinationDist = Mathf.Max(Vector2.Distance(transform.position.XY(), destination), 0.0625f);
            if (lastdd >= 0f)
            {
                localDistanceElapsed *= destinationDist / lastdd;
            }
        }

        public void OnRigidbodyCollision(CollisionData rigidbodyCollision)
        {
            projectile.m_hasPierced = false;
        }

        protected IEnumerator HandleDestruction()
        {
            canCollide = true;
            yield return null;
            if (GetComponent<BounceProjModifier>() != null && GetComponent<BounceProjModifier>().numberOfBounces > 0)
            {
                canCollide = false;
                GetComponent<BounceProjModifier>().numberOfBounces--;
                isDestroying = false;
            }
            else
            {
                projectile.DieInAir(false, true, true, false);
            }
            yield break;
        }

        public float forcedDistance = -1f;
        public float timeToLandWithNormalShotSpeed = 0.5f;
        public float visualHeight = 5f;
        public bool canHitAnythingEvenWhenNotGrounded;
        private Vector3 lastSpritePosition;
        private float destinationDist = -1f;
        private bool isDestroying;
        private float originalHeightOffGround;
        private bool canCollide;
        private float localDistanceElapsed;
        private Vector3 localLastPosition;
    }
}