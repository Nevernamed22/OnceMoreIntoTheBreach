﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using System.Collections;
using System.Reflection;
using MonoMod.RuntimeDetour;

namespace NevernamedsItems
{
    class GreyGuonStone : AdvancedPlayerOrbitalItem
    {
        public static PlayerOrbital orbitalPrefab;
        public static PlayerOrbital upgradeOrbitalPrefab;
        public static void Init()
        {
            string itemName = "Grey Guon Stone"; 
            string resourceName = "NevernamedsItems/Resources/greyguon_icon";

            GameObject obj = new GameObject();
            var item = obj.AddComponent<GreyGuonStone>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Vengeful Rock";
            string longDesc = "Any creature that harms this stone's bearer shall be harmed in kind." + "\n\nBlood unto blood, as it has always been.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");
            item.quality = PickupObject.ItemQuality.C;

            BuildPrefab();
            item.OrbitalPrefab = orbitalPrefab;
            BuildSynergyPrefab();
            //ETGModConsole.Log((GreyGuonStone.upgradeOrbitalPrefab != null).ToString());

            item.AddToSubShop(ItemBuilder.ShopType.Cursula);            

            item.HasAdvancedUpgradeSynergy = true;
            item.AdvancedUpgradeSynergy = "Greyer Guon Stone";
            item.AdvancedUpgradeOrbitalPrefab = GreyGuonStone.upgradeOrbitalPrefab.gameObject;
            //ETGModConsole.Log("Finished greyguon init");
        }
        public static void BuildPrefab()
        {
            if (GreyGuonStone.orbitalPrefab != null) return;
            GameObject prefab = SpriteBuilder.SpriteFromResource("NevernamedsItems/Resources/greyguon_ingame");
            prefab.name = "Grey Guon Orbital";
            var body = prefab.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(7, 13));
            body.CollideWithTileMap = false;
            body.CollideWithOthers = true;
            body.PrimaryPixelCollider.CollisionLayer = CollisionLayer.EnemyBulletBlocker;

            orbitalPrefab = prefab.AddComponent<PlayerOrbital>();
            orbitalPrefab.motionStyle = PlayerOrbital.OrbitalMotionStyle.ORBIT_PLAYER_ALWAYS;
            orbitalPrefab.shouldRotate = false;
            orbitalPrefab.orbitRadius = 2.5f;
            orbitalPrefab.orbitDegreesPerSecond = 90f;
            orbitalPrefab.SetOrbitalTier(0);

            GameObject.DontDestroyOnLoad(prefab);
            FakePrefab.MarkAsFakePrefab(prefab);
            prefab.SetActive(false);
            //ETGModConsole.Log("finished greyguon prefab");
        }

        public static void BuildSynergyPrefab()
        {
            bool flag = GreyGuonStone.upgradeOrbitalPrefab == null;
            if (flag)
            {
                GameObject gameObject = SpriteBuilder.SpriteFromResource("NevernamedsItems/Resources/greyguon_synergy", null, true);
                gameObject.name = "Grey Guon Orbital Synergy Form";
                SpeculativeRigidbody speculativeRigidbody = gameObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(12, 12));
                GreyGuonStone.upgradeOrbitalPrefab = gameObject.AddComponent<PlayerOrbital>();
                speculativeRigidbody.CollideWithTileMap = false;
                speculativeRigidbody.CollideWithOthers = true;
                speculativeRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.EnemyBulletBlocker;
                GreyGuonStone.upgradeOrbitalPrefab.shouldRotate = false;
                GreyGuonStone.upgradeOrbitalPrefab.orbitRadius = 2.5f;
                GreyGuonStone.upgradeOrbitalPrefab.orbitDegreesPerSecond = 90f;
                GreyGuonStone.upgradeOrbitalPrefab.orbitDegreesPerSecond = 120f;
                GreyGuonStone.upgradeOrbitalPrefab.SetOrbitalTier(0);
                UnityEngine.Object.DontDestroyOnLoad(gameObject);
                FakePrefab.MarkAsFakePrefab(gameObject);
                gameObject.SetActive(false);
                //ETGModConsole.Log("Finished greyguon upgrade init");

            }
        }
        private void OwnerHitByProjectile(Projectile incomingProjectile, PlayerController arg2)
        {
            if (incomingProjectile.Owner)
            {
                var target = incomingProjectile.Owner;
                float damageToDeal = 1;
                float userDamage = Owner.stats.GetStatValue(PlayerStats.StatType.Damage);
                if (Owner.HasPickupID(286) || Owner.HasPickupID(158) || Owner.HasPickupID(434))
                {
                    damageToDeal = userDamage * 50;
                }
                else
                {
                    damageToDeal = userDamage * 25;
                }
                if (target.aiActor.IsBlackPhantom) damageToDeal *= 3f;
                target.healthHaver.ApplyDamage(damageToDeal, Vector2.zero, "Guon Wrath", CoreDamageTypes.None, DamageCategory.Unstoppable, true, null, false);
            }
        }
        public override void Pickup(PlayerController player)
        {
            player.OnHitByProjectile += this.OwnerHitByProjectile;
            base.Pickup(player);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.OnHitByProjectile -= this.OwnerHitByProjectile;
            return base.Drop(player);
        }
        protected override void OnDestroy()
        {
            Owner.OnHitByProjectile -= this.OwnerHitByProjectile;
            base.OnDestroy();
        }
    }
}

