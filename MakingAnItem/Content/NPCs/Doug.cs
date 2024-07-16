using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.DungeonAPI;
using Alexandria.NPCAPI;
using Alexandria.BreakableAPI;
using Dungeonator;
using Alexandria.Misc;
using Alexandria.ItemAPI;
using SaveAPI;

namespace NevernamedsItems
{
    public static class Doug
    {
        public static GameObject mapIcon;
        public static GenericLootTable DougLootTable;
        public static void AddToLootPool(int id, float weight = 1)
        {
            if (DougLootTable == null) { DougLootTable = LootUtility.CreateLootTable(); }
            DougLootTable.AddItemToPool(id, weight);
        }
        public static void Init()
        {
            #region Strings
            List<string> TalkStrings = new List<string>()
            {
                "I roll my shop around. Here, there, any way the wind blows.",
                "My store is number 1, never number 2!",
                "I left my home town when it was destroyed by a horrible green bug, sporting a 90's 'tude.",
                "Have you met Bello? He runs the main shop down here. I quite admire his stools.",
                "My goods are always duty-free!",
                "Doug's Traveling Emporium is back! Get a load of my quality wares!",
                "My prices are great, and always dropping!"
            };
            foreach (string str in TalkStrings) { ETGMod.Databases.Strings.Core.AddComplex("#NNDOUG_GENERIC_TALK", str); }

            ETGMod.Databases.Strings.Core.AddComplex("#NNDOUG_STOPPER_TALK", "Push on.");

            ETGMod.Databases.Strings.Core.AddComplex("#NNDOUG_PURCHASE_TALK", "Thank you for your business!");

            ETGMod.Databases.Strings.Core.AddComplex("#NNDOUG_NOSALE_TALK", "Don't waste my time!");
            ETGMod.Databases.Strings.Core.AddComplex("#NNDOUG_NOSALE_TALK", "Sorry, no samples.");
            ETGMod.Databases.Strings.Core.AddComplex("#NNDOUG_NOSALE_TALK", "Your wad is lookin' a little light! Ball up some more [sprite \"ui_coin\"]...");

            ETGMod.Databases.Strings.Core.AddComplex("#NNDOUG_INTRO_TALK", "Welcome, welcome!");
            ETGMod.Databases.Strings.Core.AddComplex("#NNDOUG_INTRO_TALK", "Hello!");

            ETGMod.Databases.Strings.Core.AddComplex("#NNDOUG_ATTACKED_TALK", "Jerkus!");

            ETGMod.Databases.Strings.Core.AddComplex("#NNDOUG_STEAL_TALK", "Thief!");
            #endregion

            List<int> LootTable = new List<int>()
            {
                286, //+1 Bullets
                113, //Rocket Powered Bullets
                298, //Shock Rounds
                638, //Devolver Rounds
                640, //Vorpal Bullets
                822, //Katana Bullets
                655, //Hungry Bullets
                111, //Heavy Bullets
                288, //Bouncy Bullets
                304, //Explosive Rounds
                172, //Ghost Bullets
                373, //Alpha Bullet
                374, //Omega Bullets
                241, //Scattershot
                204, //Irradiated Lead
                295, //Hot Lead
                410, //Battery Bullets
                278, //Frost Bullets
                527, //Charming Rounds
                533, //Magic Bullets
                277, //Fat Bullets
                323, //Angry Bullets
                579, //Blank Bullets
                661, //Orbital Bullets
                284, //Homing Bullets
                352, //Shadow Bullets
                375, //Easy Reload Bullets
                523, //Stout Bullets
                636, //Snowballets
                530, //Remote Bullets
                528, //Zombie Bullets
                531, //Flak Bullets
                538, //Silver Bullets
                532, //Gilded Bullets
                627, //Platinum Bullets
                569, //Chaos Bullets
                571, //Cursed Bullets
                521, //Chance Bullets
                568, //Helix Bullets
                630, //Bumbullets
                524, //Bloody 9mm
                815, //Lichs Eye Bullets
            };
            foreach (int i in LootTable) { AddToLootPool(i); }

            mapIcon = ItemBuilder.SpriteFromBundle("doug_mapicon", Initialisation.NPCCollection.GetSpriteIdByName("doug_mapicon"), Initialisation.NPCCollection, new GameObject("doug_mapicon"));
            mapIcon.MakeFakePrefab();

            var doug = ItemBuilder.SpriteFromBundle("doug_idle_001", Initialisation.NPCCollection.GetSpriteIdByName("doug_idle_001"), Initialisation.NPCCollection, new GameObject("Doug"));
            SpeculativeRigidbody rigidbody = ShopAPI.GenerateOrAddToRigidBody(doug, CollisionLayer.LowObstacle, PixelCollider.PixelColliderGeneration.Manual, true, true, true, false, false, false, false, true, new IntVector2(9, 9), new IntVector2(5, -1));
            rigidbody.AddCollisionLayerOverride(CollisionMask.LayerToMask(CollisionLayer.BulletBlocker));

            GameObject shopObj = TempNPCTools.MakeIntoShopkeeper("Doug", "nn", doug, "doug_idle", "doug_talk", Initialisation.NPCCollection, Initialisation.npcAnimationCollection,
                   DougLootTable,
                   CustomShopItemController.ShopCurrencyType.COINS,
                   "#NNDOUG_GENERIC_TALK",
                   "#NNDOUG_STOPPER_TALK",
                   "#NNDOUG_PURCHASE_TALK",
                   "#NNDOUG_NOSALE_TALK",
                   "#NNDOUG_INTRO_TALK",
                   "#NNDOUG_ATTACKED_TALK",
                   "#NNDOUG_STEAL_TALK",
                   new Vector3(10f / 16f, 21f / 16f, 0), //Textbox Offset
                   new Vector3(31f / 16f, 32f / 16f, 0),
                   itemPositions: new List<Vector3> { new Vector3(1f, 1f, 1), new Vector3(2.5f, 0.5f, 1), new Vector3(4f, 1f, 1) }.ToArray(),
                   hasMinimapIcon: true,
                   minimapIcon: mapIcon,
                   Carpet: "doug_carpet",
                   costModifier: 0.8f,
                   addToShopAnnex: true,
                   shopAnnexWeight: 0.1f,
                   OnPurchase: DougBuy,
                   voice: "bug",
                   prerequisites: new List<DungeonPrerequisite>() {
                        new DungeonPrerequisite{
                        prerequisiteType = DungeonPrerequisite.PrerequisiteType.FLAG,
                        requireFlag = true,
                        saveFlagToCheck = GungeonFlags.SHOP_HAS_MET_BEETLE
                   }}.ToArray()
                   );

            AIAnimator aIAnimator = doug.GetComponent<AIAnimator>();
            aIAnimator.IdleAnimation = new DirectionalAnimation
            {
                Type = DirectionalAnimation.DirectionType.Single,
                Prefix = "doug_idle",
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
                Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
                Prefix = string.Empty,
                AnimNames = new string[]
                {
                        "doug_talkright",
                        "doug_talkleft",
                },
                Flipped = new DirectionalAnimation.FlipType[2]
            };
            AIAnimator.NamedDirectionalAnimation yesAnim = new AIAnimator.NamedDirectionalAnimation
            {
                name = "yes",
                anim = new DirectionalAnimation
                {
                    Prefix = string.Empty,
                    Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
                    Flipped = new DirectionalAnimation.FlipType[2],
                    AnimNames = new List<string>() { "doug_nodright", "doug_nodleft" }.ToArray(),
                }
            };
            AIAnimator.NamedDirectionalAnimation noAnim = new AIAnimator.NamedDirectionalAnimation
            {
                name = "no",
                anim = new DirectionalAnimation
                {
                    Prefix = string.Empty,
                    Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
                    Flipped = new DirectionalAnimation.FlipType[2],
                    AnimNames = new List<string>() { "doug_talkright", "doug_talkleft" }.ToArray(),
                }
            };
            if (aIAnimator.OtherAnimations == null) aIAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>();
            aIAnimator.OtherAnimations.Add(yesAnim);
            aIAnimator.OtherAnimations.Add(noAnim);

            var shitBall = ItemBuilder.SpriteFromBundle("dung_pack_idle_001", Initialisation.NPCCollection.GetSpriteIdByName("dung_pack_idle_001"), Initialisation.NPCCollection, new GameObject("Dung Pack"));
            shitBall.transform.SetParent(shopObj.transform);
            shitBall.transform.localPosition = new Vector3(20f / 16f, 45f / 16f);
            SpeculativeRigidbody dungrigidbody = ShopAPI.GenerateOrAddToRigidBody(shitBall, CollisionLayer.LowObstacle, PixelCollider.PixelColliderGeneration.Manual, true, true, true, false, false, false, false, true, new IntVector2(22, 20), new IntVector2(10, -1));
            dungrigidbody.AddCollisionLayerOverride(CollisionMask.LayerToMask(CollisionLayer.BulletBlocker));
            shitBall.GetComponent<MeshRenderer>().lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            shitBall.GetComponent<tk2dSprite>().renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTiltedCutout");
            shitBall.GetComponent<tk2dSprite>().usesOverrideMaterial = true;

            tk2dSpriteAnimator shitBallAnimator = shitBall.GetOrAddComponent<tk2dSpriteAnimator>();
            shitBallAnimator.Library = Initialisation.npcAnimationCollection;
            shitBallAnimator.defaultClipId = Initialisation.npcAnimationCollection.GetClipIdByName("doug_shitball");
            shitBallAnimator.DefaultClipId = Initialisation.npcAnimationCollection.GetClipIdByName("doug_shitball");
            shitBallAnimator.playAutomatically = true;


            Dictionary<GameObject, float> dict = new Dictionary<GameObject, float>() { { shopObj, 1f } };
            DungeonPlaceable placeable = BreakableAPIToolbox.GenerateDungeonPlaceable(dict);
            placeable.isPassable = true;
            placeable.width = 5;
            placeable.height = 5;
            StaticReferences.StoredDungeonPlaceables.Add("dougcustom", placeable);
            Alexandria.DungeonAPI.StaticReferences.customPlaceables.Add("nn:dougcustom", placeable);


            var backwall = ItemBuilder.SpriteFromBundle("dougwall_main", Initialisation.NPCCollection.GetSpriteIdByName("dougwall_main"), Initialisation.NPCCollection, new GameObject("Poop Wall"));
            backwall.MakeFakePrefab();
            /*SpeculativeRigidbody backwallrigidbody = ShopAPI.GenerateOrAddToRigidBody(
                backwall, CollisionLayer.LowObstacle, PixelCollider.PixelColliderGeneration.Tk2dPolygon,
                collideWithTileMap: false,
                CollideWithOthers: true,
                CanBeCarried: false,
                CanBePushed: false, 
                RecheckTriggers: false, 
                IsTrigger: false
                );*/


            SpeculativeRigidbody orAddComponent = backwall.GetOrAddComponent<SpeculativeRigidbody>();
            orAddComponent.CollideWithOthers = true;
            orAddComponent.CollideWithTileMap = false;
            orAddComponent.Velocity = Vector2.zero;
            orAddComponent.MaxVelocity = Vector2.zero;
            orAddComponent.ForceAlwaysUpdate = false;
            orAddComponent.CanPush = false;
            orAddComponent.CanBePushed = false;
            orAddComponent.PushSpeedModifier = 1f;
            orAddComponent.CanCarry = false;
            orAddComponent.CanBeCarried = false;
            orAddComponent.PreventPiercing = false;
            orAddComponent.SkipEmptyColliders = false;
            orAddComponent.RecheckTriggers = false;
            orAddComponent.UpdateCollidersOnRotation = false;
            orAddComponent.UpdateCollidersOnScale = false;
            orAddComponent.PixelColliders = new List<PixelCollider>()
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
            backwall.GetComponent<MeshRenderer>().lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            backwall.GetComponent<tk2dSprite>().renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTiltedCutout");
            backwall.GetComponent<tk2dSprite>().usesOverrideMaterial = true;
            backwall.GetComponent<tk2dSprite>().HeightOffGround = -10f;

            var backwallShadow = ItemBuilder.SpriteFromBundle("dougwall_shadow", Initialisation.NPCCollection.GetSpriteIdByName("dougwall_shadow"), Initialisation.NPCCollection, new GameObject("Shadow"));
            tk2dSprite backwallShadowSprite = backwallShadow.GetComponent<tk2dSprite>();
            backwallShadowSprite.HeightOffGround = -1.7f;
            backwallShadowSprite.SortingOrder = 0;
            backwallShadowSprite.IsPerpendicular = false;
            backwallShadowSprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTilted");
            backwallShadowSprite.usesOverrideMaterial = true;
            backwallShadow.transform.SetParent(backwall.transform);
            backwallShadow.transform.localPosition = new Vector3(0f, -3f / 16f);

            var stuff = ItemBuilder.SpriteFromBundle("dougwall_crap", Initialisation.NPCCollection.GetSpriteIdByName("dougwall_crap"), Initialisation.NPCCollection, new GameObject("Crap"));
            tk2dSprite stuffSprite = stuff.GetComponent<tk2dSprite>();
            stuffSprite.HeightOffGround = -1f;
            stuffSprite.SortingOrder = 0;
            stuffSprite.IsPerpendicular = false;
            stuffSprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTilted");
            stuffSprite.usesOverrideMaterial = true;
            stuff.transform.SetParent(backwall.transform);
            stuff.transform.localPosition = new Vector3(0f, -5f / 16f);

            var overhang = ItemBuilder.SpriteFromBundle("dougwall_overhang", Initialisation.NPCCollection.GetSpriteIdByName("dougwall_overhang"), Initialisation.NPCCollection, new GameObject("Overhang"));
            tk2dSprite overhangSprite = overhang.GetComponent<tk2dSprite>();
            overhangSprite.HeightOffGround = 10f;
            overhangSprite.SortingOrder = 0;
            overhangSprite.IsPerpendicular = true;
            overhangSprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTiltedCutout");
            overhangSprite.usesOverrideMaterial = true;
            overhang.transform.SetParent(backwall.transform);
            overhang.transform.localPosition = new Vector3(-16f / 16f, 69f / 16f);

            Dictionary<GameObject, float> backwalldict = new Dictionary<GameObject, float>() { { backwall, 1f } };
            DungeonPlaceable backwallplaceable = BreakableAPIToolbox.GenerateDungeonPlaceable(backwalldict);
            backwallplaceable.isPassable = true;
            backwallplaceable.width = 13;
            backwallplaceable.height = 9;
            StaticReferences.StoredDungeonPlaceables.Add("doug_wall", backwallplaceable);
            Alexandria.DungeonAPI.StaticReferences.customPlaceables.Add("nn:doug_wall", backwallplaceable);


            SharedInjectionData npcTable = GameManager.Instance.GlobalInjectionData.entries[2].injectionData;
            npcTable.InjectionData.Add(new ProceduralFlowModifierData()
            {
                annotation = "DougShop",
                DEBUG_FORCE_SPAWN = false,
                OncePerRun = false,
                placementRules = new List<ProceduralFlowModifierData.FlowModifierPlacementType>()
                 {
                     ProceduralFlowModifierData.FlowModifierPlacementType.END_OF_CHAIN
                 },
                roomTable = null,
                exactRoom = RoomFactory.BuildNewRoomFromResource("NevernamedsItems/Content/NPCs/Rooms/DougRoom.newroom").room,
                IsWarpWing = false,
                RequiresMasteryToken = false,
                chanceToLock = 0,
                selectionWeight = 1f,
                chanceToSpawn = 1,
                RequiredValidPlaceable = null,
                prerequisites = new List<DungeonPrerequisite>() {
                        new DungeonPrerequisite{
                        prerequisiteType = DungeonPrerequisite.PrerequisiteType.FLAG,
                        requireFlag = true,
                        saveFlagToCheck = GungeonFlags.SHOP_HAS_MET_BEETLE
                }}.ToArray(),
                CanBeForcedSecret = false,
                RandomNodeChildMinDistanceFromEntrance = 0,
                exactSecondaryRoom = null,
                framedCombatNodes = 0,
            });
        }
        public static bool DougBuy(PlayerController player, PickupObject item, int idfk)
        {
            SaveAPIManager.RegisterStatChange(CustomTrackedStats.DOUG_ITEMS_PURCHASED, 1);
            return false;
        }
    }
}
