using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dungeonator;
using UnityEngine;
using ItemAPI;
using Alexandria.Misc;

namespace NevernamedsItems
{
    class BloodglassGuonStone : PassiveItem
    {
        public static int BloodGlassGuonStoneID;
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<BloodglassGuonStone>(
            "Bloodglass Guon Stone",
            "We Are The Crystal Haems",
            "An ancient glass blessing, perverted by Blobulonian technology." + "\n\nCrystallises spilt blood into glass guon stones.",
            "bloodglassguonstone_icon");
            item.quality = PickupObject.ItemQuality.C;
            BloodGlassGuonStoneID = item.PickupObjectId;
            item.RemovePickupFromLootTables();
        }

        private void SpawnGuons(PlayerController player)
        {
            if (UnityEngine.Random.value < 0.4f)
            {
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(565).gameObject, player);
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(565).gameObject, player);
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(565).gameObject, player);
            }
        }
        public override void Pickup(PlayerController player)
        {
            bool hasntAlreadyBeenCollected = !this.m_pickedUpThisRun;
            if (hasntAlreadyBeenCollected)
            {
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(565).gameObject, player);
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(565).gameObject, player);
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(565).gameObject, player);
            }
            player.OnReceivedDamage += this.SpawnGuons;
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.OnReceivedDamage -= this.SpawnGuons;
            return debrisObject;
        }
        public override void OnDestroy()
        {
          if (Owner)  Owner.OnReceivedDamage -= this.SpawnGuons;
            base.OnDestroy();
        }
    }
}