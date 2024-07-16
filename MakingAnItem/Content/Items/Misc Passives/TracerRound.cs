using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;
using SaveAPI;
using Alexandria.Misc;

namespace NevernamedsItems
{
    public class TracerRound : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<TracerRound>(
             "Tracer Rounds",
             "Follow The Red Line",
             "Shots have a chance to leave a trail of fire, marking their exact trajectory." + "\n\nStandard issue for military training exercises, weapons tests, and really bad assassins.",
             "tracerrounds_improved");
            item.AddToSubShop(ItemBuilder.ShopType.Trorc);
            item.quality = PickupObject.ItemQuality.D;
            item.SetTag("bullet_modifier");
            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.PURCHASED_TRACERROUNDS, true);
            item.AddItemToTrorcMetaShop(8);
            Doug.AddToLootPool(item.PickupObjectId);

            ID = item.PickupObjectId;
        }
        public static int ID;
        public void onFired(Projectile bullet, float eventchancescaler)
        {
            if (!Owner.HasPickupID(GracefulGoop.ID))
            {
                if (UnityEngine.Random.value < (0.1f * eventchancescaler) || Owner.PlayerHasActiveSynergy("Ring of Fire"))
                {
                    GoopModifier fireTrail = bullet.gameObject.AddComponent<GoopModifier>();
                    fireTrail.InFlightSpawnRadius = Owner.PlayerHasActiveSynergy("Even More Visible!") ? 1 : 0.5f;
                    fireTrail.SpawnGoopInFlight = true;
                    fireTrail.InFlightSpawnFrequency = 0.01f;
                    fireTrail.goopDefinition = Owner.PlayerHasActiveSynergy("Hot Tempered") ? GoopUtility.GreenFireDef : GoopUtility.FireDef;

                    bullet.baseData.speed *= 1.25f;
                    bullet.OnDestruction += OnProjectileDeath;
                }
            }
        }
        private void OnProjectileDeath(Projectile self)
        {
            UnityEngine.Object.Instantiate<GameObject>((PickupObjectDatabase.GetById((Owner != null && Owner.PlayerHasActiveSynergy("Hot Tempered")) ? 722 : 336) as Gun).DefaultModule.projectiles[0].hitEffects.overrideMidairDeathVFX, self.LastPosition, Quaternion.identity);
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
                shrink.timeToChangeOver = 0.165f;
                proj.IgnoreTileCollisionsFor(0.1f);

                GoopModifier fireTrail = spawned.gameObject.AddComponent<GoopModifier>();
                fireTrail.InFlightSpawnRadius = Owner.PlayerHasActiveSynergy("Even More Visible!") ? 1 : 0.5f;
                fireTrail.SpawnGoopInFlight = true;
                fireTrail.InFlightSpawnFrequency = 0.05f;
                fireTrail.goopDefinition = (Owner != null && Owner.PlayerHasActiveSynergy("Hot Tempered")) ? GoopUtility.GreenFireDef : GoopUtility.FireDef;
            }
        }
        public override void Pickup(PlayerController player)
        {
            player.PostProcessProjectile += this.onFired;
            base.Pickup(player);
        }
        public override void DisableEffect(PlayerController player)
        {
            if (player) { player.PostProcessProjectile -= this.onFired; }
            base.DisableEffect(player);
        }

        private int currentItems, lastItems;
        private int currentGuns, lastGuns;
        public override void Update()
        {
            if (Owner)
            {
                CalculateStats(Owner);
            }

            else { return; }
        }
        private void CalculateStats(PlayerController player)
        {
            currentItems = player.passiveItems.Count;
            currentGuns = player.inventory.AllGuns.Count;
            if (currentItems != lastItems || currentGuns != lastGuns)
            {
                bool deservesFireimmunity = false;
                if (Owner.HasPickupID(481) || Owner.HasPickupID(275) || Owner.HasPickupID(661)) deservesFireimmunity = true;
                HandleFireImmunity(deservesFireimmunity);
                lastItems = currentItems;
                lastGuns = currentGuns;
            }
        }
        private void HandleFireImmunity(bool shouldGiveFireImmunity)
        {
            if (shouldGiveFireImmunity)
            {
                this.m_fireImmunity = new DamageTypeModifier();
                this.m_fireImmunity.damageMultiplier = 0f;
                this.m_fireImmunity.damageType = CoreDamageTypes.Fire;
                Owner.healthHaver.damageTypeModifiers.Add(this.m_fireImmunity);
            }
            else
            {
                Owner.healthHaver.damageTypeModifiers.Remove(this.m_fireImmunity);
            }
        }

        private DamageTypeModifier m_fireImmunity;
    }
}