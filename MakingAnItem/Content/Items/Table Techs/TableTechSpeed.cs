﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    public class TableTechSpeed : TableFlipItem
    {
        public static void Init()
        {
            TableFlipItem item = ItemSetup.NewItem<TableTechSpeed>(
              "Table Tech Speed",
              "Flip Acceleration",
              "Flipping a table increases the bearer's movement speed temporarily." + "\n\nAppendix F of the \"Tabla Sutra\". Flipping is to create motion. In motion there is life, and joy. To flip, is to live.",
              "tabletechspeed_icon") as TableFlipItem;
            item.quality = PickupObject.ItemQuality.D;
            item.SetTag("table_tech");
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnTableFlipped = (Action<FlippableCover>)Delegate.Combine(player.OnTableFlipped, new Action<FlippableCover>(this.SpeedEffect));
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject result = base.Drop(player);
            player.OnTableFlipped = (Action<FlippableCover>)Delegate.Remove(player.OnTableFlipped, new Action<FlippableCover>(this.SpeedEffect));
            return result;
        }
        public override void OnDestroy()
        {
            if (Owner)
            {
                Owner.OnTableFlipped -= SpeedEffect;
            }
            base.OnDestroy();
        }
        private void SpeedEffect(FlippableCover obj)
        {
            PlayerController owner = base.Owner;
            owner.PlayEffectOnActor(SharedVFX.SpeedUpVFX, new Vector3(0, 0.25f, 0), true, true);
            PlayerToolbox tools = owner.GetComponent<PlayerToolbox>();
            if (tools)
            {
                float time = 7f;
                if (owner.PlayerHasActiveSynergy("Sound Barrier")) time = 14f;
                tools.DoTimedStatModifier(PlayerStats.StatType.MovementSpeed, 2f, time, StatModifier.ModifyMethod.ADDITIVE);
            }
        }
    }
}
