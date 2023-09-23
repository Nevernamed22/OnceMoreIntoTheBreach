using Alexandria.ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class HeavyChamber : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<HeavyChamber>(
            "Heavy Chamber",
            "Trade-off",
            "Doubles clip size, but also doubles the time it takes to reload." + "\n\nThis chamber is sturdy enough to carry many more bullets, but reloading it is a pain.",
            "heavychamber_icon");
            item.AddPassiveStatModifier( PlayerStats.StatType.ReloadSpeed, 2, StatModifier.ModifyMethod.MULTIPLICATIVE);
            item.AddPassiveStatModifier( PlayerStats.StatType.AdditionalClipCapacityMultiplier, 2, StatModifier.ModifyMethod.MULTIPLICATIVE);
            item.CanBeDropped = true;
            item.quality = PickupObject.ItemQuality.B;
            HeavyChamberID = item.PickupObjectId;
        }
        public static int HeavyChamberID;
    }
}
