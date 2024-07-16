using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alexandria.ItemAPI;
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
            ListComponents(bullet.gameObject);
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

        public static void ListComponents(GameObject obj, int defaultIndentationLevel = 0)
        {
            string indent = "";
            for (int i = 0; i < defaultIndentationLevel; i++) { indent += "    "; }
            foreach (Component component in obj.GetComponentsInChildren<Component>())
            {
                ETGModConsole.Log(indent + component.GetType().ToString());
                if (component is Projectile)
                {
                    Projectile cont = (component as Projectile);
                    ETGModConsole.Log($"<color=#ff0000ff>{indent}    DestroyMode: </color>{cont.DestroyMode}");                   
                }
                if (component is TrailController)
                {
                    TrailController cont = (component as TrailController);
                    ETGModConsole.Log($"<color=#ff0000ff>{indent}    usesStartAnimation: </color>{cont.usesStartAnimation}");
                    ETGModConsole.Log($"<color=#ff0000ff>{indent}    usesAnimation: </color>{cont.usesAnimation}");
                    ETGModConsole.Log($"<color=#ff0000ff>{indent}    usesCascadeTimer: </color>{cont.usesCascadeTimer}");
                    ETGModConsole.Log($"<color=#ff0000ff>{indent}    cascadeTimer: </color>{cont.cascadeTimer}");
                    ETGModConsole.Log($"<color=#ff0000ff>{indent}    usesSoftMaxLength: </color>{cont.usesSoftMaxLength}");
                    ETGModConsole.Log($"<color=#ff0000ff>{indent}    softMaxLength: </color>{cont.softMaxLength}");
                    ETGModConsole.Log($"<color=#ff0000ff>{indent}    usesGlobalTimer: </color>{cont.usesGlobalTimer}");
                    ETGModConsole.Log($"<color=#ff0000ff>{indent}    globalTimer: </color>{cont.globalTimer}");
                    ETGModConsole.Log($"<color=#ff0000ff>{indent}    destroyOnEmpty: </color>{cont.destroyOnEmpty}");
                }
                if (component is StrafeBleedBuff)
                {
                    StrafeBleedBuff bleedBuff = component as StrafeBleedBuff;
                    if (bleedBuff.vfx)
                    {
                        ETGModConsole.Log($"<color=#ff0000ff>{indent}    VFX Object</color>");
                        ListComponents(bleedBuff.vfx, 1);
                    }
                }
                if (component is HealthHaver)
                {
                    ETGModConsole.Log($"<color=#ff0000ff>{indent}    Max HP: </color>{(component as HealthHaver).GetMaxHealth()}");
                    ETGModConsole.Log($"<color=#ff0000ff>{indent}    BossBarType: </color>{(component as HealthHaver).bossHealthBar}");
                }
                if (component is AIActor)
                {
                    ETGModConsole.Log($"<color=#ff0000ff>{indent}    Normal Enemy: </color>{(component as AIActor).IsNormalEnemy}");
                }
                if (component is BehaviorSpeculator)
                {
                    ETGModConsole.Log($"<color=#ff0000ff>{indent}    Attack Behaviours</color>");
                    foreach(AttackBehaviorBase behavs in (component as BehaviorSpeculator).AttackBehaviors)
                    {
                    ETGModConsole.Log($"{indent}        {behavs.GetType().ToString()}");

                    }
                }
                if (component is MinorBreakable)
                { 
                    ETGModConsole.Log($"<color=#ff0000ff>{indent}    BreakAnimName: </color>{(component as MinorBreakable).breakAnimName}");
                }
            }
            ETGModConsole.Log("<color=#ff0000ff>---------------------------------</color>");
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
                BulletComponentLister.ListComponents(data.OtherRigidbody.gameObject);
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
