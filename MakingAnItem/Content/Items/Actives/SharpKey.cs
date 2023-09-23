﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    class SharpKey : PlayerItem
    {
        public static void Init()
        {
            PlayerItem item = ItemSetup.NewItem<SharpKey>(
            "Sharp Key",
            "SaKeyfice",
            "This key is hungry for sustenance so that it may lay its eggs, and spawn the next generation of keys.",
            "sharpkey_improved") as PlayerItem;
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 5);
            item.AddPassiveStatModifier( PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE);
            item.consumable = false;
            item.quality = ItemQuality.B;
            item.AddToSubShop(ItemBuilder.ShopType.Flynt);
        }

        public override void DoEffect(PlayerController user)
        {
                AkSoundEngine.PostEvent("Play_OBJ_goldkey_pickup_01", base.gameObject);
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(67).gameObject, user);

            if (user.ForceZeroHealthState) { user.healthHaver.Armor -= 2; }
            else { user.healthHaver.ApplyHealing(-1.5f); }
        }

        public override bool CanBeUsed(PlayerController user)
        {
            if (user.ForceZeroHealthState) { return user.healthHaver.Armor > 2; }
            else { return user.healthHaver.GetCurrentHealth() > 1.5f; }
        }
    }
}
