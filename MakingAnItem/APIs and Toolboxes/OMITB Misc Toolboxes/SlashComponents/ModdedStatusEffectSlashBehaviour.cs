using Alexandria.ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class ModdedStatusEffectSlashBehaviour : ProjectileSlashingBehaviour
    {
        public override void SlashHitTarget(GameActor target, bool fatal)
        {
            if (target is AIActor)
            {
                if (appliesExsanguination)
                {
                    for (int i = 0; i < 1; i++)
                    {
                        target.gameActor.ApplyEffect(new GameActorExsanguinationEffect() { duration = exsanguinationDuration });
                    }
                }
            }
            base.SlashHitTarget(target, fatal);
        }

        public bool appliesExsanguination;
        public float exsanguinationDuration = 10f;
        public int exsanguinationStacks = 1;
    }
}