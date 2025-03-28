using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alexandria.Misc;

namespace NevernamedsItems
{
    [HarmonyPatch(typeof(Gun))]
    [HarmonyPatch("PlayIdleAnimation", MethodType.Normal)]
    public class IdlePost
    {
        [HarmonyPostfix]
        public static void HarmonyPostfix(Gun __instance)
        {
            if (__instance.usesContinuousFireAnimation && __instance.usesDirectionalAnimator)
            {
                AIAnimator anim = __instance.aiAnimator;
                if (anim != null && anim.m_currentActionState != null)
                {
                    anim.EndAnimation();
                }
            }
        }
    }
    [HarmonyPatch(typeof(SynergyFactorConstants), nameof(SynergyFactorConstants.GetSynergyFactor))]
    public static class SynergyFactorMultiplier
    {
        public static void Postfix(ref float __result)
        {
            float modification = 0;
            
            if (GameManager.Instance.PrimaryPlayer != null) { modification += GameManager.Instance.PrimaryPlayer.GetNumberOfItemInInventory(Gracelets.ID); }
            if (GameManager.Instance.SecondaryPlayer != null) { modification += GameManager.Instance.SecondaryPlayer.GetNumberOfItemInInventory(Gracelets.ID); }

            __result *= (modification * 10f);
        }
    }
}
