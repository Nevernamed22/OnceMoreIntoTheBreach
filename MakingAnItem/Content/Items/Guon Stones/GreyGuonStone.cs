using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Reflection;
using MonoMod.RuntimeDetour;
using Alexandria.ItemAPI;
using Alexandria.Misc;

namespace NevernamedsItems
{
    class GreyGuonStone : AdvancedPlayerOrbitalItem
    {
        public static PlayerOrbital upgradeOrbitalPrefab;
        public static void Init()
        {
            AdvancedPlayerOrbitalItem item = ItemSetup.NewItem<GreyGuonStone>(
            "Grey Guon Stone",
            "Vengeful Rock",
            "Any creature that harms this stone or its bearer shall be harmed in kind." + "\n\nBlood unto blood, as it has always been.",
            "greyguon_icon") as AdvancedPlayerOrbitalItem;
            item.quality = PickupObject.ItemQuality.C;

            item.OrbitalPrefab = ItemSetup.CreateOrbitalObject("Grey Guon Stone", "greyguon_animated_ingame1", new IntVector2(9, 9), new IntVector2(-4, -5), "greyguon_orbital").GetComponent<PlayerOrbital>();

            item.AddToSubShop(ItemBuilder.ShopType.Cursula);
            item.SetTag("guon_stone");

            item.HasAdvancedUpgradeSynergy = true;
            item.AdvancedUpgradeSynergy = "Greyer Guon Stone";
            item.AdvancedUpgradeOrbitalPrefab = ItemSetup.CreateOrbitalObject("Greyer Guon Stone", "greyguon_synergy", new IntVector2(12, 12), new IntVector2(-6, -6), perfectOrbitalFactor: 10);
        }
        private void OwnerHitByProjectile(Projectile incomingProjectile, PlayerController arg2)
        {
            if (incomingProjectile.Owner) { DealDamageToEnemy(incomingProjectile.Owner, 25f); }
        }
        private void DealDamageToEnemy(GameActor target, float damage)
        {
            if (target && target.healthHaver && !target.healthHaver.IsDead && Owner != null)
            {
                float finalDamage = damage * Owner.stats.GetStatValue(PlayerStats.StatType.Damage);
                if (Owner.PlayerHasActiveSynergy("Greyer Guon Stone")) finalDamage *= 2;
                if (target.aiActor && target.aiActor.IsBlackPhantom) finalDamage *= 3;
                target.healthHaver.ApplyDamage(finalDamage, Vector2.zero, "Guon Wrath", CoreDamageTypes.None, DamageCategory.Unstoppable, true, null, false);
            }
        }
        public override void OnOrbitalCreated(GameObject orbital)
        {
            SpeculativeRigidbody orbBody = orbital.GetComponent<SpeculativeRigidbody>();
            if (orbBody) orbBody.OnPreRigidbodyCollision += this.OnGuonHit;
            base.OnOrbitalCreated(orbital);
        }
        private void OnGuonHit(SpeculativeRigidbody myRigidbody, PixelCollider myCollider, SpeculativeRigidbody other, PixelCollider otherCollider)
        {
            Projectile component = other.GetComponent<Projectile>();
            if (component != null && component.Owner is AIActor) { DealDamageToEnemy(component.Owner, 5); }
        }
        public override void Pickup(PlayerController player)
        {
            player.OnHitByProjectile += this.OwnerHitByProjectile;
            base.Pickup(player);
        }
        public override void DisableEffect(PlayerController player)
        {
            if (player) player.OnHitByProjectile -= OwnerHitByProjectile;
            base.DisableEffect(player);
        }
    }
}

