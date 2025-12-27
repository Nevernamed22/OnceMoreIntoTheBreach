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

namespace NevernamedsItems
{
    public class GreaterSnakeshooter
    {
        public static tk2dSpriteCollectionData GreaterSnakeshooterSpriteCollection;
        public static tk2dSpriteAnimation GreaterSnakeshooterAnimationCollection;

        public static GameObject prefab;
        public static readonly string guid = "omitb_greatersnakeshooter";
        public static void Init()
        {
            GreaterSnakeshooterSpriteCollection = AssetBundleLoader.FastLoadSpriteCollection(Initialisation.assetBundle, "ENM_GreaterSnakeshooter_Collection", "ENM_GreaterSnakeshooter_Collection_Material");
            GreaterSnakeshooterAnimationCollection = Initialisation.assetBundle.LoadAsset<GameObject>("ENM_GreaterSnakeshooter_Anim").GetComponent<tk2dSpriteAnimation>();


            var enemy = EntityTools.SetupEntityObject("Greater Snakeshooter", "greatersnakeshooter_idleright_001", GreaterSnakeshooterSpriteCollection);

            var enemyRigidBody = EntityTools.SetupEntityRigidBody(enemy,
                new List<CollisionLayer>() { CollisionLayer.EnemyCollider, CollisionLayer.EnemyHitBox }, //Collision Layers
                new List<IntVector2>() { new IntVector2(10, 4), new IntVector2(10, 19) }, //Collider Dimensions
                new List<IntVector2>() { new IntVector2(-5, 1), new IntVector2(-5, 1) } //Collider Offsets
                );

            EntityTools.SetupEntityKnockback(enemy, weight: 70);

            tk2dSpriteAnimator animator = enemy.GetOrAddComponent<tk2dSpriteAnimator>();
            animator.Library = GreaterSnakeshooterAnimationCollection;
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
            foreach (AIAnimator.NamedDirectionalAnimation namedDir in AIanimator.OtherAnimations)
            {
                if (namedDir.name == "attack")
                {
                    foreach (string name in namedDir.anim.AnimNames)
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
            healthHaver.SetHealthMaximum(35f);
            healthHaver.ForceSetCurrentHealth(35f);

            var aiActor = enemy.AddComponent<AIActor>();
            aiActor.MovementSpeed = 2f;
            aiActor.CollisionDamage = 0.5f;
            aiActor.State = AIActor.ActorState.Normal;
            aiActor.CanTargetPlayers = true;
            aiActor.CanTargetEnemies = false;
            aiActor.ForcedPositionInAmmonomicon = 75;
            aiActor.EnemyGuid = guid;
            aiActor.AvoidRadius = 4;
            aiActor.ActorName = "Greater Snakeshooter";

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
                    AudioEvent = "Play_WPN_uzi_shot_01",
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
                    Name = "nibblesBullet",
                    BulletObject = EnemyDatabase.GetOrLoadByGuid(GUIDs.Ammoconda).bulletBank.GetBullet("nibblesBullet").BulletObject,
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
                new SequentialAttackBehaviorGroup()
                {
                    RunInClass = true,
                    OverrideCooldowns = new List<float>()
                    {
                        1f,1f,4f,
                    },
                    AttackBehaviors = new List<AttackBehaviorBase>()
                    {
                        new ShootBehavior()
                        {
                            TellAnimation = "attack",
                            RequiresTarget = true,
                            Cooldown = 0.5f,
                            InitialCooldown = 1f,
                            BulletScript = new CustomBulletScriptSelector(typeof(GreaterSnakeshooterRipoffAttack)),
                            HideGun = true,
                            ImmobileDuringStop = true,
                            ShootPoint = turretShootPoint,
                            StopDuring = ShootBehavior.StopType.Tell,
                            MoveSpeedModifier = 0,
                        },
                        new ShootBehavior()
                        {
                            TellAnimation = "attack",
                            RequiresTarget = true,
                            Cooldown = 0.5f,
                            InitialCooldown = 1f,
                            BulletScript = new CustomBulletScriptSelector(typeof(BashelliskSnakeBullets1)),
                            HideGun = true,
                            ImmobileDuringStop = true,
                            ShootPoint = turretShootPoint,
                            StopDuring = ShootBehavior.StopType.Tell,
                            MoveSpeedModifier = 0,
                        },
                        new ShootBehavior()
                        {
                            TellAnimation = "attack",
                            RequiresTarget = true,
                            Cooldown = 4f,
                            InitialCooldown = 1f,
                            BulletScript = new CustomBulletScriptSelector(typeof(GreaterSnakeshooterRipoffAttack)),
                            HideGun = true,
                            ImmobileDuringStop = true,
                            ShootPoint = turretShootPoint,
                            StopDuring = ShootBehavior.StopType.Tell,
                            MoveSpeedModifier = 0,
                        }
                    }
                }
            };

            HitEffectHandler hitEffects = enemy.GetOrAddComponent<HitEffectHandler>();

            DebrisObject casing = Breakables.GenerateDebrisObject(GreaterSnakeshooterSpriteCollection,
                    "greatersnakeshooter_shellcasing",
                    true,
                    1,
                    1,
                    900,
                    180,
                    null,
                    1,
                    "Play_WPN_magnum_shells_01",
                    null,
                    1
                    );

            GameObject hitVFX1 = VFXToolbox.CreateVFXBundle("greatersnakeshooter_hitvfx", true, 1f, -1, -1, null, false, GreaterSnakeshooterSpriteCollection, GreaterSnakeshooterAnimationCollection);
            ShellCasingSpawner sp = hitVFX1.AddComponent<ShellCasingSpawner>();
            sp.maxForce = 12;
            sp.minForce = 10;
            sp.shellsToLaunch = 10;
            sp.angleVariance = 15f;
            sp.inheritRotationAsDirection = true;
            sp.shellCasing = casing.gameObject;

            hitEffects.additionalHitEffects = new HitEffectHandler.AdditionalHitEffect[]
            {
                new HitEffectHandler.AdditionalHitEffect()
                {
                    chance = 1,
                    delay = 0.4f,
                    hitEffect = new VFXPool()
                    {
                        type = VFXPoolType.All,
                        effects = new VFXComplex[]
                        {
                            new VFXComplex()
                            {
                                effects = new VFXObject[]
                                {
                                    new VFXObject()
                                    {
                                        alignment = VFXAlignment.VelocityAligned,
                                        attached = false,
                                        destructible = false,
                                        orphaned = true,
                                        persistsOnDeath = false,
                                        usesZHeight = true,
                                        zHeight = 1,
                                        effect = hitVFX1,
                                    }
                                }
                            }
                        }
                    }
                }
            };

            ObjectVisibilityManager vis = enemy.AddComponent<ObjectVisibilityManager>();


            SpriteBuilder.AddToAmmonomicon(GreaterSnakeshooterSpriteCollection.GetSpriteDefinition("greatersnakeshooter_idleleft_001"));

            ETGMod.Databases.Strings.Enemies.AddComplex("#GREATERSNAKESHOOTER_ENCNAME", "Greater Snakeshooter");
            ETGMod.Databases.Strings.Enemies.AddComplex("#GREATERSNAKESHOOTER_SHORTDESC", "In My Boot");
            ETGMod.Databases.Strings.Enemies.AddComplex("#GREATERSNAKESHOOTER_LONGDESC", "These aged Snakeshooters far outlive their brethren by adopting a queer form of neoteny, avoiding any competition with their terrible parent.");

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
                enemyPortraitSprite = Initialisation.assetBundle.LoadAsset<Texture2D>("ammonomicon_greatersnakeshooter"),
                AmmonomiconSprite = "greatersnakeshooter_idleleft_001",
                PrimaryDisplayName = "#GREATERSNAKESHOOTER_ENCNAME",
                NotificationPanelDescription = "#GREATERSNAKESHOOTER_SHORTDESC",
                AmmonomiconFullEntry = "#GREATERSNAKESHOOTER_LONGDESC",
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
            Game.Enemies.Add("nn:greater_snakeshooter", aiActor);
        }
    }
    public class GreaterSnakeshooterRipoffAttack : Script
    {
        public override IEnumerator Top()
        {
            float input = BraveMathCollege.QuantizeFloat(base.GetAimDirection(1f, 12f), 90f);
            GreaterSnakeshooterRipoffAttack.NibblesBullet parent = null;
            for (int i = 0; i < 8; i++)
            {
                GreaterSnakeshooterRipoffAttack.NibblesBullet nibblesBullet = new GreaterSnakeshooterRipoffAttack.NibblesBullet(0.5f, parent);
                base.Fire(new Direction(BraveMathCollege.QuantizeFloat(input, 90f), DirectionType.Absolute, -1f), new Speed(12f, SpeedType.Absolute), nibblesBullet);
                parent = nibblesBullet;
            }
            return null;
        }

