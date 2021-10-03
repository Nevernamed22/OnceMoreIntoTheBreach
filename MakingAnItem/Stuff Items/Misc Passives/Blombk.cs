using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using static MonoMod.Cil.RuntimeILReferenceBag.FastDelegateInvokers;
using UnityEngine;
using ItemAPI;
using MonoMod.RuntimeDetour;
using System.Reflection;

namespace NevernamedsItems
{
    class Blombk : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Blombk";
            string resourceName = "NevernamedsItems/Resources/blombk_icon";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<Blombk>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Boomer Blanks";
            string longDesc = "Triggers a small blank whenever an explosion goes off." + "\n\nA Fuselier egg painted blue.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");
            item.quality = PickupObject.ItemQuality.C;
            item.AddToSubShop(ItemBuilder.ShopType.OldRed);
            BlombkID = item.PickupObjectId;
        }
        public static int BlombkID;       
    }
}

