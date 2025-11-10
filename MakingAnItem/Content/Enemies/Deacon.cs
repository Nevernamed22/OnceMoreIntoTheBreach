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
using UnityEngine.SocialPlatforms;

namespace NevernamedsItems
{
    public class Deacon
    {
        public static tk2dSpriteCollectionData DeaconSpriteCollection;
        public static tk2dSpriteAnimation DeaconAnimationCollection;

        public static GameObject prefab;
        public static readonly string guid = "omitb_deacon_567890983hruiwfh8auiqy387fhwoe719h4rutijouqiy";
        public static void Init()
        {
            DeaconSpriteCollection = AssetBundleLoader.FastLoadSpriteCollection(Initialisation.assetBundle, "ENM_Deacon_Collection", "ENM_Deacon_Collection_Material");
            DeaconAnimationCollection = Initialisation.assetBundle.LoadAsset<GameObject>("ENM_Deacon_Anim").GetComponent<tk2dSpriteAnimation>();


            var deacon = EntityTools.SetupEntityObject("Deacon", "deacon_idle_right_001", DeaconSpriteCollection);

            var deaconBody = EntityTools.SetupEntityRigidBody(deacon,
                new List<CollisionLayer>() { CollisionLayer.EnemyCollider, CollisionLayer.EnemyHitBox }, //Collision Layers
                new List<IntVector2>() { new IntVector2(10, 4), new IntVector2(10, 18) }, //Collider Dimensions
                new List<IntVector2>() { new IntVector2(-5, 1), new IntVector2(-5, 1) } //Collider Offsets
                );

            EntityTools.SetupEntityKnockback(deacon);

            tk2dSpriteAnimator animator = deacon.GetOrAddComponent<tk2dSpriteAnimator>();
            animator.Library = DeaconAnimationCollection;
            AIAnimator AIanimator = deacon.AddComponent<AIAnimator>();
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
                        Type = DirectionalAnimation.DirectionType.EightWay,
                        AnimNames = new string[]
                         {
                            "death_frontleft", // back
                            "death_frontleft", // back_right
                            "death_left", // right
                            "death_backleft", // front_right
                            "death_backright", // front
                            "death_backright", //front_left
                            "death_right", // left
                            "death_frontright" //back_left
                         },
                        Flipped = new DirectionalAnimation.FlipType[8]
                    }
                });


            EntityTools.AddFootstepSounds(AIanimator, new int[] { 1, 4 });

            animator.GetClipByName("specialattack").frames[3].triggerEvent = true;
            animator.GetClipByName("specialattack").frames[3].eventInfo = "fire";
            animator.GetClipByName("specialattack").frames[3].eventAudio = "Play_ENM_holy_cross_01";

            animator.GetClipByName("burst").frames[4].triggerEvent = true;
            animator.GetClipByName("burst").frames[4].eventAudio = "Play_ENM_Death";
            animator.GetClipByName("burst").frames[5].triggerEvent = true;
            animator.GetClipByName("burst").frames[5].eventInfo = "fire";
            animator.GetClipByName("burst").frames[5].eventAudio = "Play_ENM_shotgun_burst_01";

            var healthHaver = deacon.AddComponent<HealthHaver>();
            healthHaver.RegisterBodySprite(deacon.GetComponent<tk2dBaseSprite>());
            healthHaver.PreventAllDamage = false;
            healthHaver.SetHealthMaximum(45);
            healthHaver.ForceSetCurrentHealth(45f);

            healthHaver.spawnBulletScript = true;
            healthHaver.chanceToSpawnBulletScript = 0.3f;
            healthHaver.noCorpseWhenBulletScriptDeath = true;
            healthHaver.overrideDeathAnimBulletScript = "burst";
            healthHaver.bulletScriptType = HealthHaver.BulletScriptType.OnAnimEvent;
            healthHaver.bulletScript = new CustomBulletScriptSelector(typeof(OMITBDeaconDeathBurst));


            var aiActor = deacon.AddComponent<AIActor>();
            aiActor.MovementSpeed = 2f;
            aiActor.CollisionDamage = 0.5f;
            aiActor.State = AIActor.ActorState.Normal;

            aiActor.CanTargetPlayers = true;
            aiActor.CanTargetEnemies = false;
            aiActor.ForcedPositionInAmmonomicon = 16;
            aiActor.EnemyGuid = guid;
            aiActor.AvoidRadius = 4;
            aiActor.ActorName = "Deacon";
            aiActor.EnemySwitchState = "Plastic_Bullet_Man";
            aiActor.OverridePitfallAnim = "pitfall";
            aiActor.BlackPhantomProperties = new BlackPhantomProperties();
            aiActor.BlackPhantomProperties.BonusHealthPercentIncrease = 2.2f;
            aiActor.State = AIActor.ActorState.Inactive;

            AIShooter shooter = aiActor.AddAIShooter(
                new Vector2(-0.5f, 0.2f),
                93,
                handHasSprite: false
                );

            var bs = deacon.GetOrAddComponent<BehaviorSpeculator>();

            bs.AttackBehaviors = new List<AttackBehaviorBase>();
            bs.OverrideBehaviors = new List<OverrideBehaviorBase>();
            bs.OtherBehaviors = new List<BehaviorBase>();
            bs.PostAwakenDelay = 0.5f;
            AIBulletBank bank = deacon.GetOrAddComponent<AIBulletBank>();
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
                    Name = "distortion",
                    BulletObject = EnemyBulletDatabase.BouncyDisruption,
                    AudioEvent = "Play_ENM_holy_cross_01",
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
                    MuzzleFlashEffects = null,
                },
                new AIBulletBank.Entry()
                {
                    Name = "subbullet",
                    BulletObject = EnemyBulletDatabase.Regular_EightByEight,
                    AudioEvent = "",
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
                    MuzzleFlashEffects = null,

                },
                new AIBulletBank.Entry()
                {
                    Name = "flashy",
                    BulletObject = EnemyBulletDatabase.Flashing_EightByEight,
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
                    MuzzleFlashEffects = null,

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

            GameObject turretShootPoint = new GameObject("turretShootPoint");
            turretShootPoint.transform.SetParent(deacon.transform);
            turretShootPoint.transform.localPosition = new Vector3(0f, 1.75f, 0f);

            bs.AttackBehaviors = new List<AttackBehaviorBase>
            {
                new AttackBehaviorGroup()
                {

                    AttackBehaviors = new List<AttackBehaviorGroup.AttackGroupItem>()
                    {

                        new AttackBehaviorGroup.AttackGroupItem()
                        {
                            NickName = "PlaceTurret",
                            Probability = 0.5f,
                            Behavior = new ShootBehavior()
                            {
                                TellAnimation = "specialattack",
                                RequiresTarget = true,
                                Cooldown = 10f,
                                InitialCooldown = 5f,
                                BulletScript = new CustomBulletScriptSelector(typeof(OMITBDeaconTurretAttack)),
                                HideGun = true,
                                ImmobileDuringStop = true,
                                ShootPoint = turretShootPoint,
                                StopDuring = ShootBehavior.StopType.Tell,
                                MoveSpeedModifier = 0,

                            }
                        },
                        new AttackBehaviorGroup.AttackGroupItem()
                        {
                            NickName = "Basic Shooting",
                            Probability = 1,
                            Behavior = new ShootGunBehavior
                            {
                                GroupCooldownVariance = 0.2f,
                                MagazineCapacity = 1f,
                                ReloadSpeed = 3f,
                                Cooldown = 3.5f,
                                LeadAmount = 0,
                                LeadChance = 1,
                                Range = 40f,
                                BulletScript = new CustomBulletScriptSelector(typeof(OMITBDeaconBasicAttack)),
                                RespectReload = true,
                                StopDuringAttack = true,
                                WeaponType = WeaponType.BulletScript,
                                InitialCooldown = 2f,
                            }
                        }
                    }
                }
            };




            DebrisObject corpseObject = Breakables.GenerateDebrisObject(DeaconSpriteCollection, "deacon_die_backright_005",
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

            ObjectVisibilityManager vis = deacon.AddComponent<ObjectVisibilityManager>();


            SpriteBuilder.AddToAmmonomicon(DeaconSpriteCollection.GetSpriteDefinition("deacon_idle_right_001"));

            ETGMod.Databases.Strings.Enemies.AddComplex("#DEACON_ENCNAME", "Deacon");
            ETGMod.Databases.Strings.Enemies.AddComplex("#DEACON_SHORTDESC", "Ordinal");
            ETGMod.Databases.Strings.Enemies.AddComplex("#DEACON_LONGDESC", "These ranking members of the Order of the True Gun are renowned for their Kaliber-given ammomantic powers.\n\nBeware the embrace of their eucharistic buckshot.");

            EncounterTrackable tracker = deacon.gameObject.GetOrAddComponent<EncounterTrackable>();
            tracker.DoNotificationOnEncounter = true;
            tracker.EncounterGuid = guid;
            tracker.IgnoreDifferentiator = false;
            tracker.TrueEncounterGuid = guid;
            tracker.tag = "Enemy";
            tracker.prerequisites = new DungeonPrerequisite[0];
            tracker.ProxyEncounterGuid = string.Empty;
            tracker.journalData = new JournalEntry()
            {
                enemyPortraitSprite = Initialisation.assetBundle.LoadAsset<Texture2D>("ammonomicon_deacon"),
                AmmonomiconSprite = "deacon_idle_right_001",
                PrimaryDisplayName = "#DEACON_ENCNAME",
                NotificationPanelDescription = "#DEACON_SHORTDESC",
                AmmonomiconFullEntry = "#DEACON_LONGDESC",
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
                ForcedPositionInAmmonomicon = 17,
            };

            prefab = deacon;
            EncounterDatabase.Instance.Entries.Add(encounterEntry);
            EnemyDatabase.Instance.Entries.Add(enemyDatabaseEntry);
            EnemyBuilder.Dictionary.Add(guid, prefab);

            GameObject.DontDestroyOnLoad(prefab);
            FakePrefab.MarkAsFakePrefab(prefab);
            prefab.SetActive(false);
            Game.Enemies.Add("nn:deacon", aiActor);
        }
    }
    public class OMITBDeaconTurretAttack : Script
    {
        public override IEnumerator Top()
        {
            base.Fire(new Direction(0f, DirectionType.Aim, -1f), new Speed(5f, SpeedType.Absolute), new OMITBDeaconTurretAttack.TurretBullet());
            yield return base.Wait(40);
            yield break;
        }
        public class TurretBullet : Bullet
        {
            public TurretBullet() : base("distortion", false, false, false)
            {
            }

            float elapsed;
            public override IEnumerator Top()
            {
                base.ChangeSpeed(new Speed(0.01f, SpeedType.Absolute), 100);
                for (int i = 0; i < 5; i++)
                {
                    yield return base.Wait(100);
                    this.Projectile.spriteAnimator.PlayFromFrame("killithid_disruption_attack", 0);
                    yield return base.Wait(15);
                    base.Fire(new Direction(base.AimDirection - 25, DirectionType.Absolute, -1f), new Speed(5f, SpeedType.Absolute), new Bullet("subbullet", true, false, false));
                    base.Fire(new Direction(base.AimDirection, DirectionType.Absolute, -1f), new Speed(5f, SpeedType.Absolute), new Bullet("subbullet", true, false, false));
                    base.Fire(new Direction(base.AimDirection + 25, DirectionType.Absolute, -1f), new Speed(5f, SpeedType.Absolute), new Bullet("subbullet", true, false, false));
                }
                base.Vanish(false);
                yield break;
            }

        }
    }
    public class OMITBDeaconBasicAttack : Script
    {
        public static float baseSpeed = 5f;
        public static float bonusSpeedMax = 5f;
        public static float bonusSpeedMin = 0f;
        public static List<Vector2> vectors = new List<Vector2>()
        {
            new Vector2(3.7f, -3.4f),
            new Vector2(3.1f, -3.3f),
            new Vector2(2.5f, -3.1f),
            new Vector2(1.9f, -2.8f),
            new Vector2(1.4f, -2.3f),
            new Vector2(0.9f, -1.8f),
            new Vector2(0.6f, -1.2f),
            new Vector2(0.3f, -0.6f),

            new Vector2(0.0f, 0.0f),

            new Vector2(0.3f, 0.6f),
            new Vector2(0.6f, 1.2f),
            new Vector2(0.9f, 1.8f),
            new Vector2(1.4f, 2.3f),
            new Vector2(1.9f, 2.8f),
            new Vector2(2.5f, 3.1f),
            new Vector2(3.1f, 3.3f),
            new Vector2(3.7f, 3.4f),
        };
        public override IEnumerator Top()
        {
            float i = 0;
            foreach (Vector2 ve in vectors)
            {

                base.Fire(
                    new Direction(0, DirectionType.Aim, -1f),
                    new Speed(4f, SpeedType.Absolute),
                    new OMITB_ControlledBullet(ve, 0, 120)
                    );
                i++;
            }
            yield break;
        }
    }
    public class OMITBDeaconDeathBurst : Script
    {
        public int NumBullets = 32;

        public override IEnumerator Top()
        {
            float startingDir = UnityEngine.Random.Range(0f, 360f);
            for (int i = 0; i < this.NumBullets; i++)
            {
                base.Fire(new Direction(startingDir + (float)i * 360f / (float)this.NumBullets, DirectionType.Absolute, -1f), new Speed(6.5f, SpeedType.Absolute), new Bullet("flashy", false, false, false));
            }
            for (int j = 0; j < 4; j++)
            {
                QuadShot(90f * (float)j, 11f);
            }
            yield break;
        }
        private void QuadShot(float direction, float speed)
        {
            for (int i = 0; i < 6; i++)
            {
                base.Fire(new Direction(direction, DirectionType.Absolute, -1f), new Speed(speed - (float)i * 1.5f, SpeedType.Absolute), new SpeedChangingBullet(speed, 120, -1));
            }
        }
    }
}
