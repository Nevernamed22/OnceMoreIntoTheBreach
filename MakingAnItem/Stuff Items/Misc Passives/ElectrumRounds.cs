using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using ItemAPI;
using UnityEngine;
using MonoMod.RuntimeDetour;
using System.Reflection;
using SaveAPI;
using Gungeon;

namespace NevernamedsItems
{
    public class ElectrumRounds : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Electrum Rounds";
            string resourceName = "NevernamedsItems/Resources/NeoItemSprites/electrumrounds_icon";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<ElectrumRounds>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Zip Zop Zap";
            string longDesc = "Fast, penetrative bullets made of gold and silver alloy. Highly conductive, it maintains a powerful electric bond with it's home holster.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");
            item.quality = PickupObject.ItemQuality.S;
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.ProjectileSpeed, 2, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.AdditionalShotPiercing, 1, StatModifier.ModifyMethod.ADDITIVE);
        }

        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {
            GameObject LinkVFXPrefab = FakePrefab.Clone(Game.Items["shock_rounds"].GetComponent<ComplexProjectileModifier>().ChainLightningVFX);
            FakePrefab.MarkAsFakePrefab(LinkVFXPrefab);
            UnityEngine.Object.DontDestroyOnLoad(LinkVFXPrefab);
            OwnerConnectLightningModifier litening = projectile.gameObject.AddComponent<OwnerConnectLightningModifier>();
            litening.linkPrefab = LinkVFXPrefab;
        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.PostProcessProjectile -= this.PostProcessProjectile;


            return debrisObject;
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += this.PostProcessProjectile;
        }
        protected override void OnDestroy()
        {
            if (Owner)
            {
                Owner.PostProcessProjectile -= this.PostProcessProjectile;
            }
            base.OnDestroy();
        }
    }
}
