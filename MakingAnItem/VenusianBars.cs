using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ItemAPI;
using UnityEngine;

namespace NevernamedsItems
{
    class VenusianBars : PlayerItem
    {
        //Call this method from the Start() method of your ETGModule extension class
        public static void Init()
        {
            //The name of the item
            string itemName = "Venusian Bars";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it.
            string resourceName = "NevernamedsItems/Resources/venusianbars_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<VenusianBars>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Verified";
            string longDesc = "A charm forged out of pure, condensed skill by the one and only Gun God. While it doesn't allow the bearer to come even close to matching his skillz, they can still spit some mad bars and bullets.\n" + "Works best on Automatic weapons.\n\n\n\n" + "Pop Pop.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"kts" here is the item pool. In the console you'd type kts:sweating_bullets
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 1000);

            //Adds a passive modifier, like curse, coolness, damage, etc. to the item. Works for passives and actives.
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Coolness, 1f, StatModifier.ModifyMethod.ADDITIVE);

            //Set some other fields
            item.consumable = false;
            item.quality = ItemQuality.S;
        }

        //Add the item's functionality down here! I stole most of this from the Stuffed Star active item code!
        float firerateBuff = -1;
        float duration = 25f;
        protected override void DoEffect(PlayerController user)
        {
            //Play a sound effect
            AkSoundEngine.PostEvent("Play_WPN_LowerCaseR_Bye_GameOver_01", base.gameObject);

            //Activates the effect
            StartEffect(user);

            //start a coroutine which calls the EndEffect method when the item's effect duration runs out
            StartCoroutine(ItemBuilder.HandleDuration(this, duration, user, EndEffect));
        }


        //Doubles the movement speed
        private void StartEffect(PlayerController user)
        {
            //The Firerate mechanic
            float curFirerate = user.stats.GetBaseStatValue(PlayerStats.StatType.RateOfFire); 
            float newFirerate = curFirerate * 100f; 
            user.stats.SetBaseStatValue(PlayerStats.StatType.RateOfFire, newFirerate, user); 
            firerateBuff = newFirerate - curFirerate;

            //The Clip Size Mechanic
            user.PostProcessProjectile += this.Refill;

        }

        //Resets the player back to their original stats
        private void EndEffect(PlayerController user)
        {
            if (firerateBuff <= 0) return;
            float curFirerate = user.stats.GetBaseStatValue(PlayerStats.StatType.RateOfFire);
            float newFirerate = curFirerate - firerateBuff;
            user.stats.SetBaseStatValue(PlayerStats.StatType.RateOfFire, newFirerate, user);
            firerateBuff = -1;

            user.PostProcessProjectile -= this.Refill;

        }
        protected override void OnPreDrop(PlayerController user)
        {
            if (base.IsCurrentlyActive)
            {
                base.IsCurrentlyActive = false;
                EndEffect(user);
            }
        }

        //Disable or enable the active whenever you need!
        public override bool CanBeUsed(PlayerController user)
        {
            return base.CanBeUsed(user);
        }
        private void Refill(Projectile projectile, float effectChanceScalar)
        {
            if (LastOwner.CurrentGun != null)
            {
                Invoke("GiveBulletBack", 0.1f);
            }
        }
        private void GiveBulletBack()
        {
            LastOwner.CurrentGun.MoveBulletsIntoClip(1);
        }
    }
}
