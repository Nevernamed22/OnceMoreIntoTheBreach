using NpcApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LootTableAPI;
using UnityEngine;
using GungeonAPI;

namespace NevernamedsItems
{
    public static class Rusty
    {
        public static void Init()
        {
            ETGMod.Databases.Strings.Core.Set("#RUSTY_GENERIC_TALK", "I sell, yes, you! Buy! Cash yes money yes yes yes!");
            ETGMod.Databases.Strings.Core.Set("#RUSTY_STOPPER_TALK", "You are boring. Rusty is bored yes bored no yes");
            ETGMod.Databases.Strings.Core.Set("#RUSTY_PURCHASE_TALK", "YesYes! Deal Yes! Buy!");
            ETGMod.Databases.Strings.Core.Set("#RUSTY_NOSALE_TALK", "No. TooCheap. Even for me, hehehehehehh");
            ETGMod.Databases.Strings.Core.Set("#RUSTY_INTRO_TALK", "Oh! You are back! Yes! Heheh! BuyBuy!");
            ETGMod.Databases.Strings.Core.Set("#RUSTY_ATTACKED_TALK", "Rusty no die. Rusty live forever.");

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
                TracerRound.TracerRoundsID,
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
            };

            GenericLootTable RustyLootTable = LootTableTools.CreateLootTable();
            foreach (int i in LootTable)
            {
                RustyLootTable.AddItemToPool(i);
            }

            GameObject rustyObj = ItsDaFuckinShopApi.SetUpShop(
                         "Rusty",
                         "omitb",
                         new List<string>()
                         {
                        "NevernamedsItems/Resources/NPCSprites/Rusty/rusty_idle_001",
                        "NevernamedsItems/Resources/NPCSprites/Rusty/rusty_idle_002",
                        "NevernamedsItems/Resources/NPCSprites/Rusty/rusty_idle_003",
                        "NevernamedsItems/Resources/NPCSprites/Rusty/rusty_idle_004",
                        "NevernamedsItems/Resources/NPCSprites/Rusty/rusty_idle_005",
                        "NevernamedsItems/Resources/NPCSprites/Rusty/rusty_idle_006",
                         },
                         12,
                         new List<string>()
                         {
                        "NevernamedsItems/Resources/NPCSprites/Rusty/rusty_talk_001",
                        "NevernamedsItems/Resources/NPCSprites/Rusty/rusty_talk_002",
                        "NevernamedsItems/Resources/NPCSprites/Rusty/rusty_talk_003",
                        "NevernamedsItems/Resources/NPCSprites/Rusty/rusty_talk_004",
                        "NevernamedsItems/Resources/NPCSprites/Rusty/rusty_talk_005",
                         },
                         12,
                         RustyLootTable,
                         BaseShopController.AdditionalShopType.TRUCK,
                         "#RUSTY_GENERIC_TALK",
                         "#RUSTY_STOPPER_TALK",
                         "#RUSTY_PURCHASE_TALK",
                         "#RUSTY_NOSALE_TALK",
                         "#RUSTY_INTRO_TALK",
                         "#RUSTY_ATTACKED_TALK",
                         0.5f,
                         true,
                         "NevernamedsItems/Resources/NPCSprites/Rusty/rustycarpet"
                         );

            PrototypeDungeonRoom Mod_Shop_Room = RoomFactory.BuildFromResource("NevernamedsItems/Resources/EmbeddedRooms/RustyRoom.room").room;
            ItsDaFuckinShopApi.RegisterShopRoom(rustyObj, Mod_Shop_Room, new UnityEngine.Vector2(-2.5f, -3));
        }
    }
}
