using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;
using Alexandria.ItemAPI;
using SaveAPI;

namespace NevernamedsItems
{
    class OverpricedHeadband : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<OverpricedHeadband>(
            "Overpriced Headband",
            "Ultimate",
            "Apparently being rich = being cool these days." + "\n\nMaybe you should write a song about how rich you are.",
            "overpricedheadband_improved");
            item.quality = PickupObject.ItemQuality.C;
            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.PURCHASED_OVERPRICEDHEADBAND, true);
            OverpricedHeadbandID = item.PickupObjectId;
        }
        public static int OverpricedHeadbandID;
        private float currentCash, lastCash;
        public override void Update()
        {
            if (Owner)
            {

                currentCash = Owner.carriedConsumables.Currency;
                if (currentCash != lastCash)
                {
                    RemoveStat(PlayerStats.StatType.Coolness);
                    float dividedCurrency = currentCash / 25;
                    double dividedRoundedCurrency = Mathf.FloorToInt(dividedCurrency);
                    if (dividedRoundedCurrency > 0)
                    {
                        for (int i = 0; i < dividedRoundedCurrency; i++)
                        {
                            AddStat(PlayerStats.StatType.Coolness, 1f, StatModifier.ModifyMethod.ADDITIVE);
                        }
                    }
                    Owner.stats.RecalculateStats(Owner, false, false);
                    lastCash = currentCash;
                }
                else { return; }
            }
        }
        private void AddStat(PlayerStats.StatType statType, float amount, StatModifier.ModifyMethod method = StatModifier.ModifyMethod.ADDITIVE)
        {
            /*foreach (var m in passiveStatModifiers)
            {
                if (m.statToBoost == statType) return; //don't add duplicates
            }*/

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
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            return debrisObject;
        }
        public override void OnDestroy()
        {
            //Owner.SetIsFlying(false, "shade", true, false);
            base.OnDestroy();
        }
    }
}
