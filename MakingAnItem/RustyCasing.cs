using System;
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
        }

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
            cashAmount = 3;
            giveCash(player, cashAmount);
        }
        public static void blankPickupHookMethod(Action<SilencerItem, PlayerController> orig, SilencerItem self, PlayerController player)
        {
            orig(self, player);
            cashAmount = 2;
            giveCash(player, cashAmount);
        }
        public static void keyPickupHookMethod(Action<KeyBulletPickup, PlayerController> orig, KeyBulletPickup self, PlayerController player)
        {
            orig(self, player);
            if (self.IsRatKey) cashAmount = 5;
            else cashAmount = 3;
            giveCash(player, cashAmount);
        }
        public static void heartPickupHookMethod(Action<HealthPickup, PlayerController> orig, HealthPickup self, PlayerController player)
        {
            orig(self, player);
            if (player.HasPickupID(Gungeon.Game.Items["nn:rusty_casing"].PickupObjectId))
            {
                if (self.armorAmount > 0) cashAmount = 4;
                else
                {
                    if (self.healAmount == 1f)
                    {
                        cashAmount = 4;
                    }
                    else if (self.healAmount == 2f)
                    {
                        cashAmount = 8;
                    }
                    else
                    {
                        ETGModConsole.Log("The heart pickup had an unrecognised amount of heal juice in it.");
                        cashAmount = 1;
                    }
                }
                giveCash(player, cashAmount);
            }
        }
        public static void giveCash(PlayerController player, int cashAmount)
        {
            if (player.HasPickupID(Gungeon.Game.Items["nn:rusty_casing"].PickupObjectId))
            {
                if (player.HasPickupID(Gungeon.Game.Items["nn:loose_change"].PickupObjectId))
                {
                    cashAmount += 5;
                }
                for (int i = 0; i < cashAmount; i++)
                {
                    LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(68).gameObject, player);
                }
            }
        }
        public static int cashAmount;
    }
}
