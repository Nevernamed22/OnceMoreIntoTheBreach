using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SaveAPI;

using Alexandria.ItemAPI;
using UnityEngine;

namespace NevernamedsItems
{
    public class Gracelets : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<Gracelets>(
             "Gracelets",
             "Make a Match",
             "Powerful enchanted bracelets, they resonate with harmonising power- bending fate to encourage even greater levels of synergy.\n\nCreated by a powerful sorceress from the moon Sniperion.",
             "gracelets_icon");
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Coolness, 1, StatModifier.ModifyMethod.ADDITIVE);
            item.quality = PickupObject.ItemQuality.B;

            ID = item.PickupObjectId;
            item.SetupUnlockOnFlag(GungeonFlags.SYNERGRACE_UNLOCKED, true);
        }
        public static int ID;
    }
}
