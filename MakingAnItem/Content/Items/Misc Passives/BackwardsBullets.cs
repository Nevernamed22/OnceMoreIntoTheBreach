﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    public class BackwardsBullets : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<BackwardsBullets>(
            "Backwards Bullets",
            "gnaB gnaB ytoohS",
            "...thgin ymrots dna dloc a no tnemirepxe cifirroh a fo tluser eht era stellub esehT" + "\n\n!sdrawkcab levart meht sekam osla tub, lufrewop erom stellub ruoy sekaM",
            "backwardsbullets_icon");
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Accuracy, 0.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Damage, 2f, StatModifier.ModifyMethod.MULTIPLICATIVE);

            item.quality = PickupObject.ItemQuality.C;
            item.SetTag("bullet_modifier");
            Doug.AddToLootPool(item.PickupObjectId);
        }
        public void ModifyVolley(ProjectileVolleyData volleyToModify)
        {
            int count = volleyToModify.projectiles.Count;
            for (int i = 0; i < count; i++)
            {
                ProjectileModule projectileModule = volleyToModify.projectiles[i];
                projectileModule.angleFromAim += 180;
            }
        }
        public override void Pickup(PlayerController player)
        {
            player.stats.AdditionalVolleyModifiers += this.ModifyVolley;
            player.stats.RecalculateStats(player, false, false);
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            player.stats.AdditionalVolleyModifiers -= this.ModifyVolley;
            player.stats.RecalculateStats(player, false, false);
            return base.Drop(player);
        }
        public override void OnDestroy()
        {
            if (Owner)
            {
                Owner.stats.AdditionalVolleyModifiers -= this.ModifyVolley;
                Owner.stats.RecalculateStats(Owner, false, false);
            }
            base.OnDestroy();
        }
    }
}
