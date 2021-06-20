﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Gungeon;

namespace NevernamedsItems
{
    static class OMITBPlayerExtensions
    {
        public static void RemoveItemFromInventory(this PlayerController player, PickupObject item)
        {
            if (item == null) return;
            if (item is PassiveItem)
            {
                if (player.passiveItems.Contains(item as PassiveItem))
                {
                    player.passiveItems.Remove(item as PassiveItem);
                    GameUIRoot.Instance.RemovePassiveItemFromDock(item as PassiveItem);
                    player.stats.RecalculateStats(player, false, false);
                }
            }
            else if (item is PlayerItem)
            {
                if (player.activeItems.Contains(item as PlayerItem))
                {
                    player.activeItems.Remove(item as PlayerItem);
                    player.stats.RecalculateStats(player, false, false);
                }
            }
            else if (item is Gun)
            {
                if (player.inventory.AllGuns.Contains(item as Gun))
                {
                    player.inventory.RemoveGunFromInventory(item as Gun);
                    player.stats.RecalculateStats(player, false, false);
                }
            }
        }
        public static int GetNumberOfItemInInventory(this PlayerController player, int itemID)
        {
            int foundVer = 0;
            foreach (PassiveItem item in player.passiveItems) { if (item.PickupObjectId == itemID) foundVer++; }
            foreach (PlayerItem item in player.activeItems) { if (item.PickupObjectId == itemID) foundVer++; }
            return foundVer;
        }
        public static bool NextHitWillKillPlayer(this PlayerController player, float damageAmount)
        {
            if (player.healthHaver)
            {
                if (player.healthHaver.NextShotKills == true) return true;
                if (player.characterIdentity != PlayableCharacters.Robot)
                {
                    if (player.healthHaver.GetCurrentHealth() > damageAmount) return false;
                    else if (player.healthHaver.Armor > 0 && !player.healthHaver.NextDamageIgnoresArmor) return false;
                    else return true;
                }
                else
                {
                    if (player.healthHaver.Armor > 1) return true;
                    else return false;
                }
            }
            else
            {
                Debug.LogError("PLAYER HAD NO HEALTHHAVER????");

                return false;
            }
        }
        public static void RemovePassiveItemAtIndex(this PlayerController player, int index)
        {
            PassiveItem passiveItem = player.passiveItems[index];
            GameUIRoot.Instance.RemovePassiveItemFromDock(passiveItem);
            player.passiveItems.RemoveAt(index);
            UnityEngine.Object.Destroy(passiveItem);
            player.stats.RecalculateStats(player, false, false);
        }
        public static void DoEasyBlank(this PlayerController blankOwner, Vector2 blankPosition, EasyBlankType type)
        {
            if (type == EasyBlankType.MINI)
            {
                GameObject silencerVFX = (GameObject)ResourceCache.Acquire("Global VFX/BlankVFX_Ghost");
                AkSoundEngine.PostEvent("Play_OBJ_silenceblank_small_01", blankOwner.gameObject);
                GameObject gameObject = new GameObject("silencer");
                SilencerInstance silencerInstance = gameObject.AddComponent<SilencerInstance>();
                float additionalTimeAtMaxRadius = 0.25f;
                silencerInstance.TriggerSilencer(blankPosition, 25f, 5f, silencerVFX, 0f, 3f, 3f, 3f, 250f, 5f, additionalTimeAtMaxRadius, blankOwner, false, false);
            }
            else if (type == EasyBlankType.FULL)
            {
                GameObject bigSilencerVFX = (GameObject)ResourceCache.Acquire("Global VFX/BlankVFX");
                AkSoundEngine.PostEvent("Play_OBJ_silenceblank_use_01", blankOwner.gameObject);
                GameObject gameObject = new GameObject("silencer");
                SilencerInstance silencerInstance = gameObject.AddComponent<SilencerInstance>();
                silencerInstance.TriggerSilencer(blankPosition, 50f, 25f, bigSilencerVFX, 0.15f, 0.2f, 50f, 10f, 140f, 15f, 0.5f, blankOwner, true, false);
            }
        }
        public static void Enrage(this PlayerController player, float duration)
        {
            PlayerToolbox effecter = player.GetComponent<PlayerToolbox>();
            effecter.Enrage(duration);
        }
        public static ModdedCharacterID ModdedCharacterIdentity(this PlayerController player)
        {
            if (player.name == "PlayerShade(Clone)") return ModdedCharacterID.Shade;
            if (player.name == "PlayerMistake(Clone)") return ModdedCharacterID.Mistake;
            if (player.name == "PlayerTinker(Clone)") return ModdedCharacterID.Tinker;
            else return ModdedCharacterID.NONE;
        }
        public static Vector2 GetCursorPosition(this PlayerController user, float noCursorControllerRange)
        {
            Vector2 m_cachedBlinkPosition = Vector2.zero;

            GungeonActions m_activeActions = OMITBReflectionHelpers.ReflectGetField<GungeonActions>(typeof(PlayerController), "m_activeActions", user);

            bool IsKeyboardAndMouse = BraveInput.GetInstanceForPlayer(user.PlayerIDX).IsKeyboardAndMouse(false);
            if (IsKeyboardAndMouse) { m_cachedBlinkPosition = user.unadjustedAimPoint.XY() - (user.CenterPosition - user.specRigidbody.UnitCenter); }
            else
            {
                m_cachedBlinkPosition = user.PositionInDistanceFromAimDir(5);
                //if (m_activeActions != null) { m_cachedBlinkPosition += m_activeActions.Aim.Vector.normalized * BraveTime.DeltaTime * 15f; }
            }

            m_cachedBlinkPosition = BraveMathCollege.ClampToBounds(m_cachedBlinkPosition, GameManager.Instance.MainCameraController.MinVisiblePoint, GameManager.Instance.MainCameraController.MaxVisiblePoint);
            return m_cachedBlinkPosition;
        }
        public static void GiveAmmoToGunNotInHand(this PlayerController player, int idToGive, int AmmoToGive)
        {
            foreach (Gun gun in player.inventory.AllGuns)
            {
                if (gun.PickupObjectId == idToGive) { gun.GainAmmo(AmmoToGive); }
            }
        }
        public static bool AnyPlayerHasPickupID(this GameManager managerInstance, int itemID)
        {
            bool hasBeenDetectedOnPlayer = false;
            if (managerInstance.PrimaryPlayer && managerInstance.PrimaryPlayer.HasPickupID(itemID)) hasBeenDetectedOnPlayer = true;
            if (managerInstance.SecondaryPlayer && managerInstance.SecondaryPlayer.HasPickupID(itemID)) hasBeenDetectedOnPlayer = true;
            return hasBeenDetectedOnPlayer;
        }
        public static Vector2 PositionInDistanceFromAimDir(this PlayerController player, float distance)
        {
            Vector2 vector = player.CenterPosition;
            Vector2 normalized = (player.unadjustedAimPoint.XY() - vector).normalized;
            Vector2 final = player.CenterPosition + normalized * distance;
            return final;
        }
    }
    public enum EasyBlankType
    {
        FULL,
        MINI,
    }
    public enum ModdedCharacterID
    {
        NONE,
        Shade,
        Mistake,
        Tinker,
        Investigator,
        Cyborg
    }

}