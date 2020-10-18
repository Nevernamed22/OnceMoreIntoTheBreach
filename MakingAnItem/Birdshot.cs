using System;
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
    public class Birdshot : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Birdshot";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/birdshot_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<Birdshot>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Angry";
            string longDesc = "+40% damage to flying enemies." + "\n\nA collection of tiny pellets enchanted with an inherent hatred for anything that dares to leave the ground." + "\nQuarantined in the Gungeon after several unfortunate incidents in an airport.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.B;

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
            if (otherRigidbody && otherRigidbody.healthHaver && otherRigidbody.healthHaver.aiActor)
            {
                if (otherRigidbody.aiActor.IsFlying)
                {
                    float originalDamage = myRigidbody.projectile.baseData.damage;
                    myRigidbody.projectile.baseData.damage *= 1.4f;
                    GameManager.Instance.StartCoroutine(this.ChangeProjectileDamage(myRigidbody.projectile, originalDamage));
                }
            }
        }
        private IEnumerator ChangeProjectileDamage(Projectile bullet, float oldDamage)
        {
            yield return new WaitForSeconds(0.1f);
            if (bullet != null)
            {
                bullet.baseData.damage = oldDamage;
            }
            yield break;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.PostProcessProjectile -= this.PostProcessProjectile;
            return debrisObject;
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += this.PostProcessProjectile;
        }
        protected override void OnDestroy()
        {
            if (Owner)
            {
                Owner.PostProcessProjectile -= this.PostProcessProjectile;
            }
            base.OnDestroy();
        }
    }
}
