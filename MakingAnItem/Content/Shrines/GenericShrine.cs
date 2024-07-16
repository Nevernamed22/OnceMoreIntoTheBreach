using Dungeonator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class GenericShrine : BraveBehaviour, IPlayerInteractable
    {
        public RoomHandler m_room;
        private Transform talkpoint;
        private GameObject m_instanceMinimapIcon;
        private void Start()
        {
            talkpoint = base.transform.Find("talkpoint");
            this.m_room = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Round));
            this.m_room.RegisterInteractable(this);

            m_instanceMinimapIcon = Minimap.Instance.RegisterRoomIcon(m_room, (GameObject)BraveResources.Load("Global Prefabs/Minimap_Shrine_Icon", ".prefab"));
            OnPlacement();
        }
        public virtual void OnPlacement()
        {

        }
        public float GetDistanceToPoint(Vector2 point)
        {
            if (!base.sprite) return float.MaxValue;
            Bounds bounds = base.sprite.GetBounds();
            bounds.SetMinMax(bounds.min + base.transform.position, bounds.max + base.transform.position);
            float num = Mathf.Max(Mathf.Min(point.x, bounds.max.x), bounds.min.x);
            float num2 = Mathf.Max(Mathf.Min(point.y, bounds.max.y), bounds.min.y);
            return Mathf.Sqrt((point.x - num) * (point.x - num) + (point.y - num2) * (point.y - num2));
        }

        public void OnEnteredRange(PlayerController interactor)
        {
            SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white);
            base.sprite.UpdateZDepth();
        }

        public void OnExitRange(PlayerController interactor)
        {
            SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, true);
            base.sprite.UpdateZDepth();
        }

        public void Interact(PlayerController interactor)
        {
            if (!TextBoxManager.HasTextBox(talkpoint))
            {
                base.StartCoroutine(Sequence(interactor));
            }
        }
        public int timesAccepted = 0;
        public virtual bool CanAccept(PlayerController Interactor) { return true; }
        public virtual void OnAccept(PlayerController Interactor) { }
        public virtual void OnDecline(PlayerController Interactor) { }
        public virtual string AcceptText(PlayerController Interactor) { return "Accept"; }
        public virtual string DeclineText(PlayerController Interactor) { return "Decline"; }
        public virtual string PanelText(PlayerController Interactor) { return "This is a shine. Dear God."; }

        public IEnumerator Sequence(PlayerController interactor)
        {
            TextBoxManager.ShowStoneTablet(talkpoint.position, talkpoint, -1f, PanelText(interactor), true, false);
            int selectedResponse = -1;
            interactor.SetInputOverride("shrineConversation");
            yield return null;

            if (CanAccept(interactor)) { GameUIRoot.Instance.DisplayPlayerConversationOptions(interactor, null, AcceptText(interactor), DeclineText(interactor)); }
            else { GameUIRoot.Instance.DisplayPlayerConversationOptions(interactor, null, DeclineText(interactor), string.Empty); }

            while (!GameUIRoot.Instance.GetPlayerConversationResponse(out selectedResponse)) { yield return null; }

            interactor.ClearInputOverride("shrineConversation");
            TextBoxManager.ClearTextBox(talkpoint);

            if (!CanAccept(interactor)) { OnDecline(interactor); }
            else
            {
                if (selectedResponse == 0) { OnAccept(interactor); timesAccepted++; }
                else { OnDecline(interactor); }
            }

            yield break;
        }
        public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
        {
            shouldBeFlipped = false;
            return string.Empty;
        }

        public float GetOverrideMaxDistance()
        {
            return 1.5f;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        public void DeregisterMapIcon()
        {
            if (m_instanceMinimapIcon)
            {
                Minimap.Instance.DeregisterRoomIcon(this.m_room, this.m_instanceMinimapIcon);
                this.m_instanceMinimapIcon = null;
            }
        }    
    }
}
