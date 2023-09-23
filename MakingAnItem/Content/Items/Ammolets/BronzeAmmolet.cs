using UnityEngine;
using Alexandria.ItemAPI;
using Dungeonator;
using System.Collections;
using System;
using MonoMod.RuntimeDetour;
using System.Reflection;
using Gungeon;
using System.Collections.Generic;
using SaveAPI;
using Alexandria.Misc;

namespace NevernamedsItems
{
    public class BronzeAmmolet : BlankModificationItem
    {
        public static void Init()
        {
            BlankModificationItem item = ItemSetup.NewItem<BronzeAmmolet>(
            "Bronze Ammolet",
            "Blanks Diminish",
            "This ammolet appears to have shrunk in the wash, and is eager to take out it's vengeance against any home appliances or Gundead fiends that get in it's way by shrinking them as well!" + "\n\nShrunken enemies can be stomped on to kill them.",
            "bronzeammolet_icon") as BlankModificationItem;
            item.quality = PickupObject.ItemQuality.C;
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.AdditionalBlanksPerFloor, 1f, StatModifier.ModifyMethod.ADDITIVE);
            item.AddToSubShop(ItemBuilder.ShopType.OldRed);
            ID = item.PickupObjectId;

            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.PURCHASED_BRONZEAMMOLET, true);
            item.AddItemToDougMetaShop(41);
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
            if (item is BronzeAmmolet)
            {
                List<AIActor> activeEnemies = user.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
                if (activeEnemies != null)
                {
                    for (int i = 0; i < activeEnemies.Count; i++)
                    {
                        if (UnityEngine.Random.value <= 0.5f)
                        {
                            activeEnemies[i].ApplyEffect(StatusEffectHelper.GenerateSizeEffect(10, new Vector2(0.4f, 0.4f)));
                        }
                    }
                }
            }
        }
    }
}