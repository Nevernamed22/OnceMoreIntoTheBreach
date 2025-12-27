using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SaveAPI;
using Alexandria.DungeonAPI;
using Alexandria.NPCAPI;
using Alexandria.Misc;
using Dungeonator;
using Alexandria.BreakableAPI;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    public static class Rusty
    {
        public static GameObject mapIcon;
        public static GenericLootTable RustyLootTable;
        public static void AddToLootPool(int id)
        {
            if (RustyLootTable == null) { RustyLootTable = LootUtility.CreateLootTable(); }
            RustyLootTable.AddItemToPool(id);
        }
        public static void Init()
        {
            #region Strings
            ETGMod.Databases.Strings.Core.AddComplex("#RUSTY_GENERIC_TALK", "I sell, yes, you! Buy! Cash yes money yes yes yes!");
            ETGMod.Databases.Strings.Core.AddComplex("#RUSTY_GENERIC_TALK", "Rusty sells trash people throw at him. Make easy money. Heheh. Fools.");
            ETGMod.Databases.Strings.Core.AddComplex("#RUSTY_GENERIC_TALK", "You want to talk to Rusty? Nobody ever talks to Rusty.");
            ETGMod.Databases.Strings.Core.AddComplex("#RUSTY_GENERIC_TALK", "I used to be taaaaall once, yes, yes. Taaaall, and red. Red.");
            ETGMod.Databases.Strings.Core.AddComplex("#RUSTY_GENERIC_TALK", "They laugh at Rusty, yes... they all laugh...");
            ETGMod.Databases.Strings.Core.AddComplex("#RUSTY_GENERIC_TALK", "Rusty has seen things... things you are not ready to see.");
            ETGMod.Databases.Strings.Core.AddComplex("#RUSTY_GENERIC_TALK", "Chambers lurk below. Places you've never seen. Perhaps you will be ready one day, yes.");
            ETGMod.Databases.Strings.Core.AddComplex("#RUSTY_GENERIC_TALK", "Don't trust the mad bullet Alhazard. Rusty trusted him, and is now Rusty.");
            ETGMod.Databases.Strings.Core.AddComplex("#RUSTY_GENERIC_TALK", "What is it like. To have skin. Rusty wonders?");
            ETGMod.Databases.Strings.Core.AddComplex("#RUSTY_GENERIC_TALK", "One day you and Rusty will be not so different, rustythinks yes.");
            ETGMod.Databases.Strings.Core.AddComplex("#RUSTY_GENERIC_TALK", "One day, the skies will run black with the ichor of ages, and all will be unloaded...  what?");

            ETGMod.Databases.Strings.Core.AddComplex("#RUSTY_STOPPER_TALK", "You are boring. Rusty is bored yes bored no yes");
            ETGMod.Databases.Strings.Core.AddComplex("#RUSTY_STOPPER_TALK", "Rusty has no more to say to you, no, no.");
            ETGMod.Databases.Strings.Core.AddComplex("#RUSTY_STOPPER_TALK", "Mmmmm, yesyesyesyesyesyesyes.");

            ETGMod.Databases.Strings.Core.AddComplex("#RUSTY_PURCHASE_TALK", "YesYes! Deal Yes! Buy!");
            ETGMod.Databases.Strings.Core.AddComplex("#RUSTY_PURCHASE_TALK", "A good choice, yes!");
            ETGMod.Databases.Strings.Core.AddComplex("#RUSTY_PURCHASE_TALK", "A poor choice, yes!");
            ETGMod.Databases.Strings.Core.AddComplex("#RUSTY_PURCHASE_TALK", "Rusty lives another day.");
            ETGMod.Databases.Strings.Core.AddComplex("#RUSTY_PURCHASE_TALK", "Rusty will buy a new can of polish!");


            ETGMod.Databases.Strings.Core.AddComplex("#RUSTY_NOSALE_TALK", "No. TooCheap. Even for me, hehehehehehh");
            ETGMod.Databases.Strings.Core.AddComplex("#RUSTY_NOSALE_TALK", "Cash, upfront. No credit.");
            ETGMod.Databases.Strings.Core.AddComplex("#RUSTY_NOSALE_TALK", "Rusty no give credit. You come back when you're richer!");

            ETGMod.Databases.Strings.Core.AddComplex("#RUSTY_INTRO_TALK", "Oh! You are back! Yes! Heheh! BuyBuy!");
            ETGMod.Databases.Strings.Core.AddComplex("#RUSTY_INTRO_TALK", "You Live! Rusty is Glad! Yes!");
            ETGMod.Databases.Strings.Core.AddComplex("#RUSTY_INTRO_TALK", "Wallet person returns, yes, with wallet gold?");

            ETGMod.Databases.Strings.Core.AddComplex("#RUSTY_ATTACKED_TALK", "Rusty no die. Rusty live forever.");
            ETGMod.Databases.Strings.Core.AddComplex("#RUSTY_ATTACKED_TALK", "If killing Rusty was that easy, Rusty would be dead.");
            ETGMod.Databases.Strings.Core.AddComplex("#RUSTY_ATTACKED_TALK", "Rusty think you are a %&**@. Yes.");

            ETGMod.Databases.Strings.Core.AddComplex("#RUSTY_STEAL_TALK", "...Rusty saw what you did... ...yes...");
            #endregion

            List<int> LootTable = new List<int>()
            {
                120, //Armour
                224, //Blank
                127, //Junk
                558, //Bottle
                108, //Bomb
                109, //Ice Bomb
                66, //Proximity Mine
                308, //Cluster Mine
                136, //C4
                366, //Molotov
                234, //iBomb Companion App
                77, //Supply Drop
                403, //Melted Rock
                432, //Jar of Bees
                447, //Shield of the Maiden
                644, //Portable Table Device
                573, //Chest Teleporter
                65, //Knife Shield
                250, //Grappling Hook
                353, //Enraging Photo
                488, //Ring of Chest Vampirism
                256, //Heavy Boots
                137, //Map
                565, //Glass Guon Stone
                306, //Escape Rope
                106, //Jetpack
                487, //Book of Chest Anatomy
                197, //Pea Shooter
                56, //38 Special
                378, //Derringer
                539, //Boxing Glove
                79, //Makarov
                8, //Bow
                9, //Dueling Pistol
                202, //Sawed Off
                122, //Blunderbuss
                12, //Crossbow
                31, //Klobbe
                181, //Winchester Rifle
                327, //Corsair
                577, //Turbo Gun
                196, //Fossilized Gun
                10, //Mega Douser
                363, //Trick Gun
                33, //Tear Jerker
                176, //Gungeon Ant
                440, //Ruby Bracelet

                //MY ITEMS
                MistakeBullets.MistakeBulletsID,
                TracerRound.ID,
                IronSights.IronSightsID,
                GlassChamber.GlassChamberID,
                BulletBoots.BulletBootsID,
                DiamondBracelet.DiamondBraceletID,
                PearlBracelet.PearlBraceletID,
                RingOfAmmoRedemption.RingOfAmmoRedemptionID,
                MapFragment.MapFragmentID,
                GunGrease.GunGreaseID,
                LuckyCoin.LuckyCoinID,
                AppleActive.AppleID,
                SpeedPotion.SpeedPotionID,
                ShroomedGun.ShroomedGunID,
                Glock42.Glock42ID,
                DiscGun.DiscGunID,
                Purpler.PurplerID,
                TheLodger.TheLodgerID,
                HeatRay.HeatRayID,
                Spitballer.ID,
                CookieClickerEgg.ID,
            };
            foreach (int i in LootTable) { AddToLootPool(i); }


            mapIcon = ItemBuilder.SpriteFromBundle("rusty_mapicon", Initialisation.NPCCollection.GetSpriteIdByName("rusty_mapicon"), Initialisation.NPCCollection, new GameObject("rusty_mapicon"));
            mapIcon.MakeFakePrefab();

            var rusty = ItemBuilder.SpriteFromBundle("rusty_idle_001", Initialisation.NPCCollection.GetSpriteIdByName("rusty_idle_001"), Initialisation.NPCCollection, new GameObject("Rusty"));
            SpeculativeRigidbody rigidbody = ShopAPI.GenerateOrAddToRigidBody(rusty, CollisionLayer.LowObstacle, PixelCollider.PixelColliderGeneration.Manual, true, true, true, false, false, false, false, true, new IntVector2(20, 11), new IntVector2(6, -1));
            rigidbody.AddCollisionLayerOverride(CollisionMask.LayerToMask(CollisionLayer.BulletBlocker));

            GameObject shopObj = TempNPCTools.MakeIntoShopkeeper("Rusty", "nn", rusty, "rusty_idle", "rusty_talk", Initialisation.NPCCollection, Initialisation.npcAnimationCollection,
                   RustyLootTable,
                   CustomShopItemController.ShopCurrencyType.COINS,
                   "#RUSTY_GENERIC_TALK",
                   "#RUSTY_STOPPER_TALK",
                   "#RUSTY_PURCHASE_TALK",
                   "#RUSTY_NOSALE_TALK",
                   "#RUSTY_INTRO_TALK",
                   "#RUSTY_ATTACKED_TALK",
                   "#RUSTY_STEAL_TALK",
                   new Vector3(17f / 16f, 22f / 16f, 0), //Textbox Offset
                   new Vector3(38f / 16f, 44f / 16f, 0),
                   itemPositions: new List<Vector3> { new Vector3(19f/16f, 50f/16f, 1), new Vector3(24f / 16f, 23f / 16f, 1), new Vector3(57f / 16f, 19f / 16f, 1) }.ToArray(),
                   hasMinimapIcon: true,
                   minimapIcon: mapIcon,
                   Carpet: "rustycarpet",
                   CarpetOffset: new Vector2(4f/16f, 4f / 16f),
                   costModifier: 0.5f,
                   addToShopAnnex: true,
                   shopAnnexWeight: 0.1f,
                   OnPurchase: RustyBuy,
                   OnSteal: RustySteal,
                   voice: "gambler"
                   );

            Dictionary<GameObject, float> dict = new Dictionary<GameObject, float>() { { shopObj, 1f } };
            DungeonPlaceable placeable = BreakableAPIToolbox.GenerateDungeonPlaceable(dict);
            placeable.isPassable = true;
            placeable.width = 5;
            placeable.height = 5;
            StaticReferences.StoredDungeonPlaceables.Add("rusty", placeable);
            Alexandria.DungeonAPI.StaticReferences.customPlaceables.Add("nn:rusty", placeable);

            SharedInjectionData npcTable = GameManager.Instance.GlobalInjectionData.entries[2].injectionData;
            npcTable.InjectionData.Add(new ProceduralFlowModifierData()
            {
                annotation = "Rusty",
                DEBUG_FORCE_SPAWN = false,
                OncePerRun = false,
                placementRules = new List<ProceduralFlowModifierData.FlowModifierPlacementType>()
                {
                    ProceduralFlowModifierData.FlowModifierPlacementType.END_OF_CHAIN
                },
                roomTable = null,
                exactRoom = RoomFactory.BuildNewRoomFromResource("NevernamedsItems/Content/NPCs/Rooms/RustyRoom.newroom").room,
                IsWarpWing = false,
                RequiresMasteryToken = false,
                chanceToLock = 0,
                selectionWeight = 1f,
                chanceToSpawn = 1,
                RequiredValidPlaceable = null,
                prerequisites = new DungeonPrerequisite[0],
                CanBeForcedSecret = false,
                RandomNodeChildMinDistanceFromEntrance = 0,
                exactSecondaryRoom = null,
                framedCombatNodes = 0,
            });
        }
        public static bool RustyBuy(PlayerController player, PickupObject item, int idfk)
        {
            SaveAPIManager.RegisterStatChange(CustomTrackedStats.RUSTY_ITEMS_PURCHASED, 1);
            return false;
        }
        public static bool RustySteal(PlayerController player, PickupObject item, int idfk)
        {
            SaveAPIManager.RegisterStatChange(CustomTrackedStats.RUSTY_ITEMS_STOLEN, 1);
            return false;
        }
    }
}
