﻿using System;
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
            foreach (Component component in bullet.GetComponents<Component>())
            {
                ETGModConsole.Log(component.GetType().ToString());
            }
            bullet.OnHitEnemy += this.onHitEnemy;
            //bullet.specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(bullet.specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.HandlePreCollision));
        }
        public void onHitEnemy(Projectile bullet, SpeculativeRigidbody enemy, bool somethingthefuckidk)
        {
            //enemy.aiActor.Transmogrify(EnemyDatabase.GetOrLoadByGuid("cd4a4b7f612a4ba9a720b9f97c52f38c"), (GameObject)ResourceCache.Acquire("Global VFX/VFX_Item_Spawn_Poof"));
            /*if (enemy.gameObject.GetComponent<KeyBulletManController>())
            {
                ETGModConsole.Log("Enemy has a Keybullet Man Controller");
            }
            else if (!enemy.gameObject.GetComponent<KeyBulletManController>())
            {
                ETGModConsole.Log("Enemy has no Keybullet Man Controller!!!");
            }*/
        }
       /* private void HandlePreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
        {
            try
            {
                if (otherRigidbody.name != null)
                {
                    ETGModConsole.Log(otherRigidbody.name);
                }
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
                ETGModConsole.Log(e.StackTrace);
            }
        }*/
        public override void Pickup(PlayerController player)
        {
            player.PostProcessProjectile += this.onFired;
            base.Pickup(player);
        }
            //player.SetOverrideShader(ShaderCache.Acquire("Brave/LitCutoutUberPhantom"));
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject result = base.Drop(player);
            player.PostProcessProjectile -= this.onFired;
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