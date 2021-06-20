using Dungeonator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public static class CrateExtensions
    {
        public static SimplerCrateBehaviour TurnIntoSimplerCrate(this EmergencyCrateController self)
        {
            GameObject obj = self.gameObject;
            if (obj != null)
            {
                SimplerCrateBehaviour newCrateBehav = obj.AddComponent<SimplerCrateBehaviour>();
                newCrateBehav.driftAnimationName = self.driftAnimationName;
                newCrateBehav.landedAnimationName = self.landedAnimationName;
                newCrateBehav.chuteLandedAnimationName = self.chuteLandedAnimationName;
                newCrateBehav.crateDisappearAnimationName = self.crateDisappearAnimationName;
                newCrateBehav.chuteAnimator = self.chuteAnimator;
                newCrateBehav.landingTargetSprite = self.landingTargetSprite;

                UnityEngine.Object.Destroy(self);
                return newCrateBehav;
            }
            else return null;
        }
    }
    public class SimplerCrateBehaviour : BraveBehaviour
    {
        public void Trigger(Vector3 startingVelocity, Vector3 startingPosition, RoomHandler room)
        {
            this.m_parentRoom = room;
            this.m_currentPosition = startingPosition;
            this.m_currentVelocity = startingVelocity;
            this.m_hasBeenTriggered = true;
            base.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Unoccluded"));
            float num = startingPosition.z / -startingVelocity.z;
            Vector3 position = startingPosition + num * startingVelocity;
            this.m_landingTarget = SpawnManager.SpawnVFX(this.landingTargetSprite, position, Quaternion.identity);
            this.m_landingTarget.GetComponentInChildren<tk2dSprite>().UpdateZDepth();
        }
        private void Update()
        {
            if (this.m_hasBeenTriggered)
            {
                this.m_currentPosition += this.m_currentVelocity * BraveTime.DeltaTime;
                if (this.m_currentPosition.z <= 0f)
                {
                    this.m_currentPosition.z = 0f;
                    this.OnLanded();
                }
                base.transform.position = BraveUtility.QuantizeVector(this.m_currentPosition.WithZ(this.m_currentPosition.y - this.m_currentPosition.z), (float)PhysicsEngine.Instance.PixelsPerUnit);
                base.sprite.HeightOffGround = this.m_currentPosition.z;
                base.sprite.UpdateZDepth();
            }
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        private void OnLanded()
        {
            this.m_hasBeenTriggered = false;
            base.sprite.gameObject.layer = LayerMask.NameToLayer("FG_Critical");
            base.sprite.renderer.sortingLayerName = "Background";
            base.sprite.IsPerpendicular = false;
            base.sprite.HeightOffGround = -1f;
            this.m_currentPosition.z = -1f;
            base.spriteAnimator.Play(this.landedAnimationName);
            this.chuteAnimator.PlayAndDestroyObject(this.chuteLandedAnimationName, null);
            if (this.m_landingTarget)
            {
                SpawnManager.Despawn(this.m_landingTarget);
            }
            this.m_landingTarget = null;

            GameObject gameObject = PickupObjectDatabase.GetById(LootID).gameObject;

            DebrisObject spawned = LootEngine.SpawnItem(gameObject, base.sprite.WorldCenter.ToVector3ZUp(0f) + new Vector3(-0.5f, 0.5f, 0f), Vector2.zero, 0f, false, false, false);
            base.StartCoroutine(this.DestroyCrateWhenPickedUp(spawned));
        }

        private IEnumerator DestroyCrateDelayed()
        {
            yield return new WaitForSeconds(1.5f);
            if (this.m_landingTarget)
            {
                SpawnManager.Despawn(this.m_landingTarget);
            }
            this.m_landingTarget = null;
            if (this.m_parentRoom.ExtantEmergencyCrate == base.gameObject)
            {
                this.m_parentRoom.ExtantEmergencyCrate = null;
            }
            base.spriteAnimator.Play(this.crateDisappearAnimationName);
            yield break;
        }
        private IEnumerator DestroyCrateWhenPickedUp(DebrisObject spawned)
        {
            while (spawned)
            {
                yield return new WaitForSeconds(0.25f);
            }
            if (this.m_landingTarget)
            {
                SpawnManager.Despawn(this.m_landingTarget);
            }
            this.m_landingTarget = null;
            if (this.m_parentRoom.ExtantEmergencyCrate == base.gameObject)
            {
                this.m_parentRoom.ExtantEmergencyCrate = null;
            }
            base.spriteAnimator.Play(this.crateDisappearAnimationName);
            yield break;
        }      
        public int LootID;

        public void ClearLandingTarget()
        {
            if (this.m_landingTarget)
            {
                SpawnManager.Despawn(this.m_landingTarget);
            }
            this.m_landingTarget = null;
        }
        public string driftAnimationName;
        public string landedAnimationName;
        public string chuteLandedAnimationName;
        public string crateDisappearAnimationName;
        public tk2dSpriteAnimator chuteAnimator;
        public GameObject landingTargetSprite;
        private bool m_hasBeenTriggered;
        private Vector3 m_currentPosition;
        private Vector3 m_currentVelocity;
        private RoomHandler m_parentRoom;
        private GameObject m_landingTarget;
    }
}
