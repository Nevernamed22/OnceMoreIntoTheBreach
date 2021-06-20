using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using ItemAPI;
using UnityEngine;
using MonoMod.RuntimeDetour;
using System.Reflection;

namespace NevernamedsItems
{
    public class Splattershot : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Splattershot";
            string resourceName = "NevernamedsItems/Resources/splattershot_icon";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<Splattershot>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "SPLAT";
            string longDesc = "Bullets spend the beginning of their lives clustered close together, before splitting apart in a hearbreaking parable about the pain of growing up.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");
            item.quality = PickupObject.ItemQuality.B;
        }

        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {
            ProjectileSplitController splitCont = sourceProjectile.gameObject.GetOrAddComponent<ProjectileSplitController>();
            splitCont.distanceBasedSplit = true;
            if (sourceProjectile.ProjectilePlayerOwner() && sourceProjectile.ProjectilePlayerOwner().PlayerHasActiveSynergy("3 + 1 = 4"))
            {
                splitCont.amtToSplitTo = 4;
            }
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
        protected override void OnDestroy()
        {
            if (Owner)
            {
                Owner.PostProcessProjectile -= this.PostProcessProjectile;
            }
            base.OnDestroy();
        }
    }
}