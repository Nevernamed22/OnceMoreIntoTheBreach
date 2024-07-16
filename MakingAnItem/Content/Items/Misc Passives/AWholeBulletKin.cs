using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alexandria.ItemAPI;
using UnityEngine;

namespace NevernamedsItems
{
    public class AWholeBulletKin : PassiveItem
    {
        public static void Init()
        {
            PassiveItem item = ItemSetup.NewItem<AWholeBulletKin>(
              "A Whole Bullet Kin",
              "What.",
              "This is just... a bullet kin...\n\n" + "Is he moving?... Is he alive? Comatose? Also WHAT?",
              "awholebulletkin_icon") as PassiveItem;
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.ReloadSpeed, 0.8f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            item.quality = PickupObject.ItemQuality.D;
            WholeBulletKinID = item.PickupObjectId;
            item.SetTag("non_companion_living_item");
        }
        public static int WholeBulletKinID;
    }
}
