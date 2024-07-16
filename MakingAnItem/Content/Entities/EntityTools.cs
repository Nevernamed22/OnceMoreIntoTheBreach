using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alexandria.ItemAPI;
using UnityEngine;

namespace NevernamedsItems
{
    public static class EntityTools
    {
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
