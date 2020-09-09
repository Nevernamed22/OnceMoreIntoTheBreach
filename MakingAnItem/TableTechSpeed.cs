using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;

namespace NevernamedsItems
{
    public class TableTechSpeed : TableFlipItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Table Tech Speed";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/tabletechspeed_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<TableTechSpeed>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Flip Acceleration";
            string longDesc = "Flipping a table increases the bearer's movement speed temporarily."+"\n\nAppendix F of the \"Tabla Sutra\". Flipping is to create motion. In motion there is life, and joy. To flip, is to live.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item


            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.D;

            //SYNERGY WITH SPEED POTION --> Sound Barrier: Length of Table Tech Speed's bonus is doubled.            

        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnTableFlipped = (Action<FlippableCover>)Delegate.Combine(player.OnTableFlipped, new Action<FlippableCover>(this.SpeedEffect));
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject result = base.Drop(player);
            player.OnTableFlipped = (Action<FlippableCover>)Delegate.Remove(player.OnTableFlipped, new Action<FlippableCover>(this.SpeedEffect));
            return result;
        }
        protected override void OnDestroy()
        {
            DebrisObject result = base.Drop(Owner);
            Owner.OnTableFlipped = (Action<FlippableCover>)Delegate.Remove(Owner.OnTableFlipped, new Action<FlippableCover>(this.SpeedEffect));
            base.OnDestroy();
        }
        float movementBuff = -1;
        bool effectActive;
        private void SpeedEffect(FlippableCover obj)
        {
            PlayerController owner = base.Owner;
            if (effectActive == true) return;            
            else
            {
                AddStat(PlayerStats.StatType.MovementSpeed, 1.25f, StatModifier.ModifyMethod.MULTIPLICATIVE);
                base.Owner.stats.RecalculateStats(base.Owner, true, false);
                effectActive = true;
                if (Owner.HasPickupID(Gungeon.Game.Items["nn:speed_potion"].PickupObjectId)) Invoke("DisableSpeed", 14.0f);
                else Invoke("DisableSpeed", 7.0f);
            }
        }

        private void DisableSpeed()
        {
            RemoveStat(PlayerStats.StatType.MovementSpeed);
            base.Owner.stats.RecalculateStats(base.Owner, true, false);
            movementBuff = -1;
            effectActive = false;
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
