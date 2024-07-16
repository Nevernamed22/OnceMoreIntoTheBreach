using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;
using Dungeonator;
using Alexandria.ChestAPI;

namespace NevernamedsItems
{
    class PocketChest : PlayerItem
    {
        public static void Init()
        {
            PlayerItem item = ItemSetup.NewItem<PocketChest>(
            "Pocket Chest",
            "Baby Box",
            "An infant chest, containing many mysteries." + "\n\nLevels up as you deal damage, and grows up when used.",
            "pocketchest_brown_icon") as PlayerItem;

            PocketChest.spriteIDs = new int[7];
            PocketChest.spriteIDs[0] = Initialisation.itemCollection.GetSpriteIdByName("pocketchest_brown_icon"); //Brown
            PocketChest.spriteIDs[1] = Initialisation.itemCollection.GetSpriteIdByName("pocketchest_blue_icon"); //Blue
            PocketChest.spriteIDs[2] = Initialisation.itemCollection.GetSpriteIdByName("pocketchest_green_icon"); //Green
            PocketChest.spriteIDs[3] = Initialisation.itemCollection.GetSpriteIdByName("pocketchest_red_icon"); //Red
            PocketChest.spriteIDs[4] = Initialisation.itemCollection.GetSpriteIdByName("pocketchest_synergy_icon"); //Synergy
            PocketChest.spriteIDs[5] = Initialisation.itemCollection.GetSpriteIdByName("pocketchest_black_icon"); //Black
            PocketChest.spriteIDs[6] = Initialisation.itemCollection.GetSpriteIdByName("pocketchest_rainbow_icon"); //Rainbow

            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.None, 500);
            item.consumable = true;
            item.quality = ItemQuality.D;
        }
        public PocketChestTier MemorisedTier = PocketChestTier.BROWN;
        public float storedDamage = 0f;
        public enum PocketChestTier
        {
            BROWN,
            BLUE,
            GREEN,
            RED,
            BLACK,
            SYNERGY,
            RAINBOW
        }

        public override void Update()
        {
            if (storedDamage >= 6500 && (MemorisedTier == PocketChestTier.RED)) //Red --> Black/Rainbow
            {
                if (UnityEngine.Random.value <= 0.01f)
                {
                    MemorisedTier = PocketChestTier.RAINBOW;
                    base.sprite.SetSprite(PocketChest.spriteIDs[6]);
                }
                else
                {
                    MemorisedTier = PocketChestTier.BLACK;
                    base.sprite.SetSprite(PocketChest.spriteIDs[5]);
                }
            }
            else if (storedDamage >= 3500 && (MemorisedTier == PocketChestTier.GREEN || MemorisedTier == PocketChestTier.SYNERGY)) //Green/Synergy --> Red
            {
                    MemorisedTier = PocketChestTier.RED;
                    base.sprite.SetSprite(PocketChest.spriteIDs[3]);
            }
            else if (storedDamage >= 1500 && MemorisedTier == PocketChestTier.BLUE) //Blue --> Green / Synergy
            {
                if (UnityEngine.Random.value <= 0.25f)
                {
                    MemorisedTier = PocketChestTier.SYNERGY;
                    base.sprite.SetSprite(PocketChest.spriteIDs[4]);

                }
                else
                {

                    MemorisedTier = PocketChestTier.GREEN;
                    base.sprite.SetSprite(PocketChest.spriteIDs[2]);
                }

            }
            else if (storedDamage >= 500 && MemorisedTier == PocketChestTier.BROWN) //Brown --> Blue
            {
                MemorisedTier = PocketChestTier.BLUE;
                base.sprite.SetSprite(PocketChest.spriteIDs[1]);

            }

            base.Update();
        }
        private void HurtEnemy(float damage, bool fatal, HealthHaver enemy)
        {
            storedDamage += damage;
        }
        public override void Pickup(PlayerController player)
        {
            if (!this.m_pickedUpThisRun)
            {
                MemorisedTier = PocketChestTier.BROWN;
                base.sprite.SetSprite(PocketChest.spriteIDs[0]);
                storedDamage = 0f;
            }
            base.Pickup(player);
            player.OnAnyEnemyReceivedDamage += this.HurtEnemy;
        }
        public override void OnPreDrop(PlayerController user)
        {
            user.OnAnyEnemyReceivedDamage -= this.HurtEnemy;
            base.OnPreDrop(user);
        }
        public override void DoEffect(PlayerController user)
        {
            ChestUtility.ChestTier chestToSpawn = ChestUtility.ChestTier.OTHER;
            switch (MemorisedTier)
            {
                case PocketChestTier.BROWN:
                    chestToSpawn = ChestUtility.ChestTier.BROWN;
                    break;
                case PocketChestTier.BLUE:
                    chestToSpawn = ChestUtility.ChestTier.BLUE;
                    break;
                case PocketChestTier.GREEN:
                    chestToSpawn = ChestUtility.ChestTier.GREEN;
                    break;
                case PocketChestTier.RED:
                    chestToSpawn = ChestUtility.ChestTier.RED;
                    break;
                case PocketChestTier.BLACK:
                    chestToSpawn = ChestUtility.ChestTier.BLACK;
                    break;
                case PocketChestTier.SYNERGY:
                    chestToSpawn = ChestUtility.ChestTier.SYNERGY;
                    break;
                case PocketChestTier.RAINBOW:
                    chestToSpawn = ChestUtility.ChestTier.RAINBOW;
                    break;
            }
            if (chestToSpawn != ChestUtility.ChestTier.OTHER)
            {
                IntVector2 bestRewardLocation2 = user.CurrentRoom.GetBestRewardLocation(IntVector2.One * 3, RoomHandler.RewardLocationStyle.PlayerCenter, true);
                ChestUtility.SpawnChestEasy(bestRewardLocation2, chestToSpawn, !(chestToSpawn == ChestUtility.ChestTier.RAINBOW || chestToSpawn == ChestUtility.ChestTier.BROWN), Chest.GeneralChestType.UNSPECIFIED);

            }
        }
        public override bool CanBeUsed(PlayerController user)
        {
            return base.CanBeUsed(user);
        }
        public static List<Chest.GeneralChestType> ChestyBois = new List<Chest.GeneralChestType>()
        {
            Chest.GeneralChestType.ITEM,
            Chest.GeneralChestType.WEAPON,
        };

        private static int[] spriteIDs;
    }
}