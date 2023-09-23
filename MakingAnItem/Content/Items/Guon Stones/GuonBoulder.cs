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
    class GuonBoulder : AdvancedPlayerOrbitalItem
    {
        public static PlayerOrbital orbitalPrefab;
        public static void Init()
        {
            AdvancedPlayerOrbitalItem item = ItemSetup.NewItem<GuonBoulder>(
            "Guon Boulder",
            "Hefty Chunk",
            "An experiment to see just how huge a Guon Stone can be." + "\n\nAll the magic in this stone is solely dedicated just to keeping it aloft, and thus no further special effects are able to fit inside.",
            "guonboulder_icon") as AdvancedPlayerOrbitalItem;         
            item.quality = PickupObject.ItemQuality.A;
            item.SetTag("guon_stone");

            BuildPrefab();
            item.OrbitalPrefab = orbitalPrefab;       
        }

        public static void BuildPrefab()
        {
            if (GuonBoulder.orbitalPrefab != null) return;        
            GameObject prefab = ItemBuilder.SpriteFromBundle("GuonBoulderOrbital", Initialisation.itemCollection.GetSpriteIdByName("guonboulder_icon"), Initialisation.itemCollection);
            prefab.name = "Guon Boulder Orbital";
            var body = prefab.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(30, 30));
            body.CollideWithTileMap = false;
            body.CollideWithOthers = true;
            body.PrimaryPixelCollider.CollisionLayer = CollisionLayer.EnemyBulletBlocker;

            orbitalPrefab = prefab.AddComponent<PlayerOrbital>();
            orbitalPrefab.motionStyle = PlayerOrbital.OrbitalMotionStyle.ORBIT_PLAYER_ALWAYS;
            orbitalPrefab.shouldRotate = false;
            orbitalPrefab.orbitRadius = 7f;
            orbitalPrefab.orbitDegreesPerSecond = 5f;
            orbitalPrefab.SetOrbitalTier(0);

            GameObject.DontDestroyOnLoad(prefab);
            FakePrefab.MarkAsFakePrefab(prefab);
            prefab.SetActive(false);
        }       
    }
}
