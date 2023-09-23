using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using ItemAPI;
using SaveAPI;

namespace NevernamedsItems
{
    class Mutagen : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<Mutagen>(
            "Mutagen",
            "Rampant Mutation",
            "Heals a small amount whenever the afflicted individual defeats a boss." + "\n\nThis mutagen progresses in stages, just like the Gungeon itself.",
            "mutagen_icon");
            item.quality = PickupObject.ItemQuality.C;
            item.AddToSubShop(ItemBuilder.ShopType.Goopton);
            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.HAS_BEATEN_BOSS_BY_SKIN_OF_TEETH, true);
        }

        private void OnEnemyDamaged(float damage, bool fatal, HealthHaver enemy)
        {
            if (enemy.aiActor && enemy.IsBoss && fatal == true)
            {
                if (Owner.ForceZeroHealthState)
                {
                    LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(120).gameObject, Owner);
                    Owner.PlayEffectOnActor(ResourceCache.Acquire("Global VFX/vfx_healing_sparkles_001") as GameObject, Vector3.zero, true, false, false);
                    AkSoundEngine.PostEvent("Play_OBJ_heart_heal_01", base.gameObject);
                    if (Owner.HasPickupID(314))
                    {
                        LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(120).gameObject, Owner);
                    }
                    if (Owner.HasPickupID(259))
                    {
                        LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(120).gameObject, Owner);
                    }
                }
                else 
                {
                    float amountToHeal = 1;
                    if (Owner.HasPickupID(259))
                    {
                        amountToHeal = 1.5f;
                    }
                    Owner.healthHaver.ApplyHealing(amountToHeal);
                    Owner.PlayEffectOnActor(ResourceCache.Acquire("Global VFX/vfx_healing_sparkles_001") as GameObject, Vector3.zero, true, false, false);
                    AkSoundEngine.PostEvent("Play_OBJ_heart_heal_01", base.gameObject);
                    if (Owner.HasPickupID(314))
                    {
                        LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(120).gameObject, Owner);
                    }
                }
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
    }
}