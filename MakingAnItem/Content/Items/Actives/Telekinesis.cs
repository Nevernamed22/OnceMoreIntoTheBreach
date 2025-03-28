﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dungeonator;
using Alexandria.ItemAPI;
using UnityEngine;

namespace NevernamedsItems
{
    class Telekinesis : PlayerItem
    {
        public static void Init()
        {
            PlayerItem item = ItemSetup.NewItem<Telekinesis>(
              "Telekinesis",
              "Power Of The Mind",
              "Pushes all enemies in the direction aimed." + "\n\nOne hell of a headache.",
              "telekinesis_icon") as PlayerItem;          
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 2f);
            item.quality = ItemQuality.D;
            item.AddToSubShop(ItemBuilder.ShopType.Cursula);
        }
        public override void DoEffect(PlayerController user)
        {
            Vector2 vector = user.CenterPosition;
            Vector2 normalized = (user.unadjustedAimPoint.XY() - vector).normalized;
            List<AIActor> activeEnemies = user.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            if (activeEnemies != null)
            {
                for (int i = 0; i < activeEnemies.Count; i++)
                {
                    AIActor aiactor = activeEnemies[i];
                    if (aiactor)
                    {
                        if (aiactor.knockbackDoer) aiactor.knockbackDoer.ApplyKnockback(normalized, 100, false);
                    }
                }
            }
        }
        public override bool CanBeUsed(PlayerController user)
        {
            if (user && user.CurrentGun != null)
            {
                return true;
            }
            else return false;
        }
    }
}
