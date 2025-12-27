using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;
using Dungeonator;

namespace NevernamedsItems
{
    public class Showdown : PassiveItem
    {
        public static void Init()
        {
            PassiveItem item = ItemSetup.NewItem<Showdown>(
              "Showdown",
              "Now it's just you and me...",
              "Prevents bosses from being able to spawn additional backup." + "\n\nAn icon of the one-on-one gunfights of days gone by.",
              "showdown_improved") as PassiveItem;        
            item.quality = PickupObject.ItemQuality.D; 
        }
        public void AIActorMods(AIActor target)
        {
            if (!target.healthHaver.IsBoss)
            {
                if (target.GetAbsoluteParentRoom().area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.BOSS && (!target.IsHarmlessEnemy || target.EnemyGuid == GUIDs.Mine_Flayer_Claymore) && target.GetAbsoluteParentRoom().IsSealed)
                {
                    if (Owner.PlayerHasActiveSynergy("Frenemies"))
                    {
                        target.ApplyEffect(GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultPermanentCharmEffect, 1f, null);
                        target.IsHarmlessEnemy = true;
                    }
                    else
                    {
                        target.EraseFromExistenceWithRewards(true);
                    }
                    if (Owner.PlayerHasActiveSynergy("Dirty Tricks"))
                    {
                        List<AIActor> activeEnemies = target.GetAbsoluteParentRoom().GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
                        if (activeEnemies != null)
                        {
                            for (int i = 0; i < activeEnemies.Count; i++)
                            {
                                AIActor aiactor = activeEnemies[i];
                                if (aiactor.healthHaver.IsBoss)
                                {
                                    aiactor.healthHaver.ApplyDamage(50, Vector2.zero, "Dirty Tricks", CoreDamageTypes.None, DamageCategory.Unstoppable, true, null, true);
                                }
                            }
                        }
                    }
                }
            }
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            ETGMod.AIActor.OnPreStart += AIActorMods;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            ETGMod.AIActor.OnPreStart -= AIActorMods;
            return debrisObject;
        }
        public override void OnDestroy()
        {
            ETGMod.AIActor.OnPreStart -= AIActorMods;
            base.OnDestroy();
        }
    }
}

