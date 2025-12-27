using Dungeonator;
using SaveAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    class GlobalUpdate : MonoBehaviour
    {
        public void Update()
        {
            if (Dungeon.IsGenerating != DungeonWasGeneratingLastChecked)
            {
                if (GameManager.Instance)
                {
                    if (Dungeon.IsGenerating && !DungeonWasGeneratingLastChecked) //Player exited floor, transitioning to loading screen
                    {
                        if (FloorGenTools.OnDungeonLoadingStart != null) { FloorGenTools.OnDungeonLoadingStart(); }
                        if (CurseManager.CurrentActiveCurses.Count > 0)
                        {
                            if (!SaveAPIManager.GetFlag(CustomDungeonFlags.FLOOR_CLEARED_WITH_CURSE))
                            {
                                SaveAPIManager.SetFlag(CustomDungeonFlags.FLOOR_CLEARED_WITH_CURSE, true);
                            }
                        }
                        CurseManager.RemoveAllCurses();
                    }
                    if (!Dungeon.IsGenerating && DungeonWasGeneratingLastChecked) //Player entered floor, transitioning from loading screen
                    {
                        if (FloorGenTools.OnDungeonLoadingEnd != null) { FloorGenTools.OnDungeonLoadingEnd(); }
                    }
                }
                DungeonWasGeneratingLastChecked = Dungeon.IsGenerating;
            }
        }
        public static bool DungeonWasGeneratingLastChecked;
    }
}
