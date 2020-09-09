using System;
using System.Collections.Generic;
using Dungeonator;

namespace GungeonAPI
{
	// Token: 0x02000003 RID: 3
	public static class DungeonHandler
	{
		// Token: 0x0600000E RID: 14 RVA: 0x00002890 File Offset: 0x00000A90
		public static void Init()
		{
			bool flag = !DungeonHandler.initialized;
			if (flag)
			{
				RoomFactory.LoadRoomsFromRoomDirectory();
				DungeonHooks.OnPreDungeonGeneration += DungeonHandler.OnPreDungeonGen;
				DungeonHandler.initialized = true;
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000028CC File Offset: 0x00000ACC
		public static void OnPreDungeonGen(LoopDungeonGenerator generator, Dungeon dungeon, DungeonFlow flow, int dungeonSeed)
		{
			Tools.Print<string>("Attempting to override floor layout...", "5599FF", false);
			bool flag = flow.name != "Foyer Flow" && !GameManager.IsReturningToFoyerWithPlayer;
			if (flag)
			{
				bool flag2 = DungeonHandler.debugFlow;
				if (flag2)
				{
					generator.AssignFlow(flow);
				}
				Tools.Print<string>("Dungeon name: " + dungeon.name, "FFFFFF", false);
				Tools.Print<string>("Override Flow set to: " + flow.name, "FFFFFF", false);
			}
			dungeon = null;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x0000295C File Offset: 0x00000B5C
		public static void Register(RoomFactory.RoomData roomData)
		{
			PrototypeDungeonRoom room = roomData.room;
			WeightedRoom w = new WeightedRoom
			{
				room = room,
				additionalPrerequisites = new DungeonPrerequisite[0],
				weight = ((roomData.weight == 0f) ? DungeonHandler.GlobalRoomWeight : roomData.weight)
			};
			switch (room.category)
			{
				case PrototypeDungeonRoom.RoomCategory.BOSS:
					return;
				case PrototypeDungeonRoom.RoomCategory.SPECIAL:
					{
						PrototypeDungeonRoom.RoomSpecialSubCategory subCategorySpecial = room.subCategorySpecial;
						PrototypeDungeonRoom.RoomSpecialSubCategory roomSpecialSubCategory = subCategorySpecial;
						if (roomSpecialSubCategory != PrototypeDungeonRoom.RoomSpecialSubCategory.STANDARD_SHOP)
						{
							if (roomSpecialSubCategory != PrototypeDungeonRoom.RoomSpecialSubCategory.WEIRD_SHOP)
							{
								StaticReferences.RoomTables["special"].includedRooms.Add(w);
							}
							else
							{
								StaticReferences.subShopTable.InjectionData.Add(DungeonHandler.GetFlowModifier(roomData));
							}
						}
						else
						{
							StaticReferences.RoomTables["shop"].includedRooms.Add(w);
						}
						return;
					}
				case PrototypeDungeonRoom.RoomCategory.SECRET:
					StaticReferences.RoomTables["secret"].includedRooms.Add(w);
					return;
			}
			List<DungeonPrerequisite> list = new List<DungeonPrerequisite>();
			foreach (DungeonPrerequisite dungeonPrerequisite in room.prerequisites)
			{
				bool requireTileset = dungeonPrerequisite.requireTileset;
				if (requireTileset)
				{
					StaticReferences.GetRoomTable(dungeonPrerequisite.requiredTileset).includedRooms.Add(w);
					list.Add(dungeonPrerequisite);
				}
			}
			foreach (DungeonPrerequisite item in list)
			{
				room.prerequisites.Remove(item);
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002B34 File Offset: 0x00000D34
		public static ProceduralFlowModifierData GetFlowModifier(RoomFactory.RoomData roomData)
		{
			return new ProceduralFlowModifierData
			{
				annotation = roomData.room.name,
				placementRules = new List<ProceduralFlowModifierData.FlowModifierPlacementType>
				{
					ProceduralFlowModifierData.FlowModifierPlacementType.END_OF_CHAIN,
					ProceduralFlowModifierData.FlowModifierPlacementType.HUB_ADJACENT_NO_LINK
				},
				exactRoom = roomData.room,
				selectionWeight = roomData.weight,
				chanceToSpawn = 1f,
				prerequisites = roomData.room.prerequisites.ToArray(),
				CanBeForcedSecret = true
			};
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002BBC File Offset: 0x00000DBC
		public static bool BelongsOnThisFloor(RoomFactory.RoomData data, string dungeonName)
		{
			bool flag = data.floors == null || data.floors.Length == 0;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = false;
				foreach (string text in data.floors)
				{
					bool flag3 = text.ToLower().Equals(dungeonName.ToLower());
					if (flag3)
					{
						flag2 = true;
						break;
					}
				}
				result = flag2;
			}
			return result;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002C30 File Offset: 0x00000E30
		public static GenericRoomTable GetSpecialRoomTable()
		{
			foreach (MetaInjectionDataEntry metaInjectionDataEntry in GameManager.Instance.GlobalInjectionData.entries)
			{
				SharedInjectionData injectionData = metaInjectionDataEntry.injectionData;
				bool flag = ((injectionData != null) ? injectionData.InjectionData : null) != null;
				if (flag)
				{
					foreach (ProceduralFlowModifierData proceduralFlowModifierData in metaInjectionDataEntry.injectionData.InjectionData)
					{
						bool flag2 = proceduralFlowModifierData.roomTable != null && proceduralFlowModifierData.roomTable.name.ToLower().Contains("basic special rooms");
						if (flag2)
						{
							return proceduralFlowModifierData.roomTable;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002D30 File Offset: 0x00000F30
		public static void CollectDataForAnalysis(DungeonFlow flow, Dungeon dungeon)
		{
			try
			{
				foreach (WeightedRoom weightedRoom in flow.fallbackRoomTable.includedRooms.elements)
				{
					string str = "Fallback table: ";
					string str2;
					if (weightedRoom == null)
					{
						str2 = null;
					}
					else
					{
						PrototypeDungeonRoom room = weightedRoom.room;
						str2 = ((room != null) ? room.name : null);
					}
					Tools.Print<string>(str + str2, "FFFFFF", false);
				}
			}
			catch (Exception e)
			{
				Tools.PrintException(e, "FF0000");
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002DE0 File Offset: 0x00000FE0
		public static void LogProtoRoomData(PrototypeDungeonRoom room)
		{
			int num = 0;
			Tools.LogPropertiesAndFields<PrototypeDungeonRoom>(room, "ROOM");
			foreach (PrototypePlacedObjectData prototypePlacedObjectData in room.placedObjects)
			{
				Tools.Log<string>(string.Format("\n----------------Object #{0}----------------", num++));
				Tools.LogPropertiesAndFields<PrototypePlacedObjectData>(prototypePlacedObjectData, "PLACED OBJECT");
				Tools.LogPropertiesAndFields<DungeonPlaceable>((prototypePlacedObjectData != null) ? prototypePlacedObjectData.placeableContents : null, "PLACEABLE CONTENT");
				DungeonPlaceableVariant obj;
				if (prototypePlacedObjectData == null)
				{
					obj = null;
				}
				else
				{
					DungeonPlaceable placeableContents = prototypePlacedObjectData.placeableContents;
					obj = ((placeableContents != null) ? placeableContents.variantTiers[0] : null);
				}
				Tools.LogPropertiesAndFields<DungeonPlaceableVariant>(obj, "VARIANT TIERS");
			}
			Tools.Print<string>("==LAYERS==", "FFFFFF", false);
			foreach (PrototypeRoomObjectLayer prototypeRoomObjectLayer in room.additionalObjectLayers)
			{
			}
		}

		// Token: 0x04000004 RID: 4
		public static float GlobalRoomWeight = 100f;

		// Token: 0x04000005 RID: 5
		private static bool initialized = false;

		// Token: 0x04000006 RID: 6
		public static bool debugFlow = false;
	}
}
