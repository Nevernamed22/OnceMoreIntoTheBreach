using Dungeonator;
using Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tk2dRuntime.TileMap;
using UnityEngine;

namespace NevernamedsItems
{
    class LoadRoom
    {
        public static void Init()
        {
            ETGModConsole.Commands.GetGroup("nn").AddUnit("loadroom", delegate (string[] args)
            {
                string floorToCheck = "UNDEFINED";
                if (args != null && args.Length > 0 && args[0] != null) { if (!string.IsNullOrEmpty(args[0])) { floorToCheck = args[0]; } }

                PrototypeDungeonRoom room = null;

                //ETGModConsole.Log(GameManager.Instance.Dungeon.name);
                string dungeonName = GameManager.Instance.Dungeon.name.Replace("(Clone)", "").ToLower();
                //ETGModConsole.Log("1");
                Dungeon keepDungeon = DungeonDatabase.GetOrLoadByName(dungeonName);
                //ETGModConsole.Log("2");
                foreach (WeightedRoom roomsinflow in keepDungeon.PatternSettings.flows[0].fallbackRoomTable.includedRooms.elements)
                {
                //ETGModConsole.Log("tes");
                    if (roomsinflow != null && roomsinflow.room != null && roomsinflow.room.name == floorToCheck) room = roomsinflow.room;
                }
                //ETGModConsole.Log("3");
                keepDungeon = null;
                if (room != null)
                {
                //ETGModConsole.Log("4");
                    RoomHandler finalRoom = AddCustomRuntimeRoom(room);
                //ETGModConsole.Log("5");
                    finalRoom.visibility = RoomHandler.VisibilityStatus.VISITED;
                //ETGModConsole.Log("6");
                    Minimap.Instance.RevealMinimapRoom(finalRoom);
                }

            }, null);
        }
        public static RoomHandler AddCustomRuntimeRoom(PrototypeDungeonRoom prototype, bool addRoomToMinimap = true, bool addTeleporter = true, bool isSecretRatExitRoom = false, Action<RoomHandler> postProcessCellData = null, DungeonData.LightGenerationStyle lightStyle = DungeonData.LightGenerationStyle.STANDARD, bool allowProceduralDecoration = true, bool allowProceduralLightFixtures = true, bool suppressExceptionMessages = false)
        {
            Dungeon dungeon = GameManager.Instance.Dungeon;
            tk2dTileMap m_tilemap = dungeon.MainTilemap;

            if (m_tilemap == null)
            {
                ETGModConsole.Log("ERROR: TileMap object is null! Something seriously went wrong!");
                Debug.Log("ERROR: TileMap object is null! Something seriously went wrong!");
                return null;
            }

            TK2DDungeonAssembler assembler = dungeon.assembler;

            IntVector2 basePosition = IntVector2.Zero;
            IntVector2 basePosition2 = new IntVector2(50, 50);
            int num = basePosition2.x;
            int num2 = basePosition2.y;
            IntVector2 intVector = new IntVector2(int.MaxValue, int.MaxValue);
            IntVector2 intVector2 = new IntVector2(int.MinValue, int.MinValue);
            intVector = IntVector2.Min(intVector, basePosition);
            intVector2 = IntVector2.Max(intVector2, basePosition + new IntVector2(prototype.Width, prototype.Height));
            IntVector2 a = intVector2 - intVector;
            IntVector2 b = IntVector2.Min(IntVector2.Zero, -1 * intVector);
            a += b;
            IntVector2 intVector3 = new IntVector2(dungeon.data.Width + num, num);
            int newWidth = dungeon.data.Width + num * 2 + a.x;
            int newHeight = Mathf.Max(dungeon.data.Height, a.y + num * 2);
            CellData[][] array = BraveUtility.MultidimensionalArrayResize(dungeon.data.cellData, dungeon.data.Width, dungeon.data.Height, newWidth, newHeight);
            dungeon.data.cellData = array;
            dungeon.data.ClearCachedCellData();
            IntVector2 d = new IntVector2(prototype.Width, prototype.Height);
            IntVector2 b2 = basePosition + b;
            IntVector2 intVector4 = intVector3 + b2;
            CellArea cellArea = new CellArea(intVector4, d, 0);
            cellArea.prototypeRoom = prototype;
            RoomHandler targetRoom = new RoomHandler(cellArea);
            for (int k = -num; k < d.x + num; k++)
            {
                for (int l = -num; l < d.y + num; l++)
                {
                    IntVector2 p = new IntVector2(k, l) + intVector4;
                    if ((k >= 0 && l >= 0 && k < d.x && l < d.y) || array[p.x][p.y] == null)
                    {
                        CellData cellData = new CellData(p, CellType.WALL);
                        cellData.positionInTilemap = cellData.positionInTilemap - intVector3 + new IntVector2(num2, num2);
                        cellData.parentArea = cellArea;
                        cellData.parentRoom = targetRoom;
                        cellData.nearestRoom = targetRoom;
                        cellData.distanceFromNearestRoom = 0f;
                        array[p.x][p.y] = cellData;
                    }
                }
            }
            dungeon.data.rooms.Add(targetRoom);
            try
            {
                targetRoom.WriteRoomData(dungeon.data);
            }
            catch (Exception)
            {
                if (!suppressExceptionMessages)
                {
                    ETGModConsole.Log("WARNING: Exception caused during WriteRoomData step on room: " + targetRoom.GetRoomName());
                }
            }
            try
            {
                dungeon.data.GenerateLightsForRoom(dungeon.decoSettings, targetRoom, GameObject.Find("_Lights").transform, lightStyle);
            }
            catch (Exception)
            {
                if (!suppressExceptionMessages)
                {
                    ETGModConsole.Log("WARNING: Exception caused during GeernateLightsForRoom step on room: " + targetRoom.GetRoomName());
                }
            }

            postProcessCellData?.Invoke(targetRoom);

            if (targetRoom.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.SECRET) { targetRoom.BuildSecretRoomCover(); }
            GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("RuntimeTileMap", ".prefab"));
            tk2dTileMap component = gameObject.GetComponent<tk2dTileMap>();
            string str = UnityEngine.Random.Range(10000, 99999).ToString();
            gameObject.name = "Glitch_" + "RuntimeTilemap_" + str;
            component.renderData.name = "Glitch_" + "RuntimeTilemap_" + str + " Render Data";
            component.Editor__SpriteCollection = dungeon.tileIndices.dungeonCollection;
            try
            {
                TK2DDungeonAssembler.RuntimeResizeTileMap(component, a.x + num2 * 2, a.y + num2 * 2, m_tilemap.partitionSizeX, m_tilemap.partitionSizeY);
                IntVector2 intVector5 = new IntVector2(prototype.Width, prototype.Height);
                IntVector2 b3 = basePosition + b;
                IntVector2 intVector6 = intVector3 + b3;
                for (int num4 = -num2; num4 < intVector5.x + num2; num4++)
                {
                    for (int num5 = -num2; num5 < intVector5.y + num2 + 2; num5++)
                    {
                        assembler.BuildTileIndicesForCell(dungeon, component, intVector6.x + num4, intVector6.y + num5);
                    }
                }
                RenderMeshBuilder.CurrentCellXOffset = intVector3.x - num2;
                RenderMeshBuilder.CurrentCellYOffset = intVector3.y - num2;
                component.ForceBuild();
                RenderMeshBuilder.CurrentCellXOffset = 0;
                RenderMeshBuilder.CurrentCellYOffset = 0;
                component.renderData.transform.position = new Vector3(intVector3.x - num2, intVector3.y - num2, intVector3.y - num2);
            }
            catch (Exception ex)
            {
                if (!suppressExceptionMessages)
                {
                    ETGModConsole.Log("WARNING: Exception occured during RuntimeResizeTileMap / RenderMeshBuilder steps!");
                    Debug.Log("WARNING: Exception occured during RuntimeResizeTileMap/RenderMeshBuilder steps!");
                    Debug.LogException(ex);
                }
            }
            targetRoom.OverrideTilemap = component;
            if (allowProceduralLightFixtures)
            {
                for (int num7 = 0; num7 < targetRoom.area.dimensions.x; num7++)
                {
                    for (int num8 = 0; num8 < targetRoom.area.dimensions.y + 2; num8++)
                    {
                        IntVector2 intVector7 = targetRoom.area.basePosition + new IntVector2(num7, num8);
                        if (dungeon.data.CheckInBoundsAndValid(intVector7))
                        {
                            CellData currentCell = dungeon.data[intVector7];
                            TK2DInteriorDecorator.PlaceLightDecorationForCell(dungeon, component, currentCell, intVector7);
                        }
                    }
                }
            }

