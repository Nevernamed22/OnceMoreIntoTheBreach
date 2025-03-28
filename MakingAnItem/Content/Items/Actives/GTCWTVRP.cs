using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dungeonator;
using Alexandria.ItemAPI;
using UnityEngine;
using System.Collections;

namespace NevernamedsItems
{
    class GTCWTVRP : PlayerItem
    {
        public static void Init()
        {
            PlayerItem item = ItemSetup.NewItem<GTCWTVRP>(
               "GTCWTVRP",
               "'Tis but a copy",
               "'Gun That Can Wound The Very Recent Past'" + "\n\nReloads the current floor. A cheap plastic knockoff of The Gun That Can Kill The Past.",
               "gtcwtvrp_improved") as PlayerItem;

            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.None, 1000);
            item.consumable = true;
            item.quality = ItemQuality.A;
            item.AddToSubShop(ItemBuilder.ShopType.Cursula);
            item.AddToSubShop(ItemBuilder.ShopType.Trorc);
        }
        public static Dictionary<GlobalDungeonData.ValidTilesets, string[]> floors = new Dictionary<GlobalDungeonData.ValidTilesets, string[]>()
        {
            {GlobalDungeonData.ValidTilesets.CASTLEGEON, new string[]{ "tt_castle", "Base_Castle", "Bossrush_01_Castle" } },
            {GlobalDungeonData.ValidTilesets.JUNGLEGEON, new string[]{ "tt_jungle", null, null } },
            {GlobalDungeonData.ValidTilesets.BELLYGEON, new string[]{ "tt_belly", null, null } },
            {GlobalDungeonData.ValidTilesets.SEWERGEON, new string[]{ "tt_sewer", "Base_Sewer", "Bossrush_01a_Sewer" } },
            {GlobalDungeonData.ValidTilesets.GUNGEON, new string[]{ "tt5", "Base_Gungeon", "Bossrush_02_Gungeon" } },
            {GlobalDungeonData.ValidTilesets.CATHEDRALGEON, new string[]{ "tt_cathedral", "Base_Cathedral", "Bossrush_02a_Cathedral" } },
            {GlobalDungeonData.ValidTilesets.MINEGEON, new string[]{ "tt_mines", "Base_Mines", "Bossrush_03_Mines" } },
            {GlobalDungeonData.ValidTilesets.RATGEON, new string[]{ "ss_resourcefulrat", null, null } },
            {GlobalDungeonData.ValidTilesets.CATACOMBGEON, new string[]{ "tt_catacombs", "Base_Catacombs", "Bossrush_04_Catacombs" } },
            {GlobalDungeonData.ValidTilesets.OFFICEGEON, new string[]{ "tt_nakatomi", null, null } },
            {GlobalDungeonData.ValidTilesets.FORGEGEON, new string[]{ "tt_forge", "Base_Forge", "Bossrush_05_Forge" } },
            {GlobalDungeonData.ValidTilesets.WESTGEON, new string[]{ "tt_canyon", null, null } },
            {GlobalDungeonData.ValidTilesets.HELLGEON, new string[]{ "tt_bullethell", "Base_BulletHell", "Bossrush_06_BulletHell" } }
        };
        public override void DoEffect(PlayerController user)
        {
            user.StartCoroutine(HandleReload(user));
        }
        public IEnumerator HandleReload(PlayerController user)
        {
            if (GameManager.Instance.Dungeon.IsGlitchDungeon)
            {
                GameManager.Instance.InjectedFlowPath = "Core Game Flows/Secret_DoubleBeholster_Flow";
            }

            if (floors.ContainsKey(GameManager.Instance.Dungeon.tileIndices.tilesetId))
            {
                if (GameManager.Instance.CurrentGameMode == GameManager.GameMode.BOSSRUSH || GameManager.Instance.CurrentGameMode == GameManager.GameMode.SUPERBOSSRUSH)
                {
                    string[] level = floors[GameManager.Instance.Dungeon.tileIndices.tilesetId];
                    if (level[0] != null && level[1] != null && level[2] != null)
                    {
                        Pixelator.Instance.FadeToBlack(0.5f, false, 0f);
                        GameUIRoot.Instance.HideCoreUI(string.Empty);
                        GameUIRoot.Instance.ToggleLowerPanels(false, false, string.Empty);
                        yield return new WaitForSeconds(0.5f);
                        GameManager.Instance.LoadCustomFlowForDebug(level[2], level[1], level[0]);
                    }
                    else { FallBack(user); }
                    
                }
                else
                {
                    Pixelator.Instance.FadeToBlack(0.5f, false, 0f);
                    GameUIRoot.Instance.HideCoreUI(string.Empty);
                    GameUIRoot.Instance.ToggleLowerPanels(false, false, string.Empty);
                    GameManager.Instance.DelayedLoadCustomLevel(0.5f, floors[GameManager.Instance.Dungeon.tileIndices.tilesetId][0]);
                }
            }
            else
            {
                FallBack(user);   
            }
            yield break;
        }
        private void FallBack(PlayerController user)
        {
            IntVector2 bestRewardLocation = user.CurrentRoom.GetBestRewardLocation(IntVector2.One * 3, RoomHandler.RewardLocationStyle.PlayerCenter, true);
            Chest rainbow_Chest = GameManager.Instance.RewardManager.Rainbow_Chest;
            Chest.Spawn(rainbow_Chest, bestRewardLocation);
        }
    }
}
