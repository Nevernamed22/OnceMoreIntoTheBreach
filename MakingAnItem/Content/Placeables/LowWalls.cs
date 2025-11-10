using Alexandria.BreakableAPI;
using Alexandria.DungeonAPI;
using Alexandria.ItemAPI;
using Dungeonator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.Misc;

namespace NevernamedsItems
{
    public class LowWalls : BraveBehaviour
    {

        public static void Init()
        {
            GameObject LowWall = ItemBuilder.SpriteFromBundle("lowwall_gp_standalone", Initialisation.TrapCollection.GetSpriteIdByName("lowwall_gp_standalone"), Initialisation.TrapCollection, new GameObject("Low Wall"));
            LowWall.MakeFakePrefab();
            var LowWallWestEastBody = LowWall.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(new IntVector2(0, 0), new IntVector2(16, 16));
            LowWallWestEastBody.CollideWithTileMap = false;
            LowWallWestEastBody.CollideWithOthers = true;
            LowWallWestEastBody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.LowObstacle;
            LowWall.GetComponent<tk2dSprite>().GetComponent<tk2dSprite>().HeightOffGround = -1.4f;
            LowWall.GetComponent<tk2dSprite>().GetComponent<tk2dSprite>().IsPerpendicular = true;
            LowWall.GetComponent<tk2dSprite>().GetComponent<tk2dSprite>().UpdateZDepth();
            LowWall.GetComponent<MeshRenderer>().lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            LowWall.SetLayerRecursively(LayerMask.NameToLayer("FG_Critical"));
            LowWall.GetComponent<MeshRenderer>().material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTilted");
            LowWall.GetComponent<tk2dSprite>().usesOverrideMaterial = true;



            var shadowobj = ItemBuilder.SpriteFromBundle("lowwall_gp_standalone_shadow", Initialisation.TrapCollection.GetSpriteIdByName("lowwall_gp_standalone_shadow"), Initialisation.TrapCollection, new GameObject("Low Wall Shadow"));
            shadowobj.transform.SetParent(LowWall.transform);
            shadowobj.transform.localPosition = OffsetForShadowSprite["lowwall_gp_standalone_shadow"];
            tk2dSprite shadow = shadowobj.GetComponent<tk2dSprite>();
            shadow.HeightOffGround = -5f;
            shadow.SortingOrder = 0;
            shadow.IsPerpendicular = false;
            shadow.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTilted");
            shadow.usesOverrideMaterial = true;

            LowWall.AddComponent<PlacedBlockerConfigurable>();


            LowWalls wallComp = LowWall.AddComponent<LowWalls>();

            DungeonPlaceable lowWallWestEast = BreakableAPIToolbox.GenerateDungeonPlaceable(new Dictionary<GameObject, float>() { { LowWall.gameObject, 1f } });
            lowWallWestEast.isPassable = false;
            lowWallWestEast.width = 1;
            lowWallWestEast.height = 1;
            StaticReferences.StoredDungeonPlaceables.Add("low_wall", lowWallWestEast);
            Alexandria.DungeonAPI.StaticReferences.customPlaceables.Add($"nn:low_wall", lowWallWestEast);
        }

