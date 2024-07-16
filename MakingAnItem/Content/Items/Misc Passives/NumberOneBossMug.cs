using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    public class NumberOneBossMug : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<NumberOneBossMug>(
            "#1 Boss Mug",
            "You're the boss now",
            "Slightly increased damage against bosses." + "\n\nThe inscription on this eldritch relic gives you all the confidence you need to boss around the boss.",
            "numberonebossmug_improved");
            item.AddPassiveStatModifier(PlayerStats.StatType.DamageToBosses, 1.25f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            item.quality = PickupObject.ItemQuality.C;
        }
    }
}
