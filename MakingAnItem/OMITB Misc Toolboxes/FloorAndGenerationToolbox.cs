using Dungeonator;
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
            GameManager.Instance.OnNewLevelFullyLoaded += OnNewFloor;

            ratMazeFailHook = new Hook(
                typeof(ResourcefulRatMazeSystemController).GetMethod("HandleFailure", BindingFlags.Instance | BindingFlags.NonPublic),
                typeof(FloorAndGenerationToolbox).GetMethod("OnFailedRatMaze", BindingFlags.Static | BindingFlags.Public)
            );
        }
        public static Hook ratMazeFailHook;
        public static void OnNewFloor()
        {
            CurrentFloorEnemyPalette = GeneratePalette();
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
                        List<PrototypeRoomObjectLayer> hiddenlist;

                        hiddenlist = OMITBReflectionHelpers.ReflectGetField<List<PrototypeRoomObjectLayer>>(typeof(RoomHandler), "remainingReinforcementLayers", room);
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
}
