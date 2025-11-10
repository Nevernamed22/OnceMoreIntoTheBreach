using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class SpawnDebrisOnDeath : BraveBehaviour
    {

        public void Start()
        {
            switch (triggerType)
            {
                case TriggerType.DEATH:
                    base.healthHaver.OnDeath += this.OnDeath;
                    break;
                case TriggerType.PRE_DEATH:
                    base.healthHaver.OnPreDeath += this.OnPreDeath;
                    break;
                case TriggerType.ANIM_EVENT:
                    if (base.spriteAnimator) { base.spriteAnimator.AnimationEventTriggered += HandleAnimationEvent; }
                    break;
            }
        }

        public override void OnDestroy()
        {
            switch (triggerType)
            {
                case TriggerType.DEATH:
                    base.healthHaver.OnDeath -= this.OnDeath;
                    break;
                case TriggerType.PRE_DEATH:
                    base.healthHaver.OnPreDeath -= this.OnPreDeath;
                    break;
                case TriggerType.ANIM_EVENT:
                    if (base.spriteAnimator) { base.spriteAnimator.AnimationEventTriggered -= HandleAnimationEvent; }
                    break;
            }
            base.OnDestroy();
        }
        private void HandleAnimationEvent(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frameNum)
        {
            if (clip.GetFrame(frameNum).eventInfo == triggerEventName)
            {
                SpawnDebris(Vector2.right);
            }
        }
        public void OnDeath(Vector2 finalDamageDirection)
        {
            SpawnDebris(finalDamageDirection);
        }
        public void OnPreDeath(Vector2 finalDamageDirection)
        {
            SpawnDebris(finalDamageDirection);
        }

        public void SpawnDebris(Vector2 dir)
        {
            if (base.aiActor.IsFalling && !triggerOnFalling)
            {
                return;
            }
            Vector2 position = base.gameObject.transform.position;

            switch (positionToSpawn)
            {
                case SpawnPosition.TRANSFORM:
                    position = base.gameObject.transform.position;
                    break;
                case SpawnPosition.RIGID_BODY_TOPCENTER:
                    if (base.specRigidbody) { position = base.specRigidbody.UnitTopCenter; }
                    break;
                case SpawnPosition.RIGID_BODY_CENTER:
                    if (base.specRigidbody) { position = base.specRigidbody.UnitCenter; }
                    break;
                case SpawnPosition.SPRITE_CENTER:
                    if (base.sprite) { position = base.sprite.WorldCenter; }
                    break;
                case SpawnPosition.SPRITE_TOPCENTER:
                    if (base.sprite) { position = base.sprite.WorldTopCenter; }
                    break;
            }

            if (debris != null)
            {
                GameObject gameObject = SpawnManager.SpawnDebris(debrisObject, position, Quaternion.identity);
                DebrisObject component = gameObject.GetComponent<DebrisObject>();
                if (component)
                {
                    component.Trigger(dir.normalized * debrisForce, 1f, 1f);
                }
            }

            if (doShards && shardClusters != null && shardClusters.Length > 0)
            {
                float minMagnitude = 1f;
                float maxMagnitude = 2f;
                float minAngle = -180f;
                float maxAngle = 180f;

                switch (shardSpawnType)
                {
                    case ShardSpawnType.CONE:
                        minAngle = -45f;
                        maxAngle = 45f;
                        minMagnitude = dir.magnitude * 0.5f;
                        maxMagnitude = dir.magnitude * 1.5f;
                        break;
                    case ShardSpawnType.JET:
                        minAngle = -15f;
                        maxAngle = 15f;
                        minMagnitude = dir.magnitude * 0.5f;
                        maxMagnitude = dir.magnitude * 1.5f;
                        break;
                }

                int num = UnityEngine.Random.Range(0, 10);
                for (int i = 0; i < this.shardClusters.Length; i++)
                {
                    ShardCluster shardCluster = this.shardClusters[i];
                    int num2 = UnityEngine.Random.Range(shardCluster.minFromCluster, shardCluster.maxFromCluster + 1);
                    int num3 = UnityEngine.Random.Range(0, shardCluster.clusterObjects.Length);
                    for (int j = 0; j < num2; j++)
                    {
                        float lowDiscrepancyRandom = BraveMathCollege.GetLowDiscrepancyRandom(num);
                        num++;
                        float num4 = Mathf.Lerp(minAngle, maxAngle, lowDiscrepancyRandom);
                        Vector3 vector = Quaternion.Euler(0f, 0f, num4) * (dir.normalized * UnityEngine.Random.Range(minMagnitude, maxMagnitude)).ToVector3ZUp(shardVerticalSpeed);
                        int num5 = (num3 + j) % shardCluster.clusterObjects.Length;
                        GameObject gameObject = SpawnManager.SpawnDebris(shardCluster.clusterObjects[num5].gameObject, position, Quaternion.identity);
                        tk2dSprite component = gameObject.GetComponent<tk2dSprite>();
                        if (base.sprite.attachParent != null && component != null)
                        {
                            component.attachParent = base.sprite.attachParent;
                            component.HeightOffGround = base.sprite.HeightOffGround;
                        }
                        DebrisObject component2 = gameObject.GetComponent<DebrisObject>();
                        vector = Vector3.Scale(vector, shardCluster.forceAxialMultiplier) * shardCluster.forceMultiplier;
                        component2.Trigger(vector * debrisForce, base.sprite.HeightOffGround + shardObjectHeightOffset, shardCluster.rotationMultiplier);
                    }
                }
            }
        }

        public enum SpawnPosition
        {
            RIGID_BODY_CENTER,
            RIGID_BODY_TOPCENTER,
            TRANSFORM,
            SPRITE_CENTER,
            SPRITE_TOPCENTER
        }
        public enum ShardSpawnType
        {
            CONE,
            BURST,
            JET
        }
        public enum TriggerType
        {
            PRE_DEATH,
            DEATH,
            ANIM_EVENT
        }
        public SpawnPosition positionToSpawn = SpawnPosition.TRANSFORM;
        public TriggerType triggerType = TriggerType.PRE_DEATH;

        public GameObject debrisObject;
        public float debrisForce = 5f;

        public bool doShards = false;
        public ShardCluster[] shardClusters = new ShardCluster[] {};
        public Vector2 shardSpawnOffset = Vector2.zero;
        public ShardSpawnType shardSpawnType = ShardSpawnType.BURST;
        public float shardVerticalSpeed = 1.5f;
        public float shardObjectHeightOffset = 0f;

        public bool triggerOnFalling = false;
        public string triggerEventName = "debris";


    }
}
