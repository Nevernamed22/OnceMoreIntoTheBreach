using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using ItemAPI;

namespace NevernamedsItems
{
    public class RiskyRing : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Risky Ring";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/riskyring_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<RiskyRing>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "This Ring Has Fangs";
            string longDesc = "More drops when at full HP, less drops when not." + "\n\nThis ring feels slightly irradiated.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.D; //D


        }
        private float currentHP, lastHP;
        private float currentArmour, lastArmour;
        private float currentGuns, lastGuns;
        private float GetModifierAmount(PlayerController owner, bool ShouldBePositive)
        {

            if (owner.characterIdentity != PlayableCharacters.Robot)
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
            else
            {
                if (owner.PlayerHasActiveSynergy("Double Risk, Double Reward")) return 6f;
                else return 3f;
            }
        }
        private void RecalculateShit()
        {
            //ETGModConsole.Log("Current HP Percent: " + Owner.healthHaver.GetCurrentHealthPercentage());
            bool atMaxHP;
            if (Owner.healthHaver.GetCurrentHealthPercentage() == 1f) { atMaxHP = true; }
            else { atMaxHP = false; }
            // ETGModConsole.Log("AtMaxHP: " + atMaxHP);
            AlterItemStats.RemoveStatFromPassive(this, PlayerStats.StatType.Coolness);
            Owner.stats.RecalculateStats(Owner, false, false);
            float amountToMod = GetModifierAmount(Owner, atMaxHP);
            // ETGModConsole.Log("Amount to mod: " + amountToMod);
            AlterItemStats.AddStatToPassive(this, PlayerStats.StatType.Coolness, amountToMod, StatModifier.ModifyMethod.ADDITIVE);
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
        protected override void OnDestroy()
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
        protected override void Update()
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