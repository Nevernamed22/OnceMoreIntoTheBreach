﻿using System;
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
    public class EightButton : PassiveItem
    {
        public static void Init()
        {
            PassiveItem item = ItemSetup.NewItem<EightButton>(
              "88888888",
              "88888888",
              "8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888",
              "88888888_icon") as PassiveItem;
            item.quality = PickupObject.ItemQuality.C;
        }
        public int trackedShots = 0;
        private void PostProcessProjectile(Projectile bullet, float thing)
        {
            trackedShots++;
            if (trackedShots >= 8)
            {
            AkSoundEngine.PostEvent("Play_StanleyEight", Owner.gameObject);
                Projectile projectile = (PickupObjectDatabase.GetById(Sweeper.ID) as Gun).RawSourceVolley.projectiles[1].projectiles[7];
                GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, Owner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (Owner.CurrentGun == null) ? 0f : Owner.CurrentGun.CurrentAngle), true);
                Projectile component = gameObject.GetComponent<Projectile>();
                if (component != null)
                {
                    component.Owner = Owner;
                    component.Shooter = Owner.specRigidbody;
                    component.AdjustPlayerProjectileTint(Color.red, 1);
                }
                trackedShots = 0;
            }
        }
        public override void Pickup(PlayerController player)
        {
            player.PostProcessProjectile += PostProcessProjectile;
            base.Pickup(player);
            
        }
        public override DebrisObject Drop(PlayerController player)
        {
            player.PostProcessProjectile -= PostProcessProjectile;
            return base.Drop(player);
        }
        public override void OnDestroy()
        {
            if (Owner)
            {
                Owner.PostProcessProjectile -= PostProcessProjectile;
            }
            base.OnDestroy();
        }
    }
}

