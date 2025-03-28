using Alexandria.ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    class PlagueStatusEffectSetup
    {
        public static void Init()
        { 
            StaticStatusEffects.StandardPlagueEffect = StatusEffectHelper.GeneratePlagueEffect(100, 2, true, ExtendedColours.plaguePurple, true, ExtendedColours.plaguePurple);
        }
    }
    public class GameActorPlagueEffect : GameActorHealthEffect
    {
        public GameActorPlagueEffect()
        {
            this.DamagePerSecondToEnemies = 1f;
            this.TintColor = ExtendedColours.plaguePurple;
            this.DeathTintColor = ExtendedColours.plaguePurple;
            this.AppliesTint = true;
            this.AppliesDeathTint = true;
        }
        public override void EffectTick(GameActor actor, RuntimeGameActorEffectData effectData)
        {
            if (EasyGoopDefinitions.PlagueGoop != null)
            {
                DeadlyDeadlyGoopManager goop = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(EasyGoopDefinitions.PlagueGoop);
                goop.TimedAddGoopCircle(actor.specRigidbody.UnitCenter, 1.5f, 0.75f, true);
            }
            base.EffectTick(actor, effectData);
        }

    }
}


