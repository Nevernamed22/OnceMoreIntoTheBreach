using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    public class LooseChange : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<LooseChange>(
            "Loose Change",
            "Cash Money",
            "Gives some money every floor. Amount increases for every floor you've been to previously." + "\n\nGo and buy yourself something nice.",
            "loosechange_icon");
            item.quality = PickupObject.ItemQuality.D;
        }

        int floorsVisited;
        private void OnNewFloor()
        {
            PlayerController player = this.Owner;
            int moneyToGive = floorsVisited * 10;
            if (Owner.PlayerHasActiveSynergy("Lost, Never Found"))
            {
                moneyToGive += 5;
            }
            player.carriedConsumables.Currency += moneyToGive;
            floorsVisited += 1;
        }

        public override void Pickup(PlayerController player)
        {
            bool hasntAlreadyBeenCollected = !this.m_pickedUpThisRun;
            if (hasntAlreadyBeenCollected)
            {
                player.carriedConsumables.Currency += 10;
                floorsVisited = 2;
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