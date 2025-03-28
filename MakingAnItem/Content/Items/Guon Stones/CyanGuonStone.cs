using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Reflection;
using Alexandria.Misc;
using MonoMod.RuntimeDetour;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    class CyanGuonStone : AdvancedPlayerOrbitalItem
    {
        public static void Init()
        {
            AdvancedPlayerOrbitalItem item = ItemSetup.NewItem<CyanGuonStone>(
            "Cyan Guon Stone",
            "Slow and Steady",
            "Targets enemies when you stand still." + "\n\nThis rock is inhabited by a powerful spirit of lethargy.",
            "cyanguon_icon") as AdvancedPlayerOrbitalItem;
            item.quality = PickupObject.ItemQuality.A;
            item.SetTag("guon_stone");


            item.OrbitalPrefab = ItemSetup.CreateOrbitalObject("Cyan Guon Stone", "cyanguon_ingame", new IntVector2(8, 8), new IntVector2(-4, -4)).GetComponent<PlayerOrbital>();

            item.HasAdvancedUpgradeSynergy = true;
            item.AdvancedUpgradeSynergy = "Cyaner Guon Stone";
            item.AdvancedUpgradeOrbitalPrefab = ItemSetup.CreateOrbitalObject("Cyaner Guon Stone", "cyanguon_synergy", new IntVector2(12, 12), new IntVector2(-6, -6), perfectOrbitalFactor: 10);

            Projectile proj = ProjectileSetupUtility.MakeProjectile(86, 2, 1000, 50);
            proj.SetProjectileSprite("cyanguon_proj", 5, 5, true, tk2dBaseSprite.Anchor.MiddleCenter, 5, 5);
            cyanGuonProj = proj;
        }
        public static Projectile cyanGuonProj;

        bool canFire = true;
        public override void Update()
        {
            if (this.m_extantOrbital != null && Owner && Owner.specRigidbody)
            {
                if (Owner && Owner.IsInCombat && Owner.specRigidbody.Velocity == Vector2.zero && canFire)
                {
                    tk2dSprite OrbitalSprite = this.m_extantOrbital.GetComponent<tk2dSprite>();
                    if (OrbitalSprite)
                    {
                        AIActor nearestEnemy = OrbitalSprite.WorldCenter.GetNearestEnemyToPosition(true, Dungeonator.RoomHandler.ActiveEnemyType.All, null, null);
                        if (nearestEnemy != null)
                        {
                            GameObject gameObject = SpawnManager.SpawnProjectile(cyanGuonProj.gameObject, OrbitalSprite.WorldCenter, Quaternion.Euler(0f, 0f, OrbitalSprite.WorldCenter.CalculateVectorBetween(nearestEnemy.CenterPosition).ToAngle()), true);
                            Projectile component = gameObject.GetComponent<Projectile>();
                            if (component != null)
                            {
                                component.Owner = Owner;
                                component.Shooter = Owner.specRigidbody;
                                component.baseData.damage *= Owner.stats.GetStatValue(PlayerStats.StatType.Damage);
                                component.baseData.speed *= Owner.stats.GetStatValue(PlayerStats.StatType.ProjectileSpeed);
                                component.baseData.force *= Owner.stats.GetStatValue(PlayerStats.StatType.KnockbackMultiplier);
                                component.AdditionalScaleMultiplier *= Owner.stats.GetStatValue(PlayerStats.StatType.PlayerBulletScale);
                                component.UpdateSpeed();
                                Owner.DoPostProcessProjectile(component);
                            }
                            //component.ReAimBulletToNearestEnemy(100, 0);
                            canFire = false;
                            float cooldownTime = 0.35f;
                            if (Owner.PlayerHasActiveSynergy("Cyaner Guon Stone")) cooldownTime = 0.16f;
                            Invoke("resetFireCooldown", cooldownTime);
                        }
                    }
                }
            }
            base.Update();
        }
        private void resetFireCooldown() { canFire = true; }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            return base.Drop(player);
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}