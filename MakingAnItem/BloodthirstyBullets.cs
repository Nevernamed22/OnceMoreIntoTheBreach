using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using ItemAPI;

namespace NevernamedsItems
{
    public class BloodthirstyBullets : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Bloodthirsty Bullets";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/bloodthirstybullets_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<BloodthirstyBullets>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Born in the Fray";
            string longDesc = "Bullets either deal massive bonus damage to enemies, or enjam them." + "\n\nThese bullets were designed, cast, and shaped in the middle of combat to train them for the battlefield.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE);

            List<string> mandatorySynergyItems = new List<string>() { "nn:bloodthirsty_bullets", "silver_bullets" };
            CustomSynergies.Add("[todo: Add funny synergy name]", mandatorySynergyItems);

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.B;


        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += this.PostProcessProjectile;
            player.PostProcessBeam += this.PostProcessBeam;
        }
        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {
            try
            {
                sourceProjectile.OnHitEnemy += this.OnHitEnemy;
                if (AllJammedState.allJammedActive == true)
                {
                    sourceProjectile.baseData.damage *= 1.45f;
                }
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
            }
        }
        private void PostProcessBeam(BeamController sourceBeam)
        {
            try
            {
                sourceBeam.projectile.OnHitEnemy += this.OnHitEnemy;
                if (AllJammedState.allJammedActive == true)
                {
                    sourceBeam.DamageModifier *= 1.45f;
                }
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
            }
        }

        private void OnHitEnemy(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
        {
            if (arg2 != null && arg2.aiActor != null && Owner != null)
            {
                if (AllJammedState.allJammedActive == false)
                {            
                    if (arg2.aiActor.IsBlackPhantom) return;
                    else if (Owner.CurrentGun.name == "Earthworm Gun")
                    {
                        arg2.aiActor.healthHaver.ApplyDamage(2.2f, Vector2.zero, "BonusDamage", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                    }
                    else if (!arg2.healthHaver.IsDead && !arg2.healthHaver.IsBoss)
                    {
                        float procChance = UnityEngine.Random.value;
                        if (Owner.HasPickupID(538))
                        {
                            if (procChance > .8)
                            {
                                arg2.aiActor.BecomeBlackPhantom();
                            }
                            else
                            {
                                arg2.aiActor.healthHaver.ApplyDamage(arg1.ModifiedDamage * 14, Vector2.zero, "BonusDamage", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                            }
                        }
                        else
                        {
                            if (procChance > .6)
                            {
                                arg2.aiActor.BecomeBlackPhantom();
                            }
                            else
                            {
                                arg2.aiActor.healthHaver.ApplyDamage(arg1.ModifiedDamage * 30, Vector2.zero, "BonusDamage", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                            }
                        }
                    }
                }
            }
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.PostProcessProjectile -= this.PostProcessProjectile;
            player.PostProcessBeam -= this.PostProcessBeam;
            return debrisObject;
        }
        protected override void OnDestroy()
        {
            Owner.PostProcessProjectile -= this.PostProcessProjectile;
            Owner.PostProcessBeam -= this.PostProcessBeam;
            base.OnDestroy();
        }

    }
}
