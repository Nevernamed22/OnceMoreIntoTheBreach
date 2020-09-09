using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;
using ItemAPI;

namespace NevernamedsItems
{
    class LiquidMetalBody : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Liquid-Metal Body";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/liquidmetalbody_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<LiquidMetalBody>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "1000";
            string longDesc = "Grants a liquid-metal state for a brief time upon taking damage."+"\n\nBlobulonian science, once used to formulate the terrifying Leadbulon, now modified by Professor Goopton to affect other lifeforms.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.B;
            item.AddToSubShop(ItemBuilder.ShopType.Trorc);
        }
        //float m_activeElapsed = 0f;
        float m_activeDuration;
        float duration = 3.5f;
        bool m_usedOverrideMaterial;

        private void DoLiquidEffect(PlayerController player)
        {
            PlayerController owner = base.Owner;
            StartCoroutine(HandleShield(Owner));

        }
        private IEnumerator HandleShield(PlayerController user)
        {
            //bool IsCurrentlyActive = true;
            //float m_activeElapsed = 0f;
            m_activeDuration = this.duration;
            m_usedOverrideMaterial = user.sprite.usesOverrideMaterial;
            user.sprite.usesOverrideMaterial = true;
            user.SetOverrideShader(ShaderCache.Acquire("Brave/ItemSpecific/MetalSkinShader"));
            SpeculativeRigidbody specRigidbody = user.specRigidbody;
            specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreCollision));
            user.healthHaver.IsVulnerable = false;
            float elapsed = 0f;
            while (elapsed < this.duration)
            {
                elapsed += BraveTime.DeltaTime;
                user.healthHaver.IsVulnerable = false;
                yield return null;
            }
            if (user)
            {
                user.healthHaver.IsVulnerable = true;
                user.ClearOverrideShader();
                user.sprite.usesOverrideMaterial = this.m_usedOverrideMaterial;
                SpeculativeRigidbody specRigidbody2 = user.specRigidbody;
                specRigidbody2.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Remove(specRigidbody2.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreCollision));
                //IsCurrentlyActive = false;
            }
            if (this)
            {
                AkSoundEngine.PostEvent("Play_OBJ_metalskin_end_01", base.gameObject);
            }
            yield break;
        }
        private void OnPreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherCollider)
        {
            Projectile component = otherRigidbody.GetComponent<Projectile>();
            if (component != null && !(component.Owner is PlayerController))
            {
                PassiveReflectItem.ReflectBullet(component, true, Owner.specRigidbody.gameActor, 10f, 1f, 1f, 0f);
                PhysicsEngine.SkipCollision = true;
            }
        }
        public override void Pickup(PlayerController player)
        {
            player.OnReceivedDamage += this.DoLiquidEffect;
            base.Pickup(player);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.OnReceivedDamage -= this.DoLiquidEffect;
            return base.Drop(player);
        }
        protected override void OnDestroy()
        {
            Owner.OnReceivedDamage -= this.DoLiquidEffect;
            base.OnDestroy();
        }
    }
}