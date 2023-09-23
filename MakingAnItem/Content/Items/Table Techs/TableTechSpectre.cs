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
    public class TableTechSpectre : TableFlipItem
    {
        public static void Init()
        {
            var item = ItemSetup.NewItem<TableTechSpectre>(
                "Table Tech Spectre",
               "Flip Fatale",
               "Flipped tables create friendly phantoms."+"\n\nChapter 16 of the \"Tabla Sutra.\" For life is a great table, and even it too, shall be flipped.",
               "NevernamedsItems/Resources/NeoItemSprites/tabletechspectre_icon", false);
            item.quality = PickupObject.ItemQuality.B;
            (item as TableTechSpectre).TableFlocking = true;
            ID = item.PickupObjectId;
            item.SetTag("table_tech");
        }
        public static int ID;
        public override void Pickup(PlayerController player)
        {
            player.OnTableFlipped += SpawnGhost;
            base.Pickup(player);
        }
        public override void DisableEffect(PlayerController player)
        {
            if (player)
            {
                player.OnTableFlipped -= SpawnGhost;
            }
            base.DisableEffect(player);
        }

        private void SpawnGhost(FlippableCover obj)
        {
            GameObject gemy = StandardisedProjectiles.ghost.InstantiateAndFireInDirection(
                      obj.specRigidbody.UnitCenter,
                      UnityEngine.Random.Range(0, 360));
            if (Owner)
            {
                Projectile proj = gemy.GetComponent<Projectile>();
                proj.Owner = Owner;
                proj.baseData.range *= 2;
                proj.GetComponent<PierceProjModifier>().penetration += 2;
                proj.Shooter = Owner.specRigidbody;
                Owner.DoPostProcessProjectile(proj);
                proj.ScaleByPlayerStats(Owner);
                proj.specRigidbody.RegisterGhostCollisionException(obj.specRigidbody);
            }
        }
    }
}