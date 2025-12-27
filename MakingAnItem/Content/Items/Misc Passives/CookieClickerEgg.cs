using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.Misc;
using HarmonyLib;

namespace NevernamedsItems
{
    class CookieClickerEgg : PassiveItem
    {
        public static void Init()
        {
            PassiveItem item = ItemSetup.NewItem<CookieClickerEgg>(
               "\"egg\"",
               "hey, it's \"egg\"",
               "+9 CliP Shots.",
               "cookieclickeregg_icon") as PassiveItem;

            item.quality = PickupObject.ItemQuality.B;
            ID = item.PickupObjectId;
        }
        public static int ID;
        public override void Pickup(PlayerController player)
        {
            CookieClickerEgg.IncrementFlag(player, typeof(CookieClickerEgg));
            base.Pickup(player);
        }

        public override void DisableEffect(PlayerController player)
        {
            if (player != null)
            {
                CookieClickerEgg.DecrementFlag(player, typeof(CookieClickerEgg));
            }
            base.DisableEffect(player);
        }
    }

    [HarmonyPatch(typeof(ProjectileModule), nameof(ProjectileModule.GetModNumberOfShotsInClip))]
    public static class CookieClickerEggPatch
    {
        public static void Postfix(GameActor owner, ref int __result)
        {
            int numToAdd = 0;
            if (owner && owner is PlayerController)
            {
                PlayerController pl = owner as PlayerController;
                if (PassiveItem.ActiveFlagItems.ContainsKey(pl) && PassiveItem.ActiveFlagItems[pl].ContainsKey(typeof(CookieClickerEgg)))
                {
                    numToAdd = PassiveItem.ActiveFlagItems[pl][typeof(CookieClickerEgg)] * 9;
                }
            }
            if (__result > 0)
            {
                __result += numToAdd;
            }
        }
    }
}
