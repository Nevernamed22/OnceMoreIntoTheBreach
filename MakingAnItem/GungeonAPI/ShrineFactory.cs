using System;
using System.Collections.Generic;
using Dungeonator;
using ItemAPI;
using UnityEngine;

namespace GungeonAPI
{
	// Token: 0x0200000A RID: 10
	public class ShrineFactory
	{
		// Token: 0x06000058 RID: 88 RVA: 0x00004D94 File Offset: 0x00002F94
		public static void Init()
		{
			bool initialized = ShrineFactory.m_initialized;
			if (!initialized)
			{
				DungeonHooks.OnFoyerAwake += ShrineFactory.PlaceBreachShrines;
				DungeonHooks.OnPreDungeonGeneration += delegate (LoopDungeonGenerator generator, Dungeon dungeon, DungeonFlow flow, int dungeonSeed)
				{
					bool flag = flow.name != "Foyer Flow" && !GameManager.IsReturningToFoyerWithPlayer;
					if (flag)
					{
						ShrineFactory.CleanupBreachShrines();
					}
				};
				ShrineFactory.m_initialized = true;
			}
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00004DEC File Offset: 0x00002FEC
		public GameObject Build()
		{
			GameObject result;
			try
			{
				Texture2D textureFromResource = ResourceExtractor.GetTextureFromResource(this.spritePath);
				GameObject gameObject = SpriteBuilder.SpriteFromResource(this.spritePath, null, false);
				string text = (this.modID + ":" + this.name).ToLower().Replace(" ", "_");
				gameObject.name = text;
				tk2dSprite component = gameObject.GetComponent<tk2dSprite>();
				component.IsPerpendicular = true;
				component.PlaceAtPositionByAnchor(this.offset, tk2dBaseSprite.Anchor.LowerCenter);
				Transform transform = new GameObject("talkpoint").transform;
				transform.position = gameObject.transform.position + this.talkPointOffset;
				transform.SetParent(gameObject.transform);
				bool flag = !this.usesCustomColliderOffsetAndSize;
				if (flag)
				{
					IntVector2 intVector = new IntVector2(textureFromResource.width, textureFromResource.height);
					this.colliderOffset = new IntVector2(0, 0);
					this.colliderSize = new IntVector2(intVector.x, intVector.y / 2);
				}
				SpeculativeRigidbody speculativeRigidbody = component.SetUpSpeculativeRigidbody(this.colliderOffset, this.colliderSize);
				ShrineFactory.CustomShrineController customShrineController = gameObject.AddComponent<ShrineFactory.CustomShrineController>();
				customShrineController.ID = text;
				customShrineController.roomStyles = this.roomStyles;
				customShrineController.isBreachShrine = true;
				customShrineController.offset = this.offset;
				customShrineController.pixelColliders = speculativeRigidbody.specRigidbody.PixelColliders;
				customShrineController.factory = this;
				customShrineController.OnAccept = this.OnAccept;
				customShrineController.OnDecline = this.OnDecline;
				customShrineController.CanUse = this.CanUse;
				customShrineController.text = this.text;
				customShrineController.acceptText = this.acceptText;
				customShrineController.declineText = this.declineText;
				bool flag2 = this.interactableComponent == null;
				if (flag2)
				{
					SimpleShrine simpleShrine = gameObject.AddComponent<SimpleShrine>();
					simpleShrine.isToggle = this.isToggle;
					simpleShrine.OnAccept = this.OnAccept;
					simpleShrine.OnDecline = this.OnDecline;
					simpleShrine.CanUse = this.CanUse;
					simpleShrine.text = this.text;
					simpleShrine.acceptText = this.acceptText;
					simpleShrine.declineText = this.declineText;
					simpleShrine.talkPoint = transform;
				}
				else
				{
					gameObject.AddComponent(this.interactableComponent);
				}
				gameObject.name = text;
				bool flag3 = !this.isBreachShrine;
				if (flag3)
				{
					bool flag4 = !this.room;
					if (flag4)
					{
						this.room = RoomFactory.CreateEmptyRoom(12, 12);
					}
					ShrineFactory.RegisterShrineRoom(gameObject, this.room, text, this.offset);
				}
				ShrineFactory.registeredShrines.Add(text, gameObject);
				FakePrefab.MarkAsFakePrefab(gameObject);
				Tools.Print<string>("Added shrine: " + text, "FFFFFF", false);
				result = gameObject;
			}
			catch (Exception e)
			{
				Tools.PrintException(e, "FF0000");
				result = null;
			}
			return result;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x000050E4 File Offset: 0x000032E4
		public static void RegisterShrineRoom(GameObject shrine, PrototypeDungeonRoom protoroom, string ID, Vector2 offset)
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
							nonDatabasePlaceable = shrine,
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
				specialSubCatergory = "UNSPECIFIED_SPECIAL"
			};
			RoomFactory.rooms.Add(ID, roomData);
			DungeonHandler.Register(roomData);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00005224 File Offset: 0x00003424
		public static void PlaceBreachShrines()
		{
			ShrineFactory.CleanupBreachShrines();
			Tools.Print<string>("Placing breach shrines: ", "FFFFFF", false);
			foreach (GameObject gameObject in ShrineFactory.registeredShrines.Values)
			{
				try
				{
					ShrineFactory.CustomShrineController component = gameObject.GetComponent<ShrineFactory.CustomShrineController>();
					bool flag = !component.isBreachShrine;
					if (!flag)
					{
						Tools.Print<string>("    " + gameObject.name, "FFFFFF", false);
						ShrineFactory.CustomShrineController component2 = UnityEngine.Object.Instantiate<GameObject>(gameObject).GetComponent<ShrineFactory.CustomShrineController>();
						component2.Copy(component);
						component2.gameObject.SetActive(true);
						component2.sprite.PlaceAtPositionByAnchor(component2.offset, tk2dBaseSprite.Anchor.LowerCenter);
						SpriteOutlineManager.AddOutlineToSprite(component2.sprite, Color.black);
						IPlayerInteractable component3 = component2.GetComponent<IPlayerInteractable>();
						bool flag2 = component3 is SimpleInteractable;
						if (flag2)
						{
							((SimpleInteractable)component3).OnAccept = component2.OnAccept;
							((SimpleInteractable)component3).OnDecline = component2.OnDecline;
							((SimpleInteractable)component3).CanUse = component2.CanUse;
						}
						bool flag3 = !RoomHandler.unassignedInteractableObjects.Contains(component3);
						if (flag3)
						{
							RoomHandler.unassignedInteractableObjects.Add(component3);
						}
					}
				}
				catch (Exception e)
				{
					Tools.PrintException(e, "FF0000");
				}
			}
		}

		// Token: 0x0600005C RID: 92 RVA: 0x000053C0 File Offset: 0x000035C0
		private static void CleanupBreachShrines()
		{
			foreach (ShrineFactory.CustomShrineController customShrineController in UnityEngine.Object.FindObjectsOfType<ShrineFactory.CustomShrineController>())
			{
				bool flag = !FakePrefab.IsFakePrefab(customShrineController);
				if (flag)
				{
					UnityEngine.Object.Destroy(customShrineController.gameObject);
				}
				else
				{
					customShrineController.gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x04000017 RID: 23
		public string name;

		// Token: 0x04000018 RID: 24
		public string modID;

		// Token: 0x04000019 RID: 25
		public string spritePath;

		// Token: 0x0400001A RID: 26
		public string shadowSpritePath;

		// Token: 0x0400001B RID: 27
		public string text;

		// Token: 0x0400001C RID: 28
		public string acceptText;

		// Token: 0x0400001D RID: 29
		public string declineText;

		// Token: 0x0400001E RID: 30
		public Action<PlayerController, GameObject> OnAccept;

		// Token: 0x0400001F RID: 31
		public Action<PlayerController, GameObject> OnDecline;

		// Token: 0x04000020 RID: 32
		public Func<PlayerController, GameObject, bool> CanUse;

		// Token: 0x04000021 RID: 33
		public Vector3 talkPointOffset;

		// Token: 0x04000022 RID: 34
		public Vector3 offset = new Vector3(43.8f, 42.4f, 42.9f);

		// Token: 0x04000023 RID: 35
		public IntVector2 colliderOffset;

		// Token: 0x04000024 RID: 36
		public IntVector2 colliderSize;

		// Token: 0x04000025 RID: 37
		public bool isToggle;

		// Token: 0x04000026 RID: 38
		public bool usesCustomColliderOffsetAndSize;

		// Token: 0x04000027 RID: 39
		public Type interactableComponent = null;

		// Token: 0x04000028 RID: 40
		public bool isBreachShrine = false;

		// Token: 0x04000029 RID: 41
		public PrototypeDungeonRoom room;

		// Token: 0x0400002A RID: 42
		public Dictionary<string, int> roomStyles;

		// Token: 0x0400002B RID: 43
		public static Dictionary<string, GameObject> registeredShrines = new Dictionary<string, GameObject>();

		// Token: 0x0400002C RID: 44
		private static bool m_initialized;

		// Token: 0x02000021 RID: 33
		public class CustomShrineController : DungeonPlaceableBehaviour
		{
			// Token: 0x060000EC RID: 236 RVA: 0x0000B5E4 File Offset: 0x000097E4
			private void Start()
			{
				string text = base.name.Replace("(Clone)", "");
				bool flag = ShrineFactory.registeredShrines.ContainsKey(text);
				if (flag)
				{
					this.Copy(ShrineFactory.registeredShrines[text].GetComponent<ShrineFactory.CustomShrineController>());
				}
				else
				{
					Tools.PrintError<string>("Was this shrine registered correctly?: " + text, "FF0000");
				}
				SimpleInteractable component = base.GetComponent<SimpleInteractable>();
				bool flag2 = !component;
				if (!flag2)
				{
					component.OnAccept = this.OnAccept;
					component.OnDecline = this.OnDecline;
					component.CanUse = this.CanUse;
					component.text = this.text;
					component.acceptText = this.acceptText;
					component.declineText = this.declineText;
					Tools.Print<string>("Started shrine: " + text, "FFFFFF", false);
				}
			}

			// Token: 0x060000ED RID: 237 RVA: 0x0000B6BC File Offset: 0x000098BC
			public void Copy(ShrineFactory.CustomShrineController other)
			{
				this.ID = other.ID;
				this.roomStyles = other.roomStyles;
				this.isBreachShrine = other.isBreachShrine;
				this.offset = other.offset;
				this.pixelColliders = other.pixelColliders;
				this.factory = other.factory;
				this.OnAccept = other.OnAccept;
				this.OnDecline = other.OnDecline;
				this.CanUse = other.CanUse;
				this.text = other.text;
				this.acceptText = other.acceptText;
				this.declineText = other.declineText;
			}

			// Token: 0x060000EE RID: 238 RVA: 0x0000B75A File Offset: 0x0000995A
			public void ConfigureOnPlacement(RoomHandler room)
			{
				this.m_parentRoom = room;
				this.RegisterMinimapIcon();
			}

			// Token: 0x060000EF RID: 239 RVA: 0x0000B76B File Offset: 0x0000996B
			public void RegisterMinimapIcon()
			{
				this.m_instanceMinimapIcon = Minimap.Instance.RegisterRoomIcon(this.m_parentRoom, (GameObject)BraveResources.Load("Global Prefabs/Minimap_Shrine_Icon", ".prefab"), false);
			}

			// Token: 0x060000F0 RID: 240 RVA: 0x0000B79C File Offset: 0x0000999C
			public void GetRidOfMinimapIcon()
			{
				bool flag = this.m_instanceMinimapIcon != null;
				if (flag)
				{
					Minimap.Instance.DeregisterRoomIcon(this.m_parentRoom, this.m_instanceMinimapIcon);
					this.m_instanceMinimapIcon = null;
				}
			}

			// Token: 0x04000098 RID: 152
			public string ID;

			// Token: 0x04000099 RID: 153
			public bool isBreachShrine;

			// Token: 0x0400009A RID: 154
			public Vector3 offset;

			// Token: 0x0400009B RID: 155
			public List<PixelCollider> pixelColliders;

			// Token: 0x0400009C RID: 156
			public Dictionary<string, int> roomStyles;

			// Token: 0x0400009D RID: 157
			public ShrineFactory factory;

			// Token: 0x0400009E RID: 158
			public Action<PlayerController, GameObject> OnAccept;

			// Token: 0x0400009F RID: 159
			public Action<PlayerController, GameObject> OnDecline;

			// Token: 0x040000A0 RID: 160
			public Func<PlayerController, GameObject, bool> CanUse;

			// Token: 0x040000A1 RID: 161
			private RoomHandler m_parentRoom;

			// Token: 0x040000A2 RID: 162
			private GameObject m_instanceMinimapIcon;

			// Token: 0x040000A3 RID: 163
			public int numUses = 0;

			// Token: 0x040000A4 RID: 164
			public string text;

			// Token: 0x040000A5 RID: 165
			public string acceptText;

			// Token: 0x040000A6 RID: 166
			public string declineText;
		}
	}
}
