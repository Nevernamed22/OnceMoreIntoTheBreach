using Dungeonator;
using Pathfinding;
using UnityEngine;

namespace NevernamedsItems
{
    public class OMITBSimpleWanderBehaviour : MovementBehaviorBase
    {

        public OMITBSimpleWanderBehaviour()
        {
            AllowFearState = false;
            InitialDelay = 1;
            PointReachedPauseTime = -1;
            PathingTime = 6;
            StopRange = 1f;
        }

        public bool AllowFearState;

        public float InitialDelay;
        public float PointReachedPauseTime;
        public float PathingTime;
        public float StopRange;

        private float waitTimer;
        private float giveUpTimer;
        private float initial_delay_timer;

        private bool m_IsPathing;

        private Vector2? m_targetPos;

        public override void Start()
        {
            base.Start();
            m_IsPathing = false;
            // please add below line
            PointReachedPauseTime = -1f;
            // please add above line
            waitTimer = PointReachedPauseTime;
            giveUpTimer = 5f;
            initial_delay_timer = InitialDelay;
        }

        public override bool AllowFearRunState { get { return AllowFearState; } }

        public override void Upkeep()
        {
            base.Upkeep();
            DecrementTimer(ref waitTimer, false);
            DecrementTimer(ref giveUpTimer, false);
            DecrementTimer(ref initial_delay_timer, false);
        }

        public override BehaviorResult Update()
        {
            if (!m_aiActor) { return BehaviorResult.Continue; }

            if (m_IsPathing) //Actor is Moving
            {
                //If actor is within the StopRange (0.5f) of the target position, or the target is undefined, or if we've spent too long walking there, end the path and begin waiting.
                if (m_targetPos == null || Vector2.Distance(m_aiActor.Position, (Vector2)m_targetPos) <= StopRange || giveUpTimer <= 0f)
                {
                    m_targetPos = null;
                    this.m_aiActor.ClearPath();
                    waitTimer = (PointReachedPauseTime >= 0) ? PointReachedPauseTime : Random.Range(4, 6);
                    m_IsPathing = false;
                    return BehaviorResult.Continue;
                }

                //If none of the above are true, skip all remaining class behaviours to focus on pathfinding to the target
                return BehaviorResult.SkipRemainingClassBehaviors;
            }
            else //Actor is Not Moving
            {
                //If the initial delay or wait timers are greater than 0, it means the actor shouldn't be moving, so we can return Continue
                if (initial_delay_timer > 0) { return BehaviorResult.Continue; }
                if (waitTimer > 0) { return BehaviorResult.Continue; }

                //If both timers are at 0, we can select a new pathfinding position
                giveUpTimer = PathingTime;
                RoomHandler roomHandler = m_aiActor.ParentRoom;
                if (roomHandler == null) { return BehaviorResult.Continue; }
                IntVector2? int_pos = roomHandler.GetRandomAvailableCell(new IntVector2?(m_aiActor.Clearance), new CellTypes?(m_aiActor.PathableTiles), false, new CellValidator(SimpleCellValidator));
                if (!int_pos.HasValue | !SimpleCellValidator(int_pos.Value)) { return BehaviorResult.Continue; }
                m_targetPos = int_pos.Value.ToCenterVector2();
                m_aiActor.PathfindToPosition((Vector2)m_targetPos);
                m_IsPathing = true;
                giveUpTimer = PathingTime;

                return BehaviorResult.SkipRemainingClassBehaviors;

            }
        }

        private bool SimpleCellValidator(IntVector2 c)
        {
            for (int X = 0; X < m_aiActor.Clearance.x; X++)
            {
                for (int Y = 0; Y < m_aiActor.Clearance.y; Y++)
                {
                    if (GameManager.Instance.Dungeon.data.isTopWall(c.x + X, c.y + Y)) { return false; }
                    if (GameManager.Instance.Dungeon.data.isAnyFaceWall(c.x + X, c.y + Y)) { return false; }
                }
            }
            float dist = Vector2.Distance(m_aiActor.Position, c.ToVector2());
            if (dist > 7f || dist < 1) { return false; }
            return true;
        }
    }
}