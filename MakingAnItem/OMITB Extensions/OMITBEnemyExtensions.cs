using Dungeonator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    static class OMITBEnemyExtensions
    {
        public static bool IsSecretlyTheMineFlayer(this AIActor target)
        {
            if (target)
            {
                foreach (AIActor maybeFlayer in StaticReferenceManager.AllEnemies)
                {
                    if (maybeFlayer && maybeFlayer.EnemyGuid == "8b0dd96e2fe74ec7bebc1bc689c0008a" && maybeFlayer.behaviorSpeculator)
                    {
                        List<MineFlayerShellGameBehavior> activeShellGames = maybeFlayer.behaviorSpeculator.FindAttackBehaviors<MineFlayerShellGameBehavior>();
                        if (activeShellGames.Count > 0)
                        {
                            foreach (MineFlayerShellGameBehavior behav in activeShellGames)
                            {
                                AIActor myBell = OMITBReflectionHelpers.ReflectGetField<AIActor>(typeof(MineFlayerShellGameBehavior), "m_myBell", behav);
                                if (myBell != null)
                                {
                                    if (myBell == target) return true;
                                }
                            }

                        }
                    }
                }
            }
            return false;
        }
        public static bool IsInMinecart(this AIActor target)
        {
            if (target && target.behaviorSpeculator)
            {
                foreach (MovementBehaviorBase behavbase in target.behaviorSpeculator.MovementBehaviors)
                {
                    if (behavbase is RideInCartsBehavior)
                    {
                        RideInCartsBehavior cartRiding = behavbase as RideInCartsBehavior;
                        bool isRidingCart = OMITBReflectionHelpers.ReflectGetField<bool>(typeof(RideInCartsBehavior), "m_ridingCart", cartRiding);
                        return isRidingCart;
                    }
                }
                return false;
            }
            else return false;
        }
        public static void ApplyGlitter(this AIActor target)
        {
            Material material2;
            int cachedSpriteBodyCount = target.healthHaver.bodySprites.Count;
            List<tk2dBaseSprite> sprites = target.healthHaver.bodySprites;
            for (int i = 0; i < cachedSpriteBodyCount; i++)
            {
                sprites[i].usesOverrideMaterial = true;
                MeshRenderer component4 = target.healthHaver.bodySprites[i].GetComponent<MeshRenderer>();
                Material[] sharedMaterials = component4.sharedMaterials;
                Array.Resize<Material>(ref sharedMaterials, sharedMaterials.Length + 1);
                Material material = UnityEngine.Object.Instantiate<Material>(target.renderer.material);
                material.SetTexture("_MainTex", sharedMaterials[0].GetTexture("_MainTex"));
                sharedMaterials[sharedMaterials.Length - 1] = material;
                component4.sharedMaterials = sharedMaterials;
                sharedMaterials[sharedMaterials.Length - 1].shader = ShaderCache.Acquire("Brave/Internal/GlitterPassAdditive");
            }
        }
        public static RoomHandler CurrentRoom(this AIActor bullet)
        {
            Vector2 bulletPosition = bullet.sprite.WorldCenter;
            IntVector2 bulletPositionIntVector2 = bulletPosition.ToIntVector2();
            return GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(bulletPositionIntVector2);
        }
        public static void DoGeniePunch(this AIActor enemy, PlayerController owner)
        {
            if (enemy && enemy.behaviorSpeculator)
            {
                enemy.behaviorSpeculator.Stun(1f, false);
                Projectile projectile = ((Gun)ETGMod.Databases.Items[0]).DefaultModule.projectiles[0];
                GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, enemy.sprite.WorldCenter, Quaternion.Euler(0f, 0f, 0f), true);
                GameObject gameObject2 = SpawnManager.SpawnProjectile(projectile.gameObject, enemy.sprite.WorldCenter, Quaternion.Euler(0f, 0f, 0f), true);
                GameObject gameObject3 = SpawnManager.SpawnProjectile(projectile.gameObject, enemy.sprite.WorldCenter, Quaternion.Euler(0f, 0f, 0f), true);
                Projectile component = gameObject.GetComponent<Projectile>();
                Projectile component2 = gameObject2.GetComponent<Projectile>();
                Projectile component3 = gameObject3.GetComponent<Projectile>();
                if (component != null && component2 != null && component3 != null)
                {
                    component.IgnoreTileCollisionsFor(10);
                    component.Owner = owner;
                    component.baseData.damage = 0f;
                    component.baseData.force = 0f;
                    component.baseData.range = 0f;
                    component.pierceMinorBreakables = true;
                    component.collidesWithPlayer = false;
                    component2.IgnoreTileCollisionsFor(10);
                    component2.Owner = owner;
                    component2.baseData.damage = 0f;
                    component2.baseData.force = 0f;
                    component2.baseData.range = 0f;
                    component2.pierceMinorBreakables = true;
                    component2.collidesWithPlayer = false;
                    component3.IgnoreTileCollisionsFor(10);
                    component3.Owner = owner;
                    component3.baseData.damage = 0f;
                    component3.baseData.force = 0f;
                    component3.baseData.range = 0f;
                    component3.pierceMinorBreakables = true;
                    component3.collidesWithPlayer = false;
                }
            }
        }
        public static bool IsBoss(this AIActor enemy)
        {
            if (enemy && enemy.healthHaver)
            {
                if (enemy.healthHaver.IsBoss) return true;
            }
            return false;
        }
        public static bool IsMiniBoss(this AIActor enemy)
        {
            if (enemy && EasyEnemyTypeLists.MiniBosses.Contains(enemy.EnemyGuid)) return true;
            else return false;
        }
        public static DirectionalAnimation GetDirectionalAnimation(this AIAnimator self, string animName)
        {
            if (string.IsNullOrEmpty(animName))
            {
                return null;
            }
            if (animName.Equals("idle", StringComparison.OrdinalIgnoreCase))
            {
                return self.IdleAnimation;
            }
            if (animName.Equals("move", StringComparison.OrdinalIgnoreCase))
            {
                return self.MoveAnimation;
            }
            if (animName.Equals("talk", StringComparison.OrdinalIgnoreCase))
            {
                return self.TalkAnimation;
            }
            if (animName.Equals("hit", StringComparison.OrdinalIgnoreCase))
            {
                return self.HitAnimation;
            }
            if (animName.Equals("flight", StringComparison.OrdinalIgnoreCase))
            {
                return self.FlightAnimation;
            }
            DirectionalAnimation result = null;
            int num = 0;
            for (int i = 0; i < self.OtherAnimations.Count; i++)
            {
                if (animName.Equals(self.OtherAnimations[i].name, StringComparison.OrdinalIgnoreCase))
                {
                    num++;
                    result = self.OtherAnimations[i].anim;
                }
            }
            if (num == 0)
            {
                return null;
            }
            if (num == 1)
            {
                return result;
            }
            int num2 = UnityEngine.Random.Range(0, num);
            num = 0;
            for (int j = 0; j < self.OtherAnimations.Count; j++)
            {
                if (animName.Equals(self.OtherAnimations[j].name, StringComparison.OrdinalIgnoreCase))
                {
                    if (num == num2)
                    {
                        return self.OtherAnimations[j].anim;
                    }
                    num++;
                }
            }
            return null;
        }
    }
}
