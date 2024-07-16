using Alexandria.BreakableAPI;
using Alexandria.DungeonAPI;
using Alexandria.ItemAPI;
using Dungeonator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class BulletScarecrow : BraveBehaviour
    {
        public static void Init()
        {
            List<ShardCluster> ShardClusters = Breakables.GenerateBarrelStyleShardClusters(
                new List<string>() //Big Chunks
                {
                    "bulletscarecrow_debris_001",
                    "bulletscarecrow_debris_002",
                    "bulletscarecrow_debris_003",
                    "bulletscarecrow_debris_004",
                },
                new List<string>() //Medium Sized Metal Pieces
                {
                    "bulletscarecrow_weirddebris_001",
                    "bulletscarecrow_weirddebris_002",
                },
                new List<string>() //Small Metal Pieces
                {
                    "bulletscarecrow_smalldebris_001",
                    "bulletscarecrow_smalldebris_002",
                    "bulletscarecrow_smalldebris_003",
                    "bulletscarecrow_smalldebris_004",
                },
                new List<string>() //Large Wooden Pieces
                {
                    "bulletscarecrow_smalldebris_001",
                    "bulletscarecrow_smalldebris_002",
                    "bulletscarecrow_smalldebris_003",
                    "bulletscarecrow_smalldebris_004",
                },
                new List<string>() //Medium Wooden Pieces
                {
                    "bulletscarecrow_stuffing_001",
                    "bulletscarecrow_stuffing_002",
                },
                new List<string>() //Small Wooden Pieces
                {
                    "bulletscarecrow_stuffing_001",
                    "bulletscarecrow_stuffing_002",
                    "bulletscarecrow_stuffing_003",
                    "bulletscarecrow_stuffing_004",
                }
                );

            GameObject center = SetupScarecrow("bulletscarecrow_center", "bulletscarecrow_centerhit", ShardClusters);
            GameObject left = SetupScarecrow("bulletscarecrow_left", "bulletscarecrow_lefthit", ShardClusters);
            GameObject right = SetupScarecrow("bulletscarecrow_right", "bulletscarecrow_hitright", ShardClusters);


            DungeonPlaceable BulletScarecrowPlaceable = BreakableAPIToolbox.GenerateDungeonPlaceable(new Dictionary<GameObject, float>()
            {
                { center.gameObject, 1f },
                { left.gameObject, 1f },
                { right.gameObject, 1f },
            });
            BulletScarecrowPlaceable.isPassable = true;
            BulletScarecrowPlaceable.width = 1;
            BulletScarecrowPlaceable.height = 1;
            BulletScarecrowPlaceable.variantTiers[0].unitOffset = new Vector2(-4f / 16f, 0);
            BulletScarecrowPlaceable.variantTiers[1].unitOffset = new Vector2(-4f / 16f, 0);
            BulletScarecrowPlaceable.variantTiers[2].unitOffset = new Vector2(-4f / 16f, 0);
            StaticReferences.StoredDungeonPlaceables.Add("bullet_scarecrow", BulletScarecrowPlaceable);
            Alexandria.DungeonAPI.StaticReferences.customPlaceables.Add("nn:bullet_scarecrow", BulletScarecrowPlaceable);
        }
        private static GameObject SetupScarecrow(string idle, string wobbleanim, List<ShardCluster> clusters)
        {
            MajorBreakable breakable = Breakables.GenerateMajorBreakable("Bullet_Scarecrow", Initialisation.EnvironmentCollection, Initialisation.environmentAnimationCollection,
                "bulletscarecrow_center_001",
                idle,
                "bulletscarecrow_break",
                HP: 15,
                ColliderSizeX: 11,
                ColliderSizeY: 15,
                ColliderOffsetX: 7,
                ColliderOffsetY: 4,
                destroyedOnBreak: false,
                handlesOwnBreakAnim: true);

            var shadowobj = ItemBuilder.SpriteFromBundle("bulletscarecrow_shadow", Initialisation.EnvironmentCollection.GetSpriteIdByName("bulletscarecrow_shadow"), Initialisation.EnvironmentCollection, new GameObject("Shadow"));
            shadowobj.transform.SetParent(breakable.transform);
            shadowobj.transform.localPosition = new Vector3(4f / 16f, -1f / 16f);
            tk2dSprite shadow = shadowobj.GetComponent<tk2dSprite>();
            shadow.HeightOffGround = -5f;
            shadow.SortingOrder = 0;
            shadow.IsPerpendicular = false;
            shadow.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTilted");
            shadow.usesOverrideMaterial = true;

            BulletScarecrow scrow = breakable.gameObject.AddComponent<BulletScarecrow>();
            scrow.wobbleAnim = wobbleanim;
            breakable.gameObject.AddComponent<PlacedBlockerConfigurable>();

            breakable.InvulnerableToEnemyBullets = true;
            breakable.IgnoreExplosions = false;
            breakable.gameObject.layer = 22;
            breakable.sprite.HeightOffGround = -1;
            breakable.shardClusters = clusters.ToArray();
            breakable.gameObject.MakeFakePrefab();

            return breakable.gameObject;
        }
        public string wobbleAnim;
        private void Start()
        {
            base.majorBreakable.OnDamaged += OnDamaged;
            base.majorBreakable.OnBreak += OnBreak;
        }
        private void OnDamaged(float amount)
        {
            if (base.majorBreakable.HitPoints > 0f && !string.IsNullOrEmpty(wobbleAnim))
            {
                base.spriteAnimator.Play(wobbleAnim);
                if (UnityEngine.Random.value <= 0.001f) { TextBoxManager.ShowTextBox(base.sprite.WorldTopCenter, base.transform, 1f, BraveUtility.RandomElement(dialogue), "gambler", false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false); }   
            }
        }
        public static List<string> dialogue = new List<string>()
        {
            "Ouch.",
            "Stop that.",
        };
        private void OnBreak()
        {
            AkSoundEngine.PostEvent("Play_CHR_general_death_01", base.gameObject);
        }
    }
}
