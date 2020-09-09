using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using ItemAPI;

namespace NevernamedsItems
{
    class HeartPadlock : PlayerItem
    {
        //Call this method from the Start() method of your ETGModule extension class
        public static void Init()
        {
            //The name of the item
            string itemName = "Heart Padlock";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it.
            string resourceName = "NevernamedsItems/Resources/heartpadlock_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<HeartPadlock>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Locked Life";
            string longDesc = "Spend keys to heal."+"\n\nLocks such as these are commonly used by powerful Gunjurers to secure their souls to their bodies in case of catastrophic injury.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"kts" here is the item pool. In the console you'd type kts:sweating_bullets
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 2);

            //Adds a passive modifier, like curse, coolness, damage, etc. to the item. Works for passives and actives.

            //Set some other fields
            item.consumable = false;
            item.quality = ItemQuality.D;

            item.AddToSubShop(ItemBuilder.ShopType.Flynt);

            List<string> mandatorySynergyItems = new List<string>() { "nn:heart_padlock", "heart_locket" };
            CustomSynergies.Add("All Locked Up", mandatorySynergyItems);
            List<string> mandatorySynergyItems2 = new List<string>() { "nn:heart_padlock", "shelleton_key" };
            CustomSynergies.Add("Key Death", mandatorySynergyItems2);

            HeartPadlockID = item.PickupObjectId;
        }

        public static int HeartPadlockID;

        //Add the item's functionality down here! I stole most of this from the Stuffed Star active item code!
        float amountToHeal;
        int keysToTake;
        protected override void DoEffect(PlayerController user)
        {
            //Activates the effect
            PlayableCharacters characterIdentity = user.characterIdentity;

            if (characterIdentity != PlayableCharacters.Robot)
            {
                AkSoundEngine.PostEvent("Play_OBJ_goldkey_pickup_01", base.gameObject);
                if (user.HasPickupID(423)) amountToHeal = 2f;
                else amountToHeal = 1f;
                //float healthPercentLog = user.healthHaver.GetCurrentHealthPercentage();
                //ETGModConsole.Log("User Health Percentage is supposedly at:" + healthPercentLog);
                user.PlayEffectOnActor(ResourceCache.Acquire("Global VFX/vfx_healing_sparkles_001") as GameObject, Vector3.zero, true, false, false);
                user.healthHaver.ApplyHealing(amountToHeal);
                if (user.HasPickupID(166) && UnityEngine.Random.value < .50f) keysToTake = 0;
                else keysToTake = 1;
                user.carriedConsumables.KeyBullets -= keysToTake;

            }
            else if (characterIdentity == PlayableCharacters.Robot)
            {
                AkSoundEngine.PostEvent("Play_OBJ_goldkey_pickup_01", base.gameObject);
                if (user.HasPickupID(423)) amountToHeal = 2f;
                else amountToHeal = 1f;
                user.PlayEffectOnActor(ResourceCache.Acquire("Global VFX/vfx_healing_sparkles_001") as GameObject, Vector3.zero, true, false, false);
                user.healthHaver.Armor = user.healthHaver.Armor + amountToHeal;
                user.carriedConsumables.KeyBullets -= 1;
            }

            //start a coroutine which calls the EndEffect method when the item's effect duration runs out

        }

        public override bool CanBeUsed(PlayerController user)
        {
            if (user.carriedConsumables.KeyBullets >= 1)
            {
                float healthPercent = user.healthHaver.GetCurrentHealthPercentage();
                if (user.characterIdentity != PlayableCharacters.Robot && healthPercent < 1f) return true;
                else if (user.characterIdentity == PlayableCharacters.Robot) return true;
                else return false;
            }
            else return false;
        }
    }
}
