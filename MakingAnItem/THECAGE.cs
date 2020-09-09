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
    class THECAGE : IounStoneOrbitalItem
    {

        public static Hook guonHook;
        public static bool speedUp = false;
        public static PlayerOrbital orbitalPrefab;

        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            string itemName = "THE CAGE"; //The name of the item
            string resourceName = "NevernamedsItems/Resources/THECAGE_icon"; //Refers to an embedded png in the project. Make sure to embed your resources!

            GameObject obj = new GameObject();

            var item = obj.AddComponent<WoodGuonStone>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "THE CAGE";
            string longDesc = "HE COMES";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");
            item.quality = PickupObject.ItemQuality.EXCLUDED;

            BuildPrefab();
            item.OrbitalPrefab = orbitalPrefab;
            item.Identifier = IounStoneIdentifier.GENERIC;

            item.CanBeDropped = false;
        }

        public static void BuildPrefab()
        {
            if (WoodGuonStone.orbitalPrefab != null) return;
            GameObject prefab = SpriteBuilder.SpriteFromResource("NevernamedsItems/Resources/THECAGE_icon");
            prefab.name = "THE CAGE Orbital";
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
        }

        public override void Pickup(PlayerController player)
        {
            player.OnHitByProjectile += this.OwnerHitByProjectile;
            guonHook = new Hook(
                typeof(PlayerOrbital).GetMethod("Initialize"),
                typeof(WoodGuonStone).GetMethod("GuonInit")
            );

            base.Pickup(player);

        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.OnHitByProjectile -= this.OwnerHitByProjectile;
            guonHook.Dispose();
            speedUp = false;

            return base.Drop(player);
        }
        protected override void OnDestroy()
        {
            Owner.OnHitByProjectile -= this.OwnerHitByProjectile;
            guonHook.Dispose();
            speedUp = false;
            base.OnDestroy();
        }

        public static void GuonInit(Action<PlayerOrbital, PlayerController> orig, PlayerOrbital self, PlayerController player)
        {
            orig(self, player);
        }
        private void OwnerHitByProjectile(Projectile incomingProjectile, PlayerController arg2)
        {

        }
    }
}
