using Alexandria.BreakableAPI;
using Alexandria.DungeonAPI;
using Alexandria.ItemAPI;
using Alexandria.Misc;
using Dungeonator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class BloodCandle : BraveBehaviour
    {
        public static void Init()
        {
            GameObject candleHalo = ItemBuilder.SpriteFromBundle("bloodcandle_halo", Initialisation.TrapCollection.GetSpriteIdByName("bloodcandle_halo"), Initialisation.TrapCollection, new GameObject("Halo"));
            candleHalo.SetActive(true);
            tk2dSprite sprite = candleHalo.GetComponent<tk2dSprite>();
            sprite.HeightOffGround = 1;
            sprite.IsPerpendicular = false;
            sprite.renderer.material.shader = ShaderCache.Acquire("tk2d/BlendVertexColorTilted");
            sprite.usesOverrideMaterial = true;
            SpritePulser pulser = candleHalo.AddComponent<SpritePulser>();
            pulser.duration = 0.7f;
            pulser.maxDuration = 2.9f;
            pulser.maxScale = 1.1f;
            pulser.metaDuration = 6f;
            pulser.minAlpha = 0.6f;
            pulser.minDuration = 0.8f;
            pulser.minScale = 0.9f;
            pulser.m_active = true;

            MinorBreakable indivBloodCandle = Breakables.GenerateMinorBreakable("Blood Candle", Initialisation.TrapCollection, Initialisation.trapAnimationCollection,
                "bloodcandle_001",
                "bloodcandle_idle",
                null,
                "Play_OBJ_candle_fall_01",
                4, 4,
                1, 1,
                null,
                null);
            indivBloodCandle.gameObject.MakeFakePrefab();

            indivBloodCandle.specRigidbody.PixelColliders = new List<PixelCollider>()
            {
                new PixelCollider()
                {
                   ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
                   CollisionLayer = CollisionLayer.LowObstacle,
                   ManualWidth = 4,
                   ManualHeight = 4,
                   ManualOffsetX = 1,
                   ManualOffsetY = 1,
                }
            };

            indivBloodCandle.stopsBullets = false;
            indivBloodCandle.OnlyPlayerProjectilesCanBreak = false;
            indivBloodCandle.OnlyBreaksOnScreen = false;
            indivBloodCandle.resistsExplosions = true;
            indivBloodCandle.canSpawnFairy = false;
            indivBloodCandle.chanceToRain = 0;
            indivBloodCandle.dropCoins = false;
            indivBloodCandle.goopsOnBreak = false;
            indivBloodCandle.gameObject.layer = 22;
            indivBloodCandle.sprite.HeightOffGround = 0f;
            indivBloodCandle.shardClusters = new List<ShardCluster>()
            {
                BreakableAPIToolbox.GenerateShardCluster(Breakables.GenerateDebrisObjects(Initialisation.TrapCollection, new List<string>(){ "bloodcandle_debris_001" }.ToArray(),
                true, //CanRotate
                1, 1, //Lifespan Variables
                1080, //Angular Velocity
                0, //Velocity Variance
                null, //Shadow
                1f, //Mass
                null, //Audio Event
                null, //Bounce VFX
                1 //Bounces
                ), 1f, 1f, 1, 1, 1f),
                 BreakableAPIToolbox.GenerateShardCluster(Breakables.GenerateDebrisObjects(Initialisation.TrapCollection, new List<string>(){ "bloodcandle_debris_002" }.ToArray(),
                true, //CanRotate
                1, 1, //Lifespan Variables
                1080, //Angular Velocity
                0, //Velocity Variance
                null, //Shadow
                1f, //Mass
                null, //Audio Event
                null, //Bounce VFX
                1 //Bounces
                ), 1f, 1f, 1, 1, 1f),
                BreakableAPIToolbox.GenerateShardCluster(Breakables.GenerateDebrisObjects(Initialisation.TrapCollection, new List<string>(){ "bloodcandle_debrissmall_001", "bloodcandle_debrissmall_002", "bloodcandle_debrissmall_003" }.ToArray(),
                true, //CanRotate
                1, 1, //Lifespan Variables
                1080, //Angular Velocity
                40, //Velocity Variance
                null, //Shadow
                0.8f, //Mass
                null, //Audio Event
                null, //Bounce VFX
                1 //Bounces
                ), 0.5f, 2f, 1, 2, 1f)
            }.ToArray();
            indivBloodCandle.breakStyle = MinorBreakable.BreakStyle.CONE;
            indivBloodCandle.IsDecorativeOnly = false;

            PositionRandomiser randomplacement = indivBloodCandle.gameObject.AddComponent<PositionRandomiser>();
            randomplacement.xOffsetMax = 14f;
            randomplacement.yOffsetMax = 14f;

            candleHalo.transform.SetParent(indivBloodCandle.gameObject.transform);
            candleHalo.transform.localPosition = new Vector3(3f / 16f, 11f / 16f);

            var shadowobj = ItemBuilder.SpriteFromBundle("bloodcandle_shadow", Initialisation.TrapCollection.GetSpriteIdByName("bloodcandle_shadow"), Initialisation.TrapCollection, new GameObject("Shadow"));
            shadowobj.transform.SetParent(indivBloodCandle.gameObject.transform);
            shadowobj.transform.localPosition = new Vector3(-1f / 16f, -1f / 16f);
            tk2dSprite shadow = shadowobj.GetComponent<tk2dSprite>();
            shadow.HeightOffGround = -5f;
            shadow.SortingOrder = 0;
            shadow.IsPerpendicular = false;
            shadow.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTilted");
            shadow.usesOverrideMaterial = true;


            var flame = ItemBuilder.SpriteFromBundle("bloodcandle_flame_001", Initialisation.TrapCollection.GetSpriteIdByName("bloodcandle_flame_001"), Initialisation.TrapCollection, new GameObject("BloodCandleFlame"));
            tk2dSprite flameSprite = flame.GetComponent<tk2dSprite>();
            flameSprite.HeightOffGround = 1f;
            flame.transform.SetParent(indivBloodCandle.gameObject.transform);
            flame.transform.localPosition = new Vector3(-2f / 16f, 8f / 16f);
            tk2dSpriteAnimator flameAnimator = flame.GetOrAddComponent<tk2dSpriteAnimator>();
            flameAnimator.Library = Initialisation.trapAnimationCollection;
            flameAnimator.defaultClipId = Initialisation.trapAnimationCollection.GetClipIdByName("bloodcandle_flame");
            flameAnimator.DefaultClipId = Initialisation.trapAnimationCollection.GetClipIdByName("bloodcandle_flame");
            flameAnimator.playAutomatically = true;
            Material mat = new Material(EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material);
            mat.mainTexture = flameSprite.renderer.material.mainTexture;
            mat.SetColor("_EmissiveColor", new Color(255, 0, 0));
            mat.SetFloat("_EmissiveColorPower", 10);
            mat.SetFloat("_EmissivePower", 5);
            flameSprite.renderer.material = mat;

            BloodCandle candleController = indivBloodCandle.gameObject.AddComponent<BloodCandle>();
            candleController.bullet = EnemyDatabase.GetOrLoadByGuid("5f3abc2d561b4b9c9e72b879c6f10c7e").bulletBank.GetBullet("default").BulletObject.InstantiateAndFakeprefab();
            candleController.vfx = SharedVFX.BloodCandleVFX;

            DungeonPlaceable singleBloodCandle = BreakableAPIToolbox.GenerateDungeonPlaceable(new Dictionary<GameObject, float>() { { indivBloodCandle.gameObject, 1f } });
            StaticReferences.StoredDungeonPlaceables.Add("bloodcandle_singular", singleBloodCandle);
            Alexandria.DungeonAPI.StaticReferences.customPlaceables.Add("nn:bloodcandle_singular", singleBloodCandle);

            GameObject doubleBloodCandle = CustomCandles.GenerateCandomizer(2, 2, "BloodCandle", indivBloodCandle.gameObject, new List<Vector2>(){
                    new Vector2(2f, 6f),
                    new Vector2(10f, 6f),
                }, -1f, 1f, -5f, 6f, 0f);

            GameObject tripleBloodCandle = CustomCandles.GenerateCandomizer(3, 3, "BloodCandle", indivBloodCandle.gameObject, new List<Vector2>(){
                    new Vector2(0f, 4f),
                    new Vector2(10f, 1f),
                    new Vector2(9f, 9f),
                }, -3f, 3f, -3f, 3f, 0f);

            DungeonPlaceable BloodCandleCluster = BreakableAPIToolbox.GenerateDungeonPlaceable(new Dictionary<GameObject, float>() {
                { indivBloodCandle.gameObject, 1f },
                { doubleBloodCandle, 1f },
                { tripleBloodCandle, 1f },
            });
            StaticReferences.StoredDungeonPlaceables.Add("bloodcandle_cluster", BloodCandleCluster);
            Alexandria.DungeonAPI.StaticReferences.customPlaceables.Add("nn:bloodcandle_cluster", BloodCandleCluster);

        }

        private RoomHandler currentRoom;
        public GameObject bullet;
        public GameObject vfx;
        private void Start()
        {
            breakAfter = UnityEngine.Random.Range(5f, 35f);
            currentRoom = base.transform.position.GetAbsoluteRoom();
        }

        float breakAfter = 0f;
        bool spent = false;
        private void Update()
        {
            if (currentRoom != null && GameManager.Instance.BestActivePlayer != null && GameManager.Instance.BestActivePlayer.CurrentRoom == currentRoom)
            {
                if (breakAfter > 0) { breakAfter -= BraveTime.DeltaTime; }
                else if (!spent)
                {
                    PlayerController closestPlayer = GameManager.Instance.GetActivePlayerClosestToPoint(base.sprite.WorldTopCenter, false);
                    Vector2 shootVex = base.sprite.WorldTopCenter.CalculateVectorBetween(closestPlayer.specRigidbody.UnitCenter);
                    FireProjectile(base.sprite.WorldTopCenter, shootVex);

                    Transform flame = base.transform.Find("BloodCandleFlame");
                    if (flame && flame.gameObject) { UnityEngine.Object.Destroy(flame.gameObject); }

                    Transform halo = base.transform.Find("Halo");
                    if (halo && halo.gameObject) { UnityEngine.Object.Destroy(halo.gameObject); }
                    spent = true;
                }
            }
        }
        private void FireProjectile(Vector3 spawnPosition, Vector2 direction)
        {
            if (vfx != null) SpawnManager.SpawnVFX(vfx, base.sprite.WorldTopCenter, Quaternion.identity);
            AkSoundEngine.PostEvent("Play_WPN_burninghand_shot_01", base.gameObject);
            float num = Mathf.Atan2(direction.y, direction.x) * 57.29578f;
            GameObject gameObject = SpawnManager.SpawnProjectile(bullet, spawnPosition, Quaternion.Euler(0f, 0f, num), true);

            SpeculativeRigidbody spawnedProjBody = gameObject.GetComponent<SpeculativeRigidbody>();
            if (spawnedProjBody) { spawnedProjBody.RegisterGhostCollisionException(base.specRigidbody); }

            Projectile component = gameObject.GetComponent<Projectile>();
            component.Shooter = base.specRigidbody;
            component.OwnerName = StringTableManager.GetEnemiesString("#TRAP", -1);

        }
    }
}
