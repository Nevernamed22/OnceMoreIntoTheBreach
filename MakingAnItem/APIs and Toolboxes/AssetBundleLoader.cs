﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    internal class AssetBundleLoader
    {
        public static tk2dSpriteCollectionData FastLoadSpriteCollection(AssetBundle bundle, string CollectionName, string MaterialName)
        {
            //ETGModConsole.Log($"Loading Collection '{CollectionName}' with material '{MaterialName}'");
            if (bundle == null)
            {
                ETGModConsole.Log("ASSET BUNDLE WAS NULL.");
                return null;
            }
            if (bundle.LoadAsset<GameObject>(CollectionName) == null)
            {
                ETGModConsole.Log($"COLLECTION '{CollectionName}' WAS NULL");
                return null;
            }
            tk2dSpriteCollectionData Colection = bundle.LoadAsset<GameObject>(CollectionName).GetComponent<tk2dSpriteCollectionData>();
            Material material = bundle.LoadAsset<Material>(MaterialName);
            Texture texture = material.GetTexture("_MainTex");
            texture.filterMode = FilterMode.Point;
            material.SetTexture("_MainTex", texture);
            Colection.material = material;

            Colection.materials = new Material[]
            {
                material,
            };
            Colection.materialInsts = new Material[]
            {
                material,
            };
            foreach (var c in Colection.spriteDefinitions)
            {
                c.material = Colection.materials[0];
                c.materialInst = Colection.materials[0];
                c.materialId = 0;
            }
            return Colection;
        }

        public static AssetBundle LoadAssetBundleFromLiterallyAnywhere(string name, bool logs = false)
        {
            AssetBundle result = null;

            string platformFolder;

            switch (Application.platform)
            {
                case RuntimePlatform.LinuxPlayer:
                case RuntimePlatform.LinuxEditor:
                    platformFolder = "Linux";
                    break;

                case RuntimePlatform.OSXPlayer:
                case RuntimePlatform.OSXEditor:
                    platformFolder = "MacOS";
                    break;

                default:
                    platformFolder = "Windows";
                    break;
            }

            var path = Path.Combine(Path.Combine(Initialisation.FilePathFolder, platformFolder), name);

            if (File.Exists(path))
            {
                try
                {
                    result = AssetBundle.LoadFromFile(path);
                    if (logs == true)
                    {
                        global::ETGModConsole.Log("Successfully loaded assetbundle!", false);
                    }
                }
                catch (Exception ex)
                {
                    global::ETGModConsole.Log("Failed loading asset bundle from file.", false);
                    global::ETGModConsole.Log(ex.ToString(), false);
                }
            }
            else
            {
                global::ETGModConsole.Log("AssetBundle NOT FOUND!", false);
            }

            if (result != null)
            {
                var colls = result.LoadAllAssets<GameObject>().SelectMany(x => x.GetComponents<tk2dSpriteCollectionData>());

                var shaderDict = new Dictionary<string, string>()
                {
                    { "GunCollection", "tk2d/CutoutVertexColorTilted" },
                    { "GunCollection2", "tk2d/CutoutVertexColorTilted" }
                    // if you have multiple gun collections, copy this line for all of them
                };

                foreach (var coll in colls)
                {
                    if (!shaderDict.TryGetValue(coll.spriteCollectionName, out var shaderName))
                        continue;

                    var shader = ShaderCache.Acquire(shaderName);

                    foreach (var mat in coll.materials)
                    {
                        if (mat == null)
                            continue;

                        mat.shader = shader;
                    }
                }
            }

            return result;
        }
    } }
