﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Reflection;
using MonoMod.RuntimeDetour;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    public class TableTechGuon : TableFlipItem
    {
        public static void Init()
        {
            TableFlipItem item = ItemSetup.NewItem<TableTechGuon>(
              "Table Tech Guon",
              "Spinflip",
              "This highly special spinning flip technique causes chunks of table to become detatched and orbit their creator in a guon stone form before disintegrating from sheer awe." + "\n\nChapter ??? of the 'Tabla Sutra'. A true flip does not protect only once, but many times after as well.",
              "tabletechguon_icon") as TableFlipItem;
            item.quality = PickupObject.ItemQuality.C;
            item.SetTag("table_tech");
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnTableFlipped += this.GiveWoodGuon;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject result = base.Drop(player);
            player.OnTableFlipped -= this.GiveWoodGuon;
            return result;
        }
        public override void OnDestroy()
        {
            if (Owner) Owner.OnTableFlipped -= this.GiveWoodGuon;
            base.OnDestroy();
        }
        private void GiveWoodGuon(FlippableCover obj)
        {
            PlayerController owner = base.Owner;
            var woodGuon = Gungeon.Game.Items["nn:wood_guon_stone"];
            owner.AcquirePassiveItemPrefabDirectly(woodGuon as PassiveItem);
        }
    }
}