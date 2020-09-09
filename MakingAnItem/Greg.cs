/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using DirectionType = DirectionalAnimation.DirectionType;
using AnimationType = ItemAPI.CompanionBuilder.AnimationType;
using GungeonAPI;

namespace NevernamedsItems

{
    public class ExampleCompanion
    {
        public static GameObject prefab;
        private static readonly string guid = "greg_the_egg20932320123983129013"; //give your companion some unique guid

        public static void Init()
        {
            string itemName = "Greg The Egg";
            string resourceName = "NevernamedsItems/Resources/workinprogress_icon";

            GameObject obj = new GameObject();
            var item = obj.AddComponent<CompanionItem>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Coolest Kid On the Block";
            string longDesc = "This kid is so cool that people are starting to call YOU cool by association.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");
            item.quality = PickupObject.ItemQuality.B;
            item.CompanionGuid = guid; //this will be used by the item later to pull your companion from the enemy database
            item.Synergies = new CompanionTransformSynergy[0]; //this just needs to not be null
            BuildPrefab();
        }

        public static void BuildPrefab()
        {
            if (prefab != null || CompanionBuilder.companionDictionary.ContainsKey(guid))
                return;

            //Create the prefab with a starting sprite and hitbox offset/size
            prefab = CompanionBuilder.BuildPrefab("Greg The Egg", guid, "ItemAPI/Resources/BigSlime/Idle/son_idle_001", new IntVector2(1, 0), new IntVector2(9, 9));

            //Add a companion component to the prefab (could be a custom class)
            var companion = prefab.AddComponent<CompanionController>();
            companion.aiActor.MovementSpeed = 5f;

            //Add all of the needed animations (most of the animations need to have specific names to be recognized, like idle_right or attack_left)
            prefab.AddAnimation("idle_right", "ItemAPI/Resources/BigSlime/Idle", fps: 5, AnimationType.Idle, DirectionType.TwoWayHorizontal);
            prefab.AddAnimation("idle_left", "ItemAPI/Resources/BigSlime/Idle", fps: 5, AnimationType.Idle, DirectionType.TwoWayHorizontal);
            prefab.AddAnimation("run_right", "ItemAPI/Resources/BigSlime/MoveRight", fps: 7, AnimationType.Move, DirectionType.TwoWayHorizontal);
            prefab.AddAnimation("run_left", "ItemAPI/Resources/BigSlime/MoveLeft", fps: 7, AnimationType.Move, DirectionType.TwoWayHorizontal);

            //Add the behavior here, this too can be a custom class that extends AttackBehaviorBase or something like that
            var bs = prefab.GetComponent<BehaviorSpeculator>();
            bs.MovementBehaviors.Add(new CompanionFollowPlayerBehavior() { IdleAnimations = new string[] { "idle" } });

            //MAKE GREG COLLIDE WITH BULLETS
            companion.CanInterceptBullets = true;
            //companion.CanInterceptBullets = true;
            companion.aiActor.healthHaver.PreventAllDamage = true;
            companion.aiActor.specRigidbody.CollideWithOthers = true;
            companion.aiActor.specRigidbody.CollideWithTileMap = false;
            companion.aiActor.healthHaver.ForceSetCurrentHealth(1f);
            companion.aiActor.healthHaver.SetHealthMaximum(1f, null, false);
            companion.aiActor.specRigidbody.PixelColliders.Clear();
            companion.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider
            {
                ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
                //CollisionLayer = CollisionLayer.PlayerCollider,
                CollisionLayer = CollisionLayer.BulletBlocker,
                IsTrigger = false,
                BagleUseFirstFrameOnly = false,
                SpecifyBagelFrame = string.Empty,
                BagelColliderNumber = 0,
                ManualOffsetX = 0,
                ManualOffsetY = 0,
                ManualWidth = 16,
                ManualHeight = 16,
                ManualDiameter = 0,
                ManualLeftX = 0,
                ManualLeftY = 0,
                ManualRightX = 0,
                ManualRightY = 0
            });
        }
        public override void Pickup(PlayerController player)
        {

            base.Pickup(player);
            this.CreateNewCompanion(base.Owner);
        }

        private void CreateNewCompanion(PlayerController player)
        {
            AkSoundEngine.PostEvent("Play_OBJ_smallchest_spawn_01", base.gameObject);
            bool flag = this.companionsSpawned.Count + 1 == this.MaxNumberOfCompanions;
            if (flag)
            {

                bool flag4 = this.companionsSpawned.Count >= this.MaxNumberOfCompanions;
                if (flag4)
                {

                    float curDamage = base.Owner.stats.GetBaseStatValue(PlayerStats.StatType.Damage);
                    float newDamage = curDamage - 0.05f;
                    base.Owner.stats.SetBaseStatValue(PlayerStats.StatType.Damage, newDamage, base.Owner);
                    damageBuff = newDamage - curDamage;

                    AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(this.CompanionGuid);
                    Vector3 vector = player.transform.position;
                    bool flag7 = GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER;
                    bool flag8 = flag7;
                    bool flag9 = flag8;
                    if (flag9)
                    {
                        vector += new Vector3(1.125f, -0.3125f, 0f);
                    }
                    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(orLoadByGuid.gameObject, vector, Quaternion.identity);
                    CompanionController orAddComponent = gameObject.GetOrAddComponent<CompanionController>();
                    this.companionsSpawned.Add(orAddComponent);
                    orAddComponent.Initialize(player);
                    bool flag10 = orAddComponent.specRigidbody;
                    bool flag11 = flag10;
                    bool flag12 = flag11;
                    if (flag12)
                    {
                        PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(orAddComponent.specRigidbody, null, false);
                    }
                    orAddComponent.aiAnimator.PlayUntilFinished("spawn", false, null, -1f, false);
                }
            }
        }
        float killCount = 0;
        public int MaxNumberOfCompanions = 1;
        private List<CompanionController> companionsSpawned = new List<CompanionController>();

    }
}*/

