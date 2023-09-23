using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.Misc;

namespace NevernamedsItems
{
    public class GooeyHeart : PassiveItem
    {
        public static int GooeyHeartID;
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<GooeyHeart>(
            "Gooey Heart",
            "Squelchy",
            "Chance to heal upon killing a blob." + "\n\nThe heart of the Mighty Blobulord, gained through showing enough skill to leave it intact throughout the entire fight." + "\n\nWatch as it jiggles",
            "gooeyheart_icon");           
            item.quality = PickupObject.ItemQuality.C;
            item.RemovePickupFromLootTables();
            GooeyHeartID = item.PickupObjectId;
        }
        public override void DisableEffect(PlayerController player)
        {
            player.OnAnyEnemyReceivedDamage -= this.HandleHeal;
            base.DisableEffect(player);
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnAnyEnemyReceivedDamage += this.HandleHeal;
        }
        private void HandleHeal(float damage, bool fatal, HealthHaver enemy)
        {
            if (enemy && enemy.aiActor && enemy.aiActor.HasTag("blobulon"))
            {
                if (Owner.ForceZeroHealthState && UnityEngine.Random.value <= 0.025f)
                {
                    Owner.PlayEffectOnActor(ResourceCache.Acquire("Global VFX/vfx_healing_sparkles_001") as GameObject, Vector3.zero, true, false, false);
                    Owner.healthHaver.Armor = Owner.healthHaver.Armor + 1;
                }
                else if (UnityEngine.Random.value < 0.05f)
                {
                    Owner.healthHaver.ApplyHealing(0.5f);
                    Owner.PlayEffectOnActor(ResourceCache.Acquire("Global VFX/vfx_healing_sparkles_001") as GameObject, Vector3.zero, true, false, false);
                }
            }        
        }
    }
}
