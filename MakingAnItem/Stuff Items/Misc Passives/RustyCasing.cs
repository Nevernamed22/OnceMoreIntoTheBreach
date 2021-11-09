﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ItemAPI;
using UnityEngine;
using MonoMod.RuntimeDetour;
using System.Reflection;

namespace NevernamedsItems
{
    public class RustyCasing : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Rusty Casing";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/rustycasing_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<RustyCasing>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Heheheheheheh";
            string longDesc = "Yesyesyoulikestufffyouneeedmoney." + "\nThisgiveyoumoneyyesyesyes." + "\n\nHeheheheheheh";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item


            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.D;

            RustyCasingID = item.PickupObjectId;
        }
        public static int RustyCasingID;

        Hook keyPickupHook = new Hook(
                typeof(KeyBulletPickup).GetMethod("Pickup", BindingFlags.Instance | BindingFlags.Public),
                typeof(RustyCasing).GetMethod("keyPickupHookMethod")
            );
        Hook healthPickupHook = new Hook(
                typeof(HealthPickup).GetMethod("Pickup", BindingFlags.Instance | BindingFlags.Public),
                typeof(RustyCasing).GetMethod("heartPickupHookMethod")
            );
        Hook blankPickupHook = new Hook(
                typeof(SilencerItem).GetMethod("Pickup", BindingFlags.Instance | BindingFlags.Public),
                typeof(RustyCasing).GetMethod("blankPickupHookMethod")
            );
        Hook ammoPickupHook = new Hook(
                typeof(AmmoPickup).GetMethod("Pickup", BindingFlags.Instance | BindingFlags.Public),
                typeof(RustyCasing).GetMethod("ammoPickupHookMethod")
            );
        public static void ammoPickupHookMethod(Action<AmmoPickup, PlayerController> orig, AmmoPickup self, PlayerController player)
        {
            orig(self, player);
            if (ammoRegulator)
            {
                giveCash(player, 6);
                ammoRegulator = false;
            }
            else
            {
                ammoRegulator = true;
            }
        }
        static bool ammoRegulator = false;
        public static void blankPickupHookMethod(Action<SilencerItem, PlayerController> orig, SilencerItem self, PlayerController player)
        {
            orig(self, player);
            if (blankRegulator)
            {
                blankRegulator = false;
                giveCash(player, 4);
            }
            else
            {
                blankRegulator = true;
            }
        }
        static bool blankRegulator = false;
        public static void keyPickupHookMethod(Action<KeyBulletPickup, PlayerController> orig, KeyBulletPickup self, PlayerController player)
        {
            orig(self, player);
            if (keyRegulator)
            {
                if (self.IsRatKey) giveCash(player, 10);
                else giveCash(player, 6);
                keyRegulator = false;
            }
            else
            {
                keyRegulator = true;
            }
        }
        static bool keyRegulator = false;
        public static void heartPickupHookMethod(Action<HealthPickup, PlayerController> orig, HealthPickup self, PlayerController player)
        {
            orig(self, player);
            if (heartRegulator)
            {
                if (self.armorAmount > 0) giveCash(player, 8);
                else giveCash(player, (int)(self.healAmount * 4f));
                heartRegulator = false;
            }
            else heartRegulator = true;
        }
        static bool heartRegulator = false;
        public static void giveCash(PlayerController player, int cashAmount)
        {
            if (player.HasPickupID(RustyCasingID))
            {
                if (player.PlayerHasActiveSynergy("Lost, Never Found")) cashAmount += 5;
                LootEngine.SpawnCurrency(player.sprite.WorldCenter, cashAmount);                
            }
        }
    }
}