﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using ItemAPI;

namespace NevernamedsItems
{
    public class HellfireRounds : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Hellfire Rounds";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/hellfirerounds_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<HellfireRounds>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Lead and Brimstone";
            string longDesc = "These bullets hit harder the closer they get to the fires of Bullet Hell." + "\n\nMany years ago, the red flames of the pit were used one single time to soften metal for the Blacksmith's anvil. That was a bad idea.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.S;
        }
        public float currentDamageMod = 1;
        private void OnNewFloor()
        {
            CalculateStats();
        }
        private void CalculateStats()
        {
            try
            {
                RemoveStat(PlayerStats.StatType.Damage);
                Owner.stats.RecalculateStats(Owner, true, false);
                float damageMultiplier = 1;
                PlayerController player = this.Owner;
                bool detectedFloor = false;
                switch (GameManager.Instance.Dungeon.tileIndices.tilesetId)
                {
                    case GlobalDungeonData.ValidTilesets.CASTLEGEON:
                        damageMultiplier = 1.083f;
                        detectedFloor = true;
                        break;
                    case GlobalDungeonData.ValidTilesets.SEWERGEON:
                        detectedFloor = true;
                        damageMultiplier = 1.124f;
                        break;
                    case GlobalDungeonData.ValidTilesets.GUNGEON:
                        detectedFloor = true;
                        damageMultiplier = 1.166f;
                        break;
                    case GlobalDungeonData.ValidTilesets.CATHEDRALGEON:
                        detectedFloor = true;
                        damageMultiplier = 1.207f;
                        break;
                    case GlobalDungeonData.ValidTilesets.MINEGEON:
                        detectedFloor = true;
                        damageMultiplier = 1.249f;
                        break;
                    case GlobalDungeonData.ValidTilesets.RATGEON:
                        detectedFloor = true;
                        damageMultiplier = 1.290f;
                        break;
                    case GlobalDungeonData.ValidTilesets.CATACOMBGEON:
                        detectedFloor = true;
                        damageMultiplier = 1.332f;
                        break;
                    case GlobalDungeonData.ValidTilesets.OFFICEGEON:
                        detectedFloor = true;
                        damageMultiplier = 1.373f;
                        break;
                    case GlobalDungeonData.ValidTilesets.FORGEGEON:
                        detectedFloor = true;
                        damageMultiplier = 1.415f;
                        break;
                    case GlobalDungeonData.ValidTilesets.HELLGEON:
                        detectedFloor = true;
                        damageMultiplier = 1.5f;
                        break;
                    case GlobalDungeonData.ValidTilesets.JUNGLEGEON:
                        detectedFloor = true;
                        damageMultiplier = 1.124f;
                        break;
                    case GlobalDungeonData.ValidTilesets.BELLYGEON:
                        detectedFloor = true;
                        damageMultiplier = 1.207f;
                        break;
                    case GlobalDungeonData.ValidTilesets.WESTGEON:
                        detectedFloor = true;
                        damageMultiplier = 1.373f;
                        break;
                }
                if (!detectedFloor)
                {
                    damageMultiplier = currentDamageMod + 0.083f;
                }
                //ETGModConsole.Log("Damage mod is " + damageMultiplier);
                AddStat(PlayerStats.StatType.Damage, damageMultiplier, StatModifier.ModifyMethod.MULTIPLICATIVE);
                Owner.stats.RecalculateStats(Owner, true, false);
                currentDamageMod = damageMultiplier;
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
                ETGModConsole.Log(e.StackTrace);
            }
        }

        public override void Pickup(PlayerController player)
        {
            GameManager.Instance.OnNewLevelFullyLoaded += this.CalculateStats;
            base.Pickup(player);
            CalculateStats();
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject result = base.Drop(player);
            GameManager.Instance.OnNewLevelFullyLoaded -= this.CalculateStats;
            return result;
        }
        public override void OnDestroy()
        {
            GameManager.Instance.OnNewLevelFullyLoaded -= this.CalculateStats;
            base.OnDestroy();
        }
        private void AddStat(PlayerStats.StatType statType, float amount, StatModifier.ModifyMethod method = StatModifier.ModifyMethod.ADDITIVE)
        {
            StatModifier modifier = new StatModifier
            {
                amount = amount,
                statToBoost = statType,
                modifyType = method
            };
            if (this.passiveStatModifiers == null)
                this.passiveStatModifiers = new StatModifier[] { modifier };
            else
                this.passiveStatModifiers = this.passiveStatModifiers.Concat(new StatModifier[] { modifier }).ToArray();
        }

        private void RemoveStat(PlayerStats.StatType statType)
        {
            var newModifiers = new List<StatModifier>();
            for (int i = 0; i < passiveStatModifiers.Length; i++)
            {
                if (passiveStatModifiers[i].statToBoost != statType)
                    newModifiers.Add(passiveStatModifiers[i]);
            }
            this.passiveStatModifiers = newModifiers.ToArray();
        }
    }
}
