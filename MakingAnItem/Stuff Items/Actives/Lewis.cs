using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ItemAPI;
using UnityEngine;

namespace NevernamedsItems
{
    class Lewis : PlayerItem
    {
        //Call this method from the Start() method of your ETGModule extension class
        public static void Init()
        {
            //The name of the item
            string itemName = "Lewis";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it.
            string resourceName = "NevernamedsItems/Resources/lewis_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<Lewis>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "With Friends Like These...";
            string longDesc = "This absolute freeloader just sits in your active item storage doing nothing.\n\n" + "At least he kinda pays rent through providing you some stats";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"kts" here is the item pool. In the console you'd type kts:sweating_bullets
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 1000);

            //Adds a passive modifier, like curse, coolness, damage, etc. to the item. Works for passives and actives.

            //Set some other fields
            item.consumable = false;
            item.quality = ItemQuality.B;

            List<string> mandatorySynergyItems = new List<string>() { "nn:lewis", "battle_standard" };
            CustomSynergies.Add("Rally The Slacker", mandatorySynergyItems);

            LewisID = item.PickupObjectId;
        }
        public static int LewisID;
        //Add the item's functionality down here! I stole most of this from the Stuffed Star active item code!

        protected override void DoEffect(PlayerController user)
        {
            //Play a sound effect
            AkSoundEngine.PostEvent("Play_ENM_blobulord_reform_01", base.gameObject);

            //Activates the effect
            StartEffect(user);

            //start a coroutine which calls the EndEffect method when the item's effect duration runs out
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            needRestat = true;
            player.stats.RecalculateStats(player, true, false);
        }

        public DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            hasSynergy = false;
            needRestat = false;
            player.stats.RecalculateStats(player, true, false);
            return debrisObject;
        }
        protected override void OnDestroy()
        {
            hasSynergy = false;
            needRestat = false;
            LastOwner.stats.RecalculateStats(LastOwner, true, false);
            base.OnDestroy();
        }

        //Doubles the damage, makes the next shot kill the player, and stores the amount we buffed the player for later
        private void StartEffect(PlayerController user)
        {
        }

        //Resets the player back to their original stats
        private void EndEffect(PlayerController user)
        {
        }

        //Disable or enable the active whenever you need!
        public override bool CanBeUsed(PlayerController user)
        {
            return base.CanBeUsed(user);
        }

        //PSEUDOSYNERGY
        private void AddStat(PlayerStats.StatType statType, float amount, StatModifier.ModifyMethod method = StatModifier.ModifyMethod.ADDITIVE)
        {
            foreach (var m in passiveStatModifiers)
            {
                if (m.statToBoost == statType) return; //don't add duplicates
            }

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

        private bool hasSynergy, needRestat;

        public override void Update()
        {
            PlayerController player = this.LastOwner;
            if (player && player.HasActiveItem(this.PickupObjectId))
            {
                if (player.HasPickupID(529) && hasSynergy == false)
                {
                    hasSynergy = true;
                    needRestat = true;
                    RemoveStat(PlayerStats.StatType.Damage);
                    RemoveStat(PlayerStats.StatType.RateOfFire);
                    RemoveStat(PlayerStats.StatType.ReloadSpeed);
                    RemoveStat(PlayerStats.StatType.MovementSpeed);
                    RemoveStat(PlayerStats.StatType.Health);

                    //STATS WITH BANNER
                    AddStat(PlayerStats.StatType.Damage, 1.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
                    AddStat(PlayerStats.StatType.RateOfFire, 1.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
                    AddStat(PlayerStats.StatType.ReloadSpeed, 0.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
                    AddStat(PlayerStats.StatType.MovementSpeed, 1.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
                    AddStat(PlayerStats.StatType.Health, 2f, StatModifier.ModifyMethod.ADDITIVE);
                    player.stats.RecalculateStats(player, true, false);
                }

                else if (!player.HasPickupID(529) && needRestat == true)
                {
                    needRestat = false;
                    hasSynergy = false;
                    RemoveStat(PlayerStats.StatType.Damage);
                    RemoveStat(PlayerStats.StatType.RateOfFire);
                    RemoveStat(PlayerStats.StatType.ReloadSpeed);
                    RemoveStat(PlayerStats.StatType.MovementSpeed);
                    RemoveStat(PlayerStats.StatType.Health);

                    //STATS WITHOUT BANNER
                    AddStat(PlayerStats.StatType.Damage, 1.2f, StatModifier.ModifyMethod.MULTIPLICATIVE);
                    AddStat(PlayerStats.StatType.RateOfFire, 1.2f, StatModifier.ModifyMethod.MULTIPLICATIVE);
                    AddStat(PlayerStats.StatType.ReloadSpeed, 0.8f, StatModifier.ModifyMethod.MULTIPLICATIVE);
                    AddStat(PlayerStats.StatType.MovementSpeed, 1.2f, StatModifier.ModifyMethod.MULTIPLICATIVE);
                    AddStat(PlayerStats.StatType.Health, 1f, StatModifier.ModifyMethod.ADDITIVE);
                    player.stats.RecalculateStats(player, true, false);
                }
            }
            else { return; }
        }
    }
}

