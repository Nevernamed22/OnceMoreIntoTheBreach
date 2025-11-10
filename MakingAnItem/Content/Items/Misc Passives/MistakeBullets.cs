using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI ;
using Alexandria.Misc;

namespace NevernamedsItems
{
    public class MistakeBullets : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<MistakeBullets>(
            "Mistake Bullets",
            "Your Bullets Suck!",
            "Grants a firerate and reload speed bonus, in exchange for negative knockback.\n\n" + "These bullets surprisingly have no relation to the mysterious entity known as The Mistake, being crafted by a hunchbacked hermit on a distant world known as 'Space-Albania'... probably. ",
            "mistakebullets_improved");
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.KnockbackMultiplier, -3, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.RateOfFire, 1.2f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.ReloadSpeed, 0.7f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            item.SetTag("bullet_modifier");

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.D;
            MistakeBulletsID = item.PickupObjectId;
            Doug.AddToLootPool(item.PickupObjectId);
        }
        public static int MistakeBulletsID;
        private void PostProcess(Projectile bullet, float bleh)
        {
            if (bullet && bullet.ProjectilePlayerOwner() && bullet.ProjectilePlayerOwner().PlayerHasActiveSynergy("For Big Mistakes"))
            {
                bullet.baseData.force *= -1f;
            }
        }
        private void ProcessBeam(BeamController beam)
        {
            if (beam)
            {
                Projectile projectile = beam.projectile;
                if (projectile)
                {
                    this.PostProcess(projectile, 1f);
                }
            }
        }

        public override void Pickup(PlayerController player)
        {
            if (!m_pickedUpThisRun)
            {
                player.AcquirePassiveItemPrefabDirectly(PickupObjectDatabase.GetById(565) as PassiveItem);
                player.AcquirePassiveItemPrefabDirectly(PickupObjectDatabase.GetById(127) as PassiveItem);
            }
            player.PostProcessProjectile += this.PostProcess;
            player.PostProcessBeam += this.ProcessBeam;
            base.Pickup(player);
        }
        public override void DisableEffect(PlayerController player)
        {
            if (player)
            {
                player.PostProcessProjectile -= this.PostProcess;
                player.PostProcessBeam -= this.ProcessBeam;
            }
            base.DisableEffect(player);
        }
    }
}
