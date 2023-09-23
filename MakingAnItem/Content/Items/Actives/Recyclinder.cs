using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ItemAPI;
using SaveAPI;
using UnityEngine;

namespace NevernamedsItems
{
    class Recyclinder : PlayerItem
    {
        public static void Init()
        {
            PlayerItem item = ItemSetup.NewItem<Recyclinder>(
            "Recyclinder",
            "Lean Green Machine",
            "An environmentally friendly alternative to the methane-belching Gun Munchers, the Recyclinder uses proprietary technology to convert guns into items of equal quality. Waste not, want not." + "\n\nIt's probably nanites. It's ALWAYS nanites with these things, right? I'm not just going cazy?",
            "recyclinder_icon") as PlayerItem;
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 200);
            item.consumable = false;
            item.quality = ItemQuality.A;
            item.AddToSubShop(ItemBuilder.ShopType.Trorc);
            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.PURCHASED_RECYCLINDER, true);
            RecyclinderID = item.PickupObjectId;
        }
        public static int RecyclinderID;

        PickupObject.ItemQuality itemToGiveQuality = PickupObject.ItemQuality.D;
        public override void DoEffect(PlayerController user)
        {
            if (user.CharacterUsesRandomGuns)
            {
                itemToGiveQuality = PickupObject.ItemQuality.D;
                spawnRecycledItem();
                itemToGiveQuality = PickupObject.ItemQuality.C;
                spawnRecycledItem();
                itemToGiveQuality = PickupObject.ItemQuality.B;
                spawnRecycledItem();
                itemToGiveQuality = PickupObject.ItemQuality.A;
                spawnRecycledItem();
                itemToGiveQuality = PickupObject.ItemQuality.S;
                spawnRecycledItem();
                user.RemoveActiveItem(this.PickupObjectId);
            }
            else
            {
                if (user.CurrentGun.CanActuallyBeDropped(user))
                {
                    Gun currentGun = user.CurrentGun;
                    PickupObject.ItemQuality itemQuality = currentGun.quality;
                    user.inventory.DestroyCurrentGun();
                    if (currentGun.quality == PickupObject.ItemQuality.D)
                    {
                        itemToGiveQuality = PickupObject.ItemQuality.D;
                        spawnRecycledItem();
                    }
                    else if (currentGun.quality == PickupObject.ItemQuality.C)
                    {
                        itemToGiveQuality = PickupObject.ItemQuality.C;
                        spawnRecycledItem();
                    }
                    else if (currentGun.quality == PickupObject.ItemQuality.B)
                    {
                        itemToGiveQuality = PickupObject.ItemQuality.B;
                        spawnRecycledItem();
                    }
                    else if (currentGun.quality == PickupObject.ItemQuality.A)
                    {
                        itemToGiveQuality = PickupObject.ItemQuality.A;
                        spawnRecycledItem();
                    }
                    else if (currentGun.quality == PickupObject.ItemQuality.S)
                    {
                        itemToGiveQuality = PickupObject.ItemQuality.S;
                        spawnRecycledItem();
                    }
                    else
                    {
                        LootEngine.SpawnItem(PickupObjectDatabase.GetById(127).gameObject, LastOwner.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
                    }
                }
                else return;
            }
        }

        private void spawnRecycledItem()
        {
            PickupObject itemOfTypeAndQuality = LootEngine.GetItemOfTypeAndQuality<PickupObject>(itemToGiveQuality, GameManager.Instance.RewardManager.ItemsLootTable, false);
            LootEngine.SpawnItem(itemOfTypeAndQuality.gameObject, LastOwner.specRigidbody.UnitCenter, Vector2.left, 1f, false, true, false);
        }
        public override bool CanBeUsed(PlayerController user)
        {
            if (user.CurrentGun.CanActuallyBeDropped(user))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
