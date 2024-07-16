using Dungeonator;
using Alexandria.DungeonAPI;
using HutongGames.PlayMaker;
using Alexandria.ItemAPI;
using Alexandria.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using HarmonyLib;

namespace NevernamedsItems
{
    public static class BreachModifications
    {
        public static List<GameObject> placedInBreach = new List<GameObject>();

        private static bool needsToRun;

        [HarmonyPatch(typeof(Foyer), nameof(Foyer.ProcessPlayerEnteredFoyer))]
        private class ProcessPlayerEnteredFoyerPatch
        {
            static void Postfix(Foyer __instance, PlayerController p)
            {
                if (!needsToRun)return;
                OnBreachStart();
                needsToRun = false;
            }
        }

        [HarmonyPatch(typeof(Foyer), nameof(Foyer.Start))]
        private class OnFoyerStartPatch
        {
            static void Postfix(Foyer __instance)
            {
                needsToRun = true;
            }
        }

        private static void CleanupBreachObjects()
        {
            foreach (BreachPlacedItem breachItem in UnityEngine.Object.FindObjectsOfType<BreachPlacedItem>())
            {
                if (!FakePrefab.IsFakePrefab(breachItem.gameObject)) { UnityEngine.Object.Destroy(breachItem.gameObject); }
                else { breachItem.gameObject.SetActive(false); }
            }
        }

        public static Dictionary<string, GameObject> registeredShops = new Dictionary<string, GameObject>();

        public static void OnBreachStart()
        {
            BreachModifications.CleanupBreachObjects();
            foreach (GameObject gameObject in placedInBreach)
            {
                try
                {
                    var placed = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                    var comp = gameObject.GetComponent<BreachPlacedItem>();
                    placed.SetActive(true);
                    placed.transform.position = comp.positionInBreach;
                }
                catch (Exception e)
                {
                    DebugUtility.Print<string>(e.ToString(), "FF0000", true);
                }
            }
        }
    } 
    public class BreachPlacedItem : BraveBehaviour
    {
        public Vector2 positionInBreach;
    }
}