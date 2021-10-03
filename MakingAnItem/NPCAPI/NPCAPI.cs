using Dungeonator;
using GungeonAPI;
using HutongGames.PlayMaker;
using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace NpcApi
{
    class ItsDaFuckinShopApi
    {
        /// <summary>
        /// Creates a shop object along with an npc
        /// </summary>
        /// <param name="name">Name of the npc</param> 
        /// <param name="prefix">Mod prefix (for example Bot)</param> 
        /// 
        /// <param name="idleSpritePaths">List of *FULL* sprite paths for the idle animation</param> 
        /// <param name="idleFps">Fps of the idle animation (base game tends to use around 6)</param> 
        /// 
        /// <param name="talkSpritePaths">List of *FULL* sprite paths for the talk animation</param> 
        /// <param name="talkFps">Fps of the talk animation (base game tends to use around 8)</param> 
        /// 
        /// <param name="lootTable">Shop loot table</param> 
        /// <param name="shopType">What base game shop this should be treated as. Probably best set to "TRUCK"</param> 
        /// 
        /// <param name="runBasedMultilineGenericStringKey">String key for normal convos</param> 
        /// <param name="runBasedMultilineStopperStringKey">String key for if you try talking to an npc to much</param> 
        /// <param name="purchaseItemStringKey">String key for when the player buys something</param> 
        /// <param name="purchaseItemFailedStringKey">String key for when the player tries but fails to buy something</param> 
        /// <param name="introStringKey">String key for when the player enters the room</param> 
        /// <param name="attackedStringKey">String key for when the player shoots at the npc</param> 
        /// 
        /// <param name="hasCarpet">Wether the shop has a carpet or something else that they sit on</param> 
        /// <param name="carpetSpritePath">Sprite path for the carpet or whatever</param> 
        /// <returns></returns>

        public static GameObject SetUpShop(string name, string prefix, List<string> idleSpritePaths, int idleFps, List<string> talkSpritePaths, int talkFps, GenericLootTable lootTable, BaseShopController.AdditionalShopType shopType, string runBasedMultilineGenericStringKey,
            string runBasedMultilineStopperStringKey, string purchaseItemStringKey, string purchaseItemFailedStringKey, string introStringKey, string attackedStringKey, float costModifier = 1, bool hasCarpet = false, string carpetSpritePath = "")
        {

            try
            {
                var shared_auto_001 = ResourceManager.LoadAssetBundle("shared_auto_001");
                var SpeechPoint = new GameObject("SpeechPoint");
                SpeechPoint.transform.position = new Vector3(0.8125f, 2.1875f, -1.31f);



                var npcObj = SpriteBuilder.SpriteFromResource(idleSpritePaths[0], new GameObject(prefix + ":" + name));

                FakePrefab.MarkAsFakePrefab(npcObj);
                UnityEngine.Object.DontDestroyOnLoad(npcObj);
                npcObj.SetActive(false);

                npcObj.layer = 22;

                var collection = npcObj.GetComponent<tk2dSprite>().Collection;
                SpeechPoint.transform.parent = npcObj.transform;

                FakePrefab.MarkAsFakePrefab(SpeechPoint);
                UnityEngine.Object.DontDestroyOnLoad(SpeechPoint);
                SpeechPoint.SetActive(true);


                var idleIdsList = new List<int>();
                var talkIdsList = new List<int>();

                foreach (string sprite in idleSpritePaths)
                {
                    idleIdsList.Add(SpriteBuilder.AddSpriteToCollection(sprite, collection));
                }

                foreach (string sprite in talkSpritePaths)
                {
                    talkIdsList.Add(SpriteBuilder.AddSpriteToCollection(sprite, collection));
                }

                tk2dSpriteAnimator spriteAnimator = npcObj.AddComponent<tk2dSpriteAnimator>();

                SpriteBuilder.AddAnimation(spriteAnimator, collection, idleIdsList, name + "_idle", tk2dSpriteAnimationClip.WrapMode.Loop, idleFps);
                SpriteBuilder.AddAnimation(spriteAnimator, collection, talkIdsList, name + "_talk", tk2dSpriteAnimationClip.WrapMode.Loop, talkFps);

                SpeculativeRigidbody rigidbody = GenerateOrAddToRigidBody(npcObj, CollisionLayer.BulletBlocker, PixelCollider.PixelColliderGeneration.Manual, true, true, true, false, false, false, false, true, new IntVector2(20, 18), new IntVector2(5, 0));

                TalkDoerLite talkDoer = npcObj.AddComponent<TalkDoerLite>();

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
                talkDoer.audioCharacterSpeechTag = "oldman";
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


                UltraFortunesFavor dreamLuck = npcObj.AddComponent<UltraFortunesFavor>();

                dreamLuck.goopRadius = 2;
                dreamLuck.beamRadius = 2;
                dreamLuck.bulletRadius = 2;
                dreamLuck.bulletSpeedModifier = 0.8f;

                dreamLuck.vfxOffset = 0.625f;
                dreamLuck.sparkOctantVFX = shared_auto_001.LoadAsset<GameObject>("FortuneFavor_VFX_Spark");


                AIAnimator aIAnimator = GenerateBlankAIAnimator(npcObj);
                aIAnimator.spriteAnimator = spriteAnimator;
                aIAnimator.IdleAnimation = new DirectionalAnimation
                {
                    Type = DirectionalAnimation.DirectionType.Single,
                    Prefix = name + "_idle",
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
                    Prefix = name + "_talk",
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

                PlayMakerFSM iHaveNoFuckingClueWhatThisIs = npcObj.AddComponent<PlayMakerFSM>();

                UnityEngine.JsonUtility.FromJsonOverwrite(UnityEngine.JsonUtility.ToJson(basenpc.GetComponent<PlayMakerFSM>()), iHaveNoFuckingClueWhatThisIs);


                FieldInfo fsmStringParams = typeof(ActionData).GetField("fsmStringParams", BindingFlags.NonPublic | BindingFlags.Instance);

                (fsmStringParams.GetValue(iHaveNoFuckingClueWhatThisIs.FsmStates[1].ActionData) as List<FsmString>)[0].Value = runBasedMultilineGenericStringKey;
                (fsmStringParams.GetValue(iHaveNoFuckingClueWhatThisIs.FsmStates[1].ActionData) as List<FsmString>)[1].Value = runBasedMultilineStopperStringKey;

                (fsmStringParams.GetValue(iHaveNoFuckingClueWhatThisIs.FsmStates[4].ActionData) as List<FsmString>)[0].Value = purchaseItemStringKey;

                (fsmStringParams.GetValue(iHaveNoFuckingClueWhatThisIs.FsmStates[5].ActionData) as List<FsmString>)[0].Value = purchaseItemFailedStringKey;

                (fsmStringParams.GetValue(iHaveNoFuckingClueWhatThisIs.FsmStates[7].ActionData) as List<FsmString>)[0].Value = introStringKey;

                (fsmStringParams.GetValue(iHaveNoFuckingClueWhatThisIs.FsmStates[8].ActionData) as List<FsmString>)[0].Value = attackedStringKey;

                (fsmStringParams.GetValue(iHaveNoFuckingClueWhatThisIs.FsmStates[9].ActionData) as List<FsmString>)[0].Value = "#SUBSHOP_GENERIC_CAUGHT_STEALING";

                (fsmStringParams.GetValue(iHaveNoFuckingClueWhatThisIs.FsmStates[10].ActionData) as List<FsmString>)[0].Value = "#SHOP_GENERIC_NO_SALE_LABEL";

                (fsmStringParams.GetValue(iHaveNoFuckingClueWhatThisIs.FsmStates[12].ActionData) as List<FsmString>)[0].Value = "#COOP_REBUKE";


                npcObj.name = prefix + ":" + name;




                var ItemPoint1 = new GameObject("ItemPoint1");
                ItemPoint1.transform.position = new Vector3(1.125f, 2.125f, 1);
                FakePrefab.MarkAsFakePrefab(ItemPoint1);
                UnityEngine.Object.DontDestroyOnLoad(ItemPoint1);
                ItemPoint1.SetActive(true);
                var ItemPoint2 = new GameObject("ItemPoint2");
                ItemPoint2.transform.position = new Vector3(2.625f, 1f, 1);
                FakePrefab.MarkAsFakePrefab(ItemPoint2);
                UnityEngine.Object.DontDestroyOnLoad(ItemPoint2);
                ItemPoint2.SetActive(true);
                var ItemPoint3 = new GameObject("ItemPoint3");
                ItemPoint3.transform.position = new Vector3(4.125f, 2.125f, 1);
                FakePrefab.MarkAsFakePrefab(ItemPoint3);
                UnityEngine.Object.DontDestroyOnLoad(ItemPoint3);
                ItemPoint3.SetActive(true);


                var shopObj = new GameObject(prefix + ":" + name + "_Shop").AddComponent<BaseShopController>();
                FakePrefab.MarkAsFakePrefab(shopObj.gameObject);
                UnityEngine.Object.DontDestroyOnLoad(shopObj.gameObject);

                shopObj.gameObject.SetActive(false);

                shopObj.placeableHeight = 5;
                shopObj.placeableWidth = 5;
                shopObj.difficulty = 0;
                shopObj.isPassable = true;
                shopObj.baseShopType = shopType;
                shopObj.FoyerMetaShopForcedTiers = false;
                shopObj.IsBeetleMerchant = false;
                shopObj.ExampleBlueprintPrefab = null;
                shopObj.shopItems = lootTable;
                shopObj.spawnPositions = new Transform[] { ItemPoint1.transform, ItemPoint2.transform, ItemPoint3.transform };

                foreach (var pos in shopObj.spawnPositions)
                {
                    pos.parent = shopObj.gameObject.transform;
                }

                shopObj.shopItemsGroup2 = null;
                shopObj.spawnPositionsGroup2 = null;
                shopObj.spawnGroupTwoItem1Chance = 0.5f;
                shopObj.spawnGroupTwoItem2Chance = 0.5f;
                shopObj.spawnGroupTwoItem3Chance = 0.5f;
                shopObj.shopkeepFSM = npcObj.GetComponent<PlayMakerFSM>();
                shopObj.shopItemShadowPrefab = shared_auto_001.LoadAsset<GameObject>("Merchant_Key").GetComponent<BaseShopController>().shopItemShadowPrefab;
                shopObj.cat = null;
                shopObj.OptionalMinimapIcon = null;
                shopObj.ShopCostModifier = costModifier;
                shopObj.FlagToSetOnEncounter = GungeonFlags.NONE;

                npcObj.transform.parent = shopObj.gameObject.transform;
                npcObj.transform.position = new Vector3(1.9375f, 3.4375f, 5.9375f);

                if (hasCarpet)
                {
                    var carpetObj = SpriteBuilder.SpriteFromResource(carpetSpritePath, new GameObject(prefix + ":" + name + "_Carpet"));
                    carpetObj.GetComponent<tk2dSprite>().SortingOrder = 2;
                    FakePrefab.MarkAsFakePrefab(carpetObj);
                    UnityEngine.Object.DontDestroyOnLoad(carpetObj);
                    carpetObj.SetActive(true);

                    carpetObj.transform.position = new Vector3(0, 0, 1.7f);
                    carpetObj.transform.parent = shopObj.gameObject.transform;
                    carpetObj.layer = 20;
                }
                npcObj.SetActive(true);
                return shopObj.gameObject;
            }
            catch (Exception message)
            {
                ETGModConsole.Log(message.ToString());
                return null;
            }
        }

        public static void RegisterShopRoom(GameObject shop, PrototypeDungeonRoom protoroom, Vector2 offset)
        {
            protoroom.category = PrototypeDungeonRoom.RoomCategory.NORMAL;
            DungeonPrerequisite[] array = new DungeonPrerequisite[0];
            Vector2 vector = new Vector2((float)(protoroom.Width / 2) + offset.x, (float)(protoroom.Height / 2) + offset.y);
            protoroom.placedObjectPositions.Add(vector);
            protoroom.placedObjects.Add(new PrototypePlacedObjectData
            {
                contentsBasePosition = vector,
                fieldData = new List<PrototypePlacedObjectFieldData>(),
                instancePrerequisites = array,
                linkedTriggerAreaIDs = new List<int>(),
                placeableContents = new DungeonPlaceable
                {
                    width = 2,
                    height = 2,
                    respectsEncounterableDifferentiator = true,
                    variantTiers = new List<DungeonPlaceableVariant>
                    {
                        new DungeonPlaceableVariant
                        {
                            percentChance = 1f,
                            nonDatabasePlaceable = shop,
                            prerequisites = array,
                            materialRequirements = new DungeonPlaceableRoomMaterialRequirement[0]
                        }
                    }
                }
            });
            RoomFactory.RoomData roomData = new RoomFactory.RoomData
            {
                room = protoroom,
                isSpecialRoom = true,
                category = "SPECIAL",
                specialSubCategory = "WEIRD_SHOP"
            };
            RoomFactory.rooms.Add(shop.name, roomData);
            DungeonHandler.Register(roomData);
        }


        public static AIAnimator GenerateBlankAIAnimator(GameObject targetObject)
        {
            AIAnimator aianimator = targetObject.AddComponent<AIAnimator>();
            aianimator.facingType = AIAnimator.FacingType.Default;
            aianimator.faceSouthWhenStopped = false;
            aianimator.faceTargetWhenStopped = false;
            aianimator.AnimatedFacingDirection = -90f;
            aianimator.directionalType = AIAnimator.DirectionalType.Sprite;
            aianimator.RotationQuantizeTo = 0f;
            aianimator.RotationOffset = 0f;
            aianimator.ForceKillVfxOnPreDeath = false;
            aianimator.SuppressAnimatorFallback = false;
            aianimator.IsBodySprite = true;
            aianimator.IdleAnimation = new DirectionalAnimation
            {
                Type = DirectionalAnimation.DirectionType.None,
                Prefix = string.Empty,
                AnimNames = new string[0],
                Flipped = new DirectionalAnimation.FlipType[0]
            };
            aianimator.MoveAnimation = new DirectionalAnimation
            {
                Type = DirectionalAnimation.DirectionType.None,
                Prefix = string.Empty,
                AnimNames = new string[0],
                Flipped = new DirectionalAnimation.FlipType[0]
            };
            aianimator.FlightAnimation = new DirectionalAnimation
            {
                Type = DirectionalAnimation.DirectionType.None,
                Prefix = string.Empty,
                AnimNames = new string[0],
                Flipped = new DirectionalAnimation.FlipType[0]
            };
            aianimator.HitAnimation = new DirectionalAnimation
            {
                Type = DirectionalAnimation.DirectionType.None,
                Prefix = string.Empty,
                AnimNames = new string[0],
                Flipped = new DirectionalAnimation.FlipType[0]
            };
            aianimator.TalkAnimation = new DirectionalAnimation
            {
                Type = DirectionalAnimation.DirectionType.None,
                Prefix = string.Empty,
                AnimNames = new string[0],
                Flipped = new DirectionalAnimation.FlipType[0]
            };
            aianimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>(0);
            aianimator.OtherVFX = new List<AIAnimator.NamedVFXPool>(0);
            aianimator.OtherScreenShake = new List<AIAnimator.NamedScreenShake>(0);
            aianimator.IdleFidgetAnimations = new List<DirectionalAnimation>(0);
            aianimator.HitReactChance = 1f;
            aianimator.HitType = AIAnimator.HitStateType.Basic;
            return aianimator;
        }

        public static SpeculativeRigidbody GenerateOrAddToRigidBody(GameObject targetObject, CollisionLayer collisionLayer, PixelCollider.PixelColliderGeneration colliderGenerationMode = PixelCollider.PixelColliderGeneration.Tk2dPolygon, bool collideWithTileMap = false, bool CollideWithOthers = true, bool CanBeCarried = true, bool CanBePushed = false, bool RecheckTriggers = false, bool IsTrigger = false, bool replaceExistingColliders = false, bool UsesPixelsAsUnitSize = false, IntVector2? dimensions = null, IntVector2? offset = null)
        {
            SpeculativeRigidbody orAddComponent = targetObject.GetOrAddComponent<SpeculativeRigidbody>();
            orAddComponent.CollideWithOthers = CollideWithOthers;
            orAddComponent.CollideWithTileMap = collideWithTileMap;
            orAddComponent.Velocity = Vector2.zero;
            orAddComponent.MaxVelocity = Vector2.zero;
            orAddComponent.ForceAlwaysUpdate = false;
            orAddComponent.CanPush = false;
            orAddComponent.CanBePushed = CanBePushed;
            orAddComponent.PushSpeedModifier = 1f;
            orAddComponent.CanCarry = false;
            orAddComponent.CanBeCarried = CanBeCarried;
            orAddComponent.PreventPiercing = false;
            orAddComponent.SkipEmptyColliders = false;
            orAddComponent.RecheckTriggers = RecheckTriggers;
            orAddComponent.UpdateCollidersOnRotation = false;
            orAddComponent.UpdateCollidersOnScale = false;
            IntVector2 intVector = IntVector2.Zero;
            IntVector2 intVector2 = IntVector2.Zero;
            if (colliderGenerationMode != PixelCollider.PixelColliderGeneration.Tk2dPolygon)
            {
                if (dimensions != null)
                {
                    intVector2 = dimensions.Value;
                    if (!UsesPixelsAsUnitSize)
                    {
                        intVector2 = new IntVector2(intVector2.x * 16, intVector2.y * 16);
                    }
                }
                if (offset != null)
                {
                    intVector = offset.Value;
                    if (!UsesPixelsAsUnitSize)
                    {
                        intVector = new IntVector2(intVector.x * 16, intVector.y * 16);
                    }
                }
            }
            PixelCollider item = new PixelCollider
            {
                ColliderGenerationMode = colliderGenerationMode,
                CollisionLayer = collisionLayer,
                IsTrigger = IsTrigger,
                BagleUseFirstFrameOnly = (colliderGenerationMode == PixelCollider.PixelColliderGeneration.Tk2dPolygon),
                SpecifyBagelFrame = string.Empty,
                BagelColliderNumber = 0,
                ManualOffsetX = intVector.x,
                ManualOffsetY = intVector.y,
                ManualWidth = intVector2.x,
                ManualHeight = intVector2.y,
                ManualDiameter = 0,
                ManualLeftX = 0,
                ManualLeftY = 0,
                ManualRightX = 0,
                ManualRightY = 0
            };
            if (replaceExistingColliders | orAddComponent.PixelColliders == null)
            {
                orAddComponent.PixelColliders = new List<PixelCollider>
                {
                    item
                };
            }
            else
            {
                orAddComponent.PixelColliders.Add(item);
            }
            if (orAddComponent.sprite && colliderGenerationMode == PixelCollider.PixelColliderGeneration.Tk2dPolygon)
            {
                Bounds bounds = orAddComponent.sprite.GetBounds();
                orAddComponent.sprite.GetTrueCurrentSpriteDef().colliderVertices = new Vector3[]
                {
                    bounds.center - bounds.extents,
                    bounds.center + bounds.extents
                };
            }
            return orAddComponent;
        }

    }
}