using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Alexandria.ItemAPI;
using Dungeonator;
using SaveAPI;
using UnityEngine;

namespace NevernamedsItems
{
    class ShotInTheArm : PlayerItem
    {
        public static void Init()
        {
            string itemName = "Shot In The Arm";
            string resourceName = "NevernamedsItems/Resources/NeoActiveSprites/shotinthearm_icon";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<ShotInTheArm>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "The Jab";
            string longDesc = "A vial full of volatile stimulant. Briefly buffs offensive power. \n\nUsed by Primerdyne Marines to steady their trigger fingers in active combat.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed,7);       
            item.consumable = false;
            item.quality = ItemQuality.D; //D

            item.AddToSubShop(ItemBuilder.ShopType.Trorc);
            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.BOSSRUSH_MARINE, true);
        }
        public override void DoEffect(PlayerController user)
        {
            if (user)
            {
                user.StartCoroutine(HandleDamageDrainCoroutine(user));
            }
        }
        private IEnumerator HandleDamageDrainCoroutine(PlayerController coroutineTarget)
        {
            float multiplier = 3.5f;
            float scaleMult = 2f;
            float realTime = 3f;

            float elapsed = 0f;
            while (elapsed < realTime)
            {
                elapsed += BraveTime.DeltaTime;
                float t = Mathf.Clamp01(elapsed / realTime);
                float damagemodifier = Mathf.Lerp(multiplier, 1, t);
                float scaleMod = Mathf.Lerp(scaleMult, 1, t);

                this.RemovePassiveStatModifier(PlayerStats.StatType.Damage);
                this.RemovePassiveStatModifier( PlayerStats.StatType.PlayerBulletScale);
                this.AddPassiveStatModifier(PlayerStats.StatType.Damage, damagemodifier, StatModifier.ModifyMethod.MULTIPLICATIVE);
                this.AddPassiveStatModifier( PlayerStats.StatType.PlayerBulletScale, scaleMod, StatModifier.ModifyMethod.MULTIPLICATIVE);
                coroutineTarget.stats.RecalculateStats(coroutineTarget, false, false);

                yield return null;
            }
            yield break;
        }
    }
}

