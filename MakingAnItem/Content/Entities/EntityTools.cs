using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alexandria.ItemAPI;
using Brave.BulletScript;
using HutongGames.PlayMaker.Actions;
using UnityEngine;

namespace NevernamedsItems
{
    public static class EntityTools
    {

        public static AIShooter AddAIShooter(this AIActor actor, Vector2 GunAttachPoint, int gunID = 38, bool shouldReload = true, float customCooldown = 2.5f, float aimTimeScale = 1f, bool handHasSprite = true, string customSpriteName = null, tk2dSpriteCollectionData spriteCollection = null)
        {
            string entityName = (actor && !string.IsNullOrEmpty(actor.ActorName)) ? actor.ActorName : "ENTITY_NAME_MISSING";

            GameObject gunAttach = (actor.transform.Find("GunAttachPoint") != null) ? actor.transform.Find("GunAttachPoint").gameObject : new GameObject("GunAttachPoint");
            gunAttach.transform.SetParent(actor.transform);
            gunAttach.transform.localPosition = new Vector3(GunAttachPoint.x, GunAttachPoint.y, 0f);

            AIShooter shooter = actor.gameObject.GetOrAddComponent<AIShooter>();
            shooter.AimTimeScale = aimTimeScale;
            shooter.gunAttachPoint = gunAttach.transform;
            shooter.equippedGunId = gunID;
            shooter.shouldUseGunReload = shouldReload;
            shooter.customShootCooldownPeriod = customCooldown;

            if (!string.IsNullOrEmpty(customSpriteName) && handHasSprite && spriteCollection)
            {
                GameObject hand = ItemBuilder.SpriteFromBundle(customSpriteName, spriteCollection.GetSpriteIdByName(customSpriteName), spriteCollection, new GameObject($"{entityName}Hand"));
                hand.MakeFakePrefab();
                tk2dBaseSprite handSprite = hand.GetComponent<tk2dBaseSprite>();
                handSprite.HeightOffGround = 0.15f;
                handSprite.renderLayer = 0;
                handSprite.automaticallyManagesDepth = false;
                handSprite.renderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
                handSprite.renderer.material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive | MaterialGlobalIlluminationFlags.EmissiveIsBlack;
                handSprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitCutoutUber");
                handSprite.renderer.material.shaderKeywords = new string[] { "BRIGHTNESS_CLAMP_ON", "EMISSIVE_OFF", "PALETTE_OFF", "TINTING_ON" };
                PlayerHandController handController = hand.AddComponent<PlayerHandController>();
                handController.handHeightFromGun = 0.05f;
                handController.ForceRenderersOff = false;
                handController.IsPlayerPrimary = false;
                shooter.handObject = handController;
            }
            else
            {
                shooter.handObject = EnemyDatabase.GetOrLoadByGuid(handHasSprite ? "01972dee89fc4404a5c408d50007dad5" : "128db2f0781141bcb505d8f00f9e4d47").aiShooter.handObject;
            }
            return shooter;
        }
        public static void AddFootstepSounds(this AIAnimator animator, int[] frames, string eventAudio = "Play_FS_ENM")
        {
            string entityName = (animator && animator.gameActor && !string.IsNullOrEmpty(animator.gameActor.ActorName)) ? animator.gameActor.ActorName : "ENTITY_NAME_MISSING";
            foreach (string animName in animator.MoveAnimation.AnimNames)
            {
                foreach (int targetFrame in frames)
                {
                    if (animator.spriteAnimator.GetClipByName(animName) == null)
                    {
                        string err = $"Error in AddFootstepSounds for {entityName}: The string '{animName}' in the given animator's movement animations does not correspond to a real animation.";
                        ETGModConsole.Log($"<color=#ff0000ff>{err}</color>");
                        Debug.LogError(err);
                    }
                    if (animator.spriteAnimator.GetClipByName(animName).frames.Length - 1 >= targetFrame)
                    {
                        tk2dSpriteAnimationFrame footstepFrame = animator.spriteAnimator.GetClipByName(animName).frames[targetFrame];
                        footstepFrame.triggerEvent = true;
                        if (string.IsNullOrEmpty(footstepFrame.eventInfo)) { footstepFrame.eventInfo = "footstep"; }
                        footstepFrame.eventAudio = "Play_FS_ENM";
                    }
                    else
                    {
                        string err = $"Error in AddFootstepSounds for {entityName}: The animation '{animName}' in the given animator's movement animations has a length ({animator.spriteAnimator.GetClipByName(animName).frames.Length - 1}) that is less than the given footstep frame {targetFrame}.";
                        ETGModConsole.Log($"<color=#ff0000ff>{err}</color>");
                        Debug.LogError(err);
                    }
                }
            }
        }
        public static KnockbackDoer SetupEntityKnockback(GameObject entity, float weight = 50, float knockbackMultiplier = 1, float deathMultiplier = 5, bool shouldBounce = true, float collisionDecay = 0.5f, float timeScalar = 1f)
        {
            KnockbackDoer knockback = entity.AddComponent<KnockbackDoer>();
            knockback.weight = weight;
            knockback.shouldBounce = shouldBounce;
            knockback.knockbackMultiplier = knockbackMultiplier;
            knockback.deathMultiplier = deathMultiplier;
            knockback.collisionDecay = collisionDecay;
            knockback.timeScalar = timeScalar;
            return knockback;
        }
        public static GameObject SetupEntityObject(string name, string defaultSprite, tk2dSpriteCollectionData collection)
        {
            var entity = ItemBuilder.SpriteFromBundle(defaultSprite, collection.GetSpriteIdByName(defaultSprite), collection, new GameObject(name));

            tk2dBaseSprite baseSprite = entity.GetComponent<tk2dBaseSprite>();
            baseSprite.tag = "Enemy";
            baseSprite.renderLayer = 0;
            baseSprite.renderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            baseSprite.renderer.sortingOrder = 0;
            baseSprite.renderer.material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive | MaterialGlobalIlluminationFlags.EmissiveIsBlack;
            baseSprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitCutoutUber");
            baseSprite.renderer.material.shaderKeywords = new string[] { "BRIGHTNESS_CLAMP_ON", "EMISSIVE_OFF", "PALETTE_OFF", "TINTING_ON" };

            return entity;
        }

