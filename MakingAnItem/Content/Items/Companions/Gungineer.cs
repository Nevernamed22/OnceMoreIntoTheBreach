using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using Gungeon;
using Alexandria.ItemAPI;
using UnityEngine;
using SaveAPI;

namespace NevernamedsItems
{
    public class Gungineer : PassiveItem
    {
        public static void Init()
        {
            CompanionItem companionItem = ItemSetup.NewItem<CompanionItem>(
            "Gungineer",
            "Solves Problems",
            "This small minelet is the sole member of the Gungeon Engineers Union, and as such bears loyalty only to the bearer of the unions signature hammer. More than happy to do construction work- on a contract basis."+"\n\nForklift certified.",
            "gungineer_icon") as CompanionItem;
            companionItem.quality = PickupObject.ItemQuality.B;
            companionItem.CompanionGuid = Gungineer.guid;
            ID = companionItem.PickupObjectId;
            Gungineer.BuildPrefab();
            companionItem.SetupUnlockOnCustomStat(CustomTrackedStats.BEGGAR_TOTAL_DONATIONS, 634, DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN);
        }
        public static int ID;
        public static void BuildPrefab()
        {
            if (Gungineer.prefab == null || !CompanionBuilder.companionDictionary.ContainsKey(Gungineer.guid))
            {
                Gungineer.prefab = EntityTools.BuildEntity("Gungineer", Gungineer.guid, "gungineer_idleleft_001", Initialisation.companionCollection, new IntVector2(8, 8), new IntVector2(-4, 0));
                CompanionController companionController = Gungineer.prefab.AddComponent<CompanionController>();
                companionController.companionID = CompanionController.CompanionIdentifier.NONE;

                tk2dSpriteAnimator dumbanimator = Gungineer.prefab.GetComponent<tk2dSpriteAnimator>();
                dumbanimator.Library = Initialisation.companionAnimationCollection;

                AIActor aiactor = prefab.GetComponent<AIActor>();
                aiactor.CanDropCurrency = false;
                aiactor.CanDropItems = false;
                aiactor.BaseMovementSpeed = 6;
                aiactor.IgnoreForRoomClear = false;
                aiactor.TryDodgeBullets = false;
                aiactor.ActorName = "Gungineer";
                aiactor.DoDustUps = true;
                aiactor.SetIsFlying(true, "hovering", false, true);
                aiactor.ActorShadowOffset = new Vector3(0f, 0.4f);

                AIAnimator animator = Gungineer.prefab.GetComponent<AIAnimator>();
                animator.IdleAnimation = new DirectionalAnimation()
                {
                    Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
                    Flipped = new DirectionalAnimation.FlipType[2],
                    AnimNames = new List<string>() { "gungineer_idleright", "gungineer_idleleft" }.ToArray(),
                    Prefix = string.Empty,
                };
                animator.MoveAnimation = new DirectionalAnimation()
                {
                    Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
                    Flipped = new DirectionalAnimation.FlipType[2],
                    AnimNames = new List<string>() { "gungineer_moveright", "gungineer_moveleft" }.ToArray(),
                    Prefix = string.Empty,
                };
                AIAnimator.NamedDirectionalAnimation newOtheranim = new AIAnimator.NamedDirectionalAnimation
                {
                    name = "build",
                    anim = new DirectionalAnimation
                    {
                        Prefix = string.Empty,
                        Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
                        Flipped = new DirectionalAnimation.FlipType[2],
                        AnimNames = new List<string>() { "gungineer_buildright", "gungineer_buildleft" }.ToArray(),
                    }
                };
                if (animator.OtherAnimations == null) animator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>();
                animator.OtherAnimations.Add(newOtheranim);

                BehaviorSpeculator component = Gungineer.prefab.GetComponent<BehaviorSpeculator>();
                component.MovementBehaviors.Add(new BuildTrapsBehaviour());

                component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior
                {
                    CatchUpRadius = 6,
                    CatchUpMaxSpeed = 10,
                    CatchUpAccelTime = 1,
                    CatchUpSpeed = 7,
                });
            }
        }
        public static GameObject prefab;

