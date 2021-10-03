using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    class ExplosionHooks
    {
        public static void Init()
        {
            BombHook = new Hook(
    typeof(Exploder).GetMethod("Explode", BindingFlags.Static | BindingFlags.Public),
    typeof(ExplosionHooks).GetMethod("ExplosionHook", BindingFlags.Static | BindingFlags.Public));
        }
        public static Hook BombHook;

        public static void ExplosionHook(Action<Vector3, ExplosionData, Vector2, Action, bool, CoreDamageTypes, bool> orig, Vector3 position, ExplosionData data, Vector2 sourceNormal, Action onExplosionBegin = null, bool ignoreQueues = false, CoreDamageTypes damageTypes = CoreDamageTypes.None, bool ignoreDamageCaps = false)
        {
            orig(position, data, sourceNormal, onExplosionBegin, ignoreQueues, damageTypes, ignoreDamageCaps);
            try
            {
                //Blombk
                #region Blombk
                if (GameManager.Instance.AnyPlayerHasPickupID(Blombk.BlombkID))
                {
                    if (GameManager.Instance.AnyPlayerHasActiveSynergy("Atomic Blombk") && (UnityEngine.Random.value < 0.2f))
                    {
                        GameManager.Instance.GetPlayerWithItemID(Blombk.BlombkID).DoEasyBlank(position, EasyBlankType.FULL);
                    }
                    else
                    {
                        GameManager.Instance.GetPlayerWithItemID(Blombk.BlombkID).DoEasyBlank(position, EasyBlankType.MINI);
                    }
                }
                #endregion
                //Chem Grenade
                #region ChemGrenade
                if (GameManager.Instance.AnyPlayerHasPickupID(ChemGrenade.ChemGrenadeID))
                {
                    float radius = 5;
                    if (GameManager.Instance.AnyPlayerHasActiveSynergy("Toxic Shock")) radius = 8;
                    DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(EasyGoopDefinitions.PoisonDef).TimedAddGoopCircle(position, radius, 1, false);
                }
                #endregion
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
                ETGModConsole.Log(e.StackTrace);
            }
        }
        public delegate void Action<T1, T2, T3, T4, T5, T6, T7>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);
    }
}
