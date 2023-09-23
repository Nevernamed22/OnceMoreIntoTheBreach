using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Alexandria.ItemAPI;
using UnityEngine;

namespace NevernamedsItems
{
    public class NNBlankPersonality : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<NNBlankPersonality>(
            "Blank Stare",
            "Unreadable",
            "Mysterious, boring, and a bit creepy, your newfound personality will sometimes make shopkeepers give you blanks just to make you go away and stop staring.",
            "blankpersonality_icon");
            item.AddPassiveStatModifier(PlayerStats.StatType.Coolness, 1, StatModifier.ModifyMethod.ADDITIVE);
            item.quality = PickupObject.ItemQuality.C;
            item.AddToSubShop(ItemBuilder.ShopType.OldRed);
        }

        private void OnItemPurchased(PlayerController player, ShopItemController obj)
        {
            if (Owner.HasPickupID(Gungeon.Game.Items["nn:spare_blank"].PickupObjectId))
            {
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(224).gameObject, player);
            }
           else if (UnityEngine.Random.value < .50f)
            {
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(224).gameObject, player);
            }

            if (Owner.HasPickupID(Gungeon.Game.Items["nn:spare_key"].PickupObjectId) && UnityEngine.Random.value <= 0.1f)
            {
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(67).gameObject, player);
            }
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnItemPurchased += this.OnItemPurchased;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject result = base.Drop(player);
            player.OnItemPurchased -= this.OnItemPurchased;
            return result;
        }
        public override void OnDestroy()
        {
            if (Owner) Owner.OnItemPurchased -= this.OnItemPurchased;
            base.OnDestroy();
        }
    }
}
