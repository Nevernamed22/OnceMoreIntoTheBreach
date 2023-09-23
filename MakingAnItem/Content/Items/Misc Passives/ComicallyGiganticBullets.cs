using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    class ComicallyGiganticBullets : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<ComicallyGiganticBullets>(
            "Comically Gigantic Bullets",
            "What is this?",
            "These were a very bad idea." + "'This item cannot appear in normal play, but I left it in the mod for people to give themselves because I'm a maniac.' - NN",
            "comicallygiganticbullets_icon");
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Damage, 100f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            item.quality = PickupObject.ItemQuality.EXCLUDED;
        }
    }
}
