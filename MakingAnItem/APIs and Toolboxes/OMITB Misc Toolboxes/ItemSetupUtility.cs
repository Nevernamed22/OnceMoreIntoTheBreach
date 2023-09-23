using Alexandria.ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
  public static  class ItemSetup
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
    }
}
