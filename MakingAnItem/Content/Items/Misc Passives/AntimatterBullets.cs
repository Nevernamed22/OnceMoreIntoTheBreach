using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    public class AntimatterBullets : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<AntimatterBullets>(
             "Antimatter Bullets",
             "Prime Antimaterial",
             "Your bullets have a chance to create an explosion upon intersecting with enemy bullets." + "\n\nWhy does't this trigger when you actually hit an enemy?... don't ask questions.",
             "antimatterbullets_improved");
            item.quality = PickupObject.ItemQuality.A;
            item.SetTag("bullet_modifier");
            AntimatterBulletsID = item.PickupObjectId;

            Doug.AddToLootPool(item.PickupObjectId);
        }
        public static int AntimatterBulletsID;

        private void PostProcessProjectile(Projectile proj, float flot)
        {
            ExplodeOnBulletIntersection mod = proj.gameObject.GetOrAddComponent<ExplodeOnBulletIntersection>();
            mod.explosionData = smallPlayerSafeExplosion;
        }
        public override void Pickup(PlayerController player)
        {
            player.PostProcessProjectile += PostProcessProjectile;
            base.Pickup(player);
        }
        public override void DisableEffect(PlayerController player)
        {
            if (player) { player.PostProcessProjectile -= PostProcessProjectile; }
            base.DisableEffect(player);
        }      
        public static ExplosionData smallPlayerSafeExplosion = new ExplosionData()
        {
            effect = GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultSmallExplosionData.effect,
            ss = GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultSmallExplosionData.ss,
            ignoreList = GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultSmallExplosionData.ignoreList,
            damageRadius = 2.5f,
            damageToPlayer = 0f,
            doDamage = true,
            damage = 25,
            doDestroyProjectiles = false,
            doForce = true,
            debrisForce = 30f,
            preventPlayerForce = true,
            explosionDelay = 0.1f,
            usesComprehensiveDelay = false,
            doScreenShake = true,
            playDefaultSFX = true,
        };
    }
}
