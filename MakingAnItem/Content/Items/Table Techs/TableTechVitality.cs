using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Reflection;
using MonoMod.RuntimeDetour;
using Alexandria.ItemAPI;
using Alexandria.Misc;

namespace NevernamedsItems
{
    public class TableTechVitality : TableFlipItem
    {
        public static void Init()
        {
            TableTechVitality item = ItemSetup.NewItem<TableTechVitality>(
                "Table Tech Vitality",
               "Flip Health",
               "Chance to conjure rejuvenation when a table is flipped"+ "\n\nPart one of the infamous \"Tablos Apocrypha\" of the \"Tabla Sutra\", legends say that this page is written in blood...",
               "tabletechvitality_icon") as TableTechVitality;
            item.quality = PickupObject.ItemQuality.B;
            item.TableFlocking = true;
            ID = item.PickupObjectId;
            item.SetTag("table_tech");
        }
        public static int ID;
        public override void Pickup(PlayerController player)
        {
            player.OnTableFlipped += MaybeDropHeart;
            base.Pickup(player);
        }
        public override void DisableEffect(PlayerController player)
        {
            if (player)
            {
                player.OnTableFlipped -= MaybeDropHeart;
            }
            base.DisableEffect(player);
        }
        private void MaybeDropHeart(FlippableCover obj)
        {
            if (UnityEngine.Random.value <= 0.1f)
            {
                if (UnityEngine.Random.value <= 0.3f)
                {
                    LootEngine.SpawnItem(PickupObjectDatabase.GetById(73).gameObject, obj.transform.position, Vector2.zero, 1f, false, true, false);
                }
                else
                {
                    LootEngine.SpawnItem(PickupObjectDatabase.GetById(85).gameObject, obj.transform.position, Vector2.zero, 1f, false, true, false);
                }
            }
        }        
    }
}