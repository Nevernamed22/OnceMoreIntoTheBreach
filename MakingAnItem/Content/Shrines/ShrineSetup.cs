using Alexandria.BreakableAPI;
using Alexandria.DungeonAPI;
using Alexandria.ItemAPI;
using Dungeonator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    class ShrineSetup
    {
        public static void Init()
        {
            #region Generic Shrine-Base Placeable
            GameObject shrinePlaceable = new GameObject("ShrineGenericPlaceable");
            shrinePlaceable.SetActive(false);
            FakePrefab.MarkAsFakePrefab(shrinePlaceable);

            GameObject shrineBase = ItemBuilder.SpriteFromBundle("shrinebase_generic", Initialisation.NPCCollection.GetSpriteIdByName("shrinebase_generic"), Initialisation.NPCCollection, new GameObject("ShrineBase Generic"));
            shrineBase.GetComponent<tk2dSprite>().HeightOffGround = -1f;
            shrineBase.GetComponent<tk2dSprite>().SortingOrder = 0;
            shrineBase.GetComponent<tk2dSprite>().renderLayer = 0;
            shrineBase.GetComponent<MeshRenderer>().lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            shrineBase.GetComponent<tk2dSprite>().renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTiltedCutout");
            shrineBase.GetComponent<tk2dSprite>().usesOverrideMaterial = true;
            shrineBase.transform.SetParent(shrinePlaceable.transform);

            var shrineBaseBody = shrineBase.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(new IntVector2(0, -2), new IntVector2(26, 26));
            shrineBaseBody.CollideWithTileMap = false;
            shrineBaseBody.CollideWithOthers = true;
            shrineBaseBody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.HighObstacle;


            var shrineShadow = ItemBuilder.SpriteFromBundle("shrinebase_generic_shadow", Initialisation.NPCCollection.GetSpriteIdByName("shrinebase_generic_shadow"), Initialisation.NPCCollection, new GameObject("ShrineBase Generic Shadow"));
            shrineShadow.transform.SetParent(shrinePlaceable.transform);
            shrineShadow.transform.localPosition = new Vector3(-0.0625f, -0.125f);
            tk2dSprite shadow = shrineShadow.GetComponent<tk2dSprite>();
            shadow.HeightOffGround = -2f;
            shadow.SortingOrder = 0;
            shadow.renderLayer = 0;
            shadow.IsPerpendicular = false;
            shadow.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTilted");
            shadow.usesOverrideMaterial = true;

            shrinePlaceableGeneric = shrinePlaceable;
            #endregion
            #region WidePlaceables
            GameObject widePlaceable = new GameObject("ShrineWidePlaceable");
            widePlaceable.SetActive(false);
            FakePrefab.MarkAsFakePrefab(widePlaceable);

            GameObject wideBase = ItemBuilder.SpriteFromBundle("shrinebase_wide", Initialisation.NPCCollection.GetSpriteIdByName("shrinebase_wide"), Initialisation.NPCCollection, new GameObject("ShrineBase Wide"));
            wideBase.GetComponent<tk2dSprite>().HeightOffGround = -1f;
            wideBase.GetComponent<tk2dSprite>().SortingOrder = 0;
            wideBase.GetComponent<tk2dSprite>().renderLayer = 0;
            wideBase.GetComponent<MeshRenderer>().lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            wideBase.GetComponent<tk2dSprite>().renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTiltedCutout");
            wideBase.GetComponent<tk2dSprite>().usesOverrideMaterial = true;
            wideBase.transform.SetParent(widePlaceable.transform);

            var wideBaseBody = wideBase.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(new IntVector2(0, -2), new IntVector2(28, 28));
            wideBaseBody.CollideWithTileMap = false;
            wideBaseBody.CollideWithOthers = true;
            wideBaseBody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.HighObstacle;


            var wideShadow = ItemBuilder.SpriteFromBundle("shrinebase_wide_shadow", Initialisation.NPCCollection.GetSpriteIdByName("shrinebase_wide_shadow"), Initialisation.NPCCollection, new GameObject("ShrineBase Wide Shadow"));
            wideShadow.transform.SetParent(widePlaceable.transform);
            wideShadow.transform.localPosition = new Vector3(-0.0625f, -0.125f);
            tk2dSprite shadow2 = wideShadow.GetComponent<tk2dSprite>();
            shadow2.HeightOffGround = -2f;
            shadow2.SortingOrder = 0;
            shadow2.renderLayer = 0;
            shadow2.IsPerpendicular = false;
            shadow2.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTilted");
            shadow2.usesOverrideMaterial = true;

            shrinePlaceableGenericWide = widePlaceable;
            #endregion

            //Investment
            GameObject InvestmentPlaceable = FakePrefab.Clone(shrinePlaceableGeneric);
            InvestmentPlaceable.name = "Investment Shrine";
            GameObject InvestmentStatue = InvestmentShrine.Setup(InvestmentPlaceable.transform.Find("ShrineBase Generic").gameObject);
            InvestmentStatue.transform.SetParent(InvestmentPlaceable.transform);
            InvestmentStatue.transform.localPosition = new Vector3(-2f / 16f, 12f / 16f, 50f);
            InvestmentPlaceable.AddComponent<EncounterTrackable>().EncounterGuid = "nn:investment_shrine";
            AddShrineToPool(InvestmentPlaceable, new List<DungeonPrerequisite>() { }, 0.1875f, false);

            //Artemissile
            GameObject ArtemissilePlaceable = FakePrefab.Clone(shrinePlaceableGenericWide);
            ArtemissilePlaceable.name = "Artemissile Shrine";
            GameObject ArtemissileStatue = ArtemissileShrine.Setup(ArtemissilePlaceable.transform.Find("ShrineBase Wide").gameObject);
            ArtemissileStatue.transform.SetParent(ArtemissilePlaceable.transform);
            ArtemissileStatue.transform.localPosition = new Vector3(-13f / 16f, 13f / 16f, 50f);
            ArtemissilePlaceable.AddComponent<EncounterTrackable>().EncounterGuid = "nn:artemissile_shrine";
            AddShrineToPool(ArtemissilePlaceable, new List<DungeonPrerequisite>() { }, 0.1f, true);

            //Dagun
            GameObject DagunPlaceable = FakePrefab.Clone(shrinePlaceableGenericWide);
            DagunPlaceable.name = "Dagun Shrine";
            GameObject DagunStatue = DagunShrine.Setup(DagunPlaceable.transform.Find("ShrineBase Wide").gameObject);
            DagunStatue.transform.SetParent(DagunPlaceable.transform);
            DagunStatue.transform.localPosition = new Vector3(0f, 12f / 16f, 50f);
            DagunPlaceable.AddComponent<EncounterTrackable>().EncounterGuid = "nn:dagun_shrine";
            AddShrineToPool(DagunPlaceable, new List<DungeonPrerequisite>() { }, 0.1f, true);

            //Turtle
            GameObject turtlePlaceable = FakePrefab.Clone(shrinePlaceableGeneric);
            turtlePlaceable.name = "Turtle Shrine";
            GameObject turtleStatue = TurtleShrine.Setup(turtlePlaceable.transform.Find("ShrineBase Generic").gameObject);
            turtleStatue.transform.SetParent(turtlePlaceable.transform);
            turtleStatue.transform.localPosition = new Vector3(-6f / 16f, 12f / 16f, 50f);
            turtlePlaceable.AddComponent<EncounterTrackable>().EncounterGuid = "nn:turtle_shrine";
            AddShrineToPool(turtlePlaceable, new List<DungeonPrerequisite>() {
            new DungeonPrerequisite{
                 prerequisiteType = DungeonPrerequisite.PrerequisiteType.COMPARISON,
                 statToCheck = TrackedStats.TIMES_CLEARED_SEWERS,
                 prerequisiteOperation = DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN,
                 comparisonValue = 0
            }}, 0.1f, false);

            //Executioner
            GameObject executionerPlaceable = FakePrefab.Clone(shrinePlaceableGeneric);
            executionerPlaceable.name = "Executioner Shrine";
            GameObject executionerStatue = ExecutionerShrine.Setup(executionerPlaceable.transform.Find("ShrineBase Generic").gameObject);
            executionerStatue.transform.SetParent(executionerPlaceable.transform);
            executionerStatue.transform.localPosition = new Vector3(-4f / 16f, 12f / 16f, 50f);
            executionerPlaceable.AddComponent<EncounterTrackable>().EncounterGuid = "nn:executioner_shrine";
            AddShrineToPool(executionerPlaceable, new List<DungeonPrerequisite>() { }, 0.1f, false);

            //Relodin
            GameObject RelodinPlaceable = FakePrefab.Clone(shrinePlaceableGenericWide);
            RelodinPlaceable.name = "Relodin Shrine";
            GameObject RelodinStatue = RelodinShrine.Setup(RelodinPlaceable.transform.Find("ShrineBase Wide").gameObject);
            RelodinStatue.transform.SetParent(RelodinPlaceable.transform);
            RelodinStatue.transform.localPosition = new Vector3(-7f / 16f, 12f / 16f, 50f);
            RelodinPlaceable.AddComponent<EncounterTrackable>().EncounterGuid = "nn:relodin_shrine";
            AddShrineToPool(RelodinPlaceable, new List<DungeonPrerequisite>() { }, 0.1f, true);

            //Kliklok
            GameObject KliklokPlaceable = FakePrefab.Clone(shrinePlaceableGenericWide);
            KliklokPlaceable.name = "Kliklok Shrine";
            GameObject KliklokStatue = KliklokShrine.Setup(KliklokPlaceable.transform.Find("ShrineBase Wide").gameObject);
            KliklokStatue.transform.SetParent(KliklokPlaceable.transform);
            KliklokStatue.transform.localPosition = new Vector3(-11f / 16f, 12f / 16f, 50f);
            KliklokPlaceable.AddComponent<EncounterTrackable>().EncounterGuid = "nn:kliklok_shrine";
            AddShrineToPool(KliklokPlaceable, new List<DungeonPrerequisite>() { }, 0.2f, true);

            //Vulcairn
            GameObject VulcairnPlaceable = FakePrefab.Clone(shrinePlaceableGenericWide);
            VulcairnPlaceable.name = "Vulcairn Shrine";
            GameObject VulcairnStatue = VulcairnShrine.Setup(VulcairnPlaceable.transform.Find("ShrineBase Wide").gameObject);
            VulcairnStatue.transform.SetParent(VulcairnPlaceable.transform);
            VulcairnStatue.transform.localPosition = new Vector3(-12f / 16f, 12f / 16f, 50f);
            VulcairnPlaceable.AddComponent<EncounterTrackable>().EncounterGuid = "nn:vulcairn_shrine";
            AddShrineToPool(VulcairnPlaceable, new List<DungeonPrerequisite>() { }, 0.1875f, true);




            var carpet = ItemBuilder.SpriteFromBundle("shrine_carpet", Initialisation.NPCCollection.GetSpriteIdByName("shrine_carpet"), Initialisation.NPCCollection, new GameObject("Shrine Carpet"));
            carpet.MakeFakePrefab();
            tk2dSprite carpetSprite = carpet.GetComponent<tk2dSprite>();
            carpet.layer = 20;
            carpetSprite.SortingOrder = 0;
            carpetSprite.HeightOffGround = -2f;
            carpetSprite.IsPerpendicular = false;
            carpetSprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTilted");
            carpetSprite.usesOverrideMaterial = true;
            DungeonPlaceable carpetPlaceable = BreakableAPIToolbox.GenerateDungeonPlaceable(new Dictionary<GameObject, float>() { { carpet, 1f } });
            carpetPlaceable.variantTiers[0].unitOffset = new Vector2(0f, 0.5f);
            StaticReferences.StoredDungeonPlaceables.Add("shrine_carpet", carpetPlaceable);
            Alexandria.DungeonAPI.StaticReferences.customPlaceables.Add("nn:shrine_carpet", carpetPlaceable);

        }
        public static void AddShrineToPool(GameObject shrine, List<DungeonPrerequisite> prereq, float weight, bool wide) //0.25 - Common, 0.1875 - Rare, 0.1 - Very Rare
        {
            ResourceManager.LoadAssetBundle("shared_auto_001").LoadAsset<DungeonPlaceable>("whichshrinewillitbe").variantTiers.Add(new DungeonPlaceableVariant
            {
                percentChance = weight,
                nonDatabasePlaceable = shrine,
                enemyPlaceableGuid = "",
                pickupObjectPlaceableId = -1,
                forceBlackPhantom = false,
                addDebrisObject = false,
                prerequisites = prereq.ToArray(),
                unitOffset = wide ? new Vector2(2f/16f, 0f) : new Vector2(3f / 16f, 0f),
                materialRequirements = new DungeonPlaceableRoomMaterialRequirement[0],
            });
        }

        public static GameObject shrinePlaceableGeneric;
        public static GameObject shrinePlaceableGenericWide;
    }
}
