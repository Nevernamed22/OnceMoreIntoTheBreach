using System;
using System.Collections.Generic;
using System.Linq;
using Dungeonator;
using System.Text;

using ItemAPI;
using UnityEngine;

namespace NevernamedsItems
{
    class GSwitch : PlayerItem
    {
        //Call this method from the Start() method of your ETGModule extension class
        public static void Init()
        {
            //The name of the item
            string itemName = "G-Switch";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it.
            string resourceName = "NevernamedsItems/Resources/gswitch_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<GSwitch>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Wahoo!";
            string longDesc = "Turns cash into a protective barrier."+"\n\nThis particular G-Switch once ruled a part of the Keep as a usurper, before eventually being dethroned from within it's G-Switch Palace.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"kts" here is the item pool. In the console you'd type kts:sweating_bullets
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 1);

            //Adds a passive modifier, like curse, coolness, damage, etc. to the item. Works for passives and actives.


            //Set some other fields
            item.consumable = false;
            item.quality = ItemQuality.D;
        }

        // Token: 0x06007230 RID: 29232 RVA: 0x002D6104 File Offset: 0x002D4304
        protected override void DoEffect(PlayerController user)
        {
            if (user.HasPickupID(376)) user.carriedConsumables.Currency -= 1;
            else user.carriedConsumables.Currency -= 3;

            Gun gun = PickupObjectDatabase.GetById(380) as Gun;
            Gun currentGun = user.CurrentGun;
            GameObject gameObject = gun.ObjectToInstantiateOnReload.gameObject;
            GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject, user.sprite.WorldCenter, Quaternion.identity);
            SingleSpawnableGunPlacedObject @interface = gameObject2.GetInterface<SingleSpawnableGunPlacedObject>();
            BreakableShieldController component = gameObject2.GetComponent<BreakableShieldController>();
            bool flag3 = gameObject2;
            if (flag3)
            {
                @interface.Initialize(currentGun);
                component.Initialize(currentGun);
            }
        }
        public override bool CanBeUsed(PlayerController user)
        {
            if (user.HasPickupID(376))
            {
                if (user.carriedConsumables.Currency >= 1) return true;
                else return false;
            }
            else
            {
                if (user.carriedConsumables.Currency >= 3) return true;               
                else return false;
            }
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }


    }
}