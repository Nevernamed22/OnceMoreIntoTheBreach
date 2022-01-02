using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dungeonator;
using ItemAPI;
using UnityEngine;

namespace NevernamedsItems
{
    class Wonderchest : PlayerItem
    {
        //Call this method from the Start() method of your ETGModule extension class
        public static void Init()
        {
            //The name of the item
            string itemName = "Wonderchest";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it.
            string resourceName = "NevernamedsItems/Resources/wonderchest_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<Wonderchest>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "What could be inside?";
            string longDesc = "Extremely rare chests such as this one were particularly favoured by Alben Smallbore for storing his valuables." + "\n\nThe complicated magically encripted lock on this thing causes it to access a different pocket subreality depending on where it is opened.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"kts" here is the item pool. In the console you'd type kts:sweating_bullets
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.None, 1000);

            //Adds a passive modifier, like curse, coolness, damage, etc. to the item. Works for passives and actives.

            //Set some other fields
            item.consumable = true;
            item.quality = ItemQuality.B;

            item.AddToSubShop(ItemBuilder.ShopType.Flynt);

        }
        protected override void DoEffect(PlayerController user)
        {
            bool hasDeterminedValidFloor = false;
            if (GameManager.Instance.Dungeon.IsGlitchDungeon) //GLITCHED FLOOR BONUS
            {
                IntVector2 bestRewardLocation2 = user.CurrentRoom.GetBestRewardLocation(IntVector2.One * 3, RoomHandler.RewardLocationStyle.PlayerCenter, true);
                Chest black_Chest = GameManager.Instance.RewardManager.S_Chest;
                black_Chest.IsLocked = true;
                black_Chest.ChestType = BraveUtility.RandomElement(ChestyBois);
                Chest spawnedBlack = Chest.Spawn(black_Chest, bestRewardLocation2);
                bool IsAGun = UnityEngine.Random.value <= 0.5f;
                spawnedBlack.lootTable.lootTable = (IsAGun ? GameManager.Instance.RewardManager.GunsLootTable : GameManager.Instance.RewardManager.ItemsLootTable);
                spawnedBlack.RegisterChestOnMinimap(spawnedBlack.GetAbsoluteParentRoom());
            }
            if (UnityEngine.Random.value <= 0.001f) //RANDOM RARE RAINBOW
            {
                IntVector2 bestRewardLocation = user.CurrentRoom.GetBestRewardLocation(IntVector2.One * 3, RoomHandler.RewardLocationStyle.PlayerCenter, true);
                Chest rainbow_Chest = GameManager.Instance.RewardManager.Rainbow_Chest;
                Chest spawnedRainbow = Chest.Spawn(rainbow_Chest, bestRewardLocation);
                rainbow_Chest.IsLocked = false;
                spawnedRainbow.RegisterChestOnMinimap(spawnedRainbow.GetAbsoluteParentRoom());
            }
            else
            {
                switch (GameManager.Instance.Dungeon.tileIndices.tilesetId)
                {
                    case GlobalDungeonData.ValidTilesets.CASTLEGEON: //KEEP
                        int pickupID;
                        if (UnityEngine.Random.value <= .50f) pickupID = 120;
                        else pickupID = 224;
                        LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(pickupID).gameObject, user);
                        LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(pickupID).gameObject, user);
                        hasDeterminedValidFloor = true;
                        break;
                    case GlobalDungeonData.ValidTilesets.SEWERGEON: //OUBLIETTE
                        for (int i = 0; i < 3; i++) LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(67).gameObject, user);
                        hasDeterminedValidFloor = true;
                        break;
                    case GlobalDungeonData.ValidTilesets.JUNGLEGEON: //JUNGLE
                        for (int i = 0; i < 3; i++)
                        {
                            IntVector2 bestRewardLocation = user.CurrentRoom.GetRandomVisibleClearSpot(2, 2);
                            Chest brown_Chest = GameManager.Instance.RewardManager.D_Chest;
                            brown_Chest.IsLocked = false;
                            brown_Chest.ChestType = Chest.GeneralChestType.UNSPECIFIED;
                            Chest spawnedChest = Chest.Spawn(brown_Chest, bestRewardLocation);
                            spawnedChest.lootTable.lootTable = (UnityEngine.Random.value <= 0.5f ? GameManager.Instance.RewardManager.GunsLootTable : GameManager.Instance.RewardManager.ItemsLootTable);
                            spawnedChest.RegisterChestOnMinimap(spawnedChest.GetAbsoluteParentRoom());
                        }
                        hasDeterminedValidFloor = true;
                        break;
                    case GlobalDungeonData.ValidTilesets.GUNGEON: //GUNGEON PROPER
                        for (int i = 0; i < 3; i++) LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(74).gameObject, user);
                        hasDeterminedValidFloor = true;
                        break;
                    case GlobalDungeonData.ValidTilesets.CATHEDRALGEON: //ABBEY
                        ChangeStatPermanent(user, PlayerStats.StatType.Health, 2, StatModifier.ModifyMethod.ADDITIVE);
                        ChangeStatPermanent(user, PlayerStats.StatType.Curse, 2, StatModifier.ModifyMethod.ADDITIVE);
                        if (user.characterIdentity == PlayableCharacters.Robot)
                        {
                            LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(120).gameObject, user);
                            LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(120).gameObject, user);
                        }
                        hasDeterminedValidFloor = true;
                        break;
                    case GlobalDungeonData.ValidTilesets.MINEGEON: //MINES
                        IntVector2 bestRewardLocation2 = user.CurrentRoom.GetBestRewardLocation(IntVector2.One * 3, RoomHandler.RewardLocationStyle.PlayerCenter, true);
                        Chest red_Chest = GameManager.Instance.RewardManager.A_Chest;
                        red_Chest.IsLocked = false;
                        red_Chest.ChestType = BraveUtility.RandomElement(ChestyBois);
                        Chest spawnedRed = Chest.Spawn(red_Chest, bestRewardLocation2);
                        spawnedRed.lootTable.lootTable = (UnityEngine.Random.value <= 0.5f ? GameManager.Instance.RewardManager.GunsLootTable : GameManager.Instance.RewardManager.ItemsLootTable);
                        spawnedRed.RegisterChestOnMinimap(spawnedRed.GetAbsoluteParentRoom());
                        hasDeterminedValidFloor = true;
                        break;
                    case GlobalDungeonData.ValidTilesets.RATGEON: //RAT LAIR
                        LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(727).gameObject, user);
                        hasDeterminedValidFloor = true;
                        break;
                    case GlobalDungeonData.ValidTilesets.CATACOMBGEON: // HOLLOW
                        IntVector2 bestRewardLocation3 = user.CurrentRoom.GetBestRewardLocation(IntVector2.One * 3, RoomHandler.RewardLocationStyle.PlayerCenter, true);
                        Chest spawnedChestRandomHollow = GameManager.Instance.RewardManager.SpawnRewardChestAt(bestRewardLocation3);                        
                        spawnedChestRandomHollow.RegisterChestOnMinimap(spawnedChestRandomHollow.GetAbsoluteParentRoom());
                        ChangeStatPermanent(user, PlayerStats.StatType.GlobalPriceMultiplier, 0.8f, StatModifier.ModifyMethod.MULTIPLICATIVE);
                        hasDeterminedValidFloor = true;
                        break;
                    case GlobalDungeonData.ValidTilesets.OFFICEGEON: //R&G DEPT
                        for (int i = 0; i < 2; i++)
                        {
                            IntVector2 bestRewardLocation4 = user.CurrentRoom.GetRandomVisibleClearSpot(2, 2);
                            Chest spawnedChestRNG = GameManager.Instance.RewardManager.SpawnRewardChestAt(bestRewardLocation4);                            
                            spawnedChestRNG.RegisterChestOnMinimap(spawnedChestRNG.GetAbsoluteParentRoom());
                        }
                        hasDeterminedValidFloor = true;
                        break;
                    case GlobalDungeonData.ValidTilesets.WESTGEON: //OLD WEST FLOOR (EXPAND)
                        LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(727).gameObject, user);
                        LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(137).gameObject, user);
                        hasDeterminedValidFloor = true;
                        break;
                    case GlobalDungeonData.ValidTilesets.FORGEGEON: //FORGE
                        for (int i = 0; i < 6; i++) LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(120).gameObject, user);
                        PickupObject itemOfTypeAndQuality = LootEngine.GetItemOfTypeAndQuality<PickupObject>(BraveUtility.RandomElement(BToSItemTiers), GameManager.Instance.RewardManager.GunsLootTable, false);
                        LootEngine.SpawnItem(itemOfTypeAndQuality.gameObject, LastOwner.specRigidbody.UnitCenter, Vector2.left, 1f, false, true, false);
                        hasDeterminedValidFloor = true;
                        break;
                    case GlobalDungeonData.ValidTilesets.HELLGEON: //BULLET HELL
                        if (GameManager.IsGunslingerPast)
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                IntVector2 bestRewardLocation5 = user.CurrentRoom.GetRandomVisibleClearSpot(2, 2);
                                Chest black_Chest = GameManager.Instance.RewardManager.S_Chest;
                                black_Chest.IsLocked = true;
                                black_Chest.ChestType = BraveUtility.RandomElement(ChestyBois);
                                Chest spawnedBlackGunslinger = Chest.Spawn(black_Chest, bestRewardLocation5);
                                spawnedBlackGunslinger.lootTable.lootTable = (UnityEngine.Random.value <= 0.5f ? GameManager.Instance.RewardManager.GunsLootTable : GameManager.Instance.RewardManager.ItemsLootTable);
                                spawnedBlackGunslinger.RegisterChestOnMinimap(spawnedBlackGunslinger.GetAbsoluteParentRoom());
                            }
                            ChangeStatPermanent(user, PlayerStats.StatType.Curse, 5, StatModifier.ModifyMethod.ADDITIVE);
                        }
                        else
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                IntVector2 bestRewardLocation4 = user.CurrentRoom.GetRandomVisibleClearSpot(2, 2);
                                Chest randomHellMagChest = GameManager.Instance.RewardManager.SpawnRewardChestAt(bestRewardLocation4);                               
                                randomHellMagChest.RegisterChestOnMinimap(randomHellMagChest.GetAbsoluteParentRoom());
                            }
                            for (int i = 0; i < 2; i++)
                            {
                                IntVector2 bestRewardLocation4 = user.CurrentRoom.GetRandomVisibleClearSpot(2, 2);
                                Chest randomHellRandoChest = GameManager.Instance.RewardManager.SpawnTotallyRandomChest(bestRewardLocation4);                             
                                randomHellRandoChest.RegisterChestOnMinimap(randomHellRandoChest.GetAbsoluteParentRoom());
                            }
                            ChangeStatPermanent(user, PlayerStats.StatType.Curse, 3, StatModifier.ModifyMethod.ADDITIVE);
                        }
                        hasDeterminedValidFloor = true;
                        break;
                    case GlobalDungeonData.ValidTilesets.BELLYGEON: //BELLY
                        ChangeStatPermanent(user, PlayerStats.StatType.Health, 1, StatModifier.ModifyMethod.ADDITIVE);
                        user.healthHaver.ApplyHealing(100f);
                        if (user.characterIdentity == PlayableCharacters.Robot)
                        {
                            for (int i = 0; i < 5; i++) LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(120).gameObject, user);
                        }
                        hasDeterminedValidFloor = true;
                        break;
                }
                //-----------------------------------------DEFAULT CATCH EFFECT
                if (!hasDeterminedValidFloor)
                {
                    LootEngine.SpawnItem(PickupObjectDatabase.GetById(127).gameObject, user.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
                }
            }
        }
        public static List<PickupObject.ItemQuality> BToSItemTiers = new List<PickupObject.ItemQuality>()
        {
            ItemQuality.B,
            ItemQuality.A,
            ItemQuality.S,
        };
        public static List<Chest.GeneralChestType> ChestyBois = new List<Chest.GeneralChestType>()
        {
            Chest.GeneralChestType.ITEM,
            Chest.GeneralChestType.WEAPON,
        };

        private void ChangeStatPermanent(PlayerController target, PlayerStats.StatType statToChance, float amount, StatModifier.ModifyMethod modifyMethod)
        {
            StatModifier statModifier = new StatModifier();
            statModifier.amount = amount;
            statModifier.modifyType = modifyMethod;
            statModifier.statToBoost = statToChance;
            target.ownerlessStatModifiers.Add(statModifier);
            target.stats.RecalculateStats(target, false, false);
        }
    }
}
