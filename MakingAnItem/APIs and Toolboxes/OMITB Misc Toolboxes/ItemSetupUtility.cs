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

        public static void SetGunSprites(this Gun gun, string identity, int framerate = 8, bool noAmmonomicon = false)
        {
            Alexandria.Assetbundle.GunInt.SetupSprite(gun, Initialisation.gunCollection, $"{identity}_idle_001", framerate, noAmmonomicon ? null : $"{identity}_ammonomicon_001");
        }
    }
}
