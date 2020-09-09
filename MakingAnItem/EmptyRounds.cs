using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using ItemAPI;

namespace NevernamedsItems
{
    public class EmptyRounds : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Empty Rounds";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/springloadedchamber_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<EmptyRounds>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Marvellous Mechanism";
            string longDesc = "Increases damage by 30% for the first half of the clip, but drecreases it by 20% for the second." + "\n\nA miraculous clockwork doodad cannibalised from the Wind Up Gun. Proof that springs are, and will always be, the best form of potential energy.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item
            //ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.ProjectileSpeed, -1f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.EXCLUDED; //B
        }
        //private int currentClip, lastClip;
        //private Gun currentGun, lastGun;

        protected override void Update()
        {
            if (Owner)
            {
                foreach(Gun gun in Owner.inventory.AllGuns)
                {
                    
                }                

            }
        }        
        
        private void AddStat(PlayerStats.StatType statType, float amount, StatModifier.ModifyMethod method = StatModifier.ModifyMethod.ADDITIVE)
        {
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
