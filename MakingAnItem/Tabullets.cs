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
    public class Tabullets : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Tabullets";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/tabullets_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<Tabullets>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Surface Level";
            string longDesc = "Your bullets no longer damage tables, and are allowed to pass right through. Passing through a table increases a bullet's damage."+"\n\nAn initiation gift among the Knights of the Octagonal Table.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.C;

        }

        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {
            try
            {
                sourceProjectile.specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(sourceProjectile.specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.HandlePreCollision));
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
            }
        }
        private void HandlePreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
        {
            if (otherRigidbody.gameObject.name != null)
            {
                if (otherRigidbody.gameObject.name == "Table_Vertical" || otherRigidbody.gameObject.name == "Table_Horizontal")
                {
                    myRigidbody.projectile.baseData.damage *= 1.05f;
                    PhysicsEngine.SkipCollision = true;
                }
            }
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.PostProcessProjectile -= this.PostProcessProjectile;
            return debrisObject;
        }
        protected override void OnDestroy()
        {
            Owner.PostProcessProjectile -= this.PostProcessProjectile;
            base.OnDestroy();
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += this.PostProcessProjectile;
        }
    }
}