using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Gungeon;
using Dungeonator;

namespace NevernamedsItems
{
    public class MegaAmmoPickup : PickupObject, IPlayerInteractable
    {
        public bool pickedUp
        {
            get
            {
                return this.m_pickedUp;
            }
        }
        private void Start()
        {
            if (this.minimapIcon != null && !this.m_pickedUp)
            {
                this.m_minimapIconRoom = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Floor));
                this.m_instanceMinimapIcon = Minimap.Instance.RegisterRoomIcon(this.m_minimapIconRoom, this.minimapIcon, false);
            }
            SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black, 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
        }
        private void GetRidOfMinimapIcon()
        {
            if (this.m_instanceMinimapIcon != null)
            {
                Minimap.Instance.DeregisterRoomIcon(this.m_minimapIconRoom, this.m_instanceMinimapIcon);
                this.m_instanceMinimapIcon = null;
            }
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
            if (Minimap.HasInstance)
            {
                this.GetRidOfMinimapIcon();
            }
        }
        public override void Pickup(PlayerController player)
        {
            if (this.m_pickedUp)
            {
                return;
            }
            this.m_pickedUp = true;
            if (!player.carriedConsumables.InfiniteKeys) player.carriedConsumables.KeyBullets -= 1;
            this.GetRidOfMinimapIcon();
            if (this.pickupVFX != null)
            {

            }
            UnityEngine.Object.Destroy(base.gameObject);
            AkSoundEngine.PostEvent("Play_OBJ_chest_open_01", base.gameObject);
        }
        public float GetDistanceToPoint(Vector2 point)
        {
            if (!base.sprite)
            {
                return 1000f;
            }
            Bounds bounds = base.sprite.GetBounds();
            bounds.SetMinMax(bounds.min + base.transform.position, bounds.max + base.transform.position);
            float num = Mathf.Max(Mathf.Min(point.x, bounds.max.x), bounds.min.x);
            float num2 = Mathf.Max(Mathf.Min(point.y, bounds.max.y), bounds.min.y);
            return Mathf.Sqrt((point.x - num) * (point.x - num) + (point.y - num2) * (point.y - num2)) / 1.5f;
        }
        public float GetOverrideMaxDistance()
        {
            return -1f;
        }
        public void OnEnteredRange(PlayerController interactor)
        {
            if (!this)
            {
                return;
            }
            if (!interactor.CurrentRoom.IsRegistered(this) && !RoomHandler.unassignedInteractableObjects.Contains(this))
            {
                return;
            }
            SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, false);
            SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white, 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
            base.sprite.UpdateZDepth();
        }
        public void OnExitRange(PlayerController interactor)
        {
            if (!this)
            {
                return;
            }
            SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, true);
            SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black, 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
            base.sprite.UpdateZDepth();
        }
        public void Interact(PlayerController interactor)
        {
            if (!this)
            {
                return;
            }
            if (interactor && interactor.carriedConsumables.KeyBullets <= 0 && !interactor.carriedConsumables.InfiniteKeys)
            {
                return;
            }
            if (RoomHandler.unassignedInteractableObjects.Contains(this))
            {
                RoomHandler.unassignedInteractableObjects.Remove(this);
            }
            SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, true);
            this.Pickup(interactor);
        }
        public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
        {
            shouldBeFlipped = false;
            return string.Empty;
        }

        public GameObject pickupVFX;
        public GameObject minimapIcon;
        private bool m_pickedUp;
        private RoomHandler m_minimapIconRoom;
        private GameObject m_instanceMinimapIcon;
    }
}
