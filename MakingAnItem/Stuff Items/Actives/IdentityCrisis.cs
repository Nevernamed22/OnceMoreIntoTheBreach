using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ItemAPI;
using UnityEngine;

namespace NevernamedsItems
{
    class IdentityCrisis : PlayerItem
    {
        //Call this method from the Start() method of your ETGModule extension class
        public static void Init()
        {
            //The name of the item
            string itemName = "Identity Crisis";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it.
            string resourceName = "NevernamedsItems/Resources/identitycrisis_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<IdentityCrisis>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "WHO AM I?";
            string longDesc = "Makes you completely forget who you are.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"kts" here is the item pool. In the console you'd type kts:sweating_bullets
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.None, 1000);

            //Adds a passive modifier, like curse, coolness, damage, etc. to the item. Works for passives and actives.

            //Set some other fields
            item.consumable = true;
            item.quality = ItemQuality.B;

            item.AddToSubShop(ItemBuilder.ShopType.Cursula);
            //SYNERGIES 
            //WITH WINGMAN, WOLF, or UNITY --> "Those We Left Behind" (Gives the slinger, Dart gun, and Lamey's starter)
            List<string> mandatorySynergyItems = new List<string>() { "nn:identity_crisis" };
            List<string> optionalSynergyItems = new List<string>() { "clone", "shadow_clone", "gun_soul" };
            CustomSynergies.Add("New run, New me", mandatorySynergyItems, optionalSynergyItems);
            //WITH CLONE, SHADOW CLONE, OR GUN SOUL --> "New Run, new me" (gives ALL character starters)            
            
            List<string> mandatorySynergyItems2 = new List<string>() { "nn:identity_crisis" };
            List<string> optionalSynergyItems2 = new List<string>() { "wingman", "wolf", "unity" };
            CustomSynergies.Add("Those We Left Behind", mandatorySynergyItems2, optionalSynergyItems2);
        }

        //Add the item's functionality down here! I stole most of this from the Stuffed Star active item code!

        protected override void DoEffect(PlayerController user)
        {
            //Play a sound effect
            float currentCurse = user.stats.GetBaseStatValue(PlayerStats.StatType.Curse);
            user.stats.SetBaseStatValue(PlayerStats.StatType.Curse, currentCurse + 1f, user);
            if (user.HasPickupID(491) || user.HasPickupID(492) || user.HasPickupID(495))
            {
                if (!user.HasPickupID(24) && !user.HasPickupID(811))
                {
                    Gun CharacterStarterGun = (PickupObjectDatabase.GetById(24) as Gun);
                    LastOwner.inventory.AddGunToInventory(CharacterStarterGun, true);
                }
                if (!user.HasPickupID(604))
                {
                    Gun CharacterStarterGun = (PickupObjectDatabase.GetById(604) as Gun);
                    LastOwner.inventory.AddGunToInventory(CharacterStarterGun, true);
                }
                if (!user.HasPickupID(603))
                {
                    Gun CharacterStarterGun = (PickupObjectDatabase.GetById(603) as Gun);
                    LastOwner.inventory.AddGunToInventory(CharacterStarterGun, true);
                }
            }
            if (user.HasPickupID(311) || user.HasPickupID(820) || user.HasPickupID(489))
            {
                GiveHunterLoadout(user);
                GiveConvictLoadout(user);
                GiveMarineLoadout(user);
                GivePilotLoadout(user);
                GiveBulletLoadout(user);
                GiveRobotLoadout(user);
            }
            else pickRandomCharacter();
            //start a coroutine which calls the EndEffect method when the item's effect duration runs out
        }