            Pathfinder.Instance.InitializeRegion(dungeon.data, targetRoom.area.basePosition + new IntVector2(-3, -3), targetRoom.area.dimensions + new IntVector2(3, 3));

            if (prototype.usesProceduralDecoration && prototype.allowFloorDecoration && allowProceduralDecoration)
            {
                TK2DInteriorDecorator decorator = new TK2DInteriorDecorator(assembler);
                try
                {
                    decorator.HandleRoomDecoration(targetRoom, dungeon, m_tilemap);
                }
                catch (Exception ex)
                {
                    ETGModConsole.Log("WARNING: Exception occured during HandleRoomDecoration steps!");
                    Debug.Log("WARNING: Exception occured during RuntimeResizeTileMap/RenderMeshBuilder steps!");
                    Debug.LogException(ex);
                }
            }

            targetRoom.PostGenerationCleanup();

            if (addRoomToMinimap)
            {
                targetRoom.visibility = RoomHandler.VisibilityStatus.VISITED;
                GameManager.Instance.StartCoroutine(Minimap.Instance.RevealMinimapRoomInternal(targetRoom, true, true, false));
                if (isSecretRatExitRoom) { targetRoom.visibility = RoomHandler.VisibilityStatus.OBSCURED; }
            }
            if (addTeleporter) { targetRoom.AddProceduralTeleporterToRoom(); }
            if (addRoomToMinimap) { Minimap.Instance.InitializeMinimap(dungeon.data); }
            DeadlyDeadlyGoopManager.ReinitializeData();
            return targetRoom;
        }
    }
}
