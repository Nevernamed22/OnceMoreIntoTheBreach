using Dungeonator;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using System.Linq;
using System.Text;

namespace NevernamedsItems
{
    [HarmonyPatch(typeof(GameManager), "ClearPerLevelData", MethodType.Normal)]
    public class GameManagerClearDataPatch
    {
        [HarmonyPostfix]
        public static void ClearPerLevelDataPatch(GameManager __instance)
        {
            StaticReferenceManagerOMITB.LowWallDict.Clear();
        }
    }
    public static class StaticReferenceManagerOMITB
    {
        public static Dictionary<IntVector2, LowWalls> LowWallDict = new Dictionary<IntVector2, LowWalls>();
    }
}
