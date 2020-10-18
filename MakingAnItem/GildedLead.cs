﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using ItemAPI;
using UnityEngine;
using MonoMod.RuntimeDetour;
using System.Reflection;

namespace NevernamedsItems
{
    public class GildedLead : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Gilded Lead";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/gildedlead_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<GildedLead>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Pays Off";
            string longDesc = "Chance to consume a casing when you fire a bullet. If a bullet consumes a casing it will have it's damage doubled." + "\nBuffed bullets that hit enemies will drop their casing onto the floor again. You miss, you lose out." + "\n\nBullets found scattered at the seat of the Charthurian Throne.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.B;

        }
        public static Color gold = new Color(230f / 255f, 174f / 255f, 21f / 255f);
        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {
            if (Owner.carriedConsumables.Currency > 0)
            {
                float procChance;
                if (Owner.HasPickupID(10)) procChance = 0.5f;
                else procChance = 0.1f;
                if (UnityEngine.Random.value < procChance * effectChanceScalar)
                {
                    Owner.carriedConsumables.Currency -= 1;
                    float damageMult = 2f;
                    if (Owner.HasPickupID(532)) damageMult = 3f;
                    sourceProjectile.baseData.damage *= damageMult;
                    sourceProjectile.AdjustPlayerProjectileTint(gold, 1, 0f);
                    sourceProjectile.OnHitEnemy += this.OnHitEnemy;
                }
            }
        }
        private void OnHitEnemy(Projectile bullet, SpeculativeRigidbody enemy, bool fuckingsomethingidk)
        {
            if (bullet != null && bullet.specRigidbody != null)
            {
                LootEngine.SpawnItem(PickupObjectDatabase.GetById(68).gameObject, bullet.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
                bullet.OnHitEnemy -= this.OnHitEnemy;
            }
        }

        private void PostProcessBeam(BeamController beam)
        {
            if (Owner.CurrentGun.PickupObjectId == 10)
            {
                beam.AdjustPlayerBeamTint(gold, 1, 0f);
            }
        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.PostProcessProjectile -= this.PostProcessProjectile;
            player.PostProcessBeam -= this.PostProcessBeam;
            return debrisObject;
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += this.PostProcessProjectile;
            player.PostProcessBeam += this.PostProcessBeam;
        }
        protected override void OnDestroy()
        {
            if (Owner)
            {
                Owner.PostProcessProjectile -= this.PostProcessProjectile;
                Owner.PostProcessBeam -= this.PostProcessBeam;
            }
            base.OnDestroy();
        }
    }
}
