using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dungeonator;
using UnityEngine;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    class FowlRing : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<FowlRing>(
            "Fowl Ring",
            "Cluck Up",
            "One enemy per room becomes a chicken." + "\n\nA symbol of poultry affinity, manifesting one's most fowl desires.",
            "fowlring_icon");
            item.quality = PickupObject.ItemQuality.D;
            FowlRingID = item.PickupObjectId;
        }
        public static int FowlRingID;
        private void EnteredCombat()
        {
            if (Owner != null && Owner.CurrentRoom != null && Owner.CurrentRoom.HasActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear))
            {
                AIActor enemy = Owner.CurrentRoom.GetRandomActiveEnemy(false);
                enemy.Transmogrify(EnemyDatabase.GetOrLoadByGuid(GUIDs.Chicken), (GameObject)ResourceCache.Acquire("Global VFX/VFX_Item_Spawn_Poof"));
            }
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnEnteredCombat += this.EnteredCombat;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.OnEnteredCombat -= this.EnteredCombat;
            return debrisObject;
        }
        public override void OnDestroy()
        {
            if (Owner)
            {
                Owner.OnEnteredCombat -= this.EnteredCombat;
            }
            base.OnDestroy();
        }
    }
}