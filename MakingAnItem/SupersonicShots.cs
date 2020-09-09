using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using ItemAPI;
using System.Collections;

namespace NevernamedsItems
{
    public class SupersonicShots : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Supersonic Shots";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/supersonicshot_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<SupersonicShots>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Nyoom";
            string longDesc = "Makes your bullets travel at supersonic speeds."+"\n\nBrought to the Gungeon by the infamous speedster Tonic.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.ProjectileSpeed, 10, StatModifier.ModifyMethod.MULTIPLICATIVE);

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.A;
            item.AddToSubShop(ItemBuilder.ShopType.Cursula);
        }
        private Gun currentHeldGun, lastHeldGun;
        protected override void Update()
        {
            currentHeldGun = Owner.CurrentGun;
            if (currentHeldGun != lastHeldGun)
            {
                if (Owner.CurrentGun.PickupObjectId == 149)
                {
                    GiveSynergyBoost();
                }
                else
                {
                    RemoveSynergyBoost();
                }
                lastHeldGun = currentHeldGun;
            }
            base.Update();
        }
        private void GiveSynergyBoost()
        {
            EnableVFX(Owner);
            AddStat(PlayerStats.StatType.MovementSpeed, 1.45f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            AddStat(PlayerStats.StatType.DodgeRollSpeedMultiplier, 1.45f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            AddStat(PlayerStats.StatType.Damage, 1.2f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            Owner.stats.RecalculateStats(Owner, true, false);

        }
        private void RemoveSynergyBoost()
        {
            DisableVFX(Owner);
            RemoveStat(PlayerStats.StatType.MovementSpeed);
            RemoveStat(PlayerStats.StatType.DodgeRollSpeedMultiplier);
            RemoveStat(PlayerStats.StatType.Damage);
            Owner.stats.RecalculateStats(Owner, true, false);

        }
        private void EnableVFX(PlayerController user)
        {
            Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(user.sprite);
            outlineMaterial.SetColor("_OverrideColor", new Color(1f, 1f, 200f));
            activeOutline = true;
        }

        private void DisableVFX(PlayerController user)
        {
            Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(user.sprite);
            outlineMaterial.SetColor("_OverrideColor", new Color(0f, 0f, 0f));
            activeOutline = false;
        }
        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {
            sourceProjectile.AdjustPlayerProjectileTint(Color.blue, 1, 0f);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.healthHaver.OnDamaged -= this.PlayerTookDamage;
            player.PostProcessProjectile -= this.PostProcessProjectile;
            return debrisObject;
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += this.PostProcessProjectile;
            player.healthHaver.OnDamaged += this.PlayerTookDamage;
        }
        protected override void OnDestroy()
        {
            Owner.healthHaver.OnDamaged -= this.PlayerTookDamage;
            Owner.PostProcessProjectile -= this.PostProcessProjectile;
            base.OnDestroy();
        }
        private void AddStat(PlayerStats.StatType statType, float amount, StatModifier.ModifyMethod method = StatModifier.ModifyMethod.ADDITIVE)
        {
            /*foreach (var m in passiveStatModifiers)
            {
                if (m.statToBoost == statType) return; //don't add duplicates
            }*/

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
        bool activeOutline = false;
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
        private void PlayerTookDamage(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
        {
            if (activeOutline == true)
            {
                GameManager.Instance.StartCoroutine(this.GainOutline());
            }

            else if (activeOutline == false)
            {
                GameManager.Instance.StartCoroutine(this.LoseOutline());
            }
        }

        private IEnumerator GainOutline()
        {
            PlayerController user = Owner;
            yield return new WaitForSeconds(0.05f);
            EnableVFX(user);
            yield break;
        }

        private IEnumerator LoseOutline()
        {
            PlayerController user = Owner;
            yield return new WaitForSeconds(0.05f);
            DisableVFX(user);
            yield break;
        }
    }
}

