using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using ItemAPI;

namespace NevernamedsItems
{
    public class NNGundertale : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Gundertale";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/legboot_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<NNGundertale>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Knock It Off";
            string longDesc = "From a peculiar and less graphically appealing alternate dimension where dodge rolling through bullets or into enemies restores ammo." + "\n\nThis boot is as long as your entire leg!";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.EXCLUDED; //S

            //Synergy with the Balloon Gun --> Double Radius.
            //Synergy with Armour of Thorns --> Deal damage to all enemies pushed. dam = dodgerolldam * 3.
        }


        private void onDodgeRolledOverBullet(Projectile bullet)
        {
            float procChance;
            if (Owner.HasPickupID(394)) procChance = 0.7f;
            else procChance = 0.4f;
            if (bullet.Owner && bullet.Owner is AIActor)
            {
                if (UnityEngine.Random.value < 1 /*procChance*/) MakeEnemyNPC(bullet.Owner.aiActor);
            }
        }
        private void onDodgeRolledIntoEnemy(PlayerController player, AIActor enemy)
        {
            enemy.healthHaver.flashesOnDamage = false;
            enemy.healthHaver.RegenerateCache();
            float procChance;
            if (Owner.HasPickupID(394)) procChance = 0.7f;
            else procChance = 0.4f;
            if (enemy && enemy is AIActor)
            {
                if (UnityEngine.Random.value < 1 /*procChance*/) MakeEnemyNPC(enemy.aiActor);
            }
        }
        private void MakeEnemyNPC(AIActor enemy)
        {
            var CurrentRoom = enemy.transform.position.GetAbsoluteRoom();
            CurrentRoom.DeregisterEnemy(enemy);
            if (enemy.specRigidbody)
            {
                UnityEngine.Object.Destroy(enemy.specRigidbody);
            }
            if (enemy.behaviorSpeculator)
            {
                UnityEngine.Object.Destroy(enemy.behaviorSpeculator);
            }
            if (enemy.healthHaver)
            {
                UnityEngine.Object.Destroy(enemy.healthHaver);
            }                       
            if (enemy.aiAnimator)
            {
                enemy.aiAnimator.PlayUntilCancelled("idle", false, null, -1f, false);
            }           
        }
        public override void Pickup(PlayerController player)
        {
            player.OnDodgedProjectile += this.onDodgeRolledOverBullet;
            player.OnRolledIntoEnemy += this.onDodgeRolledIntoEnemy;
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject result = base.Drop(player);
            player.OnDodgedProjectile -= this.onDodgeRolledOverBullet;
            player.OnRolledIntoEnemy -= this.onDodgeRolledIntoEnemy;
            return result;
        }
        protected override void OnDestroy()
        {
            Owner.OnDodgedProjectile -= this.onDodgeRolledOverBullet;
            Owner.OnRolledIntoEnemy -= this.onDodgeRolledIntoEnemy;
            base.OnDestroy();
        }
    }
}