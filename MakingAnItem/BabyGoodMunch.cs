using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ItemAPI;
using UnityEngine;

namespace NevernamedsItems
{
    class BabyGoodMunch : PlayerItem
    {
        //Call this method from the Start() method of your ETGModule extension class
        public static void Init()
        {
            //The name of the item
            string itemName = "Baby Good Munch";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it.
            string resourceName = "NevernamedsItems/Resources/workinprogress_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<BabyGoodMunch>();

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
            item.quality = ItemQuality.EXCLUDED;
        }

        //Add the item's functionality down here! I stole most of this from the Stuffed Star active item code!
        float duration = 0f;
        protected override void DoEffect(PlayerController user)
        {
            //Play a sound effect
            AkSoundEngine.PostEvent("Play_CHR_muncher_eat_01", base.gameObject);

            //Activates the effect
            StartEffect(user);

            //start a coroutine which calls the EndEffect method when the item's effect duration runs out
            StartCoroutine(ItemBuilder.HandleDuration(this, duration, user, EndEffect));
        }


        int gunsMiniMunched = 0;
        //Doubles the movement speed
        private void StartEffect(PlayerController user)
        {
            if (user.CurrentGun.CanActuallyBeDropped(user))
            {
                Gun currentGun = user.CurrentGun;
                PickupObject.ItemQuality itemQuality = currentGun.quality;
                //string gunQuality = base.LastOwner.CurrentGun.quality;
                string gunName = currentGun.name;
                //string GunQuality = user.inventory.CurrentGun.quality;
                user.inventory.DestroyCurrentGun();

                if (gunsMiniMunched == 0)
                {
                    ETGModConsole.Log("First Gun Munched: " + gunName + "\nQuality: " + itemQuality);
                    gunsMiniMunched += 1;
                    PickupObject.ItemQuality itemQuality1 = itemQuality;
                }
                else if (gunsMiniMunched == 1)
                {
                    ETGModConsole.Log("Second Gun Munched: " + gunName + "\nQuality: " + itemQuality);
                    //string secondItemTier = itemQuality;
                    gunsMiniMunched = 0;
                    PickupObject.ItemQuality itemQuality2 = itemQuality;
                    //GetGunOutputQuality(itemQuality1, itemQuality2);
                }
                else
                {
                    return;
                }
            }
        }

        private void GetGunOutputQuality(PickupObject.ItemQuality itemQuality1, PickupObject.ItemQuality itemQuality2)
        {
            
        }

        private void GiveGunReward()
        {
            //int randomGunOutputQuality = UnityEngine.Random.Range(1, 5);
            PlayerController player = this.LastOwner;
            //if (randomGunOutputQuality == 1)
            //{
            PickupObject.ItemQuality gunQuality = PickupObject.ItemQuality.S;
            PickupObject itemOfTypeAndQuality = LootEngine.GetItemOfTypeAndQuality<PickupObject>(gunQuality, GameManager.Instance.RewardManager.GunsLootTable, false);
            LootEngine.SpawnItem(itemOfTypeAndQuality.gameObject, player.specRigidbody.UnitCenter, Vector2.left, 1f, false, true, false);
            //}
        }

        //Resets the player back to their original stats
        private void EndEffect(PlayerController user)
        {
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }

        public DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            return debrisObject;
        }

        //Disable or enable the active whenever you need!
        public override bool CanBeUsed(PlayerController user)
        {
            return base.CanBeUsed(user);
        }
    }
}
