using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    class GlassRounds : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<GlassRounds>(
             "Glass Rounds",
             "Right Through You",
             "+5% damage for every Glass Guon Stone the bearer posesses." + "\n\nDisciples of the Lady of Pane are known for using these special bullets in reverence to their goddess.",
             "glassrounds_icon");
            item.quality = PickupObject.ItemQuality.C;
            item.SetTag("bullet_modifier");
            Doug.AddToLootPool(item.PickupObjectId);
        }
        private int currentItems, lastItems;
        public override void Update()
        {
            if (Owner)
            {
                CalculateStats(Owner);
            }

            else { return; }
        }

        private void CalculateStats(PlayerController player)
        {
            currentItems = player.passiveItems.Count;
            if (currentItems != lastItems)
            {
                this.RemovePassiveStatModifier(PlayerStats.StatType.Damage);
                foreach (PassiveItem item in player.passiveItems)
                {
                    if (item.PickupObjectId == 565)
                    {
                        this.AddPassiveStatModifier(PlayerStats.StatType.Damage, Owner.PlayerHasActiveSynergy("Glassworks") ? 1.1f : 1.05f, StatModifier.ModifyMethod.MULTIPLICATIVE);
                    }
                }

                lastItems = currentItems;
                player.stats.RecalculateStats(player, true, false);
            }
        }

        private void OnNewFloor()
        {
            if (Owner) { Owner.AcquirePassiveItemPrefabDirectly(PickupObjectDatabase.GetById(565) as PassiveItem); }
        }

        public override void Pickup(PlayerController player)
        {
            if (!m_pickedUpThisRun) { player.AcquirePassiveItemPrefabDirectly(PickupObjectDatabase.GetById(565) as PassiveItem); }
            GameManager.Instance.OnNewLevelFullyLoaded += this.OnNewFloor;
            base.Pickup(player);
        }
        public override void DisableEffect(PlayerController player)
        {
            GameManager.Instance.OnNewLevelFullyLoaded -= this.OnNewFloor;
            base.DisableEffect(player);
        }
    }
}