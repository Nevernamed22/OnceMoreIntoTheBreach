using System;
using System.Collections.Generic;
using System.Linq;
using Dungeonator;
using System.Text;

using Alexandria.ItemAPI;
using UnityEngine;

namespace NevernamedsItems
{
    class GSwitch : PlayerItem
    {
        //Call this method from the Start() method of your ETGModule extension class
        public static void Init()
        {
            PlayerItem item = ItemSetup.NewItem<GSwitch>(
              "G-Switch",
              "Wahoo!",
              "Turns cash into a protective barrier." + "\n\nThis particular G-Switch once ruled a part of the Keep as a usurper, before eventually being dethroned from within it's G-Switch Palace.",
              "gswitch_icon") as PlayerItem;
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 1);
            item.consumable = false;
            item.quality = ItemQuality.D;
            GSwitchID = item.PickupObjectId;
        }
        public static int GSwitchID;
        // Token: 0x06007230 RID: 29232 RVA: 0x002D6104 File Offset: 0x002D4304
        public override void DoEffect(PlayerController user)
        {
            if (user.PlayerHasActiveSynergy("G is for Gain")) user.carriedConsumables.Currency -= 1;
            else user.carriedConsumables.Currency -= 3;

            MiscToolbox.SpawnShield(user, user.sprite.WorldCenter);
            if (user.PlayerHasActiveSynergy("Prototype Form")) DoSynergyShields(user.CurrentRoom, user);
        }
        private void DoSynergyShields(RoomHandler room, PlayerController user)
        {
            //ETGModConsole.Log("Did Synergy Shields");
            CurrencyPickup[] allShit = FindObjectsOfType<CurrencyPickup>();
            foreach (CurrencyPickup shit in allShit)
            {
                //ETGModConsole.Log("Found a casing");

                if (shit.transform.position.GetAbsoluteRoom() == room)
                {
                    //ETGModConsole.Log("Casing was in room");

                    MiscToolbox.SpawnShield(user, shit.sprite.WorldCenter);
                //ETGModConsole.Log("Spawned shield");
                }
            }
        }
        public override bool CanBeUsed(PlayerController user)
        {
            if (user.HasPickupID(376))
            {
                if (user.carriedConsumables.Currency >= 1) return true;
                else return false;
            }
            else
            {
                if (user.carriedConsumables.Currency >= 3) return true;               
                else return false;
            }
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }


    }
}