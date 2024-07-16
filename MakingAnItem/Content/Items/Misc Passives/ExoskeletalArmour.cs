using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;
using Alexandria.ItemAPI;
using SaveAPI;

namespace NevernamedsItems
{
    class ExoskeletalArmour : PassiveItem
    {
        public static void Init()
        {
            PassiveItem item = ItemSetup.NewItem<ExoskeletalArmour>(
            "Meat Shield",
            "Self Sacrifice",
            "Causes health to take damage before armour, and gives a little of both.",
            "meatshield_improved") as PassiveItem;
            item.quality = PickupObject.ItemQuality.D;
            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.PURCHASED_MEATSHIELD, true);
            item.AddItemToDougMetaShop(15);
            item.ArmorToGainOnInitialPickup = 2;
            MeatShieldID = item.PickupObjectId;
        }
        public static int MeatShieldID;
        public override void Update()
        {
            if (Owner)
            {
                if (!Owner.ForceZeroHealthState)
                {
                    if (!Owner.healthHaver.NextDamageIgnoresArmor && Owner.healthHaver.GetCurrentHealth() >= 0.5f)
                    {
                        Owner.healthHaver.NextDamageIgnoresArmor = true;
                    }
                }
            }
        }
        public override void Pickup(PlayerController player)
        {
            if (!this.m_pickedUpThisRun)
            {
                player.healthHaver.ApplyHealing(1);
            }
            base.Pickup(player);
        }
    }
}
