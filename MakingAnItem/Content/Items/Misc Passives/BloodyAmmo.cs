using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Alexandria.ItemAPI;
using Alexandria.Misc;
using UnityEngine;
using MonoMod.RuntimeDetour;
using System.Reflection;

namespace NevernamedsItems
{
    public class BloodyAmmo : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<BloodyAmmo>(
            "Bloody Ammo",
            "Lead Vessel",
            "Heals the owner upon picking up ammo." + "\n\nAn ammo box with it's skin removed.",
            "bloodyammo_icon");
            item.quality = PickupObject.ItemQuality.B;
            BloodyAmmoID = item.PickupObjectId;
        }
        public static int BloodyAmmoID;
        public void OnAmmoCollected(PlayerController player, AmmoPickup self)
        {
            player.healthHaver.ApplyHealing(0.5f);
            if (player.ForceZeroHealthState)
            {
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(120).gameObject, player);
            }
        }
        public override void Pickup(PlayerController player)
        {
            if (player.GetExtComp()) player.GetExtComp().OnPickedUpAmmo += OnAmmoCollected;
            base.Pickup(player);
        }
        public override void DisableEffect(PlayerController player)
        {
            if (player.GetExtComp()) player.GetExtComp().OnPickedUpAmmo -= OnAmmoCollected;
            base.DisableEffect(player);
        }
    }
}
