using Alexandria.ItemAPI;
using Alexandria.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class JarateGoop : SpecialGoopBehaviourDoer
    {
        public static void Init()
        {
            EasyGoopDefinitions.JarateGoop = ScriptableObject.CreateInstance<GoopDefinition>();
            EasyGoopDefinitions.JarateGoop.CanBeIgnited = false;
            EasyGoopDefinitions.JarateGoop.damagesEnemies = false;
            EasyGoopDefinitions.JarateGoop.damagesPlayers = false;
            EasyGoopDefinitions.JarateGoop.baseColor32 = Color.yellow;
            EasyGoopDefinitions.JarateGoop.goopTexture = GoopUtility.PoisonDef.goopTexture;
            EasyGoopDefinitions.JarateGoop.usesLifespan = true;
            EasyGoopDefinitions.JarateGoop.lifespan = 20f;
            EasyGoopDefinitions.JarateGoop.name = "omitbjarategoop";
            GoopUtility.RegisterComponentToGoopDefinition(EasyGoopDefinitions.JarateGoop, typeof(JarateGoop));
        }
        public override void DoGoopEffectUpdate(DeadlyDeadlyGoopManager goop, GameActor actor, IntVector2 position)
        {
            if (actor && actor.aiActor && actor.aiActor.HasBeenEngaged && actor.healthHaver)
            {
                actor.ApplyEffect(new GameActorJarateEffect()
                {
                    duration = 10,
                    stackMode = GameActorEffect.EffectStackingMode.Refresh,
                    HealthMultiplier = actor.healthHaver.IsBoss ? 0.75f : 0.66f,
                    SpeedMultiplier = 0.9f,
                });
            }
            base.DoGoopEffectUpdate(goop, actor, position);
        }
    }
}



