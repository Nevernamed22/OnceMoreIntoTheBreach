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
    public class MengerAmmoBox : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Menger Ammo Box";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/mengerammobox_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<MengerAmmoBox>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Fractal Replenishment";
            string longDesc = "Equalises regular and spread ammo boxes."+"\n\nA delicate fractal of infinitely patterned bullets. Occasionally coughs up lead from the void.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item


            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.B;
        }
        Hook ammoPickupHook = new Hook(
                typeof(AmmoPickup).GetMethod("Pickup", BindingFlags.Instance | BindingFlags.Public),
                typeof(MengerAmmoBox).GetMethod("ammoPickupHookMethod")
            );
        public static void ammoPickupHookMethod(Action<AmmoPickup, PlayerController> orig, AmmoPickup self, PlayerController player)
        {
            orig(self, player);
            //ETGModConsole.Log("Ammo pickup was triggered");
            if (player.HasPickupID(Gungeon.Game.Items["nn:menger_ammo_box"].PickupObjectId))
            {
                if (canGiveAmmo)
                {
                    if (self.mode == AmmoPickup.AmmoPickupMode.FULL_AMMO)
                    {
                        for (int i = 0; i < player.inventory.AllGuns.Count; i++)
                        {
                            if (player.inventory.AllGuns[i] && player.CurrentGun != player.inventory.AllGuns[i])
                            {
                                player.inventory.AllGuns[i].GainAmmo(Mathf.FloorToInt((float)player.inventory.AllGuns[i].AdjustedMaxAmmo * 0.2f));
                            }
                        }
                        player.CurrentGun.ForceImmediateReload(false);
                        if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
                        {
                            PlayerController otherPlayer = GameManager.Instance.GetOtherPlayer(player);
                            if (!otherPlayer.IsGhost)
                            {
                                for (int j = 0; j < otherPlayer.inventory.AllGuns.Count; j++)
                                {
                                    if (otherPlayer.inventory.AllGuns[j])
                                    {
                                        otherPlayer.inventory.AllGuns[j].GainAmmo(Mathf.FloorToInt((float)otherPlayer.inventory.AllGuns[j].AdjustedMaxAmmo * 0.2f));
                                    }
                                }
                                otherPlayer.CurrentGun.ForceImmediateReload(false);
                            }
                        }
                    }
                    else if(self.mode == AmmoPickup.AmmoPickupMode.SPREAD_AMMO)
                    {
                        if (player.CurrentGun != null && player.CurrentGun.CanGainAmmo)
                        {
                            player.CurrentGun.GainAmmo(player.CurrentGun.AdjustedMaxAmmo);
                            player.CurrentGun.ForceImmediateReload(false);
                        }
                    }
                    canGiveAmmo = false;
                }
                else
                {
                    canGiveAmmo = true;
                }
            }
        }
        static bool canGiveAmmo = false;
    }
}