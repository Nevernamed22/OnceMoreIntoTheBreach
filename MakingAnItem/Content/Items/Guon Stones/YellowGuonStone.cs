using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Reflection;
using MonoMod.RuntimeDetour;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    class YellowGuonStone : AdvancedPlayerOrbitalItem
    {
        public static void Init()
        {
            AdvancedPlayerOrbitalItem item = ItemSetup.NewItem<YellowGuonStone>(
            "Yellow Guon Stone",
            "Yellowstone",
            "Grants brief invulnerability on killing an enemy." + "\n\nThe Yellow Guon handles defense, so that it's bearer may never stop attacking. At least, in theory.",
            "yellowguon_icon") as AdvancedPlayerOrbitalItem;

            item.quality = PickupObject.ItemQuality.B;
            item.SetTag("guon_stone");

            item.OrbitalPrefab = ItemSetup.CreateOrbitalObject("Yellow Guon Stone", "yellowguon_ingame", new IntVector2(9, 9), new IntVector2(-4, -5)).GetComponent<PlayerOrbital>();
        }


        public override void Pickup(PlayerController player)
        {
            player.OnAnyEnemyReceivedDamage += this.OnEnemyDamaged;
            base.Pickup(player);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.OnAnyEnemyReceivedDamage -= this.OnEnemyDamaged;
            return base.Drop(player);
        }
        public override void OnDestroy()
        {
            if (Owner) Owner.OnAnyEnemyReceivedDamage -= this.OnEnemyDamaged;
            base.OnDestroy();
        }
        private void OnEnemyDamaged(float damage, bool fatal, HealthHaver enemyHealth)
        {
            if (enemyHealth && fatal)
            {
                StartCoroutine(HandleShield(Owner));
            }
        }
        float m_activeDuration = 1f;
        float duration = 1f;
        bool m_usedOverrideMaterial;
        private IEnumerator HandleShield(PlayerController user)
        {
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
    }
}
