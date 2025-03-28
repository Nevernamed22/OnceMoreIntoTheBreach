﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    public class KeyBullwark : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<KeyBullwark>(
            "Key Bulwark",
            "Keyfensive Maneuver",
            "Converts all your keys into armour upon entering a new floor. Every key converted gives a small, permanent damage upgrade.",
            "keybulwark_icon");
            item.quality = PickupObject.ItemQuality.B;
            item.AddToSubShop(ItemBuilder.ShopType.Flynt);
          KeyBulwarkID =  item.PickupObjectId;
        }
        public static int KeyBulwarkID;
        private void OnNewFloor()
        {
            PlayerController player = this.Owner;
            int currentKeys = player.carriedConsumables.KeyBullets;
            player.healthHaver.Armor += currentKeys;
            float keysToDamage = currentKeys * 0.05f;
            float curDamage = player.stats.GetBaseStatValue(PlayerStats.StatType.Damage);
            float damageToGive = curDamage + keysToDamage;
            player.stats.SetBaseStatValue(PlayerStats.StatType.Damage, damageToGive, player);
            player.carriedConsumables.KeyBullets -= currentKeys;
        }

        public override void Pickup(PlayerController player)
        {
            bool hasntAlreadyBeenCollected = !this.m_pickedUpThisRun;
            if (hasntAlreadyBeenCollected)
            {
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(67).gameObject, player);
            }
            GameManager.Instance.OnNewLevelFullyLoaded += this.OnNewFloor;
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject result = base.Drop(player);
            GameManager.Instance.OnNewLevelFullyLoaded -= this.OnNewFloor;
            return result;
        }
        public override void OnDestroy()
        {
            GameManager.Instance.OnNewLevelFullyLoaded -= this.OnNewFloor;
            base.OnDestroy();
        }
    }
}
