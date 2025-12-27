using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;
using Alexandria.ItemAPI;
using Gungeon;

namespace NevernamedsItems
{
    class MrFahrenheit : PassiveItem
    {
        public static void Init()
        {
            PassiveItem item = ItemSetup.NewItem<MrFahrenheit>(
              "Mr. Fahrenheit",
              "200 Degrees",
              "Sprint around, leaving a firey trail!" + "\n\nThere's no stopping you!",
              "mrfahrenheit_icon") as PassiveItem;
            item.quality = PickupObject.ItemQuality.B;
            AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
            MrFahrenheit.goopDefs = new List<GoopDefinition>();
            foreach (string text in MrFahrenheit.goops)
            {
                GoopDefinition goopDefinition;
                try
                {
                    GameObject gameObject = assetBundle.LoadAsset(text) as GameObject;
                    goopDefinition = gameObject.GetComponent<GoopDefinition>();
                }
                catch
                {
                    goopDefinition = (assetBundle.LoadAsset(text) as GoopDefinition);
                }
                goopDefinition.name = text.Replace("assets/data/goops/", "").Replace(".asset", "");
                MrFahrenheit.goopDefs.Add(goopDefinition);
            }
            List<GoopDefinition> list = MrFahrenheit.goopDefs;

            Game.Items.Rename("nn:mr._fahrenheit", "nn:mr_fahrenheit");
        }
        private int currentItems, lastItems;

        public override void Update()
        {
            if (Owner && GameManager.Instance.Dungeon && !GameManager.Instance.IsLoadingLevel && !GameManager.Instance.Dungeon.IsEndTimes)
            {
                if (!Owner.IsDodgeRolling && Challenges.CurrentChallenge != ChallengeType.KEEP_IT_COOL)
                {
                    if (Owner.HasPickupID(Gungeon.Game.Items["nn:supersonic_shots"].PickupObjectId))
                    {
                        GoopDefinition GreenFireDef = (PickupObjectDatabase.GetById(698) as Gun).DefaultModule.projectiles[0].GetComponent<GoopModifier>().goopDefinition;
                        var ddgm = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(GreenFireDef);
                        ddgm.AddGoopCircle(Owner.sprite.WorldBottomCenter, 1);
                    }
                    else
                    {
                        var ddgm = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(MrFahrenheit.goopDefs[0]);
                        ddgm.AddGoopCircle(Owner.sprite.WorldBottomCenter, 1);
                    }
                }
                currentItems = Owner.passiveItems.Count;
                if (currentItems != lastItems)
                {
                    if (Owner.HasPickupID(Gungeon.Game.Items["nn:supersonic_shots"].PickupObjectId))
                    {
                        handleSpeeds(true);
                    }
                    else
                    {
                        handleSpeeds(false);
                    }
                    lastItems = currentItems;
                }
            }
        }
        private void handleSpeeds(bool IncreasedSpeed)
        {
            RemoveStat(PlayerStats.StatType.MovementSpeed);
            if (IncreasedSpeed == true)
            {
                AddStat(PlayerStats.StatType.MovementSpeed, 4, StatModifier.ModifyMethod.ADDITIVE);
            }
            else
            {
                AddStat(PlayerStats.StatType.MovementSpeed, 2, StatModifier.ModifyMethod.ADDITIVE);
            }
            Owner.stats.RecalculateStats(Owner, true, false);
        }
        private void OnFired(Projectile bullet, float khajhfdjsfhfdjs)
        {
            if (Owner.CurrentGun.PickupObjectId == 597)
            {
                if (bullet.gameObject.name == "Planet_Mercury_Projectile(Clone)")
                {
                    bullet.AdjustPlayerProjectileTint(ExtendedColours.pink, 1);
                    bullet.AdditionalScaleMultiplier *= 3;
                    bullet.OnHitEnemy += this.ApplyCharmEffect;
                }
            }
        }
        private void ApplyCharmEffect(Projectile bullet, SpeculativeRigidbody target, bool he)
        {
            target.aiActor.ApplyEffect(GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultPermanentCharmEffect, 1f, null);
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += this.OnFired;
            this.m_fireImmunity = new DamageTypeModifier();
            this.m_fireImmunity.damageMultiplier = 0f;
            this.m_fireImmunity.damageType = CoreDamageTypes.Fire;
            Owner.healthHaver.damageTypeModifiers.Add(this.m_fireImmunity);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            player.PostProcessProjectile -= this.OnFired;
            Owner.healthHaver.damageTypeModifiers.Remove(this.m_fireImmunity);

            DebrisObject debrisObject = base.Drop(player);
            return debrisObject;
        }
        public override void OnDestroy()
        {
            if (Owner)
            {
                Owner.healthHaver.damageTypeModifiers.Remove(this.m_fireImmunity);
                Owner.PostProcessProjectile -= this.OnFired;
            }
            base.OnDestroy();
        }
        private DamageTypeModifier m_fireImmunity;
        private static List<GoopDefinition> goopDefs;

        private static string[] goops = new string[]
        {
            "assets/data/goops/napalmgoopthatworks.asset",
            "assets/data/goops/napalmgoopthatworks.asset",
        };
        private void AddStat(PlayerStats.StatType statType, float amount, StatModifier.ModifyMethod method = StatModifier.ModifyMethod.ADDITIVE)
        {
            StatModifier modifier = new StatModifier
            {
                amount = amount,
                statToBoost = statType,
                modifyType = method
            };
            if (this.passiveStatModifiers == null)
                this.passiveStatModifiers = new StatModifier[] { modifier };
            else
                this.passiveStatModifiers = this.passiveStatModifiers.Concat(new StatModifier[] { modifier }).ToArray();
        }

        private void RemoveStat(PlayerStats.StatType statType)
        {
            var newModifiers = new List<StatModifier>();
            for (int i = 0; i < passiveStatModifiers.Length; i++)
            {
                if (passiveStatModifiers[i].statToBoost != statType)
                    newModifiers.Add(passiveStatModifiers[i]);
            }
            this.passiveStatModifiers = newModifiers.ToArray();
        }
    }
}
