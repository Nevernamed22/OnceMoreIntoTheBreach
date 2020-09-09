using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using ItemAPI;

namespace NevernamedsItems
{
    class FullArmourJacket : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Full Armour Jacket";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/fullarmourjacket_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<FullArmourJacket>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Live n' Lerm";
            string longDesc = "Every piece of Armour increases damage by 6%." + "\n\nThe best defence is a good offence.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item            
            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.B;
        }
        private float currentArmour, lastArmour;
        protected override void Update()
        {
            if (Owner)
            {
                CalculateHealth(Owner);
            }

            else { return; }
        }

        private void CalculateHealth(PlayerController player)
        {
            currentArmour = player.healthHaver.Armor;
            if (currentArmour != lastArmour)
            {
                RemoveStat(PlayerStats.StatType.Damage);
                float damageAmountMult;
                if (player.HasPickupID(Gungeon.Game.Items["nn:junkllets"].PickupObjectId))
                {
                    damageAmountMult = currentArmour * 0.07f;
                    damageAmountMult += 1f;
                }
                else
                {
                    damageAmountMult = currentArmour * 0.06f;
                    damageAmountMult += 1f;
                }
                AddStat(PlayerStats.StatType.Damage, damageAmountMult, StatModifier.ModifyMethod.MULTIPLICATIVE);
                lastArmour = currentArmour;
                player.stats.RecalculateStats(player, true, false);
            }

        }
        private void AddStat(PlayerStats.StatType statType, float amount, StatModifier.ModifyMethod method = StatModifier.ModifyMethod.ADDITIVE)
        {
            /*foreach (var m in passiveStatModifiers)
            {
                if (m.statToBoost == statType) return; //don't add duplicates
            }*/

            StatModifier modifier = new StatModifier
            {
                amount = amount,
                statToBoost = statType,
                modifyType = method
            };

            if (this.passiveStatModifiers == null)
                this.passiveStatModifiers = new StatModifier[] { modifier };
            else
                this.passiveStatModifiers = this.passiveStatModifiers.Concat(new StatModifier[] { modifier }).ToArray();
        }

        private void RemoveStat(PlayerStats.StatType statType)
        {
            var newModifiers = new List<StatModifier>();
            for (int i = 0; i < passiveStatModifiers.Length; i++)
            {
                if (passiveStatModifiers[i].statToBoost != statType)
                    newModifiers.Add(passiveStatModifiers[i]);
            }
            this.passiveStatModifiers = newModifiers.ToArray();
        }
        public override void Pickup(PlayerController player)
        {
            //player.SetIsFlying(true, "shade", true, false);
            bool hasntAlreadyBeenCollected = !this.m_pickedUpThisRun;
            if (hasntAlreadyBeenCollected)
            {
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(120).gameObject, player);
            }
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            //player.SetIsFlying(false, "shade", true, false);
            DebrisObject debrisObject = base.Drop(player);
            return debrisObject;
        }
        protected override void OnDestroy()
        {
            //Owner.SetIsFlying(false, "shade", true, false);
            base.OnDestroy();
        }
    }
}