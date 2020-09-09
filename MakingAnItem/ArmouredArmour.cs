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
    public class ArmouredArmour : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Armoured Armour";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/armouredarmour_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<ArmouredArmour>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Why ARE they shields?";
            string longDesc = "Doubles all armour pickups."+"\n\nA suit of armour made out of smaller, less effective pieces of armour."+"\nIt's genius!";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item


            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.EXCLUDED;
        }
        Hook healthPickupHook = new Hook(
                typeof(HealthPickup).GetMethod("Pickup", BindingFlags.Instance | BindingFlags.Public),
                typeof(ArmouredArmour).GetMethod("heartPickupHookMethod", BindingFlags.Instance | BindingFlags.Public),
                typeof(HealthPickup)
            );
        public void heartPickupHookMethod(Action<HealthPickup, PlayerController> orig, HealthPickup self, PlayerController player)
        {
            orig(self, player);
            if (player.HasPickupID(Gungeon.Game.Items["nn:armoured_armour"].PickupObjectId))
            {
                if (self.armorAmount > 0)
                {
                    if (ArmourInt == 1)
                    {
                        player.healthHaver.Armor += 1;
                        ArmourInt = 0;
                    }
                    else
                    {
                        ArmourInt += 1;
                    }
                    
                }
            }
        }
        int ArmourInt = 0;
        public override void Pickup(PlayerController player)
        {
            bool hasntAlreadyBeenCollected = !this.m_pickedUpThisRun;
            if (hasntAlreadyBeenCollected)
            {
                player.healthHaver.Armor += 2;
            }
            base.Pickup(player);
        }
    }
}