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
    public class BloodyAmmo : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Bloody Ammo";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/bloodyammo_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<BloodyAmmo>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Lead Vessel";
            string longDesc = "Heals the owner upon picking up ammo." + "\n\nAn ammo box with it's skin removed.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item


            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.B;
        }
        Hook ammoPickupHook = new Hook(
                typeof(AmmoPickup).GetMethod("Pickup", BindingFlags.Instance | BindingFlags.Public),
                typeof(BloodyAmmo).GetMethod("ammoPickupHookMethod")
            );
        public static void ammoPickupHookMethod(Action<AmmoPickup, PlayerController> orig, AmmoPickup self, PlayerController player)
        {
            orig(self, player);
            //ETGModConsole.Log("Ammo pickup was triggered");
            if (player.HasPickupID(Gungeon.Game.Items["nn:bloody_ammo"].PickupObjectId))
            {
                if (canGiveHealth)
                {
                    player.healthHaver.ApplyHealing(0.5f);
                    if (player.characterIdentity == PlayableCharacters.Robot)
                    {
                        LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(120).gameObject, player);
                    }
                    canGiveHealth = false;
                }
                else
                {
                    canGiveHealth = true;
                }
            }
        }
        static bool canGiveHealth = false;
    }
}
