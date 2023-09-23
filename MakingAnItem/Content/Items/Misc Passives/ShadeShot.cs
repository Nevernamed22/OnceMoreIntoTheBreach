using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Alexandria.ItemAPI;
using UnityEngine;
using MonoMod.RuntimeDetour;
using System.Reflection;
using SaveAPI;
using Alexandria.Misc;

namespace NevernamedsItems
{
    public class ShadeShot : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<ShadeShot>(
            "Shade Shot",
            "Astral Disentanglement",
            "Double Shot, with no cost." + "\nA fitting reward for a hard-won victory!" + "\n\nAncient Bullets, buried in a nameless lord's tomb to be taken with them to the next life.",
            "shadeshot_icon");
            item.quality = PickupObject.ItemQuality.S;
            item.SetTag("bullet_modifier");
            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.DRAGUN_KILLED_SHADE, true);
        }

        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {
            StartCoroutine(spawnBonusShot(sourceProjectile));
        }
        private IEnumerator spawnBonusShot(Projectile sourceProjectile)
        {
            yield return null;
            sourceProjectile.SpawnChainedShadowBullets(1, 0.05f);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.PostProcessProjectile -= this.PostProcessProjectile;
            return debrisObject;
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += this.PostProcessProjectile;
        }
        public override void OnDestroy()
        {
            if (Owner)
            {
                Owner.PostProcessProjectile -= this.PostProcessProjectile;
            }
            base.OnDestroy();
        }
    }
}
