using Alexandria.ItemAPI;
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
        public static void AddShellCasing(this Gun gun, int numToLaunchOnFire = 1, int fireFrameToLaunch = 0, int numToLaunchOnReload = 0, int reloadFrameToLaunch = 0, string shellSpriteName = null)
        {

            if (!string.IsNullOrEmpty(shellSpriteName))
            {
                gun.shellCasing = Breakables.GenerateDebrisObject(Initialisation.GunDressingCollection, 
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
                    ).gameObject;
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

        public static void AddClipSprites(this Gun gun, string name, string fullNameOverride = null, string emptyNameOverride = null)
        {
            // gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            // gun.DefaultModule.customAmmoType = CustomClipAmmoTypeToolbox.AddCustomAmmoType("Love Pistol Hearts", "NevernamedsItems/Resources/CustomGunAmmoTypes/lovepistol_clipfull", "NevernamedsItems/Resources/CustomGunAmmoTypes/lovepistol_clipempty");

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
        }
        public static void Add<T>(ref T[] array, T toAdd)
        {
            List<T> list = array.ToList();
            list.Add(toAdd);
            array = list.ToArray<T>();
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
    }
}
