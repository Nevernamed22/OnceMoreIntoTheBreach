using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    class CompanionisedEnemyUtility
    {
        public static void InitHooks()
        {
            DisplaceHook = new Hook(
    typeof(DisplaceBehavior).GetMethod("SpawnImage", BindingFlags.Instance | BindingFlags.NonPublic),
    typeof(CompanionisedEnemyUtility).GetMethod("DisplacedImageSpawnHook", BindingFlags.Instance | BindingFlags.NonPublic),
    typeof(DisplaceBehavior));
        }
        public static Hook DisplaceHook;
        private void DisplacedImageSpawnHook(Action<DisplaceBehavior> orig, DisplaceBehavior sourceBehaviour)
        {
            orig(sourceBehaviour);
            AIActor attkOwner = sourceBehaviour.GetAttackBehaviourOwner();
            if (attkOwner != null)
            {
                if (attkOwner.GetComponent<CustomEnemyTagsSystem>() != null && attkOwner.GetComponent<CustomEnemyTagsSystem>().isKalibersEyeMinion)
                {
                    AIActor image = OMITBReflectionHelpers.ReflectGetField<AIActor>(typeof(DisplaceBehavior), "m_image", sourceBehaviour);
                    if (image != null)
                    {
                        PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(image.specRigidbody, null, false);

                        CustomEnemyTagsSystem tags = image.gameObject.GetOrAddComponent<CustomEnemyTagsSystem>();
                        tags.isKalibersEyeMinion = true;
                        tags.ignoreForGoodMimic = true;

                        if (attkOwner.CompanionOwner != null)
                        {
                            CompanionController orAddComponent = image.gameObject.GetOrAddComponent<CompanionController>();
                            orAddComponent.companionID = CompanionController.CompanionIdentifier.NONE;
                            orAddComponent.Initialize(attkOwner.CompanionOwner);
                        }

                        image.OverrideHitEnemies = true;
                        image.CollisionDamage = 0.5f;
                        image.CollisionDamageTypes |= CoreDamageTypes.Electric;

                        CompanionisedEnemyBulletModifiers companionisedBullets = image.gameObject.GetOrAddComponent<CompanionisedEnemyBulletModifiers>();
                        companionisedBullets.jammedDamageMultiplier = 2f;
                        companionisedBullets.TintBullets = true;
                        companionisedBullets.TintColor = ExtendedColours.honeyYellow;
                        companionisedBullets.baseBulletDamage = 10f;
                        companionisedBullets.scaleSpeed = true;
                        companionisedBullets.scaleDamage = true;
                        companionisedBullets.scaleSize = false;
                        companionisedBullets.doPostProcess = false;
                        if (attkOwner.CompanionOwner != null) companionisedBullets.enemyOwner = attkOwner.CompanionOwner;

                        image.ApplyEffect(GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultPermanentCharmEffect, 1f, null);
                        image.gameObject.AddComponent<KillOnRoomClear>();

                        
                        if (EasyEnemyTypeLists.MultiPhaseEnemies.Contains(image.EnemyGuid) || EasyEnemyTypeLists.EnemiesWithInvulnerablePhases.Contains(image.EnemyGuid))
                        {
                            EraseFromExistenceOnRoomClear destroyTrickyEnemy = image.gameObject.AddComponent<EraseFromExistenceOnRoomClear>();
                            destroyTrickyEnemy.Delay = 1f;
                        }
                        image.IsHarmlessEnemy = true;
                        image.RegisterOverrideColor(Color.grey, "Ressurection");
                        image.IgnoreForRoomClear = true;
                        if (image.gameObject.GetComponent<SpawnEnemyOnDeath>())
                        {
                          UnityEngine.Object.Destroy(image.gameObject.GetComponent<SpawnEnemyOnDeath>());
                        }
                    }
                }
            }
        }
        public static AIActor SpawnCompanionisedEnemy(PlayerController owner, string enemyGuid, IntVector2 position, bool doTint, Color enemyTint, int baseDMG, int jammedDMGMult, bool shouldBeJammed)
        {
            var enemyToSpawn = EnemyDatabase.GetOrLoadByGuid(enemyGuid);
            UnityEngine.Object.Instantiate<GameObject>(EasyVFXDatabase.BloodiedScarfPoofVFX, position.ToVector3(), Quaternion.identity);
            AIActor TargetActor = AIActor.Spawn(enemyToSpawn, position, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(position), true, AIActor.AwakenAnimationType.Default, true);
            PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(TargetActor.specRigidbody, null, false);

            CompanionController orAddComponent = TargetActor.gameObject.GetOrAddComponent<CompanionController>();
            orAddComponent.companionID = CompanionController.CompanionIdentifier.NONE;
            orAddComponent.Initialize(owner);

            if (shouldBeJammed == true)
            {
                TargetActor.BecomeBlackPhantom();
            }
            CompanionisedEnemyBulletModifiers companionisedBullets = TargetActor.gameObject.GetOrAddComponent<CompanionisedEnemyBulletModifiers>();
            companionisedBullets.jammedDamageMultiplier = jammedDMGMult;
            companionisedBullets.TintBullets = true;
            companionisedBullets.TintColor = ExtendedColours.honeyYellow;
            companionisedBullets.baseBulletDamage = baseDMG;

            TargetActor.ApplyEffect(GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultPermanentCharmEffect, 1f, null);
            TargetActor.gameObject.AddComponent<KillOnRoomClear>();
            ContinualKillOnRoomClear contKill = TargetActor.gameObject.AddComponent<ContinualKillOnRoomClear>();
            if (EasyEnemyTypeLists.MultiPhaseEnemies.Contains(TargetActor.EnemyGuid))
            {
                contKill.forceExplode = true;
                contKill.eraseInsteadOfKill = true;
            }
            TargetActor.IsHarmlessEnemy = true;
            if (doTint) TargetActor.RegisterOverrideColor(enemyTint, "CompanionisedEnemyTint");
            TargetActor.IgnoreForRoomClear = true;

            return TargetActor;
        }
    }
    public class ContinualKillOnRoomClear : MonoBehaviour
    {
        public ContinualKillOnRoomClear()
        {
            this.lengthOfNonCombatSurvival = 1f;
        }
        private void Start()
        {
            this.self = base.GetComponent<AIActor>();
        }
        private void Update()
        {
            bool anyPlayerIsInCombat = GameManager.Instance.PrimaryPlayer.IsInCombat;
            if (GameManager.Instance.SecondaryPlayer != null || GameManager.Instance.SecondaryPlayer.IsInCombat) anyPlayerIsInCombat = true;
            if (!anyPlayerIsInCombat && !ithasBegun)
            {
                ithasBegun = true;
                Invoke("Suicide", lengthOfNonCombatSurvival);
            }
        }
        private void Suicide()
        {
            if (self & self.healthHaver && self.healthHaver.IsAlive)
            {
                if (forceExplode) Exploder.DoDefaultExplosion(self.specRigidbody.UnitCenter, Vector2.zero);
                if (eraseInsteadOfKill)
                {
                    self.EraseFromExistenceWithRewards();
                }
                else
                {
                    if (self.healthHaver)
                    {
                        self.healthHaver.ApplyDamage(float.MaxValue, Vector2.zero, "Erasure");
                    }
                }
            }
        }
        private bool ithasBegun = false;
        private AIActor self;
        public float lengthOfNonCombatSurvival;
        public bool eraseInsteadOfKill;
        public bool forceExplode;
    }
}
