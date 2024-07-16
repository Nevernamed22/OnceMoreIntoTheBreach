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
    class LowWalls : BraveBehaviour
    {
        public static void Init()
        {
            RegisterWall("lowwall_gp_westeast", "lowwall_gp_westeast_shadow", new Vector3(-4f / 16f, 0f, 50f), new IntVector2(0, -1), new IntVector2(16, 17), "lowwall_westeast");
            RegisterWall("lowwall_gp_westeastsouth", "lowwall_gp_westeastsouth_shadow", new Vector3(-4f / 16f, -8f / 16f, 50f), new IntVector2(0, 4), new IntVector2(16, 12), "lowwall_westeastsouth");
            RegisterWall("lowwall_gp_westeastnorth", "lowwall_gp_westeastnorth_shadow", new Vector3(-4f / 16f, 0f, 50f), new IntVector2(0, 0), new IntVector2(16, 16), "lowwall_westeastnorth");

            RegisterWall("lowwall_gp_westnorthsouth", "lowwall_gp_westnorthsouth_shadow", new Vector3(-4f / 16f, -8f / 16f, 50f), new IntVector2(0, 0), new IntVector2(16, 21), "lowwall_westnorthsouth");
            RegisterWall("lowwall_gp_eastnorthsouth", "lowwall_gp_eastnorthsouth_shadow", new Vector3(0f, -8f / 16f, 50f), new IntVector2(0, 0), new IntVector2(16, 21), "lowwall_eastnorthsouth");
            RegisterWall("lowwall_gp_northsouth", "lowwall_gp_northsouth_shadow", new Vector3(0f, -8f / 16f, 50f), new IntVector2(0, -1), new IntVector2(16, 21), "lowwall_northsouth");

            RegisterWall("lowwall_gp_northeastcorner", "lowwall_gp_northeastcorner_shadow", new Vector3(1f, 0f, 50f), new IntVector2(0, 0), new IntVector2(16, 21), "lowwall_northeastcorner");
            RegisterWall("lowwall_gp_northwestcorner", "lowwall_gp_northwestcorner_shadow", new Vector3(-4f / 16f, 0f, 50f), new IntVector2(0, 0), new IntVector2(16, 21), "lowwall_northwestcorner");

            RegisterWall("lowwall_gp_southeastcorner", "lowwall_gp_southeastcorner_shadow", new Vector3(0f, -8f / 16f, 50f), new IntVector2(0, -1), new IntVector2(16, 17), "lowwall_southeastcorner");
            RegisterWall("lowwall_gp_southwestcorner", "lowwall_gp_southwestcorner_shadow", new Vector3(-4f / 16f, -8f / 16f, 50f), new IntVector2(0, -1), new IntVector2(16, 17), "lowwall_southwestcorner");
        }

        public static void RegisterWall(string spritename, string shadowname, Vector3 shadowOffset, IntVector2 hitboxOffset, IntVector2 hitboxSize, string name)
        {
            GameObject LowWall = ItemBuilder.SpriteFromBundle(spritename, Initialisation.TrapCollection.GetSpriteIdByName(spritename), Initialisation.TrapCollection, new GameObject("Low Wall"));
            LowWall.MakeFakePrefab();
            var LowWallWestEastBody = LowWall.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(hitboxOffset, hitboxSize);
            LowWallWestEastBody.CollideWithTileMap = false;
            LowWallWestEastBody.CollideWithOthers = true;
            LowWallWestEastBody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.LowObstacle;
            LowWall.GetComponent<tk2dSprite>().GetComponent<tk2dSprite>().HeightOffGround = 0f;
            LowWall.GetComponent<tk2dSprite>().GetComponent<tk2dSprite>().IsPerpendicular = false;
            LowWall.GetComponent<MeshRenderer>().lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            LowWall.SetLayerRecursively(LayerMask.NameToLayer("FG_Critical"));
            LowWall.GetComponent<MeshRenderer>().material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTilted");
            LowWall.GetComponent<tk2dSprite>().usesOverrideMaterial = true;



            var shadowobj = ItemBuilder.SpriteFromBundle(shadowname, Initialisation.TrapCollection.GetSpriteIdByName(shadowname), Initialisation.TrapCollection, new GameObject("Low Wall Shadow"));
            shadowobj.transform.SetParent(LowWall.transform);
            shadowobj.transform.localPosition = shadowOffset;
            tk2dSprite shadow = shadowobj.GetComponent<tk2dSprite>();
            shadow.HeightOffGround = -5f;
            shadow.SortingOrder = 0;
            shadow.IsPerpendicular = false;
            shadow.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTilted");
            shadow.usesOverrideMaterial = true;

            LowWall.AddComponent<PlacedBlockerConfigurable>();

            LowWalls wallComp = LowWall.AddComponent<LowWalls>();
            wallComp.spritename = spritename;

            DungeonPlaceable lowWallWestEast = BreakableAPIToolbox.GenerateDungeonPlaceable(new Dictionary<GameObject, float>() { { LowWall.gameObject, 1f } });
            lowWallWestEast.isPassable = false;
            lowWallWestEast.width = 1;
            lowWallWestEast.height = 1;
            StaticReferences.StoredDungeonPlaceables.Add(name, lowWallWestEast);
            Alexandria.DungeonAPI.StaticReferences.customPlaceables.Add($"nn:{name}", lowWallWestEast);
        }
        public IntVector2 position;
        public string spritename = "";
        private void Start()
        {
            position = ((Vector2)base.transform.position).ToIntVector2();
            if (levelNames.ContainsKey(GameManager.Instance.Dungeon.tileIndices.tilesetId))
            {
                base.sprite.SetSprite(Initialisation.TrapCollection.GetSpriteIdByName(spritename.Replace("gp", levelNames[GameManager.Instance.Dungeon.tileIndices.tilesetId])));
            }
        }
        public static Dictionary<GlobalDungeonData.ValidTilesets, string> levelNames = new Dictionary<GlobalDungeonData.ValidTilesets, string>()
        {
            { GlobalDungeonData.ValidTilesets.CASTLEGEON, "keep" },
        };
    }
}