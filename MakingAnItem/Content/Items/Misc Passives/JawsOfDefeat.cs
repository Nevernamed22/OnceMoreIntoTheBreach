using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Alexandria.ItemAPI;
using SaveAPI;
using UnityEngine;

namespace NevernamedsItems
{
    public class JawsOfDefeat : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<JawsOfDefeat>(
            "Jaws Of Defeat",
            "Press To See Graveyard",
            "Increases Damage and Firerate by 0.5% for every death on the current save file, up to 1000 deaths." + "\n\nA charm worn by the very first adventurer... ever.",
            "jawsofdefeat_icon");
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 3f, StatModifier.ModifyMethod.ADDITIVE);
            item.quality = PickupObject.ItemQuality.S;
            JawsOfDefeatID = item.PickupObjectId;
            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.BOSSRUSH_SHADE, true);
        }
        public override void Pickup(PlayerController player)
        {
            if (!m_pickedUpThisRun)
            {
                float deaths = GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.NUMBER_DEATHS);
                if (deaths > 0)
                {
                    float multiplier = (0.005f * deaths) + 1;
                    float finalMult = Mathf.Min(multiplier, 6);
                    this.AddPassiveStatModifier( PlayerStats.StatType.Damage, finalMult, StatModifier.ModifyMethod.MULTIPLICATIVE);
                    this.AddPassiveStatModifier( PlayerStats.StatType.RateOfFire, finalMult, StatModifier.ModifyMethod.MULTIPLICATIVE);
                }
            }
            base.Pickup(player);
        }
        public static int JawsOfDefeatID;
    }
}
