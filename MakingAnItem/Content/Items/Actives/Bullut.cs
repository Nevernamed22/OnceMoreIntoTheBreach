using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dungeonator;
using Alexandria.ItemAPI;
using UnityEngine;

namespace NevernamedsItems
{
    class Bullut : PlayerItem
    {
        public static void Init()
        {
            PlayerItem item = ItemSetup.NewItem<Bullut>(
              "Bullut",
              "Supposed Delicacy",
              "Bullet Embryos, boiled inside their shells. Apparently this is supposed to be food." + "\n\nEating this makes the Gundead, Kaliber, and pretty much everyone else mad at you, though the Gunsling King describes it as a... rewarding experience.",
              "bullut_icon") as PlayerItem;
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.None, 1000);
            item.consumable = true;
            item.quality = ItemQuality.D;
        }
        public override void DoEffect(PlayerController user)
        {
            ChangeStatPermanent(user, PlayerStats.StatType.Curse, 2, StatModifier.ModifyMethod.ADDITIVE);
            ChangeStatPermanent(user, PlayerStats.StatType.EnemyProjectileSpeedMultiplier, 1.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            for (int i = 0; i < 3; i++)
            {
                var locationToSpawn = user.CurrentRoom.GetRandomVisibleClearSpot(2, 2);
                Chest spawnedChest = GameManager.Instance.RewardManager.SpawnRewardChestAt(locationToSpawn);
                spawnedChest.RegisterChestOnMinimap(spawnedChest.GetAbsoluteParentRoom());
            }
            //SYNERGY CHEST
            Chest Synergy_Chest = GameManager.Instance.RewardManager.Synergy_Chest;
            Synergy_Chest.IsLocked = true;           
            Chest SpawnedSynergy = Chest.Spawn(Synergy_Chest, user.CurrentRoom.GetRandomVisibleClearSpot(2, 2));
            SpawnedSynergy.RegisterChestOnMinimap(SpawnedSynergy.GetAbsoluteParentRoom());
            //RED CHEST
            Chest Red_Chest = GameManager.Instance.RewardManager.A_Chest;
            Red_Chest.IsLocked = true;
            Red_Chest.ChestType = Chest.GeneralChestType.UNSPECIFIED;
            Chest SpawnedRed = Chest.Spawn(Red_Chest, user.CurrentRoom.GetRandomVisibleClearSpot(2, 2));
            SpawnedRed.lootTable.lootTable = (UnityEngine.Random.value <= 0.5f ? GameManager.Instance.RewardManager.GunsLootTable : GameManager.Instance.RewardManager.ItemsLootTable);
            SpawnedRed.RegisterChestOnMinimap(SpawnedRed.GetAbsoluteParentRoom());
            //BLACK CHEST
            Chest Black_Chest = GameManager.Instance.RewardManager.S_Chest;
            Black_Chest.IsLocked = true;
            Chest SpawnedBlack = Chest.Spawn(Black_Chest, user.CurrentRoom.GetRandomVisibleClearSpot(2, 2));
            SpawnedBlack.lootTable.lootTable = (UnityEngine.Random.value <= 0.5f ? GameManager.Instance.RewardManager.GunsLootTable : GameManager.Instance.RewardManager.ItemsLootTable);
            SpawnedBlack.RegisterChestOnMinimap(SpawnedBlack.GetAbsoluteParentRoom());
            //GIVE KEYS
            for (int i = 0; i < 10; i++) LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(67).gameObject, user);
        }
        private void ChangeStatPermanent(PlayerController target, PlayerStats.StatType statToChance, float amount, StatModifier.ModifyMethod modifyMethod)
        {
            StatModifier statModifier = new StatModifier();
            statModifier.amount = amount;
            statModifier.modifyType = modifyMethod;
            statModifier.statToBoost = statToChance;
            target.ownerlessStatModifiers.Add(statModifier);
            target.stats.RecalculateStats(target, false, false);
        }
    }
}
