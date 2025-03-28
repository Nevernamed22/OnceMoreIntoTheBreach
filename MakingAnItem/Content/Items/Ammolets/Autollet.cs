﻿using System.Text;
using UnityEngine;
using MonoMod.RuntimeDetour;
using System.Reflection;
using System.Collections.Generic;
using Dungeonator;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    public class Autollet : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<Autollet>(
            "Autollet",
            "Automatic and Effective",
            "Automatically triggers a free blank upon entering an unvisited room with enemies." + "\n\nThe end product of using science to reverse engineer the strange and esoteric Elder Blank.",
            "autollet_icon");
            item.CanBeDropped = true;
            item.quality = PickupObject.ItemQuality.C;

            item.AddToSubShop(ItemBuilder.ShopType.OldRed);
            AutolletID = item.PickupObjectId;
            item.SetTag("ammolet");
        }
        public static int AutolletID;

        public List<RoomHandler> roomsVisitedThisFloor = new List<RoomHandler>() { };
        public RoomHandler lastRoom;
        public RoomHandler currentRoom;
        private void TriggerBlankIfAppropriate()
        {
            if (Owner.CurrentRoom.GetActiveEnemiesCount(RoomHandler.ActiveEnemyType.All) > 0 || Owner.PlayerHasActiveSynergy("Code Blanks"))
            {
                Owner.ForceBlank(45f, 0.5f, false, true, null, true, -1f);
            }
        }
        public override void Update()
        {
            if (Owner && Owner.CurrentRoom != null)
            {
                currentRoom = Owner.CurrentRoom;
                if (currentRoom != lastRoom)
                {
                    if (!roomsVisitedThisFloor.Contains(currentRoom))
                    {

                        TriggerBlankIfAppropriate();
                        roomsVisitedThisFloor.Add(currentRoom);

                    }
                    lastRoom = currentRoom;
                }
            }
            base.Update();
        }
        private void NewFloor()
        {
            roomsVisitedThisFloor.Clear();
        }
        public override void Pickup(PlayerController player)
        {
            GameManager.Instance.OnNewLevelFullyLoaded += this.NewFloor;
            base.Pickup(player);
        }
        public override void DisableEffect(PlayerController player)
        {
            GameManager.Instance.OnNewLevelFullyLoaded -= this.NewFloor;
            base.DisableEffect(player);
        }
    }
}
