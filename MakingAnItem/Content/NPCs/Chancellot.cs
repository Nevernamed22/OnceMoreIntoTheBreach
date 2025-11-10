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
    public class Chancellot : BraveBehaviour, IPlayerInteractable
    {
        public static void Init()
        {
            mapIcon = ItemBuilder.SpriteFromBundle("chancelot_mapicon", Initialisation.NPCCollection.GetSpriteIdByName("chancelot_mapicon"), Initialisation.NPCCollection, new GameObject("chancelot_mapicon"));
            mapIcon.MakeFakePrefab();

            GameObject chancellotShop = new GameObject("chancellotShop");
            chancellotShop.SetActive(false);
            FakePrefab.MarkAsFakePrefab(chancellotShop);

            var counter = ItemBuilder.SpriteFromBundle("chancellot_counter", Initialisation.NPCCollection.GetSpriteIdByName("chancellot_counter"), Initialisation.NPCCollection, new GameObject("Counter"));
            counter.transform.SetParent(chancellotShop.transform);
            counter.GetComponent<tk2dSprite>().HeightOffGround = -1f;                                    //-15 / -1
            var counterBody = counter.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(new IntVector2(0, 0), new IntVector2(128, 17));
            counterBody.CollideWithTileMap = false;
            counterBody.CollideWithOthers = true;
            counter.GetComponent<MeshRenderer>().material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTiltedCutout");
            counter.GetComponent<tk2dSprite>().usesOverrideMaterial = true;
            counterBody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.LowObstacle;

            var carpet = ItemBuilder.SpriteFromBundle("chancellot_carpet", Initialisation.NPCCollection.GetSpriteIdByName("chancellot_carpet"), Initialisation.NPCCollection, new GameObject("Carpet"));
            tk2dSprite carpetSprite = carpet.GetComponent<tk2dSprite>();
            carpet.layer = 20;
            carpetSprite.SortingOrder = 2;
            carpetSprite.IsPerpendicular = false;
            carpetSprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTilted");
            carpetSprite.usesOverrideMaterial = true;
            carpet.transform.SetParent(chancellotShop.transform);
            carpet.transform.localPosition = new Vector3(0f, 1f);


            var chancelot = ItemBuilder.SpriteFromBundle("chancelot_idle_001", Initialisation.NPCCollection.GetSpriteIdByName("chancelot_idle_001"), Initialisation.NPCCollection, new GameObject("Chancelot"));
            chancelot.transform.SetParent(chancellotShop.transform);
            chancelot.transform.localPosition = new Vector3(66f / 16f, 15f / 16f);
            chancelot.AddComponent<Chancellot>();
            var chancelotBody = chancelot.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(new IntVector2(8, 2), new IntVector2(17, 34));
            chancelotBody.CollideWithTileMap = false;
            chancelotBody.CollideWithOthers = true;

            UltraFortunesFavor fortune = chancelot.AddComponent<UltraFortunesFavor>();
            fortune.sparkOctantVFX = ResourceManager.LoadAssetBundle("shared_auto_002").LoadAsset<GameObject>("npc_blank_jailed").GetComponentInChildren<UltraFortunesFavor>().sparkOctantVFX;

            tk2dSpriteAnimator chancelotAnimator = chancelot.GetOrAddComponent<tk2dSpriteAnimator>();
            chancelotAnimator.Library = Initialisation.npcAnimationCollection;
            chancelotAnimator.defaultClipId = Initialisation.npcAnimationCollection.GetClipIdByName("chancelot_idle");
            chancelotAnimator.DefaultClipId = Initialisation.npcAnimationCollection.GetClipIdByName("chancelot_idle");
            chancelotAnimator.playAutomatically = true;

            Transform talktransform = new GameObject("talkpoint").transform;
            talktransform.SetParent(chancelot.transform);
            talktransform.transform.localPosition = new Vector3(0f, 49f / 16f);


            Dictionary<GameObject, float> dict = new Dictionary<GameObject, float>() { { chancellotShop, 1f } };
            DungeonPlaceable placeable = BreakableAPIToolbox.GenerateDungeonPlaceable(dict);
            StaticReferences.StoredDungeonPlaceables.Add("chancelot_shop", placeable);
            Alexandria.DungeonAPI.StaticReferences.customPlaceables.Add("nn:chancelot_shop", placeable);

            //Room Decor

            var carpetStandalone = ItemBuilder.SpriteFromBundle("chancellot_carpet", Initialisation.NPCCollection.GetSpriteIdByName("chancellot_carpet"), Initialisation.NPCCollection, new GameObject("Carpet"));
            carpetStandalone.MakeFakePrefab();
            tk2dSprite carpetStandaloneSprite = carpetStandalone.GetComponent<tk2dSprite>();
            carpetStandalone.layer = 20;
            carpetStandaloneSprite.SortingOrder = 0;
            carpetStandaloneSprite.IsPerpendicular = false;
            carpetStandaloneSprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTilted");
            carpetStandaloneSprite.usesOverrideMaterial = true;
            DungeonPlaceable carpetPlaceable = BreakableAPIToolbox.GenerateDungeonPlaceable(new Dictionary<GameObject, float>() { { carpetStandalone, 1f } });
            StaticReferences.StoredDungeonPlaceables.Add("chancelot_carpet", carpetPlaceable);
            Alexandria.DungeonAPI.StaticReferences.customPlaceables.Add("nn:chancelot_carpet", carpetPlaceable);


            var taxidermy = ItemBuilder.SpriteFromBundle("chancelot_taxidermy", Initialisation.NPCCollection.GetSpriteIdByName("chancelot_taxidermy"), Initialisation.NPCCollection, new GameObject("taxidermy"));
            taxidermy.MakeFakePrefab();
            tk2dSprite taxidermySprite = taxidermy.GetComponent<tk2dSprite>();
            taxidermySprite.HeightOffGround = -1f;
            taxidermySprite.SortingOrder = 0;
            taxidermySprite.renderLayer = 0;
            taxidermySprite.renderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            taxidermySprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTiltedCutout");
            taxidermySprite.usesOverrideMaterial = true;
            DungeonPlaceable taxidermyPlaceable = BreakableAPIToolbox.GenerateDungeonPlaceable(new Dictionary<GameObject, float>() { { taxidermy, 1f } });
            StaticReferences.StoredDungeonPlaceables.Add("chancelot_taxidermy", taxidermyPlaceable);
            Alexandria.DungeonAPI.StaticReferences.customPlaceables.Add("nn:chancelot_taxidermy", taxidermyPlaceable);

            var chancecrate1 = ItemBuilder.SpriteFromBundle("chancelot_crate_001", Initialisation.NPCCollection.GetSpriteIdByName("chancelot_crate_001"), Initialisation.NPCCollection, new GameObject("Chance Crate"));
            chancecrate1.MakeFakePrefab();
            tk2dSprite chancecrate1Sprite = chancecrate1.GetComponent<tk2dSprite>();
            chancecrate1Sprite.HeightOffGround = -1f;
            chancecrate1Sprite.SortingOrder = 0;
            chancecrate1Sprite.renderLayer = 0;
            chancecrate1Sprite.renderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            chancecrate1Sprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTiltedCutout");
            chancecrate1Sprite.usesOverrideMaterial = true;
            DungeonPlaceable chancecrate1Placeable = BreakableAPIToolbox.GenerateDungeonPlaceable(new Dictionary<GameObject, float>() { { chancecrate1, 1f } });
            StaticReferences.StoredDungeonPlaceables.Add("chancelot_chancecrate_1", chancecrate1Placeable);
            Alexandria.DungeonAPI.StaticReferences.customPlaceables.Add("nn:chancelot_chancecrate_1", chancecrate1Placeable);

            var chancecrate2 = ItemBuilder.SpriteFromBundle("chancelot_crate_002", Initialisation.NPCCollection.GetSpriteIdByName("chancelot_crate_002"), Initialisation.NPCCollection, new GameObject("Chance Crate"));
            chancecrate2.MakeFakePrefab();
            tk2dSprite chancecrate2Sprite = chancecrate2.GetComponent<tk2dSprite>();
            chancecrate2Sprite.HeightOffGround = -1f;
            chancecrate2Sprite.SortingOrder = 0;
            chancecrate2Sprite.renderLayer = 0;
            chancecrate2Sprite.renderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            chancecrate2Sprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTiltedCutout");
            chancecrate2Sprite.usesOverrideMaterial = true;
            DungeonPlaceable chancecrate2Placeable = BreakableAPIToolbox.GenerateDungeonPlaceable(new Dictionary<GameObject, float>() { { chancecrate2, 1f } });
            StaticReferences.StoredDungeonPlaceables.Add("chancelot_chancecrate_2", chancecrate2Placeable);
            Alexandria.DungeonAPI.StaticReferences.customPlaceables.Add("nn:chancelot_chancecrate_2", chancecrate2Placeable);

            var shelf = ItemBuilder.SpriteFromBundle("chancelot_shelf", Initialisation.NPCCollection.GetSpriteIdByName("chancelot_shelf"), Initialisation.NPCCollection, new GameObject("shelf"));
            shelf.MakeFakePrefab();
            tk2dSprite shelfSprite = shelf.GetComponent<tk2dSprite>();
            shelfSprite.HeightOffGround = -1f;
            shelfSprite.SortingOrder = 0;
            shelfSprite.renderLayer = 0;
            shelfSprite.renderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            shelfSprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTiltedCutout");
            shelfSprite.usesOverrideMaterial = true;
            DungeonPlaceable shelfPlaceable = BreakableAPIToolbox.GenerateDungeonPlaceable(new Dictionary<GameObject, float>() { { shelf, 1f } });
            StaticReferences.StoredDungeonPlaceables.Add("chancelot_shelf", shelfPlaceable);
            Alexandria.DungeonAPI.StaticReferences.customPlaceables.Add("nn:chancelot_shelf", shelfPlaceable);

            SharedInjectionData npcTable = GameManager.Instance.GlobalInjectionData.entries[0].injectionData;
            npcTable.InjectionData.Add(new ProceduralFlowModifierData()
            {
                annotation = "Chancelot Shop",
                DEBUG_FORCE_SPAWN = false,
                OncePerRun = false,
                placementRules = new List<ProceduralFlowModifierData.FlowModifierPlacementType>()
                {
                    ProceduralFlowModifierData.FlowModifierPlacementType.END_OF_CHAIN
                },
                roomTable = null,
                exactRoom = RoomFactory.BuildNewRoomFromResource("NevernamedsItems/Content/NPCs/Rooms/ChancelotRoomAnnex.newroom").room,
                IsWarpWing = false,
                RequiresMasteryToken = false,
                chanceToLock = 0,
                selectionWeight = 0.15f,
                chanceToSpawn = 1,
                RequiredValidPlaceable = null,
                prerequisites = new List<DungeonPrerequisite>()
                {
                    new DungeonPrerequisite{
                    prerequisiteType = DungeonPrerequisite.PrerequisiteType.FLAG,
                    requireFlag = true,
                    saveFlagToCheck = GungeonFlags.BOSSKILLED_DRAGUN,
                }}.ToArray(),
                CanBeForcedSecret = false,
                RandomNodeChildMinDistanceFromEntrance = 0,
                exactSecondaryRoom = null,
                framedCombatNodes = 0,
            });
        }
        public static GameObject mapIcon;
        public RoomHandler m_room;
        private Transform talkpoint;
        public static List<Chancellot> allChancelots = new List<Chancellot>();
        private void Start()
        {
            talkpoint = base.transform.Find("talkpoint");
            SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black);
            this.m_room = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Round));
            this.m_room.RegisterInteractable(this);

            Minimap.Instance.RegisterRoomIcon(m_room, mapIcon, false);
            m_room.Entered += PlayerEnteredRoom;
            allChancelots.Add(this);
        }
        private void PlayerEnteredRoom(PlayerController player)
        {
            if (player.IsStealthed || !SaveAPIManager.GetFlag(CustomDungeonFlags.CHANCELOT_METONCE)) { return; }
            else
            {
                base.StartCoroutine(Conversation(BraveUtility.RandomElement(entryStrings), player));
            }
        }
        public bool angry = false;
        public void Inform(string information, int moneySpent = 0)
        {
            switch (information)
            {
                case "loss":
                    base.StartCoroutine(Conversation(BraveUtility.RandomElement(onSlotLoss), GameManager.Instance.PrimaryPlayer, angry ? "miffed" : "laugh"));
                    break;
                case "minorwin":
                    base.StartCoroutine(Conversation(BraveUtility.RandomElement(onSlotWin), GameManager.Instance.PrimaryPlayer, "miffed"));
                    break;
                case "bigwin":
                    base.StartCoroutine(Conversation(BraveUtility.RandomElement(onSlotBigWin), GameManager.Instance.PrimaryPlayer, "angry"));
                    break;
                case "dispenserfail":
                    base.StartCoroutine(Conversation(BraveUtility.RandomElement(angry ? onDispenseFailAngry : onDispenseFail), GameManager.Instance.PrimaryPlayer, angry ? "miffed" : "laugh"));
                    break;
                case "dispenserbuy":
                    base.StartCoroutine(Conversation(BraveUtility.RandomElement(angry ? onDispenseAngry : onDispense), GameManager.Instance.PrimaryPlayer, angry ? "miffed" : null));
                    break;
                case "break":
                    base.StartCoroutine(Conversation(BraveUtility.RandomElement(breakSlotMachine), GameManager.Instance.PrimaryPlayer, "angry"));
                    angry = true;
                    break;
            }
            if (moneySpent > 0) { SaveAPIManager.RegisterStatChange(CustomTrackedStats.CHANCELOT_MONEY_SPENT, moneySpent); }
        }
        private void Update()
        {
            PlayerController closestPlayer = GameManager.Instance.GetActivePlayerClosestToPoint(base.transform.position.XY(), false);
            if (closestPlayer != null)
            {
                base.sprite.FlipX = closestPlayer.CenterPosition.x > base.transform.position.x;
            }
        }

        public IEnumerator Conversation(string dialogue, PlayerController speaker, string animation = "talk")
        {
            base.spriteAnimator.PlayForDuration($"chancelot_{animation}", 2f, "chancelot_idle", false);
            TextBoxManager.ShowTextBox(talkpoint.position, talkpoint, 2f, dialogue, "gambler", false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);
            yield break;
        }
        public IEnumerator LongConversation(List<string> dialogue, PlayerController speaker, bool clearAfter = false, string animation = "talk")
        {
            speaker.SetInputOverride("npcConversation");
            Pixelator.Instance.LerpToLetterbox(0.35f, 0.25f);
            CameraController mainCameraController = GameManager.Instance.MainCameraController;
            mainCameraController.SetManualControl(true, true);
            mainCameraController.OverridePosition = base.transform.position;

            int conversationIndex = 0;
            base.spriteAnimator.Play($"chancelot_{animation}");
            while (conversationIndex <= dialogue.Count - 1)
            {
                TextBoxManager.ClearTextBox(talkpoint);
                TextBoxManager.ShowTextBox(talkpoint.position, talkpoint, -1f, dialogue[conversationIndex], "gambler", instant: false, showContinueText: true);
                float timer = 0;
                while (!BraveInput.GetInstanceForPlayer(speaker.PlayerIDX).ActiveActions.GetActionFromType(GungeonActions.GungeonActionType.Interact).WasPressed || timer < 0.4f)
                {
                    timer += BraveTime.DeltaTime;
                    yield return null;
                }
                conversationIndex++;
            }
            if (clearAfter) { TextBoxManager.ClearTextBox(talkpoint); }
            base.spriteAnimator.Play("chancelot_idle");

            speaker.ClearInputOverride("npcConversation");
            Pixelator.Instance.LerpToLetterbox(1, 0.25f);
            GameManager.Instance.MainCameraController.SetManualControl(false, true);

            yield break;
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
            if (!TextBoxManager.HasTextBox(talkpoint))
            {
                base.StartCoroutine(HandleInteract(interactor));
            }
        }

        public IEnumerator HandleInteract(PlayerController interactor)
        {
            if (!SaveAPIManager.GetFlag(CustomDungeonFlags.CHANCELOT_METONCE))
            {
                yield return LongConversation(new List<string>() {
                                    "Ahh. Another bullet-riddled fool stumbles into my establishment.",
                                    "Tell me, did you save the [sprite \"ui_coin\"]?",
                                    "You'll need them.",
                                    "For here, I...",
                                    "SER CHANCELOT",
                                    "...will test you in games of great chance!",
                                    "Or, rather, that machine in the corner will... self service and all that.",
                                    "It matters not. Bother me no further, Staker.",
                                    "...you are of no rank to speak with a knight..."

                            }, interactor, true);
                SaveAPIManager.SetFlag(CustomDungeonFlags.CHANCELOT_METONCE, true);
            }
            else
            {
                if (timesspoken < 2)
                {
                    List<string> interactions = new List<string>(talkStrings);
                    if (SaveAPIManager.GetFlag(CustomDungeonFlags.ALLJAMMEDMODE_ENABLED_GENUINE)) { interactions.AddRange(allJammedInteractions); }
                    if (firstInteract != null) { interactions.Remove(firstInteract); }

                    string chosen = BraveUtility.RandomElement(angry ? talkAngry : talkStrings);
                    yield return Conversation(chosen, interactor, angry ? "miffed" : "talk");
                    firstInteract = chosen;
                }
                else
                {
                    yield return Conversation(BraveUtility.RandomElement(angry ? talkAngry : boredStrings), interactor, angry ? "miffed" : "talk");
                }
                timesspoken++;
            }
            yield break;
        }
        int timesspoken = 0;
        string firstInteract = null;
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
            allChancelots.Remove(this);
            base.OnDestroy();
        }

        //Regular Dialogue
        public static List<string> allJammedInteractions = new List<string>()
        {
            "Something about you... the shadows cling to you in droves...",
            "You seem... familiar.",
            "That magic about you... surely not...",
            "Leave this place, cursed one.",
            "I deal not with sorcery of the kind you wear. That is long ago.",
            "So... the old monster still draws breath..."
        };
        public static List<string> entryStrings = new List<string>()
        {
            "I smell something...",
            "Ah, you... survived.",
            "Another lowborn...",
            "Mhm. 'Welcome'",
            "Please, spend some [sprite \"ui_coin\"]"
        };
        public static List<string> talkStrings = new List<string>()
        {
            "Such a strange little thing you are. Most stupefying that you managed to survive the Gungeon at all...",
            "Does this one fancy itself a gambler? Do they have the stomach to try?",
            "If you see Winchester, tell him he's no longer allowed in my establishment. He knows why.",
            "That Bello fellow has neither a sense of style or business.",
            "If you desire to make a purchase, peruse the Dispensers over there. I don't keep wares at the counter.",
            "The Dispensers may be drab, but they keep the sticky fingers of Gungeoneers like you at bay.",
            "...those fools at the Table...   ...one day they will rue their hubris.",
            "It matters not what those at the Table say! I am a knight, and will be respected as such!",
        };
        public static List<string> boredStrings = new List<string>()
        {
            "If you must bother me, at least spend some [sprite \"ui_coin\"]",
            "Tarry not.",
            "Shuffle off, Lowborn",
            "You irritate me."
        };
        public static List<string> onDispense = new List<string>()
        {
            "...much obliged.",
            "Very well.",
            "Your 'donation' is appreciated."
        };
        public static List<string> onDispenseFail = new List<string>()
        {
            "You need more [sprite \"ui_coin\"], taffer.",
            "Scoundrel.",
            "You can't haggle with the Dispensers, they know not fear or reason.",
            "[sprite \"ui_coin\"] only."
        };
        public static List<string> onSlotWin = new List<string>()
        {
            "...I suppose everyone gets one...",
            "Did you bring a lucky charm?",
            "No matter.",
            "...lucky pull.",
            "...good- for you..."
        };
        public static List<string> onSlotLoss = new List<string>()
        {
            "Why not try again? Surely you could win it all back...",
            "Thank you for the [sprite \"ui_coin\"]",
            "There's one born every minute...",
            "Lady luck smiles on me this day.",
            "Lost it.",
            "Shame.",
            "So sad.",
            "The house always wins."
        };
        public static List<string> onSlotBigWin = new List<string>()
        {
            "How!",
            "Preposterous!",
            "Taffer!",
            "You must surely be cheating!"
        };

        //Angry Dialogue
        public static List<string> breakSlotMachine = new List<string>()
        {
            "You BROKE it!?",
            "Sweet Mercy!",
            "Bastard!"
        };
        public static List<string> onDispenseAngry = new List<string>()
        {
            "Purchase what you need, and GET OUT!",
            "I have my eye on you, taffer!",
            "If I could raise the prices on the Dispensers, I would."
        };
        public static List<string> onDispenseFailAngry = new List<string>()
        {
            "Trying to rob me now, lowborn?",
            "Scoundrel.",
            "Get Out!",
            "Parasite."
        };
        public static List<string> talkAngry = new List<string>()
        {
            "Get out.",
            "You aren't welcome here.",
            "...thief and a scoundrel...",
            "...no respect for knights...",
            "You are lucky I do not cut thee down where thee stand."
        };
    }
}


