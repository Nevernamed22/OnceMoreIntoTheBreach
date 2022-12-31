﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Alexandria.ItemAPI;
using Alexandria.Misc;
using UnityEngine;
using MonoMod.RuntimeDetour;
using System.Reflection;

namespace NevernamedsItems
{
    public class BlankDie : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Blank Die";
            string resourceName = "NevernamedsItems/Resources/NeoItemSprites/blankdie_icon";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<BlankDie>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Roll On";
            string longDesc = "Triggers blanks at random.\n\nA six sided die with no pips on any of it's faces. Used by gamblers to clumsily cheat games in ages gone by.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");
            item.quality = PickupObject.ItemQuality.D;
            ID = item.PickupObjectId;
        }
        public static int ID;
        private void Update()
        {
            if (Owner)
            {
                if (timer >= 0)
                {
                    timer -= BraveTime.DeltaTime;
                }
                else
                {
                    Owner.DoEasyBlank(Owner.CenterPosition, EasyBlankType.FULL);
                    timer = UnityEngine.Random.Range(1f, 70f);
                }
            }
        }
        private float timer = 10;
    }
}