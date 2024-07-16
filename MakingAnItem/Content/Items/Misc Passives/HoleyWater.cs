using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Alexandria.ItemAPI;
using SaveAPI;
using UnityEngine;

namespace NevernamedsItems
{
    public class HoleyWater : PassiveItem
    {
        public static void Init()
        {
            PassiveItem item = ItemSetup.NewItem<HoleyWater>(
            "Holey Water",
            "Pure",
            "Prevents the more esoteric effects of curse." + "\n\nDistilled with water taken from the elusive Shrine of Cleansing.",
            "holeywater_icon") as PassiveItem;
            item.quality = PickupObject.ItemQuality.D;
            HoleyWaterID = item.PickupObjectId;
            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.FLOOR_CLEARED_WITH_CURSE, true);
        }
        public static int HoleyWaterID;
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            return base.Drop(player);
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}
