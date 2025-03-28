using HarmonyLib;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public static class OMITBActions
    {
        public static ChSclModify ModifyChanceScalar;
        public delegate void ChSclModify(ref float scalar, PlayerController player);
        public static Action<MinorBreakable> MinorBreakableBroken;
    }

    [HarmonyPatch(typeof(MinorBreakable))]
    [HarmonyPatch("DestabilizeAttachedObjects", MethodType.Normal)]
    public static class MinorBreakableStuff
    {
        [HarmonyPrefix]
        public static void BreakPref(MinorBreakable __instance, Vector2 vec)
        {
            //Debug.Log("BrokenObject");
            if (__instance && __instance.enabled && OMITBActions.MinorBreakableBroken != null) {  OMITBActions.MinorBreakableBroken(__instance); }
        }
    }
    
    public static class ChanceScalarPatch
    {
        [HarmonyPatch(typeof(PlayerController), nameof(PlayerController.DoPostProcessProjectile))]
        [HarmonyILManipulator]
        public static void ChanceScalarPatch_Transpiler(ILContext ctx)
        {
            var crs = new ILCursor(ctx);

            // Goes right before PostProcessProjectile.Invoke() to modify its last argument (the chance scalar)
            if (!crs.TryGotoNext(MoveType.Before, instruction => instruction.MatchCallOrCallvirt<Action<Projectile, float>>(nameof(Action<Projectile, float>.Invoke))))
                return;

            // Emits (adds) two instructions that effectively replace the last argument (arg normally) with ChanceScalarIncrease(arg, this) (this being the PlayerController)
            // The cursor's position is after the instruction that loads the arg local, meaning at this point it'd be the value at the top of the stack.
            // The Ldarg_0 instruction loads the player.
            // The second instruction calls your method.

            // Since your method has two arguments, it will take two values from the stack: the just-emitted player value and the current value of "arg".
            // It will then push the returned value back to the stack, the returned value being the modified scalar
            crs.Emit(OpCodes.Ldarg_0);
            crs.Emit(OpCodes.Call, AccessTools.Method(typeof(ChanceScalarPatch), nameof(ChanceScalarIncrease)));
        }

        public static float ChanceScalarIncrease(float current, PlayerController player)
        {
            if (OMITBActions.ModifyChanceScalar != null)
            {
                OMITBActions.ModifyChanceScalar(ref current, player);
            }
            return current;
        }
    }
}
