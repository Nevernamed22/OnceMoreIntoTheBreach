using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dungeonator;
using Alexandria.ItemAPI;
using UnityEngine;

namespace NevernamedsItems
{
    class OpulentBlank : PlayerItem
    {
        public static void Init()
        {
            PlayerItem item = ItemSetup.NewItem<OpulentBlank>(
            "Opulent Blank",
            "Spin Bullets To Gold",
            "Turns all enemy bullets to gold. One use." + "\n\nAn extremely rare variant of the regular Blank.",
            "opulentblank_icon") as PlayerItem;
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.None, 1000);
            item.consumable = true;
            item.quality = ItemQuality.D;
            item.AddToSubShop(ItemBuilder.ShopType.OldRed);
        }
        public override void DoEffect(PlayerController user)
        {
            for (int i = 0; i < StaticReferenceManager.AllProjectiles.Count; i++)
            {
                Projectile projectile = StaticReferenceManager.AllProjectiles[i];
                if (projectile)
                {
                    if (!(projectile.Owner is PlayerController))
                    {
                        if (projectile.collidesWithPlayer || projectile.Owner is AIActor)
                        {
                            if (!projectile.ImmuneToBlanks)
                            {
                                LootEngine.SpawnCurrency(projectile.specRigidbody.UnitCenter, 1);
                            }
                        }
                    }
                }
            }
            user.ForceBlank();
        }       
    }
}