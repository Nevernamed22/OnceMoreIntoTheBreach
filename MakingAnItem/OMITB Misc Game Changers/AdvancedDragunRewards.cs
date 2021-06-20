using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Gungeon;

namespace NevernamedsItems
{
    public class AdvancedDragunRewards
    {
        public static void handleDragunRewards()
        {
            PlayerController player = GameManager.Instance.PrimaryPlayer;
            Chest Black_Chest = GameManager.Instance.RewardManager.S_Chest;
            Black_Chest.IsLocked = true;
            Chest Synergy_Chest = GameManager.Instance.RewardManager.Synergy_Chest;
            Synergy_Chest.IsLocked = false;
            Chest SpawnedBlack = Chest.Spawn(Black_Chest, player.CurrentRoom.GetRandomVisibleClearSpot(1, 1));
            SpawnedBlack.lootTable.lootTable = (UnityEngine.Random.value <= 0.5f ? GameManager.Instance.RewardManager.GunsLootTable : GameManager.Instance.RewardManager.ItemsLootTable);
            SpawnedBlack.RegisterChestOnMinimap(SpawnedBlack.GetAbsoluteParentRoom());
            for (int i = 0; i < 2; i++)
            {
                Chest SpawnedSynergy = Chest.Spawn(Synergy_Chest, player.CurrentRoom.GetRandomVisibleClearSpot(1, 1));
            SpawnedSynergy.RegisterChestOnMinimap(SpawnedSynergy.GetAbsoluteParentRoom());
            }

        }
    }
}