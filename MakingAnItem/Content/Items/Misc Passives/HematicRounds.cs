﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;
using MonoMod.RuntimeDetour;
using System.Reflection;
using Dungeonator;
using SaveAPI;

namespace NevernamedsItems
{
    public class HematicRounds : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<HematicRounds>(
             "Hematic Rounds",
             "Blood... So Much Blood...",
             "Increases damage the more times it's bearer takes damage. Resets per room." + "\n\nThese red blood shells are sloshing with the good stuff.",
             "hematicrounds_icon");           
            item.quality = PickupObject.ItemQuality.C;
            item.SetTag("bullet_modifier");
            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.PURCHASED_HEMATICROUNDS, true);
            item.AddItemToDougMetaShop(50);
            Doug.AddToLootPool(item.PickupObjectId);
        }
        public override void Pickup(PlayerController player)
        {
            player.PostProcessProjectile += PostProj;
            player.PostProcessBeam += PostBeam;
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            player.PostProcessBeam -= PostBeam;
            player.PostProcessProjectile -= PostProj;
            return base.Drop(player);
        }
        private int timesHit = 0;
        private RoomHandler lastCheckedRoom;
        private void PostProj(Projectile proj, float i)
        {
            proj.baseData.damage *= (1 + (0.5f * timesHit));
        }
        private void PostBeam(BeamController b)
        {
            if (b.projectile)
            {
                b.projectile.baseData.damage *= (1 + (0.5f * timesHit));
            }
        }
        public override void Update()
        {
            if (Owner && Owner.CurrentRoom != null)
            {
                if (Owner.CurrentRoom != lastCheckedRoom)
                {
                    if (!(Owner.PlayerHasActiveSynergy("Blood Transfusion") && UnityEngine.Random.value <= 0.5f)) timesHit = 0;
                    lastCheckedRoom = Owner.CurrentRoom;
                }
            }
            base.Update();
        }
        public override void OnDestroy()
        {
            if (Owner)
            {
                Owner.PostProcessBeam -= PostBeam;
                Owner.PostProcessProjectile -= PostProj;
            }
            base.OnDestroy();
        }

    }
}
