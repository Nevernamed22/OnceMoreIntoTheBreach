﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;
using SaveAPI;

namespace NevernamedsItems
{
    public class RiskyRing : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<RiskyRing>(
            "Risky Ring",
            "This Ring Has Fangs",
            "More drops when at full HP, less drops when not." + "\n\nThis ring feels slightly irradiated.",
            "riskyring_icon");
            item.quality = PickupObject.ItemQuality.D; 
            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.HASBEENDAMAGEDBYRISKRIFLE, true);
        }
        private float currentHP, lastHP;
        private float currentArmour, lastArmour;
        private float currentGuns, lastGuns;
        private float GetModifierAmount(PlayerController owner, bool ShouldBePositive)
        {
            if (owner.ForceZeroHealthState)
            {
                if (owner.PlayerHasActiveSynergy("Double Risk, Double Reward")) return 6f;
                else return 3f;
            }
            else
            {
                if (ShouldBePositive)
                {
                    if (owner.PlayerHasActiveSynergy("Double Risk, Double Reward")) return 6f;
                    else return 3f;
                }
                else
                {
                    float temp;
                    if (owner.PlayerHasActiveSynergy("Double Risk, Double Reward")) temp = 6f;
                    else temp = 3f;
                    if (owner.stats.GetStatValue(PlayerStats.StatType.Coolness) >= temp) { return temp * -1f; }
                    else { return owner.stats.GetStatValue(PlayerStats.StatType.Coolness) * -1f; }
                }
            }
            
        }
        private void RecalculateShit()
        {
            bool atMaxHP;
            if (Owner.healthHaver.GetCurrentHealthPercentage() == 1f) { atMaxHP = true; }
            else { atMaxHP = false; }
            this.RemovePassiveStatModifier(PlayerStats.StatType.Coolness);
            Owner.stats.RecalculateStats(Owner, false, false);
            float amountToMod = GetModifierAmount(Owner, atMaxHP);
            this.AddPassiveStatModifier( PlayerStats.StatType.Coolness, amountToMod, StatModifier.ModifyMethod.ADDITIVE);
            Owner.stats.RecalculateStats(Owner, false, false);
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnRoomClearEvent += this.HandleChestSpawnSynergy;
            RecalculateShit();
        }
        public override DebrisObject Drop(PlayerController player)
        {
            player.OnRoomClearEvent -= this.HandleChestSpawnSynergy;
            return base.Drop(player);
        }
        public override void OnDestroy()
        {
            if (Owner)
            {
                Owner.OnRoomClearEvent -= this.HandleChestSpawnSynergy;
            }
            base.OnDestroy();
        }
        private void HandleChestSpawnSynergy(PlayerController guy)
        {
            if (guy != null && guy.PlayerHasActiveSynergy("Ultra Mutation"))
            {
                if (UnityEngine.Random.value <= 0.05f)
                {
                    var locationToSpawn = guy.CurrentRoom.GetRandomVisibleClearSpot(2, 2);
                    Chest spawnedChest = GameManager.Instance.RewardManager.SpawnRewardChestAt(locationToSpawn);
                    spawnedChest.RegisterChestOnMinimap(spawnedChest.GetAbsoluteParentRoom());
                }
            }

        }
        public override void Update()
        {
            if (Owner)
            {
                currentHP = Owner.healthHaver.GetCurrentHealth();
                currentArmour = Owner.healthHaver.Armor;
                currentGuns = Owner.inventory.AllGuns.Count;
                if (currentHP != lastHP || currentArmour != lastArmour || currentGuns != lastGuns)
                {
                    RecalculateShit();
                    lastHP = currentHP;
                    lastArmour = currentArmour;
                    lastGuns = currentGuns;
                }

                if (Owner.IsInCombat && this.CanBeDropped) { this.CanBeDropped = false; }
                else if (!Owner.IsInCombat && !this.CanBeDropped) { this.CanBeDropped = true; }
            }
            base.Update();
        }
    }
}