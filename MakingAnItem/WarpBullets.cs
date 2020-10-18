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
    public class WarpBullets : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Warp Bullets";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/warpbullets_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<WarpBullets>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Bullets Teleport";
            string longDesc = "Your bullets have a chance to teleport behind enemies.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.C;

        }

        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {
            float procChance = 0.20f;
            if (Owner.PlayerHasActiveSynergy("Corrupted By Warp")) procChance *= effectChanceScalar;
            try
            {
                if (UnityEngine.Random.value <= procChance)
                {
                    GameObject BaseEnemyRogueBullet = EnemyDatabase.GetOrLoadByGuid("56fb939a434140308b8f257f0f447829").bulletBank.GetBullet("rogue").BulletObject;
                    Projectile BaseEnemyProjectileComponent = BaseEnemyRogueBullet.GetComponent<Projectile>();
                    if (BaseEnemyProjectileComponent != null)
                    {
                        TeleportProjModifier tp = BaseEnemyProjectileComponent.GetComponent<TeleportProjModifier>();
                        if (tp != null)
                        {
                            PlayerProjectileTeleportModifier rogueTeleport = sourceProjectile.gameObject.AddComponent<PlayerProjectileTeleportModifier>();
                            rogueTeleport.teleportVfx = tp.teleportVfx;
                            rogueTeleport.teleportCooldown = tp.teleportCooldown;
                            rogueTeleport.teleportPauseTime = tp.teleportPauseTime;
                            rogueTeleport.trigger = PlayerProjectileTeleportModifier.TeleportTrigger.DistanceFromTarget;
                            rogueTeleport.distToTeleport = tp.distToTeleport * 2f;
                            rogueTeleport.behindTargetDistance = tp.behindTargetDistance *= 0.9f;
                            rogueTeleport.leadAmount = tp.leadAmount;
                            rogueTeleport.minAngleToTeleport = tp.minAngleToTeleport;
                            rogueTeleport.numTeleports = 2;
                            rogueTeleport.type = PlayerProjectileTeleportModifier.TeleportType.BehindTarget;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
            }
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
