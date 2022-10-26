using UnityEngine;
using Alexandria.ItemAPI;
using Dungeonator;
using System.Collections;
using System;
using MonoMod.RuntimeDetour;
using System.Reflection;
using Gungeon;
using System.Linq;
using System.Collections.Generic;
using Alexandria.Misc;

namespace NevernamedsItems
{
    public class FuriousAmmolet : BlankModificationItem
    {
        public static void Init()
        {
            string itemName = "Furious Ammolet";
            string resourceName = "NevernamedsItems/Resources/furiousammolet_icon";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<FuriousAmmolet>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Blanks Enrage";
            string longDesc = "Using a blank sends the bearer of this Ammolet into a bloody rage."+"\n\nMade of a disgusting alloy of blood and iron, this ammolet is warm to the touch.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");
            item.quality = PickupObject.ItemQuality.B;
            item.AddToSubShop(ItemBuilder.ShopType.OldRed);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.AdditionalBlanksPerFloor, 1f, StatModifier.ModifyMethod.ADDITIVE);
            ID = item.PickupObjectId;
            item.SetTag("ammolet");
        }
        private static int ID;
        public override void Pickup(PlayerController player)
        {
            player.GetExtComp().OnBlankModificationItemProcessed += OnBlankModTriggered;
            base.Pickup(player);
        }
        public override void DisableEffect(PlayerController player)
        {
            player.GetExtComp().OnBlankModificationItemProcessed -= OnBlankModTriggered;
            base.DisableEffect(player);
        }

        private void OnBlankModTriggered(PlayerController user, SilencerInstance blank, Vector2 pos, BlankModificationItem item)
        { 
            if (item is FuriousAmmolet)
            {
                user.GetExtComp().Enrage(7f, false);
            }
        }     
    }
}

