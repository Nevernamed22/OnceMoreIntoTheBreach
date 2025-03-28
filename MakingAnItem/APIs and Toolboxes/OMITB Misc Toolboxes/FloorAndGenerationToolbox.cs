﻿using Dungeonator;
using MonoMod.RuntimeDetour;
using SaveAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    class FloorAndGenerationToolbox
    {
        public static void Init()
        {

            ratMazeFailHook = new Hook(
                typeof(ResourcefulRatMazeSystemController).GetMethod("HandleFailure", BindingFlags.Instance | BindingFlags.NonPublic),
                typeof(FloorAndGenerationToolbox).GetMethod("OnFailedRatMaze", BindingFlags.Static | BindingFlags.Public)
            );
            floorLoadPlayerHook = new Hook(
                typeof(PlayerController).GetMethod("BraveOnLevelWasLoaded", BindingFlags.Instance | BindingFlags.Public),
                typeof(FloorAndGenerationToolbox).GetMethod("OnNewFloor", BindingFlags.Static | BindingFlags.Public)
            );
            floorDepartureHook = new Hook(
                typeof(ElevatorDepartureController).GetMethod("DoDeparture", BindingFlags.Instance | BindingFlags.Public),
                typeof(FloorAndGenerationToolbox).GetMethod("OnFloorDeparture", BindingFlags.Instance | BindingFlags.Public),
                typeof(ElevatorDepartureController)
            );
            NewSessionStarted = new Hook(
                typeof(GameStatsManager).GetMethod("BeginNewSession", BindingFlags.Instance | BindingFlags.Public),
                typeof(FloorAndGenerationToolbox).GetMethod("NewSession", BindingFlags.Static | BindingFlags.Public)
            );

        }
        public static Hook ratMazeFailHook;
        public static Hook floorLoadPlayerHook;
        public static Hook floorDepartureHook;
        public static Hook NewSessionStarted;
        public static Action OnFloorExited;
        public static Action OnFloorEntered;
        public static Action<PlayerController> OnNewGame;
        public static void NewSession(Action<GameStatsManager, PlayerController> orig, GameStatsManager self, PlayerController player)
        {
            orig(self, player);
            if (OnNewGame != null)
            {
                OnNewGame(player);
            }
        }
        public static void OnFloorLoaded()
        {
            if (OnFloorEntered != null)
            {
                OnFloorEntered();
            }
        }
        public static void OnFloorUnloaded(List<PlayerController> Players)
        {
            if (OnFloorExited != null)
            {
                OnFloorExited();
            }
            foreach (PlayerController player in Players)
            {

            }
            if (CurseManager.CurrentActiveCurses.Count > 0)
            {
                if (!SaveAPIManager.GetFlag(CustomDungeonFlags.FLOOR_CLEARED_WITH_CURSE))
                {
                    SaveAPIManager.SetFlag(CustomDungeonFlags.FLOOR_CLEARED_WITH_CURSE, true);
                }
            }
            CurseManager.RemoveAllCurses();
        }
        private static bool hookDoubleUpPrevention;
        public static void OnNewFloor(Action<PlayerController> orig, PlayerController self)
        {
            bool isSecondary = false;
            if (GameManager.Instance.SecondaryPlayer && GameManager.Instance.SecondaryPlayer == self) isSecondary = true;
            bool flag = (!isSecondary) || (isSecondary && !GameManager.Instance.PrimaryPlayer);
            if (flag)
            {
                if (hookDoubleUpPrevention)
                {
                    //ETGModConsole.Log("Level loaded hook ran");
                    CurrentFloorEnemyPalette = GeneratePalette();
                    Challenges.OnLevelLoaded();
                    hookDoubleUpPrevention = false;
                }
                else
                {
                    hookDoubleUpPrevention = true;
                }
            }
            orig(self);
        }
        public void OnFloorDeparture(Action<ElevatorDepartureController> orig, ElevatorDepartureController self)
        {
            orig(self);
        }
        public static List<string> CurrentFloorEnemyPalette;
        private static bool EnemyIsValid(string enemyGUID, bool canReturnMimic, bool canReturnBoss)
        {
            if (enemyGUID != null)
            {
                AIActor enemy = EnemyDatabase.GetOrLoadByGuid(enemyGUID);
                if (enemy)
                {
                    AIActor realEnemy = enemy;
                    if (enemy is AIActorDummy)
                    {
                        if ((enemy as AIActorDummy).realPrefab.GetComponent<AIActor>() != null)
                        {
                            realEnemy = (enemy as AIActorDummy).realPrefab.GetComponent<AIActor>();
                        }
                    }
                    if ((!canReturnMimic && !realEnemy.IsMimicEnemy) || canReturnMimic)
                    {
                        if ((!canReturnBoss && realEnemy.healthHaver && !realEnemy.healthHaver.IsBoss) || canReturnBoss)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public static List<string> GeneratePalette(bool canReturnMimics = false, bool canReturnBosses = false)
        {
            List<string> templist = new List<string>();
            Dungeon d = GameManager.Instance.Dungeon;
            if (d != null)
            {
                DungeonData ddata = d.data;
                foreach (AIActor enemy in StaticReferenceManager.AllEnemies)
                {
                    if (enemy.GetComponent<CompanionController>() == null)
                    {
                        if (EnemyIsValid(enemy.EnemyGuid, canReturnMimics, canReturnBosses))
                        {
                            if (!templist.Contains(enemy.EnemyGuid)) templist.Add(enemy.EnemyGuid);
                        }
                    }
                }
                if (ddata != null)
                {
                    foreach (RoomHandler room in ddata.rooms)
                    {
                        //Debug.Log("-------- Checking ROOM: " + room.GetRoomName());
                        List<PrototypeRoomObjectLayer> hiddenlist = room.remainingReinforcementLayers;

                        //Debug.Log("Wavecheck passed the Reflection");
                        if (hiddenlist != null && hiddenlist.Count > 0)
                        {
                            foreach (PrototypeRoomObjectLayer layer in hiddenlist)
                            {
                                if (layer != null)
                                {
                                    if (layer.placedObjects != null)
                                    {
                                        foreach (PrototypePlacedObjectData objData in layer.placedObjects)
                                        {
                                            if (objData != null)
                                            {
                                                if (objData.unspecifiedContents != null)
                                                {
                                                    if (objData.unspecifiedContents.GetComponent<AIActor>() != null)
                                                    {
                                                        if (EnemyIsValid(objData.unspecifiedContents.GetComponent<AIActor>().EnemyGuid, canReturnMimics, canReturnBosses))
                                                        {
                                                            if (!templist.Contains(objData.unspecifiedContents.GetComponent<AIActor>().EnemyGuid)) templist.Add(objData.unspecifiedContents.GetComponent<AIActor>().EnemyGuid);
                                                        }
                                                    }
                                                }
                                                else if (objData.placeableContents != null)
                                                {
                                                    foreach (DungeonPlaceableVariant variantTier in objData.placeableContents.variantTiers)
                                                    {
                                                        if (variantTier.enemyPlaceableGuid != null)
                                                        {
                                                            //Debug.Log("FOUND ENEMY GUID: " + variantTier.enemyPlaceableGuid);
                                                            if (!templist.Contains(variantTier.enemyPlaceableGuid)) templist.Add(variantTier.enemyPlaceableGuid);
                                                        }
                                                    }
                                                }
                                                else Debug.LogError("unspecifiedContents AND placeableContents are NULL!");
                                            }
                                            else Debug.LogError("Object data in the placed objects list is NULL!");
                                        }
                                    }
                                    else Debug.LogError("List of placed objects in the layer is NULL!");
                                }
                                else Debug.LogError("Individual object layer is NULL!");
                            }
                        }
                        else Debug.Log("There are no reinforcement waves in room: " + room.GetRoomName());
                    }
                }
            }
            return templist;
        }

        public static void OnFailedRatMaze(Action<ResourcefulRatMazeSystemController, PlayerController> orig, ResourcefulRatMazeSystemController self, PlayerController playa)
        {
            if (!SaveAPIManager.GetFlag(CustomDungeonFlags.FAILEDRATMAZE))
            {
                SaveAPIManager.SetFlag(CustomDungeonFlags.FAILEDRATMAZE, true);
            }
            orig(self, playa);

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
