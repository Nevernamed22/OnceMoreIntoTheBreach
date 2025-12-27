using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alexandria.ItemAPI;
using UnityEngine;

namespace NevernamedsItems
{
    public class ProjectileInstakillBehaviour : MonoBehaviour
    {
        public ProjectileInstakillBehaviour()
        {
            bossBonusDMG = 1;
            protectBosses = true;
        }
        public void Start()
        {
            this.m_projectile = base.GetComponent<Projectile>();
            if (m_projectile) this.m_projectile.OnHitEnemy += this.OnHitEnemy;
        }
        private void OnHitEnemy(Projectile bullet, SpeculativeRigidbody enemy, bool fatal)
        {
            if (enemy && enemy.aiActor && enemy.healthHaver && !fatal)
            {
                bool shouldActivate = false;
                if (enemyGUIDsToKill.Contains(enemy.aiActor.EnemyGuid)) shouldActivate = true;
                foreach (string tag in tagsToKill) { if (enemy.aiActor.HasTag(tag)) shouldActivate = true; }
                if (shouldActivate && enemy.healthHaver.IsAlive)
                {
                    if (soundEvents != null && soundEvents.Count() > 0)
                    {
                        foreach(string sound in soundEvents) { AkSoundEngine.PostEvent(sound, enemy.gameObject); }                    
                    }
                    if (vfx != null) { enemy.aiActor.PlayEffectOnActor(vfx, Vector3.zero); }
                    if (extraKnockback > 0)
                    {
                        enemy.knockbackDoer.ApplyKnockback(bullet.Direction, extraKnockback);
                    }
                    if (onInstaKill != null) { onInstaKill(bullet, enemy.aiActor); }
                    enemy.healthHaver.ApplyDamage((enemy.healthHaver.IsBoss && !enemy.healthHaver.IsSubboss && protectBosses) ? bossBonusDMG : 1E+07f, impartFinalDamageDirection ? bullet.Direction : Vector2.zero, "Erasure", CoreDamageTypes.None, (enemy.healthHaver.IsBoss && !enemy.healthHaver.IsSubboss && protectBosses) ? DamageCategory.Environment : DamageCategory.Unstoppable, true, null, false);
                }
                if (enemyGUIDSToEraseFromExistence.Contains(enemy.aiActor.EnemyGuid)) enemy.aiActor.EraseFromExistenceWithRewards();
            }
        }
        public float bossBonusDMG;
        private Projectile m_projectile;
        public List<string> enemyGUIDsToKill = new List<string>();
        public List<string> enemyGUIDSToEraseFromExistence = new List<string>();
        public List<string> tagsToKill = new List<string>();
        public bool protectBosses;
        public GameObject vfx = null;
        public List<string> soundEvents = new List<string>();
        public bool impartFinalDamageDirection = true;
        public float extraKnockback = 0f;
        public Action<Projectile, AIActor> onInstaKill;
    }
}
