using Alexandria.BreakableAPI;
using Alexandria.DungeonAPI;
using Alexandria.ItemAPI;
using Dungeonator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.Misc;

namespace NevernamedsItems
{
    class StatueTraps
    {
        public static void Init()
        {
            BulletKinStatueTrap = SpriteBuilder.SpriteFromResource("NevernamedsItems/Resources/PlaceableObjects/kinstatuetrap_bullet.png", new GameObject("kinstatuetrap_bullet"));
            BulletKinStatueTrap.MakeFakePrefab();
            var BulletKinStatueTrapBody = BulletKinStatueTrap.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(new IntVector2(0, -2), new IntVector2(16, 18));
            BulletKinStatueTrapBody.CollideWithTileMap = false;
            BulletKinStatueTrapBody.CollideWithOthers = true;
            BulletKinStatueTrapBody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.HighObstacle;
            BulletKinStatueTrap.GetComponent<tk2dSprite>().GetComponent<tk2dSprite>().HeightOffGround = 0.1f;

            Transform shootPoint = new GameObject("shootPoint").transform;
            shootPoint.SetParent(BulletKinStatueTrap.transform);
            shootPoint.localPosition = new Vector3(0.5f, 1.25f);


            var shadowobj = SpriteBuilder.SpriteFromResource("NevernamedsItems/Resources/PlaceableObjects/kinstatuetrap_shadow.png", new GameObject("kinstatuetrap_shadow"));
            shadowobj.transform.SetParent(BulletKinStatueTrap.transform);
            shadowobj.transform.localPosition = new Vector3(-0.0625f, -0.125f, 50f);
            tk2dSprite shadow = shadowobj.GetComponent<tk2dSprite>();
            shadow.HeightOffGround = 0f;
            shadow.SortingOrder = 0;
            shadow.IsPerpendicular = false;
            shadow.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTilted");
            shadow.usesOverrideMaterial = true;


            GameObject bulletKinShot = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5").bulletBank.GetBullet("default").BulletObject.InstantiateAndFakeprefab();
            KinStatueTrap trapComp = BulletKinStatueTrap.gameObject.AddComponent<KinStatueTrap>();
            trapComp.trapShot = bulletKinShot;
            trapComp.vfxOffset = new Vector2(-0.375f, 0.3125f);
            trapComp.vfx = VFXToolbox.CreateVFXBundle("KinStatueTrapBulletActivate", new IntVector2(27, 34), tk2dBaseSprite.Anchor.LowerLeft, true, 10f);
            trapComp.vfx.GetComponent<tk2dSprite>().HeightOffGround = 10f;
            trapComp.triggerTimerDelay = 2f;
            trapComp.triggerTimerOffset = 3f;


            DungeonPlaceable megastatuebrokenplaceable = BreakableAPIToolbox.GenerateDungeonPlaceable(new Dictionary<GameObject, float>() { { BulletKinStatueTrap.gameObject, 1f } });
            megastatuebrokenplaceable.isPassable = false;
            megastatuebrokenplaceable.width = 1;
            megastatuebrokenplaceable.height = 1;
            StaticReferences.StoredDungeonPlaceables.Add("kinstatuetrap_bullet", megastatuebrokenplaceable);
            Alexandria.DungeonAPI.StaticReferences.customPlaceables.Add("nn:kinstatuetrap_bullet", megastatuebrokenplaceable);
        }
        public static GameObject BulletKinStatueTrap;
    }
    public class KinStatueTrap : BasicTrapController
    {
        public KinStatueTrap()
        {
            triggerMethod = TriggerMethod.Timer;
        }
        public GameObject shootPoint;
        public GameObject trapShot;
        public GameObject vfx;
        public Vector3 vfxOffset;

        public override void Start()
        {
            shootPoint = base.gameObject.transform.Find("shootPoint").gameObject;
            base.Start();
        }
        public override void TriggerTrap(SpeculativeRigidbody target)
        {
            base.TriggerTrap(target);
            PlayerController closestPlayer = GameManager.Instance.GetActivePlayerClosestToPoint(shootPoint.transform.position.XY(), false);
            if (closestPlayer && closestPlayer.specRigidbody)
            {
                if (closestPlayer.CurrentRoom == base.m_parentRoom)
                {
                    Vector2 shootVex = shootPoint.transform.position.CalculateVectorBetween(closestPlayer.specRigidbody.UnitCenter);
                    this.ShootProjectileInDirection(this.shootPoint.transform.position, shootVex);
                }
            }
        }
        private void ShootProjectileInDirection(Vector3 spawnPosition, Vector2 direction)
        {
            if (vfx != null) SpawnManager.SpawnVFX(vfx, base.gameObject.transform.position + vfxOffset, Quaternion.identity);
            AkSoundEngine.PostEvent("Play_TRP_bullet_shot_01", base.gameObject);
            float num = Mathf.Atan2(direction.y, direction.x) * 57.29578f;
            GameObject gameObject = SpawnManager.SpawnProjectile(trapShot, spawnPosition, Quaternion.Euler(0f, 0f, num), true);

            SpeculativeRigidbody spawnedProjBody = gameObject.GetComponent<SpeculativeRigidbody>();
            if (spawnedProjBody) { spawnedProjBody.RegisterGhostCollisionException(base.specRigidbody); }
            Projectile component = gameObject.GetComponent<Projectile>();
            component.Shooter = base.specRigidbody;
            component.OwnerName = StringTableManager.GetEnemiesString("#TRAP", -1);
        }
    }
}
