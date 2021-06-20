using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;
using Gungeon;

namespace NevernamedsItems
{
    class PassiveTestingItem : PassiveItem
    {
        public static void Init()
        {
            string itemName = "PassiveTestingItem";
            string resourceName = "NevernamedsItems/Resources/workinprogress_icon";
            GameObject obj = new GameObject();
            var item = obj.AddComponent<PassiveTestingItem>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);


            string shortDesc = "wip";
            string longDesc = "Did you seriously give yourself a testing item just to read the flavour text?";


            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            item.quality = PickupObject.ItemQuality.EXCLUDED;
            item.sprite.IsPerpendicular = true;
            item.CanBeDropped = true;
            item.CanBeSold = true;
        }
        //bullet.BecomeBlackBullet();
        //bullet.PlayerKnockbackForce = 100f;
        //bullet.AppliesKnockbackToPlayer = true;
        //JamPlayerBulletModifier jamProjectileModifier = bullet.gameObject.AddComponent<JamPlayerBulletModifier>();
        public void onFired(Projectile bullet, float eventchancescaler)
        {

           // bullet.statusEffectsToApply.Add(StaticStatusEffects.greenFireEffect);
           // bullet.collidesWithProjectiles = true;
            //bullet.OnHitEnemy += this.onHitEnemy;
            //bullet.specRigidbody.OnCollision += this.onCollision;
            //bullet.specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(bullet.specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.HandlePreCollision));
        }
        public void onHitEnemy(Projectile bullet, SpeculativeRigidbody enemy, bool somethingthefuckidk)
        {
            if (enemy && enemy.aiActor && enemy.aiActor.EnemyGuid != null)
            {
                ETGModConsole.Log(enemy.aiActor.EnemyGuid);
            }
        }
        public void onCollision(CollisionData data)
        {
            if (data.OtherRigidbody != null)
            {
                foreach (Component component in data.OtherRigidbody.gameObject.GetComponentsInChildren<Component>())
                {
                    if (component != null)
                    {
                        ETGModConsole.Log(component.GetType().ToString());
                    }
                }
                Projectile bulletness = data.OtherRigidbody.gameObject.GetComponentInChildren<Projectile>();
                if (bulletness != null)
                {
                ETGModConsole.Log("<color=#ff0000ff>Projectile Bools:</color>");
                    ETGModConsole.Log("CollidesWithProjectiles: " + bulletness.collidesWithProjectiles);
                    ETGModConsole.Log("CollidesOnlyWithPlayerProjectiles: " + bulletness.collidesOnlyWithPlayerProjectiles);
                    ETGModConsole.Log("CollidesWithEnemies: " + bulletness.collidesWithEnemies);
                    ETGModConsole.Log("CollidesWithPlayer: " + bulletness.collidesWithPlayer);
                }

                ETGModConsole.Log("<color=#ff0000ff>---------------------------------</color>");
            }
        }
        private void Roll(PlayerController playa)
        {

        }
        private void RollOverBullet(Projectile projectile)
        {
            foreach (Component component in projectile.gameObject.GetComponentsInChildren<Component>())
            {
                if (component != null)
                {
                    ETGModConsole.Log(component.GetType().ToString());
                }
            }
        }
        private void PostProcessBeam(BeamController beam)
        {
            if (beam && beam.gameObject)
            {
                beam.gameObject.GetOrAddComponent<WanderingBeamComp>();
            }
        }
        public override void Pickup(PlayerController player)
        {
            player.PostProcessProjectile += this.onFired;
            player.PostProcessBeam += this.PostProcessBeam;
            player.OnPreDodgeRoll += this.Roll;
            player.OnDodgedProjectile += this.RollOverBullet;
            base.Pickup(player);
        }
        //player.SetOverrideShader(ShaderCache.Acquire("Brave/LitCutoutUberPhantom"));
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject result = base.Drop(player);
            player.PostProcessProjectile -= this.onFired;
            player.PostProcessBeam -= this.PostProcessBeam;
            player.OnDodgedProjectile -= this.RollOverBullet;

            return result;
        }

    }
    public class JamPlayerBulletModifier : MonoBehaviour
    {
        private void Awake()
        {
            this.m_projectile = base.GetComponent<Projectile>();
        }
        private void Update()
        {
            if (!m_projectile.IsBlackBullet)
            {
                m_projectile.BecomeBlackBullet();
            }
        }
        private Projectile m_projectile;
    }
}