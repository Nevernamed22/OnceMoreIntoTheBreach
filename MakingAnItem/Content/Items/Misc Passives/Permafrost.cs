using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;
using Dungeonator;

namespace NevernamedsItems
{
    public class Permafrost : PassiveItem
    {
        public static void Init()
        {
            PassiveItem item = ItemSetup.NewItem<Permafrost>(
              "Permafrost",
              "Cold Snap",
              "This vengeful spirit brings with it the terrifying chill of oblivion." + "\n\nUse it wisely, and do not disrespect it.",
              "permafrost_icon") as PassiveItem;       
            item.AddPassiveStatModifier(PlayerStats.StatType.Curse, 2f, StatModifier.ModifyMethod.ADDITIVE);
            item.quality = PickupObject.ItemQuality.S;
            item.AddToSubShop(ItemBuilder.ShopType.Cursula);
        }
        public void onEnteredCombat()
        {
            try
            {
                //ETGModConsole.Log("OnEnteredCombat was Triggered");
                List<AIActor> activeEnemies = Owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
                if (activeEnemies != null)
                {
                    foreach (AIActor aiactor in activeEnemies)
                    {
                        if (aiactor.IsNormalEnemy)
                        {
                            //ETGModConsole.Log("There are active eligible enemies!");
                            float freezeAmount = 0;
                            if (aiactor.healthHaver.IsBoss) freezeAmount = 100;
                            else freezeAmount = 150;
                            ApplyDirectStatusEffects.ApplyDirectFreeze(aiactor.gameActor, 3, freezeAmount, StaticStatusEffects.chaosBulletsFreeze.UnfreezeDamagePercent, ExtendedColours.freezeBlue, ExtendedColours.freezeBlue, EffectResistanceType.None, "Permafrost", true, true);
                        }
                    }
                }
                else
                {
                    //ETGModConsole.Log("Active enemies is NULL!");
                }
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
                ETGModConsole.Log(e.StackTrace);
            }
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnEnteredCombat += this.onEnteredCombat;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.OnEnteredCombat -= this.onEnteredCombat;
            return debrisObject;
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}