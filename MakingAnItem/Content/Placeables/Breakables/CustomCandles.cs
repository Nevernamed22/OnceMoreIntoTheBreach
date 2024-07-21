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
    public class CustomCandles
    {
        public static void Init()
        {
            GameObject candleHalo = ItemBuilder.SpriteFromBundle("candle_halo_003", Initialisation.EnvironmentCollection.GetSpriteIdByName("candle_halo_003"), Initialisation.EnvironmentCollection, new GameObject("Halo"));
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

            GameObject candleHaloBlue = ItemBuilder.SpriteFromBundle("candle_haloblue_003", Initialisation.EnvironmentCollection.GetSpriteIdByName("candle_haloblue_003"), Initialisation.EnvironmentCollection, new GameObject("Halo"));
            candleHaloBlue.SetActive(true);
            tk2dSprite spriteHaloBlue = candleHaloBlue.GetComponent<tk2dSprite>();
            spriteHaloBlue.HeightOffGround = 1;
            spriteHaloBlue.IsPerpendicular = false;
            spriteHaloBlue.renderer.material.shader = ShaderCache.Acquire("tk2d/BlendVertexColorTilted");
            spriteHaloBlue.usesOverrideMaterial = true;
            SpritePulser pulserHaloBlue = candleHaloBlue.AddComponent<SpritePulser>();
            pulserHaloBlue.duration = 0.7f;
            pulserHaloBlue.maxDuration = 2.9f;
            pulserHaloBlue.maxScale = 1.1f;
            pulserHaloBlue.metaDuration = 6f;
            pulserHaloBlue.minAlpha = 0.6f;
            pulserHaloBlue.minDuration = 0.8f;
            pulserHaloBlue.minScale = 0.9f;
            pulserHaloBlue.m_active = true;


            #region Lampstand
            MinorBreakable breakable = Breakables.GenerateMinorBreakable("Lampstand", Initialisation.EnvironmentCollection, Initialisation.environmentAnimationCollection,
                "lampstand_idle_001",
                "lampstand_idle",
                null,
                "Play_WPN_metalbullet_impact_01",
                3, 24,
                1, 1,
                null,
                null);

            breakable.gameObject.MakeFakePrefab();

            var shadowobj = ItemBuilder.SpriteFromBundle("lampstand_shadow", Initialisation.EnvironmentCollection.GetSpriteIdByName("lampstand_shadow"), Initialisation.EnvironmentCollection, new GameObject("Shadow"));
            shadowobj.transform.SetParent(breakable.transform);
            shadowobj.transform.localPosition = new Vector3(-1f / 16f, -1f / 16f);
            tk2dSprite shadow = shadowobj.GetComponent<tk2dSprite>();
            shadow.HeightOffGround = -5f;
            shadow.SortingOrder = 0;
            shadow.IsPerpendicular = false;
            shadow.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTilted");
            shadow.usesOverrideMaterial = true;

            breakable.stopsBullets = true;
            breakable.OnlyPlayerProjectilesCanBreak = false;
            breakable.OnlyBreaksOnScreen = false;
            breakable.resistsExplosions = false;
            breakable.canSpawnFairy = false;
            breakable.chanceToRain = 0;
            breakable.dropCoins = false;
            breakable.goopsOnBreak = false;
            breakable.gameObject.layer = 22;
            breakable.sprite.HeightOffGround = 0f;
            breakable.shardClusters = new List<ShardCluster>()
            {
                BreakableAPIToolbox.GenerateShardCluster(Breakables.GenerateDebrisObjects(Initialisation.EnvironmentCollection, new List<string>(){ "lampstand_debris_base_001","lampstand_debris_base_002" }.ToArray(),
                true, //CanRotate
                1, 1, //Lifespan Variables
                1080, //Angular Velocity
                0, //Velocity Variance
                null, //Shadow
                0.5f, //Mass
                null, //Audio Event
                null, //Bounce VFX
                1 //Bounces
                ), 0.1f, 0.5f, 1, 2, 0.1f),
                BreakableAPIToolbox.GenerateShardCluster(Breakables.GenerateDebrisObjects(Initialisation.EnvironmentCollection, new List<string>(){ "lampstand_debris_pole_001","lampstand_debris_pole_002", "lampstand_debris_pole_003" }.ToArray(),
                true, //CanRotate
                1, 1, //Lifespan Variables
                1080, //Angular Velocity
                0, //Velocity Variance
                null, //Shadow
                1f, //Mass
                null, //Audio Event
                null, //Bounce VFX
                1 //Bounces
                ), 1f, 1f, 2, 3, 1f),
                BreakableAPIToolbox.GenerateShardCluster(Breakables.GenerateDebrisObjects(Initialisation.EnvironmentCollection, new List<string>(){ "lampstand_debris_cup" }.ToArray(),
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
                BreakableAPIToolbox.GenerateShardCluster(Breakables.GenerateDebrisObjects(Initialisation.EnvironmentCollection, new List<string>(){ "lampstand_debris_candle" }.ToArray(),
                true, //CanRotate
                1, 1, //Lifespan Variables
                1080, //Angular Velocity
                0, //Velocity Variance
                null, //Shadow
                1f, //Mass
                null, //Audio Event
                null, //Bounce VFX
                1 //Bounces
                ), 1f, 1f, 1, 1, 1f)
            }.ToArray();
            breakable.breakStyle = MinorBreakable.BreakStyle.CONE;
            breakable.IsDecorativeOnly = true;

            candleHalo.transform.SetParent(breakable.gameObject.transform);
            candleHalo.transform.localPosition = new Vector3(2f / 16f, 29f / 16f);

            breakable.specRigidbody.PixelColliders = new List<PixelCollider>()
            {
                new PixelCollider()
                {
                   ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
                   CollisionLayer = CollisionLayer.BulletBreakable,
                   ManualWidth = 3,
                   ManualHeight = 24,
                   ManualOffsetX = 1,
                   ManualOffsetY = 1,
                },
                new PixelCollider()
                {
                   ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
                   CollisionLayer = CollisionLayer.HighObstacle,
                   ManualWidth = 3,
                   ManualHeight = 3,
                   ManualOffsetX = 1,
                   ManualOffsetY = 1,
                }
            };

            DungeonPlaceable Placeable = BreakableAPIToolbox.GenerateDungeonPlaceable(new Dictionary<GameObject, float>() { { breakable.gameObject, 1f } });
            Placeable.isPassable = true;
            Placeable.width = 1;
            Placeable.height = 1;
            Placeable.variantTiers[0].unitOffset = new Vector2(6f / 16f, 6f / 16f);
            StaticReferences.StoredDungeonPlaceables.Add("lampstand", Placeable);
            Alexandria.DungeonAPI.StaticReferences.customPlaceables.Add("nn:lampstand", Placeable);
            #endregion

            #region BlueCandles
            MinorBreakable indivBlueCandle = Breakables.GenerateMinorBreakable("Blue Candle", Initialisation.EnvironmentCollection, Initialisation.environmentAnimationCollection,
                "bluecandle_idle_001",
                "bluecandle_idle",
                null,
                "Play_OBJ_candle_fall_01",
                3, 3,
                0, 1,
                null,
                null);
            indivBlueCandle.gameObject.MakeFakePrefab();

            indivBlueCandle.stopsBullets = false;
            indivBlueCandle.OnlyPlayerProjectilesCanBreak = false;
            indivBlueCandle.OnlyBreaksOnScreen = false;
            indivBlueCandle.resistsExplosions = false;
            indivBlueCandle.canSpawnFairy = false;
            indivBlueCandle.chanceToRain = 0;
            indivBlueCandle.dropCoins = false;
            indivBlueCandle.goopsOnBreak = false;
            indivBlueCandle.gameObject.layer = 22;
            indivBlueCandle.sprite.HeightOffGround = 0f;
            indivBlueCandle.shardClusters = new List<ShardCluster>()
            {
                BreakableAPIToolbox.GenerateShardCluster(Breakables.GenerateDebrisObjects(Initialisation.EnvironmentCollection, new List<string>(){ "bluecandle_debris_001" }.ToArray(),
                true, //CanRotate
                1, 1, //Lifespan Variables
                1080, //Angular Velocity
                0, //Velocity Variance
                null, //Shadow
                1f, //Mass
                null, //Audio Event
                null, //Bounce VFX
                1 //Bounces
                ), 1f, 1f, 1, 1, 1f)
            }.ToArray();
            indivBlueCandle.breakStyle = MinorBreakable.BreakStyle.CONE;
            indivBlueCandle.IsDecorativeOnly = true;

            PositionRandomiser randomplacement = indivBlueCandle.gameObject.AddComponent<PositionRandomiser>();
            randomplacement.xOffsetMax = 14f;
            randomplacement.yOffsetMax = 14f;
            PlacedObjectRotator randomrotation = indivBlueCandle.gameObject.AddComponent<PlacedObjectRotator>();
            randomrotation.chanceToTriggerOnStart = 0.05f;

            candleHaloBlue.transform.SetParent(indivBlueCandle.gameObject.transform);
            candleHaloBlue.transform.localPosition = new Vector3(1f / 16f, 6f / 16f);


            DungeonPlaceable singleBlueCandle = BreakableAPIToolbox.GenerateDungeonPlaceable(new Dictionary<GameObject, float>() { { indivBlueCandle.gameObject, 1f } });
            StaticReferences.StoredDungeonPlaceables.Add("bluecandle_singular", singleBlueCandle);
            Alexandria.DungeonAPI.StaticReferences.customPlaceables.Add("nn:bluecandle_singular", singleBlueCandle);


            DungeonPlaceable BlueCandleCluster = BreakableAPIToolbox.GenerateDungeonPlaceable(new Dictionary<GameObject, float>() {
                { GenerateCandomizer(1, 4, "BlueCandle", indivBlueCandle.gameObject, new List<Vector2>(){
                    new Vector2(4f, 4f),
                    new Vector2(10f, 3f),
                    new Vector2(2f, 11f),
                    new Vector2(11f, 13f),
                }, -3f, 3f, -3f, 3f, 0.23f), 1f },
                { GenerateCandomizer(1, 4, "BlueCandle", indivBlueCandle.gameObject, new List<Vector2>(){
                    new Vector2(8f, 3f),
                    new Vector2(3f, 9f),
                    new Vector2(13f, 7f),
                    new Vector2(7f, 12f),
                }, -3f, 3f, -3f, 3f, 0.23f), 1f }
            });
            StaticReferences.StoredDungeonPlaceables.Add("bluecandle_cluster", BlueCandleCluster);
            Alexandria.DungeonAPI.StaticReferences.customPlaceables.Add("nn:bluecandle_cluster", BlueCandleCluster);

            DungeonPlaceable BlueCandleClusterExclusive = BreakableAPIToolbox.GenerateDungeonPlaceable(new Dictionary<GameObject, float>() {
                { GenerateCandomizer(-2, 4, "BlueCandle", indivBlueCandle.gameObject, new List<Vector2>(){
                    new Vector2(4f, 4f),
                    new Vector2(10f, 3f),
                    new Vector2(2f, 11f),
                    new Vector2(11f, 13f),
                }, -3f, 3f, -3f, 3f, 0.23f), 1f },
                { GenerateCandomizer(-2, 4, "BlueCandle", indivBlueCandle.gameObject, new List<Vector2>(){
                    new Vector2(8f, 3f),
                    new Vector2(3f, 9f),
                    new Vector2(13f, 7f),
                    new Vector2(7f, 12f),
                }, -3f, 3f, -3f, 3f, 0.23f), 1f }
            });
            StaticReferences.StoredDungeonPlaceables.Add("bluecandle_cluster_exclusive", BlueCandleClusterExclusive);
            Alexandria.DungeonAPI.StaticReferences.customPlaceables.Add("nn:bluecandle_cluster_exclusive", BlueCandleClusterExclusive);

            #endregion
        }
        public static GameObject GenerateCandomizer(int minCandles, int maxCandles, string candleIdentifier, GameObject candlePrefab, List<Vector2> Positions, float xOffsetMin = 0f, float xOffsetMax = 0f, float yOffsetMin = 0f, float yOffsetMax = 0f, float chanceToKnockOneOver = 0f)
        {
            GameObject candomizer = new GameObject();
            candomizer.MakeFakePrefab();

            int iterator = 1;
            foreach (Vector2 pos in Positions)
            {
                GameObject candle1 = FakePrefab.Clone(candlePrefab);
                candle1.name = $"{candleIdentifier} {iterator}";
                candle1.transform.SetParent(candomizer.transform);
                Vector2 loc = new Vector2(pos.x / 16f, pos.y / 16f);
                candle1.transform.localPosition = loc;

                if (xOffsetMin != 0f || xOffsetMax != 0f || yOffsetMin != 0f || yOffsetMax != 0f)
                {
                    PositionRandomiser randomiser = candle1.GetOrAddComponent<PositionRandomiser>();
                    randomiser.xOffsetMin = xOffsetMin;
                    randomiser.xOffsetMax = xOffsetMax;
                    randomiser.yOffsetMin = yOffsetMin;
                    randomiser.yOffsetMax = yOffsetMax;
                }
                
                PlacedObjectRotator randomrotation = candle1.gameObject.GetOrAddComponent<PlacedObjectRotator>();
                randomrotation.chanceToTriggerOnStart = 0f;

                iterator++;
            }

            Candomizer comp = candomizer.AddComponent<Candomizer>();
            comp.candleIdentifier = candleIdentifier;
            comp.minCandles = minCandles;
            comp.maxCandles = maxCandles;
            comp.chanceForOneToBeKnockedOver = chanceToKnockOneOver;
            return candomizer;
        }
    }
    public class PlacedObjectRotator : BraveBehaviour
    {
        private void Start()
        {
            if (UnityEngine.Random.value <= chanceToTriggerOnStart) { DoRotation(); }
        }
        public void DoRotation()
        {
            float rotation = 0f;
            if (strict) { rotation = UnityEngine.Random.value <= 0.5f ? minRotation : maxnRotation; }
            else { rotation = UnityEngine.Random.Range(minRotation, maxnRotation); }

            float z = base.transform.rotation.eulerAngles.z;
            base.transform.rotation = Quaternion.Euler(0f, 0f, z + rotation);

            if (base.specRigidbody) { base.specRigidbody.Reinitialize(); }
        }
        public float minRotation = -90f;
        public float maxnRotation = 90f;
        public bool strict = true;
        public float chanceToTriggerOnStart = 1f;
    }
    public class PositionRandomiser : BraveBehaviour
    {
        private void Start()
        {
            Vector3 offset = new Vector3(UnityEngine.Random.Range(xOffsetMin/16f, xOffsetMax / 16f), UnityEngine.Random.Range(yOffsetMin / 16f, yOffsetMax / 16f));
            if (base.transform.parent != null) { base.transform.localPosition += offset; }
            else { base.transform.position += offset; }
            if (base.specRigidbody) { base.specRigidbody.Reinitialize(); }
        }
        public float xOffsetMin;
        public float xOffsetMax;
        public float yOffsetMin;
        public float yOffsetMax;

    }
    public class Candomizer : BraveBehaviour
    {
        public int minCandles = 0;
        public int maxCandles = 4;
        public string candleIdentifier;
        public float chanceForOneToBeKnockedOver;
        private void Start()
        {
            candles = new List<GameObject>();
            foreach (Transform child in base.transform)
            {
                if (child && child.gameObject && child.gameObject.name.StartsWith(candleIdentifier)) { candles.Add(child.gameObject); }
            }

            int numberOfCandles = UnityEngine.Random.Range(minCandles, maxCandles + 1);
            int numToRemove = candles.Count - numberOfCandles;
            if (numToRemove > 0)
            {
                for (int i = 0; i < numToRemove; i++)
                {
                    if (candles.Count > 0)
                    {
                        GameObject deadCandle = BraveUtility.RandomElement(candles);
                        candles.Remove(deadCandle);
                        UnityEngine.Object.Destroy(deadCandle);
                    }
                }
            }
            if ((UnityEngine.Random.value <= chanceForOneToBeKnockedOver) && candles.Count > 0)
            {
                GameObject knocker = BraveUtility.RandomElement(candles);
                if (knocker.GetComponent<PlacedObjectRotator>()) { knocker.GetComponent<PlacedObjectRotator>().DoRotation(); }
            }
        }
        private List<GameObject> candles;
    }
}
