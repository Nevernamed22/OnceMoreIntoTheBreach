using Alexandria.ItemAPI;
using Alexandria.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class ProjectileSplitController : MonoBehaviour
    {
        public ProjectileSplitController()
        {
            distanceTillSplit = 7.5f;
            splitAngles = 35;
            amtToSplitTo = 0;
            dmgMultAfterSplit = 0.66f;
            sizeMultAfterSplit = 0.8f;
            chanceToSplit = 1;
        }
        private void Start()
        {
            parentProjectile = base.GetComponent<Projectile>();
            parentOwner = parentProjectile.ProjectilePlayerOwner();
        }
        private void Update()
        {
            if (parentProjectile != null && distanceBasedSplit && !hasSplit)
            {
                if (parentProjectile.GetElapsedDistance() > distanceTillSplit)
                {
                    SplitProjectile();
                }
            }
        }
        private void SplitProjectile()
        {
            if (UnityEngine.Random.value <= chanceToSplit)
            {
                float ProjectileInterval = splitAngles / ((float)amtToSplitTo - 1);
                float currentAngle = parentProjectile.Direction.ToAngle();
                float startAngle = currentAngle + (splitAngles * 0.5f);
                int iteration = 0;
                for (int i = 0; i < amtToSplitTo; i++)
                {
                    float finalAngle = startAngle - (ProjectileInterval * iteration);

                    GameObject newBulletOBJ = FakePrefab.Clone(parentProjectile.gameObject);
                    GameObject spawnedBulletOBJ = SpawnManager.SpawnProjectile(newBulletOBJ, parentProjectile.sprite.WorldCenter, Quaternion.Euler(0f, 0f, finalAngle), true);
                    Projectile component = spawnedBulletOBJ.GetComponent<Projectile>();
                    if (component != null)
                    {
                        component.Owner = parentOwner;
                        component.Shooter = parentOwner.specRigidbody;
                        component.baseData.damage *= dmgMultAfterSplit;
                        component.RuntimeUpdateScale(sizeMultAfterSplit);
                        ProjectileSplitController split2 = component.gameObject.GetComponent<ProjectileSplitController>();
                        if (split2)
                        {
                            if (curRecursionAmount < maxRecursionAmount)
                            {
                                split2.curRecursionAmount = curRecursionAmount + 1;
                            }
                            else { UnityEngine.Object.Destroy(split2); }
                        }
                    }

                    iteration++;
                }
                UnityEngine.Object.Destroy(parentProjectile.gameObject);
            }
            hasSplit = true;
        }
        private Projectile parentProjectile;
        private PlayerController parentOwner;
        private bool hasSplit;

        //Publics
        public bool distanceBasedSplit;
        public float distanceTillSplit;
        public bool splitOnEnemy;
        public float splitAngles;
        public int amtToSplitTo;
        public float chanceToSplit;

        public float dmgMultAfterSplit;
        public float sizeMultAfterSplit;

        public int maxRecursionAmount;
        private int curRecursionAmount;
    }
}
