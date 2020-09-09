using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using ItemAPI;

namespace NevernamedsItems
{
    public class MistakeBullets : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Mistake Bullets";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/mistakebullets_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<MistakeBullets>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Your Bullets Suck!";
            string longDesc = "Gain a firerate and reload speed bonus, in exchange for negative knockback.\n\n" + "No relation to the actual Mistake though. These bullets were made by a hunchbacked hermit living in space Albania or something.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.KnockbackMultiplier, -3, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.RateOfFire, 1.2f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.ReloadSpeed, 0.7f, StatModifier.ModifyMethod.MULTIPLICATIVE);

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.D;
        }

        public override void Pickup(PlayerController player)
        {
            bool flag = !this.m_pickedUpThisRun;
            if (flag)
            {
                PickupObject byId = PickupObjectDatabase.GetById(565);
                player.AcquirePassiveItemPrefabDirectly(byId as PassiveItem);
                PickupObject byId2 = PickupObjectDatabase.GetById(127);
                player.AcquirePassiveItemPrefabDirectly(byId2 as PassiveItem);
            }
            base.Pickup(player);
        }
    }
}
