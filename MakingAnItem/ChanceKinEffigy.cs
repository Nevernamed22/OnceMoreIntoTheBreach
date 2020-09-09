using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using ItemAPI;

namespace NevernamedsItems
{
    class ChanceKinEffigy : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Chance Effigy";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/chanceeffigy_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<ChanceKinEffigy>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Guns Upon A Time";
            string longDesc = "Chance Kin drop bonus supplies." + "\n\nHailing from the same ludicrous sect who forged the Keybullet Effigy. Their religious rites, while inclusive of Chance Kin, rarely focus on them as they are perceived as lesser spirits.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.D;
            //item.AddToSubShop(ItemBuilder.ShopType.Cursula);
        }

        //SYNERGY WITH SPARE KEY --> "Spare Keybullet Kin"
        private void OnEnemyDamaged(float damage, bool fatal, HealthHaver enemy)
        {
            if (enemy.aiActor && (enemy.aiActor.EnemyGuid == "699cd24270af4cd183d671090d8323a1" || enemy.aiActor.EnemyGuid == "a446c626b56d4166915a4e29869737fd"))
            {
                if (fatal == true)
                {
                    int lootID = BraveUtility.RandomElement(lootIDlist);
                    LootEngine.SpawnItem(PickupObjectDatabase.GetById(lootID).gameObject, enemy.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
                    if (Owner.HasPickupID(Gungeon.Game.Items["nn:keybullet_effigy"].PickupObjectId))
                    {
                        float currentDamage = Owner.stats.GetBaseStatValue(PlayerStats.StatType.Damage);
                        Owner.stats.SetBaseStatValue(PlayerStats.StatType.Damage, currentDamage * 1.1f, Owner);
                    }
                }
            }

        }
        public static List<int> lootIDlist = new List<int>()
        {
            78, //Ammo
            600, //Spread Ammo
            565, //Glass Guon Stone
            73, //Half Heart
            85, //Heart
            120, //Armor
        };

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
