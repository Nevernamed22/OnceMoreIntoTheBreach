using Alexandria.BreakableAPI;
using Alexandria.ChestAPI;
using Alexandria.DungeonAPI;
using Alexandria.ItemAPI;
using Alexandria.Misc;
using Brave.BulletScript;
using Dungeonator;
using HarmonyLib;
using HutongGames.PlayMaker.Actions;
using SaveAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using static NevernamedsItems.BowlerShop;
using static UnityEngine.UI.GridLayoutGroup;

namespace NevernamedsItems
{
    public class FortuneTeller : BraveBehaviour
    {
        public static void Init()
        {
            GameObject fortuneTeller = new GameObject("fortuneTeller");
            fortuneTeller.SetActive(false);
            fortuneTeller.AddComponent<FortuneTeller>();
            FakePrefab.MarkAsFakePrefab(fortuneTeller);

            #region Tent
            var tent = ItemBuilder.SpriteFromBundle("fortuneteller_tent", Initialisation.NPCCollection.GetSpriteIdByName("fortuneteller_tent"), Initialisation.NPCCollection, new GameObject("Tent"));
            SpeculativeRigidbody tentRigidBody = tent.GetOrAddComponent<SpeculativeRigidbody>();
            tentRigidBody.CollideWithOthers = true;
            tentRigidBody.CollideWithTileMap = false;
            tentRigidBody.Velocity = Vector2.zero;
            tentRigidBody.MaxVelocity = Vector2.zero;
            tentRigidBody.ForceAlwaysUpdate = false;
            tentRigidBody.CanPush = false;
            tentRigidBody.CanBePushed = false;
            tentRigidBody.PushSpeedModifier = 1f;
            tentRigidBody.CanCarry = false;
            tentRigidBody.CanBeCarried = false;
            tentRigidBody.PreventPiercing = false;
            tentRigidBody.SkipEmptyColliders = false;
            tentRigidBody.RecheckTriggers = false;
            tentRigidBody.UpdateCollidersOnRotation = false;
            tentRigidBody.UpdateCollidersOnScale = false;
            tentRigidBody.PixelColliders = new List<PixelCollider>()
            {
                new PixelCollider()
                {
                    CollisionLayer =  CollisionLayer.BulletBlocker,
                },
                 new PixelCollider()
                {
                    CollisionLayer =  CollisionLayer.HighObstacle,
                }
            };
            tentRigidBody.GetComponent<MeshRenderer>().lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            tentRigidBody.GetComponent<tk2dSprite>().renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTiltedCutout");
            tentRigidBody.GetComponent<tk2dSprite>().usesOverrideMaterial = true;
            tentRigidBody.GetComponent<tk2dSprite>().HeightOffGround = -1f;
            tentRigidBody.transform.SetParent(fortuneTeller.transform);
            #endregion

            #region Table
            var counter = ItemBuilder.SpriteFromBundle("fortuneteller_table", Initialisation.NPCCollection.GetSpriteIdByName("fortuneteller_table"), Initialisation.NPCCollection, new GameObject("Table"));
            counter.transform.SetParent(fortuneTeller.transform);
            counter.transform.localPosition = new Vector3(30f / 16f, -12f / 16f);
            counter.GetComponent<tk2dSprite>().HeightOffGround = -1f;
            var counterBody = counter.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(new IntVector2(0, 0), new IntVector2(58, 36));
            counterBody.CollideWithTileMap = false;
            counterBody.CollideWithOthers = true;
            counter.GetComponent<MeshRenderer>().material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTiltedCutout");
            counter.GetComponent<tk2dSprite>().usesOverrideMaterial = true;
            counterBody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.LowObstacle;

            var tableoutline = ItemBuilder.SpriteFromBundle("fortuneteller_table_outline", Initialisation.NPCCollection.GetSpriteIdByName("fortuneteller_table_outline"), Initialisation.NPCCollection, new GameObject("TableOutline"));
            tableoutline.transform.SetParent(fortuneTeller.transform);
            tableoutline.transform.localPosition = new Vector3(29f / 16f, -13f / 16f);
            tableoutline.GetComponent<tk2dSprite>().HeightOffGround = -1f;
            tableoutline.GetComponent<tk2dSprite>().renderer.material.shader = ShaderCache.Acquire("tk2d/CutoutVertexColorTilted");
            tableoutline.GetComponent<tk2dSprite>().usesOverrideMaterial = true;

            tableoutline.SetActive(false);
            #endregion

            #region CardPoints
            GameObject cardPoint1 = new GameObject("CardPoint_1");
            cardPoint1.transform.SetParent(fortuneTeller.transform);
            cardPoint1.transform.localPosition = new Vector3((35f + 7f) / 16f, -1f / 16f);

            GameObject cardPoint2 = new GameObject("CardPoint_2");
            cardPoint2.transform.SetParent(fortuneTeller.transform);
            cardPoint2.transform.localPosition = new Vector3((52f + 7f) / 16f, -1f / 16f);

            GameObject cardPoint3 = new GameObject("CardPoint_3");
            cardPoint3.transform.SetParent(fortuneTeller.transform);
            cardPoint3.transform.localPosition = new Vector3((68f + 8f) / 16f, -1f / 16f);
            #endregion

            #region Shadow
            var shadow = ItemBuilder.SpriteFromBundle("fortuneteller_shadow", Initialisation.NPCCollection.GetSpriteIdByName("fortuneteller_shadow"), Initialisation.NPCCollection, new GameObject("Shadow"));
            tk2dSprite shadowSprite = shadow.GetComponent<tk2dSprite>();
            shadowSprite.HeightOffGround = -1.7f;
            shadowSprite.SortingOrder = 0;
            shadowSprite.IsPerpendicular = false;
            shadowSprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTilted");
            shadowSprite.usesOverrideMaterial = true;
            shadowSprite.transform.SetParent(fortuneTeller.transform);
            shadowSprite.transform.localPosition = new Vector3(-7f / 16f, -14f / 16f);
            #endregion

            #region InTentPolish
            var backdrop = ItemBuilder.SpriteFromBundle("fortuneteller_backdrop", Initialisation.NPCCollection.GetSpriteIdByName("fortuneteller_backdrop"), Initialisation.NPCCollection, new GameObject("Backdrop"));
            tk2dSprite backdropSprite = backdrop.GetComponent<tk2dSprite>();
            backdropSprite.HeightOffGround = -1.1f;
            backdropSprite.SortingOrder = 0;
            backdropSprite.IsPerpendicular = true;
            backdropSprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTiltedCutout");
            backdropSprite.usesOverrideMaterial = true;
            backdropSprite.transform.SetParent(fortuneTeller.transform);
            backdropSprite.transform.localPosition = new Vector3(18f / 16f, 18f / 16f);
            #endregion

            #region Teller
            var tellerArm = ItemBuilder.SpriteFromBundle("fortuneteller_arm_idle_001", Initialisation.NPCCollection.GetSpriteIdByName("fortuneteller_arm_idle_001"), Initialisation.NPCCollection, new GameObject("Teller_Arm"));
            tellerArm.transform.SetParent(fortuneTeller.transform);
            tellerArm.transform.localPosition = new Vector3(59f / 16f, 11f / 16f);
            tk2dSpriteAnimator tellerArmAnimator = tellerArm.GetOrAddComponent<tk2dSpriteAnimator>();
            tellerArmAnimator.sprite.HeightOffGround = 3f;
            tellerArmAnimator.Library = Initialisation.npcAnimationCollection;
            tellerArmAnimator.defaultClipId = Initialisation.npcAnimationCollection.GetClipIdByName("fortuneteller_arm_ascend");
            tellerArmAnimator.DefaultClipId = Initialisation.npcAnimationCollection.GetClipIdByName("fortuneteller_arm_ascend");
            tellerArmAnimator.playAutomatically = true;

            var tellerEyes = ItemBuilder.SpriteFromBundle("fortuneteller_eyes_gone_001", Initialisation.NPCCollection.GetSpriteIdByName("fortuneteller_eyes_gone_001"), Initialisation.NPCCollection, new GameObject("Teller_Eyes"));
            tellerEyes.transform.SetParent(fortuneTeller.transform);
            tellerEyes.transform.localPosition = new Vector3(41f / 16f, 26f / 16f);
            tk2dSpriteAnimator tellerEyesAnimator = tellerEyes.GetOrAddComponent<tk2dSpriteAnimator>();
            tellerEyesAnimator.sprite.HeightOffGround = 1f;
            tellerEyesAnimator.Library = Initialisation.npcAnimationCollection;
            tellerEyesAnimator.defaultClipId = Initialisation.npcAnimationCollection.GetClipIdByName("fortuneteller_eyes_gone");
            tellerEyesAnimator.DefaultClipId = Initialisation.npcAnimationCollection.GetClipIdByName("fortuneteller_eyes_gone");
            tellerEyesAnimator.playAutomatically = true;
            #endregion

            #region InteractPoint
            Transform transform = new GameObject("interactPoint").transform;
            transform.SetParent(fortuneTeller.transform);
            transform.transform.localPosition = new Vector3(59f / 16f, -12f / 16f);
            transform.gameObject.AddComponent<FortuneTellerInteractPointController>();

            Transform talkpoint = new GameObject("talkpoint").transform;
            talkpoint.SetParent(fortuneTeller.transform);
            talkpoint.transform.localPosition = new Vector3(60f / 16f, 46f / 16f);
            #endregion

            Dictionary<GameObject, float> dict = new Dictionary<GameObject, float>() { { fortuneTeller, 1f } };
            DungeonPlaceable placeable = BreakableAPIToolbox.GenerateDungeonPlaceable(dict);
            StaticReferences.StoredDungeonPlaceables.Add("omitb_fortune_teller", placeable);
            Alexandria.DungeonAPI.StaticReferences.customPlaceables.Add("nn:omitb_fortune_teller", placeable);

            mapIcon = ItemBuilder.SpriteFromBundle("fortuneteller_mapicon", Initialisation.NPCCollection.GetSpriteIdByName("fortuneteller_mapicon"), Initialisation.NPCCollection, new GameObject("fortuneteller_mapicon"));
            mapIcon.MakeFakePrefab();

            GameObject card = ItemBuilder.SpriteFromBundle("tarotcard_flipped_001", Initialisation.NPCCollection.GetSpriteIdByName("tarotcard_flipped_001"), Initialisation.NPCCollection, new GameObject("Tarot Card"));
            card.MakeFakePrefab();
            tk2dSpriteAnimator cardAnimator = card.GetOrAddComponent<tk2dSpriteAnimator>();
            cardAnimator.sprite.HeightOffGround = 1f;
            cardAnimator.Library = Initialisation.npcAnimationCollection;
            cardAnimator.defaultClipId = Initialisation.npcAnimationCollection.GetClipIdByName("tarotcard_flipped");
            cardAnimator.DefaultClipId = Initialisation.npcAnimationCollection.GetClipIdByName("tarotcard_flipped");
            cardAnimator.playAutomatically = false;
            card.gameObject.AddComponent<TarotCardController>();
            Material mat = new Material(EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material);
            mat.mainTexture = cardAnimator.sprite.renderer.material.mainTexture;
            mat.SetColor("_EmissiveColor", Color.white); //RGB value of the color you want glowing
            mat.SetFloat("_EmissiveColorPower", 5); // no idea tbh
            mat.SetFloat("_EmissivePower", 20); //brightness
            cardAnimator.sprite.renderer.material = mat;
            cardPrefab = card;

            cardRevealVFX = VFXToolbox.CreateVFXBundle("tarotcard_revealvfx", true, 10f, 20, 5, Color.white, false, Initialisation.NPCCollection, Initialisation.npcAnimationCollection);
            cardRevealVFX.GetComponent<tk2dBaseSprite>().IsPerpendicular = true;


        }
        public static List<FortuneTeller> allFortuneTellers = new List<FortuneTeller>();

