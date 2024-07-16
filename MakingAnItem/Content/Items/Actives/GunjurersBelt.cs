using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

using System.Reflection;
using UnityEngine;
using Alexandria.ItemAPI;
using Dungeonator;

namespace NevernamedsItems
{
    class GunjurersBelt : TargetedAttackPlayerItem
    {
        public static void Init()
        {
            TargetedAttackPlayerItem item = ItemSetup.NewItem<GunjurersBelt>(
               "Gunjurers Belt",
               "Poof!",
               "Knitted by an Apprentice Gunjurer as part of his ammomantic exams, it allows the bearer to slip beyond the curtain.",
               "gunjurersbelt_icon") as TargetedAttackPlayerItem;           
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 5);
            item.consumable = false;
            item.quality = ItemQuality.D;
            item.doesStrike = false;
            item.doesGoop = false;
            item.DoScreenFlash = false;
            item.reticleQuad = (PickupObjectDatabase.GetById(443) as TargetedAttackPlayerItem).reticleQuad;
        }
        public override void DoActiveEffect(PlayerController user)
        {
            tk2dBaseSprite cursor = this.m_extantReticleQuad;
            Vector2 overridePos = cursor.WorldCenter;
            TeleportPlayerToCursorPosition.StartTeleport(user, overridePos);            
            base.DoActiveEffect(user);
        }
        public override bool CanBeUsed(PlayerController user)
        {
            return base.CanBeUsed(user);
        }             
    }
}
