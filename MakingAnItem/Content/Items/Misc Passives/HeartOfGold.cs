using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dungeonator;
using UnityEngine;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    class HeartOfGold : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<HeartOfGold>(
            "Heart of Gold",
            "Red Gold",
            "Squirts out some cash upon it's bearer taking damage." + "\n\nA small statue of a much larger and far more hostile golden heart discovered deep underground...",
            "heartofgold_icon");
            item.quality = PickupObject.ItemQuality.C;
            HeartOfGoldID = item.PickupObjectId;
        }

        public static int HeartOfGoldID;
        private void giveCash(PlayerController user)
        {
            int cashMoney = 10;
            if (Owner.PlayerHasActiveSynergy("Do-Gooder")) cashMoney *= 2;
            LootEngine.SpawnCurrency(user.sprite.WorldCenter, cashMoney);
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnReceivedDamage += this.giveCash;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.OnReceivedDamage -= this.giveCash;
            return debrisObject;
        }
        public override void OnDestroy()
        {
            if (Owner) Owner.OnReceivedDamage -= this.giveCash;
            base.OnDestroy();
        }
    }
}
