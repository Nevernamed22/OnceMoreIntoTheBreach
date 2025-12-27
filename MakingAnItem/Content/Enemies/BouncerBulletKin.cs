using Alexandria.BreakableAPI;
using Alexandria.EnemyAPI;
using Alexandria.ItemAPI;
using Alexandria.CharacterAPI;
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
    public class BouncerBulletKin
    {
        public static tk2dSpriteCollectionData BouncerBulletKinSpriteCollection;
        public static tk2dSpriteAnimation BouncerBulletKinAnimationCollection;

        public static GameObject prefab;
        public static readonly string guid = "omitb_bouncerbulletkin_iopupkkppoughihuhomomoarfdar";
        public static void Init()
        {
            BouncerBulletKinSpriteCollection = AssetBundleLoader.FastLoadSpriteCollection(Initialisation.assetBundle, "ENM_BouncerBulletKin_Collection", "ENM_BouncerBulletKin_Collection_Material");
            BouncerBulletKinAnimationCollection = Initialisation.assetBundle.LoadAsset<GameObject>("ENM_BouncerBulletKin_Anim").GetComponent<tk2dSpriteAnimation>();


            var enemy = EntityTools.SetupEntityObject("Bouncer Bullet Kin", "bouncerbullet_idle_001", BouncerBulletKinSpriteCollection);

            var enemyRigidBody = EntityTools.SetupEntityRigidBody(enemy,
                new List<CollisionLayer>() { CollisionLayer.EnemyCollider, CollisionLayer.EnemyHitBox }, //Collision Layers
                new List<IntVector2>() { new IntVector2(10, 4), new IntVector2(10, 18) }, //Collider Dimensions
                new List<IntVector2>() { new IntVector2(-5, 1), new IntVector2(-5, 1) } //Collider Offsets
                );

            EntityTools.SetupEntityKnockback(enemy);

            tk2dSpriteAnimator animator = enemy.GetOrAddComponent<tk2dSpriteAnimator>();
            animator.Library = BouncerBulletKinAnimationCollection;
            AIAnimator AIanimator = enemy.AddComponent<AIAnimator>();
            AIanimator.IdleAnimation = new DirectionalAnimation()
            {
                Type = DirectionalAnimation.DirectionType.FourWay,
                AnimNames = new string[]
                {
                    "idle_back",
                    "idle_right",
                    "idle_left",
                    "idle_back"
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
                        Type = DirectionalAnimation.DirectionType.FourWay,
                        AnimNames = new string[]
                         {
                            "die_backright",
                            "die_right",
                            "die_left",
                            "die_backleft"
                         },
                        Flipped = new DirectionalAnimation.FlipType[4]
                    }
                });

            EntityTools.AddFootstepSounds(AIanimator, new int[] { 1, 4 });


            var healthHaver = enemy.AddComponent<HealthHaver>();
            healthHaver.RegisterBodySprite(enemy.GetComponent<tk2dBaseSprite>());
            healthHaver.PreventAllDamage = false;
            healthHaver.SetHealthMaximum(15);
            healthHaver.ForceSetCurrentHealth(15f);

            var aiActor = enemy.AddComponent<AIActor>();
            aiActor.MovementSpeed = 2f;
            aiActor.CollisionDamage = 0.5f;
            aiActor.State = AIActor.ActorState.Normal;
            aiActor.CanTargetPlayers = true;
            aiActor.CanTargetEnemies = false;
            aiActor.ForcedPositionInAmmonomicon = 5;
            aiActor.EnemyGuid = guid;
            aiActor.AvoidRadius = 4;
            aiActor.ActorName = "Bouncer Bullet Kin";
            aiActor.EnemySwitchState = "Metal_Bullet_Man";
            aiActor.OverridePitfallAnim = "pitfall";
            aiActor.BlackPhantomProperties = new BlackPhantomProperties();
            aiActor.BlackPhantomProperties.BonusHealthPercentIncrease = 2.2f;

            AIShooter shooter = aiActor.AddAIShooter(
                new Vector2(-0.5f, 0.2f),
                BouncerUzi.ID,
                customSpriteName: "bouncerbulletkin_hand_001",
                spriteCollection: BouncerBulletKinSpriteCollection
                );

            var bs = enemy.GetOrAddComponent<BehaviorSpeculator>();
            bs.PostAwakenDelay = 0.5f;
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
                    BulletObject = EnemyBulletDatabase.Bouncing,
                    AudioEvent = "Play_WPN_uzi_shot_01",
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
                    MagazineCapacity = 15f,
                    ReloadSpeed = 4f,
                    Cooldown = 0.2f,
                    Range = 20f,
                    BulletScript = null,
                    RespectReload = true,
                    StopDuringAttack = false,
                    WeaponType = WeaponType.AIShooterProjectile,
                                InitialCooldown = 1f,
                },
                new ShootGunBehavior
                {
                    GroupCooldownVariance = 0.2f,
                    MagazineCapacity = 1f,
                    ReloadSpeed = 4f,
                    Cooldown = 8f,
                    Range = 20f,
                    BulletScript = new CustomBulletScriptSelector(typeof(BouncerBulletKin_Waves)),
                    RespectReload = false,
                    StopDuringAttack = true,
                    WeaponType = WeaponType.BulletScript,
                                InitialCooldown = 4f,
                }
            };

            DebrisObject corpseObject = Breakables.GenerateDebrisObject(BouncerBulletKinSpriteCollection, "bouncerbullet_diefrontleft_005",
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


            SpriteBuilder.AddToAmmonomicon(BouncerBulletKinSpriteCollection.GetSpriteDefinition("bouncerbullet_idleleft_001"));

            ETGMod.Databases.Strings.Enemies.AddComplex("#BOUNCERBULLETKIN_ENCNAME", "Bouncer Bullet Kin");
            ETGMod.Databases.Strings.Enemies.AddComplex("#BOUNCERBULLETKIN_SHORTDESC", "Gun Club");
            ETGMod.Databases.Strings.Enemies.AddComplex("#BOUNCERBULLETKIN_LONGDESC", "These technically-minded Bullet Kin coat their ammunition in rubber, giving it advanced ricocheting properties.\n\nFeared by Rubber Kin.");

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
                enemyPortraitSprite = Initialisation.assetBundle.LoadAsset<Texture2D>("ammonomicon_bouncerbulletkin"),
                AmmonomiconSprite = "bouncerbullet_idleleft_001",
                PrimaryDisplayName = "#BOUNCERBULLETKIN_ENCNAME",
                NotificationPanelDescription = "#BOUNCERBULLETKIN_SHORTDESC",
                AmmonomiconFullEntry = "#BOUNCERBULLETKIN_LONGDESC",
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
                ForcedPositionInAmmonomicon = 5,
            };

            prefab = enemy;
            EncounterDatabase.Instance.Entries.Add(encounterEntry);
            EnemyDatabase.Instance.Entries.Add(enemyDatabaseEntry);
            EnemyBuilder.Dictionary.Add(guid, prefab);

            GameObject.DontDestroyOnLoad(prefab);
            FakePrefab.MarkAsFakePrefab(prefab);
            prefab.SetActive(false);
            Game.Enemies.Add("nn:bouncer_bullet_kin", aiActor);
        }
    }
    public class BouncerBulletKin_Waves : Script
    {
        public override IEnumerator Top()
        {
            for (int i = 0; i < 3; i++)
            {
                bool vert = UnityEngine.Random.value < 0.5f;

                float angle = 25 * (vert ? -1 : 1);
                angle += UnityEngine.Random.Range(-20, 20);

                for (int bullet = 0; bullet < 5; bullet++)
                {
                    base.Fire(new Direction(angle, DirectionType.Aim, -1f), new Speed(5f, SpeedType.Absolute), null);
                    angle += vert ? 5 : -5;
                    yield return base.Wait(2.5f);
                }
                yield return base.Wait(10);
            }
            yield return base.Wait(10);
            yield break;
        }
    }
}
