using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;
using SaveAPI;

namespace NevernamedsItems
{
    class FalseBlank : ReusableBlankitem
    {
        public static void Init()
        {
            PlayerItem item = ItemSetup.NewItem<FalseBlank>(
            "False Blank",
            "Blanksphemer",
            "This artefact was created by the devilish Shell'tan to trick unwary Gungeoneers. With every use, more of the bearer's soul is stripped away, and exchanged with pure darkness.",
            "falseblank_icon") as PlayerItem;
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 50);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE);
            item.consumable = false;
            item.quality = ItemQuality.D;           
            item.AddToSubShop(ItemBuilder.ShopType.OldRed);
            item.AddToSubShop(ItemBuilder.ShopType.Cursula);
        }
        public override void Pickup(PlayerController player)
        {
            if (!this.m_pickedUpThisRun)
            {
                timesUsed = 0;
            }
            base.Pickup(player);    
        }
        int timesUsed = 0;
        public override void DoEffect(PlayerController user)
        {
            if (!(user.PlayerHasActiveSynergy("False Pretences") && UnityEngine.Random.value < .50f))
            {

                    giveSomeCurse(user);
                
            }
    
            if (user.PlayerHasActiveSynergy("Transparent Lies"))
            {
                PickupObject byId = PickupObjectDatabase.GetById(565);
                user.AcquirePassiveItemPrefabDirectly(byId as PassiveItem);
            }
            timesUsed += 1;
            user.ForceBlank(25f, 0.5f, false, true, null, true, -1f);
            if (timesUsed >= 10)
            {
                if (!SaveAPIManager.GetFlag(CustomDungeonFlags.USED_FALSE_BLANK_TEN_TIMES))
                {
                    SaveAPIManager.SetFlag(CustomDungeonFlags.USED_FALSE_BLANK_TEN_TIMES, true);
                }
            }
        }

        private void giveSomeCurse(PlayerController user)
        {
            float currentCurse = user.stats.GetBaseStatValue(PlayerStats.StatType.Curse);
            user.stats.SetBaseStatValue(PlayerStats.StatType.Curse, currentCurse + 0.5f, user);
        }
        public override bool CanBeUsed(PlayerController user)
        {
            return base.CanBeUsed(user);
        }
    }
}
