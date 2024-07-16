using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    public class PearlBracelet : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<PearlBracelet>(
            "Pearl Bracelet",
            "Thrown Guns Afflict",
            "Thrown guns afflict a whole host of status effects, and return to their owner." + "\n\nPearls aren't really proper gemstones, but the people who make these are Wizards, not Geologists.",
            "pearlbracelet_icon");
            item.quality = PickupObject.ItemQuality.D;
            PearlBraceletID = item.PickupObjectId;
        }
        public static int PearlBraceletID;
        private void HandleReturnLikeBoomerang(DebrisObject obj)
        {
            obj.PreventFallingInPits = true;
            obj.OnGrounded = (Action<DebrisObject>)Delegate.Remove(obj.OnGrounded, new Action<DebrisObject>(this.HandleReturnLikeBoomerang));
            PickupMover pickupMover = obj.gameObject.AddComponent<PickupMover>();
            if (pickupMover.specRigidbody)
            {
                pickupMover.specRigidbody.CollideWithTileMap = false;
            }
            pickupMover.minRadius = 1f;
            pickupMover.moveIfRoomUnclear = true;
            pickupMover.stopPathingOnContact = false;
        }
        private void PostProcessThrownGun(Projectile thrownGunProjectile)
        {
            thrownGunProjectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(thrownGunProjectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.AddSlowEffect));
            thrownGunProjectile.pierceMinorBreakables = true;
            thrownGunProjectile.IgnoreTileCollisionsFor(0.01f);
            thrownGunProjectile.OnBecameDebrisGrounded = (Action<DebrisObject>)Delegate.Combine(thrownGunProjectile.OnBecameDebrisGrounded, new Action<DebrisObject>(this.HandleReturnLikeBoomerang));
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessThrownGun += this.PostProcessThrownGun;
        }
        GameActorFireEffect fireEffect = Gungeon.Game.Items["hot_lead"].GetComponent<BulletStatusEffectItem>().FireModifierEffect;
        GameActorCharmEffect charmEffect = Gungeon.Game.Items["charming_rounds"].GetComponent<BulletStatusEffectItem>().CharmModifierEffect;
        private void AddSlowEffect(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
        {
            Gun gun = ETGMod.Databases.Items["triple_crossbow"] as Gun;
            GameActorSpeedEffect gameActorSpeedEffect = gun.DefaultModule.projectiles[0].speedEffect;
            gameActorSpeedEffect.duration = 20f;
            arg2.aiActor.ApplyEffect(gameActorSpeedEffect, 1, null);
            arg2.gameActor.ApplyEffect(this.fireEffect, 1f, null);
            arg2.gameActor.ApplyEffect(this.charmEffect, 1f, null);
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
