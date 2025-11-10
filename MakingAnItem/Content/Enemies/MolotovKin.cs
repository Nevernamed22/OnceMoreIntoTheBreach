using Alexandria.BreakableAPI;
using Alexandria.EnemyAPI;
using Alexandria.ItemAPI;
using Alexandria.Misc;
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
    public class MolotovKin
    {
        public static tk2dSpriteCollectionData MolotovKinSpriteCollection;
        public static tk2dSpriteAnimation MolotovKinAnimationCollection;

        public static GameObject prefab;
        public static readonly string guid = "omitb_molotovkin_ppppppkdhfyuuietyeuigyeuioweiywuywu";
        public static void Init()
        {
            MolotovKinSpriteCollection = AssetBundleLoader.FastLoadSpriteCollection(Initialisation.assetBundle, "ENM_MolotovKin_Collection", "ENM_MolotovKin_Collection_Material");
            MolotovKinAnimationCollection = Initialisation.assetBundle.LoadAsset<GameObject>("ENM_MolotovKin_Anim").GetComponent<tk2dSpriteAnimation>();


            var enemy = EntityTools.SetupEntityObject("Molotovnik", "molotovkin_idleright_001", MolotovKinSpriteCollection);

            var enemyRigidBody = EntityTools.SetupEntityRigidBody(enemy,
                new List<CollisionLayer>() { CollisionLayer.EnemyCollider, CollisionLayer.EnemyHitBox }, //Collision Layers
                new List<IntVector2>() { new IntVector2(10, 4), new IntVector2(10, 18) }, //Collider Dimensions
                new List<IntVector2>() { new IntVector2(-5, 1), new IntVector2(-5, 1) } //Collider Offsets
                );

            EntityTools.SetupEntityKnockback(enemy);

            tk2dSpriteAnimator animator = enemy.GetOrAddComponent<tk2dSpriteAnimator>();
            animator.Library = MolotovKinAnimationCollection;
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

            EntityTools.AddFootstepSounds(AIanimator, new int[] { 1, 4 });

            GoopDoer gooper = enemy.AddComponent<GoopDoer>();
            gooper.goopDefinition = GoopUtility.FireDef;
            gooper.positionSource = GoopDoer.PositionSource.GroundCenter;
            gooper.isTimed = true;
            gooper.goopTime = 0.5f;
            gooper.updateTiming = GoopDoer.UpdateTiming.TriggerOnly;
            gooper.goopSizeVaries = true;
            gooper.goopSizeRandom = true;
            gooper.radiusMax = 3f;
            gooper.radiusMin = 2f;
            gooper.updateOnAnimFrames = true;

            tk2dSpriteAnimationFrame goopFrame = animator.spriteAnimator.GetClipByName("death").frames[4];
            goopFrame.triggerEvent = true;
            goopFrame.eventInfo = "goop";
            goopFrame.eventAudio = "Play_WPN_molotov_impact_01";

            var healthHaver = enemy.AddComponent<HealthHaver>();
            healthHaver.RegisterBodySprite(enemy.GetComponent<tk2dBaseSprite>());
            healthHaver.PreventAllDamage = false;
            healthHaver.SetHealthMaximum(25);
            healthHaver.ForceSetCurrentHealth(25f);
            //Debug.Log("-2");

            var aiActor = enemy.AddComponent<AIActor>();
            aiActor.MovementSpeed = 2f;
            aiActor.CollisionDamage = 0.5f;
            aiActor.State = AIActor.ActorState.Normal;
            aiActor.CanTargetPlayers = true;
            aiActor.CanTargetEnemies = false;
            aiActor.ForcedPositionInAmmonomicon = 34;
            aiActor.EnemyGuid = guid;
            aiActor.AvoidRadius = 4;
            aiActor.ActorName = "Molotovnik";
            aiActor.EnemySwitchState = "Metal_Bullet_Man";
            aiActor.OverridePitfallAnim = "pitfall";
            aiActor.BlackPhantomProperties = new BlackPhantomProperties();
            aiActor.BlackPhantomProperties.BonusHealthPercentIncrease = 2.2f;
            //Debug.Log("-1");
            aiActor.EffectResistances = new ActorEffectResistance[]
            {
                new ActorEffectResistance()
                {
                    resistAmount = 0.5f,
                    resistType = EffectResistanceType.Fire,
                }
            };
            //Debug.Log("-0.5");

            AIShooter shooter = aiActor.AddAIShooter(
                new Vector2(-0.5f, 0.2f),
                292,
                customSpriteName: "molotovkin_hand_001",
                spriteCollection: MolotovKinSpriteCollection
                );
            //Debug.Log("0");

            var bs = enemy.GetOrAddComponent<BehaviorSpeculator>();
            bs.PostAwakenDelay = 0.5f;
            bs.AttackBehaviors = new List<AttackBehaviorBase>();
            bs.OverrideBehaviors = new List<OverrideBehaviorBase>();
            bs.OtherBehaviors = new List<BehaviorBase>();

            projectileMolotov = EnemyBulletDatabase.Molotov.InstantiateAndFakeprefab();
            projectileMolotov.GetComponent<Projectile>().BulletScriptSettings = new BulletScriptSettings()
            {
                overrideMotion = true,
            };
            //Debug.Log("1");
            AIBulletBank bank = enemy.GetOrAddComponent<AIBulletBank>();
            bank.CollidesWithEnemies = false;
            bank.PlayAudio = true;
            bank.PlayShells = true;
            bank.PlayVfx = true;
            bank.rampBullets = true;
            bank.rampStartHeight = 15;
            bank.rampTime = 1;
            bank.Bullets = new List<AIBulletBank.Entry>()
            {
                new AIBulletBank.Entry()
                {
                    Name = "default",
                    BulletObject = EnemyBulletDatabase.Regular_EightByEight,
                    AudioEvent = "Play_WPN_sniperrifle_shot_01",
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
                    Name = "molotov",
                    BulletObject = projectileMolotov,
                    AudioEvent = "Play_WPN_sniperrifle_shot_01",
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
                        range = 100,
                        speed = 9,
                        UsesCustomAccelerationCurve = false,
                        onDestroyBulletScript = new BulletScriptSelector(){ scriptTypeName = "" }
                    },
                    rampBullets = true,
                    rampStartHeight = 15,
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
            //Debug.Log("2");


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
                    CustomRange = 12,
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
                    ReloadSpeed = 4f,
                    Cooldown = 8f,
                    Range = 20f,
                    BulletScript =  new CustomBulletScriptSelector(typeof(MolotovKin_BasicAttack)),
                    RespectReload = true,
                    StopDuringAttack = false,
                    WeaponType = WeaponType.BulletScript,
                    RequiresLineOfSight = true,
                    OverrideBulletName = "molotov",
                                InitialCooldown = 2f,
                }
            };

            //Debug.Log("3");

            aiActor.CorpseObject = null;

            ObjectVisibilityManager vis = enemy.AddComponent<ObjectVisibilityManager>();

            SpriteBuilder.AddToAmmonomicon(MolotovKinSpriteCollection.GetSpriteDefinition("molotovkin_idleright_001"));

            ETGMod.Databases.Strings.Enemies.AddComplex("#MOLOTOVNIK_ENCNAME", "Molotovnik");
            ETGMod.Databases.Strings.Enemies.AddComplex("#MOLOTOVNIK_SHORTDESC", "Drinks With The Meal");
            ETGMod.Databases.Strings.Enemies.AddComplex("#MOLOTOVNIK_LONGDESC", "These glass gundead train in the deepest regions of the forge for many years to master the art of bending fire. When that inevitably fails, the molotov launcher is the next step.");

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
                enemyPortraitSprite = Initialisation.assetBundle.LoadAsset<Texture2D>("ammonomicon_molotovkin"),
                AmmonomiconSprite = "molotovkin_idleright_001",
                PrimaryDisplayName = "#MOLOTOVNIK_ENCNAME",
                NotificationPanelDescription = "#MOLOTOVNIK_SHORTDESC",
                AmmonomiconFullEntry = "#MOLOTOVNIK_LONGDESC",
                SpecialIdentifier = JournalEntry.CustomJournalEntryType.NONE,
                SuppressKnownState = false,
                SuppressInAmmonomicon = false,
                IsEnemy = true,
                DisplayOnLoadingScreen = false,
                RequiresLightBackgroundInLoadingScreen = false
            };
            //Debug.Log("4");

            ShardCluster shardClusterBig = BreakableAPIToolbox.GenerateShardCluster(Breakables.GenerateDebrisObjects(MolotovKinSpriteCollection,
                new List<string>() { "molotovkin_shard_big_001", "molotovkin_shard_big_002", "molotovkin_shard_big_003" }.ToArray(),
                true, //CanRotate
                1, 1, //Lifespan Variables
                1080, //Angular Velocity
                0, //Velocity Variance
                null, //Shadow
                0.5f, //Mass
                null, //Audio Event
                null, //Bounce VFX
                1 //Bounces
                ), 0.1f, 0.5f, 1, 2, 0.1f);
            ShardCluster shardClusterMedium = BreakableAPIToolbox.GenerateShardCluster(Breakables.GenerateDebrisObjects(MolotovKinSpriteCollection,
                new List<string>() { "molotovkin_shard_med_001", "molotovkin_shard_med_002", "molotovkin_shard_med_003" }.ToArray(),
                true, //CanRotate
                1, 1, //Lifespan Variables
                1080, //Angular Velocity
                0, //Velocity Variance
                null, //Shadow
                1f, //Mass
                null, //Audio Event
                null, //Bounce VFX
                1 //Bounces
                ), 1f, 1f, 2, 3, 1f);
            ShardCluster shardClusterSmall = BreakableAPIToolbox.GenerateShardCluster(Breakables.GenerateDebrisObjects(MolotovKinSpriteCollection,
                new List<string>() { "molotovkin_shard_small_001", "molotovkin_shard_small_002", "molotovkin_shard_small_003" }.ToArray(),
                true, //CanRotate
                1, 1, //Lifespan Variables
                1080, //Angular Velocity
                0, //Velocity Variance
                null, //Shadow
                1f, //Mass
                null, //Audio Event
                null, //Bounce VFX
                1 //Bounces
                ), 2f, 2f, 2, 4, 1f);

            SpawnDebrisOnDeath deathDebris = enemy.AddComponent<SpawnDebrisOnDeath>();
            deathDebris.shardClusters = new List<ShardCluster>() { shardClusterBig, shardClusterMedium, shardClusterSmall }.ToArray();
            deathDebris.shardSpawnType = SpawnDebrisOnDeath.ShardSpawnType.BURST;
            deathDebris.shardVerticalSpeed = 2;
            deathDebris.triggerType = SpawnDebrisOnDeath.TriggerType.ANIM_EVENT;
            deathDebris.triggerEventName = "goop";
            deathDebris.doShards = true;
            deathDebris.debrisForce = 2f;
            deathDebris.positionToSpawn = SpawnDebrisOnDeath.SpawnPosition.SPRITE_CENTER;
            //Debug.Log("5");

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
                ForcedPositionInAmmonomicon = 34,
            };
            //Debug.Log("6");

            prefab = enemy;
            EncounterDatabase.Instance.Entries.Add(encounterEntry);
            EnemyDatabase.Instance.Entries.Add(enemyDatabaseEntry);
            EnemyBuilder.Dictionary.Add(guid, prefab);

            GameObject.DontDestroyOnLoad(prefab);
            FakePrefab.MarkAsFakePrefab(prefab);
            prefab.SetActive(false);
            Game.Enemies.Add("nn:molotovnik", aiActor);
        }
        public static GameObject projectileMolotov;
        public class MolotovKin_BasicAttack : Script
        {
            private float? m_playerDist;
            public override IEnumerator Top()
            {
                float aimDirection = base.GetAimDirection(0, 5f);
                aimDirection += UnityEngine.Random.Range(-10f, 10f);

                Bullet bullet = new Bullet("molotov", false, false, false);
                base.Fire(new Direction(aimDirection, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), bullet);
                ArcProjectile arcProjectile = bullet.Projectile as ArcProjectile;
                float? playerDist = this.m_playerDist;
                if (playerDist == null)
                {
                    float timeInFlight = arcProjectile.GetTimeInFlight();
                    Vector2 vector = this.BulletManager.PlayerPosition() + this.BulletManager.PlayerVelocity() * timeInFlight;
                    this.m_playerDist = new float?(Vector2.Distance(base.Position, vector));
                }
                arcProjectile.AdjustSpeedToHit(base.Position + BraveMathCollege.DegreesToVector(aimDirection, this.m_playerDist.Value));

                yield break;
            }
        }

    }
}
