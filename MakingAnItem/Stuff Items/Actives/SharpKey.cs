﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using ItemAPI;

namespace NevernamedsItems
{
    class SharpKey : PlayerItem
    {
        //Call this method from the Start() method of your ETGModule extension class
        public static void Init()
        {
            //The name of the item
            string itemName = "Sharp Key";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it.
            string resourceName = "NevernamedsItems/Resources/sharpkey_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<SharpKey>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "SaKeyfice";
            string longDesc = "This key is hungry for sustenance so that it may lay its eggs, and spawn the next generation of keys.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"kts" here is the item pool. In the console you'd type kts:sweating_bullets
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 5);

            //Adds a passive modifier, like curse, coolness, damage, etc. to the item. Works for passives and actives.
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE);

            //Set some other fields
            item.consumable = false;
            item.quality = ItemQuality.B;
            item.AddToSubShop(ItemBuilder.ShopType.Flynt);
        }

        //Add the item's functionality down here! I stole most of this from the Stuffed Star active item code!

        public override void DoEffect(PlayerController user)
        {
            //Activates the effect
            PlayableCharacters characterIdentity = user.characterIdentity;

            if (characterIdentity != PlayableCharacters.Robot)
            {
                AkSoundEngine.PostEvent("Play_OBJ_goldkey_pickup_01", base.gameObject);
                user.healthHaver.ApplyHealing(-1.5f);
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(67).gameObject, user);
            }
            else if (characterIdentity == PlayableCharacters.Robot)
            {
                AkSoundEngine.PostEvent("Play_OBJ_goldkey_pickup_01", base.gameObject);
                user.healthHaver.Armor = user.healthHaver.Armor - 2;
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(67).gameObject, user);
            }

            //start a coroutine which calls the EndEffect method when the item's effect duration runs out

        }

        public override bool CanBeUsed(PlayerController user)
        {
            PlayableCharacters characterIdentity = user.characterIdentity;
            if (characterIdentity == PlayableCharacters.Robot)
            {
                return user.healthHaver.Armor > 2;
            }
            else
            {
                return user.healthHaver.GetCurrentHealth() > 1.5f;
            }

        }
    }
}
