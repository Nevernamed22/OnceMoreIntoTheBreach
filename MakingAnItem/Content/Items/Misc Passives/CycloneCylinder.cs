using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    public class CycloneCylinder : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<CycloneCylinder>(
            "Cyclone Boots",
            "Gusty Dodges",
            "Dodge rolling releases a magical gust of wind that pushes all enemies around you back." + "\n\nThe magical wind is stored in the boots themselves, and it feels oddly tickly on your toes.",
            "cycloneboots_icon");
            item.quality = PickupObject.ItemQuality.D;

            //Synergy with the Balloon Gun --> Double Radius.
            //Synergy with Armour of Thorns --> Deal damage to all enemies pushed. dam = dodgerolldam * 3.
            List<string> mandatorySynergyItems = new List<string>() { "nn:cyclone_boots", "balloon_gun" };
            CustomSynergies.Add("Let Loose", mandatorySynergyItems);
            List<string> mandatorySynergyItems2 = new List<string>() { "nn:cyclone_boots", "armor_of_thorns" };
            CustomSynergies.Add("Scytheclone", mandatorySynergyItems2);
        }


        private void onDodgeRoll(PlayerController player, Vector2 dirVec)
        {
            int radius = 10;
            if (Owner.PlayerHasActiveSynergy("Let Loose")) radius = 20;

            Exploder.DoRadialKnockback(player.specRigidbody.UnitCenter, 100, radius);

            if (Owner.PlayerHasActiveSynergy("Scytheclone"))
            {
                float curDodgeDam = player.stats.GetStatValue(PlayerStats.StatType.DodgeRollDamage);
                Exploder.DoRadialDamage(curDodgeDam * 3, player.specRigidbody.UnitCenter, radius, false, true, false, null);
            }
        }

        public override void Pickup(PlayerController player)
        {
            player.OnRollStarted += this.onDodgeRoll;
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject result = base.Drop(player);
            player.OnRollStarted -= this.onDodgeRoll;
            return result;
        }
        public override void OnDestroy()
        {
            if (Owner) Owner.OnRollStarted -= this.onDodgeRoll;
            base.OnDestroy();
        }
    }
}
