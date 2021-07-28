using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ItemAPI;
using UnityEngine;

namespace NevernamedsItems
{
    public class AWholeBulletKin : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "A Whole Bullet Kin";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/awholebulletkin_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<AWholeBulletKin>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "What.";
            string longDesc = "This is just... a bullet kin...\n\n" + "Is he moving?... Is he alive? Comatose? Also WHAT?";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.ReloadSpeed, 0.8f, StatModifier.ModifyMethod.MULTIPLICATIVE);

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.D;

            //ETGModConsole.Log("ID: " + item.PickupObjectId);

            WholeBulletKinID = item.PickupObjectId;

            //ETGModConsole.Log("Stored ID: " + WholeBulletKinID);
        }
        public static int WholeBulletKinID;
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject result = base.Drop(player);
            return result;
        }
    }
}
