using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;
using Gungeon;

namespace NevernamedsItems
{
    class BulletComponentLister : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Bullet Component Lister";
            string resourceName = "NevernamedsItems/Resources/workinprogress_icon";
            GameObject obj = new GameObject();
            var item = obj.AddComponent<BulletComponentLister>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);


            string shortDesc = "wip";
            string longDesc = "Lists the components present in fired projectiles.";


            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            item.quality = PickupObject.ItemQuality.EXCLUDED;
            item.CanBeDropped = true;
            item.CanBeSold = true;
        }
        public void onFired(Projectile bullet, float eventchancescaler)
        {
            foreach (Component component in bullet.GetComponents<Component>())
            {
                ETGModConsole.Log(component.GetType().ToString());
            }
            ETGModConsole.Log("<color=#ff0000ff>---------------------------------</color>");
        }
        public override void Pickup(PlayerController player)
        {
            player.PostProcessProjectile += this.onFired;
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject result = base.Drop(player);
            player.PostProcessProjectile -= this.onFired;
            return result;
        }

    }
    class ObjectComponentLister : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Object Component Lister";
            string resourceName = "NevernamedsItems/Resources/workinprogress_icon";
            GameObject obj = new GameObject();
            var item = obj.AddComponent<ObjectComponentLister>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);


            string shortDesc = "wip";
            string longDesc = "Lists the components present in objects that you shoot.";


            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            item.quality = PickupObject.ItemQuality.EXCLUDED;
            item.CanBeDropped = true;
            item.CanBeSold = true;
        }
        public void onFired(Projectile bullet, float eventchancescaler)
        {
            bullet.specRigidbody.OnCollision += this.onHitEnemy;
        }
        public void onHitEnemy(CollisionData data)
        {
            if (data.OtherRigidbody && data.OtherRigidbody.gameObject)
            {
                foreach (Component component in data.OtherRigidbody.gameObject.GetComponents<Component>())
                {
                    if (component != null)
                    {

                        ETGModConsole.Log(component.GetType().ToString());
                    }
                }
                ETGModConsole.Log("<color=#ff0000ff>---------------------------------</color>");
            }
        }
        public override void Pickup(PlayerController player)
        {
            player.PostProcessProjectile += this.onFired;
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject result = base.Drop(player);
            player.PostProcessProjectile -= this.onFired;
            return result;
        }

    }
}
