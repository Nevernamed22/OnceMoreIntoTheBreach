using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dungeonator;
using UnityEngine;
using ItemAPI;

namespace NevernamedsItems
{
    class CheeseHeart : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Cheese Heart";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/cheeseheart_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<CheeseHeart>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Eat Your Heart Out";
            string longDesc = "Sprays cheese everywhere on hit."+"\n\nCarefully sculpted, and completely anatomically correct!";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.C;

            CheeseHeartID = item.PickupObjectId;
        }
        public static int CheeseHeartID;
        private void charmAll(PlayerController user)
        {
            DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(EasyGoopDefinitions.CheeseDef).TimedAddGoopCircle(Owner.sprite.WorldCenter, 10, 1, false);
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnReceivedDamage += this.charmAll;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.OnReceivedDamage -= this.charmAll;
            return debrisObject;
        }
        protected override void OnDestroy()
        {
            Owner.OnReceivedDamage -= this.charmAll;
            base.OnDestroy();
        }
    }
}

//GoopDefinition GreenFireDef = (PickupObjectDatabase.GetById(698) as Gun).DefaultModule.projectiles[0].GetComponent<GoopModifier>().goopDefinition;
