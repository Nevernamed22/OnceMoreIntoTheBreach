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
    public class Ammolock : BlankModificationItem
    {
        public static void Init()
        {
            BlankModificationItem item = ItemSetup.NewItem<Ammolock>(
            "Ammolock",
            "Blanks Clamp",
            "Blanks lock enemies in place, unable to move!" + "\n\nForged out of impossible Neutronium Alloy, this Ammolet saps Gundead of all their energy.",
            "ammolock_icon") as BlankModificationItem;
            item.quality = PickupObject.ItemQuality.C;
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.AdditionalBlanksPerFloor, 1f, StatModifier.ModifyMethod.ADDITIVE);
            item.AddToSubShop(ItemBuilder.ShopType.OldRed);
            ID = item.PickupObjectId;
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
            if (item is Ammolock)
            {
                List<AIActor> activeEnemies = user.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
                if (activeEnemies != null)
                {
                    for (int i = 0; i < activeEnemies.Count; i++)
                    {
                        AIActor aiactor = activeEnemies[i];
                        if (aiactor)
                        {
                            aiactor.gameActor.ApplyEffect(StatusEffectHelper.GenerateLockdown(10), 1f, null);
                            if (user.PlayerHasActiveSynergy("Under Lock And Key") && aiactor.healthHaver) aiactor.healthHaver.ApplyDamage(7 * user.carriedConsumables.KeyBullets, Vector2.zero, "Under Lock And Key");
                        }
                    }
                }
            }
        }
    }
}