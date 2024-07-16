using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dungeonator;
using UnityEngine;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    class ClaySculpture : PassiveItem
    {
        public static void Init()
        {
            PassiveItem item = ItemSetup.NewItem<ClaySculpture>(
              "Clay Sculpture",
              "This Gungeon Has Gundead In It",
              "Upon taking damage, Gundead are consumed by the floor beneath them." + "\n\nThe foundations of the Gungeon are built on a special type of 12-gauge granular clay, which is the ABSOLUTE best for sculptures such as these." + "\nJust be careful... you don't want to catch anything...",
              "claysculpture_icon") as PassiveItem;
            item.quality = PickupObject.ItemQuality.C;
        }
        private void charmAll(PlayerController user)
        {
            AIActor randomEnemy = user.CurrentRoom.GetRandomActiveEnemy();
            if (randomEnemy != null && !randomEnemy.healthHaver.IsBoss) randomEnemy.ForceFall();
            if (Owner.HasPickupID(159))
            {
                AIActor randomEnemy2 = user.CurrentRoom.GetRandomActiveEnemy();
                if (randomEnemy2 != null && !randomEnemy2.healthHaver.IsBoss) randomEnemy2.ForceFall();
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
            if (Owner) Owner.OnReceivedDamage -= this.charmAll;
            base.OnDestroy();
        }
    }
}