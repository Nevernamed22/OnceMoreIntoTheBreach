using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;
using SaveAPI;

namespace NevernamedsItems
{
    public class SpringloadedChamber : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<SpringloadedChamber>(
            "Springloaded Chamber",
            "Marvellous Mechanism",
            "Increases damage by 30% for the first half of the clip, but decreases it by 20% for the second." + "\n\nA miraculous clockwork doodad cannibalised from the Wind Up Gun. Proof that springs are, and will always be, the best form of potential energy.",
            "springloadedchamber_icon");
            item.AddPassiveStatModifier( PlayerStats.StatType.ReloadSpeed, 0.8f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            item.quality = PickupObject.ItemQuality.B; 
            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.PURCHASED_SPRINGLOADEDCHAMBER, true);
            item.AddItemToTrorcMetaShop(19);
            ID = item.PickupObjectId;
        }
        private int currentClip, lastClip;
        private Gun currentGun, lastGun;
        public static int ID;
        public override void Update()
        {
            if (Owner)
            {
                currentClip = Owner.CurrentGun.ClipShotsRemaining;
                currentGun = Owner.CurrentGun;
                if (currentClip != lastClip || currentGun != lastGun)
                {
                    int clipSizeThing = 2;
                    if (Owner.HasPickupID(334)) clipSizeThing = 4;
                    if (Owner.CurrentGun.ClipShotsRemaining > Owner.CurrentGun.ClipCapacity / clipSizeThing)
                    {
                        //ETGModConsole.Log("dmg up");
                        DamageBonusState();
                    }
                    else
                    {
                        //ETGModConsole.Log("dmg down");
                        DamageDownState();
                    }
                    lastClip = currentClip;
                    lastGun = currentGun;
                }

            }
        }
        private bool DamageIsUP;
        private void DamageBonusState()
        {
            if (!DamageIsUP)
            {
                RemoveStat(PlayerStats.StatType.Damage);
                RemoveStat(PlayerStats.StatType.PlayerBulletScale);                
                AddStat(PlayerStats.StatType.Damage, 1.3f, StatModifier.ModifyMethod.MULTIPLICATIVE);
                AddStat(PlayerStats.StatType.PlayerBulletScale, 1.2f, StatModifier.ModifyMethod.MULTIPLICATIVE);
                DamageIsUP = true;
                Owner.stats.RecalculateStats(Owner, true, false);
            }
        }
        private void DamageDownState()
        {
            if (DamageIsUP)
            {
                RemoveStat(PlayerStats.StatType.Damage);
                RemoveStat(PlayerStats.StatType.PlayerBulletScale);
                float damageMod = 0.8f;
                float scaleMod = 0.8f;
                if (Owner.HasPickupID(69))
                {
                    damageMod = 1;
                    scaleMod = 1;
                }
                AddStat(PlayerStats.StatType.PlayerBulletScale, scaleMod, StatModifier.ModifyMethod.MULTIPLICATIVE);
                AddStat(PlayerStats.StatType.Damage, damageMod, StatModifier.ModifyMethod.MULTIPLICATIVE);
                Owner.stats.RecalculateStats(Owner, true, false);
                DamageIsUP = false;
            }
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
