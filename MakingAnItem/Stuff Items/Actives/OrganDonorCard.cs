using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using ItemAPI;

namespace NevernamedsItems
{
    class OrganDonorCard : PlayerItem
    {
        //Call this method from the Start() method of your ETGModule extension class
        public static void Init()
        {
            //The name of the item
            string itemName = "Organ Donor Card";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it.
            string resourceName = "NevernamedsItems/Resources/organdonorcard_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<OrganDonorCard>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Gift of Life";
            string longDesc = "Donate your hearts to someone who needs them." + "\n\nCompensates you in a small amount of cash, and extra Active Item Storage." + "\n\nNot reccomended for use by perverted Turtles.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"kts" here is the item pool. In the console you'd type kts:sweating_bullets
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 5);

            //Adds a passive modifier, like curse, coolness, damage, etc. to the item. Works for passives and actives.
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.AdditionalItemCapacity, 1f, StatModifier.ModifyMethod.ADDITIVE);

            //Set some other fields
            item.consumable = false;
            item.quality = ItemQuality.D;

            item.AddToSubShop(ItemBuilder.ShopType.Trorc);
        }

        //Add the item's functionality down here! I stole most of this from the Stuffed Star active item code!

        protected override void DoEffect(PlayerController user)
        {
            int cashToGive = 20;
            if (LastOwner.HasPickupID(Gungeon.Game.Items["nn:rusty_casing"].PickupObjectId)) cashToGive *= 2;
            if (LastOwner.HasPickupID(Gungeon.Game.Items["nn:heart_of_gold"].PickupObjectId)) cashToGive *= 2;
            for (int i = 0; i < cashToGive; i++)
            {
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(68).gameObject, LastOwner);
            }

            //Activates the effect
            PlayableCharacters characterIdentity = user.characterIdentity;

            if (characterIdentity != PlayableCharacters.Robot)
            {
                float MaxHP2 = user.stats.GetBaseStatValue(PlayerStats.StatType.Health);
                MaxHP2 -= 1;
                user.stats.SetBaseStatValue(PlayerStats.StatType.Health, MaxHP2, user);
                float currentStat = user.stats.GetBaseStatValue(PlayerStats.StatType.AdditionalItemCapacity);
                currentStat += 1f;
                user.stats.SetBaseStatValue(PlayerStats.StatType.AdditionalItemCapacity, currentStat, user);
            }
            else if (characterIdentity == PlayableCharacters.Robot)
            {
                user.healthHaver.Armor = user.healthHaver.Armor - 2;
                float currentStat = user.stats.GetBaseStatValue(PlayerStats.StatType.AdditionalItemCapacity);
                currentStat += 1f;
                user.stats.SetBaseStatValue(PlayerStats.StatType.AdditionalItemCapacity, currentStat, user);
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
                return user.healthHaver.GetMaxHealth() > 1f;
            }

        }
    }
}