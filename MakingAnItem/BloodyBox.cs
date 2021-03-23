using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dungeonator;
using UnityEngine;
using ItemAPI;
using SaveAPI;

namespace NevernamedsItems
{
    class BloodyBox : PlayerItem
    {
        //Call this method from the Start() method of your ETGModule extension class
        public static void Init()
        {
            //The name of the item
            string itemName = "Bloody Box";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it.
            string resourceName = "NevernamedsItems/Resources/bloodybox_icon2";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<BloodyBox>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Oozing and Gurgling";
            string longDesc = "Sacrifice blood to the gods of infinity, in return for a chest."+"\n\nA neatly layered series of chests, emerging upwards infinitely from the gaping maw of the eternal void. The screaming voices from the abyss rattle up through the tower, and they demand sustenance.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"kts" here is the item pool. In the console you'd type kts:sweating_bullets
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 5);

            //Adds a passive modifier, like curse, coolness, damage, etc. to the item. Works for passives and actives.
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE);

            //Set some other fields
            item.consumable = false;
            item.quality = ItemQuality.B;

            item.AddToSubShop(ItemBuilder.ShopType.Flynt);
            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.KILLEDJAMMEDMIMIC, true);
        }

        //Add the item's functionality down here! I stole most of this from the Stuffed Star active item code!

        protected override void DoEffect(PlayerController user)
        {
            //Activates the effect
            PlayableCharacters characterIdentity = user.characterIdentity;
            var locationToSpawn = user.CurrentRoom.GetBestRewardLocation(IntVector2.One * 3, RoomHandler.RewardLocationStyle.PlayerCenter, true);

            if (characterIdentity != PlayableCharacters.Robot)
            {
                AkSoundEngine.PostEvent("Play_VO_lichA_cackle_01", base.gameObject);
                user.healthHaver.ApplyHealing(-2.5f);
                Chest spawnedChest = GameManager.Instance.RewardManager.SpawnRewardChestAt(locationToSpawn);
                spawnedChest.RegisterChestOnMinimap(spawnedChest.GetAbsoluteParentRoom());
            }
            else if (characterIdentity == PlayableCharacters.Robot)
            {
                AkSoundEngine.PostEvent("Play_VO_lichA_cackle_01", base.gameObject);
                user.healthHaver.Armor = user.healthHaver.Armor - 2;
                Chest spawnedChest = GameManager.Instance.RewardManager.SpawnRewardChestAt(locationToSpawn);
                spawnedChest.RegisterChestOnMinimap(spawnedChest.GetAbsoluteParentRoom());
            }

            //start a coroutine which calls the EndEffect method when the item's effect duration runs out

        }

        public override bool CanBeUsed(PlayerController user)
        {
            PlayableCharacters characterIdentity = user.characterIdentity;
            if (characterIdentity == PlayableCharacters.Robot)
            {
                return user.healthHaver.Armor > 2;
            }
            else
            {
                return user.healthHaver.GetCurrentHealth() > 2.5f;
            }

        }
    }
}
