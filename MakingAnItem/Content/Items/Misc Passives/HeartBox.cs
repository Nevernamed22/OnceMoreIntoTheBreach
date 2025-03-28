using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.Misc;
using System.Collections;

namespace NevernamedsItems
{
    public class HeartBox : PassiveItem
    {
        public static int ID;
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<HeartBox>(
            "Heart Box",
            "High Capacity",
            "Left over from a bulk shipment of heart pickups ordered by the Gungeon Acquisitions Department. \n\nCan store all sorts of things!",
            "heartbox_icon");
            item.AddPassiveStatModifier(PlayerStats.StatType.Health, 1, StatModifier.ModifyMethod.ADDITIVE);
            item.AddPassiveStatModifier(PlayerStats.StatType.AdditionalItemCapacity, 1, StatModifier.ModifyMethod.ADDITIVE);
            item.AddPassiveStatModifier(PlayerStats.StatType.AmmoCapacityMultiplier, 1.25f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            item.quality = PickupObject.ItemQuality.B;
            item.ItemSpansBaseQualityTiers = true;
            item.ItemRespectsHeartMagnificence = true;
            ID = item.PickupObjectId;
        }
        public override void Pickup(PlayerController player)
        {
            player.healthHaver.OnDamaged += OnDamaged;
            base.Pickup(player);
        }
        public override void DisableEffect(PlayerController player)
        {
            if (player)
            {
                if (player.IsStealthed)
                {
                    BreakStealth(player);
                }
                player.healthHaver.OnDamaged -= OnDamaged;
            }
            base.DisableEffect(player);
        }
        private void OnDamaged(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
        {
            if (Owner && Owner.PlayerHasActiveSynergy("Sleepin In a Cardboard Box"))
            {
                StealthEffect(Owner);
            }
        }
        private void StealthEffect(PlayerController player)
        {
            this.BreakStealth(player);
            player.OnItemStolen += this.BreakStealthOnSteal;
            player.ChangeSpecialShaderFlag(1, 1f);
            player.SetIsStealthed(true, "heartbox");
            player.SetCapableOfStealing(true, "heartbox", null);
            GameManager.Instance.StartCoroutine(this.Unstealthy(player));
        }
        private void BreakStealth(PlayerController player)
        {
            player.ChangeSpecialShaderFlag(1, 0f);
            player.OnItemStolen -= this.BreakStealthOnSteal;
            player.SetIsStealthed(false, "heartbox");
            player.SetCapableOfStealing(false, "heartbox", null);
            player.OnDidUnstealthyAction -= this.BreakStealth;
            AkSoundEngine.PostEvent("Play_ENM_wizardred_appear_01", base.gameObject);
        }
        private IEnumerator Unstealthy(PlayerController player)
        {
            yield return new WaitForSeconds(0.15f);
            player.OnDidUnstealthyAction += this.BreakStealth;
            yield break;
        }
        private void BreakStealthOnSteal(PlayerController arg1, ShopItemController arg2) { this.BreakStealth(arg1); }
        
    }
}
