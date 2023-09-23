using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;
using MonoMod.RuntimeDetour;
using System.Reflection;
using SaveAPI;

namespace NevernamedsItems
{
    public class CursedTumbler : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<CursedTumbler>(
            "Cursed Tumbler",
            "What's In The Box?",
            "Gives chests a chance to be Jammed." + "\n\nAn 4th dimensional set of mechanisms designed to lock away all evil in this world.",
            "cursedtumbler_icon");
            item.AddPassiveStatModifier( PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE);
            item.CanBeDropped = true;
            item.quality = PickupObject.ItemQuality.B;
           
            CursedTumblerID = item.PickupObjectId;
            item.SetupUnlockOnCustomStat(CustomTrackedStats.JAMMED_CHESTS_OPENED, 0, DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN);

        }
        public static int CursedTumblerID;        
    }       
}