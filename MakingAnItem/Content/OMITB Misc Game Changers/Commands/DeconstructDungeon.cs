using Dungeonator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NevernamedsItems
{
    class DeconstructDungeon
    {
        protected static AutocompletionSettings deconstructAutocomplete = new AutocompletionSettings(delegate (string input)
        {
            List<string> list = new List<string>() { "base_jungle", "base_cathedral", "base_sewer", "base_nakatomi", "base_castle", "base_catacombs", "base_forge", "base_foyer", "base_gungeon", "base_mines", "base_nakatomi", "base_resourcefulrat", "base_tutorial" };
            return list.ToArray();
        });
        public static void Init()
        {
            ETGModConsole.Commands.GetGroup("nn").AddUnit("deconstructdungeon", delegate (string[] args)
            {
                string floorToCheck = "UNDEFINED";
                if (args != null && args.Length > 0 && args[0] != null) { if (!string.IsNullOrEmpty(args[0])) { floorToCheck = args[0]; } }
                if (GameManager.Instance == null) { ETGModConsole.Log("Somehow the fucking game manager was null lol rip get fucked mate."); return; }


                ETGModConsole.Log("<color=#09b022>-------------------------------------</color>");
                ETGModConsole.Log($"<color=#09b022>Checking for Dungeon:</color> {floorToCheck}");

                bool hasAlreadyLoggedRooms = false;
                Dungeon keepDungeon = DungeonDatabase.GetOrLoadByName(floorToCheck);
                if (keepDungeon == null) ETGModConsole.Log("<color=#ff0000ff>COULD NOT FIND DUNGEON</color>");
                else
                {
                    ETGModConsole.Log("<color=#4dfffc>Pattern Settings:</color>");
                    if (keepDungeon && keepDungeon.PatternSettings != null)
                    {
                        ETGModConsole.Log("<color=#4dfffc>  Flows:</color>");
                        if (keepDungeon.PatternSettings.flows != null && keepDungeon.PatternSettings.flows.Count > 0)
                        {
                            int flowNum = 0;
                            foreach (DungeonFlow flow in keepDungeon.PatternSettings.flows)
                            {
                                ETGModConsole.Log($"<color=#4dfffc>  Flow {flowNum} ({flow.name}):</color>");

                                ETGModConsole.Log($"<color=#4dfffc>    Injections:</color>");
                                if (flow.flowInjectionData != null && flow.flowInjectionData.Count > 0)
                                {
                                    int injnum = 0;
                                    foreach (ProceduralFlowModifierData inj in flow.flowInjectionData)
                                    {
                                        ETGModConsole.Log($"<color=#4dfffc>     Injection {injnum}:</color>");
                                        DeconstructThing(inj);
                                    injnum++;
                                    }
                                }
                                else ETGModConsole.Log("<color=#ff0000ff>    Injections table was null or empty.</color>");


                                ETGModConsole.Log($"<color=#4dfffc>    Shared Injections:</color>");
                                if (flow.sharedInjectionData != null && flow.sharedInjectionData.Count > 0)
                                {
                                    int injnum = 0;
                                    foreach (SharedInjectionData inj in flow.sharedInjectionData)
                                    {
                                        ETGModConsole.Log($"<color=#4dfffc>     Shared Inj {injnum}:</color>");

                                        if (inj.InjectionData != null && inj.InjectionData.Count > 0)
                                        {
                                            int injnum2 = 0;
                                            foreach (ProceduralFlowModifierData inj2 in inj.InjectionData)
                                            {
                                                ETGModConsole.Log($"<color=#4dfffc>         Injection {injnum2}:</color>");
                                                DeconstructThing(inj2, "         ");
                                            injnum2++;
                                            }
                                        }
                                        else ETGModConsole.Log("<color=#ff0000ff>    Injections table was null or empty.</color>");
                                    injnum++;
                                    }
                                }
                                else ETGModConsole.Log("<color=#ff0000ff>    Shared Injections table was null or empty.</color>");


                                ETGModConsole.Log($"<color=#4dfffc>     Rooms:</color>");
                                if (flow.fallbackRoomTable != null)
                                {
                                    if (flow.fallbackRoomTable.includedRooms != null)
                                    {
                                        if (flow.fallbackRoomTable.includedRooms.elements != null)
                                        {
                                            if (hasAlreadyLoggedRooms)
                                            {
                                                ETGModConsole.Log($"     Number of Rooms: {flow.fallbackRoomTable.includedRooms.elements.Count}");
                                            }
                                            else
                                            {
                                                hasAlreadyLoggedRooms = true;
                                                foreach (WeightedRoom wRoom in flow.fallbackRoomTable.includedRooms.elements)
                                                {
                                                    if (wRoom.room != null && !string.IsNullOrEmpty(wRoom.room.name))
                                                    {
                                                        ETGModConsole.Log("     " + wRoom.room.name);
                                                    }
                                                }
                                            }
                                        }
                                        else ETGModConsole.Log("<color=#ff0000ff>       Fallback room table had no elements.</color>");
                                    }
                                    else ETGModConsole.Log("<color=#ff0000ff>       Fallback room table had no room collection.</color>");
                                }
                                else ETGModConsole.Log("<color=#ff0000ff>       Fallback room table was null.</color>");


                                flowNum++;
                            }
                        }
                        else ETGModConsole.Log("<color=#ff0000ff>        Flows were null or empty</color>");
                    }
                    else ETGModConsole.Log("<color=#ff0000ff>    Pattern settings null</color>");
                }
                keepDungeon = null;
            }, deconstructAutocomplete);




        }
        public static void DeconstructThing(ProceduralFlowModifierData flowmod, string gap = "     ")
        {

            ETGModConsole.Log($"{gap}Annotation: {flowmod.annotation}");
            ETGModConsole.Log($"{gap}OncePerRun: {flowmod.OncePerRun}");
            ETGModConsole.Log($"{gap}IsWarpWing: {flowmod.IsWarpWing}");
            ETGModConsole.Log($"{gap}RequiresMasteryToken: {flowmod.RequiresMasteryToken}");
            ETGModConsole.Log($"{gap}ChanceToLock: {flowmod.chanceToLock}");
            ETGModConsole.Log($"{gap}SelectionWeight: {flowmod.selectionWeight}");
            ETGModConsole.Log($"{gap}ChanceToSpawn: {flowmod.chanceToSpawn}");
            ETGModConsole.Log($"{gap}Placement Rules:");
            if (flowmod.placementRules != null && flowmod.placementRules.Count > 0)
            {
                foreach (ProceduralFlowModifierData.FlowModifierPlacementType typ in flowmod.placementRules) { ETGModConsole.Log($"{gap}    {typ}"); }
            }
            else { ETGModConsole.Log($"{gap}    None"); }
            if (flowmod.RequiredValidPlaceable)
            {
                ETGModConsole.Log($"{gap}RequiredValidPlaceable: {flowmod.RequiredValidPlaceable.name}");
            }
            else { ETGModConsole.Log($"{gap}RequiredValidPlaceable: None"); }

            ETGModConsole.Log($"<color=#4dfffc>{gap}Exact Room:</color>");
            if (flowmod.exactRoom)
            {
                ETGModConsole.Log($"{gap}   {flowmod.exactRoom.name}");
            }
            else { ETGModConsole.Log($"{gap}    None"); }
            ETGModConsole.Log($"<color=#4dfffc>{gap}Secondary Exact Room:</color>");
            if (flowmod.exactSecondaryRoom)
            {
                ETGModConsole.Log($"{gap}   {flowmod.exactSecondaryRoom.name}");
            }
            else { ETGModConsole.Log($"{gap}    None"); }

            ETGModConsole.Log($"<color=#4dfffc>{gap}Room Table:</color>");
            if (flowmod.roomTable != null && flowmod.roomTable.includedRooms != null && flowmod.roomTable.includedRooms.elements != null && flowmod.roomTable.includedRooms.elements.Count > 0)
            {
                foreach (WeightedRoom wRoom in flowmod.roomTable.includedRooms.elements)
                {
                    if (wRoom.room != null && !string.IsNullOrEmpty(wRoom.room.name))
                    {
                        ETGModConsole.Log($"{gap}   {wRoom.room.name}");
                    }
                }

            }
            else { ETGModConsole.Log($"{gap}    None"); }
        }
    }
}
