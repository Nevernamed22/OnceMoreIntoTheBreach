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
using System.Linq;

namespace NevernamedsItems
{
    public class PrismaticSnail
    {
        public static void Init()
        {
            string name = "Prismatic Snail";
            string resourcePath = "NevernamedsItems/Resources/Companions/PrismaticSnail/prismaticsnail_icon";
            GameObject gameObject = new GameObject();
            CompanionItem companionItem = gameObject.AddComponent<CompanionItem>();
            ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
            string shortDesc = "27616";
            string longDesc = "A curious mollusc in a crystal shell. When you put your ear to the opening, you can hear it frantically reciting the same string of numbers over and over again...";
            companionItem.SetupItem(shortDesc, longDesc, "nn");
            companionItem.quality = PickupObject.ItemQuality.B;
            companionItem.CompanionGuid = guid;
            BuildPrefab();

            //BULLET STATS
            Projectile projectile = (PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0].InstantiateAndFakeprefab();

            BasicBeamController beamComp = projectile.GenerateBeamPrefab(
                "NevernamedsItems/Resources/BeamSprites/pocolocobeam_mid_001",
                new Vector2(4, 4),
                new Vector2(0, 0),
                new List<string>()
                {
                "NevernamedsItems/Resources/BeamSprites/pocolocobeam_mid_001",
                "NevernamedsItems/Resources/BeamSprites/pocolocobeam_mid_002",
                "NevernamedsItems/Resources/BeamSprites/pocolocobeam_mid_003",
                "NevernamedsItems/Resources/BeamSprites/pocolocobeam_mid_004",
                "NevernamedsItems/Resources/BeamSprites/pocolocobeam_mid_005",
                "NevernamedsItems/Resources/BeamSprites/pocolocobeam_mid_006",
                "NevernamedsItems/Resources/BeamSprites/pocolocobeam_mid_007",
                "NevernamedsItems/Resources/BeamSprites/pocolocobeam_mid_008",
                "NevernamedsItems/Resources/BeamSprites/pocolocobeam_mid_009",
                "NevernamedsItems/Resources/BeamSprites/pocolocobeam_mid_010",
                "NevernamedsItems/Resources/BeamSprites/pocolocobeam_mid_011",
                "NevernamedsItems/Resources/BeamSprites/pocolocobeam_mid_012",
                },
                13,
                //Impact
                new List<string>()
                {
                "NevernamedsItems/Resources/BeamSprites/yellowbeam_impact_001",
                "NevernamedsItems/Resources/BeamSprites/yellowbeam_impact_002",
                "NevernamedsItems/Resources/BeamSprites/yellowbeam_impact_003",
                "NevernamedsItems/Resources/BeamSprites/yellowbeam_impact_004",
                },
                13,
                new Vector2(4, 4),
                new Vector2(7, 7),
                //End
                null,
                -1,
                null,
                null,
                //Beginning
                null,
                -1,
                null,
                null,
                //Other Variables
                10
                );


            projectile.baseData.damage = 5f;
            projectile.baseData.force *= 1f;
            projectile.baseData.range *= 200;


            beamComp.boneType = BasicBeamController.BeamBoneType.Straight;
            beamComp.startAudioEvent = "Play_WPN_radiationlaser_shot_01";
            beamComp.endAudioEvent = "Stop_WPN_All";
            prismBeam = projectile.gameObject;
        }
        public static GameObject prismBeam;
        public static void BuildPrefab()
        {
            if (!(prefab != null || CompanionBuilder.companionDictionary.ContainsKey(guid)))
            {
                prefab = CompanionBuilder.BuildPrefab("PrismaticSnail", guid, "NevernamedsItems/Resources/Companions/PrismaticSnail/prismatismsnail_idleleft_001", new IntVector2(7, 1), new IntVector2(11, 8));
                var companionController = prefab.AddComponent<PrismaticSnailController>();
                companionController.aiActor.MovementSpeed = 4f;

                AIAnimator animator = prefab.GetOrAddComponent<AIAnimator>();

                animator.AdvAddAnimation("idle",
                    DirectionalAnimation.DirectionType.TwoWayHorizontal,
                    CompanionBuilder.AnimationType.Idle,
                    new List<AnimationUtilityExtensions.DirectionalAnimationData>()
                    {
                        new AnimationUtilityExtensions.DirectionalAnimationData()
                        {
                            subAnimationName = "idle_right",
                            wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop,
                            fps = 9,
                            pathDirectory = "NevernamedsItems/Resources/Companions/PrismaticSnail/prismatismsnail_idleright",
                        },
                        new AnimationUtilityExtensions.DirectionalAnimationData()
                        {
                            subAnimationName = "idle_left",
                            wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop,
                            fps = 9,
                            pathDirectory = "NevernamedsItems/Resources/Companions/PrismaticSnail/prismatismsnail_idleleft",
                        }
                    }
                    );
                animator.AdvAddAnimation("move",
                    DirectionalAnimation.DirectionType.TwoWayHorizontal,
                    CompanionBuilder.AnimationType.Move,
                    new List<AnimationUtilityExtensions.DirectionalAnimationData>()
                    {
                        new AnimationUtilityExtensions.DirectionalAnimationData()
                        {
                            subAnimationName = "move_right",
                            wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop,
                            fps = 9,
                            pathDirectory = "NevernamedsItems/Resources/Companions/PrismaticSnail/prismatismsnail_moveright",
                        },
                        new AnimationUtilityExtensions.DirectionalAnimationData()
                        {
                            subAnimationName = "move_left",
                            wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop,
                            fps = 9,
                            pathDirectory = "NevernamedsItems/Resources/Companions/PrismaticSnail/prismatismsnail_moveleft",
                        }
                    }
                    );

                animator.AdvAddAnimation("attack",
                    DirectionalAnimation.DirectionType.TwoWayHorizontal,
                    CompanionBuilder.AnimationType.Other,
                    new List<AnimationUtilityExtensions.DirectionalAnimationData>()
                    {
                        new AnimationUtilityExtensions.DirectionalAnimationData()
                        {
                            subAnimationName = "attack_right",
                            wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop,
                            fps = 1,
                            pathDirectory = "NevernamedsItems/Resources/Companions/PrismaticSnail/prismatismsnail_attackright",
                        },
                        new AnimationUtilityExtensions.DirectionalAnimationData()
                        {
                            subAnimationName = "attack_left",
                            wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop,
                            fps = 1,
                            pathDirectory = "NevernamedsItems/Resources/Companions/PrismaticSnail/prismatismsnail_attackleft",
                        }
                    }
                    );


                BehaviorSpeculator component = prefab.GetComponent<BehaviorSpeculator>();
                CustomCompanionBehaviours.SimpleCompanionApproach approach = new CustomCompanionBehaviours.SimpleCompanionApproach();
                approach.DesiredDistance = 5f;
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
        public class PrismaticSnailController : CompanionController
        {
            private void Start()
            {
                this.Owner = this.m_owner;
                timerRemaining = 7f;
                attacking = false;
            }
            private void Update()
            {
                if (Owner && Owner.IsInCombat && (base.aiActor.Position.GetAbsoluteRoom() != null && Owner.CurrentRoom != null && base.aiActor.Position.GetAbsoluteRoom() == Owner.CurrentRoom))
                {
                    if (timerRemaining > 0)
                    {
                        timerRemaining -= BraveTime.DeltaTime;
                    }
                    else if (!attacking)
                    {
                        base.StartCoroutine(Attack());
                    }
                }
            }
            private bool attacking;
            private IEnumerator Attack()
            {
                attacking = true;
                base.aiActor.MovementSpeed = 0;

                float amt = 360;
                if (UnityEngine.Random.value <= 0.5f) amt = -360;
                for (int i = 0; i < 8; i++)
                {
                    StartCoroutine(BeamLerp(i * 45, (i * 45) + amt));
                }
                this.aiAnimator.PlayForDuration("attack", 5);

                yield return new WaitForSeconds(5f);
                base.aiActor.MovementSpeed = 4;
                timerRemaining = 7f;
                attacking = false;
                yield break;
            }

            private IEnumerator BeamLerp(float startAngle, float endAngle)
            {
                float elapsed = 0f;

                BeamController beam = BeamAPI.FreeFireBeamFromAnywhere(prismBeam.GetComponent<Projectile>(), base.m_owner, base.gameObject, Vector2.zero, startAngle, 5, true);
                Projectile beamprojcomponent = beam.GetComponent<Projectile>();
                if (PassiveItem.IsFlagSetForCharacter(this.Owner, typeof(BattleStandardItem))) beamprojcomponent.baseData.damage *= BattleStandardItem.BattleStandardCompanionDamageMultiplier;
                if (this.Owner.CurrentGun && this.Owner.CurrentGun.LuteCompanionBuffActive) beamprojcomponent.baseData.damage *= 2;

                beamprojcomponent.baseData.damage *= Owner.stats.GetStatValue(PlayerStats.StatType.Damage);
                beamprojcomponent.baseData.speed *= Owner.stats.GetStatValue(PlayerStats.StatType.ProjectileSpeed);
                beamprojcomponent.baseData.force *= Owner.stats.GetStatValue(PlayerStats.StatType.KnockbackMultiplier);
                beamprojcomponent.BossDamageMultiplier *= Owner.stats.GetStatValue(PlayerStats.StatType.DamageToBosses);

                while (elapsed <= 5f)
                {
                    if (beam)
                    {
                        float finalAngle = Mathf.Lerp(startAngle, endAngle, elapsed / 5f);
                        beam.Direction = finalAngle.DegreeToVector2();
                        elapsed += BraveTime.DeltaTime;
                        yield return null;
                    }
                }
                yield break;
            }


            private float timerRemaining;
            private PlayerController Owner;
        }
        public static GameObject prefab;
        private static readonly string guid = "omitb_prismaticsnail_companion";
    }
}