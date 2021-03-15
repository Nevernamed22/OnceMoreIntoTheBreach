using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NevernamedsItems
{
    class LootHelpers
    {
        public static PickupObject.ItemQuality RandomTier()
        {
            List<PickupObject.ItemQuality> tiers = new List<PickupObject.ItemQuality>()
            {
                PickupObject.ItemQuality.S,
                PickupObject.ItemQuality.A,
                PickupObject.ItemQuality.B,
                PickupObject.ItemQuality.C,
                PickupObject.ItemQuality.D
            };
            return BraveUtility.RandomElement(tiers);
        }
    }
}
