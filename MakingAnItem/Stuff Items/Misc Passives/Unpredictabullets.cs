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
    public class Unpredictabullets : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Unpredictabullets";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/unpredictabullets_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<Unpredictabullets>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Who? What? When? Where?";
            string longDesc = "Unpredictably modifies bullet stats."+"\n\nCreated by firing enough bullets at the wall and seeing what stuck.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.B;

        }

        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {
            try
            {
                sourceProjectile.baseData.damage *= GetModifierAmount(Owner);
                sourceProjectile.baseData.range *= GetModifierAmount(Owner);
                sourceProjectile.baseData.speed *= GetModifierAmount(Owner);
                sourceProjectile.baseData.force *= GetModifierAmount(Owner);
                sourceProjectile.AdditionalScaleMultiplier *= GetModifierAmount(Owner);
                sourceProjectile.UpdateSpeed();
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
            }
        }      
        private float GetModifierAmount(PlayerController owner)
        {
            int max = 200;
            int min = 10;
            if (owner.PlayerHasActiveSynergy("Cause And Effect")) max = 250;
            float initialPick = UnityEngine.Random.Range(min, max + 1);
            float finalmod = initialPick /= 100f;
            return finalmod;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.PostProcessProjectile -= this.PostProcessProjectile;
            return debrisObject;
        }
        public override void OnDestroy()
        {
            if (Owner) Owner.PostProcessProjectile -= this.PostProcessProjectile;
            base.OnDestroy();
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += this.PostProcessProjectile;
        }
    }
}
