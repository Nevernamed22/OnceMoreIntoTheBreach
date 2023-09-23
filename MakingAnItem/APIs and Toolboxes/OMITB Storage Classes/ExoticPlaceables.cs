using Alexandria.ItemAPI;
using Dungeonator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    class ExoticPlaceables
    {
        public static void Init()
        {
            #region SpecialLoot
            NonRatStealableArmor = PickupObjectDatabase.GetById(120).gameObject.InstantiateAndFakeprefab();
            NonRatStealableArmor.GetComponent<PickupObject>().IgnoredByRat = true;

            NonRatStealableAmmo = PickupObjectDatabase.GetById(78).gameObject.InstantiateAndFakeprefab();
            NonRatStealableAmmo.GetComponent<PickupObject>().IgnoredByRat = true;

            NonRatStealableSpreadAmmo = PickupObjectDatabase.GetById(600).gameObject.InstantiateAndFakeprefab();
            NonRatStealableSpreadAmmo.GetComponent<PickupObject>().IgnoredByRat = true;

            MapPlaceable = PickupObjectDatabase.GetById(137).gameObject.InstantiateAndFakeprefab();
            MapPlaceable.GetComponent<PickupObject>().IgnoredByRat = true;
            MapPlaceable.GetOrAddComponent<SquishyBounceWiggler>();

            GlassGuonPlaceable = PickupObjectDatabase.GetById(565).gameObject.InstantiateAndFakeprefab();
            GlassGuonPlaceable.GetComponent<PickupObject>().IgnoredByRat = true;
            GlassGuonPlaceable.GetOrAddComponent<SquishyBounceWiggler>();

            dragunSkull.GetComponent<MajorBreakable>().SpawnItemOnBreak = false;
            if (dragunSkull.GetComponent<DebrisObject>()) UnityEngine.Object.Destroy(dragunSkull.GetComponent<DebrisObject>());
            if (dragunVertebrae.GetComponent<DebrisObject>()) UnityEngine.Object.Destroy(dragunVertebrae.GetComponent<DebrisObject>());

            FiftyCasingPlaceable = PickupObjectDatabase.GetById(74).gameObject.InstantiateAndFakeprefab();
            if (FiftyCasingPlaceable.GetComponent<PickupMover>()) UnityEngine.Object.Destroy(FiftyCasingPlaceable.GetComponent<PickupMover>());

            SingleCasingPlaceable = PickupObjectDatabase.GetById(68).gameObject.InstantiateAndFakeprefab();
            if (SingleCasingPlaceable.GetComponent<PickupMover>()) UnityEngine.Object.Destroy(SingleCasingPlaceable.GetComponent<PickupMover>());

            FiveCasingPlaceable = PickupObjectDatabase.GetById(70).gameObject.InstantiateAndFakeprefab();
            if (FiveCasingPlaceable.GetComponent<PickupMover>()) UnityEngine.Object.Destroy(FiveCasingPlaceable.GetComponent<PickupMover>());
            #endregion
            #region NonFloorStolen
            BulletKingThrone = EnemyDatabase.GetOrLoadByGuid("ffca09398635467da3b1f4a54bcfda80").GetComponent<BulletKingDeathController>().thronePrefab.InstantiateAndFakeprefab();
            OldKingThrone = EnemyDatabase.GetOrLoadByGuid("5729c8b5ffa7415bb3d01205663a33ef").GetComponent<BulletKingDeathController>().thronePrefab.InstantiateAndFakeprefab();
            GameObject skeletonNote = LoadHelper.LoadAssetFromAnywhere<GameObject>("assets/data/prefabs/interactable objects/notes/skeleton_note_001.prefab");
            SecretRoomSkeleton = skeletonNote.transform.Find("skleleton").gameObject.InstantiateAndFakeprefab();
            #endregion
            #region Oubliette
            Dungeon sewerDungeon = DungeonDatabase.GetOrLoadByName("base_sewer");
            foreach (WeightedRoom wRoom in sewerDungeon.PatternSettings.flows[0].fallbackRoomTable.includedRooms.elements)
            {
                if (wRoom.room != null && !string.IsNullOrEmpty(wRoom.room.name))
                {
                    if (wRoom.room.name.ToLower().StartsWith("sewer_trash_compactor_001"))
                    {
                        HorizontalCrusher = wRoom.room.placedObjects[0].nonenemyBehaviour.gameObject.InstantiateAndFakeprefab();
                    }
                }
            }
            sewerDungeon = null;
            #endregion
            #region Abbey
            Dungeon abbeyDungeon = DungeonDatabase.GetOrLoadByName("base_cathedral");
            foreach (WeightedRoom wRoom in abbeyDungeon.PatternSettings.flows[0].fallbackRoomTable.includedRooms.elements)
            {
                if (wRoom.room != null && !string.IsNullOrEmpty(wRoom.room.name))
                {
                    if (wRoom.room.name.ToLower().StartsWith("cathedral_brent_standard_02"))
                    {
                        Pew = wRoom.room.placedObjects[0].nonenemyBehaviour.gameObject.InstantiateAndFakeprefab();
                    }
                }
            }
            abbeylight = abbeyDungeon.roomMaterialDefinitions[0].facewallLightStamps[0].objectReference;
            abbeylightside = abbeyDungeon.roomMaterialDefinitions[0].sidewallLightStamps[0].objectReference;
            tanpot = abbeyDungeon.stampData.objectStamps[14].objectReference;

            abbeyDungeon = null;
            #endregion
            #region R&GDept
            Dungeon rngDungeon = DungeonDatabase.GetOrLoadByName("base_nakatomi");
            if (rngDungeon)
            {
                rnglight = rngDungeon.roomMaterialDefinitions[0].facewallLightStamps[0].objectReference;
                rnglightside = rngDungeon.roomMaterialDefinitions[0].sidewallLightStamps[0].objectReference;

                futurelight = rngDungeon.roomMaterialDefinitions[7].facewallLightStamps[0].objectReference;
                futurelightside = rngDungeon.roomMaterialDefinitions[7].sidewallLightStamps[0].objectReference;

                if (rngDungeon.PatternSettings.flows[0].name == "FS4_Nakatomi_Flow")
                {
                    if (rngDungeon.PatternSettings.flows[0].AllNodes.Count == 14)
                    {
                        MopAndBucket = rngDungeon.PatternSettings.flows[0].AllNodes[0].overrideExactRoom.placedObjects[0].nonenemyBehaviour.gameObject.InstantiateAndFakeprefab();
                        CardboardBox3 = rngDungeon.PatternSettings.flows[0].AllNodes[0].overrideExactRoom.placedObjects[2].nonenemyBehaviour.gameObject.InstantiateAndFakeprefab();
                        ACUnit = rngDungeon.PatternSettings.flows[0].AllNodes[1].overrideExactRoom.placedObjects[1].nonenemyBehaviour.gameObject.InstantiateAndFakeprefab();
                        ACVent = rngDungeon.PatternSettings.flows[0].AllNodes[1].overrideExactRoom.placedObjects[2].nonenemyBehaviour.gameObject.InstantiateAndFakeprefab();
                        KitchenChairFront = rngDungeon.PatternSettings.flows[0].AllNodes[4].overrideExactRoom.placedObjects[1].nonenemyBehaviour.gameObject.InstantiateAndFakeprefab();
                        KitchenChairLeft = rngDungeon.PatternSettings.flows[0].AllNodes[4].overrideExactRoom.placedObjects[8].nonenemyBehaviour.gameObject.InstantiateAndFakeprefab();
                        KitchenChairRight = rngDungeon.PatternSettings.flows[0].AllNodes[4].overrideExactRoom.placedObjects[12].nonenemyBehaviour.gameObject.InstantiateAndFakeprefab();
                        KitchenCounter = rngDungeon.PatternSettings.flows[0].AllNodes[4].overrideExactRoom.placedObjects[16].nonenemyBehaviour.gameObject.InstantiateAndFakeprefab();
                        SteelTableHorizontal = rngDungeon.PatternSettings.flows[0].AllNodes[4].overrideExactRoom.placedObjects[6].nonenemyBehaviour.gameObject.InstantiateAndFakeprefab();
                        SteelTableVertical = rngDungeon.PatternSettings.flows[0].AllNodes[4].overrideExactRoom.placedObjects[3].nonenemyBehaviour.gameObject.InstantiateAndFakeprefab();
                        BathroomStallDividerNorth = rngDungeon.PatternSettings.flows[0].AllNodes[6].overrideExactRoom.placedObjects[0].nonenemyBehaviour.gameObject.InstantiateAndFakeprefab();
                        BathroomStallDividerEast = rngDungeon.PatternSettings.flows[0].AllNodes[6].overrideExactRoom.placedObjects[6].nonenemyBehaviour.gameObject.InstantiateAndFakeprefab();
                        BathroomStallDividerWest = rngDungeon.PatternSettings.flows[0].AllNodes[6].overrideExactRoom.placedObjects[9].nonenemyBehaviour.gameObject.InstantiateAndFakeprefab();
                        ToiletNorth = rngDungeon.PatternSettings.flows[0].AllNodes[6].overrideExactRoom.placedObjects[2].nonenemyBehaviour.gameObject.InstantiateAndFakeprefab();
                        ToiletEast = rngDungeon.PatternSettings.flows[0].AllNodes[6].overrideExactRoom.placedObjects[7].nonenemyBehaviour.gameObject.InstantiateAndFakeprefab();
                        ToiletWest = rngDungeon.PatternSettings.flows[0].AllNodes[6].overrideExactRoom.placedObjects[10].nonenemyBehaviour.gameObject.InstantiateAndFakeprefab();
                        GlassWallVertical = rngDungeon.PatternSettings.flows[0].AllNodes[7].overrideExactRoom.placedObjects[0].nonenemyBehaviour.gameObject.InstantiateAndFakeprefab();
                        GlassWallHorizontal = rngDungeon.PatternSettings.flows[0].AllNodes[7].overrideExactRoom.placedObjects[6].nonenemyBehaviour.gameObject.InstantiateAndFakeprefab();
                        LargeDesk = rngDungeon.PatternSettings.flows[0].AllNodes[8].overrideExactRoom.placedObjects[0].nonenemyBehaviour.gameObject.InstantiateAndFakeprefab();
                        TechnoFloorCellEmpty = rngDungeon.PatternSettings.flows[0].AllNodes[10].overrideExactRoom.placedObjects[0].nonenemyBehaviour.gameObject.InstantiateAndFakeprefab();
                        TechnoFloorCellCaterpillar = rngDungeon.PatternSettings.flows[0].AllNodes[10].overrideExactRoom.placedObjects[4].nonenemyBehaviour.gameObject.InstantiateAndFakeprefab();
                        TechnoFloorCellLeever = rngDungeon.PatternSettings.flows[0].AllNodes[10].overrideExactRoom.placedObjects[13].nonenemyBehaviour.gameObject.InstantiateAndFakeprefab();
                        TechnoFloorCellSpider = rngDungeon.PatternSettings.flows[0].AllNodes[10].overrideExactRoom.placedObjects[14].nonenemyBehaviour.gameObject.InstantiateAndFakeprefab();
                        WideComputerBreakable = rngDungeon.PatternSettings.flows[0].AllNodes[10].overrideExactRoom.placedObjects[6].nonenemyBehaviour.gameObject.InstantiateAndFakeprefab();
                        MetalCrate = rngDungeon.PatternSettings.flows[0].AllNodes[10].overrideExactRoom.placedObjects[10].nonenemyBehaviour.gameObject.InstantiateAndFakeprefab();
                        HologramWallHorizontal = rngDungeon.PatternSettings.flows[0].AllNodes[11].overrideExactRoom.placedObjects[0].nonenemyBehaviour.gameObject.InstantiateAndFakeprefab();
                        HologramWallVertical = rngDungeon.PatternSettings.flows[0].AllNodes[11].overrideExactRoom.placedObjects[7].nonenemyBehaviour.gameObject.InstantiateAndFakeprefab();
                        VentilationTube = rngDungeon.PatternSettings.flows[0].AllNodes[11].overrideExactRoom.placedObjects[8].nonenemyBehaviour.gameObject.InstantiateAndFakeprefab();
                        TallComputerBreakable = rngDungeon.PatternSettings.flows[0].AllNodes[11].overrideExactRoom.placedObjects[13].nonenemyBehaviour.gameObject.InstantiateAndFakeprefab();
                        AgunimBossMatt = rngDungeon.PatternSettings.flows[0].AllNodes[12].overrideExactRoom.placedObjects[1].nonenemyBehaviour.gameObject.InstantiateAndFakeprefab();
                        AlienTank = rngDungeon.PatternSettings.flows[0].AllNodes[13].overrideExactRoom.placedObjects[9].nonenemyBehaviour.gameObject.InstantiateAndFakeprefab();
                        DecorativeElectricFloor = rngDungeon.PatternSettings.flows[0].AllNodes[13].overrideExactRoom.placedObjects[29].nonenemyBehaviour.gameObject.InstantiateAndFakeprefab();
                    }
                    else { ETGModConsole.Log("<color=#ff0000ff>ERROR: R&G DEPARTMENT FLOW 0 HAS AN INCORRECT AMOUNT OF NODES</color>"); }
                }
                else { ETGModConsole.Log("<color=#ff0000ff>ERROR: R&G DEPARTMENT FLOW 0 HAS AN INCORRECT NAME, AND HAS BEEN ALTERED</color>"); }
            }
            else { ETGModConsole.Log("<color=#ff0000ff>ERROR: R&G DEPARTMENT DUNGEON PREFAB WAS NULL</color>"); }
            rngDungeon = null;
            #endregion
            #region Forge
            Dungeon forgeDungeon = DungeonDatabase.GetOrLoadByName("base_forge");

            anvil1 = forgeDungeon.stampData.objectStamps[6].objectReference;
            anvil2 = forgeDungeon.stampData.objectStamps[7].objectReference;


            forgelight = forgeDungeon.roomMaterialDefinitions[0].facewallLightStamps[0].objectReference;
            forgelightside = forgeDungeon.roomMaterialDefinitions[0].sidewallLightStamps[0].objectReference;

            foreach (WeightedRoom wRoom in forgeDungeon.PatternSettings.flows[0].fallbackRoomTable.includedRooms.elements)
            {
                if (wRoom.room != null && !string.IsNullOrEmpty(wRoom.room.name))
                {
                    if (wRoom.room.name.ToLower().StartsWith("forge_normal_cubulead_03"))
                    {
                        VerticalCrusher = wRoom.room.placedObjects[0].nonenemyBehaviour.gameObject.InstantiateAndFakeprefab();
                        FireBarTrap = wRoom.room.placedObjects[7].nonenemyBehaviour.gameObject.InstantiateAndFakeprefab();
                    }
                    if (wRoom.room.name.ToLower().StartsWith("forge_connector_flamepipes_01"))
                    {
                        FlamePipeNorth = wRoom.room.placedObjects[1].nonenemyBehaviour.gameObject.InstantiateAndFakeprefab();
                        FlamePipeWest = wRoom.room.placedObjects[3].nonenemyBehaviour.gameObject.InstantiateAndFakeprefab();
                        FlamePipeEast = wRoom.room.placedObjects[2].nonenemyBehaviour.gameObject.InstantiateAndFakeprefab();
                    }

                }
            }
            forgeDungeon = null;
            #endregion
            #region BulletHell
            Dungeon hellDungeon = DungeonDatabase.GetOrLoadByName("base_bullethell");
            foreach (WeightedRoom wRoom in hellDungeon.PatternSettings.flows[0].fallbackRoomTable.includedRooms.elements)
            {
                if (wRoom.room != null && !string.IsNullOrEmpty(wRoom.room.name))
                {
                    if (wRoom.room.name.ToLower().StartsWith("hell_connector_pathburst_01"))
                    {
                        FlameburstTrap = wRoom.room.placedObjects[3].nonenemyBehaviour.gameObject.InstantiateAndFakeprefab();
                    }
                }
            }

            helleton = hellDungeon.roomMaterialDefinitions[0].facewallLightStamps[0].objectReference;
            helletonside = hellDungeon.roomMaterialDefinitions[0].sidewallLightStamps[0].objectReference;


            hellDungeon = null;
            #endregion

            #region Hollow
            Dungeon hollowDungeon = DungeonDatabase.GetOrLoadByName("base_catacombs");

            skel0 = hollowDungeon.stampData.objectStamps[0].objectReference;
            skel1 = hollowDungeon.stampData.objectStamps[1].objectReference;
            skel2 = hollowDungeon.stampData.objectStamps[2].objectReference;
            skel3 = hollowDungeon.stampData.objectStamps[3].objectReference;
            skel6 = hollowDungeon.stampData.objectStamps[6].objectReference;
            skel7 = hollowDungeon.stampData.objectStamps[7].objectReference;

            hollowDungeon = null;
            #endregion

            #region BulletPast
            Dungeon bulletPast = DungeonDatabase.GetOrLoadByName("finalscenario_bullet");

            crumbletrap = bulletPast.PatternSettings.flows[0].AllNodes[3].overrideExactRoom.placedObjects[1].nonenemyBehaviour.gameObject.InstantiateAndFakeprefab();

            bulletPast = null;
            #endregion

            #region Keep
            Dungeon keepdungeon = DungeonDatabase.GetOrLoadByName("base_castle");

            FireplaceRoomTable = keepdungeon.PatternSettings.flows[0].sharedInjectionData[1].InjectionData[1].roomTable;

            RewardCellTable = keepdungeon.PatternSettings.flows[0].sharedInjectionData[0].InjectionData[1].roomTable;
            OubTrapdoor = keepdungeon.PatternSettings.flows[0].sharedInjectionData[1].InjectionData[0].exactRoom.placedObjects[0].nonenemyBehaviour.gameObject;

            keepdungeon = null;
            #endregion

            Dungeon tutorialDungeon = DungeonDatabase.GetOrLoadByName("base_tutorial");

            SignRight = tutorialDungeon.PatternSettings.flows[0].AllNodes[11].overrideExactRoom.placedObjects[13].nonenemyBehaviour.gameObject;
            SignLeft = tutorialDungeon.PatternSettings.flows[0].AllNodes[11].overrideExactRoom.placedObjects[14].nonenemyBehaviour.gameObject;
            SignUp = tutorialDungeon.PatternSettings.flows[0].AllNodes[9].overrideExactRoom.placedObjects[1].nonenemyBehaviour.gameObject;

            secretroomlayout = tutorialDungeon.PatternSettings.flows[0].AllNodes[15].overrideExactRoom.placedObjects[0].nonenemyBehaviour.gameObject.transform.GetChild(2).gameObject;
      
            tutorialDungeon = null;

            PrototypeDungeonRoom roomPrefab = LoadHelper.LoadAssetFromAnywhere<PrototypeDungeonRoom>("shop02");
            TeleporterSign = roomPrefab.placedObjects[10].nonenemyBehaviour.gameObject;
            ShopLayout = roomPrefab.placedObjects[12].nonenemyBehaviour.gameObject;
            Crates = ShopLayout.transform.GetChild(1).gameObject;
            Crate = ShopLayout.transform.GetChild(5).gameObject;
            Sack = ShopLayout.transform.GetChild(3).gameObject;
            ShellBarrel = ShopLayout.transform.GetChild(10).gameObject;
            Shelf = ShopLayout.transform.GetChild(7).gameObject;
            Mask = ShopLayout.transform.GetChild(6).gameObject;
            Wallsword = ShopLayout.transform.GetChild(11).gameObject;
            StandingShelf = ShopLayout.transform.GetChild(8).gameObject;
            AKBarrel = ShopLayout.transform.GetChild(9).gameObject;
            Stool = ShopLayout.transform.GetChild(12).gameObject;
            roomPrefab = null;


        }

        #region Objects
        public static GameObject secretroomlayout;

        public static GameObject SignUp;
        public static GameObject SignLeft;
        public static GameObject SignRight;


        public static GameObject Crates;
        public static GameObject Crate;
        public static GameObject Sack;
        public static GameObject ShellBarrel;
        public static GameObject Shelf;
        public static GameObject Mask;
        public static GameObject Wallsword;
        public static GameObject StandingShelf;
        public static GameObject AKBarrel;
        public static GameObject Stool;


        public static GameObject TeleporterSign;
        public static GameObject ShopLayout;

        public static GenericRoomTable FireplaceRoomTable;
        public static GenericRoomTable RewardCellTable;

        public static GameObject OubTrapdoor;
        public static GameObject OubButton;
        //Loot
        public static GameObject NonRatStealableArmor;
        public static GameObject NonRatStealableAmmo;
        public static GameObject NonRatStealableSpreadAmmo;
        public static GameObject MapPlaceable;
        public static GameObject GlassGuonPlaceable;
        public static GameObject FiftyCasingPlaceable;
        public static GameObject SingleCasingPlaceable;
        public static GameObject FiveCasingPlaceable;
        //Traps
        public static GameObject HorizontalCrusher;
        public static GameObject VerticalCrusher;
        public static GameObject FireBarTrap;
        public static GameObject FlamePipeNorth;
        public static GameObject FlamePipeEast;
        public static GameObject FlamePipeWest;
        public static GameObject FlameburstTrap;
        //Decoration
        public static GameObject MopAndBucket;
        public static GameObject SecretRoomSkeleton;
        public static GameObject Pew;
        public static GameObject OldKingThrone;
        public static GameObject BulletKingThrone;
        public static GameObject AlienTank;
        public static GameObject DecorativeElectricFloor;
        public static GameObject AgunimBossMatt;
        public static GameObject TallComputerBreakable;
        public static GameObject VentilationTube;
        public static GameObject HologramWallVertical;
        public static GameObject HologramWallHorizontal;
        public static GameObject MetalCrate;
        public static GameObject WideComputerBreakable;
        public static GameObject TechnoFloorCellSpider;
        public static GameObject TechnoFloorCellLeever;
        public static GameObject TechnoFloorCellCaterpillar;
        public static GameObject TechnoFloorCellEmpty;
        public static GameObject LargeDesk;
        public static GameObject GlassWallHorizontal;
        public static GameObject GlassWallVertical;
        public static GameObject ToiletWest;
        public static GameObject ToiletEast;
        public static GameObject ToiletNorth;
        public static GameObject BathroomStallDividerWest;
        public static GameObject BathroomStallDividerEast;
        public static GameObject BathroomStallDividerNorth;
        public static GameObject SteelTableVertical;
        public static GameObject SteelTableHorizontal;
        public static GameObject KitchenCounter;
        public static GameObject KitchenChairRight;
        public static GameObject KitchenChairLeft;
        public static GameObject KitchenChairFront;
        public static GameObject ACVent;
        public static GameObject ACUnit;
        public static GameObject CardboardBox3;

        public static GameObject abbeylight;
        public static GameObject abbeylightside;
        public static GameObject tanpot;

        public static GameObject skel0;
        public static GameObject skel1;
        public static GameObject skel2;
        public static GameObject skel3;
        public static GameObject skel6;
        public static GameObject skel7;

        public static GameObject anvil1;
        public static GameObject anvil2;

        public static GameObject helleton;
        public static GameObject helletonside;

        public static GameObject rnglight;
        public static GameObject rnglightside;

        public static GameObject futurelight;
        public static GameObject futurelightside;

        public static GameObject forgelight;
        public static GameObject forgelightside;

        public static GameObject crumbletrap;


        public static GameObject dragunVertebrae = EnemyDatabase.GetOrLoadByGuid("465da2bb086a4a88a803f79fe3a27677").GetComponent<DraGunDeathController>().neckDebris.InstantiateAndFakeprefab();
        public static GameObject dragunSkull = EnemyDatabase.GetOrLoadByGuid("465da2bb086a4a88a803f79fe3a27677").GetComponent<DraGunDeathController>().skullDebris.InstantiateAndFakeprefab();
        #endregion
    }
}
