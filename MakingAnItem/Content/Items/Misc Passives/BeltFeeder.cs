using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Alexandria.ItemAPI;
using Gungeon;
using SaveAPI;
using UnityEngine;

namespace NevernamedsItems
{
    public class BeltFeeder : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<BeltFeeder>(
            "Belt Feeder",
            "More Lead",
            "Feeds fresh ammo into your guns at a much higher rate.\n\nUtterly mundane, but nonetheless effective.",
            "beltfeeder_icon");
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.ProjectileSpeed, 1.1f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.KnockbackMultiplier, 1.1f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.RateOfFire, 1.3f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            item.quality = PickupObject.ItemQuality.C;
            item.AddToSubShop(ItemBuilder.ShopType.Trorc);
            ID = item.PickupObjectId;

            item.SetupUnlockOnCustomStat(CustomTrackedStats.DOUG_ITEMS_PURCHASED, 2, DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN);
        }
        public static int ID;
        public float timeShooting;
        float amt;
        public StatModifier cur;
        public override void Update()
        {
            if (Owner)
            {
                BraveInput instanceForPlayer = BraveInput.GetInstanceForPlayer(Owner.PlayerIDX);
                if (Owner.PlayerHasActiveSynergy("Belting Out") && instanceForPlayer.GetButton(GungeonActions.GungeonActionType.Shoot) && Owner.CurrentGun && !Owner.CurrentGun.IsReloading)
                {
                    timeShooting += BraveTime.DeltaTime;
                    amt = Mathf.Lerp(1f, 4f, timeShooting / 10f);


                    List<StatModifier> m0ds = this.passiveStatModifiers.ToList();
                    m0ds.Remove(cur);
                    cur = new StatModifier()
                    {
                        amount = amt,
                        modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE,
                        statToBoost = PlayerStats.StatType.RateOfFire,
                    };
                    m0ds.Add(cur);
                    this.passiveStatModifiers = m0ds.ToArray();

                    Owner.stats.RecalculateStats(Owner);
                }
                else if (amt > 1f)
                {
                    List<StatModifier> m0ds = this.passiveStatModifiers.ToList();
                    m0ds.Remove(cur);
                    this.passiveStatModifiers = m0ds.ToArray();
                    this.RemovePassiveStatModifier(PlayerStats.StatType.PlayerBulletScale);
                    Owner.stats.RecalculateStats(Owner);
                    timeShooting = 0;
                }
            }

            base.Update();
        }
        public override void DisableEffect(PlayerController player)
        {
            if (player && player.ownerlessStatModifiers.Contains(cur))
            {
                player.ownerlessStatModifiers.Remove(cur);
                player.stats.RecalculateStats(player);
            }
            base.DisableEffect(player);
        }
    }
}
