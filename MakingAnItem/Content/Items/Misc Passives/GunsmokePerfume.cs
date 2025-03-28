﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;
using MonoMod.RuntimeDetour;
using System.Reflection;
using Dungeonator;

namespace NevernamedsItems
{
    public class GunsmokePerfume : AffectEnemiesInProximityTickItem
    {
        public static void Init()
        {
            AffectEnemiesInProximityTickItem item = ItemSetup.NewItem<GunsmokePerfume>(
              "Gunsmoke Perfume",
              "Ode To Glock 42",
              "Charms enemies who get too close." + "\n\nThe enticing aroma of a battle hardened gunslinger!",
              "gunsmokeperfume_icon") as AffectEnemiesInProximityTickItem;
            item.range = 2;
            item.rangeMultSynergy = "Practically Pungent";
            item.synergyRangeMult = 2;
            item.CanBeDropped = true;
            item.quality = PickupObject.ItemQuality.B;
        }
        public override void AffectEnemy(AIActor aiactor)
        {
            aiactor.ApplyEffect(StaticStatusEffects.charmingRoundsEffect);
            if (Owner.PlayerHasActiveSynergy("Regular Hottie")) aiactor.ApplyEffect(StaticStatusEffects.hotLeadEffect);
            base.AffectEnemy(aiactor);
        }
    }
}
