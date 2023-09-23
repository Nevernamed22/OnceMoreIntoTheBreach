using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dungeonator;
using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.Misc;
using SaveAPI;

namespace NevernamedsItems
{
    public class GracefulGoop : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Graceful Goop";
            string resourceName = "NevernamedsItems/Resources/gracefulgoop_icon";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<GracefulGoop>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "They Have Died... Inside";
            string longDesc = "Bullets trail poison." + "\n\nBrewed (and probably drunk) by a tragic comedian on the brink.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");
            item.AddToSubShop(ItemBuilder.ShopType.Goopton);

            item.quality = PickupObject.ItemQuality.B;

            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.PURCHASED_GRACEFULGOOP, true);
            item.AddItemToGooptonMetaShop(25);
            ID = item.PickupObjectId;
        }
        public static int ID;
        public void onFired(Projectile bullet, float eventchancescaler)
        {
            GoopModifier poisonTrail = bullet.gameObject.AddComponent<GoopModifier>();
            poisonTrail.SpawnGoopInFlight = true;
            poisonTrail.SpawnGoopOnCollision = false;
            poisonTrail.InFlightSpawnRadius = 0.5f;
            poisonTrail.InFlightSpawnFrequency = 0.01f;
            poisonTrail.goopDefinition = GoopUtility.PoisonDef;
            if (Owner.PlayerHasActiveSynergy("Hot Tempered") && (UnityEngine.Random.value < 0.1f || Owner.PlayerHasActiveSynergy("Ring of Fire")))
            {
                poisonTrail.goopDefinition = GoopUtility.GreenFireDef;
                if (Owner.PlayerHasActiveSynergy("Even More Visible!")) poisonTrail.InFlightSpawnRadius = 1;

                bullet.baseData.speed *= 1.25f;
                bullet.UpdateSpeed();

                bullet.OnDestruction += OnProjectileDeath;
            }

            if (Owner.CurrentGun.PickupObjectId == 33) IsaacIsDeadLetsCrabDance();
        }
        private void OnProjectileDeath(Projectile self)
        {
            UnityEngine.Object.Instantiate<GameObject>((PickupObjectDatabase.GetById(Owner.PlayerHasActiveSynergy("Hot Tempered") ? 722 : 336) as Gun).DefaultModule.projectiles[0].hitEffects.overrideMidairDeathVFX, self.LastPosition, Quaternion.identity);
            for (int i = 0; i < 5; i++)
            {
                GameObject spawned = (PickupObjectDatabase.GetById(83) as Gun).DefaultModule.projectiles[0].InstantiateAndFireInDirection(self.LastPosition, UnityEngine.Random.Range(0, 360));
                Projectile proj = spawned.GetComponent<Projectile>();
                proj.baseData.damage = 3f;
                proj.AssignToPlayer(Owner);
                ScaleChangeOverTimeModifier shrink = spawned.AddComponent<ScaleChangeOverTimeModifier>();
                shrink.destroyAfterChange = true;
                shrink.scaleMultAffectsDamage = false;
                shrink.ScaleToChangeTo = 0.1f;
                shrink.suppressDeathFXIfdestroyed = true;
                shrink.timeToChangeOver = 0.5f;
                proj.IgnoreTileCollisionsFor(0.1f);

                GoopModifier fireTrail = spawned.gameObject.AddComponent<GoopModifier>();
                fireTrail.InFlightSpawnRadius = Owner.PlayerHasActiveSynergy("Even More Visible!") ? 1 : 0.5f;
                fireTrail.SpawnGoopInFlight = true;
                fireTrail.InFlightSpawnFrequency = 0.05f;
                fireTrail.goopDefinition = Owner.PlayerHasActiveSynergy("Hot Tempered") ? GoopUtility.GreenFireDef : GoopUtility.FireDef;
            }
        }
        private void IsaacIsDeadLetsCrabDance()
        {
            Owner.inventory.RemoveGunFromInventory(Owner.CurrentGun);
            IntVector2 bestRewardLocation = Owner.CurrentRoom.GetBestRewardLocation(IntVector2.One * 3, RoomHandler.RewardLocationStyle.PlayerCenter, true);
            Exploder.DoDefaultExplosion(Owner.sprite.WorldCenter, new Vector2());
            if (UnityEngine.Random.value < 0.5)
            {
                Chest Red_Chest = GameManager.Instance.RewardManager.A_Chest;
                Red_Chest.IsLocked = false;
                Chest.Spawn(Red_Chest, bestRewardLocation);
            }
            else if (UnityEngine.Random.value < 0.2)
            {
                Chest Black_Chest = GameManager.Instance.RewardManager.S_Chest;
                Black_Chest.IsLocked = true;
                Chest.Spawn(Black_Chest, bestRewardLocation);
            }
            else
            {
                Chest Synergy_Chest = GameManager.Instance.RewardManager.Synergy_Chest;
                Synergy_Chest.IsLocked = false;
                Chest.Spawn(Synergy_Chest, bestRewardLocation);
            }
        }

        private void onFiredBeam(BeamController sourceBeam)
        {
            if (sourceBeam && sourceBeam.gameObject && sourceBeam.gameObject.GetComponent<GoopModifier>() == null && sourceBeam.GetComponent<BasicBeamController>())
            {
                GoopModifier goop = sourceBeam.gameObject.AddComponent<GoopModifier>();
                goop.goopDefinition = GoopUtility.PoisonDef;
                goop.SpawnGoopInFlight = true;
                goop.InFlightSpawnRadius = 0.5f;
                goop.InFlightSpawnFrequency = 0.05f;
                goop.BeamEndRadius = 1f;
                goop.CollisionSpawnRadius = 3f;
                sourceBeam.GetComponent<BasicBeamController>().m_beamGoopModifier = goop;
            }
        }
        private DamageTypeModifier m_poisonImmunity;
        private void SpawnCasingSynergy(float damage, bool fatal, HealthHaver enemy)
        {
            if (enemy && enemy.aiActor && fatal)
            {
                if (Owner.HasPickupID(93) || Owner.HasPickupID(321) || Owner.HasPickupID(641) || Owner.HasPickupID(53) || Owner.HasPickupID(231) || Owner.HasPickupID(532) || Owner.HasPickupID(Gungeon.Game.Items["nn:golden_armour"].PickupObjectId) || Owner.HasPickupID(Gungeon.Game.Items["nn:gold_guon_stone"].PickupObjectId))
                {
                    LootEngine.SpawnItem(PickupObjectDatabase.GetById(68).gameObject, enemy.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
                }
            }
        }
        public override void Pickup(PlayerController player)
        {
            player.PostProcessProjectile += onFired;
            player.OnAnyEnemyReceivedDamage += SpawnCasingSynergy;
            player.PostProcessBeam += onFiredBeam;

            m_poisonImmunity = new DamageTypeModifier();
            m_poisonImmunity.damageMultiplier = 0f;
            m_poisonImmunity.damageType = CoreDamageTypes.Poison;
            player.healthHaver.damageTypeModifiers.Add(m_poisonImmunity);
            base.Pickup(player);
        }
        public override void DisableEffect(PlayerController player)
        {
            if (player)
            {
                player.OnAnyEnemyReceivedDamage -= SpawnCasingSynergy;
                player.PostProcessProjectile -= onFired;
                player.PostProcessBeam -= onFiredBeam;
                player.healthHaver.damageTypeModifiers.Remove(m_poisonImmunity);
            }
            base.DisableEffect(player);
        }
    }
}
