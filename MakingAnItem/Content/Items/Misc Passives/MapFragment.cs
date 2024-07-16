using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;
using MonoMod.RuntimeDetour;
using System.Reflection;
using System.Collections.Generic;
using Dungeonator;

namespace NevernamedsItems
{
    public class MapFragment : PassiveItem
    {
        public static void Init()
        {
            PassiveItem item = ItemSetup.NewItem<MapFragment>(
            "Map Fragment",
            "Selective Information",
            "Reveals nearby rooms." + "\n\nSeemingly torn from a larger map.",
            "mapfragment_icon") as PassiveItem;
            item.CanBeDropped = true;
            item.CustomCost = 20;
            item.UsesCustomCost = true;
            item.quality = PickupObject.ItemQuality.D;
            MapFragmentID = item.PickupObjectId;
        }
        public static int MapFragmentID;
        public RoomHandler lastRoom;
        public override void Update()
        {
            if (Owner && Owner.CurrentRoom != null)
            {
                if (Owner.CurrentRoom != lastRoom)
                {
                    foreach (RoomHandler adjacentRoom in Owner.CurrentRoom.connectedRooms)
                    {
                        if (!adjacentRoom.IsSecretRoom || Owner.PlayerHasActiveSynergy("Trust In The All-Seeing"))
                        {
                            Minimap.Instance.RevealMinimapRoom(adjacentRoom, true, true, false);
                            if (Owner.PlayerHasActiveSynergy("Restoration"))
                            {
                                foreach (RoomHandler adjacentRoom2 in adjacentRoom.connectedRooms)
                                {
                                    if (!adjacentRoom.IsSecretRoom || Owner.PlayerHasActiveSynergy("Trust In The All-Seeing"))
                                    {
                                        Minimap.Instance.RevealMinimapRoom(adjacentRoom2, true, true, false);

                                    }
                                }
                            }
                        }
                        lastRoom = Owner.CurrentRoom;
                    }
                }
                base.Update();
            }
        }

    }
}
