using System;
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
    public class PickledPepper : AffectEnemiesInProximityTickItem
    {
        public static void Init()
        {
            AffectEnemiesInProximityTickItem item = ItemSetup.NewItem<PickledPepper>(
               "Pickled Pepper",
               "Picked In Pecks",
               "Poisons enemies who come too close. \n\nA Gungeon Pepper soaked in pickling brine until it turns a bright green colour. \n\nThe noxious vapour from this delicacy is enough to dissolve your foes from the inside out.",
               "pickledpepper_icon") as AffectEnemiesInProximityTickItem;          
            item.range = 3.5f;
            item.CanBeDropped = true;
            item.quality = PickupObject.ItemQuality.B;
        }
        public override void AffectEnemy(AIActor aiactor)
        {
            aiactor.ApplyEffect(StaticStatusEffects.irradiatedLeadEffect);
            base.AffectEnemy(aiactor);
        }
    }
}
