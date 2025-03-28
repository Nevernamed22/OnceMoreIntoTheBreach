using Alexandria.ItemAPI;
using Dungeonator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class FiveFingerDiscount : PassiveItem
    {
        public static void Init()
        {
            PassiveItem item = ItemSetup.NewItem<FiveFingerDiscount>(
               "Five Finger Discount",
               "Sleight of Hand",
               "A cost-saving technique passed down among elite criminal circles for eons...",
               "fivefingerdiscount_icon") as PassiveItem;
            item.CanBeDropped = true;
            item.quality = PickupObject.ItemQuality.B;

            var inter = ItemBuilder.SpriteFromBundle("fivefingerdiscount_interactible_001", Initialisation.EnvironmentCollection.GetSpriteIdByName("fivefingerdiscount_interactible_001"), Initialisation.EnvironmentCollection, new GameObject("Five Finger Discount Interactible"));
            inter.MakeFakePrefab();
            tk2dSpriteAnimator chancelotAnimator = inter.GetOrAddComponent<tk2dSpriteAnimator>();
            chancelotAnimator.Library = Initialisation.environmentAnimationCollection;
            chancelotAnimator.defaultClipId = Initialisation.environmentAnimationCollection.GetClipIdByName("fivefingerdiscount_idle");
            chancelotAnimator.DefaultClipId = Initialisation.environmentAnimationCollection.GetClipIdByName("fivefingerdiscount_idle");
            chancelotAnimator.playAutomatically = true;
            inter.AddComponent<FiveFingerInteractible>();
            Interactible = inter;
        }
        public static GameObject Interactible;
        private RoomHandler lastroom;
        private List<RoomHandler> preprocessedRooms = new List<RoomHandler>();
        public override void Update()
        {
            if (Owner != null && Owner.CurrentRoom != null)
            {
                if (Owner.CurrentRoom != lastroom)
                {
                    foreach (BaseShopController shop in StaticReferenceManager.AllShops)
                    {
                        if (shop && shop.m_room != null && !preprocessedRooms.Contains(shop.m_room))
                        {
                           IntVector2 vec = shop.m_room.GetCenteredVisibleClearSpot(1, 1);
                            UnityEngine.Object.Instantiate(Interactible, (Vector2)vec, Quaternion.identity);
                            LootEngine.DoDefaultPurplePoof((Vector2)vec);
                            preprocessedRooms.Add(shop.m_room);
                        }
                    }
                    lastroom = Owner.CurrentRoom;
                }
            }
            base.Update();
        }
    }
    public class FiveFingerInteractible : BraveBehaviour, IPlayerInteractable
    {
        public RoomHandler m_room;
        private void Start()
        {
            SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black);
            m_room = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Round));
            m_room.RegisterInteractable(this);
        }
        public float GetDistanceToPoint(Vector2 point)
        {
            return Vector2.Distance(point, base.transform.position) / 1.5f;
        }

        public void OnEnteredRange(PlayerController interactor)
        {
            SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, true);
            SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white);
        }

        public void OnExitRange(PlayerController interactor)
        {
            SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, true);
            SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black);
        }

        public void Interact(PlayerController interactor)
        {
            if (!interactor.IsStealthed)
            {
                AkSoundEngine.PostEvent("Play_ENM_wizardred_appear_01", base.gameObject);
                interactor.StartCoroutine(this.HandleStealth(interactor));
            }
        }
        private IEnumerator HandleStealth(PlayerController user)
        {
            user.ChangeSpecialShaderFlag(1, 1f);
            user.SetIsStealthed(true, "fivefingerdiscount");
            user.SetCapableOfStealing(true, "FiveFingerDiscount", null);
            user.specRigidbody.AddCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.EnemyHitBox, CollisionLayer.EnemyCollider));
            LootEngine.DoDefaultPurplePoof(user.CenterPosition);
            yield return null;
            user.OnDidUnstealthyAction += this.BreakStealth;
            user.OnItemStolen += this.BreakStealthOnSteal;
            yield break;
        }
        private void BreakStealthOnSteal(PlayerController arg1, ShopItemController arg2)
        {
            this.BreakStealth(arg1);
        }
        private void BreakStealth(PlayerController obj)
        {
            LootEngine.DoDefaultPurplePoof(obj.CenterPosition);
            obj.OnDidUnstealthyAction -= this.BreakStealth;
            obj.OnItemStolen -= this.BreakStealthOnSteal;
            obj.specRigidbody.RemoveCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.EnemyHitBox, CollisionLayer.EnemyCollider));
            obj.ChangeSpecialShaderFlag(1, 0f);
            obj.SetIsStealthed(false, "fivefingerdiscount");
            obj.SetCapableOfStealing(false, "FiveFingerDiscount", null);
            AkSoundEngine.PostEvent("Play_ENM_wizardred_appear_01", base.gameObject);
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
    }
}

