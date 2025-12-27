using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Text;
using System.Reflection;
using Dungeonator;
using Alexandria.ItemAPI;
using UnityEngine;
using System.Linq.Expressions;

namespace NevernamedsItems
{
    class MagickeCauldron : PlayerItem
    {
        public static void Init()
        {
            PlayerItem item = ItemSetup.NewItem<MagickeCauldron>(
              "Alchemy Crucible",
              "Philosopher's Own",
              "Magically changes enemies into other enemies from the same floor." + "\nBest used on strong enemies, where the only way forwards is down." + "\n\nDon't get used to yourself. You're gonna have to change.",
              "magickecauldron_icon") as PlayerItem;  
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 150);
            item.consumable = false;
            item.quality = ItemQuality.C;
        }
        public static GameObject overheadder = ResourceCache.Acquire("Global VFX/VFX_Debuff_Status") as GameObject;
        public static List<AIActor> enemiesToReRoll = new List<AIActor>();
        public static List<AIActor> enemiesToPostMogModify = new List<AIActor>();
        public List<string> GetGuidList(PlayerController user)
        {
            if (user.PlayerHasActiveSynergy("I CAN DO ANYTHING")) { return GUIDs.GenerateChaosPalette(true, true, true, true, true, true); }
            else { return GUIDs.CurrentFloorEnemyPalette; }
        }

        public override void DoEffect(PlayerController user)
        {
            try
            {
                List<string> possibleTargetGUIDS = GetGuidList(user);

                List<AIActor> activeEnemies = user.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
                if (activeEnemies != null)
                {
                    enemiesToReRoll.Clear();
                    enemiesToPostMogModify.Clear();
                    for (int i = 0; i < activeEnemies.Count; i++)
                    {
                        enemiesToReRoll.Add(activeEnemies[i]);
                    }
                    foreach (AIActor enemy in enemiesToReRoll)
                    {
                        string guidToTransmogTo = GUIDs.Bullet_Kin;
                        if (possibleTargetGUIDS != null && possibleTargetGUIDS.Count > 0) { guidToTransmogTo = BraveUtility.RandomElement(possibleTargetGUIDS); }

                        //Modify Enemy
                        if (enemy.gameObject.GetComponent<ExplodeOnDeath>()) { Destroy(enemy.gameObject.GetComponent<ExplodeOnDeath>()); }
                        if (!enemy.healthHaver.IsVulnerable) enemy.healthHaver.IsVulnerable = true;

                        //ACTUALLY DO THE TRANSMOGGING
                        enemy.Transmogrify(EnemyDatabase.GetOrLoadByGuid(guidToTransmogTo), (GameObject)ResourceCache.Acquire("Global VFX/VFX_Item_Spawn_Poof"));
                    }
                    List<AIActor> activeEnemies2 = user.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
                    if (activeEnemies2 != null)
                    {
                        for (int i = 0; i < activeEnemies2.Count; i++)
                        {
                            AIActor aiactor = activeEnemies2[i];
                            enemiesToPostMogModify.Add(aiactor);
                            if (aiactor.IsTransmogrified)
                            {
                                if (user.PlayerHasActiveSynergy("A Moste Accursed Brew"))
                                {
                                    aiactor.ApplyEffect(new AIActorDebuffEffect
                                    {
                                        HealthMultiplier = 0.5f,
                                        CooldownMultiplier = 1f,
                                        SpeedMultiplier = 1f,
                                        KeepHealthPercentage = true,
                                        OverheadVFX = ResourceCache.Acquire("Global VFX/VFX_Debuff_Status") as GameObject,
                                        duration = 1000000f
                                    }, 1f, null);

                                }
                            }
                        }
                    }
                    foreach (AIActor enemy in enemiesToPostMogModify) PostTransmogrifyEnemy(enemy);

                }
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
                ETGModConsole.Log(e.StackTrace);
            }
        }
        public void PostTransmogrifyEnemy(AIActor enemy)
        {
            try
            {
                if (enemy.EnemyGuid == GUIDs.Spent)
                {
                    if (enemy.GetComponent<SpawnEnemyOnDeath>()) Destroy(enemy.GetComponent<SpawnEnemyOnDeath>());
                }
                if (enemy.IsTransmogrified) enemy.IsTransmogrified = false;
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
                ETGModConsole.Log(e.StackTrace);
            }
        }

        public override bool CanBeUsed(PlayerController user)
        {
            List<AIActor> activeEnemies = user.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            if (activeEnemies != null && activeEnemies.Count > 0) return true;
            else return false;
        }
    }
}
