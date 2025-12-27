using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.DungeonAPI;
using System.Reflection;
using Alexandria.NPCAPI;
using Alexandria.Misc;
using Dungeonator;
using Alexandria.BreakableAPI;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    public static class Ironside
    {
        public static GenericLootTable IronsideLootTable;
        public static GameObject mapIcon;
        public static void AddToLootPool(int id)
        {
            if (IronsideLootTable == null) { IronsideLootTable = LootUtility.CreateLootTable(); }
            IronsideLootTable.AddItemToPool(id);
        }
        public static void Init()
        {
            #region Strings
            ETGMod.Databases.Strings.Core.AddComplex("#IRONSIDE_GENERIC_TALK", "The Gungeon is a tough place, it pays to have thick skin.");
            ETGMod.Databases.Strings.Core.AddComplex("#IRONSIDE_GENERIC_TALK", "My pepaw always told me to stay prepared. So that's what I do.");
            ETGMod.Databases.Strings.Core.AddComplex("#IRONSIDE_GENERIC_TALK", "The cost of being heavily armoured is never letting anyone in.");
            ETGMod.Databases.Strings.Core.AddComplex("#IRONSIDE_GENERIC_TALK", "Names Ironside. I specialise in armour and protective supplies.");
            ETGMod.Databases.Strings.Core.AddComplex("#IRONSIDE_GENERIC_TALK", "Like the carpet? I got it custom made.");
            ETGMod.Databases.Strings.Core.AddComplex("#IRONSIDE_GENERIC_TALK", "Blanks are all well and good, but you're not always going to have them. Ask yourself, do you want bullets goin' straight to your heart(s)?");
            ETGMod.Databases.Strings.Core.AddComplex("#IRONSIDE_GENERIC_TALK", "I have done and I have dared, for everything to be prepared.");
            ETGMod.Databases.Strings.Core.AddComplex("#IRONSIDE_GENERIC_TALK", "Did you see that tarnished old slug rooting around in my garbage? What a freak.");
            ETGMod.Databases.Strings.Core.AddComplex("#IRONSIDE_GENERIC_TALK", "Armour up Gungeoneer, the road ahead is long and the bullets don't get any softer.");

            ETGMod.Databases.Strings.Core.AddComplex("#IRONSIDE_STOPPER_TALK", "No more chit-chat.");
            ETGMod.Databases.Strings.Core.AddComplex("#IRONSIDE_STOPPER_TALK", "I don't have time to talk longer.");
            ETGMod.Databases.Strings.Core.AddComplex("#IRONSIDE_STOPPER_TALK", "Got things to be, places to do.");

            ETGMod.Databases.Strings.Core.AddComplex("#IRONSIDE_PURCHASE_TALK", "Pleasure doin' business with you.");
            ETGMod.Databases.Strings.Core.AddComplex("#IRONSIDE_PURCHASE_TALK", "Pays to be prepared.");
            ETGMod.Databases.Strings.Core.AddComplex("#IRONSIDE_PURCHASE_TALK", "Armoured and ready!");

            ETGMod.Databases.Strings.Core.AddComplex("#IRONSIDE_NOSALE_TALK", "You'd better get out there and find some more [sprite \"armor_money_icon_001\"].");
            ETGMod.Databases.Strings.Core.AddComplex("#IRONSIDE_NOSALE_TALK", "Sorry pal, no [sprite \"armor_money_icon_001\"] no sale.");

            ETGMod.Databases.Strings.Core.AddComplex("#IRONSIDE_INTRO_TALK", "AVAST FIEND- Oh, no. It's just you.");
            ETGMod.Databases.Strings.Core.AddComplex("#IRONSIDE_INTRO_TALK", "Staying safe out there?");
            ETGMod.Databases.Strings.Core.AddComplex("#IRONSIDE_INTRO_TALK", "Welcome back, you're looking beat up.");
            ETGMod.Databases.Strings.Core.AddComplex("#IRONSIDE_INTRO_TALK", "Good grief... I didn't know someone could have that many bruises.");

            ETGMod.Databases.Strings.Core.AddComplex("#IRONSIDE_ATTACKED_TALK", "Hah, no way you're getting through this armour!");
            ETGMod.Databases.Strings.Core.AddComplex("#IRONSIDE_ATTACKED_TALK", "Nice try, but one of us is squishy and it ain't me.");
            ETGMod.Databases.Strings.Core.AddComplex("#IRONSIDE_ATTACKED_TALK", "You're gonna wish you hadn't done that one day.");

            ETGMod.Databases.Strings.Core.AddComplex("#IRONSIDE_STEAL_TALK", "THIEF!");
            ETGMod.Databases.Strings.Core.AddComplex("#IRONSIDE_STEAL_TALK", "TAFFER!");
            ETGMod.Databases.Strings.Core.AddComplex("#IRONSIDE_STEAL_TALK", "ARMS UP!");
            #endregion

            List<int> LootTable = new List<int>()
            {
                160, //Gunknight Stuff
                161,
                162,
                163,
                450, //Armor Synthesizer
                457, //Armor of Thorns
                64, //Potion of Lead Skin
                65, //Knife Shield
                380, //Betrayers Shield
                447, //Shield of the Maiden
                634, //Crisis Stone
                111, //Heavy Bullets
                256, //Heavy Boots
                564, //Full Metal Jacket
                290, //Sunglasses
                312, //Blast Helmet
                314, //Nanomachines
                454, //Hazmat Suit
                598, //Stone Dome
                545, //AC-15
                178, //Crestfaller
                154, //Trashcannon
                FullArmourJacket.FullArmourJacketID,
                KnightlyBullets.KnightlyBulletsID,
                ArmourBandage.ArmourBandageID,
                GoldenArmour.GoldenArmourID,
                ExoskeletalArmour.MeatShieldID,
                ArmouredArmour.ArmouredArmourID,
                KeyBullwark.KeyBulwarkID,
                TinHeart.TinHeartID,
                HeavyChamber.HeavyChamberID,
                TableTechInvulnerability.ID,
                LiquidMetalBody.LiquidMetalBodyID,
                HornedHelmet.HornedHelmetID,
                LeadSoul.LeadSoulID,
                GSwitch.GSwitchID,
                MaidenRifle.MaidenRifleID,
                RapidRiposte.ID,
                Converter.ConverterID,
            };
            foreach (int i in LootTable) { AddToLootPool(i); }

            mapIcon = ItemBuilder.SpriteFromBundle("ironside_mapicon", Initialisation.NPCCollection.GetSpriteIdByName("ironside_mapicon"), Initialisation.NPCCollection, new GameObject("ironside_mapicon"));
            mapIcon.MakeFakePrefab();

            var ironside = ItemBuilder.SpriteFromBundle("ironside_idle_001", Initialisation.NPCCollection.GetSpriteIdByName("ironside_idle_001"), Initialisation.NPCCollection, new GameObject("Ironside"));
            SpeculativeRigidbody rigidbody = ShopAPI.GenerateOrAddToRigidBody(ironside, CollisionLayer.LowObstacle, PixelCollider.PixelColliderGeneration.Manual, true, true, true, false, false, false, false, true, new IntVector2(11, 11), new IntVector2(9, -1));
            rigidbody.AddCollisionLayerOverride(CollisionMask.LayerToMask(CollisionLayer.BulletBlocker));

            GameObject shopObj = TempNPCTools.MakeIntoShopkeeper("Ironside", "nn", ironside, "ironside_idle", "ironside_talk", Initialisation.NPCCollection, Initialisation.npcAnimationCollection,
                   IronsideLootTable,
                   CustomShopItemController.ShopCurrencyType.CUSTOM,
                   "#IRONSIDE_GENERIC_TALK",
                   "#IRONSIDE_STOPPER_TALK",
                   "#IRONSIDE_PURCHASE_TALK",
                   "#IRONSIDE_NOSALE_TALK",
                   "#IRONSIDE_INTRO_TALK",
                   "#IRONSIDE_ATTACKED_TALK",
                   "#IRONSIDE_STEAL_TALK",
                   new Vector3(15f / 16f, 33f / 16f, 0), //Textbox Offset
                   new Vector3(27f / 16f, 56f / 16f, 0), //NPC Offset
                   itemPositions: ShopAPI.defaultItemPositions,
                   hasMinimapIcon: true,
                   minimapIcon: mapIcon,
                   Carpet: "ironside_carpet",
                   CarpetOffset: new Vector2(-1f / 16f, -1f / 16f),
                   CustomCanBuy: Ironside.IronsideCustomCanBuy,
                   CustomRemoveCurrency: Ironside.IronsideCustomRemoveCurrency,
                   CustomPrice: Ironside.IronsideCustomPrice,
                   OnPurchase: IronsideBuy,
                   currencyIconPath: "NevernamedsItems/Resources/NPCSprites/Ironside/armourcurrency_icon.png",
                   currencyName: "Armor",
                   addToShopAnnex: true,
                   shopAnnexWeight: 0.08f,
                   voice: "golem"
                   );

            Dictionary<GameObject, float> dict = new Dictionary<GameObject, float>() { { shopObj, 1f } };
            DungeonPlaceable placeable = BreakableAPIToolbox.GenerateDungeonPlaceable(dict);
            placeable.isPassable = true;
            placeable.width = 5;
            placeable.height = 5;
            StaticReferences.StoredDungeonPlaceables.Add("ironside", placeable);
            Alexandria.DungeonAPI.StaticReferences.customPlaceables.Add("nn:ironside", placeable);

            SharedInjectionData npcTable = GameManager.Instance.GlobalInjectionData.entries[2].injectionData;
            npcTable.InjectionData.Add(new ProceduralFlowModifierData()
            {
                annotation = "Ironside",
                DEBUG_FORCE_SPAWN = false,
                OncePerRun = false,
                placementRules = new List<ProceduralFlowModifierData.FlowModifierPlacementType>()
                {
                    ProceduralFlowModifierData.FlowModifierPlacementType.END_OF_CHAIN
                },
                roomTable = null,
                exactRoom = RoomFactory.BuildNewRoomFromResource("NevernamedsItems/Content/NPCs/Rooms/IronsideRoom.newroom").room,
                IsWarpWing = false,
                RequiresMasteryToken = false,
                chanceToLock = 0,
                selectionWeight = 0.8f,
                chanceToSpawn = 1,
                RequiredValidPlaceable = null,
                prerequisites = new DungeonPrerequisite[0],
                CanBeForcedSecret = false,
                RandomNodeChildMinDistanceFromEntrance = 0,
                exactSecondaryRoom = null,
                framedCombatNodes = 0,
            });
        }

        public static bool IronsideBuy(PlayerController player, PickupObject item, int idfk)
        {
            return false;
        }
        public static int IronsideCustomPrice(CustomShopController shop, CustomShopItemController itemCont, PickupObject item)
        {
            int price = 1;
            switch (item.quality)
            {
                case PickupObject.ItemQuality.S:
                    price = 4;
                    break;
                case PickupObject.ItemQuality.A:
                    price = 3;
                    break;
                case PickupObject.ItemQuality.B:
                    price = 2;
                    break;
                case PickupObject.ItemQuality.C:
                    price = 2;
                    break;
            }
            if (item is PassiveItem && (item as PassiveItem).ArmorToGainOnInitialPickup > 0) price += (item as PassiveItem).ArmorToGainOnInitialPickup;
            else if (item is Gun && (item as Gun).ArmorToGainOnPickup > 0) price += (item as Gun).ArmorToGainOnPickup;

            List<int> AdditionalArmorIDs = new List<int>() { ArmouredArmour.ArmouredArmourID };

            if (AdditionalArmorIDs.Contains(item.PickupObjectId)) price++;
            price = Mathf.Min(price, 5);
            return price;
        }
        public static int IronsideCustomRemoveCurrency(CustomShopController shop, PlayerController player, int cost)
        {
            if (player.characterIdentity != OMITBChars.Shade) player.healthHaver.Armor -= cost;
            else
            {
                FieldInfo _itemControllers = typeof(CustomShopController).GetField("m_itemControllers", BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (CustomShopItemController item in _itemControllers.GetValue(shop) as List<ShopItemController>)
                {
                    item.ForceOutOfStock();
                }
            }
            return 1;
        }
        public static bool IronsideCustomCanBuy(CustomShopController shop, PlayerController player, int cost)
        {
            if (player.characterIdentity == OMITBChars.Shade) return true;
            if (player.healthHaver.Armor >= cost && player.healthHaver.GetCurrentHealth() > 0)
            {
                return true;
            }
            else if (player.healthHaver.Armor > cost)
            {
                return true;
            }

            return false;
        }
    }
}

