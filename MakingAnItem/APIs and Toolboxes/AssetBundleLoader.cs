using System;
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
                {
                    if (File.Exists(Initialisation.FilePathFolder + "/" + name))
                    {
                        try
                        {
                            result = AssetBundle.LoadFromFile(Path.Combine(Initialisation.FilePathFolder, name));
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
                }
                return result;
            }
        
    }
}
