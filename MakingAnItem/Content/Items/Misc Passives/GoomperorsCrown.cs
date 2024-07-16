using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;
using Dungeonator;
using SaveAPI;

namespace NevernamedsItems
{
    public class GoomperorsCrown : PassiveItem
    {
        public static void Init()
        {
            PassiveItem item = ItemSetup.NewItem<GoomperorsCrown>(
              "Goomperors Crown",
              "The Slime Must Flow",
              "The crown of the ancient Blobulonian emperor Gool Atinous." + "\n\nChance to slow down entire rooms!",
              "goomperorscrown_icon") as PassiveItem;         
            item.quality = PickupObject.ItemQuality.S;
            item.AddToSubShop(ItemBuilder.ShopType.Goopton);
            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.PURCHASED_GOOMPERORSCROWN, true);
            item.AddItemToGooptonMetaShop(30);
        }
        public void AIActorMods(AIActor target)
        {
            if (ShouldSlowThisRoom)
            {
                if (target && target.aiActor && target.aiActor.EnemyGuid != null)
                {
                    ApplyDirectStatusEffects.ApplyDirectSlow(target, 10000000000f, 0.75f, Color.white, Color.white, EffectResistanceType.None, "Goomperors Crown", false, false);
                }
            }
        }
        public bool ShouldSlowThisRoom;
        public void onEnteredCombat()
        {
            if (UnityEngine.Random.value < 0.25f) ShouldSlowThisRoom = true;
            else ShouldSlowThisRoom = false;
            if (ShouldSlowThisRoom)
            {
                List<AIActor> activeEnemies = Owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
                if (activeEnemies != null)
                {
                    for (int i = 0; i < activeEnemies.Count; i++)
                    {
                        AIActor aiactor = activeEnemies[i];
                        if (aiactor.IsNormalEnemy)
                        {
                            ApplyDirectStatusEffects.ApplyDirectSlow(aiactor.gameActor, 10000000000f, 0.75f, Color.white, Color.white, EffectResistanceType.None, "Goomperors Crown", false, false);
                        }
                    }
                }
            }
        }
        private void onRoomCleared(PlayerController player)
        {
            ShouldSlowThisRoom = false;
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            ETGMod.AIActor.OnPreStart += AIActorMods;
            player.OnEnteredCombat += this.onEnteredCombat;
            player.OnRoomClearEvent += this.onRoomCleared;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            ETGMod.AIActor.OnPreStart -= AIActorMods;
            player.OnEnteredCombat -= this.onEnteredCombat;
            player.OnRoomClearEvent -= this.onRoomCleared;
            return debrisObject;
        }
        public override void OnDestroy()
        {
            ETGMod.AIActor.OnPreStart -= AIActorMods;
            base.OnDestroy();
        }
    }
}
