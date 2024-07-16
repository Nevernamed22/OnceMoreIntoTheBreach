using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Alexandria.ItemAPI;
using UnityEngine;

namespace NevernamedsItems
{
    public class SpaceMetal : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<SpaceMetal>(
            "Lump of Space Metal",
            "Mined Fresh To You",
            "This rich lump of unrefined space metal is prized throughout Hegemony of Man systems for all the useful minerals and materials that can be drawn from within it.",
            "spacemetal_improved");
            item.AddPassiveStatModifier( PlayerStats.StatType.AdditionalBlanksPerFloor, 1, StatModifier.ModifyMethod.ADDITIVE);
            item.CanBeDropped = true;
            item.quality = PickupObject.ItemQuality.A;           
        }
        public override void Pickup(PlayerController player)
        {
            bool hasntAlreadyBeenCollected = !this.m_pickedUpThisRun;
            if (hasntAlreadyBeenCollected)
            {
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(67).gameObject, player);
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(67).gameObject, player);
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(67).gameObject, player);
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(120).gameObject, player);
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(120).gameObject, player);
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(120).gameObject, player);
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(120).gameObject, player);
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(120).gameObject, player);
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(74).gameObject, player);
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(224).gameObject, player);
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(565).gameObject, player);
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(565).gameObject, player);
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(565).gameObject, player);
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(565).gameObject, player);
            }
            base.Pickup(player);
        }
    }
}
