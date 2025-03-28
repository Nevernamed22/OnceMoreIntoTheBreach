using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class SpawnEnemyOnBulletSpawn : MonoBehaviour
    {
        public SpawnEnemyOnBulletSpawn()
        {
            this.procChance = 1;
            this.deleteProjAfterSpawn = true;
            this.companioniseEnemy = true;
            this.ignoreSpawnedEnemyForGoodMimic = true;
            this.killSpawnedEnemyOnRoomClear = true;
            this.doPostProcessOnEnemyBullets = true;
            this.scaleEnemyDamage = true;
            this.scaleEnemyProjSize = true;
            this.scaleEnemyProjSpeed = true;
            this.enemyBulletDamage = 10f;
        }
        private void Start()
        {
            this.m_projectile = base.GetComponent<Projectile>();
            if (this.m_projectile.Owner is PlayerController) { this.projOwner = this.m_projectile.Owner as PlayerController; }
            GameManager.Instance.StartCoroutine(handleSpawn());
        }
        private IEnumerator handleSpawn()
        {
            yield return null;
            if (UnityEngine.Random.value <= this.procChance)
            {
                if (guidToSpawn != null)
                {
                    var enemyToSpawn = EnemyDatabase.GetOrLoadByGuid(guidToSpawn);
                    var position = this.m_projectile.specRigidbody.UnitCenter;
                    Instantiate<GameObject>(SharedVFX.SpiratTeleportVFX, position, Quaternion.identity);

                    AIActor TargetActor = AIActor.Spawn(enemyToSpawn, position, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(position.ToIntVector2()), true, AIActor.AwakenAnimationType.Default, true);

                    PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(TargetActor.specRigidbody, null, false);

                    if (ignoreSpawnedEnemyForGoodMimic)
                    {
                        CustomEnemyTagsSystem tags = TargetActor.gameObject.GetOrAddComponent<CustomEnemyTagsSystem>();
                        tags.ignoreForGoodMimic = true;
                    }

                    if (companioniseEnemy && this.projOwner != null)
                    {
                        CompanionController orAddComponent = TargetActor.gameObject.GetOrAddComponent<CompanionController>();
                        orAddComponent.companionID = CompanionController.CompanionIdentifier.NONE;
                        orAddComponent.Initialize(this.projOwner);

                        CompanionisedEnemyBulletModifiers companionisedBullets = TargetActor.gameObject.GetOrAddComponent<CompanionisedEnemyBulletModifiers>();
                        companionisedBullets.jammedDamageMultiplier = 2f;
                        companionisedBullets.TintBullets = true;
                        companionisedBullets.TintColor = ExtendedColours.honeyYellow;
                        companionisedBullets.baseBulletDamage = enemyBulletDamage;
                        companionisedBullets.scaleDamage = this.scaleEnemyDamage;
                        companionisedBullets.doPostProcess = this.doPostProcessOnEnemyBullets;
                        companionisedBullets.scaleSize = this.scaleEnemyProjSize;
                        companionisedBullets.scaleSpeed = this.scaleEnemyProjSpeed;
                        companionisedBullets.enemyOwner = this.projOwner;
                    }

                    if (killSpawnedEnemyOnRoomClear)
                    {
                        TargetActor.gameObject.AddComponent<KillOnRoomClear>();
                    }

                    TargetActor.IsHarmlessEnemy = true;
                    TargetActor.IgnoreForRoomClear = true;
                    TargetActor.StartCoroutine(PostSpawn(TargetActor, knockbackAmountAwayFromOwner, m_projectile.Direction));
                    if (TargetActor.gameObject.GetComponent<SpawnEnemyOnDeath>())
                    {
                        Destroy(TargetActor.gameObject.GetComponent<SpawnEnemyOnDeath>());
                    }
                    if (deleteProjAfterSpawn) { Destroy(this.m_projectile.gameObject); }
                }
            }
        }
        private IEnumerator PostSpawn(AIActor spawnedEnemy, float knockbackAway, Vector2 dir)
        {
            yield return null;
            if (knockbackAway > 0)
            {
                if (spawnedEnemy.knockbackDoer)
                {
                    spawnedEnemy.knockbackDoer.ApplyKnockback(dir, knockbackAway);
                }
            }
            yield break;
        }
        public float knockbackAmountAwayFromOwner;
        private Projectile m_projectile;
        private PlayerController projOwner;
        public float procChance;
        public float enemyBulletDamage;
        public bool companioniseEnemy;
        public bool killSpawnedEnemyOnRoomClear;
        public bool deleteProjAfterSpawn;
        public bool ignoreSpawnedEnemyForGoodMimic;
        public string guidToSpawn;
        public bool scaleEnemyDamage;
        public bool scaleEnemyProjSize;
        public bool scaleEnemyProjSpeed;
        public bool doPostProcessOnEnemyBullets;
    }
}
