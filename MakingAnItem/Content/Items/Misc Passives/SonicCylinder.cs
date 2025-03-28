using System;
using System.Collections.Generic;
using System.Linq;
using Gungeon;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.Misc;
using Dungeonator;

namespace NevernamedsItems
{
    public class SonicCylinder : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<SonicCylinder>(
            "Sonic Cylinder",
            "Blue Hole",
            "Legends tell of a gun that created a powerful bullet banishing sonic wave with every function of its double action trigger. \n\nThis is one of the few remaining parts of that powerful pre-gungeon artefact.",
            "soniccylinder_icon");
            item.quality = PickupObject.ItemQuality.A;
            item.AddToSubShop(ItemBuilder.ShopType.Cursula);
            item.AddToSubShop(ItemBuilder.ShopType.OldRed);

            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 2f, StatModifier.ModifyMethod.ADDITIVE);

            ID = item.PickupObjectId;
        }
        public static int ID;
        private void HandleGunReloaded(PlayerController player, Gun playerGun)
        {
            if (playerGun.ClipShotsRemaining == 0 && player)
            {
                if (UnityEngine.Random.value <= ((float)playerGun.ClipCapacity / 50f)) player.DoEasyBlank(playerGun.barrelOffset.position, EasyBlankType.MINI);
            }
        }
        
        public override void Pickup(PlayerController player)
        {
            player.OnReloadedGun += HandleGunReloaded;
            base.Pickup(player);
        }
        public override void DisableEffect(PlayerController player)
        {
            if (player) player.OnReloadedGun -= HandleGunReloaded;

            base.DisableEffect(player);
        }
    }
}