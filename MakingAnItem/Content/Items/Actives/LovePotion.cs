using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;
using SaveAPI;

namespace NevernamedsItems
{
    class LovePotion : PlayerItem
    {
        public static void Init()
        {
            PlayerItem item = ItemSetup.NewItem<LovePotion>(
           "Love Potion",
           "The Sausage Principle",
           "This potent potion of love was made by the Three Witches as part of a dashing romantic plot that was doomed to fail" + "\n\nIf you like something, never learn how it was made",
           "lovepotion_icon") as PlayerItem;

            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 150);
            item.consumable = false;
            item.quality = ItemQuality.D;
            item.AddToSubShop(ItemBuilder.ShopType.Goopton);
            item.AddToSubShop(ItemBuilder.ShopType.Cursula);
            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.PURCHASED_LOVEPOTION, true);
            item.AddItemToGooptonMetaShop(10);
        }

        public override void DoEffect(PlayerController user)
        {
            float length = 13;
            float width = 2.5f;
            if (user.PlayerHasActiveSynergy("Ooh Eee Ooh Ah Ah!"))
            {
                length = 20;
                width = 4;
            }
                DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(EasyGoopDefinitions.CharmGoopDef);
            Vector2 vector = user.CenterPosition;
            Vector2 normalized = (user.unadjustedAimPoint.XY() - vector).normalized;
            goopManagerForGoopType.TimedAddGoopLine(user.CenterPosition, user.CenterPosition + normalized * length, width, 0.5f);
            if (user.PlayerHasActiveSynergy("Number 9")) goopManagerForGoopType.TimedAddGoopLine(user.CenterPosition, user.CenterPosition + (normalized * -1) * length, width, 0.5f);
            //goopManagerForGoopType.gameObject.AddComponent<PurifiedWaterGoop>();
        }

        public override bool CanBeUsed(PlayerController user)
        {
            return true;
        }
    }
}

