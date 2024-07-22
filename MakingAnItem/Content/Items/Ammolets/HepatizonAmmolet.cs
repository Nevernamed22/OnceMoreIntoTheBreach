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
    public class HepatizonAmmolet : BlankModificationItem
    {
        public static void Init()
        {
            BlankModificationItem item = ItemSetup.NewItem<HepatizonAmmolet>(
            "Hepatizon Ammolet",
            "Blanks Infect",
            "Blanks blast out microparticles of infected fluid, spreading the plague to enemies." + "\n\nSome say the original plague virus was brought to the Gungeon by the Resourceful Rat.",
            "hepatizonammolet_icon") as BlankModificationItem;
            item.quality = PickupObject.ItemQuality.B;
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.AdditionalBlanksPerFloor, 1f, StatModifier.ModifyMethod.ADDITIVE);
            item.AddToSubShop(ItemBuilder.ShopType.OldRed);
            ID = item.PickupObjectId;

            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.PURCHASED_HEPATIZONAMMOLET, true);
            item.AddItemToDougMetaShop(47);
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
            if (player != null && player.GetExtComp()) player.GetExtComp().OnBlankModificationItemProcessed -= OnBlankModTriggered;
            base.DisableEffect(player);
        }
        private void OnBlankModTriggered(PlayerController user, SilencerInstance blank, Vector2 pos, BlankModificationItem item)
        {
            if (item is HepatizonAmmolet)
            {
                List<AIActor> activeEnemies = user.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
                if (activeEnemies != null)
                {
                    for (int i = 0; i < activeEnemies.Count; i++)
                    {
                        if (UnityEngine.Random.value <= 0.33f)
                        {
                            activeEnemies[i].ApplyEffect(StaticStatusEffects.StandardPlagueEffect);
                        }
                    }
                }
            }
        }
    }
}