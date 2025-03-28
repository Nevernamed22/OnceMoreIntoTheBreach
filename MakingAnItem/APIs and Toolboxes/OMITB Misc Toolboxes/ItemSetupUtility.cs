using Alexandria.Assetbundle;
using Alexandria.ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public static class ItemSetup
    {
        public static PickupObject NewItem<T>(string name, string subtitle, string description, string filepath, bool assetbundle = true) where T : Component
        {
            GameObject obj = new GameObject(name);
            Component item = obj.AddComponent(typeof(T));
            if (assetbundle)
            {
                ItemBuilder.AddSpriteToObjectAssetbundle(name, Initialisation.itemCollection.GetSpriteIdByName(filepath), Initialisation.itemCollection, obj);
            }
            else
            {
                ItemBuilder.AddSpriteToObject(name, filepath, obj);
            }
            ItemBuilder.SetupItem(item as PickupObject, subtitle, description, "nn");
            return item as PickupObject;
        }
        public static void SetProjectileSprite(this Projectile proj, string spriteID, int pixelWidth, int pixelHeight, bool lightened = true, tk2dBaseSprite.Anchor anchor = tk2dBaseSprite.Anchor.LowerLeft, int? overrideColliderPixelWidth = null, int? overrideColliderPixelHeight = null, bool anchorChangesCollider = true, bool fixesScale = false, int? overrideColliderOffsetX = null, int? overrideColliderOffsetY = null, Projectile overrideProjectileToCopyFrom = null, bool useFolder = false)
        {
            if (useFolder)
            {
                proj.SetProjectileSpriteRight(spriteID, pixelWidth, pixelHeight, lightened, anchor, overrideColliderPixelWidth, overrideColliderPixelHeight, anchorChangesCollider, fixesScale, overrideColliderOffsetX, overrideColliderOffsetY, overrideProjectileToCopyFrom);
            }
            else
            {
                proj.SetProjectileCollisionRight(spriteID, Initialisation.ProjectileCollection, pixelWidth, pixelHeight, lightened, anchor, overrideColliderPixelWidth, overrideColliderPixelHeight, anchorChangesCollider, fixesScale, overrideColliderOffsetX, overrideColliderOffsetY, overrideProjectileToCopyFrom);
            }
        }

        public static void SetGunSprites(this Gun gun, string identity, int framerate = 8, bool noAmmonomicon = false, int collection = 1)
        {
            Dictionary<int, tk2dSpriteCollectionData> collections = new Dictionary<int, tk2dSpriteCollectionData>()
            {
                {1, Initialisation.gunCollection },
                {2, Initialisation.gunCollection2 },
            };

            Alexandria.Assetbundle.GunInt.SetupSprite(gun, collections[collection], $"{identity}_idle_001", framerate, noAmmonomicon ? null : $"{identity}_ammonomicon_001");
        }
      
        public static GameObject CreateOrbitalObject(string name, string defaultSprite, IntVector2 colliders, IntVector2 colliderOffsets, string animationClip = null, float orbitRadius = 2.5f, float rotationDegreesPerSecond = 120f, int orbitalTier = 0, PlayerOrbital.OrbitalMotionStyle orbitmotionStyle = PlayerOrbital.OrbitalMotionStyle.ORBIT_PLAYER_ALWAYS, float perfectOrbitalFactor = 0f)
        {
            GameObject prefab = ItemBuilder.SpriteFromBundle(defaultSprite, Initialisation.itemCollection.GetSpriteIdByName(defaultSprite), Initialisation.itemCollection, new GameObject(name));
            
            var body = prefab.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(colliderOffsets, colliders);
            body.CollideWithTileMap = false;
            body.CollideWithOthers = true;
            body.PrimaryPixelCollider.CollisionLayer = CollisionLayer.EnemyBulletBlocker;

            PlayerOrbital orbitalPrefab = prefab.AddComponent<PlayerOrbital>();
            orbitalPrefab.motionStyle = orbitmotionStyle;
            orbitalPrefab.shouldRotate = false;
            orbitalPrefab.orbitRadius = orbitRadius;
            orbitalPrefab.orbitDegreesPerSecond = rotationDegreesPerSecond;
            orbitalPrefab.perfectOrbitalFactor = perfectOrbitalFactor;
            orbitalPrefab.SetOrbitalTier(orbitalTier);

            if (animationClip != null)
            {
                tk2dSpriteAnimator animator = prefab.GetOrAddComponent<tk2dSpriteAnimator>();
                animator.Library = Initialisation.itemAnimationCollection;
                animator.DefaultClipId = Initialisation.itemAnimationCollection.GetClipIdByName(animationClip);
                animator.playAutomatically = true;
            }

            prefab.MakeFakePrefab();

            return prefab;
        }

    }
}
