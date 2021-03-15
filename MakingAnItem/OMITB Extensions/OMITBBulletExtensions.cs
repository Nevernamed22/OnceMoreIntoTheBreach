using Dungeonator;
using ItemAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    static class OMITBBulletExtensions
    {
        public static PlayerController ProjectilePlayerOwner(this Projectile bullet)
        {
            if (bullet && bullet.Owner && bullet.Owner is PlayerController) return bullet.Owner as PlayerController;
            else return null;
        }
        public static RoomHandler GetAbsoluteRoom(this Projectile bullet)
        {
            Vector2 bulletPosition = bullet.sprite.WorldCenter;
            IntVector2 bulletPositionIntVector2 = bulletPosition.ToIntVector2();
            return GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(bulletPositionIntVector2);
        }
        public static void ReflectBullet(this Projectile p, bool retargetReflectedBullet, GameActor newOwner, float minReflectedBulletSpeed, bool doPostProcessing = false, float scaleModifier = 1f, float baseDamage = 10f, float spread = 0f, string sfx = null)
        {
            p.RemoveBulletScriptControl();
            if (sfx != null) AkSoundEngine.PostEvent(sfx, GameManager.Instance.gameObject);
            if (retargetReflectedBullet && p.Owner && p.Owner.specRigidbody)
            {
                p.Direction = (p.Owner.specRigidbody.GetUnitCenter(ColliderType.HitBox) - p.specRigidbody.UnitCenter).normalized;
            }
            if (spread != 0f) p.Direction = p.Direction.Rotate(UnityEngine.Random.Range(-spread, spread));
            if (p.Owner && p.Owner.specRigidbody) p.specRigidbody.DeregisterSpecificCollisionException(p.Owner.specRigidbody);

            p.Owner = newOwner;
            p.SetNewShooter(newOwner.specRigidbody);
            p.allowSelfShooting = false;
            p.collidesWithPlayer = false;
            p.collidesWithEnemies = true;
            if (scaleModifier != 1f)
            {
                SpawnManager.PoolManager.Remove(p.transform);
                p.RuntimeUpdateScale(scaleModifier);
            }
            if (p.Speed < minReflectedBulletSpeed) p.Speed = minReflectedBulletSpeed;
            p.baseData.damage = baseDamage;
            if (doPostProcessing)
            {
                PlayerController player = (newOwner as PlayerController);
                if (player != null)
                {
                    p.baseData.damage *= player.stats.GetStatValue(PlayerStats.StatType.Damage);
                    p.baseData.speed *= player.stats.GetStatValue(PlayerStats.StatType.ProjectileSpeed);
                    p.UpdateSpeed();
                    p.baseData.force *= player.stats.GetStatValue(PlayerStats.StatType.KnockbackMultiplier);
                    p.baseData.range *= player.stats.GetStatValue(PlayerStats.StatType.RangeMultiplier);
                    p.BossDamageMultiplier *= player.stats.GetStatValue(PlayerStats.StatType.DamageToBosses);
                    p.RuntimeUpdateScale(player.stats.GetStatValue(PlayerStats.StatType.PlayerBulletScale));
                    player.DoPostProcessProjectile(p);
                }
            }
            p.UpdateCollisionMask();
            p.Reflected();
            p.SendInDirection(p.Direction, true, true);
        }
        public static void ConvertToHelixMotion(this Projectile bullet, bool isInverted)
        {
            if (bullet.OverrideMotionModule != null && bullet.OverrideMotionModule is OrbitProjectileMotionModule)
            {
                OrbitProjectileMotionModule orbitProjectileMotionModule = bullet.OverrideMotionModule as OrbitProjectileMotionModule;
                orbitProjectileMotionModule.StackHelix = true;
                orbitProjectileMotionModule.ForceInvert = isInverted;
            }
            else if (!isInverted)
            {
                bullet.OverrideMotionModule = new HelixProjectileMotionModule();
            }
            else
            {
                bullet.OverrideMotionModule = new HelixProjectileMotionModule
                {
                    ForceInvert = true
                };
            }
        }
        public static void ApplyCompanionModifierToBullet(this Projectile bullet, PlayerController owner)
        {
            if (PassiveItem.IsFlagSetForCharacter(owner, typeof(BattleStandardItem)))
            {
                bullet.baseData.damage *= BattleStandardItem.BattleStandardCompanionDamageMultiplier;
            }
            if (owner.CurrentGun && owner.CurrentGun.LuteCompanionBuffActive)
            {
                bullet.baseData.damage *= 2f;
                bullet.RuntimeUpdateScale(1f / bullet.AdditionalScaleMultiplier);
                bullet.RuntimeUpdateScale(1.75f);
            }
        }
        public static List<GameActorEffect> GetFullListOfStatusEffects(this Projectile bullet, bool ignoresProbability = false)
        {
            List<GameActorEffect> Effects = new List<GameActorEffect>();
            if (bullet.statusEffectsToApply.Count > 0)
            {
                Effects.AddRange(bullet.statusEffectsToApply);
            }
            if (bullet.AppliesBleed && (UnityEngine.Random.value <= bullet.BleedApplyChance || ignoresProbability)) Effects.Add(bullet.bleedEffect);
            if (bullet.AppliesCharm && (UnityEngine.Random.value <= bullet.CharmApplyChance || ignoresProbability)) Effects.Add(bullet.charmEffect);
            if (bullet.AppliesCheese && (UnityEngine.Random.value <= bullet.CheeseApplyChance || ignoresProbability)) Effects.Add(bullet.cheeseEffect);
            if (bullet.AppliesFire && (UnityEngine.Random.value <= bullet.FireApplyChance || ignoresProbability)) Effects.Add(bullet.fireEffect);
            if (bullet.AppliesFreeze && (UnityEngine.Random.value <= bullet.FreezeApplyChance || ignoresProbability)) Effects.Add(bullet.freezeEffect);
            if (bullet.AppliesPoison && (UnityEngine.Random.value <= bullet.PoisonApplyChance || ignoresProbability)) Effects.Add(bullet.healthEffect);
            if (bullet.AppliesSpeedModifier && (UnityEngine.Random.value <= bullet.SpeedApplyChance || ignoresProbability)) Effects.Add(bullet.speedEffect);
            return Effects;
        }
    }
    static class OMITBBeamExtensions
    {

    }
    static class ChainedShadowBulletsHandler
    {
        public static void SpawnChainedShadowBullets(this Projectile source, int numberInChain, float pauseLength, float chainScaleMult = 1, Projectile overrideProj = null)
        {
            GameManager.Instance.Dungeon.StartCoroutine(ChainedShadowBulletsHandler.HandleShadowChainDelay(source, numberInChain, pauseLength, chainScaleMult, overrideProj));
        }
        private static IEnumerator HandleShadowChainDelay(Projectile proj, int amount, float delay, float scaleMult, Projectile overrideproj)
        {
            GameObject prefab = FakePrefab.Clone(proj.gameObject);
            if (overrideproj != null) prefab = FakePrefab.Clone(overrideproj.gameObject);
            Projectile prefabproj = prefab.GetComponent<Projectile>();
            Vector3 position = proj.transform.position;
            float rotation = proj.Direction.ToAngle();
            bool isInitialProjectile = true;
            for (int i = 0; i < amount; i++)
            {
                if (delay > 0f)
                {
                    float ela = 0f;
                    if (isInitialProjectile)
                    {
                        float initDelay = delay - 0.03f;
                        while (ela < initDelay)
                        {
                            ela += BraveTime.DeltaTime;
                            yield return null;
                        }
                        isInitialProjectile = false;
                    }
                    else
                    {
                        while (ela < delay)
                        {
                            ela += BraveTime.DeltaTime;
                            yield return null;
                        }
                    }
                }
                ChainedShadowBulletsHandler.SpawnShadowBullet(prefabproj, position, rotation, scaleMult);
            }
            yield break;
        }
        public static Projectile SpawnShadowBullet(Projectile obj, Vector3 position, float rotation, float chainScaleMult = 1)
        {
            GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(obj.gameObject, position, Quaternion.Euler(0f, 0f, rotation));
            if (gameObject2.GetComponent<AutoDoShadowChainOnSpawn>()) UnityEngine.Object.Destroy(gameObject2.GetComponent<AutoDoShadowChainOnSpawn>());
            gameObject2.transform.position += gameObject2.transform.right * -0.5f;
            Projectile component2 = gameObject2.GetComponent<Projectile>();
            component2.specRigidbody.Reinitialize();
            component2.collidesWithPlayer = false;
            component2.Owner = obj.Owner;
            component2.Shooter = obj.Shooter;
            component2.baseData.damage = obj.baseData.damage;
            component2.baseData.range = obj.baseData.range;
            component2.baseData.speed = obj.baseData.speed;
            component2.baseData.force = obj.baseData.force;
            component2.RuntimeUpdateScale(chainScaleMult);
            return component2;
        }
    }
}
