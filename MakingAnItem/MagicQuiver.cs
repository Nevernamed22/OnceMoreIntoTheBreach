using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using ItemAPI;
using System.Collections;

namespace NevernamedsItems
{
    public class MagicQuiver : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Magic Quiver";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/magicquiver_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<MagicQuiver>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Stranger Ranger";
            string longDesc = "Increases the damage of arrow-based weapons." + "\n\nAlben Smallbore was commissioned to create this artefact by a disappointed Bowman, disatisfied with the Gungeon's nigh dismissal of his craft.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.C;
        }
        private Gun currentHeldGun, lastHeldGun;
        protected override void Update()
        {
            currentHeldGun = Owner.CurrentGun;
            if (currentHeldGun != lastHeldGun)
            {
                if (bowWeapons.Contains(Owner.CurrentGun.PickupObjectId))
                {
                    GiveSynergyBoost();
                }
                else
                {
                    RemoveSynergyBoost();
                }
                lastHeldGun = currentHeldGun;
            }
            base.Update();
        }
        public static List<int> bowWeapons = new List<int>
        {
            4, //Sticky Crossbow
            8, //Bow
            12, //Crossbow
            52, //Crescent Crossbow
            126, //Shotbow
            200, //Charmed Bow
            210, //Gunbow
            227, //Wristbow
            381, //Triple Crossbow
            482, //Gunzheng
        };
        public override void Pickup(PlayerController player)
        {
            if (PickupObjectDatabase.GetByEncounterName("Exalted Armbow") != null) bowWeapons.Add(PickupObjectDatabase.GetByEncounterName("Exalted Armbow").PickupObjectId);
            if (PickupObjectDatabase.GetByEncounterName("Stake Launcher") != null) bowWeapons.Add(PickupObjectDatabase.GetByEncounterName("Stake Launcher").PickupObjectId);
            base.Pickup(player);
        }
        private void GiveSynergyBoost()
        {
            RemoveStat(PlayerStats.StatType.Damage);
            AddStat(PlayerStats.StatType.Damage, 1.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            Owner.stats.RecalculateStats(Owner, true, false);

        }
        private void RemoveSynergyBoost()
        {
            RemoveStat(PlayerStats.StatType.Damage);
            Owner.stats.RecalculateStats(Owner, true, false);

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
    }
}
