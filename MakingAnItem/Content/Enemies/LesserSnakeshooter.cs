using Alexandria.BreakableAPI;
using Alexandria.EnemyAPI;
using Alexandria.ItemAPI;
using Alexandria.CharacterAPI;
using Alexandria.SoundAPI;
using Brave.BulletScript;
using Gungeon;
using Microsoft.Cci;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

namespace NevernamedsItems
{
    public class LesserSnakeshooter
    {
        public static tk2dSpriteCollectionData LesserSnakeshooterSpriteCollection;
        public static tk2dSpriteAnimation LesserSnakeshooterAnimationCollection;

        public static GameObject prefab;
        public static readonly string guid = "omitb_lessersnakeshooter";
        public static void Init()
        {
            LesserSnakeshooterSpriteCollection = AssetBundleLoader.FastLoadSpriteCollection(Initialisation.assetBundle, "ENM_LesserSnakeshooter_Collection", "ENM_LesserSnakeshooter_Collection_Material");
            LesserSnakeshooterAnimationCollection = Initialisation.assetBundle.LoadAsset<GameObject>("ENM_LesserSnakeshooter_Anim").GetComponent<tk2dSpriteAnimation>();


            var enemy = EntityTools.SetupEntityObject("Lesser Snakeshooter", "snakeshooter_idleright_001", LesserSnakeshooterSpriteCollection);

            var enemyRigidBody = EntityTools.SetupEntityRigidBody(enemy,
                new List<CollisionLayer>() { CollisionLayer.EnemyCollider, CollisionLayer.EnemyHitBox }, //Collision Layers
                new List<IntVector2>() { new IntVector2(10, 4), new IntVector2(10, 19) }, //Collider Dimensions
                new List<IntVector2>() { new IntVector2(-5, 1), new IntVector2(-5, 1) } //Collider Offsets
                );

            EntityTools.SetupEntityKnockback(enemy, weight: 70);

            tk2dSpriteAnimator animator = enemy.GetOrAddComponent<tk2dSpriteAnimator>();
            animator.Library = LesserSnakeshooterAnimationCollection;
            AIAnimator AIanimator = enemy.AddComponent<AIAnimator>();
            AIanimator.IdleAnimation = new DirectionalAnimation()
            {
                Type = DirectionalAnimation.DirectionType.EightWay,
                AnimNames = new string[]
                {
                    "idle_back", //Back
                    "idle_right", //Back Right
                    "idle_right", //Right
                    "idle_frontright", //Front Right
                    "idle_front", //Front
                    "idle_frontleft", //Front Left
                    "idle_left", //Left
                    "idle_left" //Back Left
                },
                Flipped = new DirectionalAnimation.FlipType[8]
            };
            AIanimator.MoveAnimation = new DirectionalAnimation()
            {
                Type = DirectionalAnimation.DirectionType.EightWay,
                AnimNames = new string[]
                {
                    "move_back", //Back
                    "move_right", //Back Right
                    "move_right", //Right
                    "move_frontright", //Front Right
                    "move_front", //Front
                    "move_frontleft", //Front Left
                    "move_left", //Left
                    "move_left" //Back Left
                },
                Flipped = new DirectionalAnimation.FlipType[8]
            };
            AIanimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>();
            AIanimator.OtherAnimations.Add(
                new AIAnimator.NamedDirectionalAnimation()
                {
                    name = "attack",
                    anim = new DirectionalAnimation()
                    {
                        Type = DirectionalAnimation.DirectionType.FourWayCardinal,
                        AnimNames = new string[]
                        {
                            "attack_back", //North
                            "attack_right", //East
                            "attack_front", //South
                            "attack_left", //West
                        },
                        Flipped = new DirectionalAnimation.FlipType[4]
                    }
                });

            //EntityTools.AddFootstepSounds(AIanimator, new int[] { 1, 4 });
            foreach(AIAnimator.NamedDirectionalAnimation namedDir in AIanimator.OtherAnimations)
            {
                if (namedDir.name == "attack")
                {
                    foreach(string name in namedDir.anim.AnimNames)
                    {
                        animator.GetClipByName(name).frames[3].triggerEvent = true;
                        animator.GetClipByName(name).frames[3].eventInfo = "fire";
                        animator.GetClipByName(name).frames[3].eventAudio = "Play_VO_bashellisk_hiss_01";
                    }
                }
            }

            tk2dSpriteAnimationFrame outLineDis = animator.spriteAnimator.GetClipByName("death").frames[2];
            outLineDis.triggerEvent = true;
            outLineDis.eventInfo = "DisableOutlines";
            outLineDis.eventOutline = tk2dSpriteAnimationFrame.OutlineModifier.TurnOff;

            var healthHaver = enemy.AddComponent<HealthHaver>();
            healthHaver.RegisterBodySprite(enemy.GetComponent<tk2dBaseSprite>());
            healthHaver.PreventAllDamage = false;
            healthHaver.SetHealthMaximum(27f);
            healthHaver.ForceSetCurrentHealth(27f);

            var aiActor = enemy.AddComponent<AIActor>();
            aiActor.MovementSpeed = 2f;
            aiActor.CollisionDamage = 0.5f;
            aiActor.State = AIActor.ActorState.Normal;
            aiActor.CanTargetPlayers = true;
            aiActor.CanTargetEnemies = false;
            aiActor.ForcedPositionInAmmonomicon = 75;
            aiActor.EnemyGuid = guid;
            aiActor.AvoidRadius = 4;
            aiActor.ActorName = "Lesser Snakeshooter";

            aiActor.EnemySwitchState = "Bashellisk";
            //aiActor.OverridePitfallAnim = "pitfall";
            
            aiActor.BlackPhantomProperties = new BlackPhantomProperties();
            aiActor.BlackPhantomProperties.BonusHealthPercentIncrease = 2.2f;

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
                    BulletObject = EnemyBulletDatabase.Flashing_EightByEight,
                    AudioEvent = "Play_ENM_snake_shot_01",
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
                },
                new AIBulletBank.Entry()
                {
                    Name = "snakeBullet",
                    BulletObject = EnemyBulletDatabase.Cue_Ball,
                    AudioEvent = "Play_ENM_snake_shot_01",
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

            GameObject turretShootPoint = new GameObject("shootPoint");
            turretShootPoint.transform.SetParent(enemy.transform);
            turretShootPoint.transform.localPosition = new Vector3(0f, 1.125f, 0f);

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
                new ShootBehavior()
                {
                    TellAnimation = "attack",
                    RequiresTarget = true,
                    Cooldown = 3f,
                    InitialCooldown = 1f,
                    BulletScript = new CustomBulletScriptSelector(typeof(BashelliskSnakeBullets1)),
                    HideGun = true,
                    ImmobileDuringStop = true,
                    ShootPoint = turretShootPoint,
                    StopDuring = ShootBehavior.StopType.Tell,
                    MoveSpeedModifier = 0,
                }
            };

