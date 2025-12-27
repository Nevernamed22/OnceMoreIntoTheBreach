using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static NevernamedsItems.HandGunCardBullet;

namespace NevernamedsItems
{
    [HarmonyPatch(typeof(Gun), "Attack")]
    public class GunAttackPatch
    {
        static bool Prefix(Gun __instance, ProjectileData overrideProjectileData, GameObject overrideBulletObject, ref Gun.AttackResult __result)
        {
            if (__instance != null && __instance.gameObject.GetComponent<GunShootNegater>() != null)
            {
                bool negate = __instance.gameObject.GetComponent<GunShootNegater>().ShouldBeNegated();
                if (negate) { __result = Gun.AttackResult.Fail; }
                return !negate;
            }
            return true;
        }
    }

    public interface GunShootNegater
    {
        bool ShouldBeNegated();
    }
}
