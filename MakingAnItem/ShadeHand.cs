using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;
using ItemAPI;

namespace NevernamedsItems
{
    class ShadeHand : PlayerItem
    {
        //Call this method from the Start() method of your ETGModule extension class
        public static void Init()
        {
            //The name of the item
            string itemName = "Hand of Night";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it.
            string resourceName = "NevernamedsItems/Resources/shadehand_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<ShadeHand>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Giveth and Taketh Away";
            string longDesc = "The cold and clammy hands of a long-dead, particularly wrathful shade.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"kts" here is the item pool. In the console you'd type kts:sweating_bullets
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 800);

            //Adds a passive modifier, like curse, coolness, damage, etc. to the item. Works for passives and actives.

            //Set some other fields
            item.consumable = false;
            item.quality = ItemQuality.EXCLUDED;
            item.CanBeDropped = false;
        }

        //Add the item's functionality down here! I stole most of this from the Stuffed Star active item code!

        protected override void DoEffect(PlayerController user)
        {
            //Activates the effect
            PlayableCharacters characterIdentity = user.characterIdentity;

            StealthEffect();
            //start a coroutine which calls the EndEffect method when the item's effect duration runs out
            base.StartCoroutine(ItemBuilder.HandleDuration(this, this.duration, user, new Action<PlayerController>(this.BreakStealth)));

        }
        private void StealthEffect()
        {
            PlayerController owner = base.LastOwner;
            this.BreakStealth(owner);
            owner.OnItemStolen += this.BreakStealthOnSteal;
            owner.ChangeSpecialShaderFlag(1, 1f);
            owner.healthHaver.OnDamaged += this.OnDamaged;
            owner.SetIsStealthed(true, "shade");
            owner.SetCapableOfStealing(true, "shade", null);
            GameManager.Instance.StartCoroutine(this.Unstealthy());
        }

        // Token: 0x060001A3 RID: 419 RVA: 0x0000F355 File Offset: 0x0000D555
        private IEnumerator Unstealthy()
        {
            yield return new WaitForSeconds(0.15f);
            LastOwner.OnDidUnstealthyAction += this.BreakStealth;
            yield break;
        }

        // Token: 0x060001A4 RID: 420 RVA: 0x0000F364 File Offset: 0x0000D564
        private void OnDamaged(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
        {
            this.BreakStealth(LastOwner);
        }

        // Token: 0x060001A5 RID: 421 RVA: 0x0000F381 File Offset: 0x0000D581
        private void BreakStealthOnSteal(PlayerController arg1, ShopItemController arg2)
        {
            this.BreakStealth(arg1);
        }

        // Token: 0x060001A6 RID: 422 RVA: 0x0000F38C File Offset: 0x0000D58C
        private void BreakStealth(PlayerController player)
        {
            player.ChangeSpecialShaderFlag(1, 0f);
            player.OnItemStolen -= this.BreakStealthOnSteal;
            player.SetIsStealthed(false, "shade");
            player.healthHaver.OnDamaged -= this.OnDamaged;
            player.SetCapableOfStealing(false, "shade", null);
            player.OnDidUnstealthyAction -= this.BreakStealth;
            AkSoundEngine.PostEvent("Play_ENM_wizardred_appear_01", base.gameObject);
        }
        private float duration = 10f;
    }
}
