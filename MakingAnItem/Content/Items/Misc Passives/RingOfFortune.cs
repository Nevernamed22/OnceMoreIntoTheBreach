using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;
using SaveAPI;

namespace NevernamedsItems
{
    class RingOfFortune : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<RingOfFortune>(
            "Ring Of Fortune",
            "+1 To Fortune",
            "Grants a single casing for every enemy slain.\n\nUsed by a gundead Beggar to barely scrape by in the days of old, before his eyesight became too bad to shoot down Bullats.",
            "ringoffortune_icon");
            item.quality = PickupObject.ItemQuality.B;
            item.additionalMagnificenceModifier = 1;
            item.SetTag("lucky");
            ID = item.PickupObjectId;
            item.SetupUnlockOnCustomStat(CustomTrackedStats.BEGGAR_TOTAL_DONATIONS, 2554, DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN);
        }
        public static int ID;
        private void OnEnemyDamaged(float damage, bool fatal, HealthHaver enemy)
        {
            if (fatal == true && base.Owner)
            {
                LootEngine.TryGivePrefabToPlayer(PickupObjectDatabase.GetById(68).gameObject, base.Owner, false);
            }
        }
        public override void Pickup(PlayerController player)
        {
            player.OnAnyEnemyReceivedDamage += this.OnEnemyDamaged;
            base.Pickup(player);
        }
        public override void DisableEffect(PlayerController player)
        {
            if (player) player.OnAnyEnemyReceivedDamage -= this.OnEnemyDamaged;
            base.DisableEffect(player);
        }
    }
}
