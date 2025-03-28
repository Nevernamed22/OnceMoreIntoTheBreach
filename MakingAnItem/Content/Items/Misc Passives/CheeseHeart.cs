using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dungeonator;
using UnityEngine;
using Alexandria.ItemAPI;
using SaveAPI;

namespace NevernamedsItems
{
    class CheeseHeart : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<CheeseHeart>(
            "Cheese Heart",
            "Eat Your Heart Out",
            "Sprays cheese everywhere on hit." + "\n\nCarefully sculpted, and completely anatomically correct!",
            "cheeseheart_icon");
            item.quality = PickupObject.ItemQuality.C;
            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.FAILEDRATMAZE, true);
            CheeseHeartID = item.PickupObjectId;
        }
        public static int CheeseHeartID;
        public override void Pickup(PlayerController player)
        {
            player.OnReceivedDamage += OnHitEffect;
            base.Pickup(player);
        }
        public override void DisableEffect(PlayerController player)
        {
            if (player) { player.OnReceivedDamage -= OnHitEffect; }
            base.DisableEffect(player);
        }       
        private void OnHitEffect(PlayerController user)
        {
            DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(EasyGoopDefinitions.CheeseDef).TimedAddGoopCircle(Owner.sprite.WorldCenter, 10, 1, false);
        }
    }
}
