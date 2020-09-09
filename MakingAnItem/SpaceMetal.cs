using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ItemAPI;
using UnityEngine;

namespace NevernamedsItems
{
    public class SpaceMetal : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Lump of Space Metal";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/spacemetal_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<SpaceMetal>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Mined Fresh To You";
            string longDesc = "This rich lump of unrefined space metal is prized throughout Hegemony of Man systems for all the useful minerals and materials that can be drawn from within it.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.AdditionalBlanksPerFloor, 1, StatModifier.ModifyMethod.ADDITIVE);
            item.CanBeDropped = true;

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.A;

            List<string> mandatorySynergyItems = new List<string>() { "nn:lucky_coin" };
            List<string> optionalSynergyItems = new List<string>() { "nn:lump_of_space_metal", "nn:loose_change", "coin_crown", "iron_coin", "gold_junk", "table_tech_money" };
            CustomSynergies.Add("Prosperity", mandatorySynergyItems, optionalSynergyItems);

            List<string> mandatorySynergyItems2 = new List<string>() { "nn:miners_bullets" };
            List<string> optionalSynergyItems2 = new List<string>() { "nn:lump_of_space_metal", "mine_cutter" };
            CustomSynergies.Add("Miiiining Away~", mandatorySynergyItems2, optionalSynergyItems2);
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
