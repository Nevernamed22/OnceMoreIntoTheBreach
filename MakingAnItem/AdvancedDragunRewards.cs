using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Gungeon;

namespace NevernamedsItems
{
    public class AdvancedDragunRewards : ETGModule
    {
        public override void Exit()
        {
        }
        public override void Start()
        {
        }
        public override void Init()
        {
            ETGMod.AIActor.OnPreStart += this.AIActorMods;
        }
        private void AIActorMods(AIActor enemy)
        {
            if (enemy != null && enemy.aiActor != null)
            {
                if (enemy.aiActor.EnemyGuid == "05b8afe0b6cc4fffa9dc6036fa24c8ec")
                {
                    enemy.healthHaver.OnDeath += this.handleDragunRewards;
                }
            }
        }
        private void handleDragunRewards(Vector2 direction)
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