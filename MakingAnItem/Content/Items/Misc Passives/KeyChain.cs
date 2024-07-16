using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Alexandria.ItemAPI;
using UnityEngine;
using MonoMod.RuntimeDetour;
using System.Reflection;

namespace NevernamedsItems
{
    public class KeyChain : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<KeyChain>(
            "Keychain",
            "Chain Reaction",
            "25% chance to double any keys picked up." + "\n\nNobody quite understands how keys work in the Gungeon, but this thing apparently gives you more of them so who cares.",
            "keychain_icon");
            item.quality = PickupObject.ItemQuality.B;
            item.AddToSubShop(ItemBuilder.ShopType.Flynt);
        }

        Hook keyPickupHook = new Hook(
                typeof(KeyBulletPickup).GetMethod("Pickup", BindingFlags.Instance | BindingFlags.Public),
                typeof(KeyChain).GetMethod("keyPickupHookMethod")
            );
        public static void keyPickupHookMethod(Action<KeyBulletPickup, PlayerController> orig, KeyBulletPickup self, PlayerController player)
        {
            orig(self, player);
            if (player.HasPickupID(Gungeon.Game.Items["nn:keychain"].PickupObjectId))
            {
                if (!self.IsRatKey && UnityEngine.Random.value < .25f) player.carriedConsumables.KeyBullets += 1;
                else if (self.IsRatKey && UnityEngine.Random.value < .1f) player.carriedConsumables.ResourcefulRatKeys += 1;
            }
        }
    }
}
