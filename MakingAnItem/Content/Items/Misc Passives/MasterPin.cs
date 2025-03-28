using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SaveAPI;

using Alexandria.ItemAPI;
using Alexandria.Misc;
using UnityEngine;
using HarmonyLib;
using System.Collections;

namespace NevernamedsItems
{
    public class MasterPin : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<MasterPin>(
             "Master Pin",
             "Open Sesame",
             "Opens most doors. The mark of a skilled 'reverse-escape-artist', otherwise known as an 'intrusionist'.\n\nFor these maestros of the craft, the infinite locked doors and hidden passageways of the Gungeon represent the ultimate challenge.",
             "masterpin_icon");

            item.quality = PickupObject.ItemQuality.B;
            item.additionalMagnificenceModifier = 1;
            ID = item.PickupObjectId;
        }
        public static int ID;

        public override void Pickup(PlayerController player)
        {
            StartCoroutine(UnlockAllDoors());
            GameManager.Instance.OnNewLevelFullyLoaded += OnPostLevelLoad;
            base.Pickup(player);
        }
        public override void DisableEffect(PlayerController player)
        {
            GameManager.Instance.OnNewLevelFullyLoaded -= OnPostLevelLoad;
            base.DisableEffect(player);
        }
        private void OnPostLevelLoad()
        {
            StartCoroutine(UnlockAllDoors());
        }
        private IEnumerator UnlockAllDoors()
        {
            while (GameManager.Instance.IsLoadingLevel)
            {
                yield return null;
            }
            List<InteractableDoorController> gates = FindObjectsOfType<InteractableDoorController>().ToList();
            foreach (InteractableDoorController gate in gates)
            {
                if (gate && !gate.m_hasOpened)
                {
                    for (int i = 0; i < gate.WorldLocks.Count; i++)
                    {
                        if (gate.WorldLocks[i] && gate.WorldLocks[i].IsLocked && gate.WorldLocks[i].lockMode != InteractableLock.InteractableLockMode.NPC_JAIL)
                        {
                            gate.WorldLocks[i].ForceUnlock();
                        }
                    }
                }
            }
            yield return null;
            List<DungeonDoorController> doors = FindObjectsOfType<DungeonDoorController>().ToList();
            foreach (DungeonDoorController door in doors)
            {
                if (door && !door.m_open)
                {
                    if (door.isLocked) { door.Unlock(); }
                    if (door.OneWayDoor) { door.DoUnseal(door.downstreamRoom); }
                }
            }
            yield return null;

            bool hasVisitedOubliette = GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.TIMES_REACHED_SEWERS) > 0;

            List<SecretRoomManager> secretRooms = FindObjectsOfType<SecretRoomManager>().ToList();
            foreach (SecretRoomManager secretRoom in secretRooms)
            {
                if (secretRoom && !secretRoom.m_isOpen)
                {
                    if (secretRoom.revealStyle != SecretRoomManager.SecretRoomRevealStyle.FireplacePuzzle || hasVisitedOubliette)
                    {
                        secretRoom.OpenDoor();
                    }
                }
            }
            yield return null;

            GlobalDungeonData.ValidTilesets tileset = GameManager.Instance.Dungeon.tileIndices.tilesetId;

            if (tileset == GlobalDungeonData.ValidTilesets.CASTLEGEON)
            {
                List<SecretFloorInteractableController> secretTrapdoors = FindObjectsOfType<SecretFloorInteractableController>().ToList();
                foreach (SecretFloorInteractableController secretTrapdoor in secretTrapdoors)
                {
                    if (secretTrapdoor && !secretTrapdoor.GoesToRatFloor && !secretTrapdoor.m_hasOpened)
                    {
                        for (int i = 0; i < secretTrapdoor.WorldLocks.Count; i++)
                        {
                            if (secretTrapdoor.WorldLocks[i] && secretTrapdoor.WorldLocks[i].IsLocked)
                            {
                                secretTrapdoor.WorldLocks[i].ForceUnlock();
                            }
                        }
                    }
                }
            }
            yield return null;

            if (tileset == GlobalDungeonData.ValidTilesets.GUNGEON)
            {
                bool hasVisitedAbbey = GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.TIMES_REACHED_CATHEDRAL) > 0;
                List<CrestDoorController> abbeyDoors = FindObjectsOfType<CrestDoorController>().ToList();
                foreach (CrestDoorController abbeyDoor in abbeyDoors)
                {
                    if (abbeyDoor && !abbeyDoor.m_isOpen && hasVisitedAbbey)
                    {
                        abbeyDoor.SarcoRigidbody.gameObject.transform.position += new Vector3(0, -2, 0);
                        abbeyDoor.SarcoRigidbody.Reinitialize();
                        abbeyDoor.m_isOpen = true;
                    }
                }
            }
            yield return null;

            if (tileset == GlobalDungeonData.ValidTilesets.MINEGEON)
            {
                bool hasVisitedRat = GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.TIMES_REACHED_RATGEON) > 0;
                foreach (ResourcefulRatMinesHiddenTrapdoor trapdoor in StaticReferenceManager.AllRatTrapdoors)
                {
                    if (trapdoor && !trapdoor.m_hasCreatedRoom)
                    {
                        trapdoor.RevealPercentage = 1f;
                        trapdoor.UpdatePlayerDustups();
                        trapdoor.BlendMaterial.SetFloat("_BlendMin", trapdoor.RevealPercentage);
                        trapdoor.LockBlendMaterial.SetFloat("_BlendMin", trapdoor.RevealPercentage);
                        trapdoor.Lock.ForceUnlock();
                    }
                }
            }
            yield return null;
            yield break;
        }
        
    }
}
