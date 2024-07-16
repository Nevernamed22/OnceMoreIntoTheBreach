using Alexandria.ItemAPI;
using Alexandria.NPCAPI;
using Dungeonator;
using HutongGames.PlayMaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using static Alexandria.NPCAPI.CustomShopController;

namespace NevernamedsItems
{
    class TempNPCTools
    {
        public static GameObject MakeIntoShopkeeper(string name, string prefix, GameObject npcOBJ, string idleName, string talkName, tk2dSpriteCollectionData spriteCollection, tk2dSpriteAnimation animLibrary, GenericLootTable lootTable, CustomShopItemController.ShopCurrencyType currency, string runBasedMultilineGenericStringKey,
            string runBasedMultilineStopperStringKey, string purchaseItemStringKey, string purchaseItemFailedStringKey, string introStringKey, string attackedStringKey, string stolenFromStringKey, Vector3 talkPointOffset, Vector3 npcOffset, string voice = "oldman", Vector3[] itemPositions = null, float costModifier = 1, bool giveStatsOnPurchase = false,
            StatModifier[] statsToGiveOnPurchase = null, Func<CustomShopController, PlayerController, int, bool> CustomCanBuy = null, Func<CustomShopController, PlayerController, int, int> CustomRemoveCurrency = null, Func<CustomShopController, CustomShopItemController, PickupObject, int> CustomPrice = null,
            Func<PlayerController, PickupObject, int, bool> OnPurchase = null, Func<PlayerController, PickupObject, int, bool> OnSteal = null, string currencyIconPath = "", string currencyName = "", bool canBeRobbed = true, string Carpet = "",
            Vector2? CarpetOffset = null, bool hasMinimapIcon = false, GameObject minimapIcon = null, bool addToShopAnnex = false, float shopAnnexWeight = 0.1f, DungeonPrerequisite[] prerequisites = null, float fortunesFavorRadius = 2,
            ShopItemPoolType poolType = ShopItemPoolType.DEFAULT, bool RainbowModeImmunity = false)
        {

            try
            {

                if (prerequisites == null)
                {
                    prerequisites = new DungeonPrerequisite[0];
                }

                var shared_auto_001 = ResourceManager.LoadAssetBundle("shared_auto_001");
                var shared_auto_002 = ResourceManager.LoadAssetBundle("shared_auto_002");
                var SpeechPoint = new GameObject("SpeechPoint");
                SpeechPoint.transform.position = talkPointOffset;


                FakePrefab.MarkAsFakePrefab(npcOBJ);
                UnityEngine.Object.DontDestroyOnLoad(npcOBJ);
                npcOBJ.SetActive(false);
                npcOBJ.layer = 22;

                var collection = npcOBJ.GetComponent<tk2dSprite>().Collection;
                SpeechPoint.transform.parent = npcOBJ.transform;

                FakePrefab.MarkAsFakePrefab(SpeechPoint);
                UnityEngine.Object.DontDestroyOnLoad(SpeechPoint);
                SpeechPoint.SetActive(true);

                tk2dSpriteAnimator spriteAnimator = npcOBJ.GetOrAddComponent<tk2dSpriteAnimator>();
                spriteAnimator.Library = animLibrary;

                TalkDoerLite talkDoer = npcOBJ.AddComponent<TalkDoerLite>();
                talkDoer.placeableWidth = 4;
                talkDoer.placeableHeight = 3;
                talkDoer.difficulty = 0;
                talkDoer.isPassable = true;
                talkDoer.usesOverrideInteractionRegion = false;
                talkDoer.overrideRegionOffset = Vector2.zero;
                talkDoer.overrideRegionDimensions = Vector2.zero;
                talkDoer.overrideInteractionRadius = -1;
                talkDoer.PreventInteraction = false;
                talkDoer.AllowPlayerToPassEventually = true;
                talkDoer.speakPoint = SpeechPoint.transform;
                talkDoer.SpeaksGleepGlorpenese = false;
                talkDoer.audioCharacterSpeechTag = voice;
                talkDoer.playerApproachRadius = 5;
                talkDoer.conversationBreakRadius = 5;
                talkDoer.echo1 = null;
                talkDoer.echo2 = null;
                talkDoer.PreventCoopInteraction = false;
                talkDoer.IsPaletteSwapped = false;
                talkDoer.PaletteTexture = null;
                talkDoer.OutlineDepth = 0.5f;
                talkDoer.OutlineLuminanceCutoff = 0.05f;
                talkDoer.MovementSpeed = 3;
                talkDoer.PathableTiles = CellTypes.FLOOR;


                UltraFortunesFavor dreamLuck = npcOBJ.AddComponent<UltraFortunesFavor>();
                dreamLuck.goopRadius = fortunesFavorRadius;
                dreamLuck.beamRadius = fortunesFavorRadius;
                dreamLuck.bulletRadius = fortunesFavorRadius;
                dreamLuck.bulletSpeedModifier = 0.8f;
                dreamLuck.vfxOffset = 0.625f;
                dreamLuck.sparkOctantVFX = shared_auto_001.LoadAsset<GameObject>("FortuneFavor_VFX_Spark");


                AIAnimator aIAnimator = ShopAPI.GenerateBlankAIAnimator(npcOBJ);
                aIAnimator.spriteAnimator = spriteAnimator;
                aIAnimator.IdleAnimation = new DirectionalAnimation
                {
                    Type = DirectionalAnimation.DirectionType.Single,
                    Prefix = idleName,
                    AnimNames = new string[]
                    {
                        ""
                    },
                    Flipped = new DirectionalAnimation.FlipType[]
                    {
                        DirectionalAnimation.FlipType.None
                    }

                };
                aIAnimator.TalkAnimation = new DirectionalAnimation
                {
                    Type = DirectionalAnimation.DirectionType.Single,
                    Prefix = talkName,
                    AnimNames = new string[]
                    {
                        ""
                    },
                    Flipped = new DirectionalAnimation.FlipType[]
                    {
                        DirectionalAnimation.FlipType.None
                    }
                };



                var basenpc = ResourceManager.LoadAssetBundle("shared_auto_001").LoadAsset<GameObject>("Merchant_Key").transform.Find("NPC_Key").gameObject;

                PlayMakerFSM iHaveNoFuckingClueWhatThisIs = npcOBJ.AddComponent<PlayMakerFSM>();

                UnityEngine.JsonUtility.FromJsonOverwrite(UnityEngine.JsonUtility.ToJson(basenpc.GetComponent<PlayMakerFSM>()), iHaveNoFuckingClueWhatThisIs);

                FieldInfo fsmStringParams = typeof(ActionData).GetField("fsmStringParams", BindingFlags.NonPublic | BindingFlags.Instance);

                (fsmStringParams.GetValue(iHaveNoFuckingClueWhatThisIs.FsmStates[1].ActionData) as List<FsmString>)[0].Value = runBasedMultilineGenericStringKey;
                (fsmStringParams.GetValue(iHaveNoFuckingClueWhatThisIs.FsmStates[1].ActionData) as List<FsmString>)[1].Value = runBasedMultilineStopperStringKey;

                (fsmStringParams.GetValue(iHaveNoFuckingClueWhatThisIs.FsmStates[4].ActionData) as List<FsmString>)[0].Value = purchaseItemStringKey;

                (fsmStringParams.GetValue(iHaveNoFuckingClueWhatThisIs.FsmStates[5].ActionData) as List<FsmString>)[0].Value = purchaseItemFailedStringKey;

                (fsmStringParams.GetValue(iHaveNoFuckingClueWhatThisIs.FsmStates[7].ActionData) as List<FsmString>)[0].Value = introStringKey;

                (fsmStringParams.GetValue(iHaveNoFuckingClueWhatThisIs.FsmStates[8].ActionData) as List<FsmString>)[0].Value = attackedStringKey;

                (fsmStringParams.GetValue(iHaveNoFuckingClueWhatThisIs.FsmStates[9].ActionData) as List<FsmString>)[0].Value = stolenFromStringKey;
                (fsmStringParams.GetValue(iHaveNoFuckingClueWhatThisIs.FsmStates[9].ActionData) as List<FsmString>)[1].Value = stolenFromStringKey;

                (fsmStringParams.GetValue(iHaveNoFuckingClueWhatThisIs.FsmStates[10].ActionData) as List<FsmString>)[0].Value = "#SHOP_GENERIC_NO_SALE_LABEL";

                (fsmStringParams.GetValue(iHaveNoFuckingClueWhatThisIs.FsmStates[12].ActionData) as List<FsmString>)[0].Value = "#COOP_REBUKE";

                var posList = new List<Transform>();
                for (int i = 0; i < itemPositions.Length; i++)
                {

                    var ItemPoint = new GameObject("ItemPoint" + i);
                    ItemPoint.transform.position = itemPositions[i];
                    FakePrefab.MarkAsFakePrefab(ItemPoint);
                    UnityEngine.Object.DontDestroyOnLoad(ItemPoint);
                    ItemPoint.SetActive(true);
                    posList.Add(ItemPoint.transform);
                }

                var shopObj = new GameObject(prefix + ":" + name + "_Shop").AddComponent<CustomShopController>();
                shopObj.AllowedToSpawnOnRainbowMode = RainbowModeImmunity;
                FakePrefab.MarkAsFakePrefab(shopObj.gameObject);
                UnityEngine.Object.DontDestroyOnLoad(shopObj.gameObject);

                shopObj.gameObject.SetActive(false);
                shopObj.currencyType = currency;
                shopObj.ActionAndFuncSetUp(CustomCanBuy, CustomRemoveCurrency, CustomPrice, OnPurchase, OnSteal);

                if (currency == CustomShopItemController.ShopCurrencyType.CUSTOM)
                {
                    if (!string.IsNullOrEmpty(currencyIconPath))
                    {
                        shopObj.customPriceSprite = ShopAPI.AddCustomCurrencyType(currencyIconPath, $"{prefix}:{currencyName}", Assembly.GetCallingAssembly());
                    }
                    else
                    {
                        shopObj.customPriceSprite = currencyName;
                    }
                }

                shopObj.canBeRobbed = canBeRobbed;

                shopObj.placeableHeight = 5;
                shopObj.placeableWidth = 5;
                shopObj.difficulty = 0;
                shopObj.isPassable = true;
                shopObj.baseShopType = BaseShopController.AdditionalShopType.TRUCK;//shopType;

                shopObj.FoyerMetaShopForcedTiers = false;
                shopObj.IsBeetleMerchant = false;
                shopObj.ExampleBlueprintPrefab = null;
                shopObj.poolType = poolType;
                shopObj.shopItems = lootTable;
                shopObj.spawnPositions = posList.ToArray();

                foreach (var pos in shopObj.spawnPositions)
                {
                    pos.parent = shopObj.gameObject.transform;
                }

                shopObj.shopItemsGroup2 = null;
                shopObj.spawnPositionsGroup2 = null;
                shopObj.spawnGroupTwoItem1Chance = 0.5f;
                shopObj.spawnGroupTwoItem2Chance = 0.5f;
                shopObj.spawnGroupTwoItem3Chance = 0.5f;
                shopObj.shopkeepFSM = npcOBJ.GetComponent<PlayMakerFSM>();
                shopObj.shopItemShadowPrefab = shared_auto_001.LoadAsset<GameObject>("Merchant_Key").GetComponent<BaseShopController>().shopItemShadowPrefab;

                shopObj.prerequisites = prerequisites;
                shopObj.cat = null;


                if (hasMinimapIcon)
                {
                    shopObj.OptionalMinimapIcon = (minimapIcon != null) ? minimapIcon : ResourceCache.Acquire("Global Prefabs/Minimap_NPC_Icon") as GameObject;
                }

                shopObj.ShopCostModifier = costModifier;
                shopObj.FlagToSetOnEncounter = GungeonFlags.NONE;
                shopObj.giveStatsOnPurchase = giveStatsOnPurchase;
                shopObj.statsToGive = statsToGiveOnPurchase;



                npcOBJ.transform.parent = shopObj.gameObject.transform;
                npcOBJ.transform.position = npcOffset;

                if (!string.IsNullOrEmpty(Carpet))
                {
                    var carpetObj = ItemBuilder.SpriteFromBundle(Carpet, Initialisation.NPCCollection.GetSpriteIdByName(Carpet), Initialisation.NPCCollection, new GameObject("Carpet"));
                    carpetObj.GetComponent<tk2dSprite>().SortingOrder = 2;
                    FakePrefab.MarkAsFakePrefab(carpetObj);
                    UnityEngine.Object.DontDestroyOnLoad(carpetObj);
                    carpetObj.SetActive(true);

                    if (CarpetOffset == null) CarpetOffset = Vector2.zero;

                    carpetObj.transform.position = new Vector3(CarpetOffset.Value.x, CarpetOffset.Value.y, 1.7f);
                    carpetObj.transform.parent = shopObj.gameObject.transform;
                    carpetObj.layer = 20;

                    carpetObj.GetComponent<tk2dSprite>().renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTilted");
                    carpetObj.GetComponent<tk2dSprite>().usesOverrideMaterial = true;
                }
                npcOBJ.SetActive(true);

                if (addToShopAnnex)
                {
                    shared_auto_002.LoadAsset<DungeonPlaceable>("shopannex_contents_01").variantTiers.Add(new DungeonPlaceableVariant
                    {
                        percentChance = shopAnnexWeight,
                        unitOffset = new Vector2(-0.5f, -1.25f),
                        nonDatabasePlaceable = shopObj.gameObject,
                        enemyPlaceableGuid = "",
                        pickupObjectPlaceableId = -1,
                        forceBlackPhantom = false,
                        addDebrisObject = false,
                        prerequisites = prerequisites, //shit for unlocks gose here sooner or later
                        materialRequirements = new DungeonPlaceableRoomMaterialRequirement[0],

                    });
                }

                ShopAPI.builtShops.Add(prefix + ":" + name, shopObj.gameObject);
                return shopObj.gameObject;
            }
            catch (Exception message)
            {
                ETGModConsole.Log(message.ToString());
                return null;
            }
        }
    }
}
