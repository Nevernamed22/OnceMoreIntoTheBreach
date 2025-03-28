using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    public class MicroAIContact : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<MicroAIContact>(
            "Aimbot",
            "Virtual Assistant",
            "This highly advanced contact lens contains a Hegemony Issue Virtual Aim Assistant Micro-AI, or Aimbot for short. It projects mathematic predictions, bullet aerodynamics simulations, and a targeting reticule onto your vision, making your accuracy second to none.",
            "aimbot_improved");
            item.AddPassiveStatModifier( PlayerStats.StatType.Accuracy, 0.01f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            item.quality = PickupObject.ItemQuality.A;
        }
    }
}
