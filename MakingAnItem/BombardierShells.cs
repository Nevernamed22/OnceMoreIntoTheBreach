using System;
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
            item.quality = PickupObject.ItemQuality.EXCLUDED; //A

        }

        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {
            if (UnityEngine.Random.value <= 0.07f)
            {
                ExplosiveModifier exploding = sourceProjectile.gameObject.GetOrAddComponent<ExplosiveModifier>();
                exploding.doExplosion = true;
                exploding.explosionData = StaticExplosionDatas.explosiveRoundsExplosion;
            }
            try
            {

                if (sourceProjectile.Shooter != null && sourceProjectile.Shooter.aiActor == null && sourceProjectile.Shooter.projectile == null)
                {
                    
                        Owner.knockbackDoer.ApplyKnockback((Owner.sprite.WorldCenter - Owner.unadjustedAimPoint.XY()).normalized, 30f);
                    
                }
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
                ETGModConsole.Log(e.StackTrace);
            }
        }
        private void PostProcessBeamTick(BeamController beam)
        {
            try
            {
                if (beam.aiShooter == null)
                {

                    Owner.knockbackDoer.ApplyKnockback((Owner.sprite.WorldCenter - Owner.unadjustedAimPoint.XY()).normalized, 45f);
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
        protected override void OnDestroy()
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
