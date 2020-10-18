using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;
using ItemAPI;

namespace NevernamedsItems
{
    public class ERRORShells : PassiveItem
    {
        public static void Init()
        {
            //ETGModConsole.Log("Error Shells was initialised");
            //The name of the item
            string itemName = "ERROR Shells";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/errorshells_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<ERRORShells>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "What do you mean 74 errors!?";
            string longDesc = "Picks a random selection of enemies to become highly efficient against.\n\n" + "These bullets were moulded by the numerous errors that went into making them, thanks to their incompetent smith.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item


            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.A;
            //ETGModConsole.Log("ERROR shells finished it's initialisation");
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            PickEnemies();
            player.PostProcessProjectile += this.PostProcessProjectile;
            player.PostProcessBeam += this.PostProcessBeam;
        }

        bool hasPicked = false;
        private void PickEnemies()
        {
            if (hasPicked) return;
            for (int i = 0; i < 10; i++)
            {
                string guid, enemyName;
                do
                {
                    int r = UnityEngine.Random.Range(0, EnemyGuidDatabase.Entries.Count);
                    enemyName = EnemyGuidDatabase.Entries.Keys.ElementAt(r);
                    guid = EnemyGuidDatabase.Entries[enemyName];
                } while (badMenToNotGoByeBye.Contains(guid) || badMenGoByeBye.Contains(guid));
                badMenGoByeBye.Add(guid);
                ETGModConsole.Log("Enemy Chosen for Death: " + enemyName);
            }
            hasPicked = true;
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

        public List<string> badMenGoByeBye = new List<string>()
        {
        };
        public List<string> badMenToNotGoByeBye = new List<string>()
        {
            EnemyGuidDatabase.Entries["gummy_spent"],
            EnemyGuidDatabase.Entries["western_shotgun_kin"],
            EnemyGuidDatabase.Entries["pirate_shotgun_kin"],
            EnemyGuidDatabase.Entries["blobuloid"],
            EnemyGuidDatabase.Entries["blobulin"],
            EnemyGuidDatabase.Entries["poisbuloid"],
            EnemyGuidDatabase.Entries["poisbulin"],
            EnemyGuidDatabase.Entries["black_skusket"],
            EnemyGuidDatabase.Entries["skusket_head"],
            EnemyGuidDatabase.Entries["shotgat"],
            EnemyGuidDatabase.Entries["necronomicon"],
            EnemyGuidDatabase.Entries["tablet_bookllett"],
            EnemyGuidDatabase.Entries["pot_fairy"],
            EnemyGuidDatabase.Entries["fridge_maiden"],
            EnemyGuidDatabase.Entries["bullet_king"],
            EnemyGuidDatabase.Entries["blobulord"],
            EnemyGuidDatabase.Entries["old_king"],
            EnemyGuidDatabase.Entries["lich"],
            EnemyGuidDatabase.Entries["megalich"],
            EnemyGuidDatabase.Entries["infinilich"],
            EnemyGuidDatabase.Entries["tiny_blobulord"],
            EnemyGuidDatabase.Entries["cannonbalrog"],
            EnemyGuidDatabase.Entries["brown_chest_mimic"],
            EnemyGuidDatabase.Entries["blue_chest_mimic"],
            EnemyGuidDatabase.Entries["green_chest_mimic"],
            EnemyGuidDatabase.Entries["red_chest_mimic"],
            EnemyGuidDatabase.Entries["black_chest_mimic"],
            EnemyGuidDatabase.Entries["rat_chest_mimic"],
            EnemyGuidDatabase.Entries["pedestal_mimic"],
            EnemyGuidDatabase.Entries["wall_mimic"],
            EnemyGuidDatabase.Entries["door_lord"],
            EnemyGuidDatabase.Entries["brollet"],
            EnemyGuidDatabase.Entries["grenat"],
            EnemyGuidDatabase.Entries["det"],
            EnemyGuidDatabase.Entries["x_det"],
            EnemyGuidDatabase.Entries["diagonal_x_det"],
            EnemyGuidDatabase.Entries["vertical_det"],
            EnemyGuidDatabase.Entries["horizontal_det"],
            EnemyGuidDatabase.Entries["diagonal_det"],
            EnemyGuidDatabase.Entries["vertical_x_det"],
            EnemyGuidDatabase.Entries["horizontal_x_det"],
            EnemyGuidDatabase.Entries["m80_kin"],
            EnemyGuidDatabase.Entries["mine_flayers_claymore"],
            EnemyGuidDatabase.Entries["office_bullet_kin"],
            EnemyGuidDatabase.Entries["office_bullette_kin"],
            EnemyGuidDatabase.Entries["western_bullet_kin"],
            EnemyGuidDatabase.Entries["pirate_bullet_kin"],
            EnemyGuidDatabase.Entries["armored_bullet_kin"],
            EnemyGuidDatabase.Entries["spectre"],
            EnemyGuidDatabase.Entries["bullat"],
            EnemyGuidDatabase.Entries["spirat"],
            EnemyGuidDatabase.Entries["gargoyle"],
            EnemyGuidDatabase.Entries["grey_cylinder"],
            EnemyGuidDatabase.Entries["red_cylinder"],
            EnemyGuidDatabase.Entries["bullet_mech"],
            EnemyGuidDatabase.Entries["arrow_head"],
            EnemyGuidDatabase.Entries["musketball"],
            EnemyGuidDatabase.Entries["western_cactus"],
            EnemyGuidDatabase.Entries["candle_kin"],
            EnemyGuidDatabase.Entries["chameleon"],
            EnemyGuidDatabase.Entries["bird_parrot"],
            EnemyGuidDatabase.Entries["western_snake"],
            EnemyGuidDatabase.Entries["kalibullet"],
            EnemyGuidDatabase.Entries["kbullet"],
            EnemyGuidDatabase.Entries["blue_fish_bullet_kin"],
            EnemyGuidDatabase.Entries["green_fish_bullet_kin"],
            EnemyGuidDatabase.Entries["ammoconda_ball"],
            EnemyGuidDatabase.Entries["summoned_treadnaughts_bullet_kin"],
            EnemyGuidDatabase.Entries["mine_flayers_bell"],
            EnemyGuidDatabase.Entries["titan_bullet_kin"],
            EnemyGuidDatabase.Entries["grip_master"],
            EnemyGuidDatabase.Entries["titan_bullet_kin_boss"],
            EnemyGuidDatabase.Entries["titaness_bullet_kin_boss"],
            EnemyGuidDatabase.Entries["fusebot"],
            EnemyGuidDatabase.Entries["candle_guy"],
            EnemyGuidDatabase.Entries["draguns_knife"],
            EnemyGuidDatabase.Entries["dragun_knife_advanced"],
            EnemyGuidDatabase.Entries["marines_past_imp"],
            EnemyGuidDatabase.Entries["convicts_past_soldier"],
            EnemyGuidDatabase.Entries["robots_past_terminator"],
            EnemyGuidDatabase.Entries["bullet_kings_toadie"],
            EnemyGuidDatabase.Entries["bullet_kings_toadie_revenge"],
            EnemyGuidDatabase.Entries["old_kings_toadie"],
            EnemyGuidDatabase.Entries["chicken"],
            EnemyGuidDatabase.Entries["rat"],
            EnemyGuidDatabase.Entries["dragun_egg_slimeguy"],
            EnemyGuidDatabase.Entries["poopulons_corn"],
            EnemyGuidDatabase.Entries["rat_candle"],
            EnemyGuidDatabase.Entries["robots_past_critter_3"],
            EnemyGuidDatabase.Entries["robots_past_critter_2"],
            EnemyGuidDatabase.Entries["robots_past_critter_1"],
            EnemyGuidDatabase.Entries["snake"],
            EnemyGuidDatabase.Entries["beholster"],
            EnemyGuidDatabase.Entries["treadnaught"],
            EnemyGuidDatabase.Entries["cannonbalrog"],
            EnemyGuidDatabase.Entries["gorgun"],
            EnemyGuidDatabase.Entries["gatling_gull"],
            EnemyGuidDatabase.Entries["ammoconda"],
            EnemyGuidDatabase.Entries["dragun"],
            EnemyGuidDatabase.Entries["dragun_advanced"],
            EnemyGuidDatabase.Entries["helicopter_agunim"],
            EnemyGuidDatabase.Entries["super_space_turtle_dummy"],
            EnemyGuidDatabase.Entries["cop_android"],
            EnemyGuidDatabase.Entries["portable_turret"],
            EnemyGuidDatabase.Entries["friendly_gatling_gull"],
            EnemyGuidDatabase.Entries["cucco"],
            EnemyGuidDatabase.Entries["cop"],
            EnemyGuidDatabase.Entries["blockner"],
        };

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
        private void OnHitEnemy(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
        {
            string enemyGuid = arg2?.aiActor?.EnemyGuid;
            if (!string.IsNullOrEmpty(enemyGuid))
            {
                try
                {
                    //ETGModConsole.Log("This enemy's Guid is: " + enemyGuid);


                    if (badMenGoByeBye.Contains(enemyGuid))
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

