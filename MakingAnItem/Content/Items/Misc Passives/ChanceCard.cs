using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;
using SaveAPI;
using Dungeonator;
using Alexandria.DungeonAPI;
using Steamworks;

namespace NevernamedsItems
{
    class ChanceCard : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<ChanceCard>(
            "Chance Card",
            "Third Chances",
            "A special card which guarantees access to Chancelots gambling parlour.\n\nThough Chancelot was expelled from the Knights of the Octagonal Table many years ago, all his business cards still falsely claim his knighthood.",
            "chancecard_icon");
            item.quality = PickupObject.ItemQuality.C;
            ID = item.PickupObjectId;

            item.SetupUnlockOnCustomStat(CustomTrackedStats.CHANCELOT_MONEY_SPENT, 499, DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN);
        }
        public static int ID;

        public static void InitRooms()
        {
            SharedInjectionData injector = ScriptableObject.CreateInstance<SharedInjectionData>();
            injector.UseInvalidWeightAsNoInjection = true;
            injector.PreventInjectionOfFailedPrerequisites = false;
            injector.IsNPCCell = false;
            injector.IgnoreUnmetPrerequisiteEntries = false;
            injector.OnlyOne = false;
            injector.ChanceToSpawnOne = 1f;
            injector.AttachedInjectionData = new List<SharedInjectionData>();
            injector.InjectionData = new List<ProceduralFlowModifierData>
            {
                new ProceduralFlowModifierData()
                {
                    annotation = "ChancelotMiniShop",
                    DEBUG_FORCE_SPAWN = false,
                    OncePerRun = false,
                    placementRules = new List<ProceduralFlowModifierData.FlowModifierPlacementType>()
                    {
                        ProceduralFlowModifierData.FlowModifierPlacementType.END_OF_CHAIN
                    },
                    roomTable = null,
                    exactRoom = RoomFactory.BuildNewRoomFromResource("NevernamedsItems/Content/NPCs/Rooms/MiniChancelotShop.newroom").room,
                    IsWarpWing = false,
                    RequiresMasteryToken = false,
                    chanceToLock = 0,
                    selectionWeight = 2,
                    chanceToSpawn = 1,
                    RequiredValidPlaceable = null,
                    prerequisites = new DungeonPrerequisite[]
                    {
                        new AdvancedDungeonPrerequisite
                        {
                            advancedAdvancedPrerequisiteType = AdvancedDungeonPrerequisite.AdvancedAdvancedPrerequisiteType.PASSIVE_ITEM_FLAG,
                            requiredPassiveFlag = typeof(ChanceCard),
                            requireTileset = false
                        }
                    },
                    CanBeForcedSecret = false,
                    RandomNodeChildMinDistanceFromEntrance = 0,
                    exactSecondaryRoom = null,
                    framedCombatNodes = 0,
                }

            };

            injector.name = "ChancelotMiniShops";
            SharedInjectionData BaseInjection = LoadHelper.LoadAssetFromAnywhere<SharedInjectionData>("Base Shared Injection Data");
            if (BaseInjection.AttachedInjectionData == null)
            {
                BaseInjection.AttachedInjectionData = new List<SharedInjectionData>();
            }
            BaseInjection.AttachedInjectionData.Add(injector);
        }

        public override void Pickup(PlayerController player)
        {
            ChanceCard.IncrementFlag(player, typeof(ChanceCard));
            base.Pickup(player);
        }

        public override void DisableEffect(PlayerController player)
        {
            if (player != null)
            {
                ChanceCard.DecrementFlag(player, typeof(ChanceCard));
            }
            base.DisableEffect(player);
        }
    }
}