        public IntVector2 position;
        public GameObject shadow;
        private void Start()
        {
            shadow = base.transform.Find("Low Wall Shadow").gameObject;
            position = ((Vector2)base.transform.position).ToIntVector2();
            StaticReferenceManagerOMITB.LowWallDict.Add(position, this);
        }
        public enum NeighborType
        {
            LOW_WALL,
            WALL,
            NOTHING,
        }
        private NeighborType GetNeighborAtPosition(IntVector2 position)
        {
            CellData cellData = GameManager.Instance.Dungeon.data[position];
            if (cellData == null)
            {
                return NeighborType.NOTHING;
            }
            else if (cellData.type == CellType.WALL) { return NeighborType.WALL; }
            else if (StaticReferenceManagerOMITB.LowWallDict.ContainsKey(position)) { return NeighborType.LOW_WALL; }
            else return NeighborType.NOTHING;
        }
        public void ConfigureOnLevelLoad()
        {
            string levelname = "gp";
            if (levelNames.ContainsKey(GameManager.Instance.Dungeon.tileIndices.tilesetId)) { levelname = levelNames[GameManager.Instance.Dungeon.tileIndices.tilesetId]; }
            string wallType = "lowwall_gp_westeast";


            wallType = GetNeighborStringType(GetAllNeighbors());

            string shadowSpriteName = $"{wallType}_shadow";
            if (overrideShadowTextures.ContainsKey(wallType)) { shadowSpriteName = overrideShadowTextures[wallType]; }
            if (shadowSpriteName == null)
            {
                shadow.GetComponent<tk2dSprite>().renderer.enabled = false;
            }
            else
            {
                if (OffsetForShadowSprite.ContainsKey(shadowSpriteName)) { shadow.transform.localPosition = OffsetForShadowSprite[shadowSpriteName]; }
                shadow.GetComponent<tk2dSprite>().SetSprite(Initialisation.TrapCollection.GetSpriteIdByName(shadowSpriteName));
            }

            /*  if (HitboxParametersForWallType.ContainsKey(wallType))
              {
                  ETGModConsole.Log(HitboxParametersForWallType[wallType][0].x +", " + HitboxParametersForWallType[wallType][0].y);
                  base.specRigidbody.PixelColliders.Clear();
                  base.specRigidbody.PixelColliders = new List<PixelCollider>() {  new PixelCollider()
                  {
                      ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
                      CollisionLayer = CollisionLayer.LowObstacle,
                      IsTrigger = false,
                      BagleUseFirstFrameOnly = false,
                      SpecifyBagelFrame = string.Empty,
                      BagelColliderNumber = 0,
                      ManualOffsetX = HitboxParametersForWallType[wallType][1].x,
                      ManualOffsetY = HitboxParametersForWallType[wallType][1].y,
                      ManualWidth = HitboxParametersForWallType[wallType][0].x,
                      ManualHeight = HitboxParametersForWallType[wallType][0].y,
                      ManualDiameter = 0,
                      ManualLeftX = 0,
                      ManualLeftY = 0,
                      ManualRightX = 0,
                      ManualRightY = 0,
                  }};
                  base.specRigidbody.Reinitialize();
              }*/

            string finalSpriteName = levelname != "gp" ? wallType.Replace("gp", levelname) : wallType;
            if (Initialisation.TrapCollection.GetSpriteIdByName(finalSpriteName, 0) != 0)
            {
                base.sprite.SetSprite(Initialisation.TrapCollection.GetSpriteIdByName(finalSpriteName));
            }
            else
            {
                base.sprite.SetSprite(Initialisation.TrapCollection.GetSpriteIdByName(wallType));
                Debug.LogWarning($"LOW WALL - Type {finalSpriteName} does not exist. Reverting to {wallType}.");
            }
        }
        public NeighborType[] GetAllNeighbors()
        {
            NeighborType[] types = new NeighborType[] {
                GetNeighborAtPosition(position + new IntVector2(0,1)), //Above
                GetNeighborAtPosition(position + new IntVector2(1,0)), //Right
                GetNeighborAtPosition(position + new IntVector2(0,-1)), //Down
                GetNeighborAtPosition(position + new IntVector2(-1,0)), //Left
            };
            return types;
        }
        public static string GetNeighborStringType(NeighborType[] neighbors)
        {
            //Check if this is a ramp
            if (neighbors[1] == NeighborType.WALL) //East Wall
            {
                if (neighbors[3] == NeighborType.LOW_WALL) return "lowwall_gp_eastwallramp_open";
                else return "lowwall_gp_eastwallramp_closed";
            }
            else if (neighbors[3] == NeighborType.WALL) //West Wall
            {
                if (neighbors[1] == NeighborType.LOW_WALL) return "lowwall_gp_westwallramp_open";
                else return "lowwall_gp_westwallramp_closed";
            }
            for (int i = 3; i >= 0; i--)
            {
                if (neighbors[i] == NeighborType.WALL) { neighbors[i] = NeighborType.NOTHING; }
            }
            foreach (NeighborType[] dictNeighbors in WallType.Keys)
            {
                if (dictNeighbors.SequenceEqual(neighbors)) { return WallType[dictNeighbors]; }
            }

            return "lowwall_gp_standalone";
        }

        public Dictionary<string, string> overrideShadowTextures = new Dictionary<string, string>()
        {
            { "lowwall_gp_westwallramp_closed", "lowwall_gp_eastnorthsouth_shadow"},
            { "lowwall_gp_westwallramp_open","lowwall_gp_northsouth_shadow"},
            { "lowwall_gp_eastwallramp_closed", "lowwall_gp_westnorthsouth_shadow"},
            { "lowwall_gp_eastwallramp_open","lowwall_gp_northsouth_shadow"},
            {"lowwall_gp_alljunction", null },
            {"lowwall_gp_southjunction", null },
            {"lowwall_gp_northjunction","lowwall_gp_northsouth_shadow" }
        };

