using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    public class BagOfHolding : PassiveItem
    {
        public static void Init()
        {
            PassiveItem item = ItemSetup.NewItem<BagOfHolding>(
               "Bag of Holding",
               "Space Gallore",
               "Drastically increases active item storage." + "\n\nThe mad wizard Alben Smallbore theorised that bags such as these could be turned into violent explosive devices if they were ever punctured. Sadly, his research was never realised.",
               "bagofholding_icon") as PassiveItem;

            item.AddPassiveStatModifier( PlayerStats.StatType.AdditionalItemCapacity, 10f, StatModifier.ModifyMethod.ADDITIVE);

            item.quality = PickupObject.ItemQuality.A;
            item.AddToSubShop(ItemBuilder.ShopType.Cursula);
        }
        public override void Pickup(PlayerController player)
        {
            bool pickemmed = m_pickedUpThisRun;
            base.Pickup(player);
            if (!pickemmed)
            {
                PlayerItem itemOfTypeAndQuality = LootEngine.GetItemOfTypeAndQuality<PlayerItem>(ItemQuality.A, GameManager.Instance.RewardManager.ItemsLootTable, true);
                LootEngine.TryGivePrefabToPlayer(itemOfTypeAndQuality.gameObject, Owner, false);
                //LootEngine.SpawnItem(itemOfTypeAndQuality.gameObject, LastOwner.specRigidbody.UnitCenter, Vector2.left, 1f, false, true, false);
            }
        }
    }

}
