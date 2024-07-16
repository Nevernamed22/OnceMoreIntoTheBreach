using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;
using Alexandria.ItemAPI;
using SaveAPI;

namespace NevernamedsItems
{
    public class ShutdownShells : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<ShutdownShells>(
            "Shutdown Shells",
            "No, shut them all down!",
            "These brutal shells are designed to burrow into the long spines of the Gungeon's Shotgun Gundead and wreak havoc, causing a complete shutdown of the central nervous system.\n\n" + "Not compatible with other lifeforms.",
            "shutdownshells_icon");
            item.quality = PickupObject.ItemQuality.B;
            item.SetTag("bullet_modifier");
            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.PURCHASED_SHUTDOWNSHELLS, true);
            item.AddItemToDougMetaShop(30);
            Doug.AddToLootPool(item.PickupObjectId);
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += this.PostProcessProjectile;
            player.PostProcessBeam += this.PostProcessBeam;
        }
        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {
            ProjectileInstakillBehaviour instakill = sourceProjectile.gameObject.GetOrAddComponent<ProjectileInstakillBehaviour>();
            instakill.tagsToKill.Add("shotgun_kin");
        }
        private void PostProcessBeam(BeamController sourceBeam)
        {
            if (sourceBeam.projectile)
            {
                this.PostProcessProjectile(sourceBeam.projectile, 1);
            }
        }
        public override void DisableEffect(PlayerController player)
        {
            player.PostProcessProjectile -= this.PostProcessProjectile;
            player.PostProcessBeam -= this.PostProcessBeam;
            base.DisableEffect(player);
        }      
    }
}
