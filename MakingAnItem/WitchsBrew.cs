using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using ItemAPI;
using SaveAPI;

namespace NevernamedsItems
{
    public class WitchsBrew : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Witches Brew";
            string resourceName = "NevernamedsItems/Resources/witchsbrew_icon";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<WitchsBrew>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Fire Burn and Cauldron Bubble";
            string longDesc = "Doubles all enemies, but makes them much weaker."+"\n\nNot suitable for vegans or vegetarians.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");
            item.quality = PickupObject.ItemQuality.A;
            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.CHALLENGE_TOILANDTROUBLE_BEATEN, true);
        }
        public void AIActorMods(AIActor target)
        {
            if (target && target.healthHaver && !target.healthHaver.IsBoss && !target.healthHaver.IsSubboss && !target.IsSecretlyTheMineFlayer())
            {
                if (target.GetComponent<CompanionController>() == null && target.GetComponent<HasBeenDoubledByWitchsBrew>() == null)
                {
                    string guid = target.EnemyGuid;
                    var enemyPrefab = EnemyDatabase.GetOrLoadByGuid(guid);
                    AIActor aiactor = AIActor.Spawn(enemyPrefab, target.gameActor.CenterPosition.ToIntVector2(VectorConversions.Floor), target.GetAbsoluteParentRoom(), true, AIActor.AwakenAnimationType.Default, true);

                    HasBeenDoubledByWitchsBrew challengitude = aiactor.gameObject.AddComponent<HasBeenDoubledByWitchsBrew>();
                    challengitude.linkedOther = target;
                    HasBeenDoubledByWitchsBrew challengitude2 = target.gameObject.AddComponent<HasBeenDoubledByWitchsBrew>();
                    challengitude2.linkedOther = aiactor;
                    aiactor.procedurallyOutlined = false;
                    target.procedurallyOutlined = false;
                    aiactor.AssignedCurrencyToDrop = target.AssignedCurrencyToDrop;
                    aiactor.AdditionalSafeItemDrops = target.AdditionalSafeItemDrops;
                    aiactor.AdditionalSimpleItemDrops = target.AdditionalSimpleItemDrops;
                    aiactor.CanTargetEnemies = target.CanTargetEnemies;
                    aiactor.CanTargetPlayers = target.CanTargetPlayers;
                    if (target.IsInReinforcementLayer) aiactor.HandleReinforcementFallIntoRoom(0f);
                    if (aiactor.EnemyGuid == "22fc2c2c45fb47cf9fb5f7b043a70122")
                    {
                        aiactor.CollisionDamage = 0f;
                        aiactor.specRigidbody.AddCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.PlayerHitBox));
                        aiactor.specRigidbody.AddCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.Projectile));
                    }
                    else if (aiactor.EnemyGuid == "249db525a9464e5282d02162c88e0357")
                    {
                        if (aiactor.gameObject.GetComponent<SpawnEnemyOnDeath>())
                        {
                            Destroy(aiactor.gameObject.GetComponent<SpawnEnemyOnDeath>());
                        }
                    }
                    ModifyHP(target.healthHaver);
                    ModifyHP(aiactor.healthHaver);

                    //Shrinky
                    int cachedLayer = aiactor.gameObject.layer;
                    int cachedOutlineLayer = cachedLayer;
                    aiactor.gameObject.layer = LayerMask.NameToLayer("Unpixelated");
                    cachedOutlineLayer = SpriteOutlineManager.ChangeOutlineLayer(aiactor.sprite, LayerMask.NameToLayer("Unpixelated"));
                    aiactor.EnemyScale = TargetScale;
                    aiactor.gameObject.layer = cachedLayer;
                    SpriteOutlineManager.ChangeOutlineLayer(aiactor.sprite, cachedOutlineLayer);


                    int cachedLayer2 = target.gameObject.layer;
                    int cachedOutlineLayer2 = cachedLayer2;
                    target.gameObject.layer = LayerMask.NameToLayer("Unpixelated");
                    cachedOutlineLayer2 = SpriteOutlineManager.ChangeOutlineLayer(target.sprite, LayerMask.NameToLayer("Unpixelated"));
                    target.EnemyScale = TargetScale;
                    target.gameObject.layer = cachedLayer2;
                    SpriteOutlineManager.ChangeOutlineLayer(target.sprite, cachedOutlineLayer2);


                    aiactor.specRigidbody.Reinitialize();
                }
            }
        }
        private void ModifyHP(HealthHaver hp)
        {
            float startMax = hp.GetMaxHealth();
            hp.SetHealthMaximum(startMax * 0.25f);
            hp.ForceSetCurrentHealth(startMax * 0.25f);
        }
        public static Vector2 TargetScale = new Vector2(0.75f, 0.75f);
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            ETGMod.AIActor.OnPreStart += AIActorMods;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            ETGMod.AIActor.OnPreStart -= AIActorMods;
            return debrisObject;
        }
        protected override void OnDestroy()
        {
            ETGMod.AIActor.OnPreStart -= AIActorMods;
            base.OnDestroy();
        }
        public class HasBeenDoubledByWitchsBrew : MonoBehaviour
        {
            public HasBeenDoubledByWitchsBrew()
            {
                linkedOther = null;
            }
            private void Start()
            {
                self = base.GetComponent<AIActor>();
                if (self && self.specRigidbody)
                {
                    self.specRigidbody.OnPreRigidbodyCollision += this.OnPreCollide;
                }
            }
            private void OnPreCollide(SpeculativeRigidbody myRigidbody, PixelCollider myCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherCollider)
            {
                if (linkedOther != null & linkedOther.specRigidbody != null)
                {
                    if (otherRigidbody == linkedOther.specRigidbody)
                    {
                        PhysicsEngine.SkipCollision = true;
                    }
                }
            }
            private AIActor self;
            public AIActor linkedOther;
        }
    }
}