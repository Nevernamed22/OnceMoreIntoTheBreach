using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using ItemAPI;

namespace NevernamedsItems
{
    class DaggerOfTheAimgel : PlayerItem
    {
        //Call this method from the Start() method of your ETGModule extension class
        public static void Init()
        {
            //The name of the item
            string itemName = "Dagger of The Aimgels";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it.
            string resourceName = "NevernamedsItems/Resources/daggeroftheaimgel_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<DaggerOfTheAimgel>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Sacrifices Must Be Made";
            string longDesc = "Plunging this dagger into your flesh has irreversible side effects, but it also imbues within you with the rage of a thousand corrupted Aimgels, fallen from Bullet Heaven.";

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

            item.AddToSubShop(ItemBuilder.ShopType.Cursula);
        }

        //Add the item's functionality down here! I stole most of this from the Stuffed Star active item code!

        protected override void DoEffect(PlayerController user)
        {
            //Activates the effect
            PlayableCharacters characterIdentity = user.characterIdentity;
            
            if (characterIdentity != PlayableCharacters.Robot)
            {
                AkSoundEngine.PostEvent("Play_VO_lichA_cackle_01", base.gameObject);
                float MaxHP2 = user.stats.GetBaseStatValue(PlayerStats.StatType.Health);
                MaxHP2 -= 1;
                user.stats.SetBaseStatValue(PlayerStats.StatType.Health, MaxHP2, user);
                float currentCurse = user.stats.GetBaseStatValue(PlayerStats.StatType.Curse);
                currentCurse += 1f;
                user.stats.SetBaseStatValue(PlayerStats.StatType.Curse, currentCurse, user);
                float currentDamage = user.stats.GetBaseStatValue(PlayerStats.StatType.Damage);
                currentDamage *= 1.20f;
                user.stats.SetBaseStatValue(PlayerStats.StatType.Damage, currentDamage, user);
            }
            else if (characterIdentity == PlayableCharacters.Robot)
            {
                AkSoundEngine.PostEvent("Play_VO_lichA_cackle_01", base.gameObject);
                user.healthHaver.Armor = user.healthHaver.Armor - 2;
                float currentCurse = user.stats.GetBaseStatValue(PlayerStats.StatType.Curse);
                currentCurse += 1f;
                user.stats.SetBaseStatValue(PlayerStats.StatType.Curse, currentCurse, user);
                float currentDamage = user.stats.GetBaseStatValue(PlayerStats.StatType.Damage);
                currentDamage *= 1.20f;
                user.stats.SetBaseStatValue(PlayerStats.StatType.Damage, currentDamage, user);
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
