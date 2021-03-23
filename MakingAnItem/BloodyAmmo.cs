﻿using System;
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
    public class BloodyAmmo : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Bloody Ammo";
            string resourceName = "NevernamedsItems/Resources/bloodyammo_icon";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<BloodyAmmo>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Lead Vessel";
            string longDesc = "Heals the owner upon picking up ammo." + "\n\nAn ammo box with it's skin removed.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");
            item.quality = PickupObject.ItemQuality.B;
            BloodyAmmoID = item.PickupObjectId;
        }
        public static int BloodyAmmoID;
    }
}
