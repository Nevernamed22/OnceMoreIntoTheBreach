using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using ItemAPI;

namespace NevernamedsItems
{
    public class SpareKey : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<SpareKey>(
            "Spare Key",
            "Important to Have",
            "Grants an extra key every floor.\nHaving a spare key is important, what if you lose the one you have?",
            "sparekey_icon");
            item.quality = PickupObject.ItemQuality.B;
            item.AddToSubShop(ItemBuilder.ShopType.Flynt);
        }
        private void OnNewFloor()
        {
            PlayerController player = this.Owner;
            LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(67).gameObject, player);
            if (Owner.HasPickupID(Gungeon.Game.Items["nn:spare_blank"].PickupObjectId))
            {
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(120).gameObject, player);
            }
        }

        public override void Pickup(PlayerController player)
        {
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
