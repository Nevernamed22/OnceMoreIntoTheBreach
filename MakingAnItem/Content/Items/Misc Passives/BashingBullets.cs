using Alexandria.ItemAPI;
using Dungeonator;
using System;
using System.Collections.Generic;
using System.Linq;
using Alexandria.Misc;
using System.Text;
using UnityEngine;
using static NevernamedsItems.GravityGun;

namespace NevernamedsItems
{
    public class BashingBullets : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<BashingBullets>(
            "Bashing Bullets",
            "Punch Out",
            "The thick leather gloves glued to the slugs of these bullets increase the kinetic force they apply to the target.",
            "bashingbullets_improved");
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.KnockbackMultiplier, 10, StatModifier.ModifyMethod.ADDITIVE);
            item.quality = PickupObject.ItemQuality.D;
            item.SetTag("bullet_modifier");
            BashingBulletsID = item.PickupObjectId;
        }
        public static int BashingBulletsID;
        public override void Pickup(PlayerController player)
        {
            player.PostProcessProjectile += PostProcessProj;
            base.Pickup(player);
        }
        public override void DisableEffect(PlayerController player)
        {
            if (player) { player.PostProcessProjectile -= PostProcessProj; }
            base.DisableEffect(player);
        }
        private void PostProcessProj(Projectile bullet, float b)
        {
            bullet.gameObject.AddComponent<BreakableBashingBehaviour>();
        }
    }
    public class BreakableBashingBehaviour : MonoBehaviour
    {
        private void Start()
        {
            self = base.GetComponent<Projectile>();
            if (self)
            {
                rigidBody = self.specRigidbody;
                owner = self.ProjectilePlayerOwner();

                if (rigidBody && owner) rigidBody.OnPreRigidbodyCollision += OnCollide;
            }
        }
        private void OnCollide(SpeculativeRigidbody selfbody, PixelCollider selfcollider, SpeculativeRigidbody otherbody, PixelCollider othercollider)
        {
            if (otherbody && otherbody.gameObject && (otherbody.gameObject.GetComponent<MinorBreakable>() || otherbody.gameObject.GetComponent<MajorBreakable>()))
            {
                GameObject hitVFX = (PickupObjectDatabase.GetById(541) as Gun).DefaultModule.chargeProjectiles[0].Projectile.hitEffects.enemy.effects[0].effects[0].effect;
                UnityEngine.Object.Instantiate<GameObject>(hitVFX, self.specRigidbody.UnitCenter, Quaternion.identity);
                Smack(otherbody.gameObject);
                PhysicsEngine.SkipCollision = true;
            }
        }

        private PlayerController owner;
        private Projectile self;
        private SpeculativeRigidbody rigidBody;
        public void Smack(GameObject thingy)
        {
            Projectile proj = null;
            IPlayerInteractable @interface = thingy.GetInterface<IPlayerInteractable>();
            if (@interface != null)
            {
                RoomHandler roomFromPosition = GameManager.Instance.Dungeon.GetRoomFromPosition(thingy.transform.position.IntXY(VectorConversions.Round));
                if (roomFromPosition.IsRegistered(@interface)) { roomFromPosition.DeregisterInteractable(@interface); }
            }

            if (thingy.GetComponent<MinorBreakable>())
            {
                thingy.GetComponent<MinorBreakable>().OnlyBrokenByCode = true;
                thingy.GetComponent<MinorBreakable>().isInvulnerableToGameActors = true;
                thingy.GetComponent<MinorBreakable>().resistsExplosions = true;

                Projectile projectile = thingy.GetOrAddComponent<Projectile>();
                projectile.Shooter = owner.specRigidbody;
                projectile.Owner = owner;
                projectile.baseData.damage = 15 * owner.stats.GetStatValue(PlayerStats.StatType.Damage);
                projectile.baseData.range = 1000f;
                projectile.baseData.speed = 20f;
                projectile.collidesWithProjectiles = false;
                projectile.shouldRotate = false;
                projectile.baseData.force = 30f;
                projectile.specRigidbody.CollideWithTileMap = true;
                projectile.specRigidbody.Reinitialize();
                projectile.specRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.Projectile;
                projectile.Start();
                projectile.projectileHitHealth = 20;
                projectile.UpdateCollisionMask();
                projectile.gameObject.AddComponent<GravityGunObjectDeathHandler>();
                proj = projectile;
            }
            else if (thingy.GetComponent<MajorBreakable>())
            {
                if (thingy.GetComponent<Chest>()) { return; }
                bool shouldBlockProjWhileHeld = false;

                thingy.GetComponent<MajorBreakable>().DamageReduction = 0.1f;
                thingy.GetComponent<MajorBreakable>().IgnoreExplosions = true;

                if (thingy.GetComponentInParent<FlippableCover>())
                {
                    MajorBreakable thingyBreakable = thingy.GetComponent<MajorBreakable>();
                    FlippableCover cover = thingy.GetComponentInParent<FlippableCover>();
                    SpeculativeRigidbody body = thingy.GetComponentInParent<SpeculativeRigidbody>();
                    cover.shadowSprite.renderer.enabled = false;
                    if (cover.IsFlipped) { shouldBlockProjWhileHeld = true; }

                    thingyBreakable.OnDamaged -= cover.Damaged;
                    thingyBreakable.OnBreak -= cover.DestroyCover;
                    for (int i = body.OnPostRigidbodyMovement.GetInvocationList().Count() - 1; i >= 0; i--)
                    {
                        Delegate Assignment = body.OnPostRigidbodyMovement.GetInvocationList()[i];
                        if (Assignment.Method.ToString().Contains("OnPostMovement"))
                        {
                            body.OnPostRigidbodyMovement = (Action<SpeculativeRigidbody, Vector2, IntVector2>)Delegate.Remove(body.OnPostRigidbodyMovement, Assignment);
                        }

                    }
                    UnityEngine.Object.Destroy(thingy.GetComponentInParent<FlippableCover>());
                }

                Projectile projectile = thingy.GetOrAddComponent<Projectile>();
                if (projectile.specRigidbody) projectile.specRigidbody.Initialize();
                projectile.Shooter = owner.specRigidbody;
                projectile.Owner = owner;
                projectile.baseData.damage = 30;

                projectile.baseData.range = 1000f;
                projectile.baseData.speed = 20f;
                if (shouldBlockProjWhileHeld) projectile.collidesWithProjectiles = true;
                else projectile.collidesWithProjectiles = false;
                projectile.pierceMinorBreakables = true;
                projectile.shouldRotate = false;
                projectile.baseData.force = 50f;
                projectile.specRigidbody.CollideWithTileMap = true;
                projectile.specRigidbody.Reinitialize();

                projectile.specRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.Projectile;
                projectile.Start();
                projectile.projectileHitHealth = 20;
                projectile.UpdateCollisionMask();
                thingy.AddComponent<GravityGunObjectDeathHandler>();

                proj = projectile;
            }
            if (proj != null)
            {
                ProjectileSpriteRotation rotator = proj.gameObject.AddComponent<ProjectileSpriteRotation>();
                proj.pierceMinorBreakables = true;
                BounceProjModifier bounce = proj.gameObject.AddComponent<BounceProjModifier>();
                bounce.numberOfBounces = 1;
                proj.SendInDirection(base.GetComponent<Projectile>().Direction, false, false);
            }
        }
    }
}

