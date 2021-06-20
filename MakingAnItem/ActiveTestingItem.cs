﻿using System;
using System.Collections.Generic;
using System.Linq;
using Dungeonator;
using System.Text;

using ItemAPI;
using UnityEngine;
using System.Collections;

namespace NevernamedsItems
{
    class ActiveTestingItem : PlayerItem
    {
        //Call this method from the Start() method of your ETGModule extension class
        public static void Init()
        {
            //The name of the item
            string itemName = "<WIP> Active Testing Item <WIP>";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it.
            string resourceName = "NevernamedsItems/Resources/workinprogress_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<ActiveTestingItem>();

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

        public override bool CanBeUsed(PlayerController user)
        {
            return true;
        }
        //float duration = 20f;
        protected override void DoEffect(PlayerController user)
        {
            /*
            Vector2 yourPosition = user.sprite.WorldCenter;
            List<AIActor> activeEnemies = user.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            if (activeEnemies != null)
            {
                for (int i = 0; i < activeEnemies.Count; i++)
                {
                    AIActor aiactor = activeEnemies[i];

                    GameActorSizeEffect shrinky = StatusEffectHelper.GenerateSizeEffect(10, new Vector2(0.4f, 0.4f));
                    aiactor.ApplyEffect(shrinky);
                }
            }*/
            TalkDoerLite[] allChests = FindObjectsOfType<TalkDoerLite>();
            foreach (TalkDoerLite chest in allChests)
            {
                ETGModConsole.Log(chest.name);
            }
        }
        public override void Update()
        {
            base.Update();
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);

        }


    }
}
