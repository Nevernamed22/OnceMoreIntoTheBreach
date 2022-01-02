﻿using Dungeonator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class SlashDoer
    {
        public static void DoSwordSlash(Vector2 position, float angle, PlayerController owner, float playerKnockbackForce, ProjInteractMode intmode, float damageToDeal, float enemyKnockbackForce, List<GameActorEffect> statusEffects, Transform parentTransform = null, float jammedDamageMult = 1, float bossDamageMult = 1)
        {
            (PickupObjectDatabase.GetById(417) as Gun).muzzleFlashEffects.SpawnAtPosition(position, angle, parentTransform, null, null, -0.05f);
            GameManager.Instance.StartCoroutine(HandleSlash(position, angle, owner, playerKnockbackForce, intmode, damageToDeal, enemyKnockbackForce, statusEffects, jammedDamageMult, bossDamageMult));
        }
        private static IEnumerator HandleSlash(Vector2 position, float angle, PlayerController owner, float knockbackForce, ProjInteractMode intmode, float damageToDeal, float enemyKnockback, List<GameActorEffect> statusEffects, float jammedDMGMult, float bossDMGMult)
        {
            int slashId = Time.frameCount;
            List<SpeculativeRigidbody> alreadyHit = new List<SpeculativeRigidbody>();
            if (knockbackForce != 0f && owner != null) owner.knockbackDoer.ApplyKnockback(BraveMathCollege.DegreesToVector(angle, 1f), knockbackForce, 0.25f, false);
            float ela = 0f;
            while (ela < 0.2f)
            {
                ela += BraveTime.DeltaTime;
                HandleHeroSwordSlash(alreadyHit, position, angle, slashId, owner, intmode, damageToDeal, enemyKnockback, statusEffects, jammedDMGMult, bossDMGMult);
                yield return null;
            }
            yield break;
        }
        public enum ProjInteractMode
        {
            IGNORE,
            DESTROY,
            REFLECT
        }
        private static bool ProjectileIsValid(Projectile proj)
        {
            if (proj && (!(proj.Owner is PlayerController) || proj.ForcePlayerBlankable)) return true;
            else return false;
        }
        private static bool ObjectWasHitBySlash(Vector2 ObjectPosition, Vector2 SlashPosition, float slashAngle, float SlashRange, float SlashDimensions)
        {
            if (Vector2.Distance(ObjectPosition, SlashPosition) < SlashRange)
            {
                float num7 = BraveMathCollege.Atan2Degrees(ObjectPosition - SlashPosition);
                float minRawAngle = Math.Min(SlashDimensions, -SlashDimensions);
                float maxRawAngle = Math.Max(SlashDimensions, -SlashDimensions);
                bool isInRange = false;
                float actualMaxAngle = slashAngle + maxRawAngle;
                float actualMinAngle = slashAngle + minRawAngle;

                if (num7.IsBetweenRange(actualMinAngle, actualMaxAngle)) isInRange = true;
                if (actualMaxAngle > 180)
                {
                    float Overflow = actualMaxAngle - 180;
                    if (num7.IsBetweenRange(-180, (-180 + Overflow))) isInRange = true;
                }
                if (actualMinAngle < -180)
                {
                    float Underflow = actualMinAngle + 180;
                    if (num7.IsBetweenRange((180 + Underflow), 180)) isInRange = true;
                }
                return isInRange;
            }
            return false;
        }
        private static void HandleHeroSwordSlash(List<SpeculativeRigidbody> alreadyHit, Vector2 arcOrigin, float slashAngle, int slashId, PlayerController owner, ProjInteractMode intmode, float damageToDeal, float enemyKnockback, List<GameActorEffect> statusEffects, float jammedDMGMult, float bossDMGMult)
        {
            float degreesOfSlash = 90f;
            float slashRange = 2.5f;
            ReadOnlyCollection<Projectile> allProjectiles2 = StaticReferenceManager.AllProjectiles;
            for (int j = allProjectiles2.Count - 1; j >= 0; j--)
            {
                Projectile projectile2 = allProjectiles2[j];
                if (ProjectileIsValid(projectile2))
                {
                    Vector2 projectileCenter = projectile2.sprite.WorldCenter;
                    if (ObjectWasHitBySlash(projectileCenter, arcOrigin, slashAngle, slashRange, degreesOfSlash))
                    {
                        if (intmode != ProjInteractMode.IGNORE || projectile2.collidesWithProjectiles)
                        {
                            if (intmode == ProjInteractMode.DESTROY || intmode == ProjInteractMode.IGNORE) projectile2.DieInAir(false, true, true, true);
                            else if (intmode == ProjInteractMode.REFLECT)
                            {
                                if (projectile2.LastReflectedSlashId != slashId)
                                {
                                    PassiveReflectItem.ReflectBullet(projectile2, true, owner, 2f, 1f, 1f, 0f);
                                    projectile2.LastReflectedSlashId = slashId;
                                }
                            }
                        }
                    }
                }
            }
            DealDamageToEnemiesInArc(owner, arcOrigin, slashAngle, slashRange, damageToDeal, enemyKnockback, statusEffects, jammedDMGMult, bossDMGMult, alreadyHit);

            List<MinorBreakable> allMinorBreakables = StaticReferenceManager.AllMinorBreakables;
            for (int k = allMinorBreakables.Count - 1; k >= 0; k--)
            {
                MinorBreakable minorBreakable = allMinorBreakables[k];
                if (minorBreakable && minorBreakable.specRigidbody)
                {
                    if (!minorBreakable.IsBroken && minorBreakable.sprite)
                    {
                        if (ObjectWasHitBySlash(minorBreakable.sprite.WorldCenter, arcOrigin, slashAngle, slashRange, degreesOfSlash))
                        {
                            minorBreakable.Break();
                        }
                    }
                }
            }
            List<MajorBreakable> allMajorBreakables = StaticReferenceManager.AllMajorBreakables;
            for (int l = allMajorBreakables.Count - 1; l >= 0; l--)
            {
                MajorBreakable majorBreakable = allMajorBreakables[l];
                if (majorBreakable && majorBreakable.specRigidbody)
                {
                    if (!alreadyHit.Contains(majorBreakable.specRigidbody))
                    {
                        if (!majorBreakable.IsSecretDoor && !majorBreakable.IsDestroyed)
                        {
                            if (ObjectWasHitBySlash(majorBreakable.specRigidbody.UnitCenter, arcOrigin, slashAngle, slashRange, degreesOfSlash))
                            {
                                float num9 = damageToDeal;
                                if (majorBreakable.healthHaver)
                                {
                                    num9 *= 0.2f;
                                }
                                majorBreakable.ApplyDamage(num9, majorBreakable.specRigidbody.UnitCenter - arcOrigin, false, false, false);
                                alreadyHit.Add(majorBreakable.specRigidbody);
                            }
                        }
                    }
                }
            }
        }
        private static void DealDamageToEnemiesInArc(PlayerController owner, Vector2 arcOrigin, float arcAngle, float arcRadius, float overrideDamage, float overrideForce, List<GameActorEffect> statusEffects, float jammedDMGMult, float bossDMGMult, List<SpeculativeRigidbody> alreadyHit = null)
        {
            RoomHandler roomHandler = owner.CurrentRoom;
            if (roomHandler == null) return;
            List<AIActor> activeEnemies = roomHandler.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            if (activeEnemies == null) return;

            for (int i = 0; i < activeEnemies.Count; i++)
            {
                AIActor aiactor = activeEnemies[i];
                if (aiactor && aiactor.specRigidbody && aiactor.IsNormalEnemy && !aiactor.IsGone && aiactor.healthHaver)
                {
                    if (alreadyHit == null || !alreadyHit.Contains(aiactor.specRigidbody))
                    {
                        for (int j = 0; j < aiactor.healthHaver.NumBodyRigidbodies; j++)
                        {
                            SpeculativeRigidbody bodyRigidbody = aiactor.healthHaver.GetBodyRigidbody(j);
                            PixelCollider hitboxPixelCollider = bodyRigidbody.HitboxPixelCollider;
                            if (hitboxPixelCollider != null)
                            {
                                Vector2 vector = BraveMathCollege.ClosestPointOnRectangle(arcOrigin, hitboxPixelCollider.UnitBottomLeft, hitboxPixelCollider.UnitDimensions);
                                float num = Vector2.Distance(vector, arcOrigin);
                                if (ObjectWasHitBySlash(vector, arcOrigin, arcAngle, arcRadius, 90))
                                {
                                    bool flag = true;
                                    int rayMask = CollisionMask.LayerToMask(CollisionLayer.HighObstacle, CollisionLayer.BulletBlocker, CollisionLayer.BulletBreakable);
                                    RaycastResult raycastResult;
                                    if (PhysicsEngine.Instance.Raycast(arcOrigin, vector - arcOrigin, num, out raycastResult, true, true, rayMask, null, false, null, null) && raycastResult.SpeculativeRigidbody != bodyRigidbody)
                                    {
                                        flag = false;
                                    }
                                    RaycastResult.Pool.Free(ref raycastResult);
                                    if (flag)
                                    {
                                        float damage = DealSwordDamageToEnemy(owner, aiactor, arcOrigin, vector, arcAngle, overrideDamage, overrideForce, statusEffects, bossDMGMult, jammedDMGMult);
                                        if (alreadyHit != null)
                                        {
                                            if (alreadyHit.Count == 0)
                                            {
                                                StickyFrictionManager.Instance.RegisterSwordDamageStickyFriction(damage);
                                            }
                                            alreadyHit.Add(aiactor.specRigidbody);
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private static float DealSwordDamageToEnemy(PlayerController owner, AIActor targetEnemy, Vector2 arcOrigin, Vector2 contact, float angle, float damage, float knockback, List<GameActorEffect> statusEffects, float bossDMGMult, float jammedDMGMult)
        {
            if (targetEnemy.healthHaver)
            {
                float damageToDeal = damage;
                if (targetEnemy.healthHaver.IsBoss) damageToDeal *= bossDMGMult;
                if (targetEnemy.IsBlackPhantom) damageToDeal *= jammedDMGMult;
                targetEnemy.healthHaver.ApplyDamage(damageToDeal, contact - arcOrigin, owner.ActorName, CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
            }
            if (targetEnemy.knockbackDoer)
            {
                targetEnemy.knockbackDoer.ApplyKnockback(contact - arcOrigin, knockback, false);
            }
            if (statusEffects != null && statusEffects.Count > 0)
            {
                foreach (GameActorEffect effect in statusEffects)
                {
                    targetEnemy.ApplyEffect(effect);
                }
            }
            return damage;
        }
    }
}
