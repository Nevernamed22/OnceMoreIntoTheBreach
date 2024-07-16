using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dungeonator;
using Alexandria.ItemAPI;
using UnityEngine;

namespace NevernamedsItems
{
    class ElevatorButton : PlayerItem
    {
        public static void Init()
        {
            PlayerItem item = ItemSetup.NewItem<ElevatorButton>(
              "Elevator Button",
              "Going... Down",
              "Transports the user to the next floor. One use." + "\n\nMay malfunction.",
              "elevatorbutton_improved") as PlayerItem;          
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.None, 1000);
            item.consumable = true;
            item.quality = ItemQuality.D;
            item.AddToSubShop(ItemBuilder.ShopType.Trorc);
        }
        public bool goUp;
        public override void DoEffect(PlayerController user)
        {
            if (user.HasPickupID(Gungeon.Game.Items["space_friend"].PickupObjectId))
            {
                goUp = true;
            }
            else
            {
                if (UnityEngine.Random.value > .05) goUp = false;
                else goUp = true;
            }
            if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.CASTLEGEON)
            {
                GameManager.Instance.LoadCustomLevel("tt5");
            }
            else if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.SEWERGEON)
            {
                if (goUp == false)
                {
                    GameManager.Instance.LoadCustomLevel("tt5");
                }
                else
                {
                    GameManager.Instance.LoadCustomLevel("tt_castle");
                }
            }
            else if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.JUNGLEGEON)
            {
                if (goUp == false)
                {
                    GameManager.Instance.LoadCustomLevel("tt5");
                }
                else
                {
                    GameManager.Instance.LoadCustomLevel("tt_castle");
                }
            }
            else if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.BELLYGEON)
            {
                if (goUp == false)
                {
                    GameManager.Instance.LoadCustomLevel("tt_mines");
                }
                else
                {
                    GameManager.Instance.LoadCustomLevel("tt5");
                }
            }
            else if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.GUNGEON)
            {
                if (goUp == false)
                {
                    GameManager.Instance.LoadCustomLevel("tt_mines");
                }
                else
                {
                    GameManager.Instance.LoadCustomLevel("tt_castle");
                }
            }
            else if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.CATHEDRALGEON)
            {
                if (goUp == false)
                {
                    GameManager.Instance.LoadCustomLevel("tt_mines");
                }
                else
                {
                    GameManager.Instance.LoadCustomLevel("tt5");
                }
            }
            else if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.MINEGEON)
            {
                if (goUp == false)
                {
                    GameManager.Instance.LoadCustomLevel("tt_catacombs");
                }
                else
                {
                    GameManager.Instance.LoadCustomLevel("tt5");
                }
            }
            else if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.RATGEON)
            {
                if (goUp == false)
                {
                    GameManager.Instance.LoadCustomLevel("tt_catacombs");
                }
                else
                {
                    GameManager.Instance.LoadCustomLevel("tt_mines");
                }
            }
            else if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.CATACOMBGEON)
            {
                if (goUp == false)
                {
                    GameManager.Instance.LoadCustomLevel("tt_forge");
                }
                else
                {
                    GameManager.Instance.LoadCustomLevel("tt_mines");
                }
            }
            else if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.OFFICEGEON)
            {
                if (goUp == false)
                {
                    GameManager.Instance.LoadCustomLevel("tt_forge");
                }
                else
                {
                    GameManager.Instance.LoadCustomLevel("tt_catacombs");
                }
            }
            else if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.FORGEGEON)
            {
                if (goUp == false)
                {
                    GameManager.Instance.LoadCustomLevel("tt_bullethell");
                }
                else
                {
                    GameManager.Instance.LoadCustomLevel("tt_catacombs");
                }
            }
            else if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.WESTGEON)
            {
                if (goUp == false)
                {
                    GameManager.Instance.LoadCustomLevel("tt_bullethell");
                }
                else
                {
                    GameManager.Instance.LoadCustomLevel("tt_catacombs");
                }
            }//Apache's glitch floor
            else if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.HELLGEON)
            {
                
                if (goUp == false)
                {
                    Exploder.DoDefaultExplosion(LastOwner.specRigidbody.UnitCenter, new Vector2());
                }
                else
                {
                    GameManager.Instance.LoadCustomLevel("tt_forge");
                }
            }
            else
            {
                IntVector2 bestRewardLocation = user.CurrentRoom.GetBestRewardLocation(IntVector2.One * 3, RoomHandler.RewardLocationStyle.PlayerCenter, true);
                Chest rainbow_Chest = GameManager.Instance.RewardManager.Rainbow_Chest;
                rainbow_Chest.IsLocked = false;
                Chest.Spawn(rainbow_Chest, bestRewardLocation);
            }

        }
    }
}