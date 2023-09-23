using UnityEngine;
using Alexandria.ItemAPI;
using Dungeonator;
using System.Collections;
using System;
using MonoMod.RuntimeDetour;
using System.Reflection;
using Gungeon;
using System.Collections.Generic;
using SaveAPI;
using Alexandria.Misc;

namespace NevernamedsItems
{
    public class KinAmmolet : BlankModificationItem
    {
        public static void Init()
        {
            BlankModificationItem item = ItemSetup.NewItem<KinAmmolet>(
            "Kin Ammolet",
            "Blanks Reinforce",
            "Blanks summon reinforcements to aid you in combat!" + "\n\nThe little pendant is sentient, and very, very confused.",
            "kinammolet_icon") as BlankModificationItem;
            item.quality = PickupObject.ItemQuality.D;
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.AdditionalBlanksPerFloor, 1f, StatModifier.ModifyMethod.ADDITIVE);
            item.AddToSubShop(ItemBuilder.ShopType.OldRed);
            item.AddToSubShop(ItemBuilder.ShopType.Cursula);
            ID = item.PickupObjectId;

            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.PURCHASED_KINAMMOLET, true);
            item.AddItemToDougMetaShop(15);
            item.SetTag("ammolet");
        }

        private static int ID;
        public override void Pickup(PlayerController player)
        {
            player.GetExtComp().OnBlankModificationItemProcessed += OnBlankModTriggered;
            base.Pickup(player);
        }
        public override void DisableEffect(PlayerController player)
        {
            player.GetExtComp().OnBlankModificationItemProcessed -= OnBlankModTriggered;
            base.DisableEffect(player);
        }

        private void OnBlankModTriggered(PlayerController user, SilencerInstance blank, Vector2 pos, BlankModificationItem item)
        {
            if (item is KinAmmolet)
            {
                RoomHandler currentRoom = user.CurrentRoom;
                if (currentRoom != null && currentRoom.IsSealed)
                {
                    string enemyToSpawnGUID = "01972dee89fc4404a5c408d50007dad5";
                    bool shouldJam = false;
                    if (user.PlayerHasActiveSynergy("Shotgun Club") && UnityEngine.Random.value <= 0.5f) enemyToSpawnGUID = "128db2f0781141bcb505d8f00f9e4d47";
                    if (user.PlayerHasActiveSynergy("Friends On The Other Side")) shouldJam = true;

                    CompanionisedEnemyUtility.SpawnCompanionisedEnemy(user, enemyToSpawnGUID, pos.ToIntVector2(), false, ExtendedColours.brown, 15, 2, shouldJam, true);
                    if ((user.PlayerHasActiveSynergy("Aim Twice, Shoot Once")))
                    {
                        CompanionisedEnemyUtility.SpawnCompanionisedEnemy(user, enemyToSpawnGUID, pos.ToIntVector2(), false, ExtendedColours.brown, 15, 2, shouldJam, true);
                    }
                }
            }
        }
    }
}
