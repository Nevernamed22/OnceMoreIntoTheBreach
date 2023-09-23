using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Alexandria.ItemAPI;
using UnityEngine;

namespace NevernamedsItems
{
    class TheShellactery : PlayerItem
    {
        public static void Init()
        {
            PlayerItem item = ItemSetup.NewItem<TheShellactery>(
            "The Shellactery",
            "Firearm Immortality",
            "Generates ammunition." + "\n\nThis ancient relic allows you to reach right through the Curtain and pluck ammo directly from the great beyond." + "\n\nTorn from the gut of an ancient Gungeoneer who was ripped back from the jaws of death, despite his best attempts...",
            "theshellactery_improved") as PlayerItem;
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 1600);
            item.AddPassiveStatModifier( PlayerStats.StatType.Curse, 2f, StatModifier.ModifyMethod.ADDITIVE);
            item.consumable = false;
            item.AddToSubShop(ItemBuilder.ShopType.Cursula);
            item.quality = ItemQuality.S;

            //SYNERGY WITH BLACK HOLE GUN --> "Black Paradox"
            List<string> mandatorySynergyItems = new List<string>() { "nn:the_shellactery", "black_hole_gun" };
            CustomSynergies.Add("Black Paradox", mandatorySynergyItems);
        }

        //Add the item's functionality down here! I stole most of this from the Stuffed Star active item code!

        public override void DoEffect(PlayerController user)
        {
            AkSoundEngine.PostEvent("Play_OBJ_shrine_accept_01", base.gameObject);
            if (user.HasPickupID(169))
            {
                LootEngine.SpawnItem(PickupObjectDatabase.GetById(600).gameObject, LastOwner.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
                LootEngine.SpawnItem(PickupObjectDatabase.GetById(600).gameObject, LastOwner.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
            }
            else LootEngine.SpawnItem(PickupObjectDatabase.GetById(78).gameObject, LastOwner.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
        }
    }
}
