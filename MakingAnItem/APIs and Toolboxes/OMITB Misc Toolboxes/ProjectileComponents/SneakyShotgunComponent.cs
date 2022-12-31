using Alexandria.ItemAPI;
using Alexandria.Misc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class SneakyShotgunComponent : MonoBehaviour
    {
        public SneakyShotgunComponent()
        {
            scaleOffOwnerAccuracy = true;
            eraseSource = true;
            numToFire = 5;
            projPrefabToFire = (PickupObjectDatabase.GetById(56) as Gun).DefaultModule.projectiles[0];
            postProcess = true;
            doVelocityRandomiser = true;
            angleVariance = 40;
            scaleMult = 1;
            damageMult = 1;
            overrideProjectileSynergy = null;
            synergyProjectilePrefab = null;
            useComplexPrefabs = false;
        }
        public bool scaleOffOwnerAccuracy;
        public bool eraseSource;
        public float angleVariance;
        public int numToFire;
        public Projectile projPrefabToFire;
        public string overrideProjectileSynergy;
        public Projectile synergyProjectilePrefab;
        public bool postProcess;
        public bool doVelocityRandomiser;
        public float damageMult;
        public float scaleMult;
        public bool useComplexPrefabs;
        public List<Projectile> complexPrefabs = new List<Projectile>();
        private void Start()
        {
            self = base.GetComponent<Projectile>();
            StartCoroutine(handleShotgunBlast());
        }
        private Projectile self;
        private IEnumerator handleShotgunBlast()
        {
            yield return null;
            if (useComplexPrefabs)
            {
                foreach(Projectile proj in complexPrefabs)
                {
                    FireIndiv(proj.gameObject);
                }
            }
            else
            {
                GameObject prefabtouse = projPrefabToFire.gameObject;
                if (!string.IsNullOrEmpty(overrideProjectileSynergy) && synergyProjectilePrefab != null && self.ProjectilePlayerOwner())
                {
                    if (self.ProjectilePlayerOwner().PlayerHasActiveSynergy(overrideProjectileSynergy))
                    {
                        prefabtouse = synergyProjectilePrefab.gameObject;
                    }
                }
                for (int i = 0; i < numToFire; i++)
                {
                    FireIndiv(prefabtouse);
                }
            }
            if (eraseSource)
            {
                UnityEngine.Object.Destroy(self.gameObject);
            }
            yield break;
        }
        private void FireIndiv(GameObject prefabtouse)
        {
            PlayerController accuracyOwner = null;
            if (scaleOffOwnerAccuracy && self.ProjectilePlayerOwner()) accuracyOwner = self.ProjectilePlayerOwner();
            float angle = ProjSpawnHelper.GetAccuracyAngled(self.Direction.ToAngle(), angleVariance, accuracyOwner);
            GameObject spawnObj = SpawnManager.SpawnProjectile(prefabtouse, self.transform.position, Quaternion.Euler(new Vector3(0f, 0f, angle)));
            Projectile component = spawnObj.GetComponent<Projectile>();
            if (component != null)
            {
                component.Owner = self.Owner;
                component.Shooter = self.Shooter;
                if (doVelocityRandomiser) component.baseData.speed *= (1f + UnityEngine.Random.Range(-5f, 5f) / 100f);
                component.UpdateSpeed();
                component.baseData.damage *= damageMult;
                component.baseData.force *= damageMult;
                component.RuntimeUpdateScale(scaleMult);
                if (postProcess && self.ProjectilePlayerOwner()) self.ProjectilePlayerOwner().DoPostProcessProjectile(component);
            }
        }
    }
}
