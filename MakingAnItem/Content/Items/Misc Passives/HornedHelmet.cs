using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.Misc;

namespace NevernamedsItems
{
    class HornedHelmet : PassiveItem
    {
        public static void Init()
        {
            PassiveItem item = ItemSetup.NewItem<HornedHelmet>(
              "Horned Helmet",
              "19 STR 7 INT",
              "You burst into a berserker rage upon the mere sight of fresh prey. What a monster!",
              "hornedhelmet_icon") as PassiveItem;
            item.quality = PickupObject.ItemQuality.B;
            item.AddToSubShop(ItemBuilder.ShopType.Trorc);
            HornedHelmetID = item.PickupObjectId;
        }
        public static int HornedHelmetID;
        public override void Pickup(PlayerController player)
        {
            player.OnEnteredCombat += this.OnEnteredCombat;
            base.Pickup(player);
        }
        public override void DisableEffect(PlayerController player)
        {
            player.OnEnteredCombat -= this.OnEnteredCombat;
            base.DisableEffect(player);
        }       
        private void OnEnteredCombat()
        {
            if (Owner && Owner.GetExtComp()) Owner.GetExtComp().Enrage(3, true);
        }
    }
}
