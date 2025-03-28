using Alexandria.Misc;
using Dungeonator;
using Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class PathfindingProjectile : Projectile
    {
        public GameActor currentTarget;
        public override void Move()
        {
            Vector2 v = GetPathVelocityContribution();
            base.specRigidbody.Velocity = v;
            base.LastVelocity = v;
            base.m_currentDirection = v.normalized;
        }

        public override void Update()
        {
            if (currentTarget == null || currentTarget.IsGone || (currentTarget.healthHaver && currentTarget.healthHaver.IsDead))
            {
                GetNextTarget();
            }

            if (repathTimer <= 0f) { UpdatePath(); }
            else { repathTimer -= BraveTime.DeltaTime; }
            base.Update();
        }
        public float repathTimer = 0.2f;
        private Vector2 GetPathVelocityContribution()
        {
            if (m_currentPath == null || m_currentPath.Count == 0) { return Vector2.zero; }

            Vector2 unitCenter = base.specRigidbody.UnitCenter;
            Vector2 pathTarget = this.GetPathTarget();
            Vector2 vector = pathTarget - unitCenter;
            if (Speed * LocalDeltaTime > vector.magnitude)
            {
                return vector / LocalDeltaTime;
            }
            return Speed * vector.normalized;
        }
        public void UpdatePath()
        {
            if (currentTarget != null)
            {
                PathfindToPosition(currentTarget.CenterPosition);
                repathTimer = 0.2f;
            }
            else if (this.GetAbsoluteRoom() != null)
            {
                IntVector2? possib = this.GetAbsoluteRoom().GetRandomAvailableCell(new IntVector2?(new IntVector2(1, 1)), new CellTypes?(PathableTiles), false);
                if (possib != null)
                {
                    PathfindToPosition((Vector2)possib);
                }
                repathTimer = 1f;
            }
        }
        public bool PathfindToPosition(Vector2 targetPosition, bool smooth = true, CellValidator cellValidator = null, ExtraWeightingFunction extraWeightingFunction = null, CellTypes? overridePathableTiles = null, bool canPassOccupied = false)
        {
            bool result = false;
            Pathfinder.Instance.RemoveActorPath(this.m_upcomingPathTiles);
            CellTypes passableCellTypes = (overridePathableTiles == null) ? this.PathableTiles : overridePathableTiles.Value;
            Path path = null;
            if (Pathfinder.Instance.GetPath(this.PathTile, targetPosition.ToIntVector2(VectorConversions.Floor), out path, new IntVector2?(new IntVector2(1, 1)), passableCellTypes, cellValidator, extraWeightingFunction, canPassOccupied))
            {
                this.m_currentPath = path;
                if (this.m_currentPath != null && this.m_currentPath.WillReachFinalGoal) { result = true; }
                if (this.m_currentPath.Count == 0) { this.m_currentPath = null; }
                else if (smooth)
                {
                    path.Smooth(base.specRigidbody.UnitCenter, base.specRigidbody.UnitDimensions / 2f, passableCellTypes, canPassOccupied, new IntVector2(1, 1));
                }
            }
            this.UpdateUpcomingPathTiles(2f);
            Pathfinder.Instance.UpdateActorPath(this.m_upcomingPathTiles);
            return result;
        }
        private void UpdateUpcomingPathTiles(float time)
        {
            this.m_upcomingPathTiles.Clear();
            this.m_upcomingPathTiles.Add(this.PathTile);
            if (this.m_currentPath != null && this.m_currentPath.Count > 0)
            {
                float num = 0f;
                Vector2 vector = SafeCenter;
                LinkedListNode<IntVector2> linkedListNode = this.m_currentPath.Positions.First;
                Vector2 vector2 = linkedListNode.Value.ToCenterVector2();
                while (num < time)
                {
                    Vector2 vector3 = vector2 - vector;
                    if (vector3.sqrMagnitude > 0.04f)
                    {
                        vector3 = vector3.normalized * 0.2f;
                    }
                    vector += vector3;
                    IntVector2 intVector = vector.ToIntVector2(VectorConversions.Floor);
                    if (this.m_upcomingPathTiles[this.m_upcomingPathTiles.Count - 1] != intVector)
                    {
                        this.m_upcomingPathTiles.Add(intVector);
                    }
                    if (vector3.magnitude < 0.2f)
                    {
                        linkedListNode = linkedListNode.Next;
                        if (linkedListNode == null)
                        {
                            break;
                        }
                        vector2 = linkedListNode.Value.ToCenterVector2();
                    }
                    num += vector3.magnitude / Speed;
                }
            }
        }
        public IntVector2 PathTile
        {
            get
            {
                return ((Vector2)SafeCenter).ToIntVector2(VectorConversions.Floor);
            }
        }
        public void GetNextTarget()
        {
            if (Owner != null && Owner is PlayerController)
            {
                if (this.GetAbsoluteRoom() != null && this.GetAbsoluteRoom().GetActiveEnemiesCount(RoomHandler.ActiveEnemyType.RoomClear) > 0)
                {
                    AIActor nearestenemy = ((Vector2)LastPosition).GetNearestEnemyToPosition(true, RoomHandler.ActiveEnemyType.RoomClear);
                    currentTarget = nearestenemy;
                }
            }
            else
            {
                PlayerController target = GameManager.Instance.GetActivePlayerClosestToPoint(LastPosition, false);
                currentTarget = target;
            }
        }
        private bool GetNextTargetPosition(out Vector2 targetPos)
        {
            if (this.m_currentPath != null && this.m_currentPath.Count > 0)
            {
                targetPos = this.m_currentPath.GetFirstCenterVector2();
                return true;
            }
            targetPos = Vector2.zero;
            return false;
        }
        private Vector2 GetPathTarget()
        {
            Vector2 unitCenter = base.specRigidbody.UnitCenter;
            Vector2 result = unitCenter;
            float num = Speed * LocalDeltaTime;
            Vector2 vector = unitCenter;
            Vector2 vector2 = unitCenter;
            while (num > 0f)
            {
                if (this.GetNextTargetPosition(out vector2))
                {
                    float num2 = Vector2.Distance(vector2, unitCenter);
                    if (num2 < num)
                    {
                        num -= num2;
                        vector = vector2;
                        result = vector;
                        if (this.m_currentPath != null && this.m_currentPath.Count > 0)
                        {
                            this.m_currentPath.RemoveFirst();
                        }
                        continue;
                    }
                    result = (vector2 - vector).normalized * num + vector;
                }
                return result;
            }
            return result;
        }
        private Path m_currentPath;
        public CellTypes PathableTiles = CellTypes.FLOOR;
        private List<IntVector2> m_upcomingPathTiles = new List<IntVector2>();
    }
}
