using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;
using MonoMod.RuntimeDetour;
using System.Reflection;
using System.Collections.Generic;
using Dungeonator;
using SaveAPI;

namespace NevernamedsItems
{
    public class ShroomedBullets : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<ShroomedBullets>(
            "Shroomed Bullets",
            "Misfired",
            "Busted shells that fragment upon leaving the barrel."+"\n\nThese particular bullets have held up quite well despite their flawed construction at the hands of a blacksmiths aspiring daughter...",
            "shroomedbullets_icon");
            item.quality = PickupObject.ItemQuality.C;
            item.SetTag("bullet_modifier");
            ID = item.PickupObjectId;
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Damage, 0.75f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            item.SetupUnlockOnCustomStat(CustomTrackedStats.BEGGAR_TOTAL_DONATIONS, 1274, DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN);
            Doug.AddToLootPool(item.PickupObjectId);
        }
        public static int ID;
        public override void Pickup(PlayerController player)
        {
            player.stats.AdditionalVolleyModifiers += ModifyVolley;
            player.stats.RecalculateStats(player, false, false);
            base.Pickup(player);
        }
        public override void DisableEffect(PlayerController player)
        {
            if (player)
            {
                player.stats.AdditionalVolleyModifiers -= ModifyVolley;
                player.stats.RecalculateStats(player, false, false);
            }
            base.DisableEffect(player);
        }
        public void ModifyVolley(ProjectileVolleyData volleyToModify)
        {
            int num = 1;
            if (Owner && Owner.PlayerHasActiveSynergy("Not Mu' Shroom Left")) { num += 1; }
            for (int j = 0; j < num; j++)
            {
                int count = volleyToModify.projectiles.Count;
                for (int i = 0; i < count; i++)
                {
                    ProjectileModule projectileModule = volleyToModify.projectiles[i];
                    projectileModule.angleFromAim += 20;

                    int sourceIndex = i;
                    if (projectileModule.CloneSourceIndex >= 0)
                    {
                        sourceIndex = projectileModule.CloneSourceIndex;
                    }
                    ProjectileModule projectileModule2 = ProjectileModule.CreateClone(projectileModule, false, sourceIndex);
                    projectileModule2.angleFromAim -= 40f;
                    projectileModule2.ignoredForReloadPurposes = true;
                    projectileModule2.ammoCost = 0;

                    volleyToModify.projectiles.Add(projectileModule2);
                }
            }
        }
    }

}

