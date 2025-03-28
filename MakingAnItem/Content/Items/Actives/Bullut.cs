using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dungeonator;
using Alexandria.ItemAPI;
using UnityEngine;
using Alexandria.ChestAPI;

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
            ChestUtility.SpawnChestEasy(user.CurrentRoom.GetRandomVisibleClearSpot(2, 2), 
                ChestUtility.ChestTier.SYNERGY, 
                true, //Locked
                Chest.GeneralChestType.UNSPECIFIED
             );

            //RED CHEST
            ChestUtility.SpawnChestEasy(user.CurrentRoom.GetRandomVisibleClearSpot(2, 2),
                ChestUtility.ChestTier.RED,
                true, //Locked
                Chest.GeneralChestType.UNSPECIFIED
             );

            //BLACK CHEST
            ChestUtility.SpawnChestEasy(user.CurrentRoom.GetRandomVisibleClearSpot(2, 2),
                ChestUtility.ChestTier.BLACK,
                true, //Locked
                Chest.GeneralChestType.UNSPECIFIED
             );

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
