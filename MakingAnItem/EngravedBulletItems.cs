﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;
using System.Collections;

namespace NevernamedsItems
{
    public class UnengravedBullets : PlayerItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Unengraved Bullets";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/unengravedbullets_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<UnengravedBullets>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Waiting for the right moment...";
            string longDesc = "The first enemy shot while this item is active becomes permanently insta-killable.\n\n" + "These bullets, while unremarkable at the moment, are brimming with murderous potential.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.None, 500);


            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.B;

            item.AddToSubShop(ItemBuilder.ShopType.Trorc);
        }

        float duration = 4;
        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {
            try
            {
                //ETGModConsole.Log("posting the process to the projectile in the mail");
                //ETGModConsole.Log($"Proj Null: {projectile == null} | OnHitEnemy null: {projectile?.OnHitEnemy == null}");
                sourceProjectile.OnHitEnemy += this.OnHitEnemy;
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
            }
        }
        private void PostProcessBeam(BeamController sourceBeam)
        {
            try
            {
                sourceBeam.projectile.OnHitEnemy += this.OnHitEnemy;
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
            }
        }
        protected override void DoEffect(PlayerController user)
        {
            //Play a sound effect
            //Activates the effect
            StartEffect(user);

            //start a coroutine which calls the EndEffect method when the item's effect duration runs out
            StartCoroutine(ItemBuilder.HandleDuration(this, duration, user, EndEffect));
        }

        private void StartEffect(PlayerController user)
        {
            ableToGiveItem = true;
            user.PostProcessProjectile += this.PostProcessProjectile;
            user.PostProcessBeam += this.PostProcessBeam;
            //ETGModConsole.Log("Unengraved Bullets has been activated");
        }
        private void EndEffect(PlayerController user)
        {
            ableToGiveItem = false;
            user.PostProcessProjectile -= this.PostProcessProjectile;
            user.PostProcessBeam -= this.PostProcessBeam;
        }
        protected override void OnPreDrop(PlayerController user)
        {
            if (base.IsCurrentlyActive)
            {
                base.IsCurrentlyActive = false;
                EndEffect(user);
            }
        }

        bool ableToGiveItem;
        public static string engravedEnemy;

        private void OnHitEnemy(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
        {
            string enemyToKillGuid = arg2?.aiActor?.EnemyGuid;
            if (!string.IsNullOrEmpty(enemyToKillGuid))
            {
                //ETGModConsole.Log("The enemy you just hit had this guid: " + enemyToKillGuid);
                if (enemyBlacklist.Contains(enemyToKillGuid))
                {
                    ETGModConsole.Log("The selected enemy (" + enemyToKillGuid + ") is on the enemy Blacklist, either because it is a boss, unkillable, or some sort of harmless thing I can't imagine someone wanting to instakill - NN");
                }
                else if (ableToGiveItem == true)
                {
                    ableToGiveItem = false;
                    engravedEnemy = enemyToKillGuid;
                    string header = "Bullets Engraved";
                    string text = "";
                    this.Notify(header, text);
                    EndEffect(LastOwner);
                    GiveEngravedBulletsRemoveUnengravedBullets();
                }
                else if (ableToGiveItem == false) return;
                else return;
            }
        }

        private void Notify(string header, string text)
        {
            tk2dBaseSprite notificationObjectSprite = GameUIRoot.Instance.notificationController.notificationObjectSprite;
            GameUIRoot.Instance.notificationController.DoCustomNotification(header, text, notificationObjectSprite.Collection, notificationObjectSprite.spriteId, UINotificationController.NotificationColor.PURPLE, true, true);
        }
        private void GiveEngravedBulletsRemoveUnengravedBullets()
        {
            var engravedBulletsItem = Gungeon.Game.Items["nn:engraved_bullets"];
            LastOwner.AcquirePassiveItemPrefabDirectly(engravedBulletsItem as PassiveItem);
            LastOwner.RemoveActiveItem(this.PickupObjectId);
        }

        public List<string> enemyBlacklist = new List<string>()
        {
            "fa6a7ac20a0e4083a4c2ee0d86f8bbf7", //Red Caped Bullet Kin
            "47bdfec22e8e4568a619130a267eab5b", //Tankers summoned by the Treadnaught
            "ea40fcc863d34b0088f490f4e57f8913", //Smiley
            "c00390483f394a849c36143eb878998f", //Shades
            "ec6b674e0acd4553b47ee94493d66422", //Gatling Gull
            "ffca09398635467da3b1f4a54bcfda80", //Bullet King
            "1b5810fafbec445d89921a4efb4e42b7", //Blobulord
            "4b992de5b4274168a8878ef9bf7ea36b", //Beholster
            "c367f00240a64d5d9f3c26484dc35833", //Gorgun
            "da797878d215453abba824ff902e21b4", //Ammoconda
            "5729c8b5ffa7415bb3d01205663a33ef", //Old King
            "fa76c8cfdf1c4a88b55173666b4bc7fb", //Treadnaught
            "8b0dd96e2fe74ec7bebc1bc689c0008a", //Mine Flayer
            "5e0af7f7d9de4755a68d2fd3bbc15df4", //Cannonbalrog
            "9189f46c47564ed588b9108965f975c9", //Door Lord
            "6868795625bd46f3ae3e4377adce288b", //Resourceful Rat
            "4d164ba3f62648809a4a82c90fc22cae", //Rat Mech
            "6c43fddfd401456c916089fdd1c99b1c", //High Priest
            "3f11bbbc439c4086a180eb0fb9990cb4", //Kill Pillars
            "f3b04a067a65492f8b279130323b41f0", //Wallmonger
            "41ee1c8538e8474a82a74c4aff99c712", //Helicopter Agunim
            "465da2bb086a4a88a803f79fe3a27677", //Dragun
            "05b8afe0b6cc4fffa9dc6036fa24c8ec", //Advanced Dragun
            "cd88c3ce60c442e9aa5b3904d31652bc", //Lich
            "68a238ed6a82467ea85474c595c49c6e", //Megalich
            "7c5d5f09911e49b78ae644d2b50ff3bf", //Infinilich
            "76bc43539fc24648bff4568c75c686d1", //Chicken
            "0ff278534abb4fbaaa65d3f638003648", //Poopulons Corn
            "6ad1cafc268f4214a101dca7af61bc91", //Passive Rat
            "14ea47ff46b54bb4a98f91ffcffb656d", //Candle Rat
        };
    }

    internal class StartEffect
    {
    }

    public class EngravedBullets : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Engraved Bullets";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/engravedbullets_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<EngravedBullets>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Bullet with your name on it";
            string longDesc = "These bullets are specially made to absolutely annihilate one specific foe.\n\n" + "They may run. They may hide. But you will find them";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item


            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.EXCLUDED;
        }
        string enemyToKillEngraved = UnengravedBullets.engravedEnemy;
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += this.PostProcessProjectile;
            player.PostProcessBeam += this.PostProcessBeam;
        }
        private void HandlePreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
        {
            string enemyGuid = otherRigidbody?.aiActor?.EnemyGuid;
            if (!string.IsNullOrEmpty(enemyGuid))
            {
                if (otherRigidbody && otherRigidbody.healthHaver)
                {

                    if (enemyGuid == enemyToKillEngraved)
                    {
                        float originalDamage = myRigidbody.projectile.baseData.damage;
                        myRigidbody.projectile.baseData.damage *= 2f;
                        GameManager.Instance.StartCoroutine(this.ChangeProjectileDamage(myRigidbody.projectile, originalDamage));
                    }

                }
            }
        }
        private IEnumerator ChangeProjectileDamage(Projectile bullet, float oldDamage)
        {
            yield return new WaitForSeconds(0.1f);
            if (bullet != null)
            {
                bullet.baseData.damage = oldDamage;
            }
            yield break;
        }
        private void OnHitEnemy(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
        {
            string enemyGuid = arg2?.aiActor?.EnemyGuid;
            if (!string.IsNullOrEmpty(enemyGuid))
            {
                if (enemyGuid == enemyToKillEngraved)
                {
                    if (enemyGuid == "062b9b64371e46e195de17b6f10e47c8" && Commands.instakillersDoubleDamage == false)
                    {
                        arg2.aiActor.EraseFromExistenceWithRewards();
                    }
                    else
                    {
                        if (Commands.instakillersDoubleDamage == true)
                        {
                            //float damageToDeal = arg1.baseData.damage;
                            arg2.aiActor.healthHaver.ApplyDamage(1f, Vector2.zero, "Erasure", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                        }
                        else
                        {
                            InstaKill(arg2.aiActor.healthHaver);
                            return;
                        }
                    }
                }
            }
        }
        public void InstaKill(HealthHaver target)
        {
            try
            {
                //ETGModConsole.Log("This poor bastard is gonna die");
                target.ApplyDamage(1E+07f, Vector2.zero, "Erasure", CoreDamageTypes.None, DamageCategory.Unstoppable, true, null, false);
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
            }
        }
        private void PostProcessBeam(BeamController sourceBeam)
        {
            try
            {
                sourceBeam.projectile.OnHitEnemy += this.OnHitEnemy;
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
            }
        }
        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {
            try
            {
                if (Commands.instakillersDoubleDamage == true)
                {
                    sourceProjectile.projectile.specRigidbody.OnPreRigidbodyCollision += this.HandlePreCollision;
                }
                else
                {
                    sourceProjectile.OnHitEnemy += this.OnHitEnemy;
                }
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
            }
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.PostProcessProjectile -= this.PostProcessProjectile;
            player.PostProcessBeam -= this.PostProcessBeam;
            return debrisObject;
        }
        protected override void OnDestroy()
        {
            Owner.PostProcessProjectile -= this.PostProcessProjectile;
            Owner.PostProcessBeam -= this.PostProcessBeam;
            base.OnDestroy();
        }
    }
}
