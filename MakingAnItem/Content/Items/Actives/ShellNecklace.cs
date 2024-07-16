using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Dungeonator;
using Alexandria.ItemAPI;
using Alexandria.Misc;
using UnityEngine;
using Alexandria.Assetbundle;
using SaveAPI;

namespace NevernamedsItems
{
    class ShellNecklace : ActiveGunVolleyModificationItem
    {
        public static void Init()
        {
            ActiveGunVolleyModificationItem item = ItemSetup.NewItem<ShellNecklace>(
              "Shell Necklace",
              "One For All",
              "Makes any gun into a shotgun!\n\nA series of brightly coloured shotgun shells on a string. Upon coming to the Gungeon, less scrupulous travellers were alarmed to learn that the Gundead did not possess ears with which to make a normal necklace- so they made do.",
              "shellnecklace_icon") as ActiveGunVolleyModificationItem;
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 600);
            item.duration = 15f;
            item.DuplicatesOfBaseModule = 5;
            item.DuplicateAngleOffset = 0;

            item.quality = ItemQuality.C;
            ID = item.PickupObjectId;

            item.SetupUnlockOnCustomStat(CustomTrackedStats.BEGGAR_TOTAL_DONATIONS, 74, DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN);
        }
        public static int ID;
        public override void Pickup(PlayerController player)
        {
            player.PostProcessProjectile += PostProcessProj;
            base.Pickup(player);
        }
        public override void OnPreDrop(PlayerController user)
        {
            user.PostProcessProjectile -= PostProcessProj;
            base.OnPreDrop(user);
        }
        public override void OnDestroy()
        {
            if (LastOwner) { LastOwner.PostProcessProjectile -= PostProcessProj; }
            base.OnDestroy();
        }
        private void PostProcessProj(Projectile proj, float f)
        {
            if (IsActive)
            {
                proj.baseData.speed *= UnityEngine.Random.Range(0.9f, 1.1f);
                proj.UpdateSpeed();
            }
        }
        public override void DoEffect(PlayerController user)
        {
            user.StartCoroutine(HandleTimedStatModifier(user, this));
            base.DoEffect(user);
        }
        public bool isActive;
        private static IEnumerator HandleTimedStatModifier(PlayerController player, ShellNecklace necklace)
        {
            necklace.isActive = true;
            StatModifier accuracyMod = new StatModifier()
            {
                amount = 2,
                statToBoost = PlayerStats.StatType.Accuracy,
                modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE,
            };
            StatModifier damageMod = new StatModifier()
            {
                amount = 0.33f,
                statToBoost = PlayerStats.StatType.Damage,
                modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE,
            };
            StatModifier firerateMod = new StatModifier()
            {
                amount = 0.5f,
                statToBoost = PlayerStats.StatType.RateOfFire,
                modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE,
            };

            player.ownerlessStatModifiers.Add(accuracyMod);
            player.ownerlessStatModifiers.Add(damageMod);
            player.ownerlessStatModifiers.Add(firerateMod);
            player.stats.RecalculateStats(player);

            yield return new WaitForSeconds(15);

            player.ownerlessStatModifiers.Remove(accuracyMod);
            player.ownerlessStatModifiers.Remove(damageMod);
            player.ownerlessStatModifiers.Remove(firerateMod);
            player.stats.RecalculateStats(player);
            if (necklace != null) necklace.isActive = false;
            yield break;
        }
    }
}
