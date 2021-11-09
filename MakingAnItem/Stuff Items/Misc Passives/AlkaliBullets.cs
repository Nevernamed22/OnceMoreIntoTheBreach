﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;
using ItemAPI;
using SaveAPI;

namespace NevernamedsItems
{
    public class AlkaliBullets : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Alkali Bullets";
            string resourceName = "NevernamedsItems/Resources/alkalibullets_icon";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<AlkaliBullets>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Violent Reaction";
            string longDesc = "The alkali metals that make up these slugs react violently with the copious amounts of fluid present in Blobulonian creatures.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");
            item.quality = PickupObject.ItemQuality.A;
            item.AddToSubShop(ItemBuilder.ShopType.Goopton);
            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.PURCHASED_ALKALIBULLETS, true);
            item.AddItemToGooptonMetaShop(30);
            AlkaliBulletsID = item.PickupObjectId;
        }
        public static int AlkaliBulletsID;
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += this.PostProcessProjectile;
            player.PostProcessBeam += this.PostProcessBeam;
        }
        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {
            InstaKillEnemyTypeBehaviour instakill = sourceProjectile.gameObject.GetOrAddComponent<InstaKillEnemyTypeBehaviour>();
            instakill.EnemyTypeToKill.AddRange(EasyEnemyTypeLists.ModInclusiveBlobsALL);
        }
        private void PostProcessBeam(BeamController sourceBeam)
        {
            if (sourceBeam.projectile)
            {
                this.PostProcessProjectile(sourceBeam.projectile, 1);
            }
        }  
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.PostProcessProjectile -= this.PostProcessProjectile;
            player.PostProcessBeam -= this.PostProcessBeam;
            return debrisObject;
        }
        protected override void OnDestroy()
        {
            Owner.PostProcessProjectile -= this.PostProcessProjectile;
            Owner.PostProcessBeam -= this.PostProcessBeam;
            base.OnDestroy();
        }

    }
}