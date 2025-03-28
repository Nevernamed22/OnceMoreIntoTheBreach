using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dungeonator;
using UnityEngine;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    class ColdOne : PassiveItem
    {
        public static void Init()
        {
            PassiveItem item = ItemSetup.NewItem<ColdOne>(
               "Cold One",
               "Brewsky",
               "Cools off active items when taking damage."+"\n\nLizard blue flavour.",
               "coldone_icon") as PassiveItem;
            item.quality = PickupObject.ItemQuality.D;
        }

        private void Cooloff(PlayerController user)
        {
           if (user.CurrentItem != null)
            {
                user.CurrentItem.remainingRoomCooldown = Math.Max(0, user.CurrentItem.remainingRoomCooldown - 1);
                user.CurrentItem.remainingTimeCooldown = Mathf.Max(0f, user.CurrentItem.remainingTimeCooldown - 5f);
                user.CurrentItem.remainingDamageCooldown = Mathf.Max(0f, user.CurrentItem.remainingDamageCooldown - 500f);
            }
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnReceivedDamage += Cooloff;
        }
        public override void DisableEffect(PlayerController player)
        {
            if (player) player.OnReceivedDamage -= Cooloff;
            base.DisableEffect(player);
        } 
    }
}
