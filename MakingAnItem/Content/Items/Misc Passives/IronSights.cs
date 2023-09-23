using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    public class IronSights : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<IronSights>(
            "Iron Sights",
            "Sturdy and Standard",
            "This convenient little knick-knack clips on to your gun, making it much easier to aim.\n\n" + "The previous owner seems to have clipped it onto a key... perhaps they were having trouble getting it in the lock?",
            "ironsights_improved");
            item.AddPassiveStatModifier( PlayerStats.StatType.Accuracy, 0.6f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            item.quality = PickupObject.ItemQuality.D;
            item.AddToSubShop(ItemBuilder.ShopType.Trorc);
            IronSightsID = item.PickupObjectId;
        }
        public static int IronSightsID;
        public override void Pickup(PlayerController player)
        {
            if (!this.m_pickedUpThisRun) { LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(67).gameObject, player); }
            base.Pickup(player);
        }
    }

}
