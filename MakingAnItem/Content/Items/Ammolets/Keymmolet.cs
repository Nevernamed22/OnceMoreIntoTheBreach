using UnityEngine;
using Alexandria.ItemAPI;
using Dungeonator;
using System.Collections;
using System;
using MonoMod.RuntimeDetour;
using System.Reflection;
using Gungeon;
using System.Collections.Generic;
using Alexandria.Misc;

namespace NevernamedsItems
{
    public class Keymmolet : BlankModificationItem
    {
        public static void Init()
        {
            BlankModificationItem item = ItemSetup.NewItem<Keymmolet>(
            "Keymmolet",
            "Blanks are Key",
            "Using a blank unlocks all chests in the room!" + "\n\nA brilliant evolution in ammolet technology, combining the radial power of a blank with the opening power of a key.",
            "keymmolet_icon") as BlankModificationItem;
            item.quality = PickupObject.ItemQuality.A;

            item.AddToSubShop(ItemBuilder.ShopType.OldRed);
            ID = item.PickupObjectId;
            item.SetTag("ammolet");
        }

        private static int ID;
        public override void Pickup(PlayerController player)
        {
            player.GetExtComp().OnBlankModificationItemProcessed += OnBlankModTriggered;
            base.Pickup(player);
        }
        public override void DisableEffect(PlayerController player)
        {
            player.GetExtComp().OnBlankModificationItemProcessed -= OnBlankModTriggered;
            base.DisableEffect(player);
        }

        private void OnBlankModTriggered(PlayerController user, SilencerInstance blank, Vector2 pos, BlankModificationItem item)
        {
            if (item is Keymmolet)
            {
                RoomHandler currentRoom = user.CurrentRoom;
                Chest[] allChests = FindObjectsOfType<Chest>();
                foreach (Chest chest in allChests)
                {
                    if (chest.transform.position.GetAbsoluteRoom() == currentRoom)
                    {
                        if (chest.ChestIdentifier == Chest.SpecialChestIdentifier.RAT && chest.IsLocked) { TextBubble.DoAmbientTalk(chest.transform, new Vector3(1, 2, 0), "Nice try", 4f); }
                        else { chest.ForceUnlock(); }
                    }
                }
            }
        }
    }
}
