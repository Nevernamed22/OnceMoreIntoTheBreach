using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using ItemAPI;

namespace NevernamedsItems
{
    public class HallowedBullets : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Hallowed Bullets";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/hallowedrounds_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<HallowedBullets>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Bullet Holeys";
            string longDesc = "These bullets are so blessed that they will preach to other projectiles mid-air. Usually this does nothing, but it does give Jammed projectiles a chance at redemption.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.D;

        }
        public void onFired(Projectile bullet, float eventchancescaler)
        {
            bullet.collidesWithProjectiles = true;
            bullet.BlackPhantomDamageMultiplier += 0.1f;
            SpeculativeRigidbody specRigidbody = bullet.specRigidbody;
            specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.HandlePreCollision));
        }
        private void onFiredBeam(BeamController sourceBeam)
        {

        }
        public override void Pickup(PlayerController player)
        {
            player.PostProcessProjectile += this.onFired;
            player.PostProcessBeam += this.onFiredBeam;
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject result = base.Drop(player);
            player.PostProcessProjectile -= this.onFired;
            player.PostProcessBeam -= this.onFiredBeam;
            return result;
        }
        protected override void OnDestroy()
        {
            Owner.PostProcessProjectile -= this.onFired;
            Owner.PostProcessBeam -= this.onFiredBeam;
            base.OnDestroy();
        }

        private void HandlePreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
        {
            if (otherRigidbody && otherRigidbody.projectile)
            {
                if (otherRigidbody.projectile.Owner is AIActor)
                {

                    if (otherRigidbody.projectile.IsBlackBullet)
                    {
                        otherRigidbody.projectile.ReturnFromBlackBullet();
                    }
                }
                PhysicsEngine.SkipCollision = true;
            }
        }
    }

}