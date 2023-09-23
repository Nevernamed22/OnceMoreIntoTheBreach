using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using ItemAPI;

namespace NevernamedsItems
{
    class BlankKey : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<BlankKey>(
            "Blank Key",
            "Implosive Openings",
            "Spending a key triggers a blank effect." + "\n\nFlynt and Old Red don't often see eye to eye, but there are some... exceptions.",
            "blankkey_icon");
            item.quality = PickupObject.ItemQuality.C;
        }
        private float currentKeys, lastKeys;
        public override void Update()
        {
            if (Owner)
            {
                CalculateKeys(Owner);
            }

            else { return; }
        }

        private void CalculateKeys(PlayerController player)
        {
            currentKeys = player.carriedConsumables.KeyBullets;
            if (currentKeys < lastKeys)
            {
                if (Owner.HasPickupID(Gungeon.Game.Items["nn:spare_blank"].PickupObjectId))
                {
                    LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(224).gameObject, player);
                }
                else
                {
                    player.ForceBlank(25f, 0.5f, false, true, null, true, -1f);
                }
            }
            lastKeys = currentKeys;

        }
        public override void Pickup(PlayerController player)
        {
            //player.SetIsFlying(true, "shade", true, false);
            bool hasntAlreadyBeenCollected = !this.m_pickedUpThisRun;
            if (hasntAlreadyBeenCollected)
            {
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(67).gameObject, player);
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(224).gameObject, player);
            }
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            //player.SetIsFlying(false, "shade", true, false);
            DebrisObject debrisObject = base.Drop(player);
            return debrisObject;
        }
        public override void OnDestroy()
        {
            //Owner.SetIsFlying(false, "shade", true, false);
            base.OnDestroy();
        }
    }
}