using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
   static class OMITBGameobjectExtensions
    {
        public static GameObject InstantiateAndFakeprefab(this GameObject target)
        {
            
            GameObject instantiatedTarget = UnityEngine.Object.Instantiate<GameObject>(target);
            instantiatedTarget.SetActive(false);
            FakePrefab.MarkAsFakePrefab(instantiatedTarget);
            UnityEngine.Object.DontDestroyOnLoad(instantiatedTarget);
            return instantiatedTarget;
        }
        public static void MakeFakePrefab(this GameObject target)
        {
            target.SetActive(false);
            FakePrefab.MarkAsFakePrefab(target);
            UnityEngine.Object.DontDestroyOnLoad(target);
        }
    }
}
