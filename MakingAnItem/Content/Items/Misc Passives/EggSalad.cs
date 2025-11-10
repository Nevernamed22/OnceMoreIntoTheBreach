using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alexandria.Misc;
using UnityEngine;
using Alexandria.ItemAPI;
using SaveAPI;

namespace NevernamedsItems
{
    public class EggSalad : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<EggSalad>(
              "Egg Salad",
              "Tastes Fishy. Why.",
              "Somebody lost an egg salad down here a long, long time ago...",
              "egg_salad_001");
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Health, 1, StatModifier.ModifyMethod.ADDITIVE);
            item.quality = PickupObject.ItemQuality.D;

        }
        public override void Pickup(PlayerController player)
        {
            if (player.ForceZeroHealthState && !m_pickedUpThisRun) { player.healthHaver.Armor += 1;  }
            base.Pickup(player);
        }

    }
    public class PrimaBean : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<PrimaBean>(
             "Prima Bean",
             "Magic?",
             "There are strange magics coursing through this bean, like fiber! It’s good for your body damnit!",
             "primabean_improved");           
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Health, 3, StatModifier.ModifyMethod.ADDITIVE);
            item.quality = PickupObject.ItemQuality.S;
            item.AddToSubShop(ItemBuilder.ShopType.Cursula);
        }
        public override void Pickup(PlayerController player)
        {
            if (player.ForceZeroHealthState && !m_pickedUpThisRun) { player.healthHaver.Armor += 3;  }
            base.Pickup(player);
        }

    }
    
    public class TitanBullets : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<TitanBullets>(
             "Titan Bullets",
             "Absolute Unit",
             "Bullets increase massively in size, and slightly in damage." + "\n\nThese bullets are so big that enemies are left in shock and awe.",
             "titanbullets_improved");            
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.PlayerBulletScale, 10, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Damage, 1.05f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            item.quality = PickupObject.ItemQuality.D;
            item.SetTag("bullet_modifier");
            item.SetupUnlockOnCustomStat(CustomTrackedStats.TITAN_KIN_KILLED, 4, DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN);
            ID = item.PickupObjectId;
            Doug.AddToLootPool(item.PickupObjectId);

        }
        public static int ID;
        public override void Pickup(PlayerController player)
        {
            player.PostProcessProjectile += this.PostProcessProjectile;
            base.Pickup(player);
        }
        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {
            sourceProjectile.AppliesStun = true;
            sourceProjectile.AppliedStunDuration = 1f;
            sourceProjectile.StunApplyChance = 0.3f * effectChanceScalar;
        }
        public override void DisableEffect(PlayerController player)
        {
            if (Owner) { player.PostProcessProjectile -= this.PostProcessProjectile; }
            base.DisableEffect(player);
        }        
    }

    public class SpareBlank : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<SpareBlank>(
             "Spare Blank",
             "+1 to Blank",
             "Never hurts to have a spare.",
             "spareblank_new");         
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.AdditionalBlanksPerFloor, 1, StatModifier.ModifyMethod.ADDITIVE);
            item.quality = PickupObject.ItemQuality.D;
            item.AddToSubShop(ItemBuilder.ShopType.OldRed);
        }
        public override void Pickup(PlayerController player)
        {
            if (!m_pickedUpThisRun) { LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(224).gameObject, player); }
            base.Pickup(player);
        }
    }


    public class PowerArmour : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<PowerArmour>(
             "Power Armour",
             "Power Fantasy",
             "A high tech suit of armour from a small planet on the rim of explored space. It's layered plasteel composition makes it incredibly tough.",
             "powerarmour_improved");
            item.CanBeDropped = false;
            item.quality = PickupObject.ItemQuality.A;
            item.AddToSubShop(ItemBuilder.ShopType.Trorc);
            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.PURCHASED_POWERARMOUR, true);
            PowerArmourID = item.PickupObjectId;
        }
        public static int PowerArmourID;
        public override void Pickup(PlayerController player)
        {
            if (!this.m_pickedUpThisRun) { player.healthHaver.Armor += 8; }
            base.Pickup(player);
        }

    }
}
