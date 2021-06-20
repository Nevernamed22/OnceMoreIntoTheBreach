using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ItemAPI;
using UnityEngine;

namespace NevernamedsItems
{
    public class FiftyCalRounds : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "50. Cal Rounds";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/50calrounds_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<FiftyCalRounds>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Randy's Favourite";
            string longDesc = "These bullets are nothing special by Gungeon standards, but they do pack a decent punch.\n\n" + "Favoured by a peculiar young Gungeoneer with even more peculiar tastes.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Damage, 1.16f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.ProjectileSpeed, 1.25f, StatModifier.ModifyMethod.MULTIPLICATIVE);

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.C;

            //Add to NPC Pools
            item.AddToSubShop(ItemBuilder.ShopType.Trorc);
            FiftyCalRoundsID = item.PickupObjectId;
        }
        public static int FiftyCalRoundsID;
    }
}
