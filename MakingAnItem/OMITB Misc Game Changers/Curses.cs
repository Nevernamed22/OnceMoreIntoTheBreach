using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using System.Collections;
using System.Reflection;
using MonoMod.RuntimeDetour;

namespace NevernamedsItems
{
    public class CurseManager
    {
        public static void Init()
        {
            CurrentActiveCurses = new List<CurseData>();
            CursePrefabs = new List<CurseData>();

            VFXScapegoat = new GameObject("CurseVFXScapegoat");
            VFXScapegoat.gameObject.SetActive(false);
            UnityEngine.Object.DontDestroyOnLoad(VFXScapegoat);
            CurseIconCollection = SpriteBuilder.ConstructCollection(VFXScapegoat, "CurseIcon_Collection");
            UnityEngine.Object.DontDestroyOnLoad(CurseIconCollection);

            floorLoadPlayerHook = new Hook(
                typeof(PlayerController).GetMethod("BraveOnLevelWasLoaded", BindingFlags.Instance | BindingFlags.Public),
                typeof(CurseManager).GetMethod("OnNewLevel", BindingFlags.Static | BindingFlags.Public)
            );
            ETGModConsole.Log("Hook Passed");
            #region SetUpCurses
            CurseData curseOfInfestation = new CurseData();
            curseOfInfestation.curseName = "Curse of Infestation";
            curseOfInfestation.curseSubtitle = "They crawl beneath the surface";
            curseOfInfestation.curseIconId = SpriteBuilder.AddSpriteToCollection("NevernamedsItems/Resources/CurseIcons/infestation_icon", CurseIconCollection);
            CursePrefabs.Add(curseOfInfestation);
            #endregion
        }
        public static Hook floorLoadPlayerHook;
        private static tk2dSpriteCollectionData CurseIconCollection;
        private static GameObject VFXScapegoat;
        public static void OnNewLevel(Action<PlayerController> orig, PlayerController self)
        {
            if (!(GameManager.Instance.PrimaryPlayer && GameManager.Instance.SecondaryPlayer && GameManager.Instance.SecondaryPlayer == self))
            {
                CurrentActiveCurses.Clear();
                if (PostNewLevelCurseProcessing != null) CurseManager.PostNewLevelCurseProcessing();
                GameManager.Instance.StartCoroutine(DoCursePopups());
            }
            orig(self);
        }

        private static IEnumerator DoCursePopups()
        {
            yield return new WaitForSeconds(1);
            if (CurrentActiveCurses.Count > 0)
            {
                foreach (CurseData curse in CurrentActiveCurses)
                {
                    DoSpecificCursePopup(curse.curseName,curse.curseSubtitle, curse.curseIconId);
                }
            }
            yield break;
        }
        private static void DoSpecificCursePopup(string cursename, string curseSubtext, int id)
        {
            GameUIRoot.Instance.notificationController.DoCustomNotification(
                   cursename,
                    curseSubtext,
                    CurseIconCollection,
                    id,
                    UINotificationController.NotificationColor.PURPLE,
                    true,
                    false
                    );
        }
        public static void AddCurse(string CurseName)
        {
            if (CurseManager.CurseIsActive(CurseName)) return;
            CurseData newCurse = null;
            foreach (CurseData data in CursePrefabs)
            {
                if (data.curseName == CurseName) newCurse = data;
            }
            CurrentActiveCurses.Add(newCurse);
            DoSpecificCursePopup(CurseName,newCurse.curseSubtitle, newCurse.curseIconId);
        }

        public static event Action PostNewLevelCurseProcessing;
        public static List<CurseData> CurrentActiveCurses;
        public static List<CurseData> CursePrefabs;
        public static bool CurseIsActive(string CurseName)
        {
            foreach (CurseData data in CurrentActiveCurses)
            {
                if (data.curseName == CurseName) return true;
            }
            return false;
        }
        public class CurseData
        {
            public string curseName = null;
            public string curseSubtitle = null;
            public int curseIconId = -1;
        }
    }
}
