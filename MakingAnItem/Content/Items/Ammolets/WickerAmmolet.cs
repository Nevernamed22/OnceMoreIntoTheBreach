using UnityEngine;
using Alexandria.ItemAPI;
using Dungeonator;
using System.Collections;
using System;
using MonoMod.RuntimeDetour;
using System.Reflection;
using Gungeon;
using System.Collections.Generic;
using Alexandria.Misc;


namespace NevernamedsItems
{
    public class WickerAmmolet : BlankModificationItem
    {
        public static void Init()
        {
            string itemName = "Wicker Ammolet";
            string resourceName = "NevernamedsItems/Resources/wickerammolet_icon";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<WickerAmmolet>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Blanks Terrify";
            string longDesc = "Modifies the elegant sigh of your blanks into a horrifying screech, sure to terrify all who hear it.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");
            item.quality = PickupObject.ItemQuality.B;

            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.AdditionalBlanksPerFloor, 1f, StatModifier.ModifyMethod.ADDITIVE);
            item.AddToSubShop(ItemBuilder.ShopType.OldRed);
            ID = item.PickupObjectId;
            item.SetTag("ammolet");
        }

        private static int ID;

        public override void Pickup(PlayerController player)
        {
            player.GetExtComp().OnBlankModificationItemProcessed += OnBlankModTriggered;
            base.Pickup(player);
            if (fleeData == null || fleeData.Player != player)
            {
                fleeData = new FleePlayerData();
                fleeData.Player = player;
                fleeData.StartDistance = 100f;
            }
        }
        public override void DisableEffect(PlayerController player)
        {
            player.GetExtComp().OnBlankModificationItemProcessed -= OnBlankModTriggered;
            base.DisableEffect(player);
        }
        private void OnBlankModTriggered(PlayerController user, SilencerInstance blank, Vector2 pos, BlankModificationItem item)
        {
            if  (item is WickerAmmolet)
            {
                AkSoundEngine.PostEvent("Play_ENM_bombshee_scream_01", user.gameObject);
                if (user.CurrentRoom.HasActiveEnemies(RoomHandler.ActiveEnemyType.All))
                {
                    foreach (AIActor aiactor in user.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All))
                    {
                        if (aiactor.behaviorSpeculator != null)
                        {
                            aiactor.behaviorSpeculator.FleePlayerData = fleeData;
                            GameManager.Instance.StartCoroutine(RemoveFear(aiactor));
                        }
                    }
                }
            }
        }      
        private static IEnumerator RemoveFear(AIActor aiactor)
        {
            yield return new WaitForSeconds(7f);
            aiactor.behaviorSpeculator.FleePlayerData = null;
            yield break;
        }
        private static FleePlayerData fleeData;
    }
}