        private void pickRandomCharacter()
        {
            int character = UnityEngine.Random.Range(1, 7);
            if (character == 1 && LastOwner.characterIdentity != PlayableCharacters.Guide)
            {
                ETGModConsole.Log("The item seleced the Hunter loadout");
                GiveHunterLoadout(LastOwner);
            }
            else if (character == 1 && LastOwner.characterIdentity == PlayableCharacters.Guide)
            {
                ETGModConsole.Log("The item seleced the Hunter loadout, but you're already the Hunter so it repicked.");
                pickRandomCharacter();
            }
            else if (character == 2 && LastOwner.characterIdentity != PlayableCharacters.Convict)
            {
                ETGModConsole.Log("The item seleced the Convict loadout");
                GiveConvictLoadout(LastOwner);
            }
            else if (character == 2 && LastOwner.characterIdentity == PlayableCharacters.Convict)
            {
                ETGModConsole.Log("The item seleced the Convict loadout, but you're already the Convict so it repicked.");
                pickRandomCharacter();
            }
            else if (character == 3 && LastOwner.characterIdentity != PlayableCharacters.Soldier)
            {
                ETGModConsole.Log("The item seleced the Marine loadout");
                GiveMarineLoadout(LastOwner);
            }
            else if (character == 3 && LastOwner.characterIdentity == PlayableCharacters.Soldier)
            {
                ETGModConsole.Log("The item seleced the Marine loadout, but you're already the Marine so it repicked.");
                pickRandomCharacter();
            }
            else if (character == 4 && LastOwner.characterIdentity != PlayableCharacters.Pilot)
            {
                ETGModConsole.Log("The item seleced the Pilot loadout");
                GivePilotLoadout(LastOwner);
            }
            else if (character == 4 && LastOwner.characterIdentity == PlayableCharacters.Pilot)
            {
                ETGModConsole.Log("The item seleced the Pilot loadout, but you're already the Pilot so it repicked.");
                pickRandomCharacter();
            }
            else if (character == 5) //THIS IS THE ROBOT'S LOADOUT
            {
                if (LastOwner.characterIdentity != PlayableCharacters.Robot)
                {
                    ETGModConsole.Log("The item seleced the Robot loadout");
                    GiveRobotLoadout(LastOwner);
                }
                else
                {
                    ETGModConsole.Log("The item seleced the Robot loadout, but you're already the Robot so it repicked.");
                    pickRandomCharacter();
                }
            }
            else if (character == 6) //THIS IS THE BULLET'S LOADOUT
            {
                if (LastOwner.characterIdentity != PlayableCharacters.Bullet)
                {
                    ETGModConsole.Log("The item seleced the Bullet loadout");
                    GiveBulletLoadout(LastOwner);
                }
                else
                {
                    ETGModConsole.Log("The item seleced the Bullet loadout, but you're already the Bullet so it repicked.");
                    pickRandomCharacter();
                }
            }
        }
        private void GiveHunterLoadout(PlayerController user)
        {
            //Give Dog
            if (!user.HasPickupID(300))
            {
                PickupObject DogItem = PickupObjectDatabase.GetById(300);
                LastOwner.AcquirePassiveItemPrefabDirectly(DogItem as PassiveItem);
            }
            //Give Crossbow
            if (!user.HasPickupID(12))
            {
                Gun Crossbow = (PickupObjectDatabase.GetById(12) as Gun);
                LastOwner.inventory.AddGunToInventory(Crossbow, true);
            }
            //Give Rusty Sidearm
            if (!user.HasPickupID(99) && !user.HasPickupID(810))
            {
                Gun RustySidearm = (PickupObjectDatabase.GetById(99) as Gun);
                LastOwner.inventory.AddGunToInventory(RustySidearm, true);
            }
        }
        private void GiveConvictLoadout(PlayerController user)
        {
            //Give Enraging Photo
            if (!user.HasPickupID(353))
            {
                PickupObject EnragingPhoto = PickupObjectDatabase.GetById(353);
                LastOwner.AcquirePassiveItemPrefabDirectly(EnragingPhoto as PassiveItem);
            }
            //Give SawedOff
            if (!user.HasPickupID(202))
            {
                Gun SawedOff = (PickupObjectDatabase.GetById(202) as Gun);
                LastOwner.inventory.AddGunToInventory(SawedOff, true);
            }
            //Give Budget Revolver
            if (!user.HasPickupID(80) && !user.HasPickupID(652))
            {
                Gun RustySidearm = (PickupObjectDatabase.GetById(80) as Gun);
                LastOwner.inventory.AddGunToInventory(RustySidearm, true);
            }
            //Give Molotov
            if (!user.HasPickupID(366))
            {
                PickupObject Molotov = PickupObjectDatabase.GetById(366);
                LootEngine.SpawnItem(Molotov.gameObject, LastOwner.specRigidbody.UnitCenter, Vector2.left, 1f, false, true, false);
            }
        }
        private void GivePilotLoadout(PlayerController user)
        {
            //Give Disarming Personality
            if (!user.HasPickupID(187))
            {
                PickupObject EnragingPhoto = PickupObjectDatabase.GetById(187);
                LastOwner.AcquirePassiveItemPrefabDirectly(EnragingPhoto as PassiveItem);
            }
            //Give Hidden Compartment
            if (!user.HasPickupID(473))
            {
                PickupObject EnragingPhoto = PickupObjectDatabase.GetById(473);
                LastOwner.AcquirePassiveItemPrefabDirectly(EnragingPhoto as PassiveItem);
            }
            //Give Rogue Special
            if (!user.HasPickupID(89) && !user.HasPickupID(651))
            {
                Gun RustySidearm = (PickupObjectDatabase.GetById(89) as Gun);
                LastOwner.inventory.AddGunToInventory(RustySidearm, true);
            }
            //Give Trusty Lockpicks
            if (!user.HasPickupID(356))
            {
                PickupObject Molotov = PickupObjectDatabase.GetById(356);
                LootEngine.SpawnItem(Molotov.gameObject, LastOwner.specRigidbody.UnitCenter, Vector2.left, 1f, false, true, false);
            }
        }
        private void GiveMarineLoadout(PlayerController user)
        {
            //Give Military Training
            if (!user.HasPickupID(354))
            {
                PickupObject CharacterPassive = PickupObjectDatabase.GetById(354);
                LastOwner.AcquirePassiveItemPrefabDirectly(CharacterPassive as PassiveItem);
            }
            //Give Marine Sidearm
            if (!user.HasPickupID(86) && !user.HasPickupID(809))
            {
                Gun CharacterStarterGun = (PickupObjectDatabase.GetById(86) as Gun);
                LastOwner.inventory.AddGunToInventory(CharacterStarterGun, true);
            }
            //Give Supply Drop
            if (!user.HasPickupID(77))
            {
                PickupObject CharacterActive = PickupObjectDatabase.GetById(77);
                LootEngine.SpawnItem(CharacterActive.gameObject, LastOwner.specRigidbody.UnitCenter, Vector2.left, 1f, false, true, false);
            }
            //Give 1 Armour
            LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(120).gameObject, LastOwner);
        }
        private void GiveRobotLoadout(PlayerController user)
        {
            //Give Battery Bullets
            if (!user.HasPickupID(410))
            {
                PickupObject CharacterPassive = PickupObjectDatabase.GetById(410);
                LastOwner.AcquirePassiveItemPrefabDirectly(CharacterPassive as PassiveItem);
            }
            //Give Robots Right Hand
            if (!user.HasPickupID(88) && !user.HasPickupID(812))
            {
                Gun CharacterStarterGun = (PickupObjectDatabase.GetById(88) as Gun);
                LastOwner.inventory.AddGunToInventory(CharacterStarterGun, true);
            }
            //Give Coolant Leak
            if (!user.HasPickupID(411))
            {
                PickupObject CharacterActive = PickupObjectDatabase.GetById(411);
                LootEngine.SpawnItem(CharacterActive.gameObject, LastOwner.specRigidbody.UnitCenter, Vector2.left, 1f, false, true, false);
            }
        }
        private void GiveBulletLoadout(PlayerController user)
        {
            //Give Live Ammo
            if (!user.HasPickupID(414))
            {
                PickupObject CharacterPassive = PickupObjectDatabase.GetById(414);
                LastOwner.AcquirePassiveItemPrefabDirectly(CharacterPassive as PassiveItem);
            }
            //Give Blasphemy
            if (!user.HasPickupID(417) && !user.HasPickupID(813))
            {
                Gun CharacterStarterGun = (PickupObjectDatabase.GetById(417) as Gun);
                LastOwner.inventory.AddGunToInventory(CharacterStarterGun, true);
            }
        }
    }
}

