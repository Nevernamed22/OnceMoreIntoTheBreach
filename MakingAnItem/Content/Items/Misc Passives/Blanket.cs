using UnityEngine;
using Alexandria.ItemAPI;
using Dungeonator;
using System.Collections;
using System;
using MonoMod.RuntimeDetour;
using System.Reflection;
using Gungeon;
using System.Linq;
using System.Collections.Generic;
using Alexandria.Misc;

namespace NevernamedsItems
{
    public class Blanket : BlankModificationItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<Blanket>(
            "Blanket",
            "Security",
            "Chance to refund used blanks." + "\n\nWrapping yourself in this child's blanket makes you feel safe, calm, and itchy.",
            "blanket_icon");
            item.quality = PickupObject.ItemQuality.B;
            item.AddToSubShop(ItemBuilder.ShopType.OldRed);
            ID = item.PickupObjectId;
            item.SetTag("ammolet");
        }
        private static int ID;
        public override void Pickup(PlayerController player)
        {
            player.OnUsedBlank += BlankSpent;
            player.GetExtComp().OnBlankModificationItemProcessed += OnBlankModTriggered;
            base.Pickup(player);
        }
        public override void DisableEffect(PlayerController player)
        {
            player.OnUsedBlank -= BlankSpent;
            player.GetExtComp().OnBlankModificationItemProcessed -= OnBlankModTriggered;
            base.DisableEffect(player);
        }
        private void BlankSpent(PlayerController player, int what)
        {
            if (UnityEngine.Random.value <= 0.5f)
            {
                player.BloopItemAboveHead(PickupObjectDatabase.GetById(224).sprite);
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(224).gameObject, player);
            }
        }
        private void OnBlankModTriggered(PlayerController user, SilencerInstance blank, Vector2 pos, BlankModificationItem item)
        {
            if (item == this)
            {
                if (user != null && user.PlayerHasActiveSynergy("My Favourite Blankie") && UnityEngine.Random.value <= 0.1f && user.IsInCombat)
                {
                    user.BloopItemAboveHead(PickupObjectDatabase.GetById(224).sprite);
                    LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(224).gameObject, user);
                }
            }
        }
    }
}

