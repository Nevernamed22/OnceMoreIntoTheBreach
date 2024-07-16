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
    public class Beggar : BraveBehaviour, IPlayerInteractable
    {
        public static void Init()
        {
            mapIcon = ItemBuilder.SpriteFromBundle("beggar_mapicon", Initialisation.NPCCollection.GetSpriteIdByName("beggar_mapicon"), Initialisation.NPCCollection, new GameObject("beggar_mapicon"));
            mapIcon.MakeFakePrefab();

            GameObject beggarplaceable = new GameObject("beggar placeable");
            beggarplaceable.SetActive(false);
            FakePrefab.MarkAsFakePrefab(beggarplaceable);



            var stuff = ItemBuilder.SpriteFromBundle("beggar_stuff", Initialisation.NPCCollection.GetSpriteIdByName("beggar_stuff"), Initialisation.NPCCollection, new GameObject("Stuff"));
            tk2dSprite stuffSprite = stuff.GetComponent<tk2dSprite>();
            stuffSprite.HeightOffGround = -2f;
            stuffSprite.SortingOrder = 0;
            stuffSprite.IsPerpendicular = false;
            stuffSprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTilted");
            stuffSprite.usesOverrideMaterial = true;
            stuff.transform.SetParent(beggarplaceable.transform);
            stuff.transform.localPosition = new Vector3(21f / 16f, 0f);


            var beggarShadow = ItemBuilder.SpriteFromBundle("beggar_shadow", Initialisation.NPCCollection.GetSpriteIdByName("beggar_shadow"), Initialisation.NPCCollection, new GameObject("beggar_shadow"));
            tk2dSprite beggarShadowSprite = beggarShadow.GetComponent<tk2dSprite>();
            beggarShadowSprite.HeightOffGround = -1.7f;
            beggarShadowSprite.SortingOrder = 0;
            beggarShadowSprite.IsPerpendicular = false;
            beggarShadowSprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTilted");
            beggarShadowSprite.usesOverrideMaterial = true;
            beggarShadow.transform.SetParent(beggarplaceable.transform);
            beggarShadow.transform.localPosition = new Vector3(1f / 16f, 5f / 16f);


            var beggar = ItemBuilder.SpriteFromBundle("beggar_idle_001", Initialisation.NPCCollection.GetSpriteIdByName("beggar_idle_001"), Initialisation.NPCCollection, new GameObject("Beggar"));
            beggar.transform.SetParent(beggarplaceable.transform);
            beggar.transform.localPosition = new Vector3(23f / 16f, 8f / 16f);
            beggar.AddComponent<Beggar>();
            var beggarBody = beggar.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(new IntVector2(4, 1), new IntVector2(12, 12));
            beggarBody.CollideWithTileMap = false;
            beggarBody.CollideWithOthers = true;

            beggar.GetOrAddComponent<NPCShootReactor>();

            UltraFortunesFavor fortune = beggar.AddComponent<UltraFortunesFavor>();
            fortune.sparkOctantVFX = ResourceManager.LoadAssetBundle("shared_auto_002").LoadAsset<GameObject>("npc_blank_jailed").GetComponentInChildren<UltraFortunesFavor>().sparkOctantVFX;

            tk2dSpriteAnimator beggarAnimator = beggar.GetOrAddComponent<tk2dSpriteAnimator>();
            beggarAnimator.Library = Initialisation.npcAnimationCollection;
            beggarAnimator.defaultClipId = Initialisation.npcAnimationCollection.GetClipIdByName("beggar_idle");
            beggarAnimator.DefaultClipId = Initialisation.npcAnimationCollection.GetClipIdByName("beggar_idle");
            beggarAnimator.playAutomatically = true;

            Transform talktransform = new GameObject("talkpoint").transform;
            talktransform.SetParent(beggar.transform);
            talktransform.transform.localPosition = new Vector3(12f / 16f, 20f / 16f);

            var box = ItemBuilder.SpriteFromBundle("beggar_box", Initialisation.NPCCollection.GetSpriteIdByName("beggar_box"), Initialisation.NPCCollection, new GameObject("Box"));
            box.transform.SetParent(beggarplaceable.transform);
            box.transform.localPosition = new Vector3(0, 6f / 16f);
            box.AddComponent<BeggarBox>();
            var boxBody = box.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(new IntVector2(3, -1), new IntVector2(18, 8));
            boxBody.CollideWithTileMap = false;
            boxBody.CollideWithOthers = true;

            Dictionary<GameObject, float> dict = new Dictionary<GameObject, float>() { { beggarplaceable, 1f } };
            DungeonPlaceable placeable = BreakableAPIToolbox.GenerateDungeonPlaceable(dict);
            StaticReferences.StoredDungeonPlaceables.Add("beggar", placeable);
            Alexandria.DungeonAPI.StaticReferences.customPlaceables.Add("nn:beggar", placeable);

            

            SharedInjectionData npcTable = GameManager.Instance.GlobalInjectionData.entries[0].injectionData;
            npcTable.InjectionData.Add(new ProceduralFlowModifierData()
            {
                annotation = "Beggar Room NonMines",
                DEBUG_FORCE_SPAWN = false,
                OncePerRun = false,
                placementRules = new List<ProceduralFlowModifierData.FlowModifierPlacementType>()
                {
                    ProceduralFlowModifierData.FlowModifierPlacementType.END_OF_CHAIN
                },
                roomTable = null,
                exactRoom = RoomFactory.BuildNewRoomFromResource("NevernamedsItems/Content/NPCs/Rooms/BeggarRoom.newroom").room,
                IsWarpWing = false,
                RequiresMasteryToken = false,
                chanceToLock = 0,
                selectionWeight = 0.05f,
                chanceToSpawn = 1,
                RequiredValidPlaceable = null,
                prerequisites = new List<DungeonPrerequisite>()
                {
                    new DungeonPrerequisite{
                    prerequisiteType = DungeonPrerequisite.PrerequisiteType.TILESET,
                    requireTileset = false,
                    requiredTileset = GlobalDungeonData.ValidTilesets.MINEGEON
                },
                     new CustomDungeonPrerequisite{
                    advancedPrerequisiteType = CustomDungeonPrerequisite.AdvancedPrerequisiteType.CUSTOM_FLAG,
                    requireCustomFlag = false,
                    customFlagToCheck = CustomDungeonFlags.GIVEN_BEGGARSBELIEF,
                }
                }.ToArray(),
                CanBeForcedSecret = false,
                RandomNodeChildMinDistanceFromEntrance = 0,
                exactSecondaryRoom = null,
                framedCombatNodes = 0,
            });
            npcTable.InjectionData.Add(new ProceduralFlowModifierData()
            {
                annotation = "Beggar Room",
                DEBUG_FORCE_SPAWN = false,
                OncePerRun = false,
                placementRules = new List<ProceduralFlowModifierData.FlowModifierPlacementType>()
                {
                    ProceduralFlowModifierData.FlowModifierPlacementType.END_OF_CHAIN
                },
                roomTable = null,
                exactRoom = RoomFactory.BuildNewRoomFromResource("NevernamedsItems/Content/NPCs/Rooms/BeggarRoom.newroom").room,
                IsWarpWing = false,
                RequiresMasteryToken = false,
                chanceToLock = 0,
                selectionWeight = 0.1f,
                chanceToSpawn = 1,
                RequiredValidPlaceable = null,
                prerequisites = new List<DungeonPrerequisite>()
                {
                     new CustomDungeonPrerequisite{
                    advancedPrerequisiteType = CustomDungeonPrerequisite.AdvancedPrerequisiteType.CUSTOM_FLAG,
                    requireCustomFlag = true,
                    customFlagToCheck = CustomDungeonFlags.GIVEN_BEGGARSBELIEF,
                }
                }.ToArray(),
                CanBeForcedSecret = false,
                RandomNodeChildMinDistanceFromEntrance = 0,
                exactSecondaryRoom = null,
                framedCombatNodes = 0,
            });

            SharedInjectionData injector = ScriptableObject.CreateInstance<SharedInjectionData>();
            injector.UseInvalidWeightAsNoInjection = true;
            injector.PreventInjectionOfFailedPrerequisites = false;
            injector.IsNPCCell = false;
            injector.IgnoreUnmetPrerequisiteEntries = false;
            injector.OnlyOne = false;
            injector.ChanceToSpawnOne = 1f;
            injector.AttachedInjectionData = new List<SharedInjectionData>();
            injector.InjectionData = new List<ProceduralFlowModifierData>
            {
                new ProceduralFlowModifierData()
                {
                    annotation = "BeggarRoomMines",
                    DEBUG_FORCE_SPAWN = false,
                    OncePerRun = false,
                    placementRules = new List<ProceduralFlowModifierData.FlowModifierPlacementType>()
                    {
                        ProceduralFlowModifierData.FlowModifierPlacementType.END_OF_CHAIN
                    },
                    roomTable = null,
                    exactRoom = RoomFactory.BuildNewRoomFromResource("NevernamedsItems/Content/NPCs/Rooms/BeggarRoom.newroom").room,
                    IsWarpWing = false,
                    RequiresMasteryToken = false,
                    chanceToLock = 0,
                    selectionWeight = 2,
                    chanceToSpawn = 1,
                    RequiredValidPlaceable = null,
                    prerequisites = new List<DungeonPrerequisite>()
                    {
                        new DungeonPrerequisite
                        {
                            prerequisiteType = DungeonPrerequisite.PrerequisiteType.TILESET,
                            requireTileset = true,
                            requiredTileset = GlobalDungeonData.ValidTilesets.MINEGEON
                        },
                        new CustomDungeonPrerequisite
                        {
                            advancedPrerequisiteType = CustomDungeonPrerequisite.AdvancedPrerequisiteType.CUSTOM_FLAG,
                            requireCustomFlag = false,
                            customFlagToCheck = CustomDungeonFlags.GIVEN_BEGGARSBELIEF,
                        }
                    }.ToArray(),
                    CanBeForcedSecret = false,
                    RandomNodeChildMinDistanceFromEntrance = 0,
                    exactSecondaryRoom = null,
                    framedCombatNodes = 0,
                }

            };

            injector.name = "BeggarRoomMines";
            SharedInjectionData BaseInjection = LoadHelper.LoadAssetFromAnywhere<SharedInjectionData>("Base Shared Injection Data");
            if (BaseInjection.AttachedInjectionData == null)
            {
                BaseInjection.AttachedInjectionData = new List<SharedInjectionData>();
            }
            BaseInjection.AttachedInjectionData.Add(injector);

            rewards = new List<BeggarReward>()
            {
                new BeggarReward(){ req = 5, ID = Spitballer.ID, giveFlag = CustomDungeonFlags.GIVEN_SPITBALLER },
                new BeggarReward(){ req = 10, ID = ScrapStrap.ID, giveFlag = CustomDungeonFlags.GIVEN_SCRAPSTRAP},
                new BeggarReward(){ req = 20, ID = FlamingShells.ID, giveFlag = CustomDungeonFlags.GIVEN_FLAMINGSHELLS},
                new BeggarReward(){ req = 40, ID = ShellNecklace.ID, giveFlag = CustomDungeonFlags.GIVEN_SHELLNECKLACE},
                new BeggarReward(){ req = 80, ID = UnderbarrelShotgun.ID, giveFlag = CustomDungeonFlags.GIVEN_UNDERBARRELSHOTGUN},
                new BeggarReward(){ req = 160, ID = WoodenKnife.ID, giveFlag = CustomDungeonFlags.GIVEN_WOODENKNIFE},
                new BeggarReward(){ req = 320, ID = Gungineer.ID, giveFlag = CustomDungeonFlags.GIVEN_GUNGINEER},
                new BeggarReward(){ req = 640, ID = ShroomedBullets.ID, giveFlag = CustomDungeonFlags.GIVEN_SHROOMEDBULLETS},
                new BeggarReward(){ req = 1280, ID = RingOfFortune.ID, giveFlag = CustomDungeonFlags.GIVEN_RINGOFFORTUNE},
                new BeggarReward(){ req = 2560, ID = BeggarsBelief.ID, giveFlag = CustomDungeonFlags.GIVEN_BEGGARSBELIEF},
            };
        }
        public static GameObject mapIcon;
        public RoomHandler m_room;
        private Transform talkpoint;
        public BeggarBox box;
        public bool busy;
        private void Start()
        {
            talkpoint = base.transform.Find("talkpoint");
            box = base.transform.parent.Find("Box").gameObject.GetComponent<BeggarBox>();
            box.master = this;
            SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black);
            this.m_room = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Round));
            this.m_room.RegisterInteractable(this);
            base.gameObject.GetComponent<NPCShootReactor>().OnShot += OnShot;
            Minimap.Instance.RegisterRoomIcon(m_room, mapIcon, false);
            m_room.Entered += PlayerEnteredRoom;
        }
        private void OnShot(Projectile proj)
        {
            base.StartCoroutine(Conversation(". . .", GameManager.Instance.PrimaryPlayer, "shake"));
        }
        private void PlayerEnteredRoom(PlayerController player)
        {
            if (player.IsStealthed) { return; }
            base.StartCoroutine(Conversation(BraveUtility.RandomElement(entryStrings), player));
        }

        public IEnumerator Conversation(string dialogue, PlayerController speaker, string animation = "nod")
        {
            base.spriteAnimator.PlayForDuration($"beggar_{animation}", 2f, "beggar_idle", false);
            TextBoxManager.ShowTextBox(talkpoint.position, talkpoint, 2f, dialogue, "oldman", false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);
            yield break;
        }
        public IEnumerator LongConversation(List<string> dialogue, PlayerController speaker, bool clearAfter = false, string animation = "nod")
        {

            int conversationIndex = 0;
            base.spriteAnimator.Play($"beggar_{animation}");
            while (conversationIndex <= dialogue.Count - 1)
            {
                TextBoxManager.ClearTextBox(talkpoint);
                TextBoxManager.ShowTextBox(talkpoint.position, talkpoint, -1f, dialogue[conversationIndex], "oldman", instant: false, showContinueText: true);
                float timer = 0;
                while (!BraveInput.GetInstanceForPlayer(speaker.PlayerIDX).ActiveActions.GetActionFromType(GungeonActions.GungeonActionType.Interact).WasPressed || timer < 0.4f)
                {
                    timer += BraveTime.DeltaTime;
                    yield return null;
                }
                conversationIndex++;
            }
            if (clearAfter) { TextBoxManager.ClearTextBox(talkpoint); }
            base.spriteAnimator.Play("beggar_idle");


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
            if (!TextBoxManager.HasTextBox(talkpoint) && !busy)
            {
                base.StartCoroutine(HandleInteract(interactor));
            }
        }
        public void OnDonationMade(int donationAmt, PlayerController donator)
        {
            hasDonated = true;
            timesspoken = 0;
            bool isRepeat = false;
            if (SaveAPIManager.GetFlag(CustomDungeonFlags.GIVEN_BEGGARSBELIEF)) { isRepeat = true; }

            if (isRepeat) { AdvancedGameStatsManager.Instance.BeggarRepeatCurrent += donationAmt; }
            else SaveAPIManager.RegisterStatChange(CustomTrackedStats.BEGGAR_TOTAL_DONATIONS, donationAmt);
            bool gaveReward = false;
            if (isRepeat)
            {
                if (AdvancedGameStatsManager.Instance.BeggarRepeatCurrent >= AdvancedGameStatsManager.Instance.BeggarRepeatTarget)
                {
                    AdvancedGameStatsManager.Instance.BeggarRepeatTarget = BraveUtility.RandomElement(new List<int> { 50, 100, 120, 150, 170, 200, 230 });
                    AdvancedGameStatsManager.Instance.BeggarRepeatCurrent = 0;

                    GenericLootTable lootTable = (UnityEngine.Random.value >= 0.5f) ? GameManager.Instance.RewardManager.GunsLootTable : GameManager.Instance.RewardManager.ItemsLootTable;
                    PickupObject item = null;
                    while (item == null)
                    {
                        GameObject temp = GameManager.Instance.CurrentRewardManager.GetItemForPlayer(donator, lootTable, BraveUtility.RandomElement(new List<PickupObject.ItemQuality> { PickupObject.ItemQuality.B, PickupObject.ItemQuality.A, PickupObject.ItemQuality.S }), new List<GameObject>() { });
                        if (temp && temp.GetComponent<PickupObject>() != null) { item = temp.GetComponent<PickupObject>(); }
                    }

                    base.StartCoroutine(GiveItem(donator, item.PickupObjectId, true));
                    gaveReward = true;
                }
            }
            else
            {
                foreach (BeggarReward reward in rewards)
                {
                    if (!SaveAPIManager.GetFlag(reward.giveFlag) && TotalDonated >= actualRequired(reward))
                    {
                        //Put Code Here for Beggar Item Give
                        base.StartCoroutine(GiveItem(donator, reward.ID, !gaveReward));
                        gaveReward = true;
                        SaveAPIManager.SetFlag(reward.giveFlag, true);
                    }
                }
            }
            if (!gaveReward)
            {
                base.StartCoroutine(Conversation(BraveUtility.RandomElement(postDonation), donator));
            }

        }
        public IEnumerator GiveItem(PlayerController interactor, int ID, bool dodialogue)
        {
            interactor.SetInputOverride("npcConversation");
            Pixelator.Instance.LerpToLetterbox(0.35f, 0.25f);
            CameraController mainCameraController = GameManager.Instance.MainCameraController;
            mainCameraController.SetManualControl(true, true);
            mainCameraController.OverridePosition = base.transform.position;

            if (ID == BeggarsBelief.ID)
            {
                yield return LongConversation(new List<string>()
                {
                    "list'n kid... you've'un givvun me a lotta [sprite \"ui_coin\"]...",
                    "an' i just wanned ta say...",
                    "thank ye",
                    "ye gave this'ol shell something better'n [sprite \"ui_coin\"]",
                    "ye gave me hope... {wj}belief{w}'n a better gungeon",
                    "take careof yerself now... i won' forget this."
                }, interactor, true);
                LootEngine.TryGivePrefabToPlayer(PickupObjectDatabase.GetById(ID).gameObject, interactor, false);
                AdvancedGameStatsManager.Instance.BeggarRepeatTarget = BraveUtility.RandomElement(new List<int> { 50, 100, 120, 150, 170, 200, 230 });
            }
            else
            {
                base.spriteAnimator.Play($"beggar_nod");
                if (dodialogue) TextBoxManager.ShowTextBox(talkpoint.position, talkpoint, 2f, BraveUtility.RandomElement(preGive), "oldman", false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);
                yield return new WaitForSeconds(1f);
                base.spriteAnimator.Play($"beggar_getitem");
                yield return new WaitForSeconds(3f);
                base.spriteAnimator.Play($"beggar_giveitem");
                yield return new WaitForSeconds(0.125f);
                if (GameStatsManager.Instance.IsRainbowRun) LootEngine.SpawnBowlerNote(GameManager.Instance.RewardManager.BowlerNoteOtherSource, base.transform.position + new Vector3(7f / 16f, -7f / 16f), interactor.CurrentRoom, true);
                else LootEngine.SpawnItem(PickupObjectDatabase.GetById(ID).gameObject, base.transform.position + new Vector3(7f / 16f, -7f / 16f), Vector2.down, 1);
                yield return new WaitForSeconds(0.5f);
                base.spriteAnimator.Play($"beggar_idle");
            }

            interactor.ClearInputOverride("npcConversation");
            Pixelator.Instance.LerpToLetterbox(1, 0.25f);
            GameManager.Instance.MainCameraController.SetManualControl(false, true);
            yield break;
        }
        public IEnumerator HandleInteract(PlayerController interactor)
        {
            if (timesspoken == 0)
            {
                if (hasDonated)
                {
                    base.StartCoroutine(Conversation(BraveUtility.RandomElement(postDonoTalkStrings), interactor));
                }
                else
                {
                    base.StartCoroutine(Conversation(BraveUtility.RandomElement(talkStrings), interactor));
                }
                timesspoken++;
            }
            else
            {
                base.StartCoroutine(Conversation(BraveUtility.RandomElement(boredStrings), interactor));
            }
            yield break;
        }
        int timesspoken = 0;
        bool hasDonated = false;
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
        public static List<string> entryStrings = new List<string>()
        {
            "...", ". . .", "...ev'nin'", "...'lo"
        };
        public static List<string> talkStrings = new List<string>()
        {
            "spare a [sprite \"ui_coin\"]?",
            ". . .",
            "mmm..",
            "ain' got much else to do..."
        };
        public static List<string> postDonoTalkStrings = new List<string>()
        {
            "ain' many with yer gen'ros'ty",
            "{wj}gung'neer{w}, huh? don' see many o' you that ain' lookun to start a fight..",
            "i don' think The Gun's even real any'mor...",
            "don' take yer you't fer granned, kid...",
            "be caref'l out there, these chamb'rs're dang'rous",
            "me'n my pal {wj}Dusty{w} usedta be a right pair o' slingers... wonder what 'appen'd ta him"
        };
        public static List<string> boredStrings = new List<string>()
        {
            "..."
        };
        public static List<string> postDonation = new List<string>()
        {
            ". . .", "thank ye'", "oblig'd", "yer a good 'egg","danke", "ta"
        };
        public static List<string> preGive = new List<string>()
        {
            "summin fer yer kindness","you d'serve it","goes 'round, eh?"
        };

        public int TotalDonated { get { return Mathf.FloorToInt(SaveAPIManager.GetPlayerStatValue(CustomTrackedStats.BEGGAR_TOTAL_DONATIONS)); } }
        public int TotalRequired
        {
            get
            {
                int num = 0;
                foreach (BeggarReward rew in rewards) { num += rew.req; }
                return num;
            }
        }

        public BeggarReward NextReward
        {
            get
            {
                BeggarReward next = null;
                foreach (BeggarReward reward in rewards)
                {
                    if (next == null)
                    {
                        if (TotalDonated < actualRequired(reward)) { next = reward; }
                    }
                }
                return next;
            }
        }
        public string NextRewardString
        {
            get
            {
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.GIVEN_BEGGARSBELIEF))
                {
                    return $"Next Goal: {AdvancedGameStatsManager.Instance.BeggarRepeatCurrent} / {AdvancedGameStatsManager.Instance.BeggarRepeatTarget}";
                }
                else
                {
                    BeggarReward next = null;
                    int prevGoals = 0;
                    foreach (BeggarReward reward in rewards)
                    {
                        if (next == null)
                        {
                            if (TotalDonated < actualRequired(reward)) { next = reward; }
                            else { prevGoals += reward.req; }
                        }
                    }
                    if (next != null)
                    {
                        return $"Next Goal: {TotalDonated - prevGoals} / {next.req}";
                    }
                    else
                    {
                        return "ERR";
                    }
                }
            }
        }
        public int AmountToNextReward
        {
            get
            {
                if (SaveAPIManager.GetFlag(CustomDungeonFlags.GIVEN_BEGGARSBELIEF))
                {
                    return AdvancedGameStatsManager.Instance.BeggarRepeatTarget - AdvancedGameStatsManager.Instance.BeggarRepeatCurrent;
                }
                else
                {
                    BeggarReward next = null;
                    int prevGoals = 0;
                    foreach (BeggarReward reward in rewards)
                    {
                        if (next == null)
                        {
                            if (TotalDonated < actualRequired(reward)) { next = reward; }
                            else { prevGoals += reward.req; }
                        }
                    }
                    if (next != null)
                    {
                        return next.req - (TotalDonated - prevGoals);
                    }
                    else
                    {
                        return 10;
                    }
                }
            }
        }
        public string TotalString
        {
            get
            {
                return TotalDonated >= TotalRequired ? "All Donation Goals Met" : $"Total: {TotalDonated} / {TotalRequired}";
            }
        }
        public int actualRequired(BeggarReward rew)
        {
            int req = 0;
            bool foundSelf = false;
            foreach (BeggarReward rew2 in rewards)
            {
                if (!foundSelf)
                {
                    req += rew2.req;
                    if (rew2.ID == rew.ID) { foundSelf = true; }
                }
            }
            return req;
        }

        public static List<BeggarReward> rewards;
        public class BeggarReward
        {
            public int req;
            public int ID;
            public CustomDungeonFlags giveFlag;
        }
    }

    public class BeggarBox : BraveBehaviour, IPlayerInteractable
    {
        public RoomHandler m_room;
        public Beggar master;
        private void Start()
        {
            SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black);
            this.m_room = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Round));
            this.m_room.RegisterInteractable(this);
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
            if (!master.busy)
            {
                base.StartCoroutine(HandleInteract(interactor));
            }
        }

        public IEnumerator HandleInteract(PlayerController interactor)
        {
            int amt = 10;
            if (SaveAPIManager.GetFlag(CustomDungeonFlags.GIVEN_BEGGARSBELIEF))
            {
                amt = 50;
                amt = Math.Min(amt, master.AmountToNextReward);
            }
            else
            {
                if (master.NextReward != null)
                {
                    if (master.NextReward.req >= 100) { amt = 50; }
                    if (master.NextReward.req >= 1000) { amt = 100; }
                    amt = Math.Min(amt, master.AmountToNextReward);
                }
            }
            amt = Math.Min(amt, interactor.carriedConsumables.Currency);

            TextBoxManager.ShowNote(base.transform.position + new Vector3(0f, 1f), base.transform, -1f,
                $"Too old fer gunnin. Donations welcome.\n{master.NextRewardString}\n{master.TotalString}"
                , true, false);
            int selectedResponse = -1;
            interactor.SetInputOverride("shrineConversation");
            yield return null;

            if (interactor.carriedConsumables.Currency > 0) { GameUIRoot.Instance.DisplayPlayerConversationOptions(interactor, null, $"Donate <{amt}[sprite \"ui_coin\"]>", "Turn Away"); }
            else { GameUIRoot.Instance.DisplayPlayerConversationOptions(interactor, null, "Turn Away", string.Empty); }

            while (!GameUIRoot.Instance.GetPlayerConversationResponse(out selectedResponse)) { yield return null; }

            interactor.ClearInputOverride("shrineConversation");
            TextBoxManager.ClearTextBox(base.transform);


            if (selectedResponse == 0 && interactor.carriedConsumables.Currency > 0)
            {
                interactor.carriedConsumables.Currency -= amt;
                master.OnDonationMade(amt, interactor);
            }

            yield break;
        }
        public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
        {
            shouldBeFlipped = false;
            return string.Empty;
        }
        public float GetOverrideMaxDistance() { return 1.5f; }
        public override void OnDestroy() { base.OnDestroy(); }
    }
}


