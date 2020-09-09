using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;
using ItemAPI;

namespace NevernamedsItems
{
    public class OsteoporosisBullets : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Osteoporosis Bullets";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/osteoporosisbullets_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<OsteoporosisBullets>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Bad To The Bone";
            string longDesc = "These bullets are a skele-TON of trouble for the various BONEheads found throughout the Gungeon, leaving them well and truly BONED";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE);

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.A;
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
                    //ETGModConsole.Log("posting the process to the projectile in the mail");
                    //ETGModConsole.Log($"Proj Null: {projectile == null} | OnHitEnemy null: {projectile?.OnHitEnemy == null}");
                    isBeam = false;
                    sourceProjectile.OnHitEnemy += this.OnHitEnemy;
                }
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
            }
        }
        public bool isBeam = true;
        private void PostProcessBeam(BeamController sourceBeam)
        {
            try
            {
                isBeam = true;
                sourceBeam.projectile.OnHitEnemy += this.OnHitEnemy;
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
                ETGModConsole.Log(e.StackTrace);
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

        public static List<string> badMenGoByeBye = new List<string>()
        {
            EnemyGuidDatabase.Entries["skullet"],
            EnemyGuidDatabase.Entries["skullmet"],
            EnemyGuidDatabase.Entries["skusket"],
            EnemyGuidDatabase.Entries["black_skusket"],
            EnemyGuidDatabase.Entries["skusket_head"],
            EnemyGuidDatabase.Entries["shelleton"],
            EnemyGuidDatabase.Entries["revolvenant"],
            EnemyGuidDatabase.Entries["lich"],
            EnemyGuidDatabase.Entries["megalich"],
            EnemyGuidDatabase.Entries["infinilich"],
            EnemyGuidDatabase.Entries["cannonbalrog"],
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
                            if (guid.Equals(EnemyGuidDatabase.Entries["lich"]) || guid.Equals(EnemyGuidDatabase.Entries["megalich"]) || guid.Equals(EnemyGuidDatabase.Entries["infinilich"]))
                            {
                                if (Commands.instakillersDoubleDamage == false)
                                {
                                    DealDamageLich(arg2.aiActor.healthHaver);
                                }
                            }
                            else if (guid.Equals(EnemyGuidDatabase.Entries["cannonbalrog"]))
                            {
                                if (Commands.instakillersDoubleDamage == false)
                                {
                                    DealDamageCannonbalrog(arg2.aiActor.healthHaver);
                                }
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
        public void DealDamageLich(HealthHaver target)
        {
            try
            {
                if (isBeam)
                {
                    target.ApplyDamage(2f, Vector2.zero, "Erasure", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                }
                else
                {
                    target.ApplyDamage(15f, Vector2.zero, "Erasure", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);

                }
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
            }
        }
        public void DealDamageCannonbalrog(HealthHaver target)
        {
            try
            {
                target.ApplyDamage(12f, Vector2.zero, "Erasure", CoreDamageTypes.None, DamageCategory.Unstoppable, true, null, false);
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
