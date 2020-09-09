using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using ItemAPI;

namespace NevernamedsItems
{
    public class BoxOfBlanks : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Box of Blanks";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/workinprogress_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<BoxOfBlanks>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "placeholder";
            string longDesc = "Grants an extra key every floor.\nHaving a spare key is important, what if you lose the one you have?";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.EXCLUDED;
        }
        private void OnNewFloor()
        {
            PlayerController player = this.Owner;
            LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(67).gameObject, player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject result = base.Drop(player);
            GameManager.Instance.OnNewLevelFullyLoaded -= this.OnNewFloor;
            return result;
        }
        protected override void OnDestroy()
        {
            GameManager.Instance.OnNewLevelFullyLoaded -= this.OnNewFloor;
            base.OnDestroy();
        }
    }
}
