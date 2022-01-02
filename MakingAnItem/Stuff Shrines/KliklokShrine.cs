
using GungeonAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static GungeonAPI.OldShrineFactory;
using Gungeon;
using ItemAPI;
using Dungeonator;
using System.Reflection;
using MonoMod.RuntimeDetour;


namespace NevernamedsItems
{
    public static class KliklokShrine
    {
        public static void Add()
        {
            OldShrineFactory aa = new OldShrineFactory
            {

                name = "KliklokShrine",
                modID = "omitb",
                text = "A shrine to Kliklok, patron god of chests. Giving a blood sacrifice to his effigy may bolster his disciples.",
                spritePath = "NevernamedsItems/Resources/Shrines/kliklok_shrine.png",
                room = RoomFactory.BuildFromResource("NevernamedsItems/Resources/EmbeddedRooms/KliklokRoom.room").room,
                RoomWeight = 1f,
                acceptText = "Pray <Lose HP>",
                declineText = "Leave",
                OnAccept = Accept,
                OnDecline = null,
                CanUse = CanUse,
                offset = new Vector3(-1.5f, -1, 0),
                talkPointOffset = new Vector3(0, 3, 0),
                isToggle = false,
                isBreachShrine = false,


            };
            aa.Build();
            spriteId = SpriteBuilder.AddSpriteToCollection(spriteDefinition, ShrineFactory.ShrineIconCollection);
        }
        public static string spriteDefinition = "NevernamedsItems/Resources/Shrines/kliklok_icon";
        public static bool CanUse(PlayerController player, GameObject shrine)
        {
            if (player.characterIdentity == PlayableCharacters.Robot)
            {
                if (player.healthHaver.Armor > 2)
                {
                    if (GetAllChests().Count > 0) return true;
                    else return false;
                }
                else return false;
            }
            else
            {
                if (player.healthHaver.GetMaxHealth() > 1)
                {
                    if (GetAllChests().Count > 0) return true;
                    else return false;
                }
                else return false;
            }
        }
        public static List<Chest> GetAllChests()
        {
            List<Chest> Validchests = new List<Chest>();
            foreach (Chest chest in StaticReferenceManager.AllChests)
            {
                if (chest && !chest.IsBroken && !chest.IsOpen && !chest.IsGlitched && !chest.IsLockBroken)
                {
                    List<ChestToolbox.ChestTier> BannedTiers = new List<ChestToolbox.ChestTier>()
                    {
                           ChestToolbox.ChestTier.OTHER,
                           ChestToolbox.ChestTier.GLITCHED,
                           ChestToolbox.ChestTier.RAINBOW,
                           ChestToolbox.ChestTier.RAT,
                           ChestToolbox.ChestTier.SECRETRAINBOW,
                           ChestToolbox.ChestTier.TRUTH,
                    };
                    if (!BannedTiers.Contains (chest.GetChestTier())) Validchests.Add(chest);
                }
            }
            return Validchests;
        }
        public static void Accept(PlayerController player, GameObject shrine)
        {
            if (player.characterIdentity == PlayableCharacters.Robot)
            {
                player.healthHaver.Armor -= 2;

            }
            else
            {
                StatModifier HP = new StatModifier
                {
                    statToBoost = PlayerStats.StatType.Health,
                    amount = -1f,
                    modifyType = StatModifier.ModifyMethod.ADDITIVE
                };
                player.ownerlessStatModifiers.Add(HP);
                player.stats.RecalculateStats(player);
            }
            foreach (Chest chest in GetAllChests())
            {
                ChestToolbox.ChestTier targetTier = ChestToolbox.ChestTier.OTHER;
                ChestToolbox.ChestTier curTier = chest.GetChestTier();
                switch (curTier)
                {
                    case ChestToolbox.ChestTier.BROWN:
                        targetTier = ChestToolbox.ChestTier.BLUE;
                        break;
                    case ChestToolbox.ChestTier.BLUE:
                        targetTier = ChestToolbox.ChestTier.GREEN;
                        break;
                    case ChestToolbox.ChestTier.GREEN:
                        if (UnityEngine.Random.value <= 0.75f) targetTier = ChestToolbox.ChestTier.RED;
                        else targetTier = ChestToolbox.ChestTier.SYNERGY;
                        break;
                    case ChestToolbox.ChestTier.RED:
                        targetTier = ChestToolbox.ChestTier.BLACK;
                        break;
                    case ChestToolbox.ChestTier.BLACK:
                        if (UnityEngine.Random.value <= 0.05f) targetTier = ChestToolbox.ChestTier.RAINBOW;
                        else chest.ForceUnlock();
                        break;
                    case ChestToolbox.ChestTier.SYNERGY:
                        if (UnityEngine.Random.value <= 0.5f) targetTier = ChestToolbox.ChestTier.RED;
                        else targetTier = ChestToolbox.ChestTier.BLACK;
                        break;
                }

                ChestToolbox.ThreeStateValue isMimic = ChestToolbox.ThreeStateValue.UNSPECIFIED;
                if (chest.IsMimic) isMimic = ChestToolbox.ThreeStateValue.FORCEYES;
                else isMimic = ChestToolbox.ThreeStateValue.FORCENO;

                if (targetTier != ChestToolbox.ChestTier.OTHER)
                {
                    Chest newChest = ChestToolbox.SpawnChestEasy(chest.sprite.WorldBottomLeft.ToIntVector2(), targetTier, chest.IsLocked, chest.ChestType, isMimic, ChestToolbox.ThreeStateValue.FORCENO);
                    if (chest.GetComponent<JammedChestBehav>()) newChest.gameObject.AddComponent<JammedChestBehav>();

                    player.CurrentRoom.DeregisterInteractable(chest);
                    chest.DeregisterChestOnMinimap();
                    UnityEngine.Object.Destroy(chest.gameObject);
                }

                shrine.GetComponent<CustomShrineController>().numUses++;
                GameUIRoot.Instance.notificationController.DoCustomNotification(
                       "Kliklok's Blessing",
                        "Chests Upgraded",
                        ShrineFactory.ShrineIconCollection,
                    spriteId,
                        UINotificationController.NotificationColor.SILVER,
                        true,
                        false
                        );
                AkSoundEngine.PostEvent("Play_OBJ_shrine_accept_01", shrine);
            }
        }
        public static int spriteId;
    }
}

