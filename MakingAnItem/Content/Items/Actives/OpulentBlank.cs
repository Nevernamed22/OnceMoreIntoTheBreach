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
            if (user.PlayerHasActiveSynergy("Wealth Untold") && user.CurrentRoom != null)
            {
                List<AIActor> activeEnemies = user.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);

                if (activeEnemies != null)
                {
                    for (int i = 0; i < activeEnemies.Count; i++)
                    {
                        AIActor aiactor = activeEnemies[i];
                        if (aiactor.IsNormalEnemy)
                        {
                            aiactor.gameActor.ApplyEffect(new GameActorGildedEffect()
                            {
                                duration = 50,
                                stackMode = GameActorEffect.EffectStackingMode.Refresh,
                            }, 1f, null);
                        }
                    }
                }
            }
            user.ForceBlank();
        }       
    }
}