        public static SpeculativeRigidbody SetupEntityRigidBody(GameObject entity, List<CollisionLayer> collisionLayers, List<IntVector2> colliderDimensions, List<IntVector2> colliderOffsets,
            bool collideWithOthers = true, bool collideWithTilemap = true, bool canBeCarried = true, bool canBePushed = false, bool canCarry = false, bool canPush = false, bool capVelocity = false)
        {
            SpeculativeRigidbody rigidBody = entity.GetOrAddComponent<SpeculativeRigidbody>();
            rigidBody.PixelColliders = new List<PixelCollider>();

            for (int i = 0; i < collisionLayers.Count(); i++)
            {
                rigidBody.PixelColliders.Add(new PixelCollider
                {
                    ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
                    CollisionLayer = collisionLayers[i],
                    IsTrigger = false,
                    BagleUseFirstFrameOnly = false,
                    SpecifyBagelFrame = string.Empty,
                    BagelColliderNumber = 0,
                    ManualOffsetX = colliderOffsets[i].x,
                    ManualOffsetY = colliderOffsets[i].y,
                    ManualWidth = colliderDimensions[i].x,
                    ManualHeight = colliderDimensions[i].y,
                    ManualDiameter = 0,
                    ManualLeftX = 0,
                    ManualLeftY = 0,
                    ManualRightX = 0,
                    ManualRightY = 0,
                });
            }
            rigidBody.CanBeCarried = canBeCarried;
            rigidBody.CanBePushed = canBePushed;
            rigidBody.CanCarry = canCarry;
            rigidBody.CanPush = canPush;
            rigidBody.CapVelocity = capVelocity;
            rigidBody.CollideWithOthers = collideWithOthers;
            rigidBody.CollideWithTileMap = collideWithTilemap;
            return rigidBody;
        }
        public static GameObject BuildEntity(string name, string guid, string defaultSprite, tk2dSpriteCollectionData Collection, IntVector2 colliderDimensions, IntVector2 colliderOffsets)
        {
            if (!CompanionBuilder.companionDictionary.ContainsKey(guid))
            {
                GameObject gameObject = ItemBuilder.SpriteFromBundle(defaultSprite, Collection.GetSpriteIdByName(defaultSprite), Collection, new GameObject(name));
                gameObject.AddComponent<ObjectVisibilityManager>();
                gameObject.name = name;

                tk2dSprite sprite = gameObject.GetComponent<tk2dSprite>();
                sprite.collection = Collection;

                SpeculativeRigidbody rigidbody = gameObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(colliderOffsets, colliderDimensions);
                rigidbody.CanBeCarried = true;
                rigidbody.CanBePushed = false;
                rigidbody.CanCarry = false;
                rigidbody.CanPush = false;
                rigidbody.CapVelocity = false;
                rigidbody.CollideWithOthers = true;
                rigidbody.CollideWithTileMap = true;

                HealthHaver health = gameObject.AddComponent<HealthHaver>();
                health.PreventAllDamage = true;
                health.SetHealthMaximum(15000f, null, false);
                health.FullHeal();

                AIActor actor = gameObject.AddComponent<AIActor>();
                actor.State = AIActor.ActorState.Normal;
                actor.EnemyGuid = guid;

                KnockbackDoer knockbackDoer = gameObject.AddComponent<KnockbackDoer>();
                knockbackDoer.weight = 35;

                tk2dSpriteAnimator spriteAnimator = gameObject.AddComponent<tk2dSpriteAnimator>();
                spriteAnimator.RegenerateCache();

                AIAnimator aiAnimator = gameObject.AddComponent<AIAnimator>();


                BehaviorSpeculator behaviour = gameObject.AddComponent<BehaviorSpeculator>();
                behaviour.MovementBehaviors = new List<MovementBehaviorBase>();
                behaviour.AttackBehaviors = new List<AttackBehaviorBase>();
                behaviour.TargetBehaviors = new List<TargetBehaviorBase>();
                behaviour.OverrideBehaviors = new List<OverrideBehaviorBase>();
                behaviour.OtherBehaviors = new List<BehaviorBase>();

                EnemyDatabaseEntry item = new EnemyDatabaseEntry
                {
                    myGuid = guid,
                    placeableWidth = 2,
                    placeableHeight = 2,
                    isNormalEnemy = false
                };
                EnemyDatabase.Instance.Entries.Add(item);
                CompanionBuilder.companionDictionary.Add(guid, gameObject);

                gameObject.MakeFakePrefab();
                return gameObject;
            }
            return null;
        }
    }
}
