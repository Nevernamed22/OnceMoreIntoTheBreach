using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;
using ItemAPI;
using SaveAPI;

namespace NevernamedsItems
{
    public class ShutdownShells : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Shutdown Shells";
            string resourceName = "NevernamedsItems/Resources/shutdownshells_icon";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<ShutdownShells>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "No, shut them all down!";
            string longDesc = "These brutal shells are designed to burrow into the long spines of the Gungeon's Shotgun Gundead and wreak havoc, causing a complete shutdown of the central nervous system.\n\n" + "Not compatible with other lifeforms.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");
            item.quality = PickupObject.ItemQuality.B;

            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.PURCHASED_SHUTDOWNSHELLS, true);
            item.AddItemToDougMetaShop(30);
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += this.PostProcessProjectile;
            player.PostProcessBeam += this.PostProcessBeam;
        }
        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {
            InstaKillEnemyTypeBehaviour instakill = sourceProjectile.gameObject.GetOrAddComponent<InstaKillEnemyTypeBehaviour>();
            instakill.EnemyTypeToKill.AddRange(EasyEnemyTypeLists.ModInclusiveShotgunKin);
        }
        private void PostProcessBeam(BeamController sourceBeam)
        {
            if (sourceBeam.projectile)
            {
                this.PostProcessProjectile(sourceBeam.projectile, 1);
            }
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.PostProcessProjectile -= this.PostProcessProjectile;
            player.PostProcessBeam -= this.PostProcessBeam;
            return debrisObject;
        }
        public override void OnDestroy()
        {
            if (Owner)
            {
                Owner.PostProcessProjectile -= this.PostProcessProjectile;
                Owner.PostProcessBeam -= this.PostProcessBeam;
            }
            base.OnDestroy();
        }

    }
}
