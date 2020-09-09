using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using ItemAPI;

namespace NevernamedsItems
{
    class KeyBulletEffigy : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Keybullet Effigy";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/keybulleteffigy_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<KeyBulletEffigy>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Aimgels on High";
            string longDesc = "Keybullet Kin drop bonus keys."+"\n\nA holy item from a historical sect of Gun Cultists that worshipped Keybullet Kin as Aimgels of Kaliber, sent down from Bullet Heaven to deliver holy gifts.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.D;
            item.AddToSubShop(ItemBuilder.ShopType.Cursula);
            item.AddToSubShop(ItemBuilder.ShopType.Flynt);
        }

        //SYNERGY WITH SPARE KEY --> "Spare Keybullet Kin"
        private void OnEnemyDamaged(float damage, bool fatal, HealthHaver enemy)
        {
            if (enemy.aiActor && (enemy.aiActor.EnemyGuid == "699cd24270af4cd183d671090d8323a1" || enemy.aiActor.EnemyGuid == "a446c626b56d4166915a4e29869737fd"))
            {
                if (fatal == true)
                {
                    if (Owner.HasPickupID(Gungeon.Game.Items["nn:spare_key"].PickupObjectId))
                    {
                        LootEngine.SpawnItem(PickupObjectDatabase.GetById(67).gameObject, enemy.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
                        LootEngine.SpawnItem(PickupObjectDatabase.GetById(67).gameObject, enemy.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
                    }
                    else LootEngine.SpawnItem(PickupObjectDatabase.GetById(67).gameObject, enemy.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
                }
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
            Owner.OnAnyEnemyReceivedDamage -= this.OnEnemyDamaged;
            base.OnDestroy();
        }
    }
}
