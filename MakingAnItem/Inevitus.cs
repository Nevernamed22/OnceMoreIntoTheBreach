using System;
using System.Collections.Generic;
using System.Linq;
using Dungeonator;
using System.Text;

using ItemAPI;
using UnityEngine;

namespace NevernamedsItems
{
    class Inevitus : PlayerItem
    {
        public static int InevitusID;
        //Call this method from the Start() method of your ETGModule extension class
        public static void Init()
        {
            //The name of the item
            string itemName = "Inevitus";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it.
            string resourceName = "NevernamedsItems/Resources/workinprogress_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<Inevitus>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Work In Progress";
            string longDesc = "This item was created by an amateur gunsmith so that he may test different concepts instead of going the whole nine yards and making a whole new item.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"kts" here is the item pool. In the console you'd type kts:sweating_bullets
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.None, 0);

            //Adds a passive modifier, like curse, coolness, damage, etc. to the item. Works for passives and actives.


            //Set some other fields
            item.consumable = false;
            item.quality = ItemQuality.EXCLUDED; //A
            InevitusID = item.PickupObjectId;
        }

        public override bool CanBeUsed(PlayerController user)
        {
            return true;
        }
        protected override void DoEffect(PlayerController user)
        {
            try
            {
                int gunsToGive = 0;
                int itemsToGive = 0;
                int activeItemsToGive = 0;

                cachedPassiveItemsToKillDrop.Clear();
                cachedGunsToDestroy.Clear();
                cachedActiveItemsToKillDrop.Clear();

                foreach (PassiveItem item in user.passiveItems)
                {
                    bool itemNeedsToBeIgnored = passiveItemsToIgnore.Contains(item.PickupObjectId);
                    if (item.CanActuallyBeDropped(user) && !itemNeedsToBeIgnored)
                    {
                        itemsToGive += 1;
                        cachedPassiveItemsToKillDrop.Add(item);
                    }
                }
                foreach (Gun gun in user.inventory.AllGuns)
                {
                    if (gun.CanActuallyBeDropped(user))
                    {
                        gunsToGive += 1;
                        cachedGunsToDestroy.Add(gun);
                    }
                }
                foreach (PlayerItem item in user.activeItems)
                {
                    bool itemNeedsToBeIgnored = passiveItemsToIgnore.Contains(item.PickupObjectId);
                    if (item.CanActuallyBeDropped(user) && !itemNeedsToBeIgnored)
                    {
                        if (UnityEngine.Random.value < 0.25f) itemsToGive += 1;
                        else activeItemsToGive += 1;
                        cachedActiveItemsToKillDrop.Add(item);
                    }
                }



                //ACTUALLY REMOVE ALL THE ITEMS
                foreach (PlayerItem item in cachedActiveItemsToKillDrop)
                {
                    if (item.PickupObjectId != InevitusID)
                    {
                        KillDropWeirdActive(item);
                    }
                }
                foreach (PassiveItem item in cachedPassiveItemsToKillDrop)
                {
                    KillDropWeirdItem(item);
                }
                foreach (Gun gun in cachedGunsToDestroy)
                {
                    user.inventory.RemoveGunFromInventory(gun);
                }


                //GIVE ITEMS
                PickupObject byId = PickupObjectDatabase.GetById(565);
                user.AcquirePassiveItemPrefabDirectly(byId as PassiveItem);
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
                ETGModConsole.Log(e.StackTrace);
            }
        }
        public static List<PlayerItem> cachedActiveItemsToKillDrop = new List<PlayerItem>();
        public static List<PassiveItem> cachedPassiveItemsToKillDrop = new List<PassiveItem>();
        public static List<Gun> cachedGunsToDestroy = new List<Gun>();


        public static List<int> passiveItemsToIgnore = new List<int>()
        {
            467, //Master Round 5
            468, //Master Round 3
            469, //Master Round 1
            470, //Master Round 4
            471, //Master Round 2
            127, //Junk
            565, //Glass Guon Stone
            316, //Gnawed Key
            InevitusID, //Inevitus Itself
        };
        public void KillDropWeirdItem(PassiveItem item)
        {
            DebrisObject debrisObject = LastOwner.DropPassiveItem(item);
            UnityEngine.Object.Destroy(debrisObject.gameObject, 1f);
        }
        public void KillDropWeirdActive(PlayerItem item)
        {
            DebrisObject debrisObject = LastOwner.DropActiveItem(item);
            UnityEngine.Object.Destroy(debrisObject.gameObject, 1f);
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);

        }


    }
}