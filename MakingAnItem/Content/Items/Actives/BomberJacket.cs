using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    class BomberJacket : PlayerItem
    {
        public static void Init()
        {
            PlayerItem item = ItemSetup.NewItem<BomberJacket>(
               "Bomber Jacket",
               "Kamakablooey",
               "Use to create an explosion around yourself." + "\n\nBrought to the Gungeon by a rather unscrupulous individual, it has since been modified to be safe to the user, and been renamed so it can be shown on the internet without losing one's entire career.",
               "bomberjacket_icon") as PlayerItem;

        
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 120);
            item.AddPassiveStatModifier( PlayerStats.StatType.AdditionalItemCapacity, 2f, StatModifier.ModifyMethod.ADDITIVE);
            item.consumable = false;
            item.quality = ItemQuality.D;

            item.AddToSubShop(ItemBuilder.ShopType.Trorc);
            ID = item.PickupObjectId;
        }
        public static int ID;

        //Add the item's functionality down here! I stole most of this from the Stuffed Star active item code!
        public override void DoEffect(PlayerController user)
        {
            DoSafeExplosion(user.specRigidbody.UnitCenter);
        }
        ExplosionData smallPlayerSafeExplosion = new ExplosionData()
        {
            damageRadius = 2.5f,
            damageToPlayer = 0f,
            doDamage = true,
            damage = 25,
            doDestroyProjectiles = true,
            doForce = true,
            debrisForce = 30f,
            preventPlayerForce = true,
            explosionDelay = 0.1f,
            usesComprehensiveDelay = false,
            doScreenShake = true,
            playDefaultSFX = true,
        };
        ExplosionData bigPlayerSafeExplosion = new ExplosionData()
        {
            damageRadius = 4f,
            damageToPlayer = 0f,
            doDamage = true,
            damage = 50,
            doDestroyProjectiles = true,
            doForce = true,
            debrisForce = 60f,
            preventPlayerForce = true,
            explosionDelay = 0.1f,
            usesComprehensiveDelay = false,
            doScreenShake = true,
            playDefaultSFX = true,
        };
        public void DoSafeExplosion(Vector3 position)
        {
            if (LastOwner.HasPickupID(332))
            {
                var defaultExplosion = GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultExplosionData;
                bigPlayerSafeExplosion.effect = defaultExplosion.effect;
                bigPlayerSafeExplosion.ignoreList = defaultExplosion.ignoreList;
                bigPlayerSafeExplosion.ss = defaultExplosion.ss;
                Exploder.Explode(position, bigPlayerSafeExplosion, Vector2.zero);
            }
            else
            {
                var defaultExplosion = GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultSmallExplosionData;
                smallPlayerSafeExplosion.effect = defaultExplosion.effect;
                smallPlayerSafeExplosion.ignoreList = defaultExplosion.ignoreList;
                smallPlayerSafeExplosion.ss = defaultExplosion.ss;
                Exploder.Explode(position, smallPlayerSafeExplosion, Vector2.zero);
            }
        }
    }
}
