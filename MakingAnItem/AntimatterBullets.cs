using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using ItemAPI;

namespace NevernamedsItems
{
    public class AntimatterBullets : IntersectEnemyBulletsItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Antimatter Bullets";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/antimatterbullets_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<AntimatterBullets>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Prime Antimaterial";
            string longDesc = "Your bullets have a chance to create an explosion upon intersecting with enemy bullets." + "\n\nWhy does't this trigger when you actually hit an enemy?... don't ask questions.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.A;

        }

        public static List<Projectile> NonValidProjectile = new List<Projectile>() { };

        public override void DoIntersectionEffect(Projectile playerBullet, Projectile enemyBullet)
        {
            if (!NonValidProjectile.Contains(enemyBullet))
            {
                DoSafeExplosion(enemyBullet.specRigidbody.UnitCenter);
                NonValidProjectile.Add(enemyBullet);
            }
            base.DoIntersectionEffect(playerBullet, enemyBullet);
        }
        ExplosionData smallPlayerSafeExplosion = new ExplosionData()
        {
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
        public void DoSafeExplosion(Vector3 position)
        {
            var defaultExplosion = GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultSmallExplosionData;
            smallPlayerSafeExplosion.effect = defaultExplosion.effect;
            smallPlayerSafeExplosion.ignoreList = defaultExplosion.ignoreList;
            smallPlayerSafeExplosion.ss = defaultExplosion.ss;
            Exploder.Explode(position, smallPlayerSafeExplosion, Vector2.zero);
        }
    }

}