            HitEffectHandler hitEffects = enemy.GetOrAddComponent<HitEffectHandler>();
            HitEffectHandler toCopy = EnemyDatabase.GetOrLoadByGuid(GUIDs.Ammoconda_Ball).GetComponent<HitEffectHandler>();
            hitEffects.additionalHitEffects = toCopy.additionalHitEffects;

            ObjectVisibilityManager vis = enemy.AddComponent<ObjectVisibilityManager>();


            SpriteBuilder.AddToAmmonomicon(LesserSnakeshooterSpriteCollection.GetSpriteDefinition("snakeshooter_idleright_001"));

            ETGMod.Databases.Strings.Enemies.AddComplex("#LESSERSNAKESHOOTER_ENCNAME", "Lesser Snakeshooter");
            ETGMod.Databases.Strings.Enemies.AddComplex("#LESSERSNAKESHOOTER_SHORTDESC", "Sheds Shells");
            ETGMod.Databases.Strings.Enemies.AddComplex("#LESSERSNAKESHOOTER_LONGDESC", "The spawn of the great Ammoconda, each Snakeshooter will shed its shells thousands of times before maturing.\n\nVery few survive to adulthood.");

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
                enemyPortraitSprite = Initialisation.assetBundle.LoadAsset<Texture2D>("ammonomicon_lessersnakeshooter"),
                AmmonomiconSprite = "snakeshooter_idleright_001",
                PrimaryDisplayName = "#LESSERSNAKESHOOTER_ENCNAME",
                NotificationPanelDescription = "#LESSERSNAKESHOOTER_SHORTDESC",
                AmmonomiconFullEntry = "#LESSERSNAKESHOOTER_LONGDESC",
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
                ForcedPositionInAmmonomicon = 75,
            };

            prefab = enemy;
            EncounterDatabase.Instance.Entries.Add(encounterEntry);
            EnemyDatabase.Instance.Entries.Add(enemyDatabaseEntry);
            EnemyBuilder.Dictionary.Add(guid, prefab);

            GameObject.DontDestroyOnLoad(prefab);
            FakePrefab.MarkAsFakePrefab(prefab);
            prefab.SetActive(false);
            Game.Enemies.Add("nn:lesser_snakeshooter", aiActor);
        }
    }
    
}
