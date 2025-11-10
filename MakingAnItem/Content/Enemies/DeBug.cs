using Alexandria.BreakableAPI;
using Alexandria.EnemyAPI;
using Alexandria.ItemAPI;
using Alexandria.NPCAPI;
using Brave.BulletScript;
using Gungeon;
using Microsoft.Cci;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class DebugEnemy
    {
        public static tk2dSpriteCollectionData DebugEnemySpriteCollection;
        public static tk2dSpriteAnimation DebugEnemyAnimationCollection;

        public static GameObject prefab;
        public static readonly string guid = "omitb_debug_lkjhgfdgglkjhgfdggggggmnbvcxaaassd";
        public static void Init()
        {
            DebugEnemySpriteCollection = AssetBundleLoader.FastLoadSpriteCollection(Initialisation.assetBundle, "ENM_Debug_Collection", "ENM_Debug_Collection_Material");
            DebugEnemyAnimationCollection = Initialisation.assetBundle.LoadAsset<GameObject>("ENM_Debug_Anim").GetComponent<tk2dSpriteAnimation>();


            var enemy = EntityTools.SetupEntityObject("De-Bug", "debugenemy_idleright_001", DebugEnemySpriteCollection);

            var enemyBody = EntityTools.SetupEntityRigidBody(enemy,
                new List<CollisionLayer>() { CollisionLayer.EnemyCollider, CollisionLayer.EnemyHitBox }, //Collision Layers
                new List<IntVector2>() { new IntVector2(10, 4), new IntVector2(10, 18) }, //Collider Dimensions
                new List<IntVector2>() { new IntVector2(-5, 1), new IntVector2(-5, 1) } //Collider Offsets
                );

            EntityTools.SetupEntityKnockback(enemy);

            tk2dSpriteAnimator animator = enemy.GetOrAddComponent<tk2dSpriteAnimator>();
            animator.Library = DebugEnemyAnimationCollection;
            AIAnimator AIanimator = enemy.AddComponent<AIAnimator>();
            AIanimator.IdleAnimation = new DirectionalAnimation()
            {
                Type = DirectionalAnimation.DirectionType.FourWay,
                AnimNames = new string[]
                {
                    "idle_backright",
                    "idle_right",
                    "idle_left",
                    "idle_backleft"
                },
                Flipped = new DirectionalAnimation.FlipType[4]
            };
            AIanimator.MoveAnimation = new DirectionalAnimation()
            {
                Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
                AnimNames = new string[]
                {
                    "move_right",
                    "move_left",
                },
                Flipped = new DirectionalAnimation.FlipType[2]
            };
            AIanimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>();
            AIanimator.OtherAnimations.Add(
                new AIAnimator.NamedDirectionalAnimation()
                {
                    name = "death",
                    anim = new DirectionalAnimation()
                    {
                        Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
                        AnimNames = new string[]
                         {
                            "die_right",
                            "die_left"
                         },
                        Flipped = new DirectionalAnimation.FlipType[2]
                    }
                });



            var healthHaver = enemy.AddComponent<HealthHaver>();
            healthHaver.RegisterBodySprite(enemy.GetComponent<tk2dBaseSprite>());
            healthHaver.PreventAllDamage = false;
            healthHaver.SetHealthMaximum(30);
            healthHaver.ForceSetCurrentHealth(30f);

            var aiActor = enemy.AddComponent<AIActor>();
            aiActor.MovementSpeed = 2f;
            aiActor.CollisionDamage = 0.5f;
            aiActor.State = AIActor.ActorState.Normal;
            aiActor.CanTargetPlayers = true;
            aiActor.CanTargetEnemies = false;
            aiActor.ForcedPositionInAmmonomicon = 16;
            aiActor.EnemyGuid = guid;
            aiActor.AvoidRadius = 4;
            aiActor.ActorName = "De-Bug";
            aiActor.EnemySwitchState = "Plastic_Bullet_Man";
            aiActor.OverridePitfallAnim = "pitfall";
            aiActor.BlackPhantomProperties = new BlackPhantomProperties();
            aiActor.BlackPhantomProperties.BonusHealthPercentIncrease = 2.2f;

            GameObject gunAttach = new GameObject("GunAttachPoint");
            gunAttach.transform.SetParent(enemy.transform);
            gunAttach.transform.localPosition = new Vector3(-0.5f, 0.2f, 0f);

            AIShooter shooter = enemy.AddComponent<AIShooter>();
            shooter.AimTimeScale = 1;
            shooter.gunAttachPoint = gunAttach.transform;
            shooter.equippedGunId = 38;
            shooter.shouldUseGunReload = true;
            shooter.customShootCooldownPeriod = 2.5f;
            shooter.handObject = EnemyDatabase.GetOrLoadByGuid("128db2f0781141bcb505d8f00f9e4d47").aiShooter.handObject;

            var bs = enemy.GetOrAddComponent<BehaviorSpeculator>();

            bs.AttackBehaviors = new List<AttackBehaviorBase>();
            bs.OverrideBehaviors = new List<OverrideBehaviorBase>();
            bs.OtherBehaviors = new List<BehaviorBase>();

            AIBulletBank bank = enemy.GetOrAddComponent<AIBulletBank>();
            bank.CollidesWithEnemies = false;
            bank.PlayAudio = true;
            bank.PlayShells = true;
            bank.PlayVfx = true;
            bank.Bullets = new List<AIBulletBank.Entry>()
            {
                new AIBulletBank.Entry()
                {
                    Name = "default",
                    BulletObject = EnemyBulletDatabase.Regular_EightByEight,
                    AudioEvent = "Play_WPN_shotgun_shot_01",
                    AudioLimitOncePerAttack = false,
                    AudioLimitOncePerFrame = true,
                    AudioSwitch = "",
                    conditionalMinDegFromNorth = 0,
                    OverrideProjectile = true,
                    PlayAudio = true,
                    preloadCount = 0,
                    ProjectileData = new ProjectileData()
                    {
                        CustomAccelerationCurveDuration = 1,
                        damage = 0.5f,
                        damping = 0,
                        force = 10,
                        IgnoreAccelCurveTime = 0,
                        range = 20,
                        speed = 8,
                        UsesCustomAccelerationCurve = false,
                        onDestroyBulletScript = new BulletScriptSelector(){ scriptTypeName = "" }
                    },
                    rampBullets = false,
                    rampStartHeight = 2,
                    rampTime = 1,
                    ShellForce = 1.75f,
                    ShellForceVariance = 0.75f,
                    ShellGroundOffset = 0f,
                    ShellPrefab = null,
                    ShellsLimitOncePerFrame = false,
                    ShellTransform = null,
                    SpawnShells = false,
                    suppressHitEffectsIfOffscreen = false,
                },
                new AIBulletBank.Entry()
                {
                    Name = "bouncer",
                    BulletObject = EnemyBulletDatabase.Bouncing,
                    AudioEvent = "Play_WPN_shotgun_shot_01",
                    AudioLimitOncePerAttack = false,
                    AudioLimitOncePerFrame = true,
                    AudioSwitch = "",
                    conditionalMinDegFromNorth = 0,
                    OverrideProjectile = true,
                    PlayAudio = true,
                    preloadCount = 0,
                    ProjectileData = new ProjectileData()
                    {
                        CustomAccelerationCurveDuration = 1,
                        damage = 0.5f,
                        damping = 0,
                        force = 10,
                        IgnoreAccelCurveTime = 0,
                        range = 20,
                        speed = 8,
                        UsesCustomAccelerationCurve = false,
                        onDestroyBulletScript = new BulletScriptSelector(){ scriptTypeName = "" }
                    },
                    rampBullets = false,
                    rampStartHeight = 2,
                    rampTime = 1,
                    ShellForce = 1.75f,
                    ShellForceVariance = 0.75f,
                    ShellGroundOffset = 0f,
                    ShellPrefab = null,
                    ShellsLimitOncePerFrame = false,
                    ShellTransform = null,
                    SpawnShells = false,
                    suppressHitEffectsIfOffscreen = false,
                },
                new AIBulletBank.Entry()
                {
                    Name = "laser",
                    BulletObject = EnemyBulletDatabase.Narrow_Ellipse,
                    AudioEvent = "Play_WPN_shotgun_shot_01",
                    AudioLimitOncePerAttack = false,
                    AudioLimitOncePerFrame = true,
                    AudioSwitch = "",
                    conditionalMinDegFromNorth = 0,
                    OverrideProjectile = true,
                    PlayAudio = false,
                    preloadCount = 0,
                    ProjectileData = new ProjectileData()
                    {
                        CustomAccelerationCurveDuration = 1,
                        damage = 0.5f,
                        damping = 0,
                        force = 10,
                        IgnoreAccelCurveTime = 0,
                        range = 20,
                        speed = 8,
                        UsesCustomAccelerationCurve = false,
                        onDestroyBulletScript = new BulletScriptSelector(){ scriptTypeName = "" }
                    },
                    rampBullets = false,
                    rampStartHeight = 2,
                    rampTime = 1,
                    ShellForce = 1.75f,
                    ShellForceVariance = 0.75f,
                    ShellGroundOffset = 0f,
                    ShellPrefab = null,
                    ShellsLimitOncePerFrame = false,
                    ShellTransform = null,
                    SpawnShells = false,
                    suppressHitEffectsIfOffscreen = false,
                }
            };


            bs.TargetBehaviors = new List<TargetBehaviorBase>
            {
                new TargetPlayerBehavior
                {
                    Radius = 35f,
                    LineOfSight = true,
                    ObjectPermanence = true,
                    SearchInterval = 0.25f,
                    PauseOnTargetSwitch = false,
                    PauseTime = 0.25f,
                }
            };
            bs.MovementBehaviors = new List<MovementBehaviorBase>
            {
                new SeekTargetBehavior
                {
                    CustomRange = 6,
                    ExternalCooldownSource = false,
                    LineOfSight = true,
                    PathInterval = 0.5f,
                    ReturnToSpawn = false,
                    StopWhenInRange = true,
                    SpecifyRange = false,
                    SpawnTetherDistance = 0,
                    MaxActiveRange = 0,
                    MinActiveRange = 0,
                }
            };
            bs.AttackBehaviors = new List<AttackBehaviorBase>
            {
                new ShootGunBehavior
                {
                    GroupCooldownVariance = 0.2f,
                    MagazineCapacity = 1f,
                    ReloadSpeed = 3f,
                    Cooldown = 3.5f,
                    Range = 20f,
                    BulletScript = new CustomBulletScriptSelector(typeof(OMITBBigBouncyShotgun)),
                    RespectReload = true,
                    StopDuringAttack = true,
                    WeaponType = WeaponType.BulletScript,
                }
            };

            DebrisObject corpseObject = Breakables.GenerateDebrisObject(DebugEnemySpriteCollection, "debugenemy_dieright_004",
               debrisObjectsCanRotate: false,
               LifeSpanMin: 1,
               LifeSpanMax: 1,
               AngularVelocity: 0,
               AngularVelocityVariance: 0,
               shadowSprite: null,
               AudioEventName: "",
               BounceVFX: null,
               DebrisBounceCount: 0,
               Mass: 1);
            aiActor.CorpseObject = corpseObject.gameObject;
            aiActor.CorpseShadow = true;

            ObjectVisibilityManager vis = enemy.AddComponent<ObjectVisibilityManager>();


            

            EnemyDatabaseEntry enemyDatabaseEntry = new EnemyDatabaseEntry()
            {
                myGuid = guid,
                placeableWidth = 2,
                placeableHeight = 2,
                isNormalEnemy = true,
                isInBossTab = false,
                encounterGuid = guid,
            };

            prefab = enemy;
            EnemyDatabase.Instance.Entries.Add(enemyDatabaseEntry);
            EnemyBuilder.Dictionary.Add(guid, prefab);

            GameObject.DontDestroyOnLoad(prefab);
            FakePrefab.MarkAsFakePrefab(prefab);
            prefab.SetActive(false);
            Game.Enemies.Add("nn:de_bug", aiActor);
        }
    }
    
}
