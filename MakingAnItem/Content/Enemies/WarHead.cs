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
using Dungeonator;

namespace NevernamedsItems
{
    public class WarHead
    {
        public static tk2dSpriteCollectionData WarHeadSpriteCollection;
        public static tk2dSpriteAnimation WarHeadAnimationCollection;

        public static GameObject prefab;
        public static readonly string guid = "omitb_warhead";

        public static void Init()
        {
            WarHeadSpriteCollection = AssetBundleLoader.FastLoadSpriteCollection(Initialisation.assetBundle, "ENM_WarHead_Collection", "ENM_WarHead_Collection_Material");
            WarHeadAnimationCollection = Initialisation.assetBundle.LoadAsset<GameObject>("ENM_WarHead_Anim").GetComponent<tk2dSpriteAnimation>();


            var enemy = EntityTools.SetupEntityObject("War Head", "warhead_idleright_001", WarHeadSpriteCollection);

            var enemyRigidBody = EntityTools.SetupEntityRigidBody(enemy,
                new List<CollisionLayer>() { CollisionLayer.EnemyCollider, CollisionLayer.EnemyHitBox }, //Collision Layers
                new List<IntVector2>() { new IntVector2(13, 5), new IntVector2(13, 17) }, //Collider Dimensions
                new List<IntVector2>() { new IntVector2(-6, 1), new IntVector2(-6, 1) } //Collider Offsets
                );

            EntityTools.SetupEntityKnockback(enemy);

            tk2dSpriteAnimator animator = enemy.GetOrAddComponent<tk2dSpriteAnimator>();
            animator.Library = WarHeadAnimationCollection;
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
                    "move_backright",
                    "move_right",
                    "move_left",
                    "move_backleft"
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
                            "death_right",
                            "death_left",
                         },
                        Flipped = new DirectionalAnimation.FlipType[2]
                    }
                });

            EntityTools.AddFootstepSounds(AIanimator, new int[] { 1, 4 });

            animator.GetClipByName("attack").frames[5].triggerEvent = true;
            animator.GetClipByName("attack").frames[5].eventInfo = "fire_rocket";
            animator.GetClipByName("attack").frames[5].eventAudio = "Play_ENM_holy_cross_01";


            var healthHaver = enemy.AddComponent<HealthHaver>();
            healthHaver.RegisterBodySprite(enemy.GetComponent<tk2dBaseSprite>());
            healthHaver.PreventAllDamage = false;
            healthHaver.SetHealthMaximum(25);
            healthHaver.ForceSetCurrentHealth(25f);

            var aiActor = enemy.AddComponent<AIActor>();
            aiActor.MovementSpeed = 3f;
            aiActor.CollisionDamage = 0.5f;
            aiActor.State = AIActor.ActorState.Normal;
            aiActor.CanTargetPlayers = true;
            aiActor.CanTargetEnemies = false;
            aiActor.ForcedPositionInAmmonomicon = 45;
            aiActor.EnemyGuid = guid;
            aiActor.AvoidRadius = 4;
            aiActor.ActorName = "War Head";
            aiActor.EnemySwitchState = "";
            aiActor.BlackPhantomProperties = new BlackPhantomProperties();
            aiActor.BlackPhantomProperties.BonusHealthPercentIncrease = 2.2f;

            var bs = enemy.GetOrAddComponent<BehaviorSpeculator>();
            bs.PostAwakenDelay = 0.5f;
            bs.AttackBehaviors = new List<AttackBehaviorBase>();
            bs.OverrideBehaviors = new List<OverrideBehaviorBase>();
            bs.OtherBehaviors = new List<BehaviorBase>();

            GameObject shootPoint = new GameObject("shootPoint");
            shootPoint.transform.SetParent(enemy.transform);
            shootPoint.transform.localPosition = new Vector3(0f, 20f / 16f, 0f);

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
                    LineOfSight = false,
                    ObjectPermanence = true,
                    SearchInterval = 0.25f,
                    PauseOnTargetSwitch = false,
                    PauseTime = 0.25f,
                }
            };
            bs.MovementBehaviors = new List<MovementBehaviorBase>
            {
                new MoveErraticallyBehavior
                {
                    AvoidTarget = true,
                    PathInterval = 1f,
                    PointReachedPauseTime = 2f,
                    PreventFiringWhileMoving = false,
                    UseTargetsRoom = false,
                    StayOnScreen = false,
                }
            };
            bs.AttackBehaviors = new List<AttackBehaviorBase>
            {
                new WarHeadRocketBehaviour
                {
                    TellAnimation = "attack",
                    Rocket = StandardisedProjectiles.skyRocket,
                    RocketOrigin = shootPoint.transform,
                }
            };

            DebrisObject corpseObject = Breakables.GenerateDebrisObject(WarHeadSpriteCollection, "warhead_death_left_007",
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


            SpriteBuilder.AddToAmmonomicon(WarHeadSpriteCollection.GetSpriteDefinition("warhead_idleright_001"));

            ETGMod.Databases.Strings.Enemies.AddComplex("#WARHEAD_ENCNAME", "War Head");
            ETGMod.Databases.Strings.Enemies.AddComplex("#WARHEAD_SHORTDESC", "");
            ETGMod.Databases.Strings.Enemies.AddComplex("#WARHEAD_LONGDESC", "");

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
                AmmonomiconSprite = "warhead_idleright_001",
                PrimaryDisplayName = "#WARHEAD_ENCNAME",
                NotificationPanelDescription = "#WARHEAD_SHORTDESC",
                AmmonomiconFullEntry = "#WARHEAD_LONGDESC",
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
            Game.Enemies.Add("nn:war_head", aiActor);
        }
        public class WarHeadRocketBehaviour : BasicAttackBehavior
        {
            public string TellAnimation;
            public Transform RocketOrigin;
            public GameObject Rocket;
            public override void Start()
            {
                base.Start();
                if (!string.IsNullOrEmpty(TellAnimation))
                {
                    this.m_aiAnimator.PlayUntilFinished(this.TellAnimation, true, null, -1f, false);
                    tk2dSpriteAnimator spriteAnimator = this.m_aiAnimator.spriteAnimator;
                    spriteAnimator.AnimationEventTriggered += HandleAnimationEvent;
                }
            }
            public override void Upkeep()
            {
                base.Upkeep();
            }
            public override bool IsReady()
            {
                return base.IsReady();
            }
            public override BehaviorResult Update()
            {
                base.Update();
                if (!IsReady()) { return BehaviorResult.Continue; }
                return  BehaviorResult.SkipAllRemainingBehaviors;
            }
            private void HandleAnimationEvent(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frameNo)
            {
                tk2dSpriteAnimationFrame frame = clip.GetFrame(frameNo);
                if (frame.eventInfo == "fire_rocket")
                {
                    this.FireRocket();
                }
            }
            private void FireRocket()
            {
                SkyRocket component = SpawnManager.SpawnProjectile(this.Rocket, this.RocketOrigin.position, Quaternion.identity, true).GetComponent<SkyRocket>();
                component.Target = this.m_aiActor.TargetRigidbody;
                tk2dSprite componentInChildren = component.GetComponentInChildren<tk2dSprite>();
                component.transform.position = component.transform.position.WithY(component.transform.position.y - componentInChildren.transform.localPosition.y);
                component.ExplosionData.ignoreList.Add(this.m_aiActor.specRigidbody);
                //this.m_aiActor.sprite.AttachRenderer(component.GetComponentInChildren<tk2dSprite>());
            }
        }
    }
}
