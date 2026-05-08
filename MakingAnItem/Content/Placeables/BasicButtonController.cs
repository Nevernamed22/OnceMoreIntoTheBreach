using Dungeonator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NevernamedsItems
{
    public class BasicButtonController : DungeonPlaceableBehaviour
    {
        private void Start()
        {
            m_parentroom = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Round));

            if (base.specRigidbody == null)
            {
                base.specRigidbody = base.GetComponentInChildren<SpeculativeRigidbody>();
            }
            base.specRigidbody.OnTriggerCollision += HandleTrigger;
        }
        private void Update()
        {
            if (IsTriggered && UntriggerDelay >= 0)
            {
                remainingTriggeredTime -= BraveTime.DeltaTime;
                if (remainingTriggeredTime <= 0)
                {
                    Untrigger();
                }
            }
        }
        private void HandleTrigger(SpeculativeRigidbody specRigidbody, SpeculativeRigidbody sourceSpecRigidbody, CollisionData collisionData)
        {
            if (IsTriggered)
            {
                remainingTriggeredTime = UntriggerDelay;
                return;
            }
            if (specRigidbody != null)
            {
                if (specRigidbody.gameObject.GetComponent<PlayerController>() && !triggeredByPlayers) { return; }
                if (specRigidbody.gameObject.GetComponent<AIActor>() && !triggeredByEnemies) { return; }
            }
            if (!CanBeTriggered) { return; }
            base.spriteAnimator.Play(triggerAnimationName);
            remainingTriggeredTime = UntriggerDelay;
            IsTriggered = true;
            SteppedOn(specRigidbody, collisionData);
        }
        private void Untrigger()
        {
            IsTriggered = false;
            if (!string.IsNullOrEmpty(untriggerAnimationName))
            {
                base.spriteAnimator.PlayUntilFinished(untriggerAnimationName, idleAnimationName);
            }
            Untriggered();
        }
        public virtual void SteppedOn(SpeculativeRigidbody stepper, CollisionData collisionData) { }
        public virtual void Untriggered() { }

        public bool CanBeTriggered = true;
        public bool IsTriggered = false;

        public float UntriggerDelay = 5f;
        public string triggerAnimationName = null;
        public string idleAnimationName = null;
        public string untriggerAnimationName = null;
        public bool triggeredByPlayers = true;
        public bool triggeredByEnemies = false;

        public RoomHandler m_parentroom;

        private float remainingTriggeredTime = 0f;

    }
}
