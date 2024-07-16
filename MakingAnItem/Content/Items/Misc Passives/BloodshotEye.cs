using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dungeonator;
using UnityEngine;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    class BloodshotEye : PassiveItem
    {
        public static void Init()
        {
            PassiveItem item = ItemSetup.NewItem<BloodshotEye>(
               "Bloodshot Eye",
               "Ow, Oof, Ouchie",
               "Slightly increases damage for every hit taken." + "\nEffect is permanent." + "\n\nLooks like you could use some eyedrops.",
               "bloodshoteye_icon") as PassiveItem;
            item.quality = PickupObject.ItemQuality.A;
            BloodshotEyeID = item.PickupObjectId;
        }
        public static int BloodshotEyeID;
        private void OnDMG(PlayerController user)
        {
            StatModifier dmgUp = new StatModifier()
            {
                statToBoost = PlayerStats.StatType.Damage,
                amount = 1.02f,
                modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE,
            };
            user.ownerlessStatModifiers.Add(dmgUp);
            user.stats.RecalculateStats(user);
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnReceivedDamage += this.OnDMG;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.OnReceivedDamage -= this.OnDMG;
            return debrisObject;
        }
        public override void OnDestroy()
        {
            if (Owner)
            {
                Owner.OnReceivedDamage -= this.OnDMG;
            }
            base.OnDestroy();
        }
    }
}