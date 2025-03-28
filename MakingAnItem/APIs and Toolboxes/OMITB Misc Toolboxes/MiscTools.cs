using Alexandria.ItemAPI;
using Alexandria.SoundAPI;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public static class MiscTools
    {
        public static List<T> DupeList<T>(T value, int length)
        {
            List<T> list = new List<T>();
            for (int i = 0; i < length; i++)
            {
                list.Add(value);
            }
            return list;
        }
        public static void AddShellCasing(this Gun gun, int numToLaunchOnFire = 1, int fireFrameToLaunch = 0, int numToLaunchOnReload = 0, int reloadFrameToLaunch = 0, string shellSpriteName = null, string audioEvent = "Play_WPN_magnum_shells_01")
        {

            if (!string.IsNullOrEmpty(shellSpriteName))
            {
                DebrisObject casing = Breakables.GenerateDebrisObject(Initialisation.GunDressingCollection,
                    shellSpriteName,
                    true,
                    1,
                    5,
                    900,
                    180,
                    null,
                    1,
                    null,
                    null,
                    1
                    );
                casing.audioEventName = audioEvent;
                gun.shellCasing = casing.gameObject;
            }
            else
            {
                gun.shellCasing = (PickupObjectDatabase.GetById(15) as Gun).shellCasing;
            }
            gun.shellCasingOnFireFrameDelay = fireFrameToLaunch;
            gun.reloadShellLaunchFrame = reloadFrameToLaunch;
            gun.shellsToLaunchOnFire = numToLaunchOnFire;
            gun.shellsToLaunchOnReload = numToLaunchOnReload;
        }
        public static List<string> addedAmmos = new List<string>();
        public static void AddClipSprites(this Gun gun, string name, string fullNameOverride = null, string emptyNameOverride = null)
        {
            if (!addedAmmos.Contains(name))
            {
                Texture2D fgTexture = Initialisation.assetBundle.LoadAsset<Texture2D>(!string.IsNullOrEmpty(fullNameOverride) ? fullNameOverride : $"{name}_clipfull");
                Texture2D bgTexture = Initialisation.assetBundle.LoadAsset<Texture2D>(!string.IsNullOrEmpty(emptyNameOverride) ? emptyNameOverride : $"{name}_clipempty");

                GameObject fgSpriteObject = new GameObject("sprite fg");
                fgSpriteObject.MakeFakePrefab();

                GameObject bgSpriteObject = new GameObject("sprite bg");
                bgSpriteObject.MakeFakePrefab();

                dfTiledSprite fgSprite = fgSpriteObject.SetupDfSpriteFromTexture<dfTiledSprite>(fgTexture, ShaderCache.Acquire("Daikon Forge/Default UI Shader"));
                dfTiledSprite bgSprite = bgSpriteObject.SetupDfSpriteFromTexture<dfTiledSprite>(bgTexture, ShaderCache.Acquire("Daikon Forge/Default UI Shader"));
                GameUIAmmoType uiammotype = new GameUIAmmoType
                {
                    ammoBarBG = bgSprite,
                    ammoBarFG = fgSprite,
                    ammoType = GameUIAmmoType.AmmoType.CUSTOM,
                    customAmmoType = name
                };
                CustomClipAmmoTypeToolbox.addedAmmoTypes.Add(uiammotype);
                foreach (GameUIAmmoController uiammocontroller in GameUIRoot.Instance.ammoControllers)
                {
                    Add(ref uiammocontroller.ammoTypes, uiammotype);
                }
                addedAmmos.Add(name);
            }
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = name;
        }
        public static void AddCustomSwitchGroup(this Gun gun, string name, string fire, string reload)
        {
            gun.gunSwitchGroup = name;
            SoundManager.AddCustomSwitchData("WPN_Guns", gun.gunSwitchGroup, "Play_WPN_Gun_Reload_01", reload);
            SoundManager.AddCustomSwitchData("WPN_Guns", gun.gunSwitchGroup, "Play_WPN_Gun_Shot_01", fire);
        }
        public static void Add<T>(ref T[] array, T toAdd)
        {
            List<T> list = array.ToList();
            list.Add(toAdd);
            array = list.ToArray<T>();
        }
        public static void SetBarrel(this Gun gun, int xoffset, int yoffset)
        {
            gun.barrelOffset.transform.localScale = Vector3.one;
            gun.barrelOffset.transform.localPosition = new Vector3((float)xoffset / 16f, (float)yoffset / 16f, 0f);
        }
        public static void AddClipDebris(this Gun gun, int frameToLaunch, int numToLaunch, string clipSpriteName = null)
        {
            if (!string.IsNullOrEmpty(clipSpriteName))
            {
                gun.clipObject = Breakables.GenerateDebrisObject(Initialisation.GunDressingCollection, clipSpriteName, true, 1, 5, 60, 20, null, 1, null, null, 1).gameObject;

            }
            else
            {
                gun.clipObject = (PickupObjectDatabase.GetById(15) as Gun).clipObject;
            }
            gun.reloadClipLaunchFrame = frameToLaunch;
            gun.clipsToLaunchOnReload = numToLaunch;
        }
        public static string FlipAnimation(this tk2dSpriteAnimator anim, string origAnimation, string newAnimation)
        {
            if (anim == null || anim.Library == null)
                return "";

            return anim.Library.FlipAnimation(origAnimation, newAnimation);
        }

        public static string FlipAnimation(this tk2dSpriteAnimation anim, string origAnimation, string newAnimation)
        {
            if (anim == null)
                return "";

            var clippy = anim.GetClipByName(origAnimation);

            if (clippy == null)
                return "";

            var newClippy = new tk2dSpriteAnimationClip()
            {
                name = newAnimation,

                fps = clippy.fps,
                loopStart = clippy.loopStart,
                wrapMode = clippy.wrapMode,
                minFidgetDuration = clippy.minFidgetDuration,
                maxFidgetDuration = clippy.maxFidgetDuration
            };

            anim.clips = anim.clips.AddToArray(newClippy);

            if (clippy.frames == null)
            {
                newClippy.frames = null;
                return newClippy.name;
            }

            newClippy.frames = new tk2dSpriteAnimationFrame[clippy.frames.Length];

            for (var i = 0; i < clippy.frames.Length; i++)
            {
                var f = clippy.frames[i];

                if (f == null)
                    continue;

                var newFrame = newClippy.frames[i] = new tk2dSpriteAnimationFrame();
                newFrame.CopyFrom(f);

                var sprite = f.spriteCollection.spriteDefinitions[f.spriteId];

                if (sprite == null)
                    continue;

                var flipSprite = sprite.FlipSprite();

                if (flipSprite == null)
                    continue;

                newFrame.spriteId = SpriteBuilder.AddSpriteToCollection(flipSprite, f.spriteCollection);
            }

            return newClippy.name;
        }

        private static tk2dSpriteDefinition FlipSprite(this tk2dSpriteDefinition orig)
        {
            if (orig == null || orig.uvs == null || orig.uvs.Length != 4)
                return null;

            return new tk2dSpriteDefinition()
            {
                name = orig.name,

                boundsDataCenter = orig.boundsDataCenter,
                boundsDataExtents = orig.boundsDataExtents,

                untrimmedBoundsDataCenter = orig.untrimmedBoundsDataCenter,
                untrimmedBoundsDataExtents = orig.untrimmedBoundsDataExtents,

                texelSize = orig.texelSize,

                position0 = orig.position0,
                position1 = orig.position1,
                position2 = orig.position2,
                position3 = orig.position3,

                uvs = new Vector2[]
                {
                    orig.uvs[1],
                    orig.uvs[0],
                    orig.uvs[3],
                    orig.uvs[2]
                },

                material = orig.material,
                materialInst = orig.materialInst != null ? new Material(orig.materialInst) : null,
                materialId = orig.materialId,

                extractRegion = orig.extractRegion,
                regionX = orig.regionX,
                regionY = orig.regionY,
                regionW = orig.regionW,
                regionH = orig.regionH,

                flipped = orig.flipped,
                complexGeometry = orig.complexGeometry,
                physicsEngine = orig.physicsEngine,
                colliderType = orig.colliderType,
                collisionLayer = orig.collisionLayer,
                colliderVertices = orig.colliderVertices.ToArray(),
                colliderConvex = orig.colliderConvex,
                colliderSmoothSphereCollisions = orig.colliderSmoothSphereCollisions
            };
        }
    }
}