        public static Dictionary<NeighborType[], string> WallType = new Dictionary<NeighborType[], string>()
        {
            //No Neighbors
            {new NeighborType[]{ NeighborType.NOTHING, NeighborType.NOTHING, NeighborType.NOTHING, NeighborType.NOTHING }, "lowwall_gp_standalone" },

            //One Neighbor
            {new NeighborType[]{ NeighborType.NOTHING, NeighborType.NOTHING, NeighborType.NOTHING, NeighborType.LOW_WALL }, "lowwall_gp_eastnorthsouth" },
            {new NeighborType[]{ NeighborType.NOTHING, NeighborType.NOTHING, NeighborType.LOW_WALL, NeighborType.NOTHING }, "lowwall_gp_westeastnorth" },
            {new NeighborType[]{ NeighborType.NOTHING, NeighborType.LOW_WALL, NeighborType.NOTHING, NeighborType.NOTHING }, "lowwall_gp_westnorthsouth" },
            {new NeighborType[]{ NeighborType.LOW_WALL, NeighborType.NOTHING, NeighborType.NOTHING, NeighborType.NOTHING }, "lowwall_gp_westeastsouth" },

            //Two Neighbors
            {new NeighborType[]{ NeighborType.NOTHING, NeighborType.LOW_WALL, NeighborType.NOTHING, NeighborType.LOW_WALL }, "lowwall_gp_northsouth" },
            {new NeighborType[]{ NeighborType.LOW_WALL, NeighborType.NOTHING, NeighborType.LOW_WALL, NeighborType.NOTHING }, "lowwall_gp_westeast" },
            {new NeighborType[]{ NeighborType.LOW_WALL, NeighborType.LOW_WALL, NeighborType.NOTHING, NeighborType.NOTHING }, "lowwall_gp_southwestcorner" },
            {new NeighborType[]{ NeighborType.LOW_WALL, NeighborType.NOTHING, NeighborType.NOTHING, NeighborType.LOW_WALL }, "lowwall_gp_southeastcorner" },
            {new NeighborType[]{ NeighborType.NOTHING, NeighborType.NOTHING, NeighborType.LOW_WALL, NeighborType.LOW_WALL }, "lowwall_gp_northeastcorner" },
            {new NeighborType[]{ NeighborType.NOTHING, NeighborType.LOW_WALL, NeighborType.LOW_WALL, NeighborType.NOTHING }, "lowwall_gp_northwestcorner" },

            //Three Neighbors
            {new NeighborType[]{ NeighborType.LOW_WALL, NeighborType.LOW_WALL, NeighborType.NOTHING, NeighborType.LOW_WALL }, "lowwall_gp_northjunction" },
            {new NeighborType[]{ NeighborType.LOW_WALL, NeighborType.NOTHING, NeighborType.LOW_WALL, NeighborType.LOW_WALL }, "lowwall_gp_westjunction" },
            {new NeighborType[]{ NeighborType.NOTHING, NeighborType.LOW_WALL, NeighborType.LOW_WALL, NeighborType.LOW_WALL }, "lowwall_gp_southjunction" },
            {new NeighborType[]{ NeighborType.LOW_WALL, NeighborType.LOW_WALL, NeighborType.LOW_WALL, NeighborType.NOTHING }, "lowwall_gp_eastjunction" },

            //Four Neighbors
            {new NeighborType[]{ NeighborType.LOW_WALL, NeighborType.LOW_WALL, NeighborType.LOW_WALL, NeighborType.LOW_WALL }, "lowwall_gp_alljunction" },
        };
        public static Dictionary<string, Vector2> OffsetForShadowSprite = new Dictionary<string, Vector2>()
        {
            { "lowwall_gp_standalone_shadow", new Vector3(-4f / 16f, -8f / 16f, 50f) },
            { "lowwall_gp_westeast_shadow", new Vector3(-4f / 16f, 0f, 50f) },
            { "lowwall_gp_westeastsouth_shadow", new Vector3(-4f / 16f, -8f / 16f, 50f) },
            { "lowwall_gp_westeastnorth_shadow", new Vector3(-4f / 16f, 0f, 50f) },
            { "lowwall_gp_westnorthsouth_shadow", new Vector3(-4f / 16f, -8f / 16f, 50f) },
            { "lowwall_gp_eastnorthsouth_shadow",new Vector3(0f, -8f / 16f, 50f) },
            { "lowwall_gp_northsouth_shadow", new Vector3(0f, -8f / 16f, 50f) },
            { "lowwall_gp_northeastcorner_shadow",  new Vector3(1f, 0f, 50f) },
            { "lowwall_gp_northwestcorner_shadow", new Vector3(-4f / 16f, 0f, 50f) },
            { "lowwall_gp_southeastcorner_shadow", new Vector3(0f, -8f / 16f, 50f) },
            { "lowwall_gp_southwestcorner_shadow", new Vector3(-4f / 16f, -8f / 16f, 50f) },

            { "lowwall_gp_eastjunction_shadow", new Vector3(-4f / 16f,0f, 50f) },
            { "lowwall_gp_westjunction_shadow", new Vector3(1f,0f, 50f) }

        };
        public static Dictionary<GlobalDungeonData.ValidTilesets, string> levelNames = new Dictionary<GlobalDungeonData.ValidTilesets, string>()
        {
            { GlobalDungeonData.ValidTilesets.CASTLEGEON, "keep" },
            { GlobalDungeonData.ValidTilesets.MINEGEON, "mine" },
        };
        public override void OnDestroy()
        {
            StaticReferenceManagerOMITB.LowWallDict.Remove(position);
            base.OnDestroy();
        }
    }
}