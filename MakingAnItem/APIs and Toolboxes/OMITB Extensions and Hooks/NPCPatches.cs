using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    [HarmonyPatch(typeof(UltraFortunesFavor))]
    [HarmonyPatch("OnTriggerCollision", MethodType.Normal)]
    public class FortunePost
    {
        [HarmonyPostfix]
        public static void HarmonyPostfix(UltraFortunesFavor __instance, SpeculativeRigidbody specRigidbody, SpeculativeRigidbody sourceSpecRigidbody, CollisionData collisionData)
        {
            if (collisionData.MyPixelCollider == __instance.m_bulletBlocker && collisionData.OtherRigidbody != null && collisionData.OtherRigidbody.projectile != null)
            {
                if (__instance.gameObject.GetComponent<NPCShootReactor>()) { __instance.gameObject.GetComponent<NPCShootReactor>().OnShot(collisionData.OtherRigidbody.projectile); }
            }
        }
    }

    public class NPCShootReactor : MonoBehaviour
    {
        public Action<Projectile> OnShot;
    }

    [HarmonyPatch(typeof(CompanionItem))]
    [HarmonyPatch("CreateCompanion", MethodType.Normal)]
    public class CreatePost
    {
        [HarmonyPostfix]
        public static void HarmonyPostfix(CompanionItem __instance, PlayerController owner)
        {
            if (__instance && __instance is ExtendedCompanionItem) { (__instance as ExtendedCompanionItem).OnCompanionCreation(owner); }
        }
    }
}
