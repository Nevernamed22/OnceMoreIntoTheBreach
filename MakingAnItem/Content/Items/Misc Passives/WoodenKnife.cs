using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.Misc;
using SaveAPI;

namespace NevernamedsItems
{
    public class WoodenKnife : TableFlipItem
    {
        public static void Init()
        {
            TableFlipItem item = ItemSetup.NewItem<WoodenKnife>(
              "Wooden Knife",
              "Set the Table",
              "Flipping a table creates 1-3 orbiting knives.\n\nFollowers of the Tabla Sutra would use knives such as these, carved from pure tablewood, for ritual purposes.",
              "woodenknife_icon") as TableFlipItem;
            item.quality = PickupObject.ItemQuality.D;
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE);
            ID = item.PickupObjectId;
            item.SetupUnlockOnCustomStat(CustomTrackedStats.BEGGAR_TOTAL_DONATIONS, 314, DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN);
        }
        public static int ID;
        public override void Pickup(PlayerController player)
        {
            player.OnTableFlipped += CreateKnives;
            base.Pickup(player);
        }
        public override void DisableEffect(PlayerController player)
        {
            if (player) { player.OnTableFlipped -= CreateKnives;  }
            base.DisableEffect(player);
        }

        private void CreateKnives(FlippableCover obj)
        {
            DoKnoife(false, null);
            if (Owner.PlayerHasActiveSynergy("Five Finger Fillet"))
            {
                DoKnoife(true,null);
            }
        }
        public void DoKnoife(bool isTable, GameObject table)
        {
            int num = UnityEngine.Random.Range(1, 4);
            if (isTable) num = 3;

            KnifeShieldEffect knifeShieldEffect = new GameObject("knife shield effect")
            {
                transform =
            {
                position = Owner.LockedApproximateSpriteCenter,
                parent =  Owner.transform
            }
            }.AddComponent<KnifeShieldEffect>();
            knifeShieldEffect.numKnives = num;
            knifeShieldEffect.remainingHealth = 0.5f;
            knifeShieldEffect.knifeDamage = 30f;
            knifeShieldEffect.circleRadius = isTable ? 5f : 2f;
            knifeShieldEffect.rotationDegreesPerSecond = 360f;
            knifeShieldEffect.throwSpeed = 10;
            knifeShieldEffect.throwRange = 25;
            knifeShieldEffect.throwRadius = 3;
            knifeShieldEffect.radiusChangeDistance = 3;
            knifeShieldEffect.deathVFX = (PickupObjectDatabase.GetById(65) as KnifeShieldItem).knifeDeathVFX;
            knifeShieldEffect.Initialize(Owner, (PickupObjectDatabase.GetById(65) as KnifeShieldItem).knifePrefab);
        }

    }
}
