using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.Misc;

namespace NevernamedsItems
{
    public class ExaltedHeart : PassiveItem
    {
        public static int ExaltedHeartID;
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<ExaltedHeart>(
            "Exalted Heart",
            "Praise Be Kaliber",
            "This ornate heart throbs with the power of Kaliber.",
            "exaltedheart_icon");
            item.AddPassiveStatModifier( PlayerStats.StatType.Health, 1, StatModifier.ModifyMethod.ADDITIVE);
            item.quality = PickupObject.ItemQuality.C;
            ExaltedHeartID = item.PickupObjectId;
            //item.AddAsChamberGunMastery("OnceMoreIntoTheBreach", 8);
            item.RemovePickupFromLootTables();
        }
    }
}
