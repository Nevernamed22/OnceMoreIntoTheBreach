using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Alexandria.ItemAPI;
using Gungeon;
using UnityEngine;

namespace NevernamedsItems
{
    public class FiftyCalRounds : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<FiftyCalRounds>(
            "50. Cal Rounds",
            "Randy's Favourite",
            "These bullets are nothing special by Gungeon standards, but they do pack a decent punch.\n\n" + "Favoured by a peculiar young Gungeoneer with even more peculiar tastes.",
            "50calrounds_icon");
            Game.Items.Rename("nn:50._cal_rounds", "nn:50_cal_rounds");
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Damage, 1.16f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.ProjectileSpeed, 1.25f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            item.quality = PickupObject.ItemQuality.C;
            item.SetTag("bullet_modifier");
            item.AddToSubShop(ItemBuilder.ShopType.Trorc);
            FiftyCalRoundsID = item.PickupObjectId;
        }
        public static int FiftyCalRoundsID;
    }
}
