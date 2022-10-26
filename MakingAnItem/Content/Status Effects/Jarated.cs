using Alexandria.ItemAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class GameActorJarateEffect : AIActorDebuffEffect
    {
        public GameActorJarateEffect()
        {
            this.TintColor = Color.yellow;
            this.DeathTintColor = Color.yellow;
            this.AppliesTint = true;
            this.AppliesDeathTint = true;
            this.AffectsPlayers = false;
            this.AffectsEnemies = true;
            this.effectIdentifier = "jarated";
            pissAccum = 0;

        }
        private float pissAccum;
        private FleePlayerData flee;

        public override void OnEffectApplied(GameActor actor, RuntimeGameActorEffectData effectData, float partialAmount = 1)
        {
            flee = new FleePlayerData();
            flee.Player = GameManager.Instance.PrimaryPlayer;
            flee.StartDistance = 5f;
            flee.StopDistance = 8f;

            actor.RemoveEffect("fire");
            if (actor is AIActor && (actor as AIActor).IsBlackPhantom) { (actor as AIActor).UnbecomeBlackPhantom(); }
            base.OnEffectApplied(actor, effectData, partialAmount);

        }

        public override void EffectTick(GameActor actor, RuntimeGameActorEffectData effectData)
        {
            pissAccum += BraveTime.DeltaTime * 3;
            if (pissAccum > 1f)
            {
                int num = Mathf.FloorToInt(pissAccum);
                pissAccum %= 1f;
                Vector2 minpos = actor.sprite.WorldBottomLeft;
                Vector2 maxpos = actor.sprite.WorldTopRight;
                for (int i = 0; i < num; i++)
                {
                    GameObject piss = UnityEngine.Object.Instantiate(EasyVFXDatabase.JarateDrip, new Vector2(UnityEngine.Random.Range(minpos.x, maxpos.x), UnityEngine.Random.Range(minpos.y, maxpos.y)), Quaternion.identity);
                    piss.transform.parent = actor.transform;
                    piss.GetComponent<tk2dBaseSprite>().HeightOffGround = 0.2f;
                    actor.sprite.AttachRenderer(piss.GetComponent<tk2dBaseSprite>());
                }
            }
            if (actor is AIActor)
            {
                AIActor aiactor = (actor as AIActor);
                if (aiactor.IsBlackPhantom) { aiactor.UnbecomeBlackPhantom(); }
                if (aiactor.behaviorSpeculator != null && flee != null)
                {
                    if (aiactor.behaviorSpeculator.FleePlayerData == null) aiactor.behaviorSpeculator.FleePlayerData = flee;
                }
            }
            
            base.EffectTick(actor, effectData);
        }
        public override void OnEffectRemoved(GameActor actor, RuntimeGameActorEffectData effectData)
        {
            if (actor is AIActor && (actor as AIActor).behaviorSpeculator != null && (actor as AIActor).behaviorSpeculator.FleePlayerData != null && (actor as AIActor).behaviorSpeculator.FleePlayerData == flee)
            {
                (actor as AIActor).behaviorSpeculator.FleePlayerData = null;
            }
            base.OnEffectRemoved(actor, effectData);
        }

    }
}
