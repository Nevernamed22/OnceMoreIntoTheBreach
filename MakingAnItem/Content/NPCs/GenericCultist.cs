using Alexandria.BreakableAPI;
using Alexandria.DungeonAPI;
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
    public class GenericCultist : BraveBehaviour, IPlayerInteractable
    {
        public static void Init()
        {
            var cultist = ItemBuilder.SpriteFromBundle("genericcultist_idle_001", Initialisation.NPCCollection.GetSpriteIdByName("genericcultist_idle_001"), Initialisation.NPCCollection, new GameObject("Gun Cultist Talking"));
            cultist.SetActive(false);
            FakePrefab.MarkAsFakePrefab(cultist);
            cultist.AddComponent<GenericCultist>();
            var cultistBody = cultist.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(new IntVector2(2, -1), new IntVector2(10, 7));
            cultistBody.CollideWithTileMap = false;
            cultistBody.CollideWithOthers = true;

            UltraFortunesFavor fortune = cultist.AddComponent<UltraFortunesFavor>();
            fortune.sparkOctantVFX = ResourceManager.LoadAssetBundle("shared_auto_002").LoadAsset<GameObject>("npc_blank_jailed").GetComponentInChildren<UltraFortunesFavor>().sparkOctantVFX;

            tk2dSpriteAnimator Animator = cultist.GetOrAddComponent<tk2dSpriteAnimator>();
            Animator.Library = Initialisation.npcAnimationCollection;
            Animator.defaultClipId = Initialisation.npcAnimationCollection.GetClipIdByName("genericcultist_idle");
            Animator.DefaultClipId = Initialisation.npcAnimationCollection.GetClipIdByName("genericcultist_idle");
            Animator.playAutomatically = true;

            cultist.GetOrAddComponent<NPCShootReactor>();

            Transform talktransform = new GameObject("talkpoint").transform;
            talktransform.SetParent(cultist.transform);
            talktransform.transform.localPosition = new Vector3(7f/16f, 19f / 16f);

            var smallStatueShadow = ItemBuilder.SpriteFromBundle("ms_shadow", Initialisation.MysteriousStrangerCollection.GetSpriteIdByName("ms_shadow"), Initialisation.MysteriousStrangerCollection, new GameObject("ms_shadow"));
            tk2dSprite smallStatueShadowSprite = smallStatueShadow.GetComponent<tk2dSprite>();
            smallStatueShadowSprite.HeightOffGround = -1.7f;
            smallStatueShadowSprite.SortingOrder = 0;
            smallStatueShadowSprite.IsPerpendicular = false;
            smallStatueShadowSprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTilted");
            smallStatueShadowSprite.usesOverrideMaterial = true;
            smallStatueShadow.transform.SetParent(cultist.transform);
            smallStatueShadow.transform.localPosition = new Vector3(7f / 16f, 0f);

            Dictionary<GameObject, float> dict = new Dictionary<GameObject, float>() { { cultist, 1f } };
            DungeonPlaceable placeable = BreakableAPIToolbox.GenerateDungeonPlaceable(dict);
            StaticReferences.StoredDungeonPlaceables.Add("generic_cultist", placeable);
            Alexandria.DungeonAPI.StaticReferences.customPlaceables.Add("nn:generic_cultist", placeable);

        }
        private Transform talkpoint;
        public RoomHandler m_room;

        private void Start()
        {
            talkpoint = base.transform.Find("talkpoint");

            SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black);
            this.m_room = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Round));
            this.m_room.RegisterInteractable(this);

            base.GetComponent<NPCShootReactor>().OnShot += OnShot;

            m_room.Entered += PlayerEnteredRoom;
        }
        public static List<string> onEnter = new List<string>()
        {
            "Welcome!",
            "Our worship, interrupted?",
            "Glory!"
        };
        public static List<string> onShot = new List<string>()
        {
            "Iconoclast!",
            "Blasphemer!",
            "Betrayer! Betrayed me!",
            "Deception!"
        };
        public static List<string> chatter = new List<string>()
        {
            "Vengeance shall be hers, she who rifles the curtain!",
            "She who reloads this chamber and the next, oh Glory to thee!",
            "The gods of the guns and nature are strange indeed...",
            "Show some respect! You are in the presence of a holy idol!",
            "Glory! Glory to Kaliber of the Seven Sidearms!",
        };
        public static List<string> bored = new List<string>()
        {
            "Prostrate thyself!",
            "Bask! Bask in glory!",
            "Bask!",
            "Glory Glory Glory!"
        };
        public static List<List<string>> longTalk = new List<List<string>>()
        {
            new List<string>()
            {
                "Hark! Another reverent of the Gun!",
                "Come, prostrate thyself at the feet of our beloved idol!",
                "Bask in reverence!"
            },
            new List<string>()
            {
                "Another iconoclast!",
                "Come to rub your irreverence of our faith in our faces?",
                "Prostrate thyself and maybe ye will be forgiven of your ignorance!"
            },
            new List<string>()
            {
                "Gaze upon the glory of the shrine of our beloved god!",
                "Glory! Glory! Glory!",
            },
        };
        private void PlayerEnteredRoom(PlayerController player)
        {
           base.StartCoroutine(Conversation(BraveUtility.RandomElement(onEnter), player));
        }
        private void OnShot(Projectile proj)
        {
            base.StartCoroutine(Conversation(BraveUtility.RandomElement(onShot), GameManager.Instance.PrimaryPlayer));
        }
        public IEnumerator Conversation(string dialogue, PlayerController speaker, string itemName = "")
        {
            TextBoxManager.ShowTextBox(talkpoint.position, talkpoint, 3f, dialogue, "owl", false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);
            yield break;
        }
        public IEnumerator LongConversation(List<string> dialogue, PlayerController speaker, bool clearAfter = false, string itemName = "")
        {
            int conversationIndex = 0;
            while (conversationIndex <= dialogue.Count - 1)
            {
                TextBoxManager.ClearTextBox(talkpoint);
                TextBoxManager.ShowTextBox(talkpoint.position, talkpoint, -1f, dialogue[conversationIndex], "owl", instant: false, showContinueText: true);
                float timer = 0;
                while (!BraveInput.GetInstanceForPlayer(speaker.PlayerIDX).ActiveActions.GetActionFromType(GungeonActions.GungeonActionType.Interact).WasPressed || timer < 0.4f)
                {
                    timer += BraveTime.DeltaTime;
                    yield return null;
                }
                conversationIndex++;
            }
            if (clearAfter) { TextBoxManager.ClearTextBox(talkpoint); }
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
        public bool spokenOnce;
        public bool Bored;
        public IEnumerator HandleInteract(PlayerController interactor)
        {
            if (!spokenOnce)
            {
                interactor.SetInputOverride("npcConversation");
                Pixelator.Instance.LerpToLetterbox(0.35f, 0.25f);
                CameraController mainCameraController = GameManager.Instance.MainCameraController;
                mainCameraController.SetManualControl(true, true);
                mainCameraController.OverridePosition = base.transform.position;

                yield return LongConversation(BraveUtility.RandomElement(longTalk), interactor, true);
                spokenOnce = true;

                interactor.ClearInputOverride("npcConversation");
                Pixelator.Instance.LerpToLetterbox(1, 0.25f);
                GameManager.Instance.MainCameraController.SetManualControl(false, true);
            }
            else if (!Bored)
            {
                yield return Conversation(BraveUtility.RandomElement(chatter), interactor);
                Bored = true;
            }
            else
            {
                yield return Conversation(BraveUtility.RandomElement(bored), interactor);
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
    }
}
