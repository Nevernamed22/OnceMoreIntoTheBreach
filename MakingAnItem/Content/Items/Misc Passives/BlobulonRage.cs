using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.Misc;

namespace NevernamedsItems
{
    public class BlobulonRage : PassiveItem
    {
        public static int BlobulonRageID;
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<BlobulonRage>(
            "Blobulon Rage",
            "Long Live The Empire",
            "The berserker rage inherent to all Blobulons, now bestowed unto you.",
            "blobulonrage_icon");
            item.AddPassiveStatModifier(PlayerStats.StatType.Damage, 1.20f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            item.quality = PickupObject.ItemQuality.C;
            BlobulonRageID = item.PickupObjectId;
            item.RemovePickupFromLootTables();

            //item.AddAsChamberGunMastery("OnceMoreIntoTheBreach", 4);
        }
    }
}