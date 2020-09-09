using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ItemAPI;
using UnityEngine;

namespace NevernamedsItems
{
    public class CaseyMimic : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "<WIP> Casey <WIP>";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/caseymimic_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<CaseyMimic>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Batt... wait a minute...";
            string longDesc = "Upon closer inspection, this seemingly normal Casey is actually a mimic!\nIt has stuck itself to your hand, and may take a while to pull off.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 2f, StatModifier.ModifyMethod.ADDITIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Damage, 0.90f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.ReloadSpeed, 1.45f, StatModifier.ModifyMethod.MULTIPLICATIVE);

            //Set the rarity of the item
            item.CanBeDropped = true;
            item.quality = PickupObject.ItemQuality.EXCLUDED;
        }

        int elevatorsCarriedDown = 0;
        private void OnNewFloor()
        {
            elevatorsCarriedDown += 1;
            ETGModConsole.Log("The Casey Mimic has been carried down " + elevatorsCarriedDown + "/2 Elevators.");
        }

        private void HandleRoomCleared(PlayerController p)
        {
            ETGModConsole.Log("A room was detected as being cleared");
        }

        /*private IEnumerator lootPayout()
        {
            //ETGModConsole.Log("Item 'Payed out' with this console log");
            
            //PickupObject byId = PickupObjectDatabase.GetById(541);
            //Owner.AcquirePassiveItemPrefabDirectly(byId as PassiveItem);
            //GameManager.Instance.RewardManager.SpawnRewardChestAt(Owner.CurrentRoom.GetRandomVisibleClearSpot(2, 2));
            //Give a 10% damage up
            float curDamage = Owner.stats.GetBaseStatValue(PlayerStats.StatType.Damage); //Get's the player's speed and stores it in a var called 'curMovement'
            float newDamage = curDamage * 1.1f; //Makes a variable named 'newMovement by multiplying 'curMovement' by 2
            Owner.stats.SetBaseStatValue(PlayerStats.StatType.Damage, newDamage, Owner);            
            yield return new WaitForSeconds(1f);
            PlayerController player = this.Owner;
            string header = "Give Casey & damage";
            string text = "Prepare to feel the pain";
            this.Notify(header, text);
            yield return new WaitForSeconds(15f);
            header = "Spawn Chest";
            text = "Prepare to feel the pain";
            this.Notify(header, text);
            yield break;*/

        public override void Pickup(PlayerController player)
        {
            GameManager.Instance.OnNewLevelFullyLoaded += this.OnNewFloor;
            this.m_owner.OnRoomClearEvent += this.HandleRoomCleared;
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject result = base.Drop(player);
            GameManager.Instance.OnNewLevelFullyLoaded -= this.OnNewFloor;
            this.m_owner.OnRoomClearEvent -= this.HandleRoomCleared;
            return result;
        }
        protected override void OnDestroy()
        {
            GameManager.Instance.OnNewLevelFullyLoaded -= this.OnNewFloor;
            this.m_owner.OnRoomClearEvent -= this.HandleRoomCleared;
            base.OnDestroy();
        }
    }
}
