using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;
using MonoMod.RuntimeDetour;
using System.Reflection;
using System.Collections.Generic;
using Dungeonator;

namespace NevernamedsItems
{
    public class TatteredMap : PassiveItem
    {
        public static void Init()
        {
            PassiveItem item = ItemSetup.NewItem<TatteredMap>(
            "Tattered Map",
            "Reveals Some Rooms",
            "Partially reveals the floor." + "\n\nThis moth-eaten parchment has seen better days.",
            "tatteredmap_icon") as PassiveItem;
            item.CanBeDropped = true;
            item.quality = PickupObject.ItemQuality.D;
            TatteredMapID = item.PickupObjectId;
        }
        public static int TatteredMapID;
        private void OnFloorLoaded()
        {
            for (int i = 0; i < GameManager.Instance.Dungeon.data.rooms.Count; i++)
            {
                if (UnityEngine.Random.value <= 0.25f)
                {
                    RoomHandler roomHandler = GameManager.Instance.Dungeon.data.rooms[i];
                    Minimap.Instance.RevealMinimapRoom(roomHandler, true, false, roomHandler == GameManager.Instance.PrimaryPlayer.CurrentRoom);
                }
            }
            Minimap.Instance.m_shouldBuildTilemap = true;
        }
        public override void Pickup(PlayerController player)
        {
            if (!this.m_pickedUpThisRun)
            {
                OnFloorLoaded();
            }
            GameManager.Instance.OnNewLevelFullyLoaded += this.OnFloorLoaded;
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            GameManager.Instance.OnNewLevelFullyLoaded -= this.OnFloorLoaded;

            return base.Drop(player);
        }
        public override void OnDestroy()
        {
            GameManager.Instance.OnNewLevelFullyLoaded -= this.OnFloorLoaded;

            base.OnDestroy();
        }

    }
}
