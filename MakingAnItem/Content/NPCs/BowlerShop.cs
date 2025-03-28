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
    public class BowlerShop
    {

        public static void Init()
        {
            mapIcon = ItemBuilder.SpriteFromBundle("bowlershop_mapicon", Initialisation.NPCCollection.GetSpriteIdByName("bowlershop_mapicon"), Initialisation.NPCCollection, new GameObject("bowlershop_mapicon"));
            mapIcon.MakeFakePrefab();

            GameObject bowlerShopPlaceable = new GameObject("BowlerShop");
            bowlerShopPlaceable.SetActive(false);
            FakePrefab.MarkAsFakePrefab(bowlerShopPlaceable);

            var carpet = ItemBuilder.SpriteFromBundle("bowlershop_carpet", Initialisation.NPCCollection.GetSpriteIdByName("bowlershop_carpet"), Initialisation.NPCCollection, new GameObject("Carpet"));
            carpet.transform.SetParent(bowlerShopPlaceable.transform);
            carpet.transform.localPosition = new Vector3(-33f / 16f, -26f / 16f);
            tk2dSprite carpetSprite = carpet.GetComponent<tk2dSprite>();
            carpetSprite.HeightOffGround = -1.7f;
            carpetSprite.SortingOrder = 0;
            carpet.layer = 20;
            carpetSprite.IsPerpendicular = false;

            var bigStatue = ItemBuilder.SpriteFromBundle("bowlershop_bigstatue", Initialisation.NPCCollection.GetSpriteIdByName("bowlershop_bigstatue"), Initialisation.NPCCollection, new GameObject("BigBowlerStatue"));
            bigStatue.transform.SetParent(bowlerShopPlaceable.transform);
            bigStatue.transform.localPosition = new Vector3(-34f / 16f, 78f / 16f);
            bigStatue.GetComponent<tk2dSprite>().HeightOffGround = 0.1f;

            var bigStatueBody = bigStatue.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(new IntVector2(24, -3), new IntVector2(35, 29));
            bigStatueBody.CollideWithTileMap = false;
            bigStatueBody.CollideWithOthers = true;
            bigStatueBody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.HighObstacle;

            var bigStatueShadow = ItemBuilder.SpriteFromBundle("bowlershop_bigstatue_shadow", Initialisation.NPCCollection.GetSpriteIdByName("bowlershop_bigstatue_shadow"), Initialisation.NPCCollection, new GameObject("BowlerStatueShadow"));
            bigStatueShadow.transform.SetParent(bigStatueBody.transform);
            bigStatueShadow.transform.localPosition = new Vector3(6f / 16f, -9f / 16f, 50f);
            tk2dSprite shadowSprite = bigStatueShadow.GetComponent<tk2dSprite>();
            shadowSprite.HeightOffGround = -1.7f;
            shadowSprite.SortingOrder = 0;
            shadowSprite.IsPerpendicular = false;
            shadowSprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTilted");
            shadowSprite.usesOverrideMaterial = true;

            var bowler = ItemBuilder.SpriteFromBundle("bowlershop_bowler_idle_001", Initialisation.NPCCollection.GetSpriteIdByName("bowlershop_bowler_idle_001"), Initialisation.NPCCollection, new GameObject("Bowler"));
            bowler.transform.SetParent(bigStatue.transform);
            bowler.transform.localPosition = new Vector3(58f / 16f, 63f / 16f);
            tk2dSprite bowlerSprite = bowler.GetComponent<tk2dSprite>();
            bowlerSprite.HeightOffGround = 20f;

            tk2dSpriteAnimator bowlerAnimator = bowler.GetOrAddComponent<tk2dSpriteAnimator>();
            bowlerAnimator.Library = Initialisation.npcAnimationCollection;
            bowlerAnimator.defaultClipId = Initialisation.npcAnimationCollection.GetClipIdByName("bowlershop_idle");
            bowlerAnimator.DefaultClipId = Initialisation.npcAnimationCollection.GetClipIdByName("bowlershop_idle");
            bowlerAnimator.playAutomatically = true;

            Transform talktransform = new GameObject("talkpoint").transform;
            talktransform.SetParent(bowler.transform);
            talktransform.transform.localPosition = new Vector3(11 / 16f, 22f / 16f);

            Transform transform = new GameObject("interactPoint").transform;
            transform.SetParent(bowler.transform);
            transform.transform.localPosition = new Vector3(16 / 16f, -47f / 16f);
            transform.gameObject.AddComponent<BowlerShopInteractible>();

            Dictionary<GameObject, float> dict = new Dictionary<GameObject, float>() { { bowlerShopPlaceable, 1f } };
            DungeonPlaceable placeable = BreakableAPIToolbox.GenerateDungeonPlaceable(dict);
            StaticReferences.StoredDungeonPlaceables.Add("bowler_shop", placeable);
            Alexandria.DungeonAPI.StaticReferences.customPlaceables.Add("nn:bowler_shop", placeable);


            var decorStatue1 = ItemBuilder.SpriteFromBundle("bowlershop statue1", Initialisation.NPCCollection.GetSpriteIdByName("bowlershop statue1"), Initialisation.NPCCollection, new GameObject("BowlerShop Statue 1"));
            decorStatue1.SetActive(false);
            FakePrefab.MarkAsFakePrefab(decorStatue1);
            decorStatue1.GetComponent<tk2dSprite>().HeightOffGround = 0.1f;
            decorStatue1.GetComponent<MeshRenderer>().lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            decorStatue1.GetComponent<tk2dSprite>().renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTiltedCutout");
            decorStatue1.GetComponent<tk2dSprite>().usesOverrideMaterial = true;
            var decorStatue1Body = decorStatue1.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(new IntVector2(1, -1), new IntVector2(35, 18));
            decorStatue1Body.CollideWithTileMap = false;
            decorStatue1Body.CollideWithOthers = true;
            decorStatue1Body.PrimaryPixelCollider.CollisionLayer = CollisionLayer.HighObstacle;

            var decorStatue2 = ItemBuilder.SpriteFromBundle("bowlershop statue2", Initialisation.NPCCollection.GetSpriteIdByName("bowlershop statue2"), Initialisation.NPCCollection, new GameObject("BowlerShop Statue 2"));
            decorStatue2.SetActive(false);
            FakePrefab.MarkAsFakePrefab(decorStatue2);
            decorStatue2.GetComponent<tk2dSprite>().HeightOffGround = 0.1f;
            var decorStatue2Body = decorStatue2.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(new IntVector2(1, -1), new IntVector2(35, 18));
            decorStatue2Body.CollideWithTileMap = false;
            decorStatue2Body.CollideWithOthers = true;
            decorStatue2.GetComponent<MeshRenderer>().lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            decorStatue2.GetComponent<tk2dSprite>().renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTiltedCutout");
            decorStatue2.GetComponent<tk2dSprite>().usesOverrideMaterial = true;
            decorStatue2Body.PrimaryPixelCollider.CollisionLayer = CollisionLayer.HighObstacle;


            var smallStatueShadow = ItemBuilder.SpriteFromBundle("bowlershop_statueshadow", Initialisation.NPCCollection.GetSpriteIdByName("bowlershop_statueshadow"), Initialisation.NPCCollection, new GameObject("BowlerSmallStatueShadow"));
            tk2dSprite smallStatueShadowSprite = smallStatueShadow.GetComponent<tk2dSprite>();
            smallStatueShadowSprite.HeightOffGround = -1.7f;
            smallStatueShadowSprite.SortingOrder = 0;
            smallStatueShadowSprite.IsPerpendicular = false;
            smallStatueShadowSprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTilted");
            smallStatueShadowSprite.usesOverrideMaterial = true;

            var smallStatueShadow2 = UnityEngine.Object.Instantiate(smallStatueShadow);

            smallStatueShadow.transform.SetParent(decorStatue1.transform);
            smallStatueShadow.transform.localPosition = new Vector3(2f / 16f, -1f / 16f, 50f);
            smallStatueShadow2.transform.SetParent(decorStatue2.transform);
            smallStatueShadow2.transform.localPosition = new Vector3(2f / 16f, -1f / 16f, 50f);

            Dictionary<GameObject, float> dict2 = new Dictionary<GameObject, float>() { { decorStatue1, 1f } };
            DungeonPlaceable placeable2 = BreakableAPIToolbox.GenerateDungeonPlaceable(dict2);
            placeable2.isPassable = false;
            placeable2.width = 2;
            placeable2.height = 2;
            StaticReferences.StoredDungeonPlaceables.Add("bowler_shop_statue1", placeable2);
            Alexandria.DungeonAPI.StaticReferences.customPlaceables.Add("nn:bowler_shop_statue1", placeable2);

            Dictionary<GameObject, float> dict3 = new Dictionary<GameObject, float>() { { decorStatue2, 1f } };
            DungeonPlaceable placeable3 = BreakableAPIToolbox.GenerateDungeonPlaceable(dict3);
            placeable3.isPassable = false;
            placeable3.width = 2;
            placeable3.height = 2;
            StaticReferences.StoredDungeonPlaceables.Add("bowler_shop_statue2", placeable3);
            Alexandria.DungeonAPI.StaticReferences.customPlaceables.Add("nn:bowler_shop_statue2", placeable3);


            SharedInjectionData npcTable = GameManager.Instance.GlobalInjectionData.entries[0].injectionData;
            npcTable.InjectionData.Add(new ProceduralFlowModifierData()
            {
                annotation = "BowlerShop",
                DEBUG_FORCE_SPAWN = false,
                OncePerRun = false,
                placementRules = new List<ProceduralFlowModifierData.FlowModifierPlacementType>()
                {
                    ProceduralFlowModifierData.FlowModifierPlacementType.END_OF_CHAIN
                },
                roomTable = null,
                exactRoom = RoomFactory.BuildNewRoomFromResource("NevernamedsItems/Content/NPCs/Rooms/BowlerShop.newroom").room,
                IsWarpWing = false,
                RequiresMasteryToken = false,
                chanceToLock = 0,
                selectionWeight = 0.067f,
                chanceToSpawn = 1,
                RequiredValidPlaceable = null,
                prerequisites = new List<DungeonPrerequisite>()
                {
                    new DungeonPrerequisite{
                    prerequisiteType = DungeonPrerequisite.PrerequisiteType.FLAG,
                    requireFlag = true,
                    saveFlagToCheck = GungeonFlags.BOWLER_EVER_SPOKEN,
                }}.ToArray(),
                CanBeForcedSecret = false,
                RandomNodeChildMinDistanceFromEntrance = 0,
                exactSecondaryRoom = null,
                framedCombatNodes = 0,
            });

        }
        public static GameObject mapIcon;
        public class BowlerShopInteractible : BraveBehaviour, IPlayerInteractable
        {
            public GameObject bowler;
            public tk2dSprite bowlerSprite;
            public tk2dSpriteAnimator bowlerSpriteAnimator;
            private RoomHandler m_room;
            private Transform talkpoint;
            private void Start()
            {
                bowler = base.transform.parent.gameObject;
                bowlerSprite = bowler.GetComponent<tk2dSprite>();
                bowlerSpriteAnimator = bowler.GetComponent<tk2dSpriteAnimator>();
                talkpoint = bowler.transform.Find("talkpoint");
                SpriteOutlineManager.AddOutlineToSprite(bowlerSprite, Color.black);
                this.m_room = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Round));
                this.m_room.RegisterInteractable(this);

                Minimap.Instance.RegisterRoomIcon(m_room, BowlerShop.mapIcon, false);
            }

            public float GetDistanceToPoint(Vector2 point)
            {
                return Vector2.Distance(point, base.transform.position) / 1.5f;
            }

            public void OnEnteredRange(PlayerController interactor)
            {
                SpriteOutlineManager.RemoveOutlineFromSprite(bowlerSprite, true);
                SpriteOutlineManager.AddOutlineToSprite(bowlerSprite, Color.white);
            }

            public void OnExitRange(PlayerController interactor)
            {
                SpriteOutlineManager.RemoveOutlineFromSprite(bowlerSprite, true);
                SpriteOutlineManager.AddOutlineToSprite(bowlerSprite, Color.black);
            }

            public void Interact(PlayerController interactor)
            {
                if (!TextBoxManager.HasTextBox(talkpoint))
                {
                    base.StartCoroutine(HandleInteract(interactor));
                }
            }

            public IEnumerator HandleInteract(PlayerController interactor)
            {
                if (hasBeenUsed)
                {
                    switch (timesInteractedPostChest)
                    {
                        case 0:
                            yield return Conversation("{wb}NO REFUUUUUNDSSSSSS~{w}", interactor);
                            break;
                        case 1:
                            yield return Conversation(BraveUtility.RandomElement(randomTalk), interactor);
                            break;
                        default:
                            yield return Conversation(BraveUtility.RandomElement(endTalk), interactor);
                            break;
                    }
                    timesInteractedPostChest++;
                }
                else
                {
                    interactor.SetInputOverride("npcConversation");
                    Pixelator.Instance.LerpToLetterbox(0.35f, 0.25f);
                    CameraController mainCameraController = GameManager.Instance.MainCameraController;
                    mainCameraController.SetManualControl(true, true);
                    mainCameraController.OverridePosition = bowler.transform.position;

                    if (hasDeclinedOnce)
                    {
                        yield return LongConversation(new List<string>() {
                                    "{wb}SOOOOOOOOO{w}",
                                    "Did you change your miiiiind?"

                            }, interactor);
                        yield return DoFinalChoice(interactor);
                    }
                    else if (SaveAPIManager.GetFlag(CustomDungeonFlags.BOWLERSHOP_METONCE))
                    {
                        yield return LongConversation(new List<string>() {
                                    "{wb}HEEEEEYYYYY{w} it's my favourite {wb}RAINBUDDY!{w}",
                                    "Do you wanna get... {wb}CHROMATIC?{w}"
                            }, interactor);
                        yield return DoFinalChoice(interactor);
                    }
                    else
                    {
                        yield return LongConversation(new List<string>() {
                                    "{wb}HEEEEeeeeeEEeeeeEEYYYYY{w}",
                                    "Fancy seeing {wb}YOU{w} here."
                            }, interactor);
                        GameUIRoot.Instance.DisplayPlayerConversationOptions(interactor, null, "Hello", "Bowler, what are you doing.");
                        int selectedResponse = -1;
                        while (!GameUIRoot.Instance.GetPlayerConversationResponse(out selectedResponse))
                            yield return null;
                        if (selectedResponse == 1)
                        {
                            yield return LongConversation(new List<string>() {
                                    "Are you inquiiiiiiring as to my {wb}MIGHTY{w} podium?",
                                    "I made it to look juuuuuust like the {wb}coolest person I know!{w}...",
                                    "...{wb}YOU{w}.",
                                    "AAAAnyways."

                            }, interactor);
                        }
                        yield return LongConversation(new List<string>() {
                                    "I thought of a neeeeeew way to put some {wb}RAINBOW{w} in your life~",
                                    "For just a little [sprite \"ui_coin\"], I can {wb}spice things up{w} with a little {wb}colour{w}!",
                                    "How about... {wb}"+ Cost +"{w} [sprite \"ui_coin\"]?"

                            }, interactor);
                        SaveAPIManager.SetFlag(CustomDungeonFlags.BOWLERSHOP_METONCE, true);

                        yield return DoFinalChoice(interactor);
                    }
                }
            }
            public int Cost
            {
                get
                {
                    float multiplier = GameManager.Instance.PrimaryPlayer.stats.GetStatValue(PlayerStats.StatType.GlobalPriceMultiplier);
                    if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && GameManager.Instance.SecondaryPlayer)
                    {
                        multiplier *= GameManager.Instance.SecondaryPlayer.stats.GetStatValue(PlayerStats.StatType.GlobalPriceMultiplier);
                    }
                    GameLevelDefinition lastLoadedLevelDefinition = GameManager.Instance.GetLastLoadedLevelDefinition();
                    multiplier *= (lastLoadedLevelDefinition == null) ? 1f : lastLoadedLevelDefinition.priceMultiplier;

                    return Mathf.FloorToInt((90 * multiplier) * 0.8f);
                }
            }

            public IEnumerator DoFinalChoice(PlayerController speaker)
            {
                GameUIRoot.Instance.DisplayPlayerConversationOptions(speaker, null, $"Yeah whatever <Pay {Cost}[sprite \"ui_coin\"]>", "That's way too expensive!");
                int response2 = -1;
                while (!GameUIRoot.Instance.GetPlayerConversationResponse(out response2))
                    yield return null;

                if (response2 == 0)
                {
                    if (speaker.carriedConsumables.Currency >= Cost)
                    {
                        yield return LongConversation(new List<string>() {
                           "Let's {wb}GOOOOOOOO{w}"
                           }, speaker, true);
                        speaker.carriedConsumables.Currency -= Cost;
                        GameObject chest = ChestUtility.SpawnChestEasy(((Vector2)base.transform.parent.parent.parent.position + new Vector2(-1f, 0f)).ToIntVector2(), ChestUtility.ChestTier.RAINBOW, false, Chest.GeneralChestType.UNSPECIFIED, Alexandria.Misc.ThreeStateValue.FORCENO, Alexandria.Misc.ThreeStateValue.FORCENO).gameObject;
                        chest.AddComponent<BowlerShopChest>().interactible = this;
                        hasBeenUsed = true;
                    }
                    else
                    {
                        hasDeclinedOnce = true;
                        yield return LongConversation(new List<string>() {
                           "Ohhhh noooo! You're BROOOOKE",
                           BraveUtility.RandomElement(brokeTalk)
                           }, speaker, true);
                    }
                }
                else
                {
                    hasDeclinedOnce = true;
                    yield return LongConversation(new List<string>() {
                         "Suit yourself!",
                         "...your {wb}BOOOOORING{w} non-{wb}RAINBOW{w} self!"
                         }, speaker, true);
                }

                speaker.ClearInputOverride("npcConversation");
                Pixelator.Instance.LerpToLetterbox(1, 0.25f);
                bowlerSpriteAnimator.Play("bowlershop_idle");
                GameManager.Instance.MainCameraController.SetManualControl(false, true);
                yield break;
            }
            public void OnOpenedChest()
            {
                base.StartCoroutine(Conversation(BraveUtility.RandomElement(chestOpen), GameManager.Instance.PrimaryPlayer));
            }
            public IEnumerator Conversation(string dialogue, PlayerController speaker)
            {
                bowlerSpriteAnimator.PlayForDuration("bowlershop_talk", 2f, "bowlershop_idle", false);
                TextBoxManager.ShowTextBox(talkpoint.position, talkpoint, 2f, dialogue, "bower", false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);
                yield break;
            }
            public IEnumerator LongConversation(List<string> dialogue, PlayerController speaker, bool clearAfter = false)
            {
                int conversationIndex = 0;
                bowlerSpriteAnimator.Play("bowlershop_talk");
                while (conversationIndex <= dialogue.Count - 1)
                {
                    TextBoxManager.ClearTextBox(talkpoint);
                    TextBoxManager.ShowTextBox(talkpoint.position, talkpoint, -1f, dialogue[conversationIndex], "bower", instant: false, showContinueText: true);
                    float timer = 0;
                    while (!BraveInput.GetInstanceForPlayer(speaker.PlayerIDX).ActiveActions.GetActionFromType(GungeonActions.GungeonActionType.Interact).WasPressed || timer < 0.4f)
                    {
                        timer += BraveTime.DeltaTime;
                        yield return null;
                    }
                    conversationIndex++;
                }
                if (clearAfter) { TextBoxManager.ClearTextBox(talkpoint); }
                bowlerSpriteAnimator.Play("bowlershop_idle");
                yield break;
            }



            public int timesInteractedPostChest = 0;
            public bool hasBeenUsed;
            public bool hasDeclinedOnce;
            public static List<string> randomTalk = new List<string>()
            {
                "Watch out for allllll the {wb}Snaaaaaaakes{w}!",
                "I sure am glad there aren't {wb}Rainbow Mimics{w}, that would be awwwwful!",
                "Someday we'll find it! The {wb}Rainbow Connection!{w}",
                "See you {wb}over the Rainbow{w}~",
            };
            public static List<string> endTalk = new List<string>()
            {
                "You should {wb}BOW{w} out...",
                "You're {wb}BOOOOOORING{w}.",
                "{wb}MMmmmmm...{w}"
            };
            public static List<string> brokeTalk = new List<string>()
            {
                 "I don't take {wb}favours{w} you know, come back when you have moooore [sprite \"ui_coin\"]!",
                 "Come back when you're a little... {wb}MMMmmmmmmmmmm...{w} RICHER",
                "That's so not {wb}RAINBOW RYTHMES{w}!"
            };
            public static List<string> chestOpen = new List<string>()
            {
                "{wb}EEEEENJOYYYYY{w}",
                "So much {wb}Cooooolour{w}",
                "Woooorth it",
            };

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
        }
    }

    public class BowlerShopChest : MonoBehaviour
    {
        public BowlerShop.BowlerShopInteractible interactible;
    }

    [HarmonyPatch(typeof(Chest))]
    [HarmonyPatch("SpewContentsOntoGround", MethodType.Normal)]
    public class ChestSpewPostfix
    {
        [HarmonyPrefix]
        public static bool HarmonyPrefix(Chest __instance, List<Transform> spawnTransforms)
        {
            BowlerShopChest bowlerShop = __instance.GetComponent<BowlerShopChest>();
            if (bowlerShop != null)
            {
                List<DebrisObject> list = new List<DebrisObject>();

                for (int i = 0; i < __instance.contents.Count; i++)
                {
                    List<DebrisObject> list2 = LootEngine.SpewLoot(new List<GameObject> { __instance.contents[i].gameObject }, spawnTransforms[i].position);
                    list.AddRange(list2);
                    for (int j = 0; j < list2.Count; j++)
                    {
                        if (list2[j]) { list2[j].PreventFallingInPits = true; }
                        if (!(list2[j].GetComponent<Gun>() != null))
                        {
                            if (!(list2[j].GetComponent<CurrencyPickup>() != null))
                            {
                                if (list2[j].specRigidbody != null)
                                {
                                    list2[j].specRigidbody.CollideWithOthers = false;
                                    DebrisObject debrisObject = list2[j];
                                    debrisObject.OnTouchedGround = (Action<DebrisObject>)Delegate.Combine(debrisObject.OnTouchedGround, new Action<DebrisObject>(__instance.BecomeViableItem));
                                }
                            }
                        }
                    }
                }
                bowlerShop.interactible.OnOpenedChest();
                GameManager.Instance.Dungeon.StartCoroutine(__instance.HandleRainbowRunLootProcessing(list));
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
