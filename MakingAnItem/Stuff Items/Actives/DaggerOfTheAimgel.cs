using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using ItemAPI;
using SaveAPI;

namespace NevernamedsItems
{
    class DaggerOfTheAimgel : PlayerItem
    {
        public static void Init()
        {
            string itemName = "Dagger of The Aimgels";
            string resourceName = "NevernamedsItems/Resources/daggeroftheaimgel_icon";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<DaggerOfTheAimgel>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Sacrifices Must Be Made";
            string longDesc = "Plunging this dagger into your flesh has irreversible side effects, but it also imbues within you with the rage of a thousand corrupted Aimgels, fallen from Bullet Heaven.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 5);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE);

            item.consumable = false;
            item.quality = ItemQuality.B;

            item.AddToSubShop(ItemBuilder.ShopType.Cursula);

            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.USEDFALLENANGELSHRINE, true);
        }

        public override void DoEffect(PlayerController user)
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
