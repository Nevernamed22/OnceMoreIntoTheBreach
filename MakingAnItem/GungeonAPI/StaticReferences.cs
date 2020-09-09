using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

namespace GungeonAPI
{
	// Token: 0x0200000E RID: 14
	public static class StaticReferences
	{
		// Token: 0x06000072 RID: 114 RVA: 0x000057C8 File Offset: 0x000039C8
		public static void Init()
		{
			StaticReferences.AssetBundles = new Dictionary<string, AssetBundle>();
			foreach (string text in StaticReferences.assetBundleNames)
			{
				try
				{
					AssetBundle assetBundle = ResourceManager.LoadAssetBundle(text);
					StaticReferences.AssetBundles.Add(text, ResourceManager.LoadAssetBundle(text));
				}
				catch (Exception e)
				{
					Tools.PrintError<string>("Failed to load asset bundle: " + text, "FF0000");
					Tools.PrintException(e, "FF0000");
				}
			}
			StaticReferences.RoomTables = new Dictionary<string, GenericRoomTable>();
			foreach (KeyValuePair<string, string> keyValuePair in StaticReferences.roomTableMap)
			{
				try
				{
					GenericRoomTable genericRoomTable = StaticReferences.GetAsset<GenericRoomTable>(keyValuePair.Value);
					bool flag = genericRoomTable == null;
					if (flag)
					{
						genericRoomTable = DungeonDatabase.GetOrLoadByName("base_" + keyValuePair.Key).PatternSettings.flows[0].fallbackRoomTable;
					}
					StaticReferences.RoomTables.Add(keyValuePair.Key, genericRoomTable);
				}
				catch (Exception e2)
				{
					Tools.PrintError<string>("Failed to load room table: " + keyValuePair.Key + ":" + keyValuePair.Value, "FF0000");
					Tools.PrintException(e2, "FF0000");
				}
			}
			Tools.Print<string>("Static references initialized.", "FFFFFF", false);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x0000595C File Offset: 0x00003B5C
		public static GenericRoomTable GetRoomTable(GlobalDungeonData.ValidTilesets tileset)
		{
			if (tileset <= GlobalDungeonData.ValidTilesets.MINEGEON)
			{
				switch (tileset)
				{
					case GlobalDungeonData.ValidTilesets.GUNGEON:
						return StaticReferences.RoomTables["gungeon"];
					case GlobalDungeonData.ValidTilesets.CASTLEGEON:
						return StaticReferences.RoomTables["castle"];
					case GlobalDungeonData.ValidTilesets.GUNGEON | GlobalDungeonData.ValidTilesets.CASTLEGEON:
						break;
					case GlobalDungeonData.ValidTilesets.SEWERGEON:
						return StaticReferences.RoomTables["sewer"];
					default:
						if (tileset == GlobalDungeonData.ValidTilesets.CATHEDRALGEON)
						{
							return StaticReferences.RoomTables["cathedral"];
						}
						if (tileset == GlobalDungeonData.ValidTilesets.MINEGEON)
						{
							return StaticReferences.RoomTables["mines"];
						}
						break;
				}
			}
			else
			{
				if (tileset == GlobalDungeonData.ValidTilesets.CATACOMBGEON)
				{
					return StaticReferences.RoomTables["catacombs"];
				}
				if (tileset == GlobalDungeonData.ValidTilesets.FORGEGEON)
				{
					return StaticReferences.RoomTables["forge"];
				}
				if (tileset == GlobalDungeonData.ValidTilesets.HELLGEON)
				{
					return StaticReferences.RoomTables["bullethell"];
				}
			}
			return StaticReferences.RoomTables["gungeon"];
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00005A64 File Offset: 0x00003C64
		public static T GetAsset<T>(string assetName) where T : UnityEngine.Object
		{
			T t = default(T);
			foreach (AssetBundle assetBundle in StaticReferences.AssetBundles.Values)
			{
				t = assetBundle.LoadAsset<T>(assetName);
				bool flag = t != null;
				if (flag)
				{
					break;
				}
			}
			return t;
		}

		// Token: 0x04000039 RID: 57
		public static Dictionary<string, AssetBundle> AssetBundles;

		// Token: 0x0400003A RID: 58
		public static Dictionary<string, GenericRoomTable> RoomTables;

		// Token: 0x0400003B RID: 59
		public static SharedInjectionData subShopTable;

		// Token: 0x0400003C RID: 60
		public static Dictionary<string, string> roomTableMap = new Dictionary<string, string>
		{
			{
				"special",
				"basic special rooms (shrines, etc)"
			},
			{
				"shop",
				"Shop Room Table"
			},
			{
				"secret",
				"secret_room_table_01"
			},
			{
				"gungeon",
				"Gungeon_RoomTable"
			},
			{
				"castle",
				"Castle_RoomTable"
			},
			{
				"mines",
				"Mines_RoomTable"
			},
			{
				"catacombs",
				"Catacomb_RoomTable"
			},
			{
				"forge",
				"Forge_RoomTable"
			},
			{
				"sewer",
				"Sewer_RoomTable"
			},
			{
				"cathedral",
				"Cathedral_RoomTable"
			},
			{
				"bullethell",
				"BulletHell_RoomTable"
			}
		};

		// Token: 0x0400003D RID: 61
		public static string[] assetBundleNames = new string[]
		{
			"shared_auto_001",
			"shared_auto_002"
		};

		// Token: 0x0400003E RID: 62
		public static string[] dungeonPrefabNames = new string[]
		{
			"base_mines",
			"base_catacombs",
			"base_forge",
			"base_sewer",
			"base_cathedral",
			"base_bullethell"
		};
	}
}
