using Alexandria.BreakableAPI;
using Alexandria.ChestAPI;
using Alexandria.DungeonAPI;
using Alexandria.ItemAPI;
using Dungeonator;
using HarmonyLib;
using SaveAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class Dispenser : BraveBehaviour, IPlayerInteractable
    {
        public static void Init()
        {
            var dispenser = ItemBuilder.SpriteFromBundle("dispenser_idle_001", Initialisation.NPCCollection.GetSpriteIdByName("dispenser_idle_001"), Initialisation.NPCCollection, new GameObject("Dispenser"));
            dispenser.SetActive(false);
            FakePrefab.MarkAsFakePrefab(dispenser);
            tk2dSprite dispenserSprite = dispenser.GetComponent<tk2dSprite>();
            dispenserSprite.HeightOffGround = 0.1f;
            dispenser.AddComponent<Dispenser>();

            tk2dSpriteAnimator dispenserAnimator = dispenser.GetOrAddComponent<tk2dSpriteAnimator>();
            dispenserAnimator.Library = Initialisation.npcAnimationCollection;
            dispenserAnimator.defaultClipId = Initialisation.npcAnimationCollection.GetClipIdByName("dispenser_idle");
            dispenserAnimator.DefaultClipId = Initialisation.npcAnimationCollection.GetClipIdByName("dispenser_idle");
            dispenserAnimator.playAutomatically = true;

            var dispenserbody = dispenser.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(new IntVector2(0, -1), new IntVector2(21, 21));
            dispenserbody.CollideWithTileMap = false;
            dispenserbody.CollideWithOthers = true;
            dispenser.GetComponent<tk2dSprite>().renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTiltedCutout");
            dispenser.GetComponent<tk2dSprite>().usesOverrideMaterial = true;
            dispenserbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.HighObstacle;

            GameObject itempoint = ItemBuilder.SpriteFromBundle("dispenser_disabled", Initialisation.NPCCollection.GetSpriteIdByName("dispenser_disabled"), Initialisation.NPCCollection, new GameObject("DispenserItem"));
            itempoint.transform.SetParent(dispenser.transform);
            itempoint.transform.localPosition = new Vector3(11f / 16f, 26f / 16f, 0f);
            tk2dSprite itempointSprite = itempoint.GetComponent<tk2dSprite>();
            itempointSprite.HeightOffGround = 10f;

            var smallStatueShadow = ItemBuilder.SpriteFromBundle("dispenser_shadow", Initialisation.NPCCollection.GetSpriteIdByName("dispenser_shadow"), Initialisation.NPCCollection, new GameObject("DispenserShadow"));
            tk2dSprite smallStatueShadowSprite = smallStatueShadow.GetComponent<tk2dSprite>();
            smallStatueShadowSprite.HeightOffGround = -1.7f;
            smallStatueShadowSprite.SortingOrder = 0;
            smallStatueShadowSprite.IsPerpendicular = false;
            smallStatueShadowSprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTilted");
            smallStatueShadowSprite.usesOverrideMaterial = true;
            smallStatueShadow.transform.SetParent(dispenser.transform);
            smallStatueShadow.transform.localPosition = new Vector3(-1f / 16f, -2f / 16f);

            Dictionary<GameObject, float> dict = new Dictionary<GameObject, float>() { { dispenser, 1f } };
            DungeonPlaceable placeable = BreakableAPIToolbox.GenerateDungeonPlaceable(dict);
            placeable.isPassable = false;
            placeable.width = 1;
            placeable.height = 1;
            StaticReferences.StoredDungeonPlaceables.Add("dispenser", placeable);
            Alexandria.DungeonAPI.StaticReferences.customPlaceables.Add("nn:dispenser", placeable);
        }
        public Chancellot master;
        public RoomHandler m_room;
        public GameObject itemPoint;
        public tk2dSprite itemSprite;
        public static List<Dispenser> AllDispensers = new List<Dispenser>();
        public int forSale;
        public List<int> options = new List<int>()
        {
            85, //Heart
            120, //Armor
            78, //Ammo
            600, //Spread Ammo
            224, //Blank
            67, //Key
        };

        private void Start()
        {

            this.m_room = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Round));
            this.m_room.RegisterInteractable(this);
            itemPoint = base.transform.Find("DispenserItem").gameObject;
            itemSprite = itemPoint.gameObject.GetComponent<tk2dSprite>();
            List<int> trimmedOptions = new List<int>(options);
            foreach (Dispenser dis in AllDispensers) { if (dis.m_room == this.m_room) { trimmedOptions.Remove(dis.forSale); } }

            if (trimmedOptions.Count > 0) { forSale = BraveUtility.RandomElement(trimmedOptions); }
            else { forSale = BraveUtility.RandomElement(trimmedOptions); }

            itemSprite.Collection = PickupObjectDatabase.GetById(forSale).sprite.collection;
            itemSprite.SetSprite(PickupObjectDatabase.GetById(forSale).sprite.spriteId);
            itemSprite.PlaceAtPositionByAnchor(itemPoint.transform.position, tk2dBaseSprite.Anchor.MiddleCenter);
            itemSprite.UpdateZDepthAttached(0.1f, base.transform.position.y, true);

            AllDispensers.Add(this);
            SpriteOutlineManager.AddOutlineToSprite(itemSprite, Color.black);
        }
        public override void OnDestroy()
        {
            AllDispensers.Remove(this);
            base.OnDestroy();
        }

        public float GetDistanceToPoint(Vector2 point)
        {
            return Vector2.Distance(point, base.transform.position) / 1.5f;
        }

        public void OnEnteredRange(PlayerController interactor)
        {
            SpriteOutlineManager.RemoveOutlineFromSprite(itemSprite, true);
            SpriteOutlineManager.AddOutlineToSprite(itemSprite, Color.white);

            PickupObject obk = PickupObjectDatabase.GetById(forSale);
            EncounterTrackable component = obk.GetComponent<EncounterTrackable>();
            string arg = (!(component != null)) ? obk.DisplayName : component.journalData.GetPrimaryDisplayName(false);

            GameObject gameObject = GameUIRoot.Instance.RegisterDefaultLabel(itemPoint.transform, new Vector3(15f / 16f, 0f, 0f), $"{arg}: {Cost}[sprite \"ui_coin\"]");
            dfLabel componentInChildren = gameObject.GetComponentInChildren<dfLabel>();
            componentInChildren.ColorizeSymbols = false;
            componentInChildren.ProcessMarkup = true;
        }
        private bool disabled = false;
        private bool busy = false;
        public void Disable()
        {
            disabled = true;
        }
        public void OnExitRange(PlayerController interactor)
        {
            SpriteOutlineManager.RemoveOutlineFromSprite(itemSprite, true);
            SpriteOutlineManager.AddOutlineToSprite(itemSprite, Color.black);
            GameUIRoot.Instance.DeregisterDefaultLabel(itemPoint.transform);
        }

        public void Interact(PlayerController interactor)
        {
            if (master == null)
            {
                foreach (Chancellot chance in Chancellot.allChancelots) { if (chance.m_room == this.m_room) { master = chance; } }
            }
            if (!disabled && !busy)
            {
                base.StartCoroutine(HandleInteraction(interactor));
            }
        }
        private IEnumerator HandleInteraction(PlayerController interactor)
        {
            busy = true;
            if (interactor.carriedConsumables.Currency > Cost)
            {
                interactor.carriedConsumables.Currency -= Cost;
                base.spriteAnimator.Play("dispenser_sale");

                yield return new WaitForSeconds(0.5f);
                Vector2 point = base.transform.position + new Vector3(6f / 16f, -10f / 16f, 0f);
                LootEngine.SpawnItem(PickupObjectDatabase.GetById(forSale).gameObject, point, Vector2.down, 1);
                if (master) { master.Inform("dispenserbuy", Cost); }
                yield return new WaitForSeconds(0.5f);
                base.spriteAnimator.Play("dispenser_idle");

            }
            else
            {
                base.spriteAnimator.Play("dispenser_nosale");
                if (master) { master.Inform("dispenserfail"); }
                yield return new WaitForSeconds(1.5f);
                base.spriteAnimator.Play("dispenser_idle");
            }
            busy = false;
            yield break;
        }
        public int Cost
        {
            get
            {
                int defaultPrice = PickupObjectDatabase.GetById(forSale).PurchasePrice;

                float discountMult = GameManager.Instance.PrimaryPlayer.stats.GetStatValue(PlayerStats.StatType.GlobalPriceMultiplier);
                if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && GameManager.Instance.SecondaryPlayer)
                {
                    discountMult *= GameManager.Instance.SecondaryPlayer.stats.GetStatValue(PlayerStats.StatType.GlobalPriceMultiplier);
                }
                GameLevelDefinition lastLoadedLevelDefinition = GameManager.Instance.GetLastLoadedLevelDefinition();
                float levelDiscount = (lastLoadedLevelDefinition == null) ? 1f : lastLoadedLevelDefinition.priceMultiplier;

                return Mathf.FloorToInt(defaultPrice * discountMult * levelDiscount * 1.05f);
            }
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
