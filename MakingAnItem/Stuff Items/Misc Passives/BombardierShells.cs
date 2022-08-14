﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using ItemAPI;
using UnityEngine;
using MonoMod.RuntimeDetour;
using System.Reflection;

namespace NevernamedsItems
{
    public class BombardierShells : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Bombardier Shells";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/bombardiershells_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<BombardierShells>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Heavy Ammunition";
            string longDesc = "Increases damage and gives a chance for projectiles to be explosive." + "\n\nThe explosive force necessary to fire these shells creates a lot of recoil.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Damage, 2f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.PlayerBulletScale, 1.3f, StatModifier.ModifyMethod.MULTIPLICATIVE);

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.A; //A


            gunFiredHook = new Hook(
                    typeof(Gun).GetMethod("ShootSingleProjectile", BindingFlags.NonPublic | BindingFlags.Instance),
                    typeof(BombardierShells).GetMethod("GunAttackHook", BindingFlags.Public | BindingFlags.Static)
                );
            BombardierShellsID = item.PickupObjectId;
        }
        public static int BombardierShellsID;
        private static Hook gunFiredHook;
        public static void GunAttackHook(Action<Gun, ProjectileModule, ProjectileData, GameObject> orig, Gun self, ProjectileModule mod, ProjectileData data = null, GameObject overrideObject = null)
        {
            try
            {
                orig(self, mod, data, overrideObject);
                if (self != null && mod != null && self.GunPlayerOwner())
                {
                    if (self.GunPlayerOwner().HasPickupID(BombardierShellsID))
                    {
                        float knockbackAmt = 40f;
                        float projDamage = 10;

                        Dictionary<ProjectileModule, ModuleShootData> moduleData = OMITBReflectionHelpers.ReflectGetField<Dictionary<ProjectileModule, ModuleShootData>>(typeof(Gun), "m_moduleData", self);
                  
                        if (overrideObject)
                        {
                            Projectile projectile = overrideObject.GetComponent<Projectile>();
                            projDamage = projectile.baseData.damage;
                        }
                        else if (mod.shootStyle == ProjectileModule.ShootStyle.Charged && moduleData != null) 
                        {
                            ProjectileModule.ChargeProjectile chargeProjectile = mod.GetChargeProjectile(moduleData[mod].chargeTime);
                            if (chargeProjectile != null)
                            {
                              Projectile  projectile = chargeProjectile.Projectile;
                            projDamage = projectile.baseData.damage;
                            }
                        }
                        else
                        {
                            Projectile projectile = mod.GetCurrentProjectile(moduleData[mod], self.GunPlayerOwner());
                            projDamage = projectile.baseData.damage;
                        }

                        float multiplier = projDamage / 10;
                        knockbackAmt *= multiplier;
                        knockbackAmt = Mathf.Min(100, knockbackAmt);

                        if (self.GunPlayerOwner().PlayerHasActiveSynergy("Forward Thinking")) knockbackAmt *= -0.5f;
                        self.GunPlayerOwner().knockbackDoer.ApplyKnockback((self.GunPlayerOwner().sprite.WorldCenter - self.GunPlayerOwner().unadjustedAimPoint.XY()).normalized, knockbackAmt);

                    }
                }
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
                ETGModConsole.Log(e.StackTrace);
            }
        }
        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {
            if (UnityEngine.Random.value <= 0.07f)
            {
                ExplosiveModifier exploding = sourceProjectile.gameObject.GetOrAddComponent<ExplosiveModifier>();
                exploding.doExplosion = true;
                exploding.explosionData = StaticExplosionDatas.explosiveRoundsExplosion;
            }
        }
        private void PostProcessBeamTick(BeamController beam)
        {
            try
            {
                if (beam.aiShooter == null)
                {
                    float knockbackAmt = 60f;
                    if (Owner.PlayerHasActiveSynergy("Forward Thinking")) knockbackAmt *= -0.5f;
                    Owner.knockbackDoer.ApplyKnockback((Owner.sprite.WorldCenter - Owner.unadjustedAimPoint.XY()).normalized, knockbackAmt);
                }
                else
                {
                    ETGModConsole.Log("Beam AIShooter was not null");
                }
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
                ETGModConsole.Log(e.StackTrace);
            }
        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.PostProcessProjectile -= this.PostProcessProjectile;
            player.PostProcessBeamChanceTick -= this.PostProcessBeamTick;
            return debrisObject;
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += this.PostProcessProjectile;
            player.PostProcessBeamChanceTick += this.PostProcessBeamTick;

        }
        public override void OnDestroy()
        {
            if (Owner)
            {
                Owner.PostProcessBeamChanceTick -= this.PostProcessBeamTick;
                Owner.PostProcessProjectile -= this.PostProcessProjectile;
            }
            base.OnDestroy();
        }
    }
}
