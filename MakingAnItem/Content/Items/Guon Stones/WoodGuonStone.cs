using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Reflection;
using MonoMod.RuntimeDetour;
using Dungeonator;
using Alexandria.Misc;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    class WoodGuonStone : AdvancedPlayerOrbitalItem
    {
        public static PlayerOrbital orbitalPrefab;
        public static void Init()
        {
            AdvancedPlayerOrbitalItem item = ItemSetup.NewItem<WoodGuonStone>(
            "Wood Guon Stone",
            "Fleeting Protection",
            "Provides brief protection, but destroys itself after a short time.",
            "woodguon_icon") as AdvancedPlayerOrbitalItem;
      
            item.quality = PickupObject.ItemQuality.EXCLUDED;
            item.SetTag("guon_stone");

            item.OrbitalPrefab = ItemSetup.CreateOrbitalObject("Wood Guon Stone", "woodguon_ingame", new IntVector2(9, 9), new IntVector2(-4, -5)).GetComponent<PlayerOrbital>();
            item.CanBeDropped = false;
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            StartCoroutine(HandleDeathTimer());        
        }
        public bool shouldBeKilledNextOpportunity;
        public override void Update()
        {
            if (!Dungeon.IsGenerating && Owner && shouldBeKilledNextOpportunity) InstaKillGuon();
            base.Update();
        }
        private IEnumerator HandleDeathTimer()
        {
            float seconds = 20f;
            if (Owner.PlayerHasActiveSynergy("Mahoguny Guon Stones")) seconds *= 2;
            yield return new WaitForSeconds(seconds);
            shouldBeKilledNextOpportunity = true;
            InstaKillGuon();
            yield break;
        }
        public void InstaKillGuon()
        {
            UnityEngine.Object.Destroy(this.m_extantOrbital);
            Owner.RemoveItemFromInventory(this);
        }
    }
}
