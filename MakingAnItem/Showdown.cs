using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using ItemAPI;
using Dungeonator;

namespace NevernamedsItems
{
    public class Showdown : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Showdown";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/goomperorscrown_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<Showdown>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Now it's just you and me...";
            string longDesc = "Prevents bosses from being able to spawn additional backup."+"\n\nAn icon of the one-on-one gunfights of days gone by.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.EXCLUDED; //D
        }
        public void AIActorMods(AIActor target)
        {
            if (EasyEnemyTypeLists.BossMinions.Contains(target.EnemyGuid))
            {

            }
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            ETGMod.AIActor.OnPreStart += AIActorMods;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            ETGMod.AIActor.OnPreStart -= AIActorMods;
            return debrisObject;
        }
        protected override void OnDestroy()
        {
            ETGMod.AIActor.OnPreStart -= AIActorMods;
            base.OnDestroy();
        }
    }
}

