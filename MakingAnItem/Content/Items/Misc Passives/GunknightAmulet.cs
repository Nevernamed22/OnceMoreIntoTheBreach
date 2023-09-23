using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;
using MonoMod.RuntimeDetour;
using System.Reflection;
using System.Collections.Generic;
using Dungeonator;
using System.Collections;

namespace NevernamedsItems
{
    public class GunknightAmulet : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<GunknightAmulet>(
            "Gunknight Amulet",
            "Banished Reload",
            "Chance to skip reload." + "\n\nThe lost amulet of Cormorant, the Aimless Gunknight. It was given to him by his father, who obtained it from his father before him, and his father before him, and his father before him...",
            "gunknightamulet_icon");
            item.CanBeDropped = true;
            item.quality = PickupObject.ItemQuality.D;
        }
        private void OnReload(PlayerController player, Gun gun)
        {
            float procChance = 0.25f;
            if (player.PlayerHasActiveSynergy("Reuknighted")) procChance = 0.5f;
            if (UnityEngine.Random.value <= procChance)
            {
            gun.ForceImmediateReload(false);
            player.StartCoroutine(HandleEffects(player, gun));
            }
        }
        private IEnumerator HandleEffects(PlayerController player, Gun gun)
        {
            yield return null;
            if (gun.CurrentOwner is PlayerController)
            {
                int i = (gun.CurrentOwner as PlayerController).PlayerIDX;
                GameUIRoot.Instance.ForceClearReload(i);
            }
            yield break;
        }
        public override void Pickup(PlayerController player)
        {
            player.OnReloadedGun += this.OnReload;
            base.Pickup(player);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.OnReloadedGun -= this.OnReload;
            return base.Drop(player);
        }
        public GunknightAmulet()
        {

        }
    }

}