        //Prefabs & Static Data
        public static GameObject mapIcon;
        public static GameObject cardPrefab;
        public static GameObject cardRevealVFX;
        public enum State
        {
            WAITING,
            MID_FORTUNE
        }

        //Instance Object Data
        public RoomHandler m_room;
        public Transform talkpoint;
        private Transform cardpoint_1;
        private Transform cardpoint_2;
        private Transform cardpoint_3;
        private FortuneTellerInteractPointController interactPoint;
        public tk2dSpriteAnimator armAnimator;
        public tk2dSpriteAnimator eyesAnimator;
        public GameObject tableOutline;

        //Flags
        public bool armOut = false; //Arm is extended
        public bool eyesVisible = false; //Eyes are not in 'gone' state
        public bool armTransitioning = false; //Arm is in the middle of transitioning between in and out
        public bool busy = false; //Teller is going something
        private bool cachedHasWhiteOutline = false; //Table has white outline
        public int numFortunesGiven = 0; //Number of fortunes given by this teller
        public bool hasDeclinedOnce = false; //Has declined a fortune from this teller once
        public State state = State.WAITING; //Current teller state
        public List<TarotCardController.TarotCards> AlreadyExplainedCards = new List<TarotCardController.TarotCards>();

        //Card-to-teller Communication
        public TarotCardController queuedFlippedCard = null;
        public TarotCardController finalChosenCard = null;

