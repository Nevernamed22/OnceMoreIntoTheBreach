using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using ItemAPI;
using UnityEngine;
using MonoMod.RuntimeDetour;
using System.Reflection;

namespace NevernamedsItems
{
    public class MengerAmmoBox : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Menger Ammo Box";
            string resourceName = "NevernamedsItems/Resources/mengerammobox_icon";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<MengerAmmoBox>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Fractal Replenishment";
            string longDesc = "Equalises regular and spread ammo boxes."+"\n\nA delicate fractal of infinitely patterned bullets. Occasionally coughs up lead from the void.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");
            item.quality = PickupObject.ItemQuality.B;

            MengerAmmoBoxID = item.PickupObjectId;
        }
        public static int MengerAmmoBoxID;       
    }
}