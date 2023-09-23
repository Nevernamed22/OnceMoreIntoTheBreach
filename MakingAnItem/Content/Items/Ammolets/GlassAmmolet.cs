using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alexandria.ItemAPI;
using UnityEngine;

namespace NevernamedsItems
{
    public class GlassAmmolet : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<GlassAmmolet>(
            "Glass Ammolet",
            "Blank Recycling",
            "Recycles spent blanks into handy defensive guon stones.",
            "glassammolet_icon");
            item.quality = PickupObject.ItemQuality.B;
            item.AddToSubShop(ItemBuilder.ShopType.OldRed);
            item.SetTag("ammolet");
        }
        private void OnUsedBlank(PlayerController arg1, int arg2)
        {
            PickupObject byId = PickupObjectDatabase.GetById(565);
            Owner.AcquirePassiveItemPrefabDirectly(byId as PassiveItem);
        }
        public override void Pickup(PlayerController player)
        {
            bool flag = !this.m_pickedUpThisRun;
            if (flag)
            {
                PickupObject byId = PickupObjectDatabase.GetById(565);
                player.AcquirePassiveItemPrefabDirectly(byId as PassiveItem);
            }
            player.OnUsedBlank += this.OnUsedBlank;
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            player.OnUsedBlank -= this.OnUsedBlank;
            return base.Drop(player);
        }
        public override void OnDestroy()
        {
            if (Owner) Owner.OnUsedBlank -= this.OnUsedBlank;
            base.OnDestroy();
        }
    }
}
