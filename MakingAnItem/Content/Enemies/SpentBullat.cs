using Alexandria.BreakableAPI;
using Alexandria.EnemyAPI;
using Alexandria.ItemAPI;
using Alexandria.Misc;
using Alexandria.NPCAPI;
using Brave.BulletScript;
using FullInspector;
using Gungeon;
using HutongGames.PlayMaker.Actions;
using Microsoft.Cci;
using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class SpentBullat
    {
        public static tk2dSpriteCollectionData SpentBullatSpriteCollection;
        public static tk2dSpriteAnimation SpentBullatAnimationCollection;

        public static GameObject prefab;
        public static readonly string guid = "omitb_spentbullat";
        public static void Init()
        {
            SpentBullatSpriteCollection = AssetBundleLoader.FastLoadSpriteCollection(Initialisation.assetBundle, "ENM_SpentBullat_Collection", "ENM_SpentBullat_Collection_Material");
            SpentBullatAnimationCollection = Initialisation.assetBundle.LoadAsset<GameObject>("ENM_SpentBullat_Anim").GetComponent<tk2dSpriteAnimation>();


            var enemy = EntityTools.SetupEntityObject("Spent Bullat", "spentbullat_idle_001", SpentBullatSpriteCollection);

            var enemyRigidBody = EntityTools.SetupEntityRigidBody(enemy,
                new List<CollisionLayer>() { CollisionLayer.EnemyCollider, CollisionLayer.EnemyHitBox }, //Collision Layers
                new List<IntVector2>() { new IntVector2(7, 8), new IntVector2(7, 8) }, //Collider Dimensions
                new List<IntVector2>() { new IntVector2(-3, 0), new IntVector2(-3, 0) } //Collider Offsets
                );

            EntityTools.SetupEntityKnockback(enemy, weight: 10, deathMultiplier: 1);

            tk2dSpriteAnimator animator = enemy.GetOrAddComponent<tk2dSpriteAnimator>();
            animator.Library = SpentBullatAnimationCollection;
            AIAnimator AIanimator = enemy.AddComponent<AIAnimator>();

            AIanimator.IdleAnimation = new DirectionalAnimation()
            {
                Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
                AnimNames = new string[]
               {
                    "idle",
                    "idle"
               },
                Flipped = new DirectionalAnimation.FlipType[2]
            };
            AIanimator.MoveAnimation = new DirectionalAnimation()
            {
                Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
                AnimNames = new string[]
                {
                    "idle",
                    "idle"
                },
                Flipped = new DirectionalAnimation.FlipType[2]
            };

            tk2dSpriteAnimationFrame outLineDis = animator.spriteAnimator.GetClipByName("death").frames[1];
            outLineDis.triggerEvent = true;
            outLineDis.eventInfo = "DisableOutlines";
            outLineDis.eventOutline = tk2dSpriteAnimationFrame.OutlineModifier.TurnOff;

            tk2dSpriteAnimationFrame chargeFrame1 = animator.spriteAnimator.GetClipByName("charge").frames[0];
            chargeFrame1.triggerEvent = true;
            chargeFrame1.eventInfo = "Charge Audio";
            chargeFrame1.eventAudio = "Play_ENM_bullat_tackle_01";

            
            var healthHaver = enemy.AddComponent<HealthHaver>();
            healthHaver.RegisterBodySprite(enemy.GetComponent<tk2dBaseSprite>());
            healthHaver.PreventAllDamage = false;
            healthHaver.SetHealthMaximum(5);
            healthHaver.ForceSetCurrentHealth(5f);

            var aiActor = enemy.AddComponent<AIActor>();
            aiActor.MovementSpeed = 4.5f;
            aiActor.CollisionDamage = 0.5f;
            aiActor.State = AIActor.ActorState.Normal;
            aiActor.CanTargetPlayers = true;
            aiActor.CanTargetEnemies = false;
            aiActor.ForcedPositionInAmmonomicon = 70;
            aiActor.EnemyGuid = guid;
            aiActor.AvoidRadius = 4;
            aiActor.ActorName = "Spent Bullat";
            aiActor.EnemySwitchState = "Bullet_Bat";
            aiActor.BlackPhantomProperties = new BlackPhantomProperties();
            aiActor.BlackPhantomProperties.BonusHealthPercentIncrease = 3f;

            aiActor.SetIsFlying(true, "Flying Entity", true, true);
            aiActor.shadowHeightOffset = -1f;

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
                    BulletObject = EnemyBulletDatabase.Regular_EightByEight,
                    AudioEvent = "Play_WPN_SAA_shot_01",
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
                new SpentBullatSeekTargetBehaviour
                {
                    pathfind = true,
                    pathInterval =0.25f,
                    stoppedTurnMultiplier = 3f,
                    targetTolerance = 30f,
                    turnTime = 0.5f,
                    dashAnim = "charge",
                    chargeTime = 0.3f,
                    chargeCooldown = 3f,
                    warpDashAnimLength = true,
                }
            };

            aiActor.CorpseObject = null;

            ObjectVisibilityManager vis = enemy.AddComponent<ObjectVisibilityManager>();


            SpriteBuilder.AddToAmmonomicon(SpentBullatSpriteCollection.GetSpriteDefinition("spentbullat_idle_001"));

            ETGMod.Databases.Strings.Enemies.AddComplex("#SPENTBULLAT_ENCNAME", "Spent Bullat");
            ETGMod.Databases.Strings.Enemies.AddComplex("#SPENTBULLAT_SHORTDESC", "Brainless Bullat");
            ETGMod.Databases.Strings.Enemies.AddComplex("#SPENTBULLAT_LONGDESC", "Damaged and malforged bullats, unable to launch themselves in a blaze of glory like their healthier cousins.\n\nSpent Bullats are often expelled from Bullat nesting grounds.");

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
                enemyPortraitSprite = Initialisation.assetBundle.LoadAsset<Texture2D>("ammonomicon_spentbullat"),
                AmmonomiconSprite = "spentbullat_idle_001",
                PrimaryDisplayName = "#SPENTBULLAT_ENCNAME",
                NotificationPanelDescription = "#SPENTBULLAT_SHORTDESC",
                AmmonomiconFullEntry = "#SPENTBULLAT_LONGDESC",
                SpecialIdentifier = JournalEntry.CustomJournalEntryType.NONE,
                SuppressKnownState = false,
                SuppressInAmmonomicon = false,
                IsEnemy = true,
                DisplayOnLoadingScreen = false,
                RequiresLightBackgroundInLoadingScreen = false
            };

            /*
            tk2dBaseSprite enemySprite = enemy.GetComponent<tk2dBaseSprite>();
            Material mat = new Material(EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material);
            mat.mainTexture = enemySprite.renderer.material.mainTexture;
            mat.SetColor("_EmissiveColor", Color.red);
            mat.SetFloat("_EmissiveColorPower", 20);
            mat.SetFloat("_EmissivePower", 20);
            enemySprite.renderer.material = mat;
            */


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
                ForcedPositionInAmmonomicon = 70,
            };

            prefab = enemy;
            EncounterDatabase.Instance.Entries.Add(encounterEntry);
            EnemyDatabase.Instance.Entries.Add(enemyDatabaseEntry);
            EnemyBuilder.Dictionary.Add(guid, prefab);

            GameObject.DontDestroyOnLoad(prefab);
            FakePrefab.MarkAsFakePrefab(prefab);
            prefab.SetActive(false);
            Game.Enemies.Add("nn:spent_bullat", aiActor);
        }
    }
    public class SpentBullatSeekTargetBehaviour : RangedMovementBehavior
    {
        public float initialCooldown = 4f;
        public override void Start()
        {
            base.Start();
            m_updateEveryFrame = true;
            m_direction = -90f;
            if (m_aiAnimator)
            {
                m_aiAnimator.FacingDirection = -90f;
            }
            m_targetCenter = m_aiActor.specRigidbody.GetUnitCenter(ColliderType.Ground);
            chargeCooldown = initialCooldown + UnityEngine.Random.Range(-chargeCooldownVariance, chargeCooldownVariance);
        }

        public override void Upkeep()
        {
            base.Upkeep();
            m_timer += m_deltaTime;
            timeSinceLastCharge += m_deltaTime;
            m_timeSinceLastUpdate += m_deltaTime;
            DecrementTimer(ref m_pathTimer, false);
            DecrementTimer(ref timeToNextCharge, false);
            DecrementTimer(ref m_chargeTimeRemaining, false);
            DecrementTimer(ref randomVelocityTime, false);

        }
        public override BehaviorResult Update()
        {
            if (randomVelocityTime <= 0)
            {
                randomVelocityTime = UnityEngine.Random.Range(0.5f, 2f);
                randomVelocity = BraveUtility.RandomAngle().DegreeToVector2().normalized * UnityEngine.Random.Range(m_aiActor.MovementSpeed * 0.1f, m_aiActor.MovementSpeed * 0.5f);
            }
            if (m_timeSinceLastUpdate > 0.4f)
            {
                m_direction = m_aiAnimator.FacingDirection;
            }
            m_timeSinceLastUpdate = 0f;

            if (pathfind && !m_aiActor.HasLineOfSightToTarget)
            {
                if (m_pathTimer <= 0f)
                {
                    UpdateTargetCenter();
                    Path path = null;
                    if (Pathfinder.Instance.GetPath(m_aiActor.PathTile, m_targetCenter.ToIntVector2(VectorConversions.Floor), out path, new IntVector2?(m_aiActor.Clearance), m_aiActor.PathableTiles, null, null, false) && path.Count > 0)
                    {
                        path.Smooth(m_aiActor.specRigidbody.UnitCenter, m_aiActor.specRigidbody.UnitDimensions / 2f, m_aiActor.PathableTiles, false, m_aiActor.Clearance);
                        m_targetCenter = path.GetFirstCenterVector2();
                    }
                    m_pathTimer += pathInterval;
                }
            }
            else
            {
                UpdateTargetCenter();
            }

            if (m_chargeTimeRemaining == 0 && timeToNextCharge <= 0 && m_aiActor.HasLineOfSightToTarget && lingeringChargeVelocity.magnitude <= 0)
            {
                m_chargeTimeRemaining = chargeTime;
                timeToNextCharge = chargeCooldown + UnityEngine.Random.Range(-chargeCooldownVariance, chargeCooldownVariance);

                Vector2 cent = m_aiActor.specRigidbody.UnitCenter;
                lingeringChargeVelocity = (m_targetCenter - cent).normalized * (m_aiActor.MovementSpeed * 3f);
                lastLingeringChargeVelocity = lingeringChargeVelocity;
                timeSinceLastCharge = 0;
                randomVelocity = Vector2.zero;
                if (!string.IsNullOrEmpty(dashAnim))
                {
                    if (warpDashAnimLength)
                    {
                        AIAnimator aiAnimator = m_aiAnimator;
                        string name = dashAnim;
                        bool suppressHitStates = true;
                        float warpClipDuration = chargeTime;
                        aiAnimator.PlayUntilFinished(name, suppressHitStates, null, warpClipDuration, false);
                    }
                    else
                    {
                        m_aiAnimator.PlayUntilFinished(dashAnim, true, null, -1f, false);
                    }
                }
                SpawnManager.SpawnVFX((PickupObjectDatabase.GetById(24) as Gun).DefaultModule.projectiles[0].hitEffects.tileMapVertical.effects[0].effects[0].effect, cent, Quaternion.identity);
                m_direction = (m_targetCenter - cent).ToAngle();
            }


            float turn_time = turnTime;

            //Multiply turn time by the 'stopped turn multiplier' if the velocity of the aiactor is less than half its movement speed.
            if (stoppedTurnMultiplier != 0f && m_aiActor.specRigidbody.Velocity.magnitude < m_aiActor.MovementSpeed / 2f)
            {
                turn_time *= stoppedTurnMultiplier;
            }

            //Get the angle to the target
            Vector2 unitCenter = m_aiActor.specRigidbody.UnitCenter;
            float angleToTarget = (m_targetCenter - unitCenter).ToAngle();

            //If there is a degree of tolerance for the target angle
            if (targetTolerance > 0f)
            {
                //Difference between the current direction of the actor and the angle to its target
                float difference = Mathf.DeltaAngle(angleToTarget, m_direction);
                //If the difference (Mathf.Abs converts to always be a positive number) between the angles is less than the allowed maximum tolerance...
                if (Mathf.Abs(difference) < targetTolerance)
                {
                    // ...the angle is just set to the actor's current direction
                    angleToTarget = m_direction;
                }
                else // Otherwise...
                {
                    // Add the tolerance value to the angle. Mathf.Sign returns 1 if the difference is positive and -1 if it is negative, ensuring that the angle added always matches the actual direction
                    angleToTarget += Mathf.Sign(difference) * targetTolerance;
                }
            }
            
            //Direction is set to a smooth value between the current direction and the angle of the target.
            // turn_time is how long (approximately) we want it to take to reach the desired angle.
            m_direction = Mathf.SmoothDampAngle(m_direction, angleToTarget, ref m_angularVelocity, turn_time);


            m_aiActor.BehaviorOverridesVelocity = true;
            m_aiActor.BehaviorVelocity = BraveMathCollege.DegreesToVector(m_direction, m_aiActor.MovementSpeed) + lingeringChargeVelocity + randomVelocity;
            lingeringChargeVelocity =  Vector2.Lerp(lastLingeringChargeVelocity, Vector2.zero, timeSinceLastCharge / chargeTime);

            return BehaviorResult.Continue;
        }
        private Vector2 lingeringChargeVelocity = Vector2.zero;
        private Vector2 lastLingeringChargeVelocity = Vector2.zero;
        private Vector2 randomVelocity = Vector2.zero;
        private float randomVelocityTime = 1f;
        public string dashAnim;
        public bool warpDashAnimLength;
        private void UpdateTargetCenter()
        {
            if (m_aiActor.TargetRigidbody)
            {
                m_targetCenter = m_aiActor.TargetRigidbody.GetUnitCenter(ColliderType.HitBox);
            }
        }
        private float timeSinceLastCharge = 0;
        public float chargeTime = 0.3f;
        private float m_chargeTimeRemaining = 0f;
        public float chargeCooldown = 1f;
        public float chargeCooldownVariance = 0.5f;
        private float timeToNextCharge = 1f;

        public float turnTime = 1f;
        public float stoppedTurnMultiplier = 1f;
        public float targetTolerance;
        public bool pathfind;
        public float pathInterval = 0.25f;

        private Vector2 m_targetCenter;
        private float m_timer;
        private float m_pathTimer;
        private float m_direction = -90f;
        private float m_angularVelocity;
        private float m_timeSinceLastUpdate;
    }
}
