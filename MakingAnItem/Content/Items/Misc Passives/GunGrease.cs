using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    class GunGrease : PassiveItem
    {
        public static void Init()
        {
            PassiveItem item = ItemSetup.NewItem<GunGrease>(
               "Gun Grease",
               "Slippery Slope",
               "Slightly increases reload speed." + "\n\nSlathering your weapons in this fluid makes them function smoother, and it also seems to transfer a significant amount of oiley muck to your target. If only you had some way to set it alight.",
               "gungrease_icon") as PassiveItem;

            item.AddPassiveStatModifier( PlayerStats.StatType.ReloadSpeed, 0.85f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            item.quality = PickupObject.ItemQuality.D;
            item.AddToSubShop(ItemBuilder.ShopType.Trorc);
            item.AddToSubShop(ItemBuilder.ShopType.Goopton);

            AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
            GunGrease.goopDefs = new List<GoopDefinition>();
            foreach (string text in GunGrease.goops)
            {
                GoopDefinition goopDefinition;
                try
                {
                    GameObject gameObject = assetBundle.LoadAsset(text) as GameObject;
                    goopDefinition = gameObject.GetComponent<GoopDefinition>();
                }
                catch
                {
                    goopDefinition = (assetBundle.LoadAsset(text) as GoopDefinition);
                }
                goopDefinition.name = text.Replace("assets/data/goops/", "").Replace(".asset", "");
                GunGrease.goopDefs.Add(goopDefinition);
            }
            List<GoopDefinition> list = GunGrease.goopDefs;
            GunGreaseID = item.PickupObjectId;
        }
        public static int GunGreaseID;
        int goopNumber;
        float goopRadius;
        private void OnEnemyDamaged(float damage, bool fatal, HealthHaver enemyHealth)
        {
            if (enemyHealth.aiActor && fatal)
            { //Napalm Strike, Big Boy, Hot Lead, Ring of Fire Resistance, Gungeon Pepper
                if (Owner.HasPickupID(242) || Owner.HasPickupID(443) || Owner.HasPickupID(295) || Owner.HasPickupID(191) || Owner.HasPickupID(253))
                {
                    goopNumber = 1;
                }
                else
                {
                    goopNumber = 0;
                } //Sanctified Oil, or Oiled Cylinder
                if (Owner.HasPickupID(Gungeon.Game.Items["nn:sanctified_oil"].PickupObjectId) || Owner.HasPickupID(165))
                {
                    goopRadius = 6f;
                }
                else
                {
                    goopRadius = 3f;
                }
                float duration = 1f;
                DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(GunGrease.goopDefs[goopNumber]).TimedAddGoopCircle(enemyHealth.specRigidbody.UnitCenter, goopRadius, duration, false);
            }
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnAnyEnemyReceivedDamage += this.OnEnemyDamaged;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.OnAnyEnemyReceivedDamage -= this.OnEnemyDamaged;
            return debrisObject;
        }
        public override void OnDestroy()
        {
            if (Owner) Owner.OnAnyEnemyReceivedDamage -= this.OnEnemyDamaged;
            base.OnDestroy();
        }
        private static List<GoopDefinition> goopDefs;

        private static string[] goops = new string[]
        {
            "assets/data/goops/oil goop.asset",
            "assets/data/goops/napalmgoopthatworks.asset",
        };
    }
}
