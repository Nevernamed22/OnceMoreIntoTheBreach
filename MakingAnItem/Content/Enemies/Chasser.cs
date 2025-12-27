using Alexandria.BreakableAPI;
using Alexandria.EnemyAPI;
using Alexandria.ItemAPI;
using Alexandria.CharacterAPI;
using Brave.BulletScript;
using Gungeon;
using HutongGames.PlayMaker.Actions;
using Microsoft.Cci;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class Chasser
    {
        public static tk2dSpriteCollectionData ChasserSpriteCollection;
        public static tk2dSpriteAnimation ChasserAnimationCollection;

        public static GameObject prefab;
        public static readonly string guid = "omitb_chasser";
        public static void Init()
        {
            ChasserSpriteCollection = AssetBundleLoader.FastLoadSpriteCollection(Initialisation.assetBundle, "ENM_Chasser_Collection", "ENM_Chasser_Collection_Material");
            ChasserAnimationCollection = Initialisation.assetBundle.LoadAsset<GameObject>("ENM_Chasser_Anim").GetComponent<tk2dSpriteAnimation>();


            var enemy = EntityTools.SetupEntityObject("Chasser", "chasser_idleright_001", ChasserSpriteCollection);

            var enemyRigidBody = EntityTools.SetupEntityRigidBody(enemy,
                new List<CollisionLayer>() { CollisionLayer.EnemyCollider, CollisionLayer.EnemyHitBox }, //Collision Layers
                new List<IntVector2>() { new IntVector2(13, 5), new IntVector2(13, 17) }, //Collider Dimensions
                new List<IntVector2>() { new IntVector2(-6, 1), new IntVector2(-6, 1) } //Collider Offsets
                );

            EntityTools.SetupEntityKnockback(enemy);

            tk2dSpriteAnimator animator = enemy.GetOrAddComponent<tk2dSpriteAnimator>();
            animator.Library = ChasserAnimationCollection;
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
                            "die_left",
                         },
                        Flipped = new DirectionalAnimation.FlipType[2]
                    }
                });




            var healthHaver = enemy.AddComponent<HealthHaver>();
            healthHaver.RegisterBodySprite(enemy.GetComponent<tk2dBaseSprite>());
            healthHaver.PreventAllDamage = false;
            healthHaver.SetHealthMaximum(15);
            healthHaver.ForceSetCurrentHealth(15f);

            var aiActor = enemy.AddComponent<AIActor>();
            aiActor.MovementSpeed = 5f;
            aiActor.CollisionDamage = 0.5f;
            aiActor.State = AIActor.ActorState.Normal;
            aiActor.CanTargetPlayers = true;
            aiActor.CanTargetEnemies = false;
            aiActor.ForcedPositionInAmmonomicon = 45;
            aiActor.EnemyGuid = guid;
            aiActor.AvoidRadius = 4;
            aiActor.ActorName = "Chasser";
            aiActor.EnemySwitchState = "Powder_Skull";
            aiActor.BlackPhantomProperties = new BlackPhantomProperties();
            aiActor.BlackPhantomProperties.BonusHealthPercentIncrease = 2.2f;

            aiActor.SetIsFlying(true, "Flying Entity", true, true);
            aiActor.shadowHeightOffset = -1f;


            var bs = enemy.GetOrAddComponent<BehaviorSpeculator>();
            bs.PostAwakenDelay = 0.5f;
            bs.AttackBehaviors = new List<AttackBehaviorBase>();
            bs.OverrideBehaviors = new List<OverrideBehaviorBase>();
            bs.OtherBehaviors = new List<BehaviorBase>();

            GameObject shootPoint = new GameObject("shootPoint");
            shootPoint.transform.SetParent(enemy.transform);
            shootPoint.transform.localPosition = new Vector3(0f, 11f / 16f, 0f);

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
                    AudioEvent = "Play_WPN_earthwormgun_shot_01",
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
                new SmoothSeekTargetBehavior
                {
                    bob = true,
                    bobMagnitude = 1f,
                    bobPeriod = 1f,
                    bobPeriodVariance = 0.2f,
                    pathfind = true,
                    pathInterval =0.25f,
                    stoppedTurnMultiplier = 3f,
                    targetTolerance = 30f,
                    turnTime = 0.5f,             
                }
            };
            bs.AttackBehaviors = new List<AttackBehaviorBase>
            {
                new ShootBehavior
                {
                    RequiresLineOfSight = false,
                    Cooldown = 0.25f,
                    Range = 20f,
                    BulletScript = new CustomBulletScriptSelector(typeof(OMITBChasser_SingleShot)),
                    RequiresTarget = true,
                    InitialCooldown = 2f,
                    ImmobileDuringStop = false,
                    ShootPoint = shootPoint,
                    StopDuring = ShootBehavior.StopType.None,
                }
            };

            aiActor.CorpseObject = null;

            ObjectVisibilityManager vis = enemy.AddComponent<ObjectVisibilityManager>();


            SpriteBuilder.AddToAmmonomicon(ChasserSpriteCollection.GetSpriteDefinition("chasser_idleright_001"));

            ETGMod.Databases.Strings.Enemies.AddComplex("#CHASSER_ENCNAME", "Chasser");
            ETGMod.Databases.Strings.Enemies.AddComplex("#CHASSER_SHORTDESC", "Chasse Sauvage");
            ETGMod.Databases.Strings.Enemies.AddComplex("#CHASSER_LONGDESC", "The dark powder rites and bloody ammomancy used to reload life into the dreaded skullet is not exclusive to organic remains.\n\nWhile weaker than their true bone relatives, this crude leaden facsimile of a cranium serves as a passable host for such magicks.");

            EncounterTrackable tracker = enemy.gameObject.GetOrAddComponent<EncounterTrackable>();
            tracker.DoNotificationOnEncounter = true;
            tracker.EncounterGuid = guid;
            tracker.IgnoreDifferentiator = false;
            tracker.TrueEncounterGuid = guid;
            tracker.tag = "Enemy";
            tracker.prerequisites = new DungeonPrerequisite[0];
            tracker.ProxyEncounterGuid = string.Empty;
            tracker.journalData = new JournalEntry()
            {
                enemyPortraitSprite = Initialisation.assetBundle.LoadAsset<Texture2D>("ammonomicon_chasser"),
                AmmonomiconSprite = "chasser_idleright_001",
                PrimaryDisplayName = "#CHASSER_ENCNAME",
                NotificationPanelDescription = "#CHASSER_SHORTDESC",
                AmmonomiconFullEntry = "#CHASSER_LONGDESC",
                SpecialIdentifier = JournalEntry.CustomJournalEntryType.NONE,
                SuppressKnownState = false,
                SuppressInAmmonomicon = false,
                IsEnemy = true,
                DisplayOnLoadingScreen = false,
                RequiresLightBackgroundInLoadingScreen = false
            };

            EncounterDatabaseEntry encounterEntry = new EncounterDatabaseEntry()
            {
                doesntDamageSecretWalls = false,
                doNotificationOnEncounter = false,
                journalData = tracker.journalData,
                isInfiniteAmmoGun = false,
                isPassiveItem = false,
                isPlayerItem = false,
                prerequisites = new DungeonPrerequisite[0],
                path = guid,
                myGuid = guid,
            };

            EnemyDatabaseEntry enemyDatabaseEntry = new EnemyDatabaseEntry()
            {
                myGuid = guid,
                placeableWidth = 2,
                placeableHeight = 2,
                isNormalEnemy = true,
                isInBossTab = false,
                encounterGuid = guid,
                ForcedPositionInAmmonomicon = 45,
            };

            prefab = enemy;
            EncounterDatabase.Instance.Entries.Add(encounterEntry);
            EnemyDatabase.Instance.Entries.Add(enemyDatabaseEntry);
            EnemyBuilder.Dictionary.Add(guid, prefab);

            GameObject.DontDestroyOnLoad(prefab);
            FakePrefab.MarkAsFakePrefab(prefab);
            prefab.SetActive(false);
            Game.Enemies.Add("nn:chasser", aiActor);
        }
        public class OMITBChasser_SingleShot : Script
        {
            public override IEnumerator Top()
            {
                base.Fire(
                       new Direction(0, DirectionType.Aim, -1f),
                       new Speed(0f, SpeedType.Absolute),
                       new LingerAndPop()
                       );
                yield break;
            }
            public class LingerAndPop : Bullet
            {
                public LingerAndPop() : base("default", false, false, false)
                {
                }
                Vector2 initialPosition = Vector2.zero;
                public override IEnumerator Top()
                {
                    yield return null;
                    initialPosition = base.Position;
                    
                    float lifeTime = 0f;
                    for (int i =0; i < 1000; i++)
                    {
                        lifeTime += base.LocalDeltaTime;
                        base.Position = initialPosition + new Vector2(0f, Mathf.PingPong(lifeTime, 0.5f));
                        yield return Wait(1);
                    }

                    base.Vanish();
                    yield break;
                }

            }
        }
    }
}
