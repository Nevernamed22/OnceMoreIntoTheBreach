﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Dungeonator;
using System.Collections.ObjectModel;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    public class TableTechAmmo : TableFlipItem
    {
        public static void Init()
        {
            string itemName = "Table Tech Ammo";
            string resourceName = "NevernamedsItems/Resources/tabletechammo_icon";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<TableTechAmmo>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Flip Replenishment";
            string longDesc = "Grants a small amount of ammo each time a table is flipped." + "\n\nChapter 10 of the \"Tabla Sutra.\" And he who flips shall never be hungry, for he will always have the flip within his heart.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");
            item.quality = PickupObject.ItemQuality.C;
            item.SetTag("table_tech");
            item.TableFlocking = true;
        }
        public override void Pickup(PlayerController player)
        {
            player.OnTableFlipped += GainAmmo;
            base.Pickup(player);     
        }
        public override void DisableEffect(PlayerController player)
        {
            player.OnTableFlipped -= GainAmmo;
            base.DisableEffect(player);
        }
        private void GainAmmo(FlippableCover obj)
        {
            int ammoToGive = UnityEngine.Random.Range(1, 5);
            if (Owner.PlayerHasActiveSynergy("Tabletop")) ammoToGive = UnityEngine.Random.Range(5, 10);
            Owner.CurrentGun.GainAmmo(ammoToGive);
        }           
    }
}