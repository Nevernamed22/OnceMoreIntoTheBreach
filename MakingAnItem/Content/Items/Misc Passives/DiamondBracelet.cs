using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;
using SaveAPI;

namespace NevernamedsItems
{
    public class DiamondBracelet : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<DiamondBracelet>(
            "Diamond Bracelet",
            "Slinger's Best Friend",
            "Thrown guns deal massive damage, and return safely to their owner." + "\n\nDespite the seeming societal progress marked by the reforging of the Ruby Bracelet, it seems there are still some bumpkins in the Gungeon's depths who insist on chucking their guns.",
            "diamondbracelet_icon");
            item.AddPassiveStatModifier( PlayerStats.StatType.ThrownGunDamage, 7f, StatModifier.ModifyMethod.ADDITIVE);

            item.quality = PickupObject.ItemQuality.D;
            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.KILLEDENEMYWITHTHROWNGUN, true);
            DiamondBraceletID = item.PickupObjectId;
        }
        public static int DiamondBraceletID;
        private void HandleReturnLikeBoomerang(DebrisObject obj)
        {
            obj.PreventFallingInPits = true;
            obj.OnGrounded -= HandleReturnLikeBoomerang;
            PickupMover pickupMover = obj.gameObject.GetOrAddComponent<PickupMover>();
            if (pickupMover.specRigidbody) { pickupMover.specRigidbody.CollideWithTileMap = false; }

            pickupMover.minRadius = 1f;
            pickupMover.moveIfRoomUnclear = true;
            pickupMover.stopPathingOnContact = false;
        }
        private void PostProcessThrownGun(Projectile thrownGunProjectile)
        {
            thrownGunProjectile.pierceMinorBreakables = true;
            thrownGunProjectile.IgnoreTileCollisionsFor(0.01f);
            thrownGunProjectile.OnBecameDebrisGrounded += HandleReturnLikeBoomerang;
        }
        public override void Pickup(PlayerController player)
        {
            player.PostProcessThrownGun += PostProcessThrownGun;
            base.Pickup(player);
        }
        public override void DisableEffect(PlayerController player)
        {
            if (player) { player.PostProcessThrownGun -= PostProcessThrownGun; }
            base.DisableEffect(player);
        }      
    }
}