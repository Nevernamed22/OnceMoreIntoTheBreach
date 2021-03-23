using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;
using ItemAPI;
using SaveAPI;

namespace NevernamedsItems
{
    public class MinersBullets : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Miners Bullets";
            string resourceName = "NevernamedsItems/Resources/minersbullets_icon";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<MinersBullets>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "So we back in the mine";
            string longDesc = "Allows for the effortless destruction of cubes.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");
            item.quality = PickupObject.ItemQuality.D;

            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.PURCHASED_MINERSBULLETS, true);
            item.AddItemToDougMetaShop(8);
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
            InstaKillEnemyTypeBehaviour instakill = sourceProjectile.gameObject.GetOrAddComponent<InstaKillEnemyTypeBehaviour>();
            instakill.EnemyTypeToKill.AddRange(EasyEnemyTypeLists.CubicEnemies);
            if (Owner.PlayerHasActiveSynergy("Eye of the Spider")) instakill.EnemyTypeToKill.Add(EnemyGuidDatabase.Entries["phaser_spider"]);
        }
        private void PostProcessBeam(BeamController sourceBeam)
        {
            if (sourceBeam.projectile)
            {
                this.PostProcessProjectile(sourceBeam.projectile, 1);
            }
        }
        private void OnEnemyDamaged(float damage, bool fatal, HealthHaver enemyHealth)
        {
            if (fatal && Owner)
            {
                if (Owner.PlayerHasActiveSynergy("Miiiining Away~"))
                {
                    string enemyGuid = enemyHealth?.aiActor?.EnemyGuid;
                    if (enemyHealth?.aiActor?.EnemyGuid == "98ca70157c364750a60f5e0084f9d3e2" && Owner.PlayerHasActiveSynergy("Eye of the Spider"))
                    {
                        LootEngine.SpawnCurrency(enemyHealth.sprite.WorldCenter, 5);
                    }
                    else if (EasyEnemyTypeLists.CubicEnemies.Contains(enemyGuid))
                    {
                        LootEngine.SpawnCurrency(enemyHealth.sprite.WorldCenter, 5);
                    }
                }
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

