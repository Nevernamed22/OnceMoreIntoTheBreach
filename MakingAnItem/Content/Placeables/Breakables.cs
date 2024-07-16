using Alexandria.BreakableAPI;
using Alexandria.DungeonAPI;
using Alexandria.ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class Breakables
    {
        public static void Init()
        {
            GreyBarrel.Init();
            BulletScarecrow.Init();
            BigBlank.Init();
            CustomCandles.Init();
        }
        public static DebrisObject[] GenerateDebrisObjects(tk2dSpriteCollectionData spriteCollection, string[] shardSpriteNames, bool debrisObjectsCanRotate = true, float LifeSpanMin = 1f, float LifeSpanMax = 1f, float AngularVelocity = 1080, float AngularVelocityVariance = 0f, tk2dSprite shadowSprite = null, float Mass = 1, string AudioEventName = null, GameObject BounceVFX = null, int DebrisBounceCount = 1, bool DoesGoopOnRest = false, GoopDefinition GoopType = null, float GoopRadius = 1f)
        {
            List<DebrisObject> DebrisObjectList = new List<DebrisObject>();
            for (int i = 0; i < shardSpriteNames.Length; i++)
            {
                GameObject debrisObject = ItemBuilder.SpriteFromBundle(shardSpriteNames[i], spriteCollection.GetSpriteIdByName(shardSpriteNames[i]), spriteCollection, new GameObject("debris"));
                FakePrefab.MarkAsFakePrefab(debrisObject);
                tk2dSprite tk2dsprite = debrisObject.GetComponent<tk2dSprite>();

                tk2dsprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTiltedCutout");
                tk2dsprite.usesOverrideMaterial = true;
                DebrisObject DebrisObj = debrisObject.AddComponent<DebrisObject>();
                DebrisObj.canRotate = debrisObjectsCanRotate;
                DebrisObj.lifespanMin = LifeSpanMin;
                DebrisObj.lifespanMax = LifeSpanMax;
                DebrisObj.bounceCount = DebrisBounceCount;
                DebrisObj.angularVelocity = AngularVelocity;
                DebrisObj.angularVelocityVariance = AngularVelocityVariance;
                if (AudioEventName != null) { DebrisObj.audioEventName = AudioEventName; }
                if (BounceVFX != null) { DebrisObj.optionalBounceVFX = BounceVFX; }
                DebrisObj.sprite = tk2dsprite;
                DebrisObj.DoesGoopOnRest = DoesGoopOnRest;
                if (GoopType != null) { DebrisObj.AssignedGoop = GoopType; } else if (GoopType == null && DebrisObj.DoesGoopOnRest == true) { DebrisObj.DoesGoopOnRest = false; }
                DebrisObj.GoopRadius = GoopRadius;
                if (shadowSprite != null) { DebrisObj.shadowSprite = shadowSprite; }
                DebrisObj.inertialMass = Mass;
                DebrisObjectList.Add(DebrisObj);
            }
            DebrisObject[] DebrisArray = DebrisObjectList.ToArray();
            return DebrisArray;
        }
        public static MinorBreakable GenerateMinorBreakable(string name, tk2dSpriteCollectionData spriteCollection, tk2dSpriteAnimation animationCollection, string defaultSprite, string idleAnim, string breakAnim, string breakAudioEvent = "Play_OBJ_pot_shatter_01", int ColliderSizeX = 16, int ColliderSizeY = 8, int ColliderOffsetX = 0, int ColliderOffsetY = 8, GameObject DestroyVFX = null, List<CollisionLayer> collisionLayerList = null)
        {
            GameObject gameObject = ItemBuilder.SpriteFromBundle(defaultSprite, spriteCollection.GetSpriteIdByName(defaultSprite), spriteCollection, new GameObject("debris"));
            FakePrefab.MarkAsFakePrefab(gameObject);
            gameObject.name = name;
            MinorBreakable breakable = gameObject.AddComponent<MinorBreakable>();

            tk2dSprite sprite = gameObject.GetOrAddComponent<tk2dSprite>();
            sprite.GetComponent<MeshRenderer>().material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTilted");
            sprite.GetComponent<tk2dSprite>().usesOverrideMaterial = true;

            IntVector2 intVector = new IntVector2(ColliderSizeX, ColliderSizeY);
            IntVector2 colliderOffset = new IntVector2(ColliderOffsetX, ColliderOffsetY);
            IntVector2 colliderSize = new IntVector2(intVector.x, intVector.y);

            var speculativeRigidbody = gameObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(colliderOffset, colliderSize);
            speculativeRigidbody.CollideWithTileMap = false;
            speculativeRigidbody.CollideWithOthers = true;

            if (collisionLayerList == null)
            {
                speculativeRigidbody.PixelColliders.Add(new PixelCollider
                {
                    ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
                    CollisionLayer = CollisionLayer.HighObstacle,
                    IsTrigger = false,
                    BagleUseFirstFrameOnly = false,
                    SpecifyBagelFrame = string.Empty,
                    BagelColliderNumber = 0,
                    ManualOffsetX = colliderOffset.x,
                    ManualOffsetY = colliderOffset.y,
                    ManualWidth = colliderSize.x,
                    ManualHeight = colliderSize.y,
                    ManualDiameter = 0,
                    ManualLeftX = 0,
                    ManualLeftY = 0,
                    ManualRightX = 0,
                    ManualRightY = 0,
                });
            }
            else
            {
                foreach (CollisionLayer layer in collisionLayerList)
                {
                    speculativeRigidbody.PixelColliders.Add(new PixelCollider
                    {
                        ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
                        CollisionLayer = layer,
                        IsTrigger = false,
                        BagleUseFirstFrameOnly = false,
                        SpecifyBagelFrame = string.Empty,
                        BagelColliderNumber = 0,
                        ManualOffsetX = colliderOffset.x,
                        ManualOffsetY = colliderOffset.y,
                        ManualWidth = colliderSize.x,
                        ManualHeight = colliderSize.y,
                        ManualDiameter = 0,
                        ManualLeftX = 0,
                        ManualLeftY = 0,
                        ManualRightX = 0,
                        ManualRightY = 0,
                    });
                }
            }


            tk2dSpriteAnimator animator = gameObject.GetOrAddComponent<tk2dSpriteAnimator>();
            animator.Library = animationCollection;
            animator.DefaultClipId = animationCollection.GetClipIdByName(idleAnim);
            animator.playAutomatically = true;
            breakable.breakAnimName = breakAnim;

            breakable.sprite = sprite;
            breakable.specRigidbody = speculativeRigidbody;
            breakable.spriteAnimator = animator;
            breakable.breakAudioEventName = breakAudioEvent;

            if (DestroyVFX != null) { breakable.AdditionalVFXObject = DestroyVFX; }
            return breakable;
        }
        public static MajorBreakable GenerateMajorBreakable(string name, tk2dSpriteCollectionData spriteCollection, tk2dSpriteAnimation animationCollection, string defaultSprite, string idleAnim, string breakAnim, float HP = 100, bool UsesCustomColliderValues = false, int ColliderSizeX = 16, int ColliderSizeY = 8, int ColliderOffsetX = 0, int ColliderOffsetY = 8, bool DistribleShards = true, VFXPool breakVFX = null, VFXPool damagedVFX = null, bool BlocksPaths = false, List<CollisionLayer> collisionLayerList = null, Dictionary<float, string> preBreakframesAndHPPercentages = null, bool destroyedOnBreak = true, bool handlesOwnBreakAnim = false)
        {
            GameObject gameObject = ItemBuilder.SpriteFromBundle(defaultSprite, spriteCollection.GetSpriteIdByName(defaultSprite), spriteCollection, new GameObject("debris"));
            FakePrefab.MarkAsFakePrefab(gameObject);
            gameObject.name = name;
            MajorBreakable breakable = gameObject.AddComponent<MajorBreakable>();

            tk2dSprite sprite = gameObject.GetOrAddComponent<tk2dSprite>();
            sprite.GetComponent<MeshRenderer>().material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTilted");
            sprite.GetComponent<tk2dSprite>().usesOverrideMaterial = true;

            IntVector2 intVector = new IntVector2(ColliderSizeX, ColliderSizeY);
            IntVector2 colliderOffset = new IntVector2(ColliderOffsetX, ColliderOffsetY);
            IntVector2 colliderSize = new IntVector2(intVector.x, intVector.y);

            var speculativeRigidbody = gameObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(colliderOffset, colliderSize);
            speculativeRigidbody.CollideWithTileMap = false;
            speculativeRigidbody.CollideWithOthers = true;

            if (collisionLayerList == null)
            {
                speculativeRigidbody.PixelColliders.Add(new PixelCollider
                {
                    ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
                    CollisionLayer = CollisionLayer.HighObstacle,
                    IsTrigger = false,
                    BagleUseFirstFrameOnly = false,
                    SpecifyBagelFrame = string.Empty,
                    BagelColliderNumber = 0,
                    ManualOffsetX = colliderOffset.x,
                    ManualOffsetY = colliderOffset.y,
                    ManualWidth = colliderSize.x,
                    ManualHeight = colliderSize.y,
                    ManualDiameter = 0,
                    ManualLeftX = 0,
                    ManualLeftY = 0,
                    ManualRightX = 0,
                    ManualRightY = 0,
                });
            }
            else
            {
                foreach (CollisionLayer layer in collisionLayerList)
                {
                    speculativeRigidbody.PixelColliders.Add(new PixelCollider
                    {
                        ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
                        CollisionLayer = layer,
                        IsTrigger = false,
                        BagleUseFirstFrameOnly = false,
                        SpecifyBagelFrame = string.Empty,
                        BagelColliderNumber = 0,
                        ManualOffsetX = colliderOffset.x,
                        ManualOffsetY = colliderOffset.y,
                        ManualWidth = colliderSize.x,
                        ManualHeight = colliderSize.y,
                        ManualDiameter = 0,
                        ManualLeftX = 0,
                        ManualLeftY = 0,
                        ManualRightX = 0,
                        ManualRightY = 0,
                    });
                }
            }

            tk2dSpriteAnimator animator = gameObject.GetOrAddComponent<tk2dSpriteAnimator>();
            animator.Library = animationCollection;
            animator.DefaultClipId = animationCollection.GetClipIdByName(idleAnim);
            animator.playAutomatically = true;

            breakable.breakAnimation = breakAnim;
            breakable.shardBreakStyle = MinorBreakable.BreakStyle.CONE;
            breakable.sprite = sprite;
            breakable.specRigidbody = speculativeRigidbody;
            breakable.spriteAnimator = animator;
            breakable.HitPoints = HP;
            breakable.HandlePathBlocking = BlocksPaths;

            breakable.destroyedOnBreak = destroyedOnBreak;
            breakable.handlesOwnBreakAnimation = handlesOwnBreakAnim;

            if (breakVFX != null) { breakable.breakVfx = breakVFX; }
            if (damagedVFX != null) { breakable.damageVfx = damagedVFX; }

            if (preBreakframesAndHPPercentages != null)
            {
                List<BreakFrame> breakFrameList = new List<BreakFrame>();
                foreach (var Entry in preBreakframesAndHPPercentages)
                {
                    BreakFrame breakFrame = new BreakFrame();
                    breakFrame.healthPercentage = Entry.Key;
                    breakFrame.sprite = Entry.Value;
                    breakFrameList.Add(breakFrame);
                }
                BreakFrame[] array = breakFrameList.ToArray();
                breakable.prebreakFrames = array;
            }
            breakable.distributeShards = DistribleShards;

            return breakable;
        }
        public static List<ShardCluster> GenerateBarrelStyleShardClusters(List<string> BigChunks, List<string> MetalPiecesMedium, List<string> MetalPiecesSmall, List<string> WoodPiecesLarge, List<string> WoodPiecesMedium, List<string> WoodPiecesSmall)
        {
            ShardCluster shardClusterBig = BreakableAPIToolbox.GenerateShardCluster(Breakables.GenerateDebrisObjects(Initialisation.EnvironmentCollection, BigChunks.ToArray(),
                true, //CanRotate
                1, 1, //Lifespan Variables
                1080, //Angular Velocity
                0, //Velocity Variance
                null, //Shadow
                0.5f, //Mass
                null, //Audio Event
                null, //Bounce VFX
                1 //Bounces
                ), 0.1f, 0.5f, 1, 2, 0.1f);
            ShardCluster shardClusterMetalMedium = BreakableAPIToolbox.GenerateShardCluster(Breakables.GenerateDebrisObjects(Initialisation.EnvironmentCollection, MetalPiecesMedium.ToArray(),
                true, //CanRotate
                1, 1, //Lifespan Variables
                1080, //Angular Velocity
                0, //Velocity Variance
                null, //Shadow
                1f, //Mass
                null, //Audio Event
                null, //Bounce VFX
                1 //Bounces
                ), 1f, 1f, 1, 2, 1f);
            ShardCluster shardClusterMetalSmall = BreakableAPIToolbox.GenerateShardCluster(Breakables.GenerateDebrisObjects(Initialisation.EnvironmentCollection, MetalPiecesSmall.ToArray(),
                true, //CanRotate
                1, 1, //Lifespan Variables
                1080, //Angular Velocity
                0, //Velocity Variance
                null, //Shadow
                1f, //Mass
                null, //Audio Event
                null, //Bounce VFX
                1 //Bounces
                ), 1f, 1f, 1, 2, 1f);
            ShardCluster shardClusterWoodSmall = BreakableAPIToolbox.GenerateShardCluster(Breakables.GenerateDebrisObjects(Initialisation.EnvironmentCollection, WoodPiecesSmall.ToArray(),
                true, //CanRotate
                1, 1, //Lifespan Variables
                1080, //Angular Velocity
                0, //Velocity Variance
                null, //Shadow
                1f, //Mass
                null, //Audio Event
                null, //Bounce VFX
                1 //Bounces
                ), 1f, 1f, 1, 4, 1f);
            ShardCluster shardClusterWoodMedium = BreakableAPIToolbox.GenerateShardCluster(Breakables.GenerateDebrisObjects(Initialisation.EnvironmentCollection, WoodPiecesMedium.ToArray(),
                true, //CanRotate
                1, 1, //Lifespan Variables
                1080, //Angular Velocity
                0, //Velocity Variance
                null, //Shadow
                1f, //Mass
                null, //Audio Event
                null, //Bounce VFX
                1 //Bounces
                ), 1f, 1f, 1, 3, 1f);
            ShardCluster shardClusterWoodLarge = BreakableAPIToolbox.GenerateShardCluster(Breakables.GenerateDebrisObjects(Initialisation.EnvironmentCollection, WoodPiecesLarge.ToArray(),
                true, //CanRotate
                1, 1, //Lifespan Variables
                1080, //Angular Velocity
                0, //Velocity Variance
                null, //Shadow
                1f, //Mass
                null, //Audio Event
                null, //Bounce VFX
                1 //Bounces
                ), 1f, 1f, 0, 2, 1f);
            return new List<ShardCluster>() { shardClusterBig, shardClusterMetalMedium, shardClusterMetalSmall, shardClusterWoodSmall, shardClusterWoodMedium, shardClusterWoodLarge };
        }

        public static DebrisObject GenerateDebrisObject(tk2dSpriteCollectionData spriteCollection, string shardSpriteName, bool debrisObjectsCanRotate = true, float LifeSpanMin = 1f, float LifeSpanMax = 1f, float AngularVelocity = 1080, float AngularVelocityVariance = 0f, tk2dSprite shadowSprite = null, float Mass = 1, string AudioEventName = null, GameObject BounceVFX = null, int DebrisBounceCount = 1, bool DoesGoopOnRest = false, GoopDefinition GoopType = null, float GoopRadius = 1f)
        {
            GameObject debrisObject = ItemBuilder.SpriteFromBundle(shardSpriteName, spriteCollection.GetSpriteIdByName(shardSpriteName), spriteCollection, new GameObject("debris"));
            FakePrefab.MarkAsFakePrefab(debrisObject);
            tk2dSprite tk2dsprite = debrisObject.GetComponent<tk2dSprite>();

            tk2dsprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTiltedCutout");
            tk2dsprite.usesOverrideMaterial = true;
            DebrisObject DebrisObj = debrisObject.AddComponent<DebrisObject>();
            DebrisObj.canRotate = debrisObjectsCanRotate;
            DebrisObj.lifespanMin = LifeSpanMin;
            DebrisObj.lifespanMax = LifeSpanMax;
            DebrisObj.bounceCount = DebrisBounceCount;
            DebrisObj.angularVelocity = AngularVelocity;
            DebrisObj.angularVelocityVariance = AngularVelocityVariance;
            if (AudioEventName != null) { DebrisObj.audioEventName = AudioEventName; }
            if (BounceVFX != null) { DebrisObj.optionalBounceVFX = BounceVFX; }
            DebrisObj.sprite = tk2dsprite;
            DebrisObj.DoesGoopOnRest = DoesGoopOnRest;
            if (GoopType != null) { DebrisObj.AssignedGoop = GoopType; } else if (GoopType == null && DebrisObj.DoesGoopOnRest == true) { DebrisObj.DoesGoopOnRest = false; }
            DebrisObj.GoopRadius = GoopRadius;
            if (shadowSprite != null) { DebrisObj.shadowSprite = shadowSprite; }
            DebrisObj.inertialMass = Mass;

            return DebrisObj;
        }
    }
}
