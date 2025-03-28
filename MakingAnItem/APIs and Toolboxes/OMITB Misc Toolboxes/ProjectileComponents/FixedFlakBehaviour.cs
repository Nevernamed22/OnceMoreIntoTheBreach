using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.Misc;
using HarmonyLib;

namespace NevernamedsItems
{
    public class FixedFlakBehaviour : BraveBehaviour
    {
        private void Start()
        {
            if (base.projectile)
            {
                projectile.OnHitEnemy += OnDest;
            }
        }
        public void AddProjectile(Projectile proj, float angle)
        {
            bullets.Add(proj);
            angles.Add(angle);
        }
        private void OnDest(Projectile proj, SpeculativeRigidbody en, bool f)
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                if (bullets[i] != null)
                {
                    Projectile instantiatedKey = bullets[i].InstantiateAndFireInDirection(proj.SafeCenter, Angle(angles[i]), angleVariance, null).GetComponent<Projectile>();
                    if (en && instantiatedKey.specRigidbody) { instantiatedKey.specRigidbody.RegisterSpecificCollisionException(en); }
                    instantiatedKey.Owner = proj.Owner;
                    instantiatedKey.Shooter = proj.Shooter;
                    if (proj.ProjectilePlayerOwner())
                    {
                        instantiatedKey.ScaleByPlayerStats(proj.ProjectilePlayerOwner());
                        if (postProcess) { proj.ProjectilePlayerOwner().DoPostProcessProjectile(instantiatedKey); }
                    }
                    if (OnFlakSpawn != null) { OnFlakSpawn(instantiatedKey); }
                }
            }
        }
        private float Angle(float k)
        {
            if (angleIsRelative && base.projectile)
            {
                float pr = k;
                k = base.projectile.Direction.ToAngle() + pr;
            }
            return k;
        }
        public Action<Projectile> OnFlakSpawn;
        public bool angleIsRelative;
        public bool postProcess;
        public float angleVariance = 0f;
        [SerializeField]
        private List<Projectile> bullets = new List<Projectile>();
        [SerializeField]
        private List<float> angles = new List<float>();
    }


}
