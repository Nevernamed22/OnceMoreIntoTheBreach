using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;
using SaveAPI;
using Dungeonator;
using Alexandria.DungeonAPI;

namespace NevernamedsItems
{
    class BeggarsBelief : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<BeggarsBelief>(
            "Beggars Belief",
            "Another Mans Treasure",
            "Guarantees an extra special bonus room every chamber."+"\n\nYou made an old bullet very happy. Good job.",
            "beggarsbelief_icon");
            item.quality = PickupObject.ItemQuality.B;
            ID = item.PickupObjectId;
            item.SetupUnlockOnCustomStat(CustomTrackedStats.BEGGAR_TOTAL_DONATIONS, 5114, DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN);
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
                    annotation = "BeggarsBeliefRoom",
                    DEBUG_FORCE_SPAWN = false,
                    OncePerRun = false,
                    placementRules = new List<ProceduralFlowModifierData.FlowModifierPlacementType>()
                    {
                        ProceduralFlowModifierData.FlowModifierPlacementType.END_OF_CHAIN
                    },
                    roomTable = null,
                    exactRoom = RoomFactory.BuildNewRoomFromResource("NevernamedsItems/Content/NPCs/Rooms/BeggarsBeliefRoom.newroom").room,
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
                            requiredPassiveFlag = typeof(BeggarsBelief),
                            requireTileset = false
                        }
                    },
                    CanBeForcedSecret = false,
                    RandomNodeChildMinDistanceFromEntrance = 0,
                    exactSecondaryRoom = null,
                    framedCombatNodes = 0,
                }

            };

            injector.name = "BeggarsBeliefRooms";
            SharedInjectionData BaseInjection = LoadHelper.LoadAssetFromAnywhere<SharedInjectionData>("Base Shared Injection Data");
            if (BaseInjection.AttachedInjectionData == null)
            {
                BaseInjection.AttachedInjectionData = new List<SharedInjectionData>();
            }
            BaseInjection.AttachedInjectionData.Add(injector);
        }

        public override void Pickup(PlayerController player)
        {
            BeggarsBelief.IncrementFlag(player, typeof(BeggarsBelief));
            base.Pickup(player);
        }

        public override void DisableEffect(PlayerController player)
        {
            BeggarsBelief.DecrementFlag(player, typeof(BeggarsBelief));
            base.DisableEffect(player);
        }
    }
}
