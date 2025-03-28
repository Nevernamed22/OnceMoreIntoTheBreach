using Alexandria.ItemAPI;
using Alexandria.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class HoleGoop : SpecialGoopBehaviourDoer
    {
        public static void Init()
        {
            EasyGoopDefinitions.PitGoop = ScriptableObject.CreateInstance<GoopDefinition>();
            EasyGoopDefinitions.PitGoop.CanBeIgnited = false;
            EasyGoopDefinitions.PitGoop.damagesEnemies = false;
            EasyGoopDefinitions.PitGoop.damagesPlayers = false;
            EasyGoopDefinitions.PitGoop.baseColor32 = Color.black;
            EasyGoopDefinitions.PitGoop.goopTexture = Initialisation.assetBundle.LoadAsset<Texture2D>("pitgooptex");
            EasyGoopDefinitions.PitGoop.usesLifespan = true;
            EasyGoopDefinitions.PitGoop.lifespan = 30f;
            EasyGoopDefinitions.PitGoop.overrideOpaqueness = 1f;
            EasyGoopDefinitions.PitGoop.name = "omitbpitgoop";
            EasyGoopDefinitions.PitGoop.goopDamageTypeInteractions = new List<GoopDefinition.GoopDamageTypeInteraction>()
            {
                new GoopDefinition.GoopDamageTypeInteraction()
                {
                    freezesGoop = false,
                    electrifiesGoop = false,
                    ignitionMode = GoopDefinition.GoopDamageTypeInteraction.GoopIgnitionMode.NONE,
                    damageType = CoreDamageTypes.None
                }
            };
            GoopUtility.RegisterComponentToGoopDefinition(EasyGoopDefinitions.PitGoop, typeof(HoleGoop));
        }
        public override void DoGoopEffectUpdate(DeadlyDeadlyGoopManager goop, GameActor actor, IntVector2 position)
        {
            if (actor && actor is AIActor && actor.aiAnimator && !actor.aiAnimator.IsPlaying("spawn") && !actor.aiAnimator.IsPlaying("awaken") && actor.healthHaver && !actor.healthHaver.IsBoss && actor.aiActor.HasBeenEngaged)
            {
                actor.ForceFall();
            }
            base.DoGoopEffectUpdate(goop, actor, position);
        }
    }
}



