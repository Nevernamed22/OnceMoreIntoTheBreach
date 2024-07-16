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
            GunpowderPheromonesID = item.PickupObjectId;
        }
        public static int GunpowderPheromonesID;
        public static List<string> explosiveKin = new List<string>()
        {
            EnemyGuidDatabase.Entries["brollet"],
            EnemyGuidDatabase.Entries["grenat"],
            EnemyGuidDatabase.Entries["grenade_kin"],
            EnemyGuidDatabase.Entries["dynamite_kin"],
            EnemyGuidDatabase.Entries["bombshee"],
            EnemyGuidDatabase.Entries["m80_kin"],
            EnemyGuidDatabase.Entries["mine_flayers_claymore"],
            EnemyGuidDatabase.Entries["det"],
            EnemyGuidDatabase.Entries["x_det"],
            EnemyGuidDatabase.Entries["diagonal_x_det"],
            EnemyGuidDatabase.Entries["vertical_det"],
            EnemyGuidDatabase.Entries["horizontal_det"],
            EnemyGuidDatabase.Entries["diagonal_det"],
            EnemyGuidDatabase.Entries["vertical_x_det"],
            EnemyGuidDatabase.Entries["horizontal_x_det"],
        };
        public static List<string> shotgunEnemies = new List<string>()
        {
            EnemyGuidDatabase.Entries["red_shotgun_kin"],
            EnemyGuidDatabase.Entries["blue_shotgun_kin"],
            EnemyGuidDatabase.Entries["veteran_shotgun_kin"],
            EnemyGuidDatabase.Entries["mutant_shotgun_kin"],
            EnemyGuidDatabase.Entries["executioner"],
            EnemyGuidDatabase.Entries["ashen_shotgun_kin"],
            EnemyGuidDatabase.Entries["shotgrub"],
            EnemyGuidDatabase.Entries["creech"],
            EnemyGuidDatabase.Entries["western_shotgun_kin"],
            EnemyGuidDatabase.Entries["shotgat"],
            EnemyGuidDatabase.Entries["pirate_shotgun_kin"],
        };
        public void AIActorMods(AIActor target)
        {
            if (target && target.aiActor && target.aiActor.EnemyGuid != null)
            {
                string enemyGuid = target?.aiActor?.EnemyGuid;
                if (!string.IsNullOrEmpty(enemyGuid))
                {
                    try
                    {
                        //ETGModConsole.Log("This enemy's Guid is: " + enemyGuid);                   
                        if (explosiveKin.Contains(enemyGuid))
                        {
                            target.ApplyEffect(GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultPermanentCharmEffect, 1f, null);
                            target.gameObject.AddComponent<KillOnRoomClear>();
                            target.IsHarmlessEnemy = true;
                            target.IgnoreForRoomClear = true;
                            if (target.gameObject.GetComponent<SpawnEnemyOnDeath>())
                            {
                                Destroy(target.gameObject.GetComponent<SpawnEnemyOnDeath>());
                            }
                            return;
                        }
                        else if (Owner.HasPickupID(Gungeon.Game.Items["nn:shutdown_shells"].PickupObjectId))
                        {
                            if (shotgunEnemies.Contains(enemyGuid))
                            {
                                target.ApplyEffect(GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultPermanentCharmEffect, 1f, null);
                                target.gameObject.AddComponent<KillOnRoomClear>();
                                target.IsHarmlessEnemy = true;
                                target.IgnoreForRoomClear = true;
                                if (target.gameObject.GetComponent<SpawnEnemyOnDeath>())
                                {
                                    Destroy(target.gameObject.GetComponent<SpawnEnemyOnDeath>());
                                }
                                return;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        ETGModConsole.Log(e.Message);
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