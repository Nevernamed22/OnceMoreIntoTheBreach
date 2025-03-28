﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;
using SaveAPI;

namespace NevernamedsItems
{
    class DaggerOfTheAimgel : PlayerItem
    {
        public static void Init()
        {
            PlayerItem item = ItemSetup.NewItem<DaggerOfTheAimgel>(
            "Dagger of The Aimgels",
            "Sacrifices Must Be Made",
            "Plunging this dagger into your flesh has irreversible side effects, but it also imbues within you with the rage of a thousand corrupted Aimgels, fallen from Bullet Heaven.",
            "daggeroftheaimgel_icon") as PlayerItem;
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 5);
            item.AddPassiveStatModifier(PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE);

            item.consumable = false;
            item.quality = ItemQuality.B;

            item.AddToSubShop(ItemBuilder.ShopType.Cursula);

            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.USEDFALLENANGELSHRINE, true);
        }

        public override void DoEffect(PlayerController user)
        {
            AkSoundEngine.PostEvent("Play_VO_lichA_cackle_01", base.gameObject);

            if (user.ForceZeroHealthState) { user.healthHaver.Armor -= 2; }
            else
            {
                StatModifier hp = new StatModifier()
                {
                    amount = -1f,
                    statToBoost = PlayerStats.StatType.Health,
                    modifyType = StatModifier.ModifyMethod.ADDITIVE,
                };
                user.ownerlessStatModifiers.Add(hp);
            }

            StatModifier curse = new StatModifier()
            {
                amount = 1.5f,
                statToBoost = PlayerStats.StatType.Curse,
                modifyType = StatModifier.ModifyMethod.ADDITIVE,
            };
            StatModifier dmg = new StatModifier()
            {
                amount = 1.20f,
                statToBoost = PlayerStats.StatType.Damage,
                modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE,
            };

            user.ownerlessStatModifiers.Add(dmg);
            user.ownerlessStatModifiers.Add(curse);
            user.stats.RecalculateStats(user);
        }
        public override bool CanBeUsed(PlayerController user)
        {
            if (user.ForceZeroHealthState) { return user.healthHaver.Armor > 2; }
            else return user.healthHaver.GetMaxHealth() > 1f;
        }
    }
}
