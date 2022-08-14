using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;

namespace NevernamedsItems
{
    class ForsakenHeart : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Forsaken Heart";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/forsakenheart_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<ForsakenHeart>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "And lo, it shall embroil within";
            string longDesc = "A cursed heart embiggens the smallest of wretches.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE);

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.B;

            

            item.AddToSubShop(ItemBuilder.ShopType.Cursula);

            ForsakenHeartID = item.PickupObjectId;
        }
        public static int ForsakenHeartID;

        private float curse = 0f;
        private float lastCurse = -1f;
        public override void Update()
        {
            base.Update();
            this.EvaluateStats();
        }
        private void EvaluateStats()
        {
            bool flag = !base.Owner || !base.Owner.stats;
            if (!flag)
            {
                this.curse = this.GetTrueTotalCurse(base.Owner);
                bool coolnessAndLastCoolnessAreIdentical = this.curse == this.lastCurse;
                if (!coolnessAndLastCoolnessAreIdentical)
                {
                    this.RemoveStat(PlayerStats.StatType.Health);
                    double unconvertedRoundedCurse = Math.Round(this.curse * 0.40);
                    float convertedRoundedCurse = (float)unconvertedRoundedCurse;
                    this.AddStat(PlayerStats.StatType.Health, convertedRoundedCurse, StatModifier.ModifyMethod.ADDITIVE);
                    base.Owner.stats.RecalculateStats(base.Owner, true, false);
                    this.lastCurse = this.curse;
                }
            }
        }
        public float GetTrueTotalCurse(PlayerController player)
        {
            float num = player.stats.GetStatValue(PlayerStats.StatType.Curse);
            return num;
        }
        private void AddStat(PlayerStats.StatType statType, float amount, StatModifier.ModifyMethod method = StatModifier.ModifyMethod.ADDITIVE)
        {
            StatModifier statModifier = new StatModifier();
            statModifier.amount = amount;
            statModifier.statToBoost = statType;
            statModifier.modifyType = method;
            foreach (StatModifier statModifier2 in this.passiveStatModifiers)
            {
                bool flag = statModifier2.statToBoost == statType;
                if (flag)
                {
                    return;
                }
            }
            bool flag2 = this.passiveStatModifiers == null;
            if (flag2)
            {
                this.passiveStatModifiers = new StatModifier[]
                {
            statModifier
                };
                return;
            }
            this.passiveStatModifiers = this.passiveStatModifiers.Concat(new StatModifier[]
            {
        statModifier
            }).ToArray<StatModifier>();
        }
        private void RemoveStat(PlayerStats.StatType statType)
        {
            List<StatModifier> list = new List<StatModifier>();
            for (int i = 0; i < this.passiveStatModifiers.Length; i++)
            {
                bool flag = this.passiveStatModifiers[i].statToBoost != statType;
                if (flag)
                {
                    list.Add(this.passiveStatModifiers[i]);
                }
            }
            this.passiveStatModifiers = list.ToArray();
        }
    }
}
