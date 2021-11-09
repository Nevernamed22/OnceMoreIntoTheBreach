using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using ItemAPI;
using SaveAPI;

namespace NevernamedsItems
{
    class NitroBullets : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Nitro Bullets";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/nitrobullets_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<NitroBullets>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Badda Bing...";
            string longDesc = "50% chance for enemies to explode violently on death."+"\n\nMade by a lunatic who loved the way the ground shook when he used his special brand of... making things go away."+"\n\nYou are not immune to these explosions. You have been warned.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.C;
            item.AddToSubShop(ItemBuilder.ShopType.Trorc);

            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.PURCHASED_NITROBULLETS, true);
            item.AddItemToDougMetaShop(15);
            NitroBulletsID = item.PickupObjectId;
        }
        public static int NitroBulletsID;
        bool hasSynergy;
        //NAME SYNERGY '...Badda Boom!'
        private void OnEnemyDamaged(float damage, bool fatal, HealthHaver enemyHealth)
        {
            if (Owner.HasPickupID(304) || Owner.HasPickupID(Gungeon.Game.Items["nn:nitroglycylinder"].PickupObjectId))
            {
                hasSynergy = true;
            }
            else
            {
                hasSynergy = false;
            }
            
            if (enemyHealth.aiActor && enemyHealth && !enemyHealth.IsBoss && fatal && enemyHealth.aiActor.IsNormalEnemy && hasSynergy == false)
            {
                if (UnityEngine.Random.value < .50f) Exploder.DoDefaultExplosion(enemyHealth.aiActor.CenterPosition, new Vector2());
            }
            else if (enemyHealth && !enemyHealth.IsBoss && fatal && enemyHealth.aiActor && enemyHealth.aiActor.IsNormalEnemy && hasSynergy == true)
            {
                Exploder.DoDefaultExplosion(enemyHealth.aiActor.CenterPosition, new Vector2());
            }

        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnAnyEnemyReceivedDamage += this.OnEnemyDamaged;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.OnAnyEnemyReceivedDamage -= this.OnEnemyDamaged;
            return debrisObject;
        }
        protected override void OnDestroy()
        {
            if (Owner) Owner.OnAnyEnemyReceivedDamage -= this.OnEnemyDamaged;
            base.OnDestroy();
        }
    }
}
