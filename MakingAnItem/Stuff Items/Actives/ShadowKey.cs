using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using ItemAPI;
using Dungeonator;

namespace NevernamedsItems
{
    class ShadowKey : PlayerItem
    {
        public static void Init()
        {
            string itemName = "Shadow Key";
            string resourceName = "NevernamedsItems/Resources/NeoActiveSprites/shadowkey_icon";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<ShadowKey>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Dark Realmed";
            string longDesc = "Duplicates a chest. One use."+"\n\nRecovered from a mysterious chest in a far off dungeon, this key is capable of turning a chest's shadow into an exact duplicate of the original.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.None, 5);
            item.consumable = true;
            item.quality = ItemQuality.D;
            item.AddToSubShop(ItemBuilder.ShopType.Flynt);
        }
        public override void DoEffect(PlayerController user)
        {
            IPlayerInteractable nearestInteractable = user.CurrentRoom.GetNearestInteractable(user.CenterPosition, 1f, user);
            if (!(nearestInteractable is Chest)) return;
            Chest dupeChest = nearestInteractable as Chest;

            IntVector2 bestRewardLocation = user.CurrentRoom.GetBestRewardLocation(IntVector2.One * 3, RoomHandler.RewardLocationStyle.PlayerCenter, true);

            ChestToolbox.ChestTier tier = dupeChest.GetChestTier();

            if (tier == ChestToolbox.ChestTier.RAT)
            {
                tier = ChestToolbox.ChestTier.RED;
            }
            else if (tier == ChestToolbox.ChestTier.TRUTH)
            {
                tier = ChestToolbox.ChestTier.BLUE;
            }

            ChestToolbox.ThreeStateValue isMimic = ChestToolbox.ThreeStateValue.UNSPECIFIED;
            if (dupeChest.IsMimic) isMimic = ChestToolbox.ThreeStateValue.FORCEYES;
            else isMimic = ChestToolbox.ThreeStateValue.FORCENO;

            ChestToolbox.ThreeStateValue isFused = ChestToolbox.ThreeStateValue.UNSPECIFIED;
            if (dupeChest.GetFuse() != null) isFused = ChestToolbox.ThreeStateValue.FORCEYES;
            else isFused = ChestToolbox.ThreeStateValue.FORCENO;

            Chest spawnedChest = ChestToolbox.SpawnChestEasy(bestRewardLocation, tier, dupeChest.IsLocked, Chest.GeneralChestType.UNSPECIFIED, isMimic, isFused);

            if (dupeChest.GetComponent<JammedChestBehav>()) spawnedChest.gameObject.AddComponent<JammedChestBehav>();
            else if (dupeChest.GetComponent<PassedOverForJammedChest>()) spawnedChest.gameObject.AddComponent<PassedOverForJammedChest>();
        }

        public override bool CanBeUsed(PlayerController user)
        {
            IPlayerInteractable nearestInteractable = user.CurrentRoom.GetNearestInteractable(user.CenterPosition, 1f, user);
            if (nearestInteractable is Chest)
            {
                return true;
            }
            else return false;
        }
    }
}

