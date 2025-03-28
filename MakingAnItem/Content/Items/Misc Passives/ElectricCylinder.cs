using Alexandria.ItemAPI;
using Dungeonator;
using System;
using System.Collections.Generic;
using System.Linq;
using Alexandria.Misc;
using System.Text;
using UnityEngine;
using static NevernamedsItems.GravityGun;
using SaveAPI;

namespace NevernamedsItems
{
    public class ElectricCylinder : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<ElectricCylinder>(
            "Electric Cylinder",
            "Vrrrt",
            "On the rare occasion that Tailor the Tinker has taken up arms, he always found the rotational velocity of gun cylinders unsatisfactory."+"\n\nAn electric motor fixes this.",
            "electriccylinder_icon");

            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.RateOfFire, 2f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.ReloadSpeed, 0.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);

            item.quality = PickupObject.ItemQuality.S;
            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.RAT_KILLED_ROBOT, true);
            ID = item.PickupObjectId;
        }
        public static int ID;
        public override void Pickup(PlayerController player)
        {
            player.OnReloadedGun += OnReload;
            base.Pickup(player);
        }
        public override void DisableEffect(PlayerController player)
        {
            if (player) { player.OnReloadedGun -= OnReload; }
            base.DisableEffect(player);
        }
        private void OnReload(PlayerController player, Gun gun)
        {
            AkSoundEngine.PostEvent("electricdrillbuzz", base.gameObject); 
        }
    } 
}
