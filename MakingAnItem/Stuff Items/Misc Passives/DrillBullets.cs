﻿using System;
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
    public class DrillBullets : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Drill Bullets";
            string resourceName = "NevernamedsItems/Resources/drillbullets_icon";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<DrillBullets>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Drrrrrrrrr";
            string longDesc = "Bullets gain in damage the more they pierce!"+"\n\nKilling people is probably the most legal thing you can do with these.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");
            item.quality = PickupObject.ItemQuality.A;
        }

        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {
            PierceProjModifier piercing = sourceProjectile.gameObject.GetOrAddComponent<PierceProjModifier>();
            piercing.penetration += 5;
            piercing.penetratesBreakables = true;
            MaintainDamageOnPierce maintenance = sourceProjectile.gameObject.GetOrAddComponent<MaintainDamageOnPierce>();
            maintenance.damageMultOnPierce = 1.25f;
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