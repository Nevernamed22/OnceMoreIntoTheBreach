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
            tk2dSprite spriteHaloBlue = candleHaloBlue.GetComponent<tk2dSprite>();
            spriteHaloBlue.HeightOffGround = 0;
            spriteHaloBlue.IsPerpendicular = false;
            spriteHaloBlue.renderer.material.shader = ShaderCache.Acquire("tk2d/BlendVertexColorTilted");
            SpritePulser pulserHaloBlue = candleHaloBlue.AddComponent<SpritePulser>();
            pulserHaloBlue.duration = 0.7f;
            pulserHaloBlue.maxDuration = 2.9f;
            pulserHaloBlue.maxScale = 1.1f;
            pulserHaloBlue.metaDuration = 6f;
            pulserHaloBlue.minAlpha = 0.6f;
            pulserHaloBlue.minDuration = 0.8f;
            pulserHaloBlue.minScale = 0.9f;



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
            breakable.IsDecorativeOnly = false;

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
        }
    }
}
