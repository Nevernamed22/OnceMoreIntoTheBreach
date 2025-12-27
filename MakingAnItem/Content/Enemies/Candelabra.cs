using Alexandria.BreakableAPI;
using Alexandria.DungeonAPI;
using Alexandria.EnemyAPI;
using Alexandria.ItemAPI;
using Alexandria.CharacterAPI;
using Brave.BulletScript;
using Dungeonator;
using Gungeon;
using Microsoft.Cci;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Alexandria.Misc;
using UnityEngine;
using System.ComponentModel;
using FullInspector;

namespace NevernamedsItems
{
    public class Candelabra
    {
        public static tk2dSpriteCollectionData CandelabraSpriteCollection;
        public static tk2dSpriteAnimation CandelabraAnimationCollection;

        public static GameObject prefab;
        public static readonly string guid = "omitb_candelabra";
        public static void Init()
        {
            CandelabraSpriteCollection = AssetBundleLoader.FastLoadSpriteCollection(Initialisation.assetBundle, "ENM_Candelabra_Collection", "ENM_Candelabra_Collection_Material");
            CandelabraAnimationCollection = Initialisation.assetBundle.LoadAsset<GameObject>("ENM_Candelabra_Anim").GetComponent<tk2dSpriteAnimation>();


            var enemy = EntityTools.SetupEntityObject("Candelabra", "candelabra_base_001", CandelabraSpriteCollection);

            var enemyRigidBody = EntityTools.SetupEntityRigidBody(enemy,
                new List<CollisionLayer>() { CollisionLayer.EnemyCollider, }, //Collision Layers
                new List<IntVector2>() { new IntVector2(20, 13) }, //Collider Dimensions
                new List<IntVector2>() { new IntVector2(-10, 1) } //Collider Offsets
                );


            tk2dSpriteAnimator animator = enemy.GetOrAddComponent<tk2dSpriteAnimator>();
            animator.Library = CandelabraAnimationCollection;
            animator.defaultClipId = CandelabraAnimationCollection.GetClipIdByName("candelabra_base");
            animator.DefaultClipId = CandelabraAnimationCollection.GetClipIdByName("candelabra_base");
            animator.playAutomatically = true;

            var healthHaver = enemy.AddComponent<HealthHaver>();
            healthHaver.RegisterBodySprite(enemy.GetComponent<tk2dBaseSprite>());
            healthHaver.PreventAllDamage = false;
            healthHaver.SetHealthMaximum(120);
            healthHaver.ForceSetCurrentHealth(120);
            healthHaver.flashesOnDamage = true;
            healthHaver.TrackPixelColliderDamage = true;

            var aiActor = enemy.AddComponent<AIActor>();
            aiActor.MovementSpeed = 0f;
            aiActor.CollisionDamage = 0f;
            aiActor.State = AIActor.ActorState.Normal;
            aiActor.CanTargetPlayers = true;
            aiActor.CanTargetEnemies = false;
            aiActor.ForcedPositionInAmmonomicon = 30;
            aiActor.EnemyGuid = guid;
            aiActor.AvoidRadius = 0;
            aiActor.ActorName = "Candelabra";
            aiActor.EnemySwitchState = "";
            aiActor.BlackPhantomProperties = new BlackPhantomProperties();
            aiActor.BlackPhantomProperties.BonusHealthPercentIncrease = 2f;


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
                    BulletObject = EnemyBulletDatabase.Fireball_EightByEight,
                    AudioEvent = "SND_WPN_burninghand_shot_01",
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
                        damage = 5f,
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
            bs.AttackBehaviors = new List<AttackBehaviorBase>
            {
                new CandelabraAttack()
                {
                    Cooldown = 4f,
                    CooldownVariance = 1f,
                    RequiresLineOfSight = false,
                    Range = 1000,
                    InitialCooldown = 3f,
                    InitialCooldownVariance = 1f,
                }
            };

            aiActor.CorpseObject = null;

            ObjectVisibilityManager vis = enemy.AddComponent<ObjectVisibilityManager>();


            GameObject candle1 = SetupCandle(enemy, new IntVector2(0, 19 + 9), "Center_Candle");

            GameObject candle2 = SetupCandle(enemy, new IntVector2(-17, 15 + 9), "Left_Candle");

            GameObject candle3 = SetupCandle(enemy, new IntVector2(16, 15 + 9), "Right_Candle");


            enemy.AddComponent<CandelabraController>();


            //SpriteBuilder.AddToAmmonomicon(MuskinSpriteCollection.GetSpriteDefinition("muskin_idleleft_001"));

            ETGMod.Databases.Strings.Enemies.AddComplex("#CANDELABRA_ENCNAME", "Candelabra");
            ETGMod.Databases.Strings.Enemies.AddComplex("#CANDELABRA_SHORTDESC", "");
            ETGMod.Databases.Strings.Enemies.AddComplex("#CANDELABRA_LONGDESC", "");

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
                PrimaryDisplayName = "#CANDELABRA_ENCNAME",
                NotificationPanelDescription = "#CANDELABRA_SHORTDESC",
                AmmonomiconFullEntry = "#CANDELABRA_LONGDESC",
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
            aiActor.isPassable = false;
            prefab = enemy;
            EncounterDatabase.Instance.Entries.Add(encounterEntry);
            EnemyDatabase.Instance.Entries.Add(enemyDatabaseEntry);
            EnemyBuilder.Dictionary.Add(guid, prefab);

            GameObject.DontDestroyOnLoad(prefab);
            FakePrefab.MarkAsFakePrefab(prefab);
            prefab.SetActive(false);
            Game.Enemies.Add("nn:candelabra", aiActor);

            SetupShootPoint(1);
            SetupShootPoint(2);
            SetupShootPoint(3);
            SetupShootPoint(4);
            SetupShootPoint(5);

            telegraph = ItemBuilder.SpriteFromBundle("candelabratelegraph_spawn_001", CandelabraSpriteCollection.GetSpriteIdByName("candelabratelegraph_spawn_001"), CandelabraSpriteCollection, new GameObject("Candelabra Telegraph Point"));
            telegraph.MakeFakePrefab();
            tk2dSpriteAnimator telegraphAnimator = telegraph.GetOrAddComponent<tk2dSpriteAnimator>();
            telegraphAnimator.sprite.HeightOffGround = -1f;
            telegraphAnimator.Library = CandelabraAnimationCollection;
            telegraphAnimator.defaultClipId = CandelabraAnimationCollection.GetClipIdByName("candelabra_telegraph");
            telegraphAnimator.DefaultClipId = CandelabraAnimationCollection.GetClipIdByName("candelabra_telegraph");
            telegraphAnimator.playAutomatically = false;
            telegraphAnimator.sprite.IsPerpendicular = false;
        }
        public static GameObject telegraph;
        private static void SetupShootPoint(int num)
        {
            GameObject shootPoint = new GameObject($"Candelabra Shootpoint {num}");
            shootPoint.MakeFakePrefab();

            CandelabraShootpointController comp = shootPoint.AddComponent<CandelabraShootpointController>();
            comp.group = num;

            DungeonPlaceable shootPointPlaceable = BreakableAPIToolbox.GenerateDungeonPlaceable(new Dictionary<GameObject, float>() { { shootPoint.gameObject, 1f } });
            shootPointPlaceable.isPassable = true;
            shootPointPlaceable.width = 1;
            shootPointPlaceable.height = 1;
            StaticReferences.StoredDungeonPlaceables.Add($"candelabra_shootpoint_{num}", shootPointPlaceable);
            Alexandria.DungeonAPI.StaticReferences.customPlaceables.Add($"nn:candelabra_shootpoint_{num}", shootPointPlaceable);
        }
        public class CandelabraShootpointController : BraveBehaviour
        {
            public static Dictionary<RoomHandler, Dictionary<int, List<CandelabraShootpointController>>> allCandelabraShootPoints = new Dictionary<RoomHandler, Dictionary<int, List<CandelabraShootpointController>>>();
            public int group = 0;
            public RoomHandler parentRoom;
            public Vector2 GetShootPoint()
            {
                return base.transform.position + new Vector3(0.5f + UnityEngine.Random.Range(-0.1f, 0.1f), 0.5f + UnityEngine.Random.Range(-0.1f, 0.1f));
            }
            private void Start()
            {
                AkSoundEngine.SetSwitch("ENV_Trap", "flame", base.gameObject);
                parentRoom = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Round));
                if (!allCandelabraShootPoints.ContainsKey(parentRoom)) { allCandelabraShootPoints.Add(parentRoom, new Dictionary<int, List<CandelabraShootpointController>>() { }); }
                if (!allCandelabraShootPoints[parentRoom].ContainsKey(group)) { allCandelabraShootPoints[parentRoom].Add(group, new List<CandelabraShootpointController>()); }
                allCandelabraShootPoints[parentRoom][group].Add(this);
            }
        }
        private static GameObject SetupCandle(GameObject holder, IntVector2 offset, string name)
        {
            var candle1 = EntityTools.SetupEntityObject(name, "candelabra_idle_001", CandelabraSpriteCollection);

            tk2dSpriteAnimator candleAnimator = candle1.GetOrAddComponent<tk2dSpriteAnimator>();
            candleAnimator.Library = CandelabraAnimationCollection;
            candleAnimator.defaultClipId = CandelabraAnimationCollection.GetClipIdByName("candelabra_idle");
            candleAnimator.DefaultClipId = CandelabraAnimationCollection.GetClipIdByName("candelabra_idle");
            candleAnimator.playAutomatically = true;

            tk2dBaseSprite candleSprite = candle1.gameObject.GetComponent<tk2dBaseSprite>();
            candleSprite.HeightOffGround = 4f;

            candle1.transform.SetParent(holder.transform);
            candle1.transform.localPosition = new Vector3(offset.x / 16f, offset.y / 16f);

            IntVector2 colliderOffset = offset + new IntVector2(-6, 0);

            SpeculativeRigidbody holderbody = holder.GetComponent<SpeculativeRigidbody>();
            holderbody.PixelColliders.Add(new PixelCollider
            {
                ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
                CollisionLayer = CollisionLayer.BulletBlocker,
                IsTrigger = false,
                BagleUseFirstFrameOnly = false,
                SpecifyBagelFrame = string.Empty,
                BagelColliderNumber = 0,
                ManualOffsetX = colliderOffset.x,
                ManualOffsetY = colliderOffset.y,
                ManualWidth = 12,
                ManualHeight = 22,
                ManualDiameter = 0,
                ManualLeftX = 0,
                ManualLeftY = 0,
                ManualRightX = 0,
                ManualRightY = 0,
            });

            var flame = EntityTools.SetupEntityObject($"{name}_flame", "candelabra_flame_001", CandelabraSpriteCollection);

            tk2dSpriteAnimator flameAnimator = flame.GetOrAddComponent<tk2dSpriteAnimator>();
            flameAnimator.Library = CandelabraAnimationCollection;
            flameAnimator.defaultClipId = CandelabraAnimationCollection.GetClipIdByName("candelabra_flame");
            flameAnimator.DefaultClipId = CandelabraAnimationCollection.GetClipIdByName("candelabra_flame");
            flameAnimator.playAutomatically = true;

            flame.transform.SetParent(candle1.transform);
            flame.transform.localPosition = new Vector3(0f, 26f / 16f);

            tk2dBaseSprite flameSprite = flame.gameObject.GetComponent<tk2dBaseSprite>();
            flameSprite.HeightOffGround = 8f;

            candle1.AddComponent<CandelabraController.CandlabraCandleController>();
            return candle1;
        }
    }
    public class CandelabraController : BraveBehaviour
    {
        public static bool DebugLogging = true;
        public void Start()
        {
            if (DebugLogging) ETGModConsole.Log($"<color=#ff5521>Candelabra</color> | Starting CandelabraController");

            Center_Candle = base.transform.Find("Center_Candle").gameObject.GetComponent<CandlabraCandleController>();
            Center_Candle.pixelColliderNum = 1;
            Left_Candle = base.transform.Find("Left_Candle").gameObject.GetComponent<CandlabraCandleController>();
            Left_Candle.pixelColliderNum = 2;
            Right_Candle = base.transform.Find("Right_Candle").gameObject.GetComponent<CandlabraCandleController>();
            Right_Candle.pixelColliderNum = 3;

            Center_Candle.gameObject.transform.Find("Center_Candle_flame").gameObject.GetComponent<tk2dSpriteAnimator>().PlayFromFrame("candelabra_flame", UnityEngine.Random.Range(0, 5));
            Left_Candle.gameObject.transform.Find("Left_Candle_flame").gameObject.GetComponent<tk2dSpriteAnimator>().PlayFromFrame("candelabra_flame", UnityEngine.Random.Range(0, 5));
            Right_Candle.gameObject.transform.Find("Right_Candle_flame").gameObject.GetComponent<tk2dSpriteAnimator>().PlayFromFrame("candelabra_flame", UnityEngine.Random.Range(0, 5));

            Center_Candle.gameObject.GetComponent<tk2dSpriteAnimator>().PlayFromFrame("candelabra_idle", UnityEngine.Random.Range(0, 4));
            Left_Candle.gameObject.GetComponent<tk2dSpriteAnimator>().PlayFromFrame("candelabra_idle", UnityEngine.Random.Range(0, 4));
            Right_Candle.gameObject.GetComponent<tk2dSpriteAnimator>().PlayFromFrame("candelabra_idle", UnityEngine.Random.Range(0, 4));

            candle_center_pixelcollider = base.specRigidbody.PixelColliders[1];
            candle_left_pixelcollider = base.specRigidbody.PixelColliders[2];
            candle_right_pixelcollider = base.specRigidbody.PixelColliders[3];


            base.healthHaver.AddTrackedDamagePixelCollider(candle_center_pixelcollider);
            base.healthHaver.AddTrackedDamagePixelCollider(candle_left_pixelcollider);
            base.healthHaver.AddTrackedDamagePixelCollider(candle_right_pixelcollider);


            base.healthHaver.OnPreDeath += OnDeath;
            base.healthHaver.ManualDeathHandling = true;

            if (DebugLogging) ETGModConsole.Log($"<color=#ff5521>Candelabra</color> | Starting CandelabraController Complete");

        }
        public bool isDead = false;
        public void OnDeath(Vector2 f)
        {
            string candleActive = "UNDEFINED";
            if (DebugLogging)
            {
                ETGModConsole.Log($"<color=#ff5521>Candelabra</color> | Candelabra OnDeath Started");
                candleActive = Center_Candle == null || Center_Candle.dead ? "NULL" : "Active";
                ETGModConsole.Log($"    Center Candle: <color=#f5cd56>{candleActive}</color>");
            }

            //Check if Center Candle is not null, and then destroy it
            if (Center_Candle != null && !Center_Candle.dead)
            {
                if (DebugLogging) ETGModConsole.Log($"    Destroying Center Candle.");
                base.StartCoroutine(DestroyCandle(Center_Candle));
            }

            //Check if Left Candle is not null, and then destroy it
            if (DebugLogging)
            {
                candleActive = Left_Candle == null || Left_Candle.dead ? "NULL" : "Active";
                ETGModConsole.Log($"    Left Candle: <color=#f5cd56>{candleActive}</color>");
            }
            if (Left_Candle != null && !Left_Candle.dead)
            {
                if (DebugLogging) ETGModConsole.Log($"    Destroying Left Candle.");
                base.StartCoroutine(DestroyCandle(Left_Candle));
            }

            //Check if Right Candle is not null, and then destroy it
            if (DebugLogging)
            {
                candleActive = Right_Candle == null || Right_Candle.dead ? "NULL" : "Active";
                ETGModConsole.Log($"    Right Candle: <color=#f5cd56>{candleActive}</color>");
            }
            if (Right_Candle != null && !Right_Candle.dead)
            {
                if (DebugLogging) ETGModConsole.Log($"    Destroying Right Candle.");
                base.StartCoroutine(DestroyCandle(Right_Candle));
            }

            if (DebugLogging) ETGModConsole.Log($"<color=#ff5521>Candelabra</color> | Finished Candle Checks, Beginning Cleanup");

            isDead = true;
            base.aiActor.ForceDeath(Vector2.zero, false);
            if (DebugLogging) ETGModConsole.Log($"    Forced Death of Actor");
            base.aiActor.ImmuneToAllEffects = true;
            base.aiActor.RemoveAllEffects(true);
            if (DebugLogging) ETGModConsole.Log($"    Removed all effects");
            if (base.aiActor.IsBlackPhantom)
            {
                if (DebugLogging) ETGModConsole.Log($"    Actor is Jammed! Beginning special Jammed death handling");
                base.StartCoroutine(HandleJammedDeath());
            }
            else
            {
                if (DebugLogging) ETGModConsole.Log($"    Adding actor to corpse list");
                StaticReferenceManager.AllCorpses.Add(base.gameObject);
            }
            if (DebugLogging) ETGModConsole.Log($"<color=#ff5521>Candelabra</color> | Candelabra OnDeath Completed");
        }
        private IEnumerator HandleJammedDeath()
        {
            if (DebugLogging) ETGModConsole.Log($"<color=#ff5521>Candelabra</color> | Beginning Jammed death handling.");
            yield return new WaitForSeconds(0.25f);
            SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite);
            if (DebugLogging) ETGModConsole.Log($"    Outlines removed");
            yield return new WaitForSeconds(0.25f);
            base.specRigidbody.enabled = false;
            if (DebugLogging) ETGModConsole.Log($"<color=#ff5521>Candelabra</color> | Jammed Death Handling completed");
            UnityEngine.Object.Destroy(base.gameObject);
            yield break;
        }
        public void Update()
        {
            if (destroyedCandles >= 3 && !base.healthHaver.IsDead)
            {
                if (DebugLogging) ETGModConsole.Log($"<color=#ff5521>Candelabra</color> | All candles are destroyed, beginning emergency death!");
                base.healthHaver.ApplyDamage(1E+07f, Vector2.zero, "All Candles Dead", CoreDamageTypes.None, DamageCategory.Unstoppable, true, null, true);

                base.aiActor.ForceDeath(Vector2.zero);
            }
            if (destroyedCandles < 2 && base.gameObject.activeSelf && !isDead)
            {
                float candelabraMaxHealth = base.healthHaver.GetMaxHealth();
                float maximumAllowedDMG = candelabraMaxHealth / 3f;
                float centerDamage;
                if (Center_Candle != null && !Center_Candle.dead && base.healthHaver.PixelColliderDamage.TryGetValue(candle_center_pixelcollider, out centerDamage))
                {
                    if (centerDamage >= maximumAllowedDMG)
                    {
                        if (DebugLogging) ETGModConsole.Log($"<color=#ff5521>Candelabra</color> | Center Candle passed damage threshold, destroying...");
                        base.StartCoroutine(DestroyCandle(Center_Candle));
                    }
                }
                float leftDamage;
                if (Left_Candle != null && !Left_Candle.dead && base.healthHaver.PixelColliderDamage.TryGetValue(candle_left_pixelcollider, out leftDamage))
                {
                    if (leftDamage >= maximumAllowedDMG)
                    {
                        if (DebugLogging) ETGModConsole.Log($"<color=#ff5521>Candelabra</color> | Left Candle passed damage threshold, destroying...");
                        base.StartCoroutine(DestroyCandle(Left_Candle));
                    }
                }
                float rightDamage;
                if (Right_Candle != null && !Right_Candle.dead && base.healthHaver.PixelColliderDamage.TryGetValue(candle_right_pixelcollider, out rightDamage))
                {
                    if (rightDamage >= maximumAllowedDMG)
                    {
                        if (DebugLogging) ETGModConsole.Log($"<color=#ff5521>Candelabra</color> | Right Candle passed damage threshold, destroying...");
                        base.StartCoroutine(DestroyCandle(Right_Candle));
                    }
                }
            }
        }
        private IEnumerator DestroyCandle(CandlabraCandleController candle)
        {
            if (candle.dead)
            {
                if (DebugLogging) ETGModConsole.Log($"<color=#ff5521>Candelabra</color> | Tried to destroy a candle that was already dead, returning.");
                yield break;
            }
            if (DebugLogging) ETGModConsole.Log($"<color=#ff5521>Candelabra</color> | Beginning Candle Destruction ");
            candleIsPopping = true;
            candle.dead = true;
            destroyedCandles++;
            candle.Predeath();
            if (DebugLogging) ETGModConsole.Log($"    Playing sound and animation.");
            AkSoundEngine.PostEvent("Play_OBJ_candle_splat_01", base.gameObject);
            candle.spriteAnimator.Play("candelabra_die");
            if (DebugLogging) ETGModConsole.Log($"    Remove candle outline.");
            SpriteOutlineManager.RemoveOutlineFromSprite(candle.sprite);
            if (DebugLogging) ETGModConsole.Log($"    Set pixel collider of candle to trigger.");
            base.specRigidbody.PixelColliders[candle.pixelColliderNum].IsTrigger = true;
            base.specRigidbody.PixelColliders[candle.pixelColliderNum].Regenerate(base.transform);
            if (DebugLogging) ETGModConsole.Log($"    Destroying Candle Flame");
            if (candle.flame) UnityEngine.Object.Destroy(candle.flame);
            yield return new WaitForSeconds(0.49f);
            if (DebugLogging) ETGModConsole.Log($"    Destroying Candle");
            UnityEngine.GameObject.Destroy(candle.gameObject);
            candleIsPopping = false;
            if (DebugLogging) ETGModConsole.Log($"<color=#ff5521>Candelabra</color> | Ending Candle Destruction ");

            yield break;
        }
        public int destroyedCandles = 0;

        public bool candleIsPopping = false;

        private PixelCollider candle_center_pixelcollider;
        private PixelCollider candle_left_pixelcollider;
        private PixelCollider candle_right_pixelcollider;

        public CandlabraCandleController Center_Candle;
        public CandlabraCandleController Left_Candle;
        public CandlabraCandleController Right_Candle;

        public class CandlabraCandleController : BraveBehaviour
        {
            public void Start()
            {
                master = transform.parent.gameObject.GetComponent<CandelabraController>();
                flame = this.transform.Find($"{this.gameObject.name}_flame").gameObject;
                shootPoint = flame.transform;
                master.healthHaver.RegisterBodySprite(this.gameObject.GetComponent<tk2dBaseSprite>(), true, pixelColliderNum);
                SpriteOutlineManager.AddOutlineToSprite(this.GetComponent<tk2dBaseSprite>(), Color.black, 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
                ObjectVisibilityManager visManager = master.gameObject.GetComponent<ObjectVisibilityManager>();
                if (visManager)
                {
                    visManager.ResetRenderersList();
                }
            }
            public void Predeath()
            {
                if (DebugLogging) ETGModConsole.Log($"<color=#ff5521>Candelabra Candle</color> | Beginning Predeath");
                if (!master || !master.healthHaver)
                {
                    if (DebugLogging) ETGModConsole.Log($"<color=#ff5521>Candelabra Candle</color> | Candles does not have a master, or the master does not have a health-haver, returning...");
                    return;
                }
                tk2dBaseSprite sprites = this.gameObject.GetComponent<tk2dBaseSprite>();
                if (master.healthHaver.bodySprites.Contains(sprites))
                {
                    master.healthHaver.bodySprites.Remove(sprites);
                    if (DebugLogging) ETGModConsole.Log($"    Removing candle sprite from bodysprites.");
                }
                if (master.healthHaver.m_independentDamageFlashers != null)
                {
                    master.healthHaver.m_independentDamageFlashers.Remove(master.specRigidbody.PixelColliders[pixelColliderNum]);
                    if (DebugLogging) ETGModConsole.Log($"    Removing pixel collider from independant damage flashers.");
                }
                if (DebugLogging) ETGModConsole.Log($"<color=#ff5521>Candelabra Candle</color> | Ending Predeath");
            }
            public void Update()
            {
                if (master.aiActor.IsBlackPhantom && !cachedJammed)
                {
                    UpdateIsJammed(true);
                    cachedJammed = true;
                }
                if (!master.aiActor.IsBlackPhantom && cachedJammed)
                {
                    UpdateIsJammed(false);
                    cachedJammed = false;
                }
            }
            public void UpdateIsJammed(bool jammed)
            {
                tk2dBaseSprite tk2dBaseSprite = base.sprite;
                tk2dBaseSprite.usesOverrideMaterial = true;
                Material material = master.sprite.renderer.material;
                if (master.aiActor.m_cachedBodySpriteShader == null)
                {
                    master.aiActor.m_cachedBodySpriteShader = material.shader;
                }
                if (jammed)
                {
                    material.shader = ShaderCache.Acquire("Brave/LitCutoutUberPhantom");
                    material.SetFloat("_PhantomGradientScale", master.aiActor.BlackPhantomProperties.GradientScale);
                    material.SetFloat("_PhantomContrastPower", master.aiActor.BlackPhantomProperties.ContrastPower);
                    if (tk2dBaseSprite != base.sprite)
                    {
                        material.SetFloat("_ApplyFade", 0f);
                    }
                }
                else
                {
                    material.shader = master.aiActor.m_cachedBodySpriteShader;
                }
                tk2dBaseSprite.renderer.material = material;
            }
            public CandelabraController master;
            public bool cachedJammed = false;
            public int pixelColliderNum = 0;
            public Transform shootPoint;
            public GameObject flame;
            public bool dead = false;
        }
    }
    public class CandelabraAttack : BasicAttackBehavior
    {
        public CandelabraController.CandlabraCandleController Candle1;
        public CandelabraController.CandlabraCandleController Candle2;
        public CandelabraController.CandlabraCandleController Candle3;
        public CandelabraController master;
        public RoomHandler room;
        public Dictionary<int, List<Candelabra.CandelabraShootpointController>> availableShootPoints = null;
        public override void Start()
        {
            master = base.m_gameObject.GetComponent<CandelabraController>();
            Candle1 = master.Center_Candle;
            Candle2 = master.Left_Candle;
            Candle3 = master.Right_Candle;
            room = base.m_aiActor.parentRoom;
            if (Candelabra.CandelabraShootpointController.allCandelabraShootPoints.ContainsKey(room))
            {
                availableShootPoints = new Dictionary<int, List<Candelabra.CandelabraShootpointController>>();
                availableShootPoints.AddRange(Candelabra.CandelabraShootpointController.allCandelabraShootPoints[room]);
            }
            base.Start();
        }
        public override void OnActorPreDeath()
        {
            ResetAndClearAllAttackData();
        }
        public bool abortAllRemainingBulletSpawns = false;
        public override void Upkeep()
        {
            if (availableShootPoints == null)
            {
                if (Candelabra.CandelabraShootpointController.allCandelabraShootPoints.ContainsKey(room))
                {
                    availableShootPoints = new Dictionary<int, List<Candelabra.CandelabraShootpointController>>();
                    availableShootPoints.AddRange(Candelabra.CandelabraShootpointController.allCandelabraShootPoints[room]);
                }
            }
            base.Upkeep();
        }

        public bool drawing;
        public CandelabraController.CandlabraCandleController currentAttackingCandle = null;
        public override BehaviorResult Update()
        {
            base.Update();
            BehaviorResult behaviorResult = base.Update();
            if (behaviorResult != BehaviorResult.Continue)
            {
                return behaviorResult;
            }
            if (!this.IsReady())
            {
                return BehaviorResult.Continue;
            }

            List<CandelabraController.CandlabraCandleController> viableCandles = new List<CandelabraController.CandlabraCandleController>();
            Candle1 = master.Center_Candle;
            Candle2 = master.Left_Candle;
            Candle3 = master.Right_Candle;
            if (Candle1 != null && !Candle1.dead) { viableCandles.Add(Candle1); }
            if (Candle2 != null && !Candle2.dead) { viableCandles.Add(Candle2); }
            if (Candle3 != null && !Candle3.dead) { viableCandles.Add(Candle3); }
            if (viableCandles.Count > 0)
            {
                currentAttackingCandle = BraveUtility.RandomElement(viableCandles);
                State = AttackState.SUMMONING;
                return BehaviorResult.RunContinuous;
            }
            return BehaviorResult.Continue;
        }


        public List<Projectile> currentBullets = new List<Projectile>();
        public List<Candelabra.CandelabraShootpointController> remainingShootpoints = new List<Candelabra.CandelabraShootpointController>();
        public List<Candelabra.CandelabraShootpointController> midSpawnShootPoints = new List<Candelabra.CandelabraShootpointController>();

        private float m_remainingDelay = 0f;
        private float m_remainingSpawnDelay = 0;
        public float DelayAfterBulletSpawn = 0.0001f;
        public float DelayAfterSummon = 1f;
        public float DelayAfterDraw = 4f;
        public override ContinuousBehaviorResult ContinuousUpdate()
        {
            if (currentAttackingCandle == null || currentAttackingCandle.dead == true) //Candle has died, update cooldowns, transition to waiting state, and finish behaviour.
            {
                this.UpdateCooldowns();
                State = AttackState.WAITING;
                m_remainingSpawnDelay = 0;
                m_remainingDelay = 0;
                currentAttackingCandle = null;
                return ContinuousBehaviorResult.Finished;
            }

            if (m_remainingSpawnDelay > 0)
            {
                m_remainingSpawnDelay -= base.m_deltaTime;
                return ContinuousBehaviorResult.Continue;
            }

            if (m_remainingDelay > 0) //If delay time is greater than zero, wait
            {
                m_remainingDelay -= base.m_deltaTime;
                if (m_remainingDelay <= 0)
                {
                    switch (State)
                    {
                        case AttackState.DRAWING:
                            State = AttackState.WAITING;
                            return ContinuousBehaviorResult.Finished;
                        case AttackState.SUMMONING:
                            State = AttackState.DRAWING;
                            return ContinuousBehaviorResult.Continue;
                    }
                    return ContinuousBehaviorResult.Continue;
                }
                else
                {
                    return ContinuousBehaviorResult.Continue;
                }
            }

            if (State == AttackState.SUMMONING)
            {
                if (remainingShootpoints.Count == 0 && midSpawnShootPoints.Count == 0)
                {
                    State = AttackState.DRAWING;
                    return ContinuousBehaviorResult.Continue;
                }
                else if (remainingShootpoints != null && remainingShootpoints.Count > 0)
                {
                    Candelabra.CandelabraShootpointController point = BraveUtility.RandomElement(remainingShootpoints);
                    remainingShootpoints.Remove(point);
                    m_remainingSpawnDelay = DelayAfterBulletSpawn;
                    point.StartCoroutine(TelegraphAndSpawnBullet(point));
                    return ContinuousBehaviorResult.Continue;
                }
            }
            return ContinuousBehaviorResult.Continue;
        }
        public IEnumerator TelegraphAndSpawnBullet(Candelabra.CandelabraShootpointController point)
        {
            midSpawnShootPoints.Add(point);
            //AkSoundEngine.PostEvent("Play_WPN_earthwormgun_shot_01", point.gameObject);
            GameObject instanceTelegraph = SpawnManager.SpawnVFX(Candelabra.telegraph, point.transform.position, Quaternion.identity);
            instanceTelegraph.GetComponent<tk2dSpriteAnimator>().Play("candelabra_telegraph");
            yield return new WaitForSeconds(1f);
            if (!abortAllRemainingBulletSpawns)
            {
                SpawnManager.SpawnVFX(SharedVFX.FlameTrapRing, point.GetShootPoint(), Quaternion.identity);
                SpawnManager.SpawnVFX(SharedVFX.FlameTrapPoof, point.GetShootPoint(), Quaternion.identity);
                AkSoundEngine.PostEvent("Play_ENV_trap_trigger", point.gameObject);
                currentBullets.Add(SpawnBullet(point));
            }
            midSpawnShootPoints.Remove(point);
            instanceTelegraph.GetComponent<tk2dSpriteAnimator>().PlayAndDestroyObject("candelabra_telegraph_vanish");
            yield break;
        }
        public Projectile SpawnBullet(Candelabra.CandelabraShootpointController point)
        {
            AIBulletBank.Entry bullet = this.m_aiActor.bulletBank.GetBullet("default");
            GameObject bulletObject = bullet.BulletObject;
            GameObject gameObject = SpawnManager.SpawnProjectile(bulletObject, point.GetShootPoint(), Quaternion.Euler(0f, 0f, 0f), true);

            Projectile component = gameObject.GetComponent<Projectile>();
            if (bullet != null && bullet.OverrideProjectile)
            {
                component.baseData.SetAll(bullet.ProjectileData);
            }
            component.Shooter = this.m_aiActor.specRigidbody;
            component.specRigidbody.Velocity = Vector2.zero;
            component.ManualControl = true;
            component.specRigidbody.CollideWithTileMap = false;
            component.collidesWithEnemies = base.m_aiActor.CanTargetEnemies;
            component.UpdateCollisionMask();
            return component;
        }


        private enum AttackState
        {
            WAITING,
            SUMMONING,
            DRAWING
        }
        private AttackState m_state;
        private AttackState State
        {
            get
            {
                return this.m_state;
            }
            set
            {
                this.EndState(this.m_state);
                this.m_state = value;
                this.BeginState(this.m_state);
            }
        }
        private void EndState(AttackState State)
        {
            switch (State)
            {
                case AttackState.SUMMONING:
                    m_remainingDelay = DelayAfterSummon;
                    break;
            }
        }
        private void BeginState(AttackState State)
        {
            switch (State)
            {
                case AttackState.SUMMONING:
                    ResetAndClearAllAttackData();
                    abortAllRemainingBulletSpawns = false;
                    remainingShootpoints.AddRange(availableShootPoints[UnityEngine.Random.Range(1, availableShootPoints.Keys.Count + 1)]);
                    if (currentAttackingCandle != null && !currentAttackingCandle.dead) currentAttackingCandle.spriteAnimator.Play("candelabra_summon");
                    DelayAfterBulletSpawn = 3f / remainingShootpoints.Count();
                    m_updateEveryFrame = true;
                    break;
                case AttackState.WAITING:
                    ResetAndClearAllAttackData();
                    m_updateEveryFrame = false;
                    if (currentAttackingCandle != null && !currentAttackingCandle.dead) currentAttackingCandle.spriteAnimator.Play("candelabra_idle");
                    break;
                case AttackState.DRAWING:
                    AkSoundEngine.PostEvent("Play_ENM_tutorialknight_appear_01", base.m_gameObject);
                    for (int i = currentBullets.Count - 1; i >= 0; i--)
                    {
                        Projectile bullet = currentBullets[i];
                        if (bullet != null)
                        {
                            bullet.ManualControl = false;
                            bullet.specRigidbody.CollideWithTileMap = true;
                            if (currentAttackingCandle)
                            {
                                bullet.SendInDirection(bullet.SafeCenter.CalculateVectorBetween(currentAttackingCandle.shootPoint.position), true, true);
                                bullet.baseData.range = Vector2.Distance(bullet.SafeCenter, currentAttackingCandle.shootPoint.position);
                            }
                            else
                            {
                                bullet.DieInAir();
                            }
                            currentBullets.Remove(bullet);
                        }
                    }
                    ResetAndClearAllAttackData();
                    if (currentAttackingCandle != null && !currentAttackingCandle.dead) currentAttackingCandle.spriteAnimator.Play("candelabra_attack");
                    m_remainingDelay = DelayAfterDraw;
                    break;

            }
        }
        private void ResetAndClearAllAttackData()
        {
            remainingShootpoints.Clear();
            remainingShootpoints = new List<Candelabra.CandelabraShootpointController>();
            midSpawnShootPoints.Clear();
            midSpawnShootPoints = new List<Candelabra.CandelabraShootpointController>();

            if (currentBullets != null && currentBullets.Count > 0)
            {
                for (int i = currentBullets.Count - 1; i >= 0; i--)
                {
                    Projectile bullet = currentBullets[i];
                    if (bullet != null)
                    {
                        bullet.ManualControl = false;
                        bullet.DieInAir();
                    }
                }
            }
            currentBullets.Clear();
            abortAllRemainingBulletSpawns = true;
        }
    }
}
