using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    public class GunpowderPheromones : PassiveItem
    {
        public static void Init()
        {
            PassiveItem item = ItemSetup.NewItem<GunpowderPheromones>(
              "Gunpowder Pheromones",
              "My Pretties",
              "This oddly aromatic powder has peculiar effects on Gundead. Explosive Gundead seem the most succeptable.",
              "gunpowderpheromones_improved") as PassiveItem;
            item.quality = PickupObject.ItemQuality.D;
            ID = item.PickupObjectId;
        }
        public static int ID;
        public static List<string> OtherAffectedEnemies = new List<string>()
        {

        };
        public static List<string> ManuallyExcludedEnemies = new List<string>()
        {
            GUIDs.Lead_Maiden,
            GUIDs.Fridge_Maiden,
            GUIDs.Shambling_Round
        };
        public void AIActorMods(AIActor target)
        {
            if (target && target.healthHaver && !target.healthHaver.IsBoss && !string.IsNullOrEmpty(target.EnemyGuid) && !ManuallyExcludedEnemies.Contains(target.EnemyGuid))
            {
                if (target.gameObject.GetComponent<ExplodeOnDeath>() != null || OtherAffectedEnemies.Contains(target.EnemyGuid) || (Owner.PlayerHasActiveSynergy("Shotgun Pheromones") && target.HasTag("shotgun_kin")))
                {
                    target.ApplyEffect(GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultPermanentCharmEffect, 1f, null);
                    target.gameObject.AddComponent<ContinualKillOnRoomClear>();
                    target.IsHarmlessEnemy = true;
                    target.IgnoreForRoomClear = true;
                    if (target.gameObject.GetComponent<SpawnEnemyOnDeath>())
                    {
                        Destroy(target.gameObject.GetComponent<SpawnEnemyOnDeath>());
                    }
                }
            }
        }
        public override void Pickup(PlayerController player)
        {
            ETGMod.AIActor.OnPreStart += AIActorMods;
            base.Pickup(player);
        }
        public override void DisableEffect(PlayerController player)
        {
            ETGMod.AIActor.OnPreStart -= AIActorMods;
            base.DisableEffect(player);
        }
    }
}