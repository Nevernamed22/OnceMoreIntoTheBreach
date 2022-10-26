using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;
using SaveAPI;

namespace NevernamedsItems
{
    class CrosshairNecklace : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Crosshair Necklace";
            string resourceName = "NevernamedsItems/Resources/NeoItemSprites/crosshairnecklace_icon";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<CrosshairNecklace>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Via Crucis";
            string longDesc = "Jammed enemies have a chance to drop pickups."+"\n\nA necklace worn on occasion by Cultists of the True Gun for protection and prosperity.";
          
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 1, StatModifier.ModifyMethod.ADDITIVE);

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");
            item.quality = PickupObject.ItemQuality.D;
            item.AddToSubShop(ItemBuilder.ShopType.Cursula);
            ID = item.PickupObjectId;
        }
        public static int ID;
        private void OnEnemyDamaged(float damage, bool fatal, HealthHaver enemyHealth)
        {
            if (enemyHealth?.aiActor?.IsBlackPhantom == true && fatal && UnityEngine.Random.value <= 0.1f)
            {
                int lootID = BraveUtility.RandomElement(BabyGoodChanceKin.lootIDlist);
                LootEngine.SpawnItem(PickupObjectDatabase.GetById(lootID).gameObject, enemyHealth.sprite.WorldCenter, Vector2.zero, 1f, false, true, false);
            }
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnAnyEnemyReceivedDamage += this.OnEnemyDamaged;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            player.OnAnyEnemyReceivedDamage -= this.OnEnemyDamaged;
            return base.Drop(player);
        }
        public override void OnDestroy()
        {
            if (Owner) Owner.OnAnyEnemyReceivedDamage -= this.OnEnemyDamaged;
            base.OnDestroy();
        }
    }
}
