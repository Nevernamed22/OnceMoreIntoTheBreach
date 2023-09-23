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
            PickupObject item = ItemSetup.NewItem<CrosshairNecklace>(
            "Crosshair Necklace",
            "Via Crucis",
            "Jammed enemies have a chance to drop pickups." + "\n\nA necklace worn on occasion by Cultists of the True Gun for protection and prosperity.",
            "crosshairnecklace_icon");
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 1, StatModifier.ModifyMethod.ADDITIVE);
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
