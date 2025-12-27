using Dungeonator;
using HarmonyLib;
using MonoMod.RuntimeDetour;
using SaveAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    
    [HarmonyPatch(typeof(ResourcefulRatMazeSystemController), nameof(ResourcefulRatMazeSystemController.HandleFailure))]
    public class RatMazeFailurePatch
    {
        [HarmonyPostfix]
        public static void MazeFailure(ResourcefulRatMazeSystemController __instance, PlayerController cp)
        {
            if (!SaveAPIManager.GetFlag(CustomDungeonFlags.FAILEDRATMAZE)) { SaveAPIManager.SetFlag(CustomDungeonFlags.FAILEDRATMAZE, true); }
            if (FloorGenTools.OnFailedRatMaze != null) { FloorGenTools.OnFailedRatMaze(__instance, cp); }
        }
    }

    [HarmonyPatch(typeof(GameStatsManager), nameof(GameStatsManager.BeginNewSession))]
    public class NewSessionPatch
    {
        [HarmonyPostfix]
        public static void NewSession(GameStatsManager __instance, PlayerController player)
        {
            if (FloorGenTools.OnNewSession != null) { FloorGenTools.OnNewSession(player); }
        }
    }

    [HarmonyPatch(typeof(Dungeon), "Regenerate", MethodType.Normal)]
    public class RegenerateDungeonPatch
    {
        [HarmonyPostfix]
        public static IEnumerator LateRegenerateDungeon(IEnumerator enumerator, Dungeon __instance, bool cleanup)
        {
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
            if (FloorGenTools.OnLateRegenerateDungeon != null) { FloorGenTools.OnLateRegenerateDungeon(__instance, cleanup); }
            if (StaticReferenceManagerOMITB.LowWallDict != null && StaticReferenceManagerOMITB.LowWallDict.Keys != null && StaticReferenceManagerOMITB.LowWallDict.Keys.Count() > 0)
            {
                foreach (IntVector2 vec in StaticReferenceManagerOMITB.LowWallDict.Keys)
                {
                    StaticReferenceManagerOMITB.LowWallDict[vec].ConfigureOnLevelLoad();
                }
            }
            yield break;
        }
    }
    public static class FloorGenTools
    {
        public static Action OnDungeonLoadingStart;
        public static Action OnDungeonLoadingEnd;
        public static Action<Dungeon, bool> OnLateRegenerateDungeon;
        public static Action<PlayerController> OnNewSession;
        public static Action<ResourcefulRatMazeSystemController, PlayerController> OnFailedRatMaze;
        public static void Init()
        {
            GameManager.Instance.OnNewLevelFullyLoaded += OnNewFloorFullyLoaded;
        }
        public static void OnNewFloorFullyLoaded()
        {
            Challenges.OnLevelLoaded();
            GUIDs.RegenerateCurrentFloorEnemyPalette(false, false, false);
        }
    }

    public class AdvancedDungeonPrerequisite : CustomDungeonPrerequisite
    {
        public override bool CheckConditionsFulfilled()
        {
            if (advancedAdvancedPrerequisiteType != AdvancedAdvancedPrerequisiteType.NONE)
            {
                if (advancedAdvancedPrerequisiteType == AdvancedAdvancedPrerequisiteType.MULTIPLE_FLOORS)
                {
                    var h = GlobalDungeonData.ValidTilesets.GUNGEON;
                    if (GameManager.Instance.BestGenerationDungeonPrefab != null)
                    {
                        h = GameManager.Instance.BestGenerationDungeonPrefab.tileIndices.tilesetId;
                    }
                    else
                    {
                        return false;
                    }
                    return validTilesets.Contains(h);
                }
                if (advancedAdvancedPrerequisiteType == AdvancedAdvancedPrerequisiteType.PASSIVE_ITEM_FLAG)
                {
                    if (PassiveItem.IsFlagSetAtAll(requiredPassiveFlag) == true)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                if (advancedAdvancedPrerequisiteType == AdvancedAdvancedPrerequisiteType.SPEEDRUN_TIMER_BEFORE)
                {
                    if (GameStatsManager.Instance.GetSessionStatValue(TrackedStats.TIME_PLAYED) <= BeforeTimeInSeconds)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                if (advancedAdvancedPrerequisiteType == AdvancedAdvancedPrerequisiteType.UNLOCK)
                {
                    if (SaveAPIManager.GetFlag(UnlockFlag) == UnlockRequirement)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                if (advancedAdvancedPrerequisiteType == AdvancedAdvancedPrerequisiteType.SPEEDRUN_TIMER_AFTER)
                {
                    if (GameStatsManager.Instance.GetSessionStatValue(TrackedStats.TIME_PLAYED) >= AfterTimeInSeconds)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }


                }
            }

            else
            {
                return base.CheckConditionsFulfilled();
            }
            return false;
        }

        public float BeforeTimeInSeconds;
        public float AfterTimeInSeconds;
        public bool UnlockRequirement;
        public CustomDungeonFlags UnlockFlag;

        public AdvancedAdvancedPrerequisiteType advancedAdvancedPrerequisiteType;


        public List<GlobalDungeonData.ValidTilesets> validTilesets = new List<GlobalDungeonData.ValidTilesets>();


        public enum AdvancedAdvancedPrerequisiteType
        {
            NONE,
            PASSIVE_ITEM_FLAG,
            SPEEDRUN_TIMER_BEFORE,
            SPEEDRUN_TIMER_AFTER,
            UNLOCK,
            MULTIPLE_FLOORS
        }
    }

}
