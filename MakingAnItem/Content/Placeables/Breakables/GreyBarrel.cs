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
    public class GreyBarrel : BraveBehaviour
    {
        public static void Init()
        {
            List<ShardCluster> ShardClusters = Breakables.GenerateBarrelStyleShardClusters(
                new List<string>() //Big Chunks
                {
                    "greybarrel_debris_001",
                    "greybarrel_debris_002",
                    "greybarrel_debris_003",
                    "greybarrel_debris_004",
                },
                new List<string>() //Medium Sized Metal Pieces
                {
                    "greybarrel_metaldebris_001",
                    "greybarrel_metaldebris_002",
                    "greybarrel_metaldebris_003",
                },
                new List<string>() //Small Metal Pieces
                {
                    "greybarrel_metalshardsmall_001",
                },
                new List<string>() //Large Wooden Pieces
                {
                    "greybarrel_woodshardlarge_001",
                    "greybarrel_woodshardlarge_002",
                    "greybarrel_woodshardlarge_003",
                },
                new List<string>() //Medium Wooden Pieces
                {
                    "greybarrel_woodshardmed_001",
                    "greybarrel_woodshardmed_002",
                    "greybarrel_woodshardmed_003",
                },
                new List<string>() //Small Wooden Pieces
                {
                    "greybarrel_woodshardsmall_001",
                    "greybarrel_woodshardsmall_002",
                }
                );


            MinorBreakable breakable = Breakables.GenerateMinorBreakable("Grey_Barrel", Initialisation.EnvironmentCollection, Initialisation.environmentAnimationCollection,
                "greybarrel_idle_001",
                "greybarrel_idle",
                "greybarrel_break",
                "Play_OBJ_barrel_break_01",
                14, 14,
                2, 0,
                null,
                null);

            breakable.gameObject.MakeFakePrefab();

            var shadowobj = ItemBuilder.SpriteFromBundle("genericbarrel_shadow_001", Initialisation.EnvironmentCollection.GetSpriteIdByName("genericbarrel_shadow_001"), Initialisation.EnvironmentCollection, new GameObject("Shadow"));
            shadowobj.transform.SetParent(breakable.transform);
            shadowobj.transform.localPosition = new Vector3(0f, -1f / 16f);
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
            breakable.sprite.HeightOffGround = -1f;
            breakable.shardClusters = ShardClusters.ToArray();
            breakable.breakStyle = MinorBreakable.BreakStyle.CONE;
            breakable.IsDecorativeOnly = false;

            GameObject bulletKinShot = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5").bulletBank.GetBullet("default").BulletObject.InstantiateAndFakeprefab();
            GreyBarrel trapComp = breakable.gameObject.AddComponent<GreyBarrel>();
            trapComp.trapShot = bulletKinShot;

            DungeonPlaceable Placeable = BreakableAPIToolbox.GenerateDungeonPlaceable(new Dictionary<GameObject, float>() { { breakable.gameObject, 1f } });
            Placeable.isPassable = true;
            Placeable.width = 1;
            Placeable.height = 1;
            Placeable.variantTiers[0].unitOffset = new Vector2(-1f / 16f, 0f);
            Alexandria.DungeonAPI.StaticReferences.StoredDungeonPlaceables.Add("grey_barrel", Placeable);
            Alexandria.DungeonAPI.StaticReferences.customPlaceables.Add("nn:grey_barrel", Placeable);
        }
        private void Start()
        {
            base.minorBreakable.OnBreak += OnBreak;
            room = base.transform.position.GetAbsoluteRoom();
            SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.red);
        }
        public GameObject trapShot;
        private RoomHandler room;
        private void OnBreak()
        {
            PlayerController closestPlayer = GameManager.Instance.GetActivePlayerClosestToPoint(base.sprite.WorldCenter, false);
            if (closestPlayer && closestPlayer.specRigidbody && room != null)
            {
                if (closestPlayer.CurrentRoom == room)
                {
                    Vector2 shootVex = base.sprite.WorldCenter.CalculateVectorBetween(closestPlayer.specRigidbody.UnitCenter);
                    this.ShootProjectileInDirection(base.sprite.WorldCenter, shootVex);
                }
            }
        }
        private void ShootProjectileInDirection(Vector3 spawnPosition, Vector2 direction)
        {
            AkSoundEngine.PostEvent("Play_TRP_bullet_shot_01", base.gameObject);
            float num = Mathf.Atan2(direction.y, direction.x) * 57.29578f;
            GameObject gameObject = SpawnManager.SpawnProjectile(trapShot, spawnPosition, Quaternion.Euler(0f, 0f, num), true);

            SpeculativeRigidbody spawnedProjBody = gameObject.GetComponent<SpeculativeRigidbody>();
            if (spawnedProjBody) { spawnedProjBody.RegisterGhostCollisionException(base.specRigidbody); }
            Projectile component = gameObject.GetComponent<Projectile>();
            component.Shooter = base.specRigidbody;
            component.OwnerName = StringTableManager.GetEnemiesString("A Barrel", -1);
        }
    }
}
