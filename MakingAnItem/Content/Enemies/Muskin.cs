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
    public class Muskin
    {
        public static tk2dSpriteCollectionData MuskinSpriteCollection;
        public static tk2dSpriteAnimation MuskinAnimationCollection;

        public static GameObject prefab;
        public static readonly string guid = "omitb_muskin_yee338q7881ooo1837467iuuueee";
        public static void Init()
        {
            MuskinSpriteCollection = AssetBundleLoader.FastLoadSpriteCollection(Initialisation.assetBundle, "ENM_Muskin_Collection", "ENM_Muskin_Collection_Material");
            MuskinAnimationCollection = Initialisation.assetBundle.LoadAsset<GameObject>("ENM_Muskin_Anim").GetComponent<tk2dSpriteAnimation>();


            var enemy = EntityTools.SetupEntityObject("Muskin", "muskin_idleleft_001", MuskinSpriteCollection);

            var enemyRigidBody = EntityTools.SetupEntityRigidBody(enemy,
                new List<CollisionLayer>() { CollisionLayer.EnemyCollider, CollisionLayer.EnemyHitBox }, //Collision Layers
                new List<IntVector2>() { new IntVector2(10, 4), new IntVector2(10, 18) }, //Collider Dimensions
                new List<IntVector2>() { new IntVector2(-5, 1), new IntVector2(-5, 1) } //Collider Offsets
                );

            EntityTools.SetupEntityKnockback(enemy);

            tk2dSpriteAnimator animator = enemy.GetOrAddComponent<tk2dSpriteAnimator>();
            animator.Library = MuskinAnimationCollection;
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
                    name = "roll",
                    anim = new DirectionalAnimation()
                    {
                        Type = DirectionalAnimation.DirectionType.FourWay,
                        AnimNames = new string[]
                         {
                            "roll_backright",
                            "roll_right",
                            "roll_left",
                            "roll_backleft"
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
            aiActor.MovementSpeed = 1.8f;
            aiActor.CollisionDamage = 0.5f;
            aiActor.State = AIActor.ActorState.Normal;
            aiActor.CanTargetPlayers = true;
            aiActor.CanTargetEnemies = false;
            aiActor.ForcedPositionInAmmonomicon = 30;
            aiActor.EnemyGuid = guid;
            aiActor.AvoidRadius = 4;
            aiActor.ActorName = "Muskin";
            aiActor.EnemySwitchState = "Metal_Bullet_Man";
            aiActor.OverridePitfallAnim = "pitfall";
            aiActor.BlackPhantomProperties = new BlackPhantomProperties();
            aiActor.BlackPhantomProperties.BonusHealthPercentIncrease = 2.2f;

            AIShooter shooter = aiActor.AddAIShooter(
                new Vector2(-0.5f, 0.2f),
                9,
                customSpriteName: "muskin_hand_001",
                spriteCollection: MuskinSpriteCollection
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
                    AudioEvent = "Play_WPN_duelingpistol_shot_01",
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
                    MagazineCapacity = 2f,
                    ReloadSpeed = 5f,
                    Cooldown = 1.6f,
                    Range = 20f,
                    BulletScript = null,
                    RespectReload = true,
                    StopDuringAttack = false,
                    WeaponType = WeaponType.AIShooterProjectile,
                },
                new DashBehavior
                {
                    avoidTarget = false,
                    dashAnim = "roll",
                    dashDirection = DashBehavior.DashDirection.KindaTowardTarget,
                    dashDistance = 3,
                    dashTime = 0.65f,
                    doDodgeDustUp = true,
                    hideGun = true,
                    warpDashAnimLength = true,
                    Cooldown = 3f
                }
            };

            aiActor.CorpseObject = null;

            ObjectVisibilityManager vis = enemy.AddComponent<ObjectVisibilityManager>();


            SpriteBuilder.AddToAmmonomicon(MuskinSpriteCollection.GetSpriteDefinition("muskin_idleleft_001"));

            ETGMod.Databases.Strings.Enemies.AddComplex("#MUSKIN_ENCNAME", "Muskin");
            ETGMod.Databases.Strings.Enemies.AddComplex("#MUSKIN_SHORTDESC", "Timeless Style");
            ETGMod.Databases.Strings.Enemies.AddComplex("#MUSKIN_LONGDESC", "Musket weaponry is rare within the Gungeon, often passed over in favour of more modern cartridge ammunition. Nonetheless, the round Muskin prefers bullets that matches it's spherical shape, no matter how outdated.\n\nAs a ball, they have the natural urge to roll.");

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
                enemyPortraitSprite = Initialisation.assetBundle.LoadAsset<Texture2D>("ammonomicon_muskin"),
                AmmonomiconSprite = "muskin_idleleft_001",
                PrimaryDisplayName = "#MUSKIN_ENCNAME",
                NotificationPanelDescription = "#MUSKIN_SHORTDESC",
                AmmonomiconFullEntry = "#MUSKIN_LONGDESC",
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
                ForcedPositionInAmmonomicon = 30,
            };

            prefab = enemy;
            EncounterDatabase.Instance.Entries.Add(encounterEntry);
            EnemyDatabase.Instance.Entries.Add(enemyDatabaseEntry);
            EnemyBuilder.Dictionary.Add(guid, prefab);

            GameObject.DontDestroyOnLoad(prefab);
            FakePrefab.MarkAsFakePrefab(prefab);
            prefab.SetActive(false);
            Game.Enemies.Add("nn:muskin", aiActor);
        }
    }
}