        private static readonly string guid = "gungineer_84298yhjvfactbbianxwyuew7xwu3";
    }
    public class BuildTrapsBehaviour : MovementBehaviorBase
    {
        public override void Upkeep()
        {
            base.Upkeep();
            base.DecrementTimer(ref this.m_repathTimer, false);
        }
        public override BehaviorResult Update()
        {
            PlayerController primaryPlayer = GameManager.Instance.PrimaryPlayer;
            if (primaryPlayer == null || primaryPlayer.CurrentRoom == null || !primaryPlayer.CurrentRoom.IsSealed)
            {
                if (speedy) { this.m_aiActor.MovementModifiers -= this.CatchUpMovementModifier; }
                speedy = false;
                arbitraryTrapPoint = Vector2.zero;
                this.m_aiAnimator.EndAnimationIf("build");
                return BehaviorResult.Continue;
            }

            //If the code reaches this point, it must be in an active, valid room with an owner

            //If the Trap Point is null, set it to a random point
            if (arbitraryTrapPoint == Vector2.zero) { arbitraryTrapPoint = (primaryPlayer.CurrentRoom.GetRandomVisibleClearSpot(2, 2) + new IntVector2(1,1)).ToCenterVector2(); }

            //Apply increased movement Speed
            if (!speedy) { this.m_aiActor.MovementModifiers += this.CatchUpMovementModifier; }
            speedy = true;

            //Check the distance to the trap point.
            float num = Vector2.Distance(arbitraryTrapPoint, this.m_aiActor.CenterPosition);
            if (num <= 1.4f) //If distance to the trap point is less than or equal to 1.4
            {
                //Clear the remaining path
                this.m_aiActor.ClearPath();

                //If Build animation is not playing, play it
                if (!this.m_aiAnimator.IsPlaying("build")) { this.m_aiAnimator.PlayUntilCancelled("build", false, null, -1f, false); }

                //Increment the building timer
                timeBuilding += base.m_deltaTime;
               //ETGModConsole.Log("DeltaTime: " + base.m_deltaTime);
                //ETGModConsole.Log("timeBuilding: " + timeBuilding);

                //If the building timer is greater than 3, build a trap, reset the timer, and end the build animation
                if (timeBuilding > 5f)
                {
                    timeBuilding = 0f;
                    CreateObj();
                    arbitraryTrapPoint = (primaryPlayer.CurrentRoom.GetRandomVisibleClearSpot(2, 2) + new IntVector2(1, 1)).ToCenterVector2();
                    this.m_aiAnimator.EndAnimationIf("build");
                }

                //Then skip the remaining behaviours - returns early
                return BehaviorResult.SkipRemainingClassBehaviors;
            }

            //If the distance to the Arbitrary Trap point is greater than 1.4, and the repath timer has expired, we choose to re-path
            if (this.m_repathTimer <= 0f)
            {
                this.m_aiAnimator.EndAnimationIf("build");
                timeBuilding = 0f;
                this.m_repathTimer = this.PathInterval;
                this.m_aiActor.PathfindToPosition(arbitraryTrapPoint, null, true, null, null, null, false);
            }

            //Skip remaining
            return BehaviorResult.SkipRemainingClassBehaviors;
        }
        private void CatchUpMovementModifier(ref Vector2 voluntaryVel, ref Vector2 involuntaryVel)
        {
            voluntaryVel = voluntaryVel.normalized * 7f;
        }
        public void CreateObj()
        {
            if (traps == null) { traps = new List<GameObject>(); }
            if (traps.Count == 0)
            {
                traps.AddRange(new List<GameObject>()
                {
                    PickupObjectDatabase.GetById(71).GetComponent<SpawnObjectPlayerItem>().objectToSpawn.gameObject,
                    PickupObjectDatabase.GetById(66).GetComponent<SpawnObjectPlayerItem>().objectToSpawn.gameObject,
                    PickupObjectDatabase.GetById(66).GetComponent<SpawnObjectPlayerItem>().objectToSpawn.gameObject,
                    PickupObjectDatabase.GetById(438).GetComponent<SpawnObjectPlayerItem>().objectToSpawn.gameObject,
                    EnemyDatabase.GetOrLoadByGuid(PickupObjectDatabase.GetById(201).GetComponent<SpawnObjectPlayerItem>().enemyGuidToSpawn).gameObject,
                    TackShooter.TackShooterObject,
                    TackShooter.TackShooterObject,
                });
            }
            if (arbitraryTrapPoint != Vector2.zero)
            {
                //Debug.Log("1");
                SpawnManager.SpawnVFX((PickupObjectDatabase.GetById(37) as Gun).DefaultModule.chargeProjectiles[0].Projectile.hitEffects.overrideMidairDeathVFX, arbitraryTrapPoint, Quaternion.identity);
                //Debug.Log("2");

                GameObject spawned = UnityEngine.Object.Instantiate<GameObject>(BraveUtility.RandomElement(traps), arbitraryTrapPoint, Quaternion.identity);
                //Debug.Log("3");
                tk2dBaseSprite spawnedSprite = spawned.GetComponent<tk2dBaseSprite>();
                //Debug.Log("4");

                if (spawnedSprite) { spawnedSprite.PlaceAtPositionByAnchor(arbitraryTrapPoint, tk2dBaseSprite.Anchor.MiddleCenter); }

               // Debug.Log("5");
                if (spawned.GetComponent<TackShooterBehaviour>()) { spawned.GetComponent<TackShooterBehaviour>().owner = base.m_aiActor.CompanionOwner; }
                //Debug.Log("6");
                AkSoundEngine.PostEvent("Play_ITM_Folding_Table_Use_01", spawned.gameObject);
            }
        }
        public bool speedy = false;
        public List<GameObject> traps = new List<GameObject>();
        public float timeBuilding = 0;
        public Vector2 arbitraryTrapPoint = Vector2.zero;
        public float PathInterval = 0.25f;
        private float m_repathTimer;
    }
}

