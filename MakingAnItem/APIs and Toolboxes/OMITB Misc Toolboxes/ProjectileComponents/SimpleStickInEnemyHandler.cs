using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class SimpleStickInEnemyHandler :  BraveBehaviour
    {
        public GameObject stickyToSpawn;
        private void Start()
        {
            base.projectile.OnHitEnemy += OnHit;
        }
        private void OnHit(Projectile self, SpeculativeRigidbody body, bool fatal)
        {
            if (body && !fatal && body.gameActor && body.sprite && body.GetComponent<tk2dSprite>())
            {
                GameActor target = body.gameActor;

                GameObject instantiatedVFX = SpawnManager.SpawnVFX(stickyToSpawn, body.transform.position, Quaternion.identity, true);
                tk2dSprite vfxSprite = instantiatedVFX.GetComponent<tk2dSprite>();
                tk2dSprite hostsprite = target.gameObject.GetComponent<tk2dSprite>();
                if (vfxSprite != null && hostsprite != null)
                {
                    hostsprite.AttachRenderer(vfxSprite);
                    vfxSprite.HeightOffGround = 0.1f;
                    vfxSprite.IsPerpendicular = true;
                    vfxSprite.usesOverrideMaterial = true;
                }

                BuffVFXAnimator buffVFXAnimat = instantiatedVFX.GetOrAddComponent<BuffVFXAnimator>();
                if (buffVFXAnimat != null)
                {
                    if (self && self.LastVelocity != Vector2.zero)
                    {
                        buffVFXAnimat.InitializePierce(body.gameActor, self.LastVelocity);
                    }
                    else
                    {
                        buffVFXAnimat.Initialize(body.gameActor);
                    }
                }
            }
        }
    }
}
