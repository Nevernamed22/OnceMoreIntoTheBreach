using Alexandria.ItemAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.Misc;

namespace NevernamedsItems
{
    public class GameActorCryingEffect : GameActorEffect
    {
        public GameActorCryingEffect()
        {
            this.TintColor = Color.blue;
            this.DeathTintColor = Color.blue;
            this.AppliesTint = false;
            this.AppliesDeathTint = false;
            this.AffectsPlayers = false;
            this.AffectsEnemies = true;
            this.effectIdentifier = "crying";
            OverheadVFX = SharedVFX.CryingOverhead;
            tearAccum = 0;

        }
        private float tearAccum;

        public override void EffectTick(GameActor actor, RuntimeGameActorEffectData effectData)
        {
            tearAccum += BraveTime.DeltaTime * 4;
            if (tearAccum > 1f)
            {
                int num = Mathf.FloorToInt(tearAccum);
                tearAccum %= 1f;
                Vector2 minpos = actor.sprite.WorldBottomLeft;
                Vector2 maxpos = actor.sprite.WorldTopRight;
                for (int i = 0; i < num; i++)
                {
                    GameObject tear = (PickupObjectDatabase.GetById(33) as Gun).DefaultModule.projectiles[0].InstantiateAndFireInDirection(new Vector2(UnityEngine.Random.Range(minpos.x, maxpos.x), UnityEngine.Random.Range(minpos.y, maxpos.y)),
                        BraveUtility.RandomAngle(), 0f, null);
                    tear.gameObject.GetOrAddComponent<BounceProjModifier>();
                    Projectile tearProj = tear.gameObject.GetComponent<Projectile>();
                    if (tearProj)
                    {
                        tearProj.Owner = GameManager.Instance.BestActivePlayer;
                        tearProj.baseData.range *= 0.6f;
                        if (actor.specRigidbody && tearProj.specRigidbody) { tearProj.specRigidbody.RegisterSpecificCollisionException(actor.specRigidbody); }
                    }
                }
            }
            base.EffectTick(actor, effectData);
        }
    }
}
