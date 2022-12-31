using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using Gungeon;
using Alexandria.ItemAPI;
using SaveAPI;
using UnityEngine;
using Alexandria.Misc;
using Alexandria.EnemyAPI;

namespace NevernamedsItems
{
    public class Cubud : PassiveItem
    {
        public static void Init()
        {
            string name = "Cubud";
            string resourcePath = "NevernamedsItems/Resources/Companions/Cubud/cubud_icon";
            GameObject gameObject = new GameObject();
            CompanionItem companionItem = gameObject.AddComponent<CompanionItem>();
            ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);

            string shortDesc = "Stanking Officer";
            string longDesc = "An up and coming officer in the Blobulonian army before it's fall- they were one of the few members of the aerial corps to volunteer for the genetic modification program.";
            companionItem.SetupItem(shortDesc, longDesc, "nn");
            companionItem.quality = PickupObject.ItemQuality.C;
            companionItem.CompanionGuid = guid;
            BuildPrefab();

            companionItem.AddToSubShop(ItemBuilder.ShopType.Goopton);
        }


        public static void BuildPrefab()
        {
            if (!(prefab != null || CompanionBuilder.companionDictionary.ContainsKey(guid)))
            {
                prefab = CompanionBuilder.BuildPrefab("Cubud", guid, "NevernamedsItems/Resources/Companions/Cubud/cubud_idleright_001", new IntVector2(12, 1), new IntVector2(7, 9));
                var companionController = prefab.AddComponent<CubudController>();
                companionController.aiActor.MovementSpeed = 4f;

                AIAnimator animator = prefab.GetOrAddComponent<AIAnimator>();

                companionController.CanCrossPits = true;
                companionController.aiActor.SetIsFlying(true, "Flying Entity", false, true);
                companionController.aiActor.ActorShadowOffset = new Vector3(0, -0.2f);

                animator.AdvAddAnimation("idle",
                    DirectionalAnimation.DirectionType.FourWay,
                    CompanionBuilder.AnimationType.Idle,
                    new List<AnimationUtilityExtensions.DirectionalAnimationData>()
                    {
                        new AnimationUtilityExtensions.DirectionalAnimationData()
                        {
                            subAnimationName = "idle_back_right",
                            wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop,
                            fps = 9,
                            pathDirectory = "NevernamedsItems/Resources/Companions/Cubud/cubud_idleback",
                        },
                        new AnimationUtilityExtensions.DirectionalAnimationData()
                        {
                            subAnimationName = "idle_front_right",
                            wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop,
                            fps = 9,
                            pathDirectory = "NevernamedsItems/Resources/Companions/Cubud/cubud_idleright",
                        },
                        new AnimationUtilityExtensions.DirectionalAnimationData()
                        {
                            subAnimationName = "idle_front_left",
                            wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop,
                            fps = 9,
                            pathDirectory = "NevernamedsItems/Resources/Companions/Cubud/cubud_idleleft",
                        },
                        new AnimationUtilityExtensions.DirectionalAnimationData()
                        {
                            subAnimationName = "idle_back_left",
                            wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop,
                            fps = 9,
                            pathDirectory = "NevernamedsItems/Resources/Companions/Cubud/cubud_idleback",
                        },
                    }
                    );
                animator.AdvAddAnimation("attack",
                    DirectionalAnimation.DirectionType.FourWay,
                    CompanionBuilder.AnimationType.Other,
                    new List<AnimationUtilityExtensions.DirectionalAnimationData>()
                    {
                        new AnimationUtilityExtensions.DirectionalAnimationData()
                        {
                            subAnimationName = "attack_back_right",
                            wrapMode = tk2dSpriteAnimationClip.WrapMode.Once,
                            fps = 9,
                            pathDirectory = "NevernamedsItems/Resources/Companions/Cubud/cubud_attackback",
                        },
                        new AnimationUtilityExtensions.DirectionalAnimationData()
                        {
                            subAnimationName = "attack_front_right",
                            wrapMode = tk2dSpriteAnimationClip.WrapMode.Once,
                            fps = 9,
                            pathDirectory = "NevernamedsItems/Resources/Companions/Cubud/cubud_attackright",
                        },
                        new AnimationUtilityExtensions.DirectionalAnimationData()
                        {
                            subAnimationName = "attack_front_left",
                            wrapMode = tk2dSpriteAnimationClip.WrapMode.Once,
                            fps = 9,
                            pathDirectory = "NevernamedsItems/Resources/Companions/Cubud/cubud_attackleft",
                        },
                        new AnimationUtilityExtensions.DirectionalAnimationData()
                        {
                            subAnimationName = "attack_back_left",
                            wrapMode = tk2dSpriteAnimationClip.WrapMode.Once,
                            fps = 9,
                            pathDirectory = "NevernamedsItems/Resources/Companions/Cubud/cubud_attackback",
                        },
                    }
                    );


                BehaviorSpeculator component = prefab.GetComponent<BehaviorSpeculator>();
                CustomCompanionBehaviours.SimpleCompanionApproach approach = new CustomCompanionBehaviours.SimpleCompanionApproach();
                approach.DesiredDistance = 7f;
                component.MovementBehaviors.Add(approach);
                component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior
                {
                    IdleAnimations = new string[]
                    {
                        "idle"
                    },
                    CatchUpRadius = 6,
                    CatchUpMaxSpeed = 10,
                    CatchUpAccelTime = 1,
                    CatchUpSpeed = 7,

                });
            }
        }
        public class CubudController : CompanionController
        {
            private void Start()
            {
                this.Owner = this.m_owner;
                attackTimer = 2f;
            }
            public override void Update()
            {
                if (Owner && !Dungeon.IsGenerating && Owner.IsInCombat && base.transform.position.GetAbsoluteRoom() == Owner.CurrentRoom)
                {
                    if (attackTimer > 0)
                    {
                        attackTimer -= BraveTime.DeltaTime;
                    }
                    else
                    {
                        if (base.aiActor.OverrideTarget)
                        {
                            Vector2 pointOnTarget = base.aiActor.OverrideTarget.ClosestPointOnRigidBody(base.sprite.WorldCenter);
                            if (pointOnTarget != Vector2.zero)
                            {
                                if (!isAttacking)
                                {
                                    StartCoroutine(Attack(pointOnTarget));
                                }
                            }

                        }
                    }
                }
            }
            private IEnumerator Attack(Vector2 pointOnTarget)
            {
                base.aiActor.MovementSpeed = 0;
                isAttacking = true;
                base.aiAnimator.PlayUntilFinished("attack");

                yield return new WaitForSeconds(0.33f);
                AkSoundEngine.PostEvent("Play_OBJ_cauldron_splash_01", base.gameObject);
                for (int i = 0; i < 4; i++)
                {
                    float startAngle = 45f + (90f * i);
                    float endAngle = -45f + (90f * i);
                    StartCoroutine(DoPoisonStream(startAngle, endAngle));
                }

                yield return new WaitForSeconds(0.1f);
                base.aiActor.MovementSpeed = base.aiActor.BaseMovementSpeed;
                isAttacking = false;
                attackTimer = 4;
                yield break;
            }
            private IEnumerator DoPoisonStream(float startAngle, float endAngle)
            {
                float elapsed = 0f;

                BeamController beam = BeamAPI.FreeFireBeamFromAnywhere((PickupObjectDatabase.GetById(208) as Gun).DefaultModule.projectiles[0], Owner, base.gameObject, Vector2.zero, startAngle, 0.1f, true);

                Projectile beamprojcomponent = beam.GetComponent<Projectile>();
                if (PassiveItem.IsFlagSetForCharacter(this.Owner, typeof(BattleStandardItem))) beamprojcomponent.baseData.damage *= BattleStandardItem.BattleStandardCompanionDamageMultiplier;
                if (this.Owner.CurrentGun && this.Owner.CurrentGun.LuteCompanionBuffActive) beamprojcomponent.baseData.damage *= 2;

                beamprojcomponent.baseData.damage *= Owner.stats.GetStatValue(PlayerStats.StatType.Damage);
                beamprojcomponent.baseData.speed *= Owner.stats.GetStatValue(PlayerStats.StatType.ProjectileSpeed);
                beamprojcomponent.baseData.force *= Owner.stats.GetStatValue(PlayerStats.StatType.KnockbackMultiplier);
                beamprojcomponent.BossDamageMultiplier *= Owner.stats.GetStatValue(PlayerStats.StatType.DamageToBosses);

                while (elapsed <= 0.1f)
                {
                    if (beam)
                    {
                        float finalAngle = Mathf.Lerp(startAngle, endAngle, elapsed / 0.1f);                    
                        beam.Direction = finalAngle.DegreeToVector2();
                        elapsed += BraveTime.DeltaTime;
                        yield return null;
                    }
                }
                yield break;
            }
            public PlayerController Owner;
            private float attackTimer;
            private bool isAttacking;
        }
        public static GameObject prefab;
        private static readonly string guid = "nn:cubud";
    }
}