        public class NibblesBullet : Bullet
        {
            public NibblesBullet(float delay, GreaterSnakeshooterRipoffAttack.NibblesBullet parent) : base("nibblesBullet", false, false, false)
            {
                this.delay = delay;
                this.parent = parent;
                if (parent != null)
                {
                    parent.child = this;
                }
            }
            public override IEnumerator Top()
            {
                base.ManualControl = true;

                //If the parent is real, this means it's a follower bullet, so we loop this code until the parent is destroyed.
                if (this.parent != null)
                {
                    while (this.parent != null && !this.parent.Destroyed)
                    {
                        yield return base.Wait(1);
                    }
                    base.Vanish(false);
                    yield break;
                }

                //While the child of this is null, we wait.
                while (this.child == null)
                {
                    yield return base.Wait(1);
                }

                //Wait 3 Frames
                yield return base.Wait(3);

                this.Projectile.spriteAnimator.Play();
                bool primedForTurn = true;

                //Iterate 120 times over 120 frames
                for (int i = 0; i < 120; i++)
                {
                    

                    if (primedForTurn)
                    {
                        float direction360 = BraveMathCollege.ClampAngle360(this.Direction);
                        float directionQuantized = BraveMathCollege.QuantizeFloat(direction360, 90f);

                        Vector2 target = base.BulletManager.PlayerPosition();

                        bool alignedX = (directionQuantized == 90 || directionQuantized == 270) && Mathf.Abs(base.Position.y - target.y) <= 1.5f;
                        bool alignedY = (directionQuantized == 0 || directionQuantized == 180) && Mathf.Abs(base.Position.x - target.x) <= 1.5f;

                        if (alignedX || alignedY)
                        {
                            this.prevDirection = this.Direction;
                            float input = BraveMathCollege.QuantizeFloat(base.GetAimDirection(1f, 12f), 90f);
                            this.Direction = input;
                            this.Projectile.spriteAnimator.StopAndResetFrameToDefault();
                            primedForTurn = false;
                        }
                    }

                    //Set previous position to the current position of this bullet.
                    this.prevPosition = base.Position;

                    //Move this bullet forwards by an amount determine by its speed
                    base.Position += BraveMathCollege.DegreesToVector(this.Direction, this.Speed / 60f * 3f);

                    //Iterate through all bullets in the chain and set their position and direction data to the last data of the bullet before them
                    GreaterSnakeshooterRipoffAttack.NibblesBullet ptr = this;
                    while (ptr.child != null)
                    {
                        if (ptr.delay > i)
                        {
                            break;
                        }
                        ptr.child.prevDirection = ptr.child.Direction;
                        ptr.child.Direction = ptr.prevDirection;
                        ptr.child.prevPosition = ptr.child.Position;
                        ptr.child.Position = ptr.prevPosition;
                        ptr = ptr.child;
                    }


                    //We wait to give the impression of jagged pixel-like motion
                    yield return base.Wait(3);
                }

                //Destroy leader bullet
                base.Vanish(false);
                yield break;
            }

            private float delay;
            private GreaterSnakeshooterRipoffAttack.NibblesBullet parent;
            private GreaterSnakeshooterRipoffAttack.NibblesBullet child;
            private float prevDirection;
            private Vector2 prevPosition;
        }
    }

}

