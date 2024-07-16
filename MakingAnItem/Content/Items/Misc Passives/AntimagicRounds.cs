using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;
using Alexandria.ItemAPI;
using MonoMod.RuntimeDetour;
using System.Reflection;

namespace NevernamedsItems
{
    public class AntimagicRounds : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<AntimagicRounds>(
            "Antimagic Rounds",
            "Casting Time: 1 Action",
            "The arcane runes and nullifying antimagic field of these bullets allows them to break through the protective wards of Gunjurers with ease.",
            "antimagicrounds_icon");
            item.quality = PickupObject.ItemQuality.B;
            item.SetTag("bullet_modifier");
            item.AddToSubShop(ItemBuilder.ShopType.Cursula);
            Doug.AddToLootPool(item.PickupObjectId);

            AntimagicRoundsID = item.PickupObjectId;        
        }
        public static int AntimagicRoundsID;
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += this.PostProcessProjectile;
            player.PostProcessBeam += this.PostProcessBeam;
        }
        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {
            ProjectileInstakillBehaviour instakill = sourceProjectile.gameObject.GetOrAddComponent<ProjectileInstakillBehaviour>();
            instakill.tagsToKill.AddRange(new List<string>{ "gunjurer", "gunsinger", "bookllet"});
            instakill.enemyGUIDsToKill.AddRange(new List<string> { EnemyGuidDatabase.Entries["wizbang"], EnemyGuidDatabase.Entries["pot_fairy"] });
        }
        private void PostProcessBeam(BeamController sourceBeam)
        {
            if (sourceBeam.projectile) this.PostProcessProjectile(sourceBeam.projectile, 1);
        }
        public override void DisableEffect(PlayerController player)
        {
            player.PostProcessProjectile -= this.PostProcessProjectile;
            player.PostProcessBeam -= this.PostProcessBeam;
            base.DisableEffect(player);
        }
    }
}

