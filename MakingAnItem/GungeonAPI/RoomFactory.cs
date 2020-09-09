using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Dungeonator;
using UnityEngine;

namespace GungeonAPI
{
	// Token: 0x02000008 RID: 8
	public static class RoomFactory
	{
		// Token: 0x0600003A RID: 58 RVA: 0x00003A08 File Offset: 0x00001C08
		public static void LoadRoomsFromRoomDirectory()
		{
			Directory.CreateDirectory(RoomFactory.roomDirectory);
			foreach (string text in Directory.GetFiles(RoomFactory.roomDirectory))
			{
				bool flag = !text.EndsWith(".room");
				if (!flag)
				{
					string fileName = Path.GetFileName(text);
					Tools.Log<string>("Found room: \"" + fileName + "\"");
					RoomFactory.RoomData roomData = RoomFactory.BuildFromFile(text);
					DungeonHandler.Register(roomData);
					RoomFactory.rooms.Add(fileName, roomData);
				}
			}
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003A94 File Offset: 0x00001C94
		public static RoomFactory.RoomData BuildFromFile(string roomPath)
		{
			Texture2D textureFromFile = ResourceExtractor.GetTextureFromFile(roomPath, ".room");
			textureFromFile.name = Path.GetFileName(roomPath);
			RoomFactory.RoomData roomData = RoomFactory.ExtractRoomDataFromFile(roomPath);
			roomData.room = RoomFactory.Build(textureFromFile, roomData);
			return roomData;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00003AD8 File Offset: 0x00001CD8
		public static RoomFactory.RoomData BuildFromResource(string roomPath)
		{
			Texture2D textureFromResource = ResourceExtractor.GetTextureFromResource(roomPath);
			RoomFactory.RoomData roomData = RoomFactory.ExtractRoomDataFromResource(roomPath);
			roomData.room = RoomFactory.Build(textureFromResource, roomData);
			return roomData;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00003B08 File Offset: 0x00001D08
		public static PrototypeDungeonRoom Build(Texture2D texture, RoomFactory.RoomData roomData)
		{
			try
			{
				PrototypeDungeonRoom prototypeDungeonRoom = RoomFactory.CreateRoomFromTexture(texture);
				RoomFactory.ApplyRoomData(prototypeDungeonRoom, roomData);
				prototypeDungeonRoom.UpdatePrecalculatedData();
				return prototypeDungeonRoom;
			}
			catch (Exception e)
			{
				Tools.PrintError<string>("Failed to build room!", "FF0000");
				Tools.PrintException(e, "FF0000");
			}
			return RoomFactory.CreateEmptyRoom(12, 12);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00003B70 File Offset: 0x00001D70
		public static void ApplyRoomData(PrototypeDungeonRoom room, RoomFactory.RoomData roomData)
		{
			bool flag = roomData.exitPositions != null;
			if (flag)
			{
				for (int i = 0; i < roomData.exitPositions.Length; i++)
				{
					DungeonData.Direction direction = (DungeonData.Direction)Enum.Parse(typeof(DungeonData.Direction), roomData.exitDirections[i].ToUpper());
					RoomFactory.AddExit(room, roomData.exitPositions[i], direction);
				}
			}
			else
			{
				RoomFactory.AddExit(room, new Vector2((float)(room.Width / 2), (float)room.Height), DungeonData.Direction.NORTH);
				RoomFactory.AddExit(room, new Vector2((float)(room.Width / 2), 0f), DungeonData.Direction.SOUTH);
				RoomFactory.AddExit(room, new Vector2((float)room.Width, (float)(room.Height / 2)), DungeonData.Direction.EAST);
				RoomFactory.AddExit(room, new Vector2(0f, (float)(room.Height / 2)), DungeonData.Direction.WEST);
			}
			bool flag2 = roomData.enemyPositions != null;
			if (flag2)
			{
				for (int j = 0; j < roomData.enemyPositions.Length; j++)
				{
					RoomFactory.AddEnemyToRoom(room, roomData.enemyPositions[j], roomData.enemyGUIDs[j], roomData.enemyReinforcementLayers[j]);
				}
			}
			bool flag3 = roomData.placeablePositions != null;
			if (flag3)
			{
				for (int k = 0; k < roomData.placeablePositions.Length; k++)
				{
					RoomFactory.AddPlaceableToRoom(room, roomData.placeablePositions[k], roomData.placeableGUIDs[k]);
				}
			}
			bool flag4 = roomData.floors != null;
			if (flag4)
			{
				foreach (string val in roomData.floors)
				{
					room.prerequisites.Add(new DungeonPrerequisite
					{
						prerequisiteType = DungeonPrerequisite.PrerequisiteType.TILESET,
						requiredTileset = Tools.GetEnumValue<GlobalDungeonData.ValidTilesets>(val)
					});
				}
			}
			bool flag5 = !string.IsNullOrEmpty(roomData.category);
			if (flag5)
			{
				room.category = Tools.GetEnumValue<PrototypeDungeonRoom.RoomCategory>(roomData.category);
			}
			bool flag6 = !string.IsNullOrEmpty(roomData.normalSubCategory);
			if (flag6)
			{
				room.subCategoryNormal = Tools.GetEnumValue<PrototypeDungeonRoom.RoomNormalSubCategory>(roomData.normalSubCategory);
			}
			bool flag7 = !string.IsNullOrEmpty(roomData.bossSubCategory);
			if (flag7)
			{
				room.subCategoryBoss = Tools.GetEnumValue<PrototypeDungeonRoom.RoomBossSubCategory>(roomData.bossSubCategory);
			}
			bool flag8 = !string.IsNullOrEmpty(roomData.specialSubCatergory);
			if (flag8)
			{
				room.subCategorySpecial = Tools.GetEnumValue<PrototypeDungeonRoom.RoomSpecialSubCategory>(roomData.specialSubCatergory);
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00003DE4 File Offset: 0x00001FE4
		public static RoomFactory.RoomData ExtractRoomDataFromFile(string path)
		{
			string data = ResourceExtractor.BytesToString(File.ReadAllBytes(path));
			return RoomFactory.ExtractRoomData(data);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00003E08 File Offset: 0x00002008
		public static RoomFactory.RoomData ExtractRoomDataFromResource(string path)
		{
			string data = ResourceExtractor.BytesToString(ResourceExtractor.ExtractEmbeddedResource(path));
			return RoomFactory.ExtractRoomData(data);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003E2C File Offset: 0x0000202C
		public static RoomFactory.RoomData ExtractRoomData(string data)
		{
			bool flag = data.Contains(RoomFactory.dataHeader);
			RoomFactory.RoomData result;
			if (flag)
			{
				string text = data.Substring(data.IndexOf(RoomFactory.dataHeader) + RoomFactory.dataHeader.Length);
				result = JsonUtility.FromJson<RoomFactory.RoomData>(text);
			}
			else
			{
				result = default(RoomFactory.RoomData);
			}
			return result;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00003E80 File Offset: 0x00002080
		public static PrototypeDungeonRoom CreateRoomFromTexture(Texture2D texture)
		{
			int width = texture.width;
			int height = texture.height;
			PrototypeDungeonRoom newPrototypeDungeonRoom = RoomFactory.GetNewPrototypeDungeonRoom(width, height);
			PrototypeDungeonRoomCellData[] array = RoomFactory.m_cellData.GetValue(newPrototypeDungeonRoom) as PrototypeDungeonRoomCellData[];
			array = new PrototypeDungeonRoomCellData[width * height];
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					array[i + j * width] = RoomFactory.CellDataFromColor(texture.GetPixel(i, j));
				}
			}
			RoomFactory.m_cellData.SetValue(newPrototypeDungeonRoom, array);
			newPrototypeDungeonRoom.name = texture.name;
			return newPrototypeDungeonRoom;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00003F2C File Offset: 0x0000212C
		public static PrototypeDungeonRoomCellData CellDataFromColor(Color32 color)
		{
			bool flag = color.Equals(Color.magenta);
			PrototypeDungeonRoomCellData result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = new PrototypeDungeonRoomCellData
				{
					state = RoomFactory.TypeFromColor(color),
					diagonalWallType = RoomFactory.DiagonalWallTypeFromColor(color),
					appearance = new PrototypeDungeonRoomCellAppearance
					{
						OverrideFloorType = CellVisualData.CellFloorType.Stone
					}
				};
			}
			return result;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00003F98 File Offset: 0x00002198
		public static CellType TypeFromColor(Color color)
		{
			bool flag = color == Color.black;
			CellType result;
			if (flag)
			{
				result = CellType.PIT;
			}
			else
			{
				bool flag2 = color == Color.white;
				if (flag2)
				{
					result = CellType.FLOOR;
				}
				else
				{
					result = CellType.WALL;
				}
			}
			return result;
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00003FD4 File Offset: 0x000021D4
		public static DiagonalWallType DiagonalWallTypeFromColor(Color color)
		{
			bool flag = color == Color.red;
			DiagonalWallType result;
			if (flag)
			{
				result = DiagonalWallType.NORTHEAST;
			}
			else
			{
				bool flag2 = color == Color.green;
				if (flag2)
				{
					result = DiagonalWallType.SOUTHEAST;
				}
				else
				{
					bool flag3 = color == Color.blue;
					if (flag3)
					{
						result = DiagonalWallType.SOUTHWEST;
					}
					else
					{
						bool flag4 = color == Color.yellow;
						if (flag4)
						{
							result = DiagonalWallType.NORTHWEST;
						}
						else
						{
							result = DiagonalWallType.NONE;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00004038 File Offset: 0x00002238
		public static RoomFactory.RoomData CreateEmptyRoomData(int width = 12, int height = 12)
		{
			return new RoomFactory.RoomData
			{
				room = RoomFactory.CreateEmptyRoom(width, height),
				category = "NORMAL",
				weight = DungeonHandler.GlobalRoomWeight
			};
		}

		// Token: 0x06000047 RID: 71 RVA: 0x0000407C File Offset: 0x0000227C
		public static PrototypeDungeonRoom CreateEmptyRoom(int width = 12, int height = 12)
		{
			PrototypeDungeonRoom result;
			try
			{
				PrototypeDungeonRoom newPrototypeDungeonRoom = RoomFactory.GetNewPrototypeDungeonRoom(width, height);
				RoomFactory.AddExit(newPrototypeDungeonRoom, new Vector2((float)(width / 2), (float)height), DungeonData.Direction.NORTH);
				RoomFactory.AddExit(newPrototypeDungeonRoom, new Vector2((float)(width / 2), 0f), DungeonData.Direction.SOUTH);
				RoomFactory.AddExit(newPrototypeDungeonRoom, new Vector2((float)width, (float)(height / 2)), DungeonData.Direction.EAST);
				RoomFactory.AddExit(newPrototypeDungeonRoom, new Vector2(0f, (float)(height / 2)), DungeonData.Direction.WEST);
				PrototypeDungeonRoomCellData[] array = RoomFactory.m_cellData.GetValue(newPrototypeDungeonRoom) as PrototypeDungeonRoomCellData[];
				array = new PrototypeDungeonRoomCellData[width * height];
				for (int i = 0; i < width; i++)
				{
					for (int j = 0; j < height; j++)
					{
						array[i + j * width] = new PrototypeDungeonRoomCellData
						{
							state = CellType.FLOOR,
							appearance = new PrototypeDungeonRoomCellAppearance
							{
								OverrideFloorType = CellVisualData.CellFloorType.Stone
							}
						};
					}
				}
				RoomFactory.m_cellData.SetValue(newPrototypeDungeonRoom, array);
				newPrototypeDungeonRoom.UpdatePrecalculatedData();
				result = newPrototypeDungeonRoom;
			}
			catch (Exception e)
			{
				Tools.PrintException(e, "FF0000");
				result = null;
			}
			return result;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x0000418C File Offset: 0x0000238C
		public static int GetStyleValue(string dungeonName, string shrineID)
		{
			bool flag = ShrineFactory.registeredShrines != null && ShrineFactory.registeredShrines.ContainsKey(shrineID);
			if (flag)
			{
				GameObject gameObject = ShrineFactory.registeredShrines[shrineID];
				ShrineFactory.CustomShrineController customShrineController = (gameObject != null) ? gameObject.GetComponent<ShrineFactory.CustomShrineController>() : null;
				bool flag2 = customShrineController != null && customShrineController.roomStyles != null && customShrineController.roomStyles.ContainsKey(dungeonName);
				if (flag2)
				{
					return customShrineController.roomStyles[dungeonName];
				}
			}
			return -1;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00004208 File Offset: 0x00002408
		public static void AddPlaceableToRoom(PrototypeDungeonRoom room, Vector2 location, string assetPath)
		{
			try
			{
				GameObject placeableFromBundles = RoomFactory.GetPlaceableFromBundles(assetPath);
				bool flag = placeableFromBundles;
				if (flag)
				{
					DungeonPrerequisite[] array = new DungeonPrerequisite[0];
					room.placedObjectPositions.Add(location);
					room.placedObjects.Add(new PrototypePlacedObjectData
					{
						contentsBasePosition = location,
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
									nonDatabasePlaceable = placeableFromBundles,
									prerequisites = array,
									materialRequirements = new DungeonPlaceableRoomMaterialRequirement[0]
								}
							}
						}
					});
				}
				else
				{
					Tools.PrintError<string>("Unable to find asset in asset bundles: " + assetPath, "FF0000");
				}
			}
			catch (Exception e)
			{
				Tools.PrintException(e, "FF0000");
			}
		}

		// Token: 0x0600004A RID: 74 RVA: 0x0000432C File Offset: 0x0000252C
		public static GameObject GetPlaceableFromBundles(string assetPath)
		{
			GameObject gameObject = null;
			foreach (AssetBundle assetBundle in RoomFactory.assetBundles)
			{
				gameObject = (assetBundle.LoadAsset(assetPath) as GameObject);
				bool flag = gameObject;
				if (flag)
				{
					break;
				}
			}
			return gameObject;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x0000437C File Offset: 0x0000257C
		public static void AddEnemyToRoom(PrototypeDungeonRoom room, Vector2 location, string guid, int layer)
		{
			DungeonPrerequisite[] array = new DungeonPrerequisite[0];
			DungeonPlaceable dungeonPlaceable = ScriptableObject.CreateInstance<DungeonPlaceable>();
			dungeonPlaceable.width = 1;
			dungeonPlaceable.height = 1;
			dungeonPlaceable.respectsEncounterableDifferentiator = true;
			dungeonPlaceable.variantTiers = new List<DungeonPlaceableVariant>
			{
				new DungeonPlaceableVariant
				{
					percentChance = 1f,
					prerequisites = array,
					enemyPlaceableGuid = guid,
					materialRequirements = new DungeonPlaceableRoomMaterialRequirement[0]
				}
			};
			PrototypePlacedObjectData prototypePlacedObjectData = new PrototypePlacedObjectData
			{
				contentsBasePosition = location,
				fieldData = new List<PrototypePlacedObjectFieldData>(),
				instancePrerequisites = array,
				linkedTriggerAreaIDs = new List<int>(),
				placeableContents = dungeonPlaceable
			};
			bool flag = layer > 0;
			if (flag)
			{
				RoomFactory.AddObjectDataToReinforcementLayer(room, prototypePlacedObjectData, layer - 1, location);
			}
			else
			{
				room.placedObjects.Add(prototypePlacedObjectData);
				room.placedObjectPositions.Add(location);
			}
			bool flag2 = !room.roomEvents.Contains(RoomFactory.sealOnEnterWithEnemies);
			if (flag2)
			{
				room.roomEvents.Add(RoomFactory.sealOnEnterWithEnemies);
			}
			bool flag3 = !room.roomEvents.Contains(RoomFactory.unsealOnRoomClear);
			if (flag3)
			{
				room.roomEvents.Add(RoomFactory.unsealOnRoomClear);
			}
		}

		// Token: 0x0600004C RID: 76 RVA: 0x000044AC File Offset: 0x000026AC
		public static void AddObjectDataToReinforcementLayer(PrototypeDungeonRoom room, PrototypePlacedObjectData objectData, int layer, Vector2 location)
		{
			bool flag = room.additionalObjectLayers.Count <= layer;
			if (flag)
			{
				for (int i = room.additionalObjectLayers.Count; i <= layer; i++)
				{
					PrototypeRoomObjectLayer item = new PrototypeRoomObjectLayer
					{
						layerIsReinforcementLayer = true,
						placedObjects = new List<PrototypePlacedObjectData>(),
						placedObjectBasePositions = new List<Vector2>()
					};
					room.additionalObjectLayers.Add(item);
				}
			}
			room.additionalObjectLayers[layer].placedObjects.Add(objectData);
			room.additionalObjectLayers[layer].placedObjectBasePositions.Add(location);
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00004550 File Offset: 0x00002750
		public static void AddExit(PrototypeDungeonRoom room, Vector2 location, DungeonData.Direction direction)
		{
			bool flag = room.exitData == null;
			if (flag)
			{
				room.exitData = new PrototypeRoomExitData();
			}
			bool flag2 = room.exitData.exits == null;
			if (flag2)
			{
				room.exitData.exits = new List<PrototypeRoomExit>();
			}
			PrototypeRoomExit prototypeRoomExit = new PrototypeRoomExit(direction, location);
			prototypeRoomExit.exitType = PrototypeRoomExit.ExitType.NO_RESTRICTION;
			Vector2 b = (direction == DungeonData.Direction.EAST || direction == DungeonData.Direction.WEST) ? new Vector2(0f, 1f) : new Vector2(1f, 0f);
			prototypeRoomExit.containedCells.Add(location + b);
			room.exitData.exits.Add(prototypeRoomExit);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x000045F8 File Offset: 0x000027F8
		public static PrototypeDungeonRoom GetNewPrototypeDungeonRoom(int width = 12, int height = 12)
		{
			PrototypeDungeonRoom prototypeDungeonRoom = ScriptableObject.CreateInstance<PrototypeDungeonRoom>();
			prototypeDungeonRoom.injectionFlags = new RuntimeInjectionFlags();
			prototypeDungeonRoom.RoomId = UnityEngine.Random.Range(10000, 1000000);
			prototypeDungeonRoom.pits = new List<PrototypeRoomPitEntry>();
			prototypeDungeonRoom.placedObjects = new List<PrototypePlacedObjectData>();
			prototypeDungeonRoom.placedObjectPositions = new List<Vector2>();
			prototypeDungeonRoom.additionalObjectLayers = new List<PrototypeRoomObjectLayer>();
			prototypeDungeonRoom.eventTriggerAreas = new List<PrototypeEventTriggerArea>();
			prototypeDungeonRoom.roomEvents = new List<RoomEventDefinition>();
			prototypeDungeonRoom.paths = new List<SerializedPath>();
			prototypeDungeonRoom.prerequisites = new List<DungeonPrerequisite>();
			prototypeDungeonRoom.excludedOtherRooms = new List<PrototypeDungeonRoom>();
			prototypeDungeonRoom.rectangularFeatures = new List<PrototypeRectangularFeature>();
			prototypeDungeonRoom.exitData = new PrototypeRoomExitData();
			prototypeDungeonRoom.exitData.exits = new List<PrototypeRoomExit>();
			prototypeDungeonRoom.allowWallDecoration = false;
			prototypeDungeonRoom.allowFloorDecoration = false;
			prototypeDungeonRoom.Width = width;
			prototypeDungeonRoom.Height = height;
			return prototypeDungeonRoom;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x000046D8 File Offset: 0x000028D8
		public static void LogExampleRoomData()
		{
			Vector2[] enemyPositions = new Vector2[]
			{
				new Vector2(4f, 4f),
				new Vector2(4f, 14f),
				new Vector2(14f, 4f),
				new Vector2(14f, 14f)
			};
			string[] enemyGUIDs = new string[]
			{
				"01972dee89fc4404a5c408d50007dad5",
				"7b0b1b6d9ce7405b86b75ce648025dd6",
				"ffdc8680bdaa487f8f31995539f74265",
				"01972dee89fc4404a5c408d50007dad5"
			};
			Vector2[] exitPositions = new Vector2[]
			{
				new Vector2(0f, 9f),
				new Vector2(9f, 0f),
				new Vector2(20f, 9f),
				new Vector2(9f, 20f)
			};
			string[] exitDirections = new string[]
			{
				"EAST",
				"SOUTH",
				"WEST",
				"NORTH"
			};
			RoomFactory.RoomData roomData = new RoomFactory.RoomData
			{
				enemyPositions = enemyPositions,
				enemyGUIDs = enemyGUIDs,
				exitPositions = exitPositions,
				exitDirections = exitDirections
			};
			Tools.Print<string>("Data to JSON: " + JsonUtility.ToJson(roomData), "FFFFFF", false);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00004840 File Offset: 0x00002A40
		public static void StraightLine()
		{
			try
			{
				Vector2[] array = new Vector2[100];
				string[] array2 = new string[100];
				int[] array3 = new int[100];
				for (int i = 0; i < array2.Length; i++)
				{
					List<EnemyDatabaseEntry> entries = EnemyDatabase.Instance.Entries;
					int index = UnityEngine.Random.Range(0, entries.Count);
					array2[i] = entries[index].encounterGuid;
					array[i] = new Vector2((float)(i * 2), 10f);
					array3[i] = 0;
				}
				Vector2[] exitPositions = new Vector2[]
				{
					new Vector2(0f, 9f),
					new Vector2(200f, 9f)
				};
				string[] exitDirections = new string[]
				{
					"WEST",
					"EAST"
				};
				RoomFactory.RoomData roomData = new RoomFactory.RoomData
				{
					enemyPositions = array,
					enemyGUIDs = array2,
					enemyReinforcementLayers = array3,
					exitPositions = exitPositions,
					exitDirections = exitDirections
				};
				Tools.Log<string>("Data to JSON: " + JsonUtility.ToJson(roomData));
			}
			catch (Exception e)
			{
				Tools.PrintException(e, "FF0000");
			}
		}

		// Token: 0x04000010 RID: 16
		public static Dictionary<string, RoomFactory.RoomData> rooms = new Dictionary<string, RoomFactory.RoomData>();

		// Token: 0x04000011 RID: 17
		public static string roomDirectory = Path.Combine(ETGMod.GameFolder, "CustomRoomData");

		// Token: 0x04000012 RID: 18
		public static AssetBundle[] assetBundles = new AssetBundle[]
		{
			ResourceManager.LoadAssetBundle("shared_auto_001"),
			ResourceManager.LoadAssetBundle("shared_auto_002")
		};

		// Token: 0x04000013 RID: 19
		private static readonly string dataHeader = "***DATA***";

		// Token: 0x04000014 RID: 20
		private static FieldInfo m_cellData = typeof(PrototypeDungeonRoom).GetField("m_cellData", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04000015 RID: 21
		private static RoomEventDefinition sealOnEnterWithEnemies = new RoomEventDefinition(RoomEventTriggerCondition.ON_ENTER_WITH_ENEMIES, RoomEventTriggerAction.SEAL_ROOM);

		// Token: 0x04000016 RID: 22
		private static RoomEventDefinition unsealOnRoomClear = new RoomEventDefinition(RoomEventTriggerCondition.ON_ENEMIES_CLEARED, RoomEventTriggerAction.UNSEAL_ROOM);

		// Token: 0x0200001F RID: 31
		public struct RoomData
		{
			// Token: 0x04000081 RID: 129
			public string category;

			// Token: 0x04000082 RID: 130
			public string normalSubCategory;

			// Token: 0x04000083 RID: 131
			public string specialSubCatergory;

			// Token: 0x04000084 RID: 132
			public string bossSubCategory;

			// Token: 0x04000085 RID: 133
			public Vector2[] enemyPositions;

			// Token: 0x04000086 RID: 134
			public string[] enemyGUIDs;

			// Token: 0x04000087 RID: 135
			public Vector2[] placeablePositions;

			// Token: 0x04000088 RID: 136
			public string[] placeableGUIDs;

			// Token: 0x04000089 RID: 137
			public int[] enemyReinforcementLayers;

			// Token: 0x0400008A RID: 138
			public Vector2[] exitPositions;

			// Token: 0x0400008B RID: 139
			public string[] exitDirections;

			// Token: 0x0400008C RID: 140
			public string[] floors;

			// Token: 0x0400008D RID: 141
			public float weight;

			// Token: 0x0400008E RID: 142
			public bool isSpecialRoom;

			// Token: 0x0400008F RID: 143
			[NonSerialized]
			public PrototypeDungeonRoom room;
		}
	}
}
