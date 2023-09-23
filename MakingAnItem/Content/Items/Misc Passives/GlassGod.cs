using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    public class GlassGod : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<GlassGod>(
            "Glass God",
            "Fragile Divinity",
            "Grants great power, but shatters upon it's bearer taking damage." + "\n\nThe emblem of the Lady of Pane's greatest champion, a shining titan known only as 'The Glass One'. After his fall in battle, outnumbered ten to one, his crest somehow made it's way to the Gungeon.",
            "glassgod_icon");
            item.AddPassiveStatModifier( PlayerStats.StatType.Coolness, 5, StatModifier.ModifyMethod.ADDITIVE);
            item.AddPassiveStatModifier( PlayerStats.StatType.Damage, 1.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            item.AddPassiveStatModifier( PlayerStats.StatType.ReloadSpeed, 0.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            item.AddPassiveStatModifier(PlayerStats.StatType.MovementSpeed, 1.2f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            item.AddPassiveStatModifier( PlayerStats.StatType.AdditionalItemCapacity, 5f, StatModifier.ModifyMethod.ADDITIVE);
            item.AddPassiveStatModifier( PlayerStats.StatType.AmmoCapacityMultiplier, 1.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            item.AddPassiveStatModifier( PlayerStats.StatType.RateOfFire, 1.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            item.AddPassiveStatModifier( PlayerStats.StatType.ProjectileSpeed, 1.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            item.AddPassiveStatModifier( PlayerStats.StatType.Accuracy, 0.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            item.quality = PickupObject.ItemQuality.A;
        }
        private void breakItem(PlayerController player)
        {
            PickupObject byId = PickupObjectDatabase.GetById(565);
            player.AcquirePassiveItemPrefabDirectly(byId as PassiveItem);
            player.AcquirePassiveItemPrefabDirectly(byId as PassiveItem);
            player.AcquirePassiveItemPrefabDirectly(byId as PassiveItem);
            player.AcquirePassiveItemPrefabDirectly(byId as PassiveItem);
            player.AcquirePassiveItemPrefabDirectly(byId as PassiveItem);
            Owner.RemovePassiveItem(this.PickupObjectId);
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnReceivedDamage += this.breakItem;
        }
        public override void DisableEffect(PlayerController player)
        {
            if (player) player.OnReceivedDamage -= this.breakItem;
            base.DisableEffect(player);
        }


    }
}
