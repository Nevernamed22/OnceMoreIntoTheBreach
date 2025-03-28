using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.Misc;

namespace NevernamedsItems
{
    public class HeartContainer : PassiveItem
    {
        public static int ID;
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<HeartContainer>(
            "Heart Container",
            "Back to Basics",
            "A high quality health storage device.\n\nDungeon divers have used devices like these for decades to avoid an untimely death.",
            "heartcontainer_icon");
            item.AddPassiveStatModifier(PlayerStats.StatType.Health, 1, StatModifier.ModifyMethod.ADDITIVE);
            item.quality = PickupObject.ItemQuality.B;
            item.ItemSpansBaseQualityTiers = true;
            item.ItemRespectsHeartMagnificence = true;
            ID = item.PickupObjectId;
        }
        public override void Pickup(PlayerController player)
        {
            if (!base.m_pickedUpThisRun)
            {
                player.healthHaver.ApplyHealing(100f);
            }
            base.Pickup(player);
        }
    }
}
