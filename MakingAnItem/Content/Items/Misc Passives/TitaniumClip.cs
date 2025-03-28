﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;
using MonoMod.RuntimeDetour;
using System.Reflection;

namespace NevernamedsItems
{
    public class TitaniumClip : PassiveItem
    {
        public static void Init()
        {
            PassiveItem item = ItemSetup.NewItem<TitaniumClip>(
              "Titanium Clip",
              "Damage Output",
              "Doubles damage of non-infinite ammo guns, but also doubles their ammo consumption." + "\n\nCreated to aid the greedy and shortsighted." + "\n\nTechnically, this is a magazine and not a clip, but I really don't care.",
              "titaniumclip_icon") as PassiveItem;        
            item.CanBeDropped = true;
            item.quality = PickupObject.ItemQuality.B;
            TitaniumClipID = item.PickupObjectId;
        }
        public static int TitaniumClipID;
        public void ModifyVolley(ProjectileVolleyData volleyToModify)
        {
            int count = volleyToModify.projectiles.Count;
            for (int i = 0; i < count; i++)
            {
                ProjectileModule projectileModule = volleyToModify.projectiles[i];
                projectileModule.ammoCost *= 2;

            }
        }
        private void RemoveBuff()
        {
            Owner.stats.AdditionalVolleyModifiers -= this.ModifyVolley;
            this.RemovePassiveStatModifier(PlayerStats.StatType.Damage);
            Owner.stats.RecalculateStats(Owner, false, false);
            buffActive = false;
        }
        private void AddBuff()
        {
            Owner.stats.AdditionalVolleyModifiers += this.ModifyVolley;
            this.AddPassiveStatModifier( PlayerStats.StatType.Damage, 2, StatModifier.ModifyMethod.MULTIPLICATIVE);
            Owner.stats.RecalculateStats(Owner, false, false);
            buffActive = true;
        }
        private bool buffActive;
        public override void Update()
        {
            if (Owner)
            {
                if (Owner.CurrentGun && !Owner.CurrentGun.InfiniteAmmo)
                {
                    if (!buffActive) { AddBuff(); }
                }
                else if (buffActive) { RemoveBuff(); }
            }
            base.Update();
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            RemoveBuff();
            return base.Drop(player);
        }
        public override void OnDestroy()
        {
            if (Owner)
            {
                RemoveBuff();
            }
            base.OnDestroy();
        }
    }
}