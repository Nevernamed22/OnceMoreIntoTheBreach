using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    public class HikingPack : PassiveItem
    {
        public static void Init()
        {
            PassiveItem item = ItemSetup.NewItem<HikingPack>(
              "Hiking Pack",
              "Go Take A Hike",
              "A large pack intended for use in hiking." + "\n\nHas plenty of pockets to hide all your spice in.",
              "hikingpack_icon") as PassiveItem;
            item.AddPassiveStatModifier(PlayerStats.StatType.AdditionalItemCapacity, 2f, StatModifier.ModifyMethod.ADDITIVE);
            item.quality = PickupObject.ItemQuality.C;
            item.AddToSubShop(ItemBuilder.ShopType.Trorc);
        }
    }
}