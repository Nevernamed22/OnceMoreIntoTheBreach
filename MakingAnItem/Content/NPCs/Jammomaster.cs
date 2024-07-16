using Alexandria.ItemAPI;
using Dungeonator;
using SaveAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    class Jammomaster : BraveBehaviour, IPlayerInteractable
    {
        public Transform talkpoint;
        public static void Init()
        {
            GameObject jammomasterPlaceable = new GameObject("JammomasterPlaceable");
            jammomasterPlaceable.SetActive(false);
            FakePrefab.MarkAsFakePrefab(jammomasterPlaceable);

            var jammomaster = ItemBuilder.SpriteFromBundle("jammomaster2_idle_001", Initialisation.NPCCollection.GetSpriteIdByName("jammomaster2_idle_001"), Initialisation.NPCCollection, new GameObject("Jammomaster"));
            jammomaster.transform.SetParent(jammomasterPlaceable.transform);
            jammomaster.transform.localPosition = new Vector3(-1f, 2f / 26);
            jammomaster.AddComponent<Jammomaster>();
            var chancelotBody = jammomaster.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(new IntVector2(19, -1), new IntVector2(9, 8));
            chancelotBody.CollideWithTileMap = false;
            chancelotBody.CollideWithOthers = true;


            tk2dSpriteAnimator Animator = jammomaster.GetOrAddComponent<tk2dSpriteAnimator>();
            Animator.Library = Initialisation.npcAnimationCollection;
            Animator.defaultClipId = Initialisation.npcAnimationCollection.GetClipIdByName("jammomaster_idle");
            Animator.DefaultClipId = Initialisation.npcAnimationCollection.GetClipIdByName("jammomaster_idle");
            Animator.playAutomatically = true;


            Transform talktransform = new GameObject("talkpoint").transform;
            talktransform.SetParent(jammomaster.transform);
            talktransform.transform.localPosition = new Vector3(23f / 16f, 23f / 16f);

            var shadow = ItemBuilder.SpriteFromBundle("jammomaster2_shadow", Initialisation.NPCCollection.GetSpriteIdByName("jammomaster2_shadow"), Initialisation.NPCCollection, new GameObject("jammomaster_shadow"));
            tk2dSprite shadowSprite = shadow.GetComponent<tk2dSprite>();
            shadowSprite.HeightOffGround = -1.7f;
            shadowSprite.SortingOrder = 0;
            shadowSprite.IsPerpendicular = false;
            shadowSprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTilted");
            shadowSprite.usesOverrideMaterial = true;
            shadow.transform.SetParent(jammomaster.transform);

            JammomasterPlaceable = jammomasterPlaceable;
            JammomasterPlaceable.AddComponent<BreachPlacedItem>().positionInBreach = new Vector3(51.2f, 50.8f, 51.3f);
            BreachModifications.placedInBreach.Add(JammomasterPlaceable);

        }
        public static GameObject JammomasterPlaceable;
        public RoomHandler m_room;
        private void Start()
        {
            talkpoint = base.transform.Find("talkpoint");
            SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black);

            this.m_room = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Round));
            this.m_room.RegisterInteractable(this);
        }
        public IEnumerator Conversation(string dialogue, PlayerController speaker)
        {
            base.spriteAnimator.PlayForDuration($"jammomaster_talk", 2f, $"jammomaster_idle", false);
            TextBoxManager.ShowTextBox(talkpoint.position, talkpoint, 2f, dialogue, "bower", false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);
            yield break;
        }
        public IEnumerator LongConversation(List<string> dialogue, PlayerController speaker, bool clearAfter = false, string itemName = "")
        {
            int conversationIndex = 0;
            base.spriteAnimator.Play($"jammomaster_talk");
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
            base.spriteAnimator.Play($"jammomaster_idle");
            yield break;
        }
        public float GetDistanceToPoint(Vector2 point)
        {
            return Vector2.Distance(point, base.specRigidbody.UnitCenter) / 1.5f;
        }

        public void OnEnteredRange(PlayerController interactor)
        {
            if (base.spriteAnimator.CurrentClip.name == "jammomaster_darktalk")
            {
                base.spriteAnimator.Play($"jammomaster_idle");
                TextBoxManager.ClearTextBox(talkpoint);
            }
            SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, true);
            SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white);
        }

        public void OnExitRange(PlayerController interactor)
        {
            if (!hasDoneWalkAway && UnityEngine.Random.value <= 0.1f)
            {
                base.spriteAnimator.PlayForDuration($"jammomaster_darktalk", 1f, $"jammomaster_idle", false);
                TextBoxManager.ShowTextBox(talkpoint.position, talkpoint, 1f, BraveUtility.RandomElement(WalkAwayStrings), "mainframe", false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);
            }
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
            interactor.SetInputOverride("npcConversation");
            Pixelator.Instance.LerpToLetterbox(0.35f, 0.25f);
            CameraController mainCameraController = GameManager.Instance.MainCameraController;
            mainCameraController.SetManualControl(true, true);
            mainCameraController.OverridePosition = base.transform.position;

            SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, true);
            SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black);

            yield return LongConversation(AllJammedState.AllJammedActive ? UnjamTalkStrings : EnjamTalkStrings, interactor, false);

            GameUIRoot.Instance.DisplayPlayerConversationOptions(interactor, null, AllJammedState.AllJammedActive ? "I am content" : "I Accept", AllJammedState.AllJammedActive ? "Please undo... whatever you did" : "...no thanks");
            int response2 = -1;
            while (!GameUIRoot.Instance.GetPlayerConversationResponse(out response2)) { yield return null; }
            TextBoxManager.ClearTextBox(talkpoint);

            if (response2 == 0)
            {
                if (AllJammedState.AllJammedActive)
                {
                    if (bored) { yield return Conversation(BraveUtility.RandomElement(BoredStrings), interactor); }
                    else { yield return Conversation(BraveUtility.RandomElement(RandomTalkStrings), interactor); bored = true; }
                }
                else
                {
                    base.spriteAnimator.Play($"jammomaster_curse");
                    yield return new WaitForSeconds(0.74f);
                    interactor.PlayEffectOnActor(ResourceCache.Acquire("Global VFX/VFX_Curse") as GameObject, Vector3.zero, true, false, false);
                    yield return new WaitForSeconds(1f);
                    base.spriteAnimator.PlayForDuration($"jammomaster_darktalk", 2f, $"jammomaster_idle", false);
                    TextBoxManager.ShowTextBox(talkpoint.position, talkpoint, 2f, "{wj}It is done...{w}", "mainframe", false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);

                    yield return new WaitForSeconds(0.5f);
                    GameUIRoot.Instance.notificationController.DoCustomNotification("All-Jammed Mode Enabled", null, Initialisation.NPCCollection, Initialisation.NPCCollection.GetSpriteIdByName("alljammedmode_icon"), UINotificationController.NotificationColor.PURPLE, false, true);
                    SaveAPIManager.SetFlag(CustomDungeonFlags.ALLJAMMEDMODE_ENABLED_CONSOLE, false);
                    SaveAPIManager.SetFlag(CustomDungeonFlags.ALLJAMMEDMODE_ENABLED_GENUINE, true);
                    yield return new WaitForSeconds(0.5f);
                }
            }
            else
            {
                if (AllJammedState.AllJammedActive)
                {
                    base.spriteAnimator.Play($"jammomaster_curse");
                    yield return new WaitForSeconds(0.74f);
                    interactor.PlayEffectOnActor((PickupObjectDatabase.GetById(538) as SilverBulletsPassiveItem).SynergyPowerVFX, new Vector3(0f, -0.5f, 0f), true, false, false);
                    AkSoundEngine.PostEvent("Play_OBJ_shrine_accept_01", base.gameObject);
                    yield return new WaitForSeconds(1f);
                    base.spriteAnimator.PlayForDuration($"jammomaster_darktalk", 2f, $"jammomaster_idle", false);
                    TextBoxManager.ShowTextBox(talkpoint.position, talkpoint, 2f, "{wj}It is...  Un-done.{w}", "mainframe", false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);

                    yield return new WaitForSeconds(0.5f);
                    GameUIRoot.Instance.notificationController.DoCustomNotification("All-Jammed Mode Disabled", null, Initialisation.NPCCollection, Initialisation.NPCCollection.GetSpriteIdByName("alljammedmode_icon"), UINotificationController.NotificationColor.PURPLE, false, true);
                    SaveAPIManager.SetFlag(CustomDungeonFlags.ALLJAMMEDMODE_ENABLED_CONSOLE, false);
                    SaveAPIManager.SetFlag(CustomDungeonFlags.ALLJAMMEDMODE_ENABLED_GENUINE, false);
                    yield return new WaitForSeconds(0.5f);
                }
                else
                {
                    if (bored) { yield return Conversation(BraveUtility.RandomElement(BoredStrings), interactor); }
                    else { yield return Conversation(BraveUtility.RandomElement(RandomTalkStrings), interactor); bored = true; }
                }
            }

            SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, true);
            SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white);

            interactor.ClearInputOverride("npcConversation");
            Pixelator.Instance.LerpToLetterbox(1, 0.25f);
            GameManager.Instance.MainCameraController.SetManualControl(false, true);

            yield break;
        }
        public bool bored = false;
        public bool hasDoneWalkAway = false;
        public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
        {
            shouldBeFlipped = false;
            return string.Empty;
        }

        public float GetOverrideMaxDistance()
        {
            return 1.5f;
        }

        public static List<string> RandomTalkStrings = new List<string>()
        {
            "Do you feel it? The drive to embrace the darkness? That is what I have become...",
            "Tarry not. They come for you.",
            "Keep your guns about you... your wits are frayed and shattered.",
            "Oh. If only you knew the truth... alas, you could never understand...",
            "Did you ever hear the story of Slinger Scarus the Wise? I thought not...",
            "The shadows grow darker... things are working as planned.",
            "Speak not her name in my presence, for the work of the Profane Iconoclast hides from her arms..."
        };
        public static List<string> BoredStrings = new List<string>()
        {
            "You waste my time...",
            "Time is precious...",
            ". . .",
            "Toil away..."
        };
        public static List<string> WalkAwayStrings = new List<string>()
        {
            "{wj}...run...{w}",
            "{wj}...soon...{w}",
            "{wj}...things are aligning...{w}",
            "{wj}Yes... the Gungeon grows restless...{w}"
        };
        public static List<string> EnjamTalkStrings = new List<string>()
        {
            "All things wise and wonderful...",
            "...all creatures great and small...",
            "...all things bright and beautiful...",
            "...I've cursed them, one and all."
        };
        public static List<string> UnjamTalkStrings = new List<string>()
        {
            "What?...",
            "What is your desire now, young slinger?"
        };
    }
}
