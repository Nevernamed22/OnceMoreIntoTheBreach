using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    public class EndlessBullets : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<EndlessBullets>(
             "Endless Bullets",
             "And They Don't Stop Comin'",
             "Grants nigh infinite range on all weapons." + "\n\nIn the Gungeon, range is your ultimate advantage. Keep your distance from the enemy.",
             "endlessbullets_icon");
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.RangeMultiplier, 100, StatModifier.ModifyMethod.MULTIPLICATIVE);
            item.quality = PickupObject.ItemQuality.C;
            item.SetTag("bullet_modifier");
            Doug.AddToLootPool(item.PickupObjectId);
        }
    }
}

