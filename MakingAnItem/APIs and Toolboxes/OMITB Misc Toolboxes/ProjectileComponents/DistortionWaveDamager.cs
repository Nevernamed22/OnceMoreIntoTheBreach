using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alexandria.Misc;
using Dungeonator;
using UnityEngine;

namespace NevernamedsItems
{
    public class DistortionWaveDamager : BraveBehaviour
    {
        public void Start()
        {
            if (base.projectile) { base.projectile.OnDestruction += OnDest; }
        }
        public float Damage = 5;
        public float Range = 4;
        public bool MultByPlayerDamage = true;
        public string audioEvent = null;
        public float lockDownDuration = -1;
        private List<GameActorEffect> eff = new List<GameActorEffect>();
        public float stunDuration = -1f;
        public void OnDest(Projectile self)
        {
            if (self)
            {
                float dmg = Damage;
                if (!string.IsNullOrEmpty(audioEvent)) { AkSoundEngine.PostEvent(audioEvent, self.gameObject); }
                if (self.ProjectilePlayerOwner() && MultByPlayerDamage) { dmg *= self.ProjectilePlayerOwner().stats.GetStatValue(PlayerStats.StatType.Damage); }
                Exploder.DoDistortionWave(self.SafeCenter, 0.5f, 0.04f, Range, 0.3f);
                Exploder.DoRadialDamage(dmg, self.SafeCenter, Range, false, true, false);
                if  (lockDownDuration > 0f) { eff.Add(StatusEffectHelper.GenerateLockdown(lockDownDuration)); }
                if (eff.Count > 0f)
                {
                    AfflictEnemiesInRadius(self.SafeCenter);
                }
            }
        }
        private void AfflictEnemiesInRadius(Vector2 vector2)
        {
            if (base.projectile)
            {
                List<AIActor> activeEnemies = vector2.GetAbsoluteRoom().GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
                if (activeEnemies != null)
                {
                    for (int i = 0; i < activeEnemies.Count; i++)
                    {
                        AIActor aiactor = activeEnemies[i];
                        if (aiactor != null && aiactor.IsNormalEnemy && aiactor.transform)
                        {
                            float num = Vector2.Distance(vector2, aiactor.CenterPosition);
                            if (num <= Range)
                            {
                                foreach(GameActorEffect ef in eff)
                                {
                                    aiactor.ApplyEffect(ef);
                                }
                                if (stunDuration > 0 && aiactor.behaviorSpeculator)
                                {
                                    aiactor.behaviorSpeculator.Stun(stunDuration);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
