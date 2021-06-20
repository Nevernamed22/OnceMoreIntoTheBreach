using Dungeonator;
using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    class AmmoPickupHooks
    {
        public static void Init()
        {
            ammoPickupHook = new Hook(
                typeof(AmmoPickup).GetMethod("Pickup", BindingFlags.Instance | BindingFlags.Public),
                typeof(AmmoPickupHooks).GetMethod("ammoPickupHookMethod")
            );

            ammoInteractHook = new Hook(
                typeof(AmmoPickup).GetMethod("Interact", BindingFlags.Instance | BindingFlags.Public),
                typeof(AmmoPickupHooks).GetMethod("ammoInteractHookMethod")
            );

            OverrideNoCheckIDs = new List<int>()
            {
                Blankannon.BlankannonId,
                Creditor.CreditorID,
            };
        }
        public static Hook ammoPickupHook;
        public static Hook ammoInteractHook;
        public static void ammoPickupHookMethod(Action<AmmoPickup, PlayerController> orig, AmmoPickup self, PlayerController player)
        {
            if (player.CurrentGun && player.CurrentGun.PickupObjectId == Blankannon.BlankannonId)
            {
                if (player.CurrentGun.CurrentAmmo == player.CurrentGun.AdjustedMaxAmmo)
                {
                    HandleFuckedUpGunAmmoPickup(self, player);
                }
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(224).gameObject, player);
            }
            orig(self, player);
            if (player.HasPickupID(MengerAmmoBox.MengerAmmoBoxID))
            {
                if (self.mode == AmmoPickup.AmmoPickupMode.FULL_AMMO)
                {
                    for (int i = 0; i < player.inventory.AllGuns.Count; i++)
                    {
                        if (player.inventory.AllGuns[i] && player.CurrentGun != player.inventory.AllGuns[i])
                        {
                            player.inventory.AllGuns[i].GainAmmo(Mathf.FloorToInt((float)player.inventory.AllGuns[i].AdjustedMaxAmmo * 0.2f));
                        }
                    }
                    player.CurrentGun.ForceImmediateReload(false);
                    if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
                    {
                        PlayerController otherPlayer = GameManager.Instance.GetOtherPlayer(player);
                        if (!otherPlayer.IsGhost)
                        {
                            for (int j = 0; j < otherPlayer.inventory.AllGuns.Count; j++)
                            {
                                if (otherPlayer.inventory.AllGuns[j])
                                {
                                    otherPlayer.inventory.AllGuns[j].GainAmmo(Mathf.FloorToInt((float)otherPlayer.inventory.AllGuns[j].AdjustedMaxAmmo * 0.2f));
                                }
                            }
                            otherPlayer.CurrentGun.ForceImmediateReload(false);
                        }
                    }
                }
                else if (self.mode == AmmoPickup.AmmoPickupMode.SPREAD_AMMO)
                {
                    if (player.CurrentGun != null && player.CurrentGun.CanGainAmmo)
                    {
                        player.CurrentGun.GainAmmo(player.CurrentGun.AdjustedMaxAmmo);
                        player.CurrentGun.ForceImmediateReload(false);
                    }
                }

            }
            if (player.HasPickupID(BloodyAmmo.BloodyAmmoID))
            {
                int bloodyAmount = player.GetNumberOfItemInInventory(BloodyAmmo.BloodyAmmoID);
                for (int i = 0; i < bloodyAmount; i++)
                {
                    player.healthHaver.ApplyHealing(0.5f);
                    if (player.characterIdentity == PlayableCharacters.Robot)
                    {
                        LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(120).gameObject, player);
                    }
                }
            }
        }
        public static void HandleFuckedUpGunAmmoPickup(AmmoPickup pickup, PlayerController player)
        {
            if (pickup.mode == AmmoPickup.AmmoPickupMode.FULL_AMMO)
            {
                string @string = StringTableManager.GetString("#AMMO_SINGLE_GUN_REFILLED_HEADER");
                string description = player.CurrentGun.GetComponent<EncounterTrackable>().journalData.GetPrimaryDisplayName(false) + " " + StringTableManager.GetString("#AMMO_SINGLE_GUN_REFILLED_BODY");
                tk2dBaseSprite sprite = player.CurrentGun.GetSprite();
                if (!GameUIRoot.Instance.BossHealthBarVisible)
                {
                    GameUIRoot.Instance.notificationController.DoCustomNotification(@string, description, sprite.Collection, sprite.spriteId, UINotificationController.NotificationColor.SILVER, false, false);
                }

            }
            else if (pickup.mode == AmmoPickup.AmmoPickupMode.SPREAD_AMMO)
            {
                for (int i = 0; i < player.inventory.AllGuns.Count; i++)
                {
                    if (player.inventory.AllGuns[i] && player.CurrentGun != player.inventory.AllGuns[i])
                    {
                        player.inventory.AllGuns[i].GainAmmo(Mathf.FloorToInt((float)player.inventory.AllGuns[i].AdjustedMaxAmmo * pickup.SpreadAmmoOtherGunsPercent));
                    }
                }
                player.CurrentGun.ForceImmediateReload(false);
                if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
                {
                    PlayerController otherPlayer = GameManager.Instance.GetOtherPlayer(player);
                    if (!otherPlayer.IsGhost)
                    {
                        for (int j = 0; j < otherPlayer.inventory.AllGuns.Count; j++)
                        {
                            if (otherPlayer.inventory.AllGuns[j])
                            {
                                otherPlayer.inventory.AllGuns[j].GainAmmo(Mathf.FloorToInt((float)otherPlayer.inventory.AllGuns[j].AdjustedMaxAmmo * pickup.SpreadAmmoOtherGunsPercent));
                            }
                        }
                        otherPlayer.CurrentGun.ForceImmediateReload(false);
                    }
                }
                string string2 = StringTableManager.GetString("#AMMO_SINGLE_GUN_REFILLED_HEADER");
                string string3 = StringTableManager.GetString("#AMMO_SPREAD_REFILLED_BODY");
                tk2dBaseSprite sprite2 = pickup.sprite;
                if (!GameUIRoot.Instance.BossHealthBarVisible)
                {
                    GameUIRoot.Instance.notificationController.DoCustomNotification(string2, string3, sprite2.Collection, sprite2.spriteId, UINotificationController.NotificationColor.SILVER, false, false);
                }
            }

            FieldInfo field = typeof(AmmoPickup).GetField("m_pickedUp", BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValue(pickup, false);

            FieldInfo field2 = typeof(AmmoPickup).GetField("m_isBeingEyedByRat", BindingFlags.Instance | BindingFlags.NonPublic);
            field2.SetValue(pickup, false);

            var type = typeof(AmmoPickup);
            var func = type.GetMethod("GetRidOfMinimapIcon", BindingFlags.Instance | BindingFlags.NonPublic);
            var ret = func.Invoke(pickup.gameObject.GetComponent<AmmoPickup>(), null);

            if (pickup.pickupVFX != null)
            {
                player.PlayEffectOnActor(pickup.pickupVFX, Vector3.zero, true, false, false);
            }
            UnityEngine.Object.Destroy(pickup.gameObject);
            AkSoundEngine.PostEvent("Play_OBJ_ammo_pickup_01", pickup.gameObject);
        }
        public static void ammoInteractHookMethod(Action<AmmoPickup, PlayerController> orig, AmmoPickup self, PlayerController player)
        {
            if (player.CurrentGun && OverrideNoCheckIDs.Contains(player.CurrentGun.PickupObjectId))
            {
                if (RoomHandler.unassignedInteractableObjects.Contains(self))
                {
                    RoomHandler.unassignedInteractableObjects.Remove(self);
                }
                SpriteOutlineManager.RemoveOutlineFromSprite(self.sprite, true);
                self.Pickup(player);
            }
            else
            {
                orig(self, player);
            }
        }
        public static List<int> OverrideNoCheckIDs;
    }
    class HealthPickupHooks
    {
        public static void Init()
        {
            prePickupLogicHook = new Hook(
                typeof(HealthPickup).GetMethod("PrePickupLogic", BindingFlags.Instance | BindingFlags.NonPublic),
                typeof(HealthPickupHooks).GetMethod("PrePickuphookLogic")
            );
        }
        public static void PrePickuphookLogic(Action<HealthPickup, SpeculativeRigidbody, SpeculativeRigidbody> orig, HealthPickup self, SpeculativeRigidbody player, SpeculativeRigidbody selfBody)
        {
            if (self && player.gameActor && player.gameActor is PlayerController)
            {
                PlayerController playerCont = player.gameActor as PlayerController;
                if (playerCont.ModdedCharacterIdentity() == ModdedCharacterID.Shade && self.armorAmount <= 0)
                {
                    if (playerCont.HasPickupID(BloodshotEye.BloodshotEyeID))
                    {
                        float percentPerArmour = 0.02f;
                        float amt1 = self.healAmount / 0.5f;
                        int amt2 = Mathf.CeilToInt(amt1);

                        StatModifier statModifier = new StatModifier();
                        statModifier.amount = (percentPerArmour * amt2) + 1;
                        statModifier.modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE;
                        statModifier.statToBoost = PlayerStats.StatType.Damage;
                        playerCont.ownerlessStatModifiers.Add(statModifier);
                        playerCont.stats.RecalculateStats(playerCont, false, false);
                    }
                    FieldInfo field = typeof(HealthPickup).GetField("m_pickedUp", BindingFlags.Instance | BindingFlags.NonPublic);
                    field.SetValue(self, true);
                    AkSoundEngine.PostEvent("Play_OBJ_coin_medium_01", self.gameObject);
                    int amountToDrop = (self.healAmount >= 1f) ? UnityEngine.Random.Range(5, 12) : UnityEngine.Random.Range(3, 7);
                    LootEngine.SpawnCurrency((!self.sprite) ? self.specRigidbody.UnitCenter : self.sprite.WorldCenter, amountToDrop, false);

                    var type = typeof(HealthPickup);
                    var func = type.GetMethod("GetRidOfMinimapIcon", BindingFlags.Instance | BindingFlags.NonPublic);
                    var ret = func.Invoke(self, null);

                    self.ToggleLabel(false);
                    UnityEngine.Object.Destroy(self.gameObject);
                }
                else
                {
                    orig(self, player, selfBody);
                }
            }
        }
        public static Hook prePickupLogicHook;
    }
}
