using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Alexandria.ItemAPI;
using UnityEngine;
using MonoMod.RuntimeDetour;
using System.Reflection;
using SaveAPI;

namespace NevernamedsItems
{
    public class ArmouredArmour : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<ArmouredArmour>(
            "Armoured Armour",
            "Why ARE they shields?",
            "Chance to double armour pickups." + "\n\nA suit of armour made out of smaller, less effective pieces of armour." + "\nIt's genius!",
            "armouredarmour_icon");
            item.quality = PickupObject.ItemQuality.B;
            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.PLAYERHELDMORETHANFIVEARMOUR, true);
            ArmouredArmourID = item.PickupObjectId;
        }
        public static int ArmouredArmourID;
        Hook healthPickupHook = new Hook(
                typeof(HealthPickup).GetMethod("Pickup", BindingFlags.Instance | BindingFlags.Public),
                typeof(ArmouredArmour).GetMethod("heartPickupHookMethod")
            );
        public static void heartPickupHookMethod(Action<HealthPickup, PlayerController> orig, HealthPickup self, PlayerController player)
        {
            orig(self, player);
            if (self.armorAmount > 0)
            {
                if (player.HasPickupID(Gungeon.Game.Items["nn:armoured_armour"].PickupObjectId))
                {
                    if (canGiveArmor)
                    {
                        float procChance = 0.5f;
                        if (player.PlayerHasActiveSynergy("Armoured Armoured Armour")) procChance = 0.75f;
                        if (UnityEngine.Random.value >= procChance)
                        {
                            player.healthHaver.Armor += 1;
                        }
                        canGiveArmor = false;
                    }
                    else
                    {
                        canGiveArmor = true;
                    }
                }
            }
        }
        static bool canGiveArmor = false;
        public override void Pickup(PlayerController player)
        {
            bool hasntAlreadyBeenCollected = !this.m_pickedUpThisRun;
            if (hasntAlreadyBeenCollected)
            {
                player.healthHaver.Armor += 1;
            }
            base.Pickup(player);
        }
    }
}