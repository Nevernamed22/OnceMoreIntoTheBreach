using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Alexandria.ItemAPI;
using UnityEngine;
using MonoMod.RuntimeDetour;
using System.Reflection;

namespace NevernamedsItems
{
    public class AmmoGland : PassiveItem
    {
        public static void Init()
        {
            PassiveItem item = ItemSetup.NewItem<AmmoGland>(
              "Ammo Gland",
              "Relorganic",
              "Gestates ammo slowly over time."+"\n\nLeeches lead and copper from the users bloodstream to generate organic munitions. While it might not be vegan, you're probably better off with less lead in your blood.",
              "ammogland_icon") as PassiveItem;
            item.quality = PickupObject.ItemQuality.A;
        }
        public float timer = 0;

        public override void Update()
        {
            if (this.Owner != null && this.Owner.CurrentGun != null)
            {
                if (Owner.CurrentGun.IsFiring) { timer = 0; }
                if (timer > Interval)
                {
                    timer = 0;
                    this.Owner.CurrentGun.GainAmmo(1);
                }
                else
                {
                    timer += BraveTime.DeltaTime;
                }
            }
            base.Update();
        }
        public float Interval
        {
            get
            {
                return 1.5f;
            }
        }

    }
}

