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
using System.Collections;
using Pathfinding;

namespace NevernamedsItems
{
    class FiringMechanism : PlayerItem
    {
        public static void Init()
        {
            PlayerItem item = ItemSetup.NewItem<FiringMechanism>(
              "Firing Mechanism",
              "Spin Stabilized",
              "The infernal chambers of this strange device violate the laws of conservation of energy; one bullet goes in, three come out." + "\n\nThe Revolvenants of the fifth chamber and beyond use devices such as these in their dark ammomantic rituals.",
              "firingmechanism_icon") as PlayerItem;
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 325);
            FiringMechanismController.Init();
            item.consumable = false;
            item.quality = ItemQuality.B;
        }
        public override void DoEffect(PlayerController user)
        {
            Vector2 position = user.PositionInDistanceFromAimDir(1);
            GameObject tackShooter = UnityEngine.Object.Instantiate<GameObject>(FiringMechanismController.MechanismPrefab, position, Quaternion.identity);
            tackShooter.GetComponent<tk2dSprite>().PlaceAtLocalPositionByAnchor(position, tk2dBaseSprite.Anchor.MiddleCenter);
            tackShooter.GetComponent<tk2dSprite>().UpdateZDepth();
        }
    }
    public class FiringMechanismController : BraveBehaviour
    {
        public static void Init()
        {
            MechanismPrefab = ItemBuilder.SpriteFromBundle("firingmechanism_idle_001", Initialisation.EnvironmentCollection.GetSpriteIdByName("firingmechanism_idle_001"), Initialisation.EnvironmentCollection, new GameObject("Firing Mechanism"));
            MechanismPrefab.MakeFakePrefab();
            var GuillotineBody = MechanismPrefab.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(new IntVector2(0, 0), new IntVector2(23, 23));
            GuillotineBody.CollideWithTileMap = false;
            GuillotineBody.CollideWithOthers = true;
            MechanismPrefab.GetComponent<tk2dSprite>().GetComponent<tk2dSprite>().HeightOffGround = -1f;
            MechanismPrefab.GetComponent<tk2dSprite>().renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTilted");
            MechanismPrefab.GetComponent<tk2dSprite>().usesOverrideMaterial = true;

            GuillotineBody.PixelColliders = new List<PixelCollider>()
            {
                new PixelCollider()
                {
                   ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
                   CollisionLayer = CollisionLayer.BulletBlocker,
                   ManualWidth = 19,
                   ManualHeight = 15,
                   ManualOffsetX = 2,
                   ManualOffsetY = 15,
                }
            };

            var shadowobj = ItemBuilder.SpriteFromBundle("firingmechanism_shadow", Initialisation.EnvironmentCollection.GetSpriteIdByName("firingmechanism_shadow"), Initialisation.EnvironmentCollection, new GameObject("Shadow"));
            shadowobj.transform.SetParent(MechanismPrefab.transform);
            shadowobj.transform.localPosition = new Vector3(-1f / 16f, -4f / 16f, 50f);
            tk2dSprite shadow = shadowobj.GetComponent<tk2dSprite>();
            shadow.HeightOffGround = -2f;
            shadow.SortingOrder = 0;
            shadow.IsPerpendicular = false;
            shadow.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTilted");
            shadow.usesOverrideMaterial = true;

            tk2dSpriteAnimator animator = MechanismPrefab.GetOrAddComponent<tk2dSpriteAnimator>();
            animator.Library = Initialisation.environmentAnimationCollection;
            animator.defaultClipId = Initialisation.environmentAnimationCollection.GetClipIdByName("firingmechanism_idle");
            animator.DefaultClipId = Initialisation.environmentAnimationCollection.GetClipIdByName("firingmechanism_idle");
            animator.playAutomatically = true;

            FiringMechanismController cont = MechanismPrefab.AddComponent<FiringMechanismController>();
            cont.idleAnimation = "firingmechanism_idle";
            cont.spawnAnimation = "firingmechanism_spawn";
            cont.spinAnimation = "firingmechanism_spin";
            cont.PlayerOnly = true;
            cont.targetsEnemies = true;

            MechanismPrefab.SetLayerRecursively(LayerMask.NameToLayer("FG_Critical"));

            DischargeVFX = VFXToolbox.CreateVFXBundle("FiringMechanismDischarge", true, 3f);
            // DischargeVFX.SetLayerRecursively(LayerMask.NameToLayer("FG_Critical"));
        }
        public static GameObject DischargeVFX;
        public static GameObject MechanismPrefab;
        public void Start()
        {
            currentRoom = base.transform.position.GetAbsoluteRoom();
            base.specRigidbody.OnPreRigidbodyCollision += OnCollision;
            if (!string.IsNullOrEmpty(spawnAnimation)) { base.spriteAnimator.PlayUntilFinished(spawnAnimation, idleAnimation); }
        }
        public RoomHandler currentRoom;
        public bool PlayerOnly = false;
        public bool targetsEnemies = false;
        public float initialDelay = 1;
        public float lifetime = 25f;
        private float timeActive = 0;
        private float timeSinceLastBullet = 0;
        public List<float> angles = new List<float>()
        {
            25f,-25f
        };
        public string spawnAnimation = null;
        public string idleAnimation = null;
        public string spinAnimation = null;

        private bool isSpinning = false;
        public void Update()
        {
            timeActive += BraveTime.DeltaTime;
            timeSinceLastBullet += BraveTime.DeltaTime;
            if (timeSinceLastBullet > 1f && isSpinning)
            {
                base.spriteAnimator.Play(idleAnimation);
                isSpinning = false;
                GameObject ov = SpawnManager.SpawnVFX(DischargeVFX, base.transform.position + new Vector3(-2f / 16f, 20f / 16f), Quaternion.identity, true);
                tk2dBaseSprite component = ov.GetComponent<tk2dBaseSprite>();
                component.HeightOffGround = 4f;
                component.UpdateZDepth();
                // ov.transform.parent = base.transform;
                //base.sprite.AttachRenderer(component);
            }
            if (lifetime > 0f && timeActive > lifetime)
            {
                AkSoundEngine.PostEvent("Play_OBJ_boulder_break_01", base.gameObject);
                SpawnManager.SpawnVFX((PickupObjectDatabase.GetById(37) as Gun).DefaultModule.chargeProjectiles[0].Projectile.hitEffects.overrideMidairDeathVFX, base.sprite.WorldCenter, Quaternion.identity);
                UnityEngine.Object.Destroy(base.gameObject);
            }
        }
        private bool ProjectileShouldBeReflected(Projectile proj)
        {
            if (proj == null) { return false; }
            if ((proj.Owner == null || proj.Owner is AIActor) && PlayerOnly) { return false; }
            if (proj.gameObject.GetComponent<FiringMechanismCloned>()) { return false; }
            return true;
        }
        private void OnCollision(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
        {
            if (otherRigidbody != null && otherRigidbody.projectile != null && ProjectileShouldBeReflected(otherRigidbody.projectile))
            {
                timeSinceLastBullet = 0f;
                if (!string.IsNullOrEmpty(spinAnimation) && !isSpinning) { base.spriteAnimator.Play(spinAnimation); isSpinning = true; }
                otherRigidbody.gameObject.AddComponent<FiringMechanismCloned>();
                AkSoundEngine.PostEvent("Play_WPN_burninghand_shot_01", base.gameObject);
                UnityEngine.Object.Instantiate<GameObject>(SharedVFX.RedFireBlastVFX, otherRigidbody.projectile.SafeCenter, Quaternion.identity);
                if (targetsEnemies && otherRigidbody.projectile.SafeCenter.GetNearestEnemyToPosition() != null)
                {
                    otherRigidbody.projectile.SendInDirection(otherRigidbody.projectile.GetVectorToNearestEnemy(), false);
                }
                foreach (float ang in angles)
                {
                    Projectile pro = otherRigidbody.projectile.InstantiateAndFireInDirection(otherRigidbody.projectile.SafeCenter, otherRigidbody.projectile.Direction.ToAngle() + ang, 5f).GetComponent<Projectile>();
                    pro.Owner = otherRigidbody.projectile.Owner;
                    pro.Shooter = otherRigidbody.projectile.Shooter;
                }
            }
            PhysicsEngine.SkipCollision = true;
        }
        public class FiringMechanismCloned : MonoBehaviour { }
    }
}