        //Debug
        public static bool FortuneTellerDebugging = true; //Log events to console
        private void Start()
        {
            talkpoint = base.transform.Find("talkpoint");
            cardpoint_1 = base.transform.Find("CardPoint_1");
            cardpoint_2 = base.transform.Find("CardPoint_2");
            cardpoint_3 = base.transform.Find("CardPoint_3");
            tableOutline = base.transform.Find("TableOutline").gameObject;
            this.m_room = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Round));

            Minimap.Instance.RegisterRoomIcon(m_room, mapIcon, false);
            allFortuneTellers.Add(this);

            armAnimator = base.transform.Find("Teller_Arm").GetComponent<tk2dSpriteAnimator>();
            eyesAnimator = base.transform.Find("Teller_Eyes").GetComponent<tk2dSpriteAnimator>();
            //base.StartCoroutine(TransitionToArmOutState(false));

            interactPoint = base.transform.Find("interactPoint").GetComponent<FortuneTellerInteractPointController>();
            interactPoint.OnConfigureWithTeller(this);
            armTransitioning = false;
        }
        public void SetArmOutlined(bool outline)
        {

            tableOutline.SetActive(outline);
            cachedHasWhiteOutline = outline;
        }
        private void Update()
        {
            bool playerIsClose = (GameManager.Instance.PrimaryPlayer != null && Vector2.Distance(interactPoint.transform.position, GameManager.Instance.PrimaryPlayer.transform.position) <= 2f) || (GameManager.Instance.SecondaryPlayer != null && Vector2.Distance(interactPoint.transform.position, GameManager.Instance.SecondaryPlayer.transform.position) <= 2f);
            if (armTransitioning && cachedHasWhiteOutline) { SetArmOutlined(false); }
            if (armOut && !armTransitioning)
            {
                if (playerIsClose && !cachedHasWhiteOutline) { SetArmOutlined(true); }
                else if (!playerIsClose && cachedHasWhiteOutline) { SetArmOutlined(false); }
            }
            if (!armTransitioning && !busy && state != State.MID_FORTUNE)
            {
                if (playerIsClose && !armOut)
                {
                    base.StartCoroutine(TransitionToArmOutState(true));
                    eyesAnimator.PlayUntilFinished("fortuneteller_eyes_appear", "fortuneteller_eyes_idle");
                    AkSoundEngine.PostEvent("Play_ENM_beholster_teleport_02", eyesAnimator.gameObject);
                }
                else if (!playerIsClose && armOut)
                {
                    base.StartCoroutine(TransitionToArmOutState(false));
                    eyesAnimator.PlayUntilFinished("fortuneteller_eyes_vanish", "fortuneteller_eyes_gone");
                    AkSoundEngine.PostEvent("Play_ENM_beholster_teleport_01", eyesAnimator.gameObject);
                }
            }
        }
        public IEnumerator TransitionToArmOutState(bool armout)
        {
            if (armTransitioning) { yield break; }
            armTransitioning = true;

            if (armout)
            {
                armAnimator.PlayUntilFinished("fortuneteller_arm_descend", "fortuneteller_arm_idle");
                armOut = true;
            }
            else
            {
                armAnimator.Play("fortuneteller_arm_ascend");
                armOut = false;
            }
            yield return new WaitForSeconds(1f);

            armTransitioning = false;
            yield break;
        }
        public int FortuneCost(PlayerController inst)
        {
            float multiplier = GameManager.Instance.PrimaryPlayer.stats.GetStatValue(PlayerStats.StatType.GlobalPriceMultiplier);
            if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && GameManager.Instance.SecondaryPlayer)
            {
                multiplier *= GameManager.Instance.SecondaryPlayer.stats.GetStatValue(PlayerStats.StatType.GlobalPriceMultiplier);
            }
            GameLevelDefinition lastLoadedLevelDefinition = GameManager.Instance.GetLastLoadedLevelDefinition();
            multiplier *= (lastLoadedLevelDefinition == null) ? 1f : lastLoadedLevelDefinition.priceMultiplier;
            if (SaveAPIManager.GetPlayerStatValue(CustomTrackedStats.TOTAL_FORTUNES_RECIEVED) == 0) { return 0; }
            else
            {
                int finalCost = Mathf.CeilToInt((numFortunesGiven == 0 ? 40f : 80f) * multiplier);
                return Math.Max(finalCost, 0);
            }
        }
        public IEnumerator HandleInteract(PlayerController interactor)
        {
            busy = true;
            if (numFortunesGiven == 2)
            {
                yield return Conversation(BraveUtility.RandomElement(NoMoreFortunesDialogue), interactor);
            }
            else if (!interactor.IsPrimaryPlayer)
            {
                yield return Conversation(BraveUtility.RandomElement(CultistRebuff), interactor);
            }
            else
            {
                busy = true;
                interactor.SetInputOverride("npcConversation");
                Pixelator.Instance.LerpToLetterbox(0.35f, 0.25f);
                CameraController mainCameraController = GameManager.Instance.MainCameraController;
                mainCameraController.SetManualControl(true, true);
                mainCameraController.OverridePosition = eyesAnimator.sprite.WorldCenter;

                eyesAnimator.Play($"fortuneteller_eyes_talk");
                if (armOut) armAnimator.Play($"fortuneteller_arm_talk");

                List<string> introDialogue = new List<string>() { };
                if (!SaveAPIManager.GetFlag(CustomDungeonFlags.FORTUNETELLER_METONCE))
                {
                    introDialogue = FirstInteractSaveFile;
                    SaveAPIManager.SetFlag(CustomDungeonFlags.FORTUNETELLER_METONCE, true);
                }
                else if (hasDeclinedOnce)
                {
                    introDialogue = BraveUtility.RandomElement(PostDeclineFortuneOffer);
                }
                else { introDialogue = numFortunesGiven == 0 ? BraveUtility.RandomElement(GenericFortuneOffer) : BraveUtility.RandomElement(SecondFortuneOffer); }

                yield return IterateLines(introDialogue, interactor, false);

                GameUIRoot.Instance.DisplayPlayerConversationOptions(interactor, null, hasDeclinedOnce ? $"Alright, tell me <Pay {FortuneCost(interactor)}[sprite \"ui_coin\"]>" : $"Tell my Fortune <Pay {FortuneCost(interactor)}[sprite \"ui_coin\"]>", hasDeclinedOnce ? "Still don't care" : "I only care about my Past");
                int response2 = -1;
                while (!GameUIRoot.Instance.GetPlayerConversationResponse(out response2))
                    yield return null;

                if (response2 == 0)
                {
                    int cost = FortuneCost(interactor);
                    if (interactor.carriedConsumables.Currency >= cost)
                    {
                        interactor.carriedConsumables.Currency -= cost;
                        yield return IterateLines(new List<string>() {
                         "Then let us see what the cards hold for you.",
                         "But baxt tuke..."
                         }, interactor, false);
                        yield return FortuneSequence(interactor);
                    }
                    else
                    {
                        hasDeclinedOnce = true;
                        yield return IterateLines(new List<string>() {
                         "Alas, your purse hangs light at your hip...",
                         "...even a mystic must eat..."
                         }, interactor, true);
                        eyesAnimator.Play("fortuneteller_eyes_idle");
                        if (armOut) armAnimator.Play($"fortuneteller_arm_idle");
                        TextBoxManager.ClearTextBox(talkpoint);
                    }
                }
                else
                {
                    hasDeclinedOnce = true;
                    yield return IterateLines(new List<string>() {
                         "Very well...",
                         "Perhaps in future, we may consult the cards together."
                         }, interactor, true);
                    eyesAnimator.Play("fortuneteller_eyes_idle");
                    if (armOut) armAnimator.Play($"fortuneteller_arm_idle");
                    TextBoxManager.ClearTextBox(talkpoint);
                }
            }
            busy = false;
            // SaveAPIManager.SetFlag(CustomDungeonFlags.FORTUNETELLER_METONCE, true);
        }
        public GameObject SpawnCard(Transform cardPoint, TarotCardController.TarotCardData cardData)
        {
            if (FortuneTellerDebugging)
            {
                ETGModConsole.Log($"<color=#bb29e3>Fortune Teller</color> | Spawning Card: <color=#f5cd56>{cardData.tarotCard}</color>");
                ETGModConsole.Log($"    Spawn Transform: <color=#f5cd56>{cardPoint.gameObject.name}</color>");
                ETGModConsole.Log($"    Spawn Position: <color=#f5cd56>{cardPoint.position}</color>");
            }

            GameObject spawnedCard = UnityEngine.GameObject.Instantiate<GameObject>(cardPrefab, cardPoint.position, Quaternion.identity);
            if (spawnedCard.GetComponent<TarotCardController>())
            {
                if (FortuneTellerDebugging) ETGModConsole.Log($"    Card passed TarotCardController Check");
                spawnedCard.GetComponent<TarotCardController>().cardData = cardData;
                spawnedCard.GetComponent<TarotCardController>().OnConfigureWithTeller(this);
            }
            //spawnedCard.SetActive(false);
            return spawnedCard;
        }
        public IEnumerator FortuneSequence(PlayerController speaker)
        {
            if (FortuneTellerDebugging) ETGModConsole.Log($"<color=#bb29e3>Fortune Teller</color> | Beginning Fortune Sequence");
            TextBoxManager.ClearTextBox(talkpoint);
            eyesAnimator.Play($"fortuneteller_eyes_magic");
            if (armOut) { yield return TransitionToArmOutState(false); }
            AkSoundEngine.PostEvent("Play_ENM_wizardred_conjure_01", eyesAnimator.gameObject);

            List<TarotCardController.TarotCardData> cardDatas = new List<TarotCardController.TarotCardData>();
            cardDatas.AddRange(TarotCards.fortuneTellerTarotCards);

            if (FortuneTellerDebugging)
            {
                ETGModConsole.Log($"<color=#bb29e3>Fortune Teller</color> | Card Positions");
                ETGModConsole.Log($"    1: <color=#f5cd56>{cardpoint_1.position}</color>");
                ETGModConsole.Log($"    2: <color=#f5cd56>{cardpoint_2.position}</color>");
                ETGModConsole.Log($"    3: <color=#f5cd56>{cardpoint_3.position}</color>");
            }

            TarotCardController.TarotCardData chosenData = BraveUtility.RandomElement(cardDatas);
            TarotCardController card1 = SpawnCard(cardpoint_1, chosenData).GetComponent<TarotCardController>();

            card1.spriteAnimator.PlayUntilFinished("tarotcard_spawn", "tarotcard_flipped", true);
            cardDatas.Remove(chosenData);

            yield return new WaitForSeconds(0.5f);

            TarotCardController.TarotCardData chosenData2 = BraveUtility.RandomElement(cardDatas);
            TarotCardController card2 = SpawnCard(cardpoint_2, chosenData2).GetComponent<TarotCardController>();

            card2.spriteAnimator.PlayUntilFinished("tarotcard_spawn", "tarotcard_flipped", true);
            cardDatas.Remove(chosenData2);

            yield return new WaitForSeconds(0.5f);

            TarotCardController.TarotCardData chosenData3 = BraveUtility.RandomElement(cardDatas);
            TarotCardController card3 = SpawnCard(cardpoint_3, chosenData3).GetComponent<TarotCardController>();

            card3.spriteAnimator.PlayUntilFinished("tarotcard_spawn", "tarotcard_flipped", true);
            cardDatas.Remove(chosenData3);

            yield return new WaitForSeconds(1f);
            if (FortuneTellerDebugging) ETGModConsole.Log($"<color=#bb29e3>Fortune Teller</color> | Completed all Card Spawns");

            m_room.DeregisterInteractable(interactPoint);

            speaker.ClearInputOverride("npcConversation");
            Pixelator.Instance.LerpToLetterbox(1, 0.25f);
            GameManager.Instance.MainCameraController.SetManualControl(false, true);
            if (FortuneTellerDebugging) ETGModConsole.Log($"<color=#bb29e3>Fortune Teller</color> | Cleared Conversation Overrides");

            CameraController mainCameraController = GameManager.Instance.MainCameraController;

            yield return Conversation("Now... turn the cards and unmask fate.", speaker, "talk", false);
            if (FortuneTellerDebugging) ETGModConsole.Log($"<color=#bb29e3>Fortune Teller</color> | Begin Wait Period");

            state = State.MID_FORTUNE;
            while (!card1.isFlipped || !card2.isFlipped || !card3.isFlipped)
            {
                if (queuedFlippedCard != null && queuedFlippedCard.cardData.flipDialogue.Count > 0)
                {
                    if (FortuneTellerDebugging)
                    {
                        ETGModConsole.Log($"<color=#bb29e3>Fortune Teller</color> | Beginning flip dialogue:");
                        foreach (string dia in queuedFlippedCard.cardData.flipDialogue)
                        {
                            ETGModConsole.Log($"    <color=#f5cd56>\"{dia}\"</color>");
                        }
                    }
                    speaker.SetInputOverride("npcConversation");
                    Pixelator.Instance.LerpToLetterbox(0.35f, 0.25f);
                    mainCameraController.SetManualControl(true, true);
                    mainCameraController.OverridePosition = eyesAnimator.sprite.WorldCenter;

                    string animation = queuedFlippedCard.cardData.tellerFaceAnimation != null ? queuedFlippedCard.cardData.tellerFaceAnimation : "talk";
                    eyesAnimator.Play($"fortuneteller_eyes_{animation}");
                    yield return IterateLines(AlreadyExplainedCards.Contains(queuedFlippedCard.cardData.tarotCard) ? new List<string>() { $"{queuedFlippedCard.cardData.name}..." } : queuedFlippedCard.cardData.flipDialogue, speaker);
                    eyesAnimator.Play($"fortuneteller_eyes_idle");
                    AlreadyExplainedCards.Add(queuedFlippedCard.cardData.tarotCard);
                    speaker.ClearInputOverride("npcConversation");
                    Pixelator.Instance.LerpToLetterbox(1, 0.25f);
                    GameManager.Instance.MainCameraController.SetManualControl(false, true);
                    queuedFlippedCard = null;
                    TextBoxManager.ClearTextBox(talkpoint);
                    if (FortuneTellerDebugging) ETGModConsole.Log($"<color=#bb29e3>Fortune Teller</color> | Ending flip dialogue");

                }
                yield return null;
            }

            speaker.SetInputOverride("npcConversation");
            Pixelator.Instance.LerpToLetterbox(0.35f, 0.25f);
            mainCameraController = GameManager.Instance.MainCameraController;
            mainCameraController.SetManualControl(true, true);
            mainCameraController.OverridePosition = eyesAnimator.sprite.WorldCenter;
            yield return IterateLines(BraveUtility.RandomElement(numFortunesGiven == 0 ? PickACardDialogue : PickACardAgainDialogue), speaker);
            speaker.ClearInputOverride("npcConversation");
            Pixelator.Instance.LerpToLetterbox(1, 0.25f);
            GameManager.Instance.MainCameraController.SetManualControl(false, true);
            eyesAnimator.Play($"fortuneteller_eyes_idle");
            queuedFlippedCard = null;
            TextBoxManager.ClearTextBox(talkpoint);

            card1.canBeInteracted = true;
            card1.selectionPhase = true;
            m_room.RegisterInteractable(card1);
            card2.canBeInteracted = true;
            card2.selectionPhase = true;
            m_room.RegisterInteractable(card2);
            card3.canBeInteracted = true;
            card3.selectionPhase = true;
            m_room.RegisterInteractable(card3);

            while (finalChosenCard == null)
            {
                yield return null;
            }

            speaker.SetInputOverride("npcConversation");
            Pixelator.Instance.LerpToLetterbox(0.35f, 0.25f);
            mainCameraController = GameManager.Instance.MainCameraController;
            mainCameraController.SetManualControl(true, true);
            mainCameraController.OverridePosition = eyesAnimator.sprite.WorldCenter;

            card1.canBeInteracted = true;
            m_room.DeregisterInteractable(card1);
            card2.canBeInteracted = true;
            m_room.DeregisterInteractable(card2);
            card3.canBeInteracted = true;
            m_room.DeregisterInteractable(card3);

            if (card1 != finalChosenCard)
            {
                SpawnManager.SpawnVFX(FortuneTeller.cardRevealVFX, card1.transform.position, Quaternion.identity);
                UnityEngine.Object.Destroy(card1.gameObject);
            }
            if (card2 != finalChosenCard)
            {
                SpawnManager.SpawnVFX(FortuneTeller.cardRevealVFX, card2.transform.position, Quaternion.identity);
                UnityEngine.Object.Destroy(card2.gameObject);
            }
            if (card3 != finalChosenCard)
            {
                SpawnManager.SpawnVFX(FortuneTeller.cardRevealVFX, card3.transform.position, Quaternion.identity);
                UnityEngine.Object.Destroy(card3.gameObject);
            }

            yield return MiscTools.LerpTransformToPosition(finalChosenCard.transform, finalChosenCard.transform.position + new Vector3(0, 0.5f, 0), 0.25f);
            yield return IterateLines(BraveUtility.RandomElement(numFortunesGiven == 0 ? BlessWeaponDialogue : BlessWeaponAgainDialogue), speaker);
            speaker.ClearInputOverride("npcConversation");
            speaker.ClearInputOverride("npcConversation");
            Pixelator.Instance.LerpToLetterbox(1, 0.25f);
            GameManager.Instance.MainCameraController.SetManualControl(false, true);
            eyesAnimator.Play($"fortuneteller_eyes_idle");
            TextBoxManager.ClearTextBox(talkpoint);

            yield return new WaitForSeconds(0.5f);
            yield return ApplyCardToGun(finalChosenCard.cardData, speaker);
            AkSoundEngine.PostEvent("Play_OBJ_ironcoin_wish_01", eyesAnimator.gameObject);
            SpawnManager.SpawnVFX(FortuneTeller.cardRevealVFX, finalChosenCard.transform.position, Quaternion.identity);
            UnityEngine.Object.Destroy(finalChosenCard.gameObject);
            finalChosenCard = null;

            numFortunesGiven++;
            SaveAPIManager.RegisterStatChange(CustomTrackedStats.TOTAL_FORTUNES_RECIEVED, 1);

            eyesAnimator.Play($"fortuneteller_eyes_magic");
            yield return TransitionToArmOutState(true);
            state = State.WAITING;
            m_room.RegisterInteractable(interactPoint);

            if (FortuneTellerDebugging) ETGModConsole.Log($"<color=#bb29e3>Fortune Teller</color> | Ending Fortune Sequence");

            yield break;
        }
        public float[] GetTotalDamageValuesOfVolley(ProjectileVolleyData volley)
        {
            float totalDamage = 0;
            float num = 0;

            foreach (ProjectileModule mod in volley.projectiles)
            {
                if (mod.projectiles != null)
                {
                    foreach (Projectile proj in mod.projectiles)
                    {
                        if (proj != null && proj.baseData != null) { totalDamage += proj.baseData.damage; num++; }
                    }
                }
                if (mod.finalVolley != null)
                {
                    float[] finalVolleystats = GetTotalDamageValuesOfVolley(mod.finalVolley);
                    totalDamage += finalVolleystats[0];
                    num += finalVolleystats[1];
                }
                if (mod.finalProjectile != null && mod.usesOptionalFinalProjectile)
                {
                    totalDamage += mod.finalProjectile.baseData.damage;
                    num++;
                }
                if (mod.chargeProjectiles != null)
                {
                    foreach (ProjectileModule.ChargeProjectile charge in mod.chargeProjectiles)
                    {
                        if (charge != null && charge.Projectile != null)
                        {
                            totalDamage += charge.Projectile.baseData.damage;
                            num++;
                        }
                    }
                }
            }

            return new float[] { totalDamage, num };
        }
        public IEnumerator ApplyCardToGun(TarotCardController.TarotCardData data, PlayerController player)
        {
            Gun currentGun = player.CurrentGun;

            if (data.tarotCard == TarotCardController.TarotCards.HANGED_MAN)
            {
                float[] gunDamageData = GetTotalDamageValuesOfVolley(currentGun.Volley);
                float meanDamage = gunDamageData[0] / gunDamageData[1];
                float finaldamage = meanDamage * 0.1f;

                List<TarotCardGunModifier> mods = new List<TarotCardGunModifier>();
                foreach (Gun g in player.inventory.AllGuns)
                {
                    if (g != currentGun)
                    {
                        TarotCardGunModifier modifier = g.gameObject.GetOrAddComponent<TarotCardGunModifier>();
                        mods.Add(modifier);
                    }
                }
                player.inventory.RemoveGunFromInventory(currentGun);
                UnityEngine.Object.Destroy(currentGun.gameObject);
                yield return null;
                foreach (TarotCardGunModifier mod in mods)
                {
                    mod.RegisterNewTarotCard(data);
                    mod.flatHangedManDamageIncrease += finaldamage;
                }
            }
            else if (data.tarotCard == TarotCardController.TarotCards.TEMPERANCE)
            {
                PickupObject.ItemQuality quality = currentGun.quality;
                if (quality == PickupObject.ItemQuality.EXCLUDED || quality == PickupObject.ItemQuality.COMMON || quality == PickupObject.ItemQuality.SPECIAL) { quality = PickupObject.ItemQuality.D; }
                if (quality != PickupObject.ItemQuality.S) quality++;
                if (quality != PickupObject.ItemQuality.S && UnityEngine.Random.value <= 0.33f) { quality++; }

                Gun newGun = LootEngine.GetItemOfTypeAndQuality<Gun>(quality, GameManager.Instance.RewardManager.GunsLootTable);
                player.inventory.AddGunToInventory(newGun, true);
                player.inventory.RemoveGunFromInventory(currentGun);
                UnityEngine.Object.Destroy(currentGun.gameObject);
            }
            else
            {
                TarotCardGunModifier modifier = currentGun.gameObject.GetOrAddComponent<TarotCardGunModifier>();
                yield return null;
                modifier.RegisterNewTarotCard(data);
            }
            GameUIRoot.Instance.notificationController.DoCustomNotification(data.name, data.subtitle, Initialisation.NPCCollection, Initialisation.NPCCollection.GetSpriteIdByName(data.spriteName), UINotificationController.NotificationColor.PURPLE, false, false);
            yield break;
        }
        public IEnumerator Conversation(string dialogue, PlayerController speaker, string animation = "talk", bool wait = true)
        {
            string finDialogue = dialogue;
            finDialogue = finDialogue.Replace("NICKNAME", StringTableManager.GetString("#PLAYER_NICK_COOPCULTIST"));

            if (FortuneTellerDebugging)
            {
                ETGModConsole.Log($"<color=#bb29e3>Fortune Teller</color> | Beginning conversation with text: <color=#f5cd56>\"{finDialogue}\"</color>");
                ETGModConsole.Log($"    Wait After: <color=#f5cd56>{wait}</color>");
            }

            //busy = true;
            eyesAnimator.PlayForDuration($"fortuneteller_eyes_{animation}", 2f, "fortuneteller_eyes_idle", false);
            if (armOut) armAnimator.PlayForDuration($"fortuneteller_arm_talk", 2f, "fortuneteller_arm_idle", false);
            TextBoxManager.ShowTextBox(talkpoint.position, talkpoint, 2f, finDialogue, "agunim", false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);
            if (wait) yield return new WaitForSeconds(2f);
            //busy = false;
            yield break;
        }
        public IEnumerator IterateLines(List<string> dialogue, PlayerController speaker, bool clearOverrides = false)
        {
            int conversationIndex = 0;
            while (conversationIndex <= dialogue.Count - 1)
            {
                TextBoxManager.ClearTextBox(talkpoint);
                TextBoxManager.ShowTextBox(talkpoint.position, talkpoint, -1f, $"{{wj}}{dialogue[conversationIndex]}{{w}}", "agunim", instant: false, showContinueText: true);
                float timer = 0;
                while (!BraveInput.GetInstanceForPlayer(speaker.PlayerIDX).ActiveActions.GetActionFromType(GungeonActions.GungeonActionType.Interact).WasPressed || timer < 0.4f)
                {
                    timer += BraveTime.DeltaTime;
                    yield return null;
                }
                conversationIndex++;
            }
            if (clearOverrides)
            {
                speaker.ClearInputOverride("npcConversation");
                Pixelator.Instance.LerpToLetterbox(1, 0.25f);
                GameManager.Instance.MainCameraController.SetManualControl(false, true);
            }
        }

        //Dialogue
        public static List<string> CultistRebuff = new List<string>()
        {
             "You cannot handle my fortunes, NICKNAME.",
             "I only wish to confer with the protagonist.",
             "Leave me, child."
        };
        public static List<List<string>> PickACardDialogue = new List<List<string>>()
        {
            new List<string>()
            {
                "And now, with the future revealed, I will allow you to pick a card...",
                "...any card...",
                "...and attach it to your weapon.",
                "...go on."
            },
        };
        public static List<List<string>> PickACardAgainDialogue = new List<List<string>>()
        {
            new List<string>()
            {
                "Here we are again...",
                "...time to choose, interloper...",
                "Which path to take... ...which road will your weapon travel?",
                "...good fortune..."
            },
        };
        public static List<List<string>> BlessWeaponDialogue = new List<List<string>>()
        {
            new List<string>()
            {
                "So it is, and will be...",
                "I bless your weapon, and with it the future shifts.",
                "...kosko bokht..."
            },
            new List<string>()
            {
                "The stars twist and bend to our will...",
                "...masters of our own fate...",
                "...puschca koshtipen..."
            },
        };
        public static List<List<string>> BlessWeaponAgainDialogue = new List<List<string>>()
        {
           new List<string>()
           {
                "Again, with the power of fate in my hand...",
                "...I bless your weapon."
           }
        };
        public static List<List<string>> AfterToldDialogue = new List<List<string>>()
        {

        };
        public static List<string> NoMoreFortunesDialogue = new List<string>()
        {
            "...kek koshtipen in dukkering knau...",
            "...I have no more cards for you...",
            "...the spirits are quietened...",
            "...pass on...",
        };
        public static List<string> FirstInteractSaveFile = new List<string>()
        {
            "...and what do we have here?",
            "It's been so long since we had a querent at our doorstep...",
            "Especially one as... soaked... in past, present, and future as you...",
            "Tell me sweetness, would you like me to tell your fortune?",
            "This very first time, we will ask nothing in return."
        };
        public static List<List<string>> GenericFortuneOffer = new List<List<string>>()
        {
            new List<string>()
            {
                "The querent is at our door once again...",
                "...I trust you remember our trade?..",
                "Would you like to probe the impenetrable darkness of the future once more?",
            },
            new List<string>()
            {
                "It is a pleasure to see you once again, bau...",
                "Do you once again have need of our services?",
                "We would be more than happy to oblige you..."
            }
        };
        public static List<List<string>> SecondFortuneOffer = new List<List<string>>()
        {
            new List<string>()
            {
                "It seems I have more cards...",
                "Shall we gaze into the darkness together once more?"
            },
            new List<string>()
            {
                "Again you come to us...",
                "Could you be seeking further insight?",
                "Very well, my lovely querent... that can be arranged."
            }
        };
        public static List<List<string>> PostDeclineFortuneOffer = new List<List<string>>()
        {
            new List<string>()
            {
                "Change your mind, beti-engri?",
                "...would you like to peer with us?..."
            },
            new List<string>()
            {
                "Have you reconsidered and reconciled with the self?",
                "It is, after all, the most important of the pleasures...",
            },
            new List<string>()
            {
                "Does it not tempt you?",
                "The desire to know... and change?...",
            }
        };
    }
    public class FortuneTellerInteractPointController : BraveBehaviour, IPlayerInteractable
    {
        public FortuneTeller master;

        public void OnConfigureWithTeller(FortuneTeller teller)
        {
            master = teller;
            if (master != null && master.m_room != null) { master.m_room.RegisterInteractable(this); }
            //SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black);
        }
        public float GetDistanceToPoint(Vector2 point)
        {
            return Vector2.Distance(point, base.transform.position) / 2f;
        }

        public void OnEnteredRange(PlayerController interactor)
        {

        }

        public void OnExitRange(PlayerController interactor)
        {
        }

        public void Interact(PlayerController interactor)
        {
            if (master && !master.armTransitioning && !master.busy && master.state != FortuneTeller.State.MID_FORTUNE && Vector2.Distance(this.transform.position, interactor.transform.position) <= 2f)
            {
                master.StartCoroutine(master.HandleInteract(interactor));
            }
        }
        public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
        {
            shouldBeFlipped = false;
            return string.Empty;
        }

        public float GetOverrideMaxDistance()
        {
            return 2f;
        }
    }
    public class TarotCardController : BraveBehaviour, IPlayerInteractable
    {
        [Serializable]
        public class TarotCardData
        {
            public string name = "TarotCard";
            public string subtitle = "Lorem Ipsum";
            public TarotCards tarotCard = TarotCards.FOOL;
            public string spriteName = "tarotcard_fool";
            public string tellerFaceAnimation = null;
            public List<string> flipDialogue = new List<string>()
            {
                "lorem ipsum",
                "dolor sit amet"
            };
            public string effectDescription = "Placeholder";
            public Action<Projectile, PlayerController, Gun> OnFiredBullet;
            public Action<BeamController, PlayerController, Gun> OnFiredBeam;
            public Action<Gun, PlayerController> OnRegisterWithGun;
            public Action<Projectile, SpeculativeRigidbody, bool> OnHitEnemy;
            public Action<BeamController, PlayerController, Gun> OnBeamChanceTick;
            public Action<PlayerController, Gun> OnGunReloaded;
            public Func<Gun, PlayerController, bool> CanBeAppliedToGun;
        }
        public FortuneTeller master;
        public bool isFlipped = false;
        public bool canBeInteracted = false;
        public bool selectionPhase = false;
        public TarotCardData cardData = null;
        public enum TarotCards
        {
            FOOL,
            MAGICIAN,
            HIGH_PRIESTESS,
            EMPRESS,
            EMPEROR,
            HEIROPHANT,
            LOVERS,
            CHARIOT,
            STRENGTH,
            HERMIT,
            WHEEL_OF_FORTUNE,
            JUSTICE,
            HANGED_MAN,
            DEATH,
            TEMPERANCE,
            DEVIL,
            TOWER,
            STAR,
            MOON,
            SUN,
            JUDGEMENT,
            WORLD,

            FOUR_PENTACLES,
            SIX_PENTACLES,
            TEN_PENTACLES,
            QUEEN_GUNS,
            KING_GUNS,
            KNIGHT_GUNS,
            NINE_CUPS
        }
        public void OnConfigureWithTeller(FortuneTeller teller)
        {
            master = teller;
            if (master != null && master.m_room != null) { master.m_room.RegisterInteractable(this); }
            canBeInteracted = true;
            SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black);
        }
        public float GetDistanceToPoint(Vector2 point)
        {
            return Vector2.Distance(point, base.transform.position) / 2f;
        }

        public void OnEnteredRange(PlayerController interactor)
        {
            SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, true);
            SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white);
            if (selectionPhase)
            {
                TextBoxManager.ShowInfoBox(this.transform.position + new Vector3(0f, 26f / 16f), this.transform, -1f, cardData.effectDescription, false, false);
            }
        }

        public void OnExitRange(PlayerController interactor)
        {
            SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, true);
            SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black);
            TextBoxManager.ClearTextBox(this.transform);
        }

        public void Interact(PlayerController interactor)
        {
            if (!canBeInteracted || cardData == null) { return; }
            base.StartCoroutine(HandleInteraction(interactor));
        }
        private IEnumerator HandleInteraction(PlayerController interactor)
        {
            if (selectionPhase)
            {
                if (cardData.CanBeAppliedToGun != null && !cardData.CanBeAppliedToGun(interactor.CurrentGun, interactor))
                {
                    TextBoxManager.ClearTextBox(this.transform);
                    interactor.SetInputOverride("npcConversation");
                    Pixelator.Instance.LerpToLetterbox(0.35f, 0.25f);
                    CameraController mainCameraController = GameManager.Instance.MainCameraController;
                    mainCameraController.SetManualControl(true, true);
                    mainCameraController.OverridePosition = master.eyesAnimator.sprite.WorldCenter;
                    master.eyesAnimator.Play($"fortuneteller_eyes_talk");

                    yield return master.IterateLines(new List<string>()
                    {
                        "The favours of this fortune are... at odds with your weapon...",
                        "Perhaps try a different card...",
                        "...or a different gun..."
                    }, interactor, true);
                    master.eyesAnimator.Play($"fortuneteller_eyes_idle");
                    TextBoxManager.ClearTextBox(master.talkpoint);
                    TextBoxManager.ShowInfoBox(this.transform.position + new Vector3(0f, 26f / 16f), this.transform, -1f, cardData.effectDescription, false, false);
                }
                else
                {
                    GameUIRoot.Instance.DisplayPlayerConversationOptions(interactor, null, $"<Take {cardData.name}>", "<Reconsider>");
                    int response2 = -1;
                    while (!GameUIRoot.Instance.GetPlayerConversationResponse(out response2))
                    {
                        yield return null;
                    }
                    if (response2 == 0)
                    {
                        this.canBeInteracted = false;
                        master.finalChosenCard = this;
                    }
                }
            }
            else
            {
                this.canBeInteracted = false;
                SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, true);
                SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black);
                this.spriteAnimator.Play("tarotcard_flip");
                AkSoundEngine.PostEvent("Play_OBJ_book_drop_01", base.gameObject);
                while (this.spriteAnimator.Playing)
                {
                    yield return null;
                }
                SpawnManager.SpawnVFX(FortuneTeller.cardRevealVFX, this.transform.position, Quaternion.identity);
                AkSoundEngine.PostEvent("Play_OBJ_ironcoin_wish_01", base.gameObject);
                this.sprite.SetSprite(Initialisation.NPCCollection.GetSpriteIdByName(cardData.spriteName));
                if (master)
                {
                    master.queuedFlippedCard = this;
                    master.m_room.DeregisterInteractable(this);

                }
                yield return null;
                this.isFlipped = true;
            }
        }
        public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
        {
            shouldBeFlipped = false;
            return string.Empty;
        }

        public float GetOverrideMaxDistance()
        {
            return 2f;
        }
    }
}

