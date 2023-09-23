using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dungeonator;
using ItemAPI;
using UnityEngine;

namespace NevernamedsItems
{
    class ClipOnAmmoPouch : PlayerItem
    {
        public static void Init()
        {
            PlayerItem item = ItemSetup.NewItem<ClipOnAmmoPouch>(
            "Clip-On Ammo Pouch",
            "Ammo Up",
            "Increases the ammo capacity of the held gun by 50%." + "\n\nClips on easy, and doesn't leave a mark!",
            "cliponammopouch_icon") as PlayerItem;
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.None, 1000);
            item.consumable = true;
            item.quality = ItemQuality.D;
            item.AddToSubShop(ItemBuilder.ShopType.Trorc);
        }
        public override void DoEffect(PlayerController user)
        {
            Gun instanceGun = user.CurrentGun;
            instanceGun.SetBaseMaxAmmo(Mathf.CeilToInt(instanceGun.GetBaseMaxAmmo() * 1.5f));
            user.stats.RecalculateStats(user);
        }
        public override bool CanBeUsed(PlayerController user)
        {
            if (user && user.CurrentGun != null && !user.CurrentGun.InfiniteAmmo)
            {
                return true;
            }
            else return false;
        }
    }
}
