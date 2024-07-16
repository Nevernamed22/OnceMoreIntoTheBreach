
using GungeonAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Gungeon;
using Alexandria.ItemAPI;
using Alexandria.ChestAPI;
using Dungeonator;
using System.Reflection;
using MonoMod.RuntimeDetour;
using Alexandria.Misc;

namespace NevernamedsItems
{
    public class KliklokShrine : GenericShrine
    {
        public static GameObject Setup(GameObject pedestal)
        {
            var shrineobj = ItemBuilder.SpriteFromBundle("shrine_kliklok", Initialisation.NPCCollection.GetSpriteIdByName("shrine_kliklok"), Initialisation.NPCCollection, new GameObject("Shrine Kliklok Statue"));
            shrineobj.GetComponent<tk2dSprite>().HeightOffGround = 1.25f;
            shrineobj.GetComponent<tk2dSprite>().renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTiltedCutout");
            shrineobj.GetComponent<tk2dSprite>().usesOverrideMaterial = true;
            pedestal.AddComponent<KliklokShrine>();
            GameObject talkpoint = new GameObject("talkpoint");
            talkpoint.transform.SetParent(pedestal.transform);
            talkpoint.transform.localPosition = new Vector3(1f, 36f / 16f, 0f);
            return shrineobj;
        }
        public bool shadeOneOff = false;
        public override bool CanAccept(PlayerController interactor)
        {
            if (GetAllChests().Count == 0) { return false; }
            if (interactor.characterIdentity == OMITBChars.Shade) { return !shadeOneOff; }
            else if (interactor.ForceZeroHealthState && interactor.healthHaver.Armor > 2) { return true; }
            else if (interactor.healthHaver.GetMaxHealth() > 1) { return true; }
            return false;
        }
        public override void OnAccept(PlayerController Interactor)
        {
            if (Interactor.characterIdentity == OMITBChars.Shade) { shadeOneOff = true; }
            else if (Interactor.ForceZeroHealthState) { Interactor.healthHaver.Armor -= 2; }
            else
            {
                StatModifier HP = new StatModifier
                {
                    statToBoost = PlayerStats.StatType.Health,
                    amount = -1f,
                    modifyType = StatModifier.ModifyMethod.ADDITIVE
                };
                Interactor.ownerlessStatModifiers.Add(HP);
                Interactor.stats.RecalculateStats(Interactor);
            }
            foreach (Chest chest in GetAllChests())
            {
                ChestUtility.ChestTier targetTier = ChestUtility.ChestTier.OTHER;
                ChestUtility.ChestTier curTier = chest.GetChestTier();
                switch (curTier)
                {
                    case ChestUtility.ChestTier.BROWN:
                        targetTier = ChestUtility.ChestTier.BLUE;
                        break;
                    case ChestUtility.ChestTier.BLUE:
                        targetTier = ChestUtility.ChestTier.GREEN;
                        break;
                    case ChestUtility.ChestTier.GREEN:
                        if (UnityEngine.Random.value <= 0.75f) targetTier = ChestUtility.ChestTier.RED;
                        else targetTier = ChestUtility.ChestTier.SYNERGY;
                        break;
                    case ChestUtility.ChestTier.RED:
                        targetTier = ChestUtility.ChestTier.BLACK;
                        break;
                    case ChestUtility.ChestTier.BLACK:
                        if (UnityEngine.Random.value <= 0.05f) targetTier = ChestUtility.ChestTier.RAINBOW;
                        else chest.ForceUnlock();
                        break;
                    case ChestUtility.ChestTier.SYNERGY:
                        if (UnityEngine.Random.value <= 0.5f) targetTier = ChestUtility.ChestTier.RED;
                        else targetTier = ChestUtility.ChestTier.BLACK;
                        break;
                }

                ThreeStateValue isMimic = ThreeStateValue.UNSPECIFIED;
                if (chest.IsMimic) isMimic = ThreeStateValue.FORCEYES;
                else isMimic = ThreeStateValue.FORCENO;

                if (targetTier != ChestUtility.ChestTier.OTHER)
                {
                    Chest newChest = ChestUtility.SpawnChestEasy(chest.sprite.WorldBottomLeft.ToIntVector2(), targetTier, chest.IsLocked, chest.ChestType, isMimic, ThreeStateValue.FORCENO);
                    if (chest.GetComponent<JammedChestBehav>()) newChest.gameObject.AddComponent<JammedChestBehav>();

                    chest.m_room.DeregisterInteractable(chest);
                    chest.DeregisterChestOnMinimap();
                    UnityEngine.Object.Destroy(chest.gameObject);
                }       
            }

            GameUIRoot.Instance.notificationController.DoCustomNotification(
                       "Kliklok's Blessing",
                        "Chests Upgraded",
                        Initialisation.NPCCollection,
                        Initialisation.NPCCollection.GetSpriteIdByName("kliklok_icon"),
                        UINotificationController.NotificationColor.SILVER,
                        true,
                        false
                        );
            AkSoundEngine.PostEvent("Play_OBJ_shrine_accept_01", base.gameObject);
        }
        public override string AcceptText(PlayerController interactor)
        {
            if (interactor.characterIdentity == OMITBChars.Shade) { return "Pray <Lose Nothing>"; }
            if (interactor.ForceZeroHealthState) { return $"Pray <Lose 2 [sprite \"armor_money_icon_001\"]>"; }
            return $"Pray <Lose 1 [sprite \"heart_big_idle_001\"] Container>";
        }
        public override string DeclineText(PlayerController Interactor)
        {
            return "Leave";
        }
        public override string PanelText(PlayerController Interactor)
        {
            return !shadeOneOff ? "A shrine to Kliklok, patron god of chests. Giving a blood sacrifice to his effigy may bolster his disciples." : "The spirits inhabiting this shrine have departed...";
        }  
        public static List<Chest> GetAllChests()
        {
            List<Chest> Validchests = new List<Chest>();
            foreach (Chest chest in StaticReferenceManager.AllChests)
            {
                if (chest && !chest.IsBroken && !chest.IsOpen && !chest.IsGlitched && !chest.IsLockBroken)
                {
                    List<ChestUtility.ChestTier> BannedTiers = new List<ChestUtility.ChestTier>()
                    {
                           ChestUtility.ChestTier.OTHER,
                           ChestUtility.ChestTier.GLITCHED,
                           ChestUtility.ChestTier.RAINBOW,
                           ChestUtility.ChestTier.RAT,
                           ChestUtility.ChestTier.SECRETRAINBOW,
                           ChestUtility.ChestTier.TRUTH,
                    };
                    if (!BannedTiers.Contains(chest.GetChestTier())) Validchests.Add(chest);
                }
            }
            return Validchests;
        }
    }
}

