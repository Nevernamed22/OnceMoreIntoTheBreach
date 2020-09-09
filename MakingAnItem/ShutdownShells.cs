using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;
using ItemAPI;

namespace NevernamedsItems
{
    public class ShutdownShells : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Shutdown Shells";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/shutdownshells_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<ShutdownShells>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "No, shut them all down!";
            string longDesc = "These brutal shells are designed to burrow into the long spines of the Gungeon's Shotgun Gundead and wreak havoc, causing a complete shutdown of the central nervous system.\n\n" + "Not compatible with other lifeforms.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item


            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.B;


        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += this.PostProcessProjectile;
            player.PostProcessBeam += this.PostProcessBeam;
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
        private void HandlePreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
        {
            string enemyGuid = otherRigidbody?.aiActor?.EnemyGuid;
            if (!string.IsNullOrEmpty(enemyGuid))
            {
                if (otherRigidbody && otherRigidbody.healthHaver)
                {
                    foreach (var guid in badMenGoByeBye)
                    {
                        if (guid.Equals(enemyGuid))
                        {
                            float originalDamage = myRigidbody.projectile.baseData.damage;
                            myRigidbody.projectile.baseData.damage *= 2f;
                            GameManager.Instance.StartCoroutine(this.ChangeProjectileDamage(myRigidbody.projectile, originalDamage));
                        }
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

        public static List<string> badMenGoByeBye = new List<string>()
        {
            EnemyGuidDatabase.Entries["red_shotgun_kin"],
            EnemyGuidDatabase.Entries["blue_shotgun_kin"],
            EnemyGuidDatabase.Entries["veteran_shotgun_kin"],
            EnemyGuidDatabase.Entries["mutant_shotgun_kin"],
            EnemyGuidDatabase.Entries["executioner"],
            EnemyGuidDatabase.Entries["ashen_shotgun_kin"],
            EnemyGuidDatabase.Entries["shotgrub"],
            EnemyGuidDatabase.Entries["creech"],
            EnemyGuidDatabase.Entries["western_shotgun_kin"],
            EnemyGuidDatabase.Entries["shotgat"],
            EnemyGuidDatabase.Entries["pirate_shotgun_kin"],
        };

        private void OnHitEnemy(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
        {
            string enemyGuid = arg2?.aiActor?.EnemyGuid;
            if (!string.IsNullOrEmpty(enemyGuid))
            {
                try
                {
                    //ETGModConsole.Log("This enemy's Guid is: " + enemyGuid);
                    foreach (var guid in badMenGoByeBye)
                    {
                        if (guid.Equals(enemyGuid))
                        {
                            if (Commands.instakillersDoubleDamage == true)
                            {

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
                catch (Exception e)
                {
                    ETGModConsole.Log(e.Message);
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
