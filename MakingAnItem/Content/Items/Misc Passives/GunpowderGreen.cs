﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.Misc;
using Dungeonator;

namespace NevernamedsItems
{
    class GunpowderGreen : PassiveItem
    {
        public static void Init()
        {
            PassiveItem item = ItemSetup.NewItem<GunpowderGreen>(
              "Gunpowder Green",
              "Soothed Trigger Finger",
              "Reload faster the more full the clip already is." + "\n\nIntroduces a certain zen into the process of reloading, allowing you to refill your guns more effectively.",
              "gunpowdergreen_icon") as PassiveItem;          
            item.quality = PickupObject.ItemQuality.D;
            ID = item.PickupObjectId;
        }
        public static int ID;

        public float clipPercentLastFrame;
        public override void Update()
        {
            if (Owner && Owner.CurrentGun)
            {
                float clipPercent = (float)Owner.CurrentGun.ClipShotsRemaining / (float)Owner.CurrentGun.ClipCapacity;
                if (clipPercent != clipPercentLastFrame)
                {
                    Recalculate(clipPercent);
                    clipPercentLastFrame = clipPercent;
                }
            }
            base.Update();
        }
        private void Recalculate(float clipPercent)
        {
            //ETGModConsole.Log("Clip Percent: " + clipPercent) ;
            this.RemovePassiveStatModifier(PlayerStats.StatType.ReloadSpeed);
            if (clipPercent < 1f)
            {
                float invertedClip = 1f - clipPercent;
            //ETGModConsole.Log("Inverted Clip Percent: " + invertedClip) ;
                this.AddPassiveStatModifier(PlayerStats.StatType.ReloadSpeed, Mathf.Max(invertedClip, 0.05f), StatModifier.ModifyMethod.MULTIPLICATIVE);
                if (Owner) { Owner.stats.RecalculateStats(Owner); }
            }
        }
    }
}