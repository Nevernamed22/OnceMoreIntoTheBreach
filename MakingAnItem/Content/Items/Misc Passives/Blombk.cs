using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using  Alexandria.ItemAPI;
using Alexandria.Misc;
using MonoMod.RuntimeDetour;
using System.Reflection;

namespace NevernamedsItems
{
    class Blombk : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<Blombk>(
            "Blombk",
            "Boomer Blanks",
            "Triggers a small blank whenever an explosion goes off." + "\n\nA Fuselier egg painted blue.",
            "blombk_icon");
            item.quality = PickupObject.ItemQuality.C;
            item.AddToSubShop(ItemBuilder.ShopType.OldRed);
            BlombkID = item.PickupObjectId;
        }
        public static int BlombkID;

        public override void Pickup(PlayerController player)
        {
            CustomActions.OnExplosionComplex += Explosion;
            base.Pickup(player);
        }
        public void Explosion(Vector3 position, ExplosionData data, Vector2 dir, Action onbegin, bool ignoreQueues, CoreDamageTypes damagetypes, bool ignoreDamageCaps)
        {
            if (Owner)
            {
                EasyBlankType type = EasyBlankType.MINI;
                if (Owner.PlayerHasActiveSynergy("Atomic Blombk") && UnityEngine.Random.value < 0.2f) type = EasyBlankType.FULL;
                Owner.DoEasyBlank(position, type);
            }
        }
        public override void DisableEffect(PlayerController player)
        {
            CustomActions.OnExplosionComplex -= Explosion;
            base.DisableEffect(player);
        }
    }
}

