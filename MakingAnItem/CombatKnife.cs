using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Dungeonator;
using ItemAPI;
using UnityEngine;

namespace NevernamedsItems
{
    class CombatKnife : PlayerItem
    {
        //Call this method from the Start() method of your ETGModule extension class
        public static void Init()
        {
            //The name of the item
            string itemName = "Combat Knife";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it.
            string resourceName = "NevernamedsItems/Resources/finishedbullet_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<CombatKnife>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Let's Finish This";
            string longDesc = "A single bullet from the legendary 'Finished Gun'." + "\n\nEven without the Gun to fire it, a good throwing arm and plenty of resolve can achieve wonderful results.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"kts" here is the item pool. In the console you'd type kts:sweating_bullets
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 7);

            //Adds a passive modifier, like curse, coolness, damage, etc. to the item. Works for passives and actives.
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE);

            //Set some other fields
            item.consumable = false;
            item.quality = ItemQuality.EXCLUDED; //D


        }
        protected override void DoEffect(PlayerController user)
        {
            int swingPower = 3;
            for (int i = 0; i < swingPower; i++)
            {
                (ETGMod.Databases.Items["wonderboy"] as Gun).muzzleFlashEffects.SpawnAtPosition(user.sprite.WorldCenter, user.CurrentGun.CurrentAngle, null, new Vector2?(Vector2.zero), new Vector2?(Vector2.zero), new float?(-0.05f), true, null, null, false);
            }
        }        
    }
}