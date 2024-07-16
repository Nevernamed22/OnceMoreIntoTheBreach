using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dungeonator;
using UnityEngine;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    class PurpleProse : PassiveItem
    {
        public static void Init()
        {
            PassiveItem item = ItemSetup.NewItem<PurpleProse>(
               "Purple Prose",
               "Pity Is A Powerful Weapon",
               "Charms all enemies in the room upon taking damage." + "\n\nA beautiful rose grown in some long lost floral garden deep within the gungeon. As you peel away it's petals, you find more inside, with seemingly no end. \n\nI guess you'll never find out if 'she loves you'.",
               "purpleprose_improved") as PassiveItem;
            item.quality = PickupObject.ItemQuality.D;
        }
        GameActorCharmEffect charmEffect = Gungeon.Game.Items["charming_rounds"].GetComponent<BulletStatusEffectItem>().CharmModifierEffect;
        private void charmAll(PlayerController user)
        {
            List<AIActor> activeEnemies = user.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            
            if (activeEnemies != null)
            {
                for (int i = 0; i < activeEnemies.Count; i++)
                {
                    AIActor aiactor = activeEnemies[i];
                    if (aiactor.IsNormalEnemy)
                    {                      
                        aiactor.gameActor.ApplyEffect(this.charmEffect, 1f, null);
                    }
                }
            }
        }    
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnReceivedDamage += this.charmAll;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.OnReceivedDamage -= this.charmAll;
            return debrisObject;
        }
        public override void OnDestroy()
        {
            if (Owner)
            {

            Owner.OnReceivedDamage -= this.charmAll;
            }
            base.OnDestroy();
        }
    }
}
