using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using ItemAPI;

namespace NevernamedsItems
{
    class GlassChamber : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Glass Chamber";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/glasschamber_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<GlassChamber>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Seeload";
            string longDesc = "Reloading is 10% faster for every Glass Guon Stone the bearer has." + "\n\nA symbol of the tentative peace between the fragile Lady of Pane, and the mighty king-god Relodin.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.C;
        }
        private int currentItems, lastItems;
        protected override void Update()
        {
            if (Owner)
            {
                CalculateStats(Owner);
            }

            else { return; }
        }

        private void CalculateStats(PlayerController player)
        {
            currentItems = player.passiveItems.Count;
            if (currentItems != lastItems)
            {
                RemoveStat(PlayerStats.StatType.ReloadSpeed);
                foreach (PassiveItem item in player.passiveItems)
                {
                    if (item.PickupObjectId == 565)
                    {
                        if (Owner.HasPickupID(Gungeon.Game.Items["nn:glass_rounds"].PickupObjectId))
                        {
                            AddStat(PlayerStats.StatType.ReloadSpeed, 0.85f, StatModifier.ModifyMethod.MULTIPLICATIVE);
                        }
                        else
                        {
                            AddStat(PlayerStats.StatType.ReloadSpeed, 0.9f, StatModifier.ModifyMethod.MULTIPLICATIVE);
                        }
                    }
                }

                lastItems = currentItems;
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
            bool flag = !this.m_pickedUpThisRun;
            if (flag)
            {
                PickupObject byId = PickupObjectDatabase.GetById(565);
                player.AcquirePassiveItemPrefabDirectly(byId as PassiveItem);
            }
            base.Pickup(player);
        }
    }
}
