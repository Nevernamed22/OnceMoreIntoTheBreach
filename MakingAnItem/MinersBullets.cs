using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;
using ItemAPI;

namespace NevernamedsItems
{
    public class MinersBullets : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Miners Bullets";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/minersbullets_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<MinersBullets>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "So we back in the mine";
            string longDesc = "Allows for the effortless destruction of cubes.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item

            //LIST OF SYNERGIES
            // Miners Bullets + Rolling Eye = Eye of the Spider
            // Miners Bullet + Lump of Space Metal = Miiiiiining away...

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.D;


        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += this.PostProcessProjectile;
            player.PostProcessBeam += this.PostProcessBeam;
            player.OnAnyEnemyReceivedDamage += this.OnEnemyDamaged;
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
                    sourceProjectile.OnHitEnemy += this.OnHitEnemy;
                }
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
                            if (otherRigidbody?.aiActor?.EnemyGuid == "98ca70157c364750a60f5e0084f9d3e2" && Owner.HasPickupID(190) || Owner.HasPickupID(90) || Owner.HasPickupID(167) || Owner.HasPickupID(Gungeon.Game.Items["nn:kaliber's_eye"].PickupObjectId))
                            {
                                float originalDamage = myRigidbody.projectile.baseData.damage;
                                myRigidbody.projectile.baseData.damage *= 2f;
                                GameManager.Instance.StartCoroutine(this.ChangeProjectileDamage(myRigidbody.projectile, originalDamage));
                            }
                            else if (otherRigidbody?.aiActor?.EnemyGuid != "98ca70157c364750a60f5e0084f9d3e2")
                            {
                                if (enemyGuid != "864ea5a6a9324efc95a0dd2407f42810" && enemyGuid != "0b547ac6b6fc4d68876a241a88f5ca6a")
                                {
                                    otherRigidbody.aiActor.healthHaver.ApplyDamage(myRigidbody.projectile.baseData.damage, Vector2.zero, "Erasure", CoreDamageTypes.None, DamageCategory.Unstoppable, true, null, false);
                                }
                                else
                                {
                                    float originalDamage = myRigidbody.projectile.baseData.damage;
                                    myRigidbody.projectile.baseData.damage *= 2f;
                                    GameManager.Instance.StartCoroutine(this.ChangeProjectileDamage(myRigidbody.projectile, originalDamage));
                                }
                            }
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
            EnemyGuidDatabase.Entries["mountain_cube"],
            EnemyGuidDatabase.Entries["lead_cube"],
            EnemyGuidDatabase.Entries["flesh_cube"],
            EnemyGuidDatabase.Entries["phaser_spider"],
            EnemyGuidDatabase.Entries["cubulon"],
            EnemyGuidDatabase.Entries["cubulead"],
        };
        private void OnEnemyDamaged(float damage, bool fatal, HealthHaver enemyHealth)
        {
            if (Commands.instakillersDoubleDamage == true && fatal)
            {
                if (Owner.HasPickupID(Gungeon.Game.Items["nn:lump_of_space_metal"].PickupObjectId) || Owner.HasPickupID(394))
                {
                    string enemyGuid = enemyHealth?.aiActor?.EnemyGuid;
                    if (!string.IsNullOrEmpty(enemyGuid))
                    {
                        foreach (var guid in badMenGoByeBye)
                        {
                            if (guid.Equals(enemyGuid))
                            {
                                if (enemyHealth?.aiActor?.EnemyGuid == "98ca70157c364750a60f5e0084f9d3e2" && Owner.HasPickupID(190) || Owner.HasPickupID(90) || Owner.HasPickupID(167) || Owner.HasPickupID(Gungeon.Game.Items["nn:kaliber's_eye"].PickupObjectId))
                                {
                                    SpawnCasings(enemyHealth.aiActor);
                                }
                                else if (enemyHealth?.aiActor?.EnemyGuid != "98ca70157c364750a60f5e0084f9d3e2")
                                {
                                    SpawnCasings(enemyHealth.aiActor);
                                }
                            }
                        }
                    }
                }
            }
        }
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
                            if (arg2?.aiActor?.EnemyGuid == "98ca70157c364750a60f5e0084f9d3e2" && Owner.HasPickupID(190) || Owner.HasPickupID(90) || Owner.HasPickupID(167) || Owner.HasPickupID(Gungeon.Game.Items["nn:kaliber's_eye"].PickupObjectId))
                            {
                                if (Commands.instakillersDoubleDamage == true)
                                {
                                    //float damageToDeal = arg1.baseData.damage;
                                    arg2.aiActor.healthHaver.ApplyDamage(1f, Vector2.zero, "Erasure", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                                }
                                else
                                {
                                    InstaKill(arg2.aiActor.healthHaver);
                                }
                                return;
                            } //Adds the 'Eye of the Spider' Synergy
                            else if (arg2?.aiActor?.EnemyGuid != "98ca70157c364750a60f5e0084f9d3e2")
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
                            else return;
                        }
                    }
                }
                catch (Exception e)
                {
                    ETGModConsole.Log(e.Message);
                }
            }
        }
        public void SpawnCasings(AIActor target)
        {
            LootEngine.SpawnItem(PickupObjectDatabase.GetById(68).gameObject, target.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
            LootEngine.SpawnItem(PickupObjectDatabase.GetById(68).gameObject, target.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
            LootEngine.SpawnItem(PickupObjectDatabase.GetById(68).gameObject, target.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
            LootEngine.SpawnItem(PickupObjectDatabase.GetById(68).gameObject, target.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
            LootEngine.SpawnItem(PickupObjectDatabase.GetById(68).gameObject, target.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
        }
        public void InstaKill(HealthHaver target)
        {
            try
            {
                //ETGModConsole.Log("This poor bastard is gonna die");
                if (Owner.HasPickupID(Gungeon.Game.Items["nn:lump_of_space_metal"].PickupObjectId) || Owner.HasPickupID(394))
                {
                    if (target.healthHaver.IsAlive)
                    {
                        SpawnCasings(target.aiActor);
                    }
                    target.ApplyDamage(1E+07f, Vector2.zero, "Erasure", CoreDamageTypes.None, DamageCategory.Unstoppable, true, null, false);
                }
                else
                {
                    target.ApplyDamage(1E+07f, Vector2.zero, "Erasure", CoreDamageTypes.None, DamageCategory.Unstoppable, true, null, false);
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
            player.OnAnyEnemyReceivedDamage -= this.OnEnemyDamaged;
            return debrisObject;
        }
        protected override void OnDestroy()
        {
            Owner.PostProcessProjectile -= this.PostProcessProjectile;
            Owner.PostProcessBeam -= this.PostProcessBeam;
            Owner.OnAnyEnemyReceivedDamage -= this.OnEnemyDamaged;
            base.OnDestroy();
        }

    }
}
