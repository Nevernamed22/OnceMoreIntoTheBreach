﻿using System;
using System.Collections.Generic;
using System.Linq;
using Gungeon;
using System.Text;

using UnityEngine;
using ItemAPI;

namespace NevernamedsItems
{
    public class Nitroglycylinder : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<Nitroglycylinder>(
            "Nitroglycylinder",
            "Reloader 'sploder",
            "Explodes safely (for you at least) upon reloading." + "\n\nThis attatchment was favoured by a masochistic gungeoneer who liked the smell of burnt hair just a little too much. After his inevitable death, it was modified to not actually hurt it's bearer.",
            "nitroglycylinder_icon");
            item.quality = PickupObject.ItemQuality.D;
            item.AddToSubShop(ItemBuilder.ShopType.Trorc);

            List<string> mandatorySynergyItems = new List<string>() { "nn:nitroglycylinder" };
            List<string> optionalSynergyItems = new List<string>() { "nn:nitro_bullets", "explosive_rounds" };
            CustomSynergies.Add("Bomb Voyage!", mandatorySynergyItems, optionalSynergyItems);

            List<string> mandatorySynergyItems2 = new List<string>() { "nn:nitro_bullets" };
            List<string> optionalSynergyItems2 = new List<string>() { "nn:nitroglycylinder", "explosive_rounds", };
            CustomSynergies.Add("...Badda Boom!", mandatorySynergyItems2, optionalSynergyItems2);

            NitroglycylinderID = item.PickupObjectId;
        }
        public static int NitroglycylinderID;
        private void HandleGunReloaded(PlayerController player, Gun playerGun)
        {
            DoSafeExplosion(Owner.specRigidbody.UnitCenter);
            
        }
        ExplosionData smallPlayerSafeExplosion = new ExplosionData()
        {
            damageRadius = 2.5f,
            damageToPlayer = 0f,
            doDamage = true,
            damage = 25,
            doDestroyProjectiles = true,
            doForce = true,
            debrisForce = 30f,
            preventPlayerForce = true,
            explosionDelay = 0.1f,
            usesComprehensiveDelay = false,
            doScreenShake = true,
            playDefaultSFX = true,
        };
        ExplosionData bigPlayerSafeExplosion = new ExplosionData()
        {
            damageRadius = 4f,
            damageToPlayer = 0f,
            doDamage = true,
            damage = 50,
            doDestroyProjectiles = true,
            doForce = true,
            debrisForce = 60f,
            preventPlayerForce = true,
            explosionDelay = 0.1f,
            usesComprehensiveDelay = false,
            doScreenShake = true,
            playDefaultSFX = true,
        };

        public void DoSafeExplosion(Vector3 position)
        {
            if (Owner.HasPickupID(304) || Owner.HasPickupID(Gungeon.Game.Items["nn:nitro_bullets"].PickupObjectId))
            {
                var defaultExplosion = GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultSmallExplosionData;
                bigPlayerSafeExplosion.effect = defaultExplosion.effect;
                bigPlayerSafeExplosion.ignoreList = defaultExplosion.ignoreList;
                bigPlayerSafeExplosion.ss = defaultExplosion.ss;
                Exploder.Explode(position, bigPlayerSafeExplosion, Vector2.zero);
            }
            else
            {
                var defaultExplosion = GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultSmallExplosionData;
                smallPlayerSafeExplosion.effect = defaultExplosion.effect;
                smallPlayerSafeExplosion.ignoreList = defaultExplosion.ignoreList;
                smallPlayerSafeExplosion.ss = defaultExplosion.ss;
                Exploder.Explode(position, smallPlayerSafeExplosion, Vector2.zero);
            }
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnReloadedGun += this.HandleGunReloaded;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.OnReloadedGun -= this.HandleGunReloaded;
            return debrisObject;
        }
        public override void OnDestroy()
        {
            if (Owner) Owner.OnReloadedGun -= this.HandleGunReloaded;
            base.OnDestroy();
        }
    }

}