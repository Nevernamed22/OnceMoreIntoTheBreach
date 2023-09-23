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
    public class AmethystBracelet : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<AmethystBracelet>(
            "Amethyst Bracelet",
            "Thrown Guns Hunt",
            "This shimmering brace was once clasped around the wrist of Artemissile, goddess of the hunt." + "\n\nIt imbues thrown weaponry with that same hunter instinct.",
            "amethystbracelet_icon");
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.ThrownGunDamage, 1.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            item.quality = PickupObject.ItemQuality.D;
            ID = item.PickupObjectId;
        }
        public static int ID;
        private void HandleReturnLikeBoomerang(DebrisObject obj)
        {
            obj.PreventFallingInPits = true;
            obj.OnGrounded = (Action<DebrisObject>)Delegate.Remove(obj.OnGrounded, new Action<DebrisObject>(this.HandleReturnLikeBoomerang));

            PickupMover pickupMover = obj.gameObject.AddComponent<PickupMover>();
            if (pickupMover.specRigidbody) { pickupMover.specRigidbody.CollideWithTileMap = false; }
            pickupMover.minRadius = 1f;
            pickupMover.moveIfRoomUnclear = true;
            pickupMover.stopPathingOnContact = false;
        }
        private void PostProcessThrownGun(Projectile thrownGunProjectile)
        {
            thrownGunProjectile.OnHitEnemy += this.OnHitEnemy;
            thrownGunProjectile.gameObject.AddComponent<PierceProjModifier>();
            thrownGunProjectile.gameObject.AddComponent<PierceDeadActors>();
            HomingModifier homing = thrownGunProjectile.gameObject.AddComponent<HomingModifier>();
            homing.HomingRadius = 100;
            homing.AngularVelocity = 2000f;

            thrownGunProjectile.pierceMinorBreakables = true;
            thrownGunProjectile.IgnoreTileCollisionsFor(0.01f);
            thrownGunProjectile.OnBecameDebrisGrounded += HandleReturnLikeBoomerang;

        }
        private void OnHitEnemy(Projectile projectile, SpeculativeRigidbody arg2, bool fatal)
        {
            if (fatal && Owner && Owner.PlayerHasActiveSynergy("Artemisfire"))
            {
                Gun gun = projectile.GetComponentInChildren<Gun>();
                if (gun)
                {
                    Projectile proj = null;
                    if (gun.DefaultModule != null)
                    {
                        if (gun.DefaultModule.projectiles.Count > 0 && gun.DefaultModule.projectiles[0] != null) { proj = gun.DefaultModule.projectiles[0]; }
                        else if (gun.DefaultModule.chargeProjectiles.Count > 0 && gun.DefaultModule.chargeProjectiles[0] != null) { proj = gun.DefaultModule.chargeProjectiles[0].Projectile; }
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        Projectile inst = null;
                        if (proj)
                        {
                            if (proj.GetComponent<BeamController>() != null)
                            {
                                BeamController bem = BeamAPI.FreeFireBeamFromAnywhere(proj, Owner, null, projectile.LastPosition, UnityEngine.Random.insideUnitCircle.ToAngle(), UnityEngine.Random.Range(1f, 3f), true, false, 0);
                                if (bem && bem.projectile) { inst = bem.projectile; }
                            }
                            else
                            {
                                inst = proj.InstantiateAndFireInDirection(projectile.LastPosition, UnityEngine.Random.insideUnitCircle.ToAngle()).GetComponent<Projectile>();
                                Owner.DoPostProcessProjectile(inst);
                            }
                        }
                        if (inst != null)
                        {
                            inst.baseData.damage *= Owner.stats.GetStatValue(PlayerStats.StatType.Damage);
                            inst.baseData.speed *= Owner.stats.GetStatValue(PlayerStats.StatType.ProjectileSpeed);
                            inst.baseData.range *= Owner.stats.GetStatValue(PlayerStats.StatType.RangeMultiplier);
                            inst.baseData.force *= Owner.stats.GetStatValue(PlayerStats.StatType.KnockbackMultiplier);

                        }
                    }
                }
            }
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessThrownGun += this.PostProcessThrownGun;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            player.PostProcessThrownGun -= this.PostProcessThrownGun;
            return base.Drop(player);
        }
        public override void OnDestroy()
        {
            if (Owner) Owner.PostProcessThrownGun -= this.PostProcessThrownGun;
            base.OnDestroy();
        }
    }
}