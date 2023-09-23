using UnityEngine;
using Dungeonator;
using System.Collections;
using System;
using MonoMod.RuntimeDetour;
using System.Reflection;
using Gungeon;
using System.Collections.Generic;
using Alexandria.Misc;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    public class SilverAmmolet : BlankModificationItem
    {
        public static void Init()
        {
            BlankModificationItem item = ItemSetup.NewItem<SilverAmmolet>(
            "Silver Ammolet",
            "Blanks Cleanse",
            "A holy artefact from The Order of The True Gun's archives." + "\n\nMade of 200% Silver, and capable of bestowing a holy cleanse upon the Jammed.",
            "silverammolet_icon") as BlankModificationItem;
            item.quality = PickupObject.ItemQuality.C;

            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.AdditionalBlanksPerFloor, 1f, StatModifier.ModifyMethod.ADDITIVE);

            item.AddToSubShop(ItemBuilder.ShopType.OldRed);
            ID = item.PickupObjectId;
            item.SetTag("ammolet");
        }
        private static int ID;
        public override void Pickup(PlayerController player)
        {
            player.GetExtComp().OnBlankModificationItemProcessed += OnBlankModTriggered;
            base.Pickup(player);
        }
        public override void DisableEffect(PlayerController player)
        {
            player.GetExtComp().OnBlankModificationItemProcessed -= OnBlankModTriggered;
            base.DisableEffect(player);
        }

        private void OnBlankModTriggered(PlayerController user, SilencerInstance blank, Vector2 pos, BlankModificationItem item)
        {
            if (item is SilverAmmolet)
            {
                if (user.CurrentRoom != null && user.CurrentRoom.HasActiveEnemies(RoomHandler.ActiveEnemyType.All))
                {
                    foreach (AIActor aiactor in user.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All))
                    {
                        if (aiactor.IsBlackPhantom)
                        {
                            float procChance = 0.5f;
                            float dmg = 30;
                            if (user.PlayerHasActiveSynergy("Blessed are The Shriek")) { procChance = 0.7f; dmg = 42; }
                            if (UnityEngine.Random.value <= procChance)
                            {
                                aiactor.PlayEffectOnActor(PickupObjectDatabase.GetById(538).GetComponent<SilverBulletsPassiveItem>().SynergyPowerVFX, new Vector3(0f, -0.5f, 0f));
                                if (aiactor.healthHaver) aiactor.healthHaver.ApplyDamage(dmg, Vector2.zero, "Silver Ammolet", CoreDamageTypes.None, DamageCategory.Normal);
                                aiactor.UnbecomeBlackPhantom();
                            }
                        }
                    }
                }
            }
        }
    }
}