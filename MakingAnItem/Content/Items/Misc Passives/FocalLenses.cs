using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Alexandria.ItemAPI;
using UnityEngine;
using MonoMod.RuntimeDetour;
using System.Reflection;
using SaveAPI;

namespace NevernamedsItems
{
    public class FocalLenses : PassiveItem
    {
        public static void Init()
        {
            PassiveItem item = ItemSetup.NewItem<FocalLenses>(
              "Focal Lenses",
              "Better Beams",
              "Doubles beam damage." + "\n\nA set of lenses capable of increasing the firepower of laserbeams... and the waterpower of the Mega Douser, somehow.",
              "focallenses_icon") as PassiveItem;
            item.quality = PickupObject.ItemQuality.D;
            item.AddPassiveStatModifier( PlayerStats.StatType.Accuracy, 0.9f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        private void PostProcessBeam(BeamController beam)
        {
            if (beam && beam.projectile)
            {
                beam.projectile.baseData.damage *= 2;
            }
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.PostProcessBeam -= this.PostProcessBeam;
            DebrisObject debrisObject = base.Drop(player);


            return debrisObject;
        }
        public override void Pickup(PlayerController player)
        {
            player.PostProcessBeam += this.PostProcessBeam;
            base.Pickup(player);
        }
        public override void OnDestroy()
        {
            if (Owner)
            {
                Owner.PostProcessBeam -= this.PostProcessBeam;
            }
            base.OnDestroy();
        }
    }
}
