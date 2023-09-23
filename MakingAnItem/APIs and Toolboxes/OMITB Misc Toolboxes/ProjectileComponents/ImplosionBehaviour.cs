using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    class ImplosionBehaviour : MonoBehaviour
    {
        public void Start()
        {
            projectile = base.GetComponent<Projectile>();
            projectile.OnDestruction += OnDestruction;
        }
        private Projectile projectile;
        private void OnDestruction(Projectile self)
        {
            if (projectile && projectile.Owner)
            {
                if (projectile.Owner is PlayerController)
                {
                    for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
                    {
                        PlayerController playerController = GameManager.Instance.AllPlayers[i];
                        if (playerController && playerController.specRigidbody) { explosionData.ignoreList.Add(playerController.specRigidbody); }
                    }
                }
                else { explosionData.ignoreList.Add(projectile.Owner.specRigidbody); }
            }
            Vector3 vector = projectile.specRigidbody.UnitCenter.ToVector3ZUp(0f);
            CoreDamageTypes coreDamageTypes = CoreDamageTypes.None;
            if (explosionData.doDamage && explosionData.damageRadius < 10f && projectile)
            {
                if (projectile.AppliesFreeze) { coreDamageTypes |= CoreDamageTypes.Ice; }
                if (projectile.AppliesFire) { coreDamageTypes |= CoreDamageTypes.Fire; }
                if (projectile.AppliesPoison) { coreDamageTypes |= CoreDamageTypes.Poison; }
                if (projectile.statusEffectsToApply != null)
                {
                    for (int j = 0; j < projectile.statusEffectsToApply.Count; j++)
                    {
                        GameActorEffect gameActorEffect = projectile.statusEffectsToApply[j];
                        if (gameActorEffect is GameActorFreezeEffect) { coreDamageTypes |= CoreDamageTypes.Ice; }
                        else if (gameActorEffect is GameActorFireEffect) { coreDamageTypes |= CoreDamageTypes.Fire; }
                        else if (gameActorEffect is GameActorHealthEffect) { coreDamageTypes |= CoreDamageTypes.Poison; }
                    }
                }
            }
            GameManager.Instance.StartCoroutine(Explode(vector, DataCloners.CopyExplosionData(explosionData), coreDamageTypes, IgnoreQueues, projectile.ignoreDamageCaps, doDistortionWave ? distortionIntensity : -1, distortionRadius, maxDistortionRadius, distortionDuration, waitTime, vfx, Suck));

        }
        private IEnumerator Explode(Vector3 vec, ExplosionData explosionData, CoreDamageTypes damageTypes, bool ignoreQueues, bool ignoreDamageCaps, float distortionIntensity, float distorionRadius, float maxDistortionRadius, float distortionDuration, float waitTime, GameObject vfx, bool doSuck)
        {
            if (vfx) UnityEngine.Object.Instantiate<GameObject>(vfx, vec, Quaternion.identity);
            float elapsed = 0;
            while (elapsed < waitTime)
            {
                if (doSuck) { Exploder.DoRadialKnockback(vec, -(100 * BraveTime.DeltaTime), 10); }
                elapsed += BraveTime.DeltaTime;
                yield return null;
            }
            Exploder.Explode(vec, explosionData, Vector2.zero, null, ignoreQueues, damageTypes, ignoreDamageCaps);


            if (distortionIntensity != -1)
            {
                Exploder.DoDistortionWave(vec, distortionIntensity, distorionRadius, maxDistortionRadius, distortionDuration);
            }
            yield break;
        }
        public bool Suck = false;
        public GameObject vfx;
        public float waitTime;
        public ExplosionData explosionData;
        public bool doDistortionWave;
        public float distortionIntensity = 1f;
        public float distortionRadius = 1f;
        public float maxDistortionRadius = 10f;
        public float distortionDuration = 0.5f;
        public bool IgnoreQueues;
    }
}
