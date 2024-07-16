using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    public class Accelerant : PassiveItem
    {
        public static void Init()
        {
            PassiveItem item = ItemSetup.NewItem<Accelerant>(
              "Accelerant",
              "Burning Sensation",
              "Triples the damage dealt to enemies by fire." + "\n\nWatching their skin melt off is actually rather horrifying. Avert your eyes.",
              "accelerant_icon") as PassiveItem;          
            item.quality = PickupObject.ItemQuality.D;
            item.AddToSubShop(ItemBuilder.ShopType.Goopton);
        }
        private DamageTypeModifier m_fireMultiplier;
        //private DamageTypeModifier m_poisonMultiplier;
        public void AIActorMods(AIActor target)
        {
            this.m_fireMultiplier = new DamageTypeModifier();
            this.m_fireMultiplier.damageMultiplier = 3f;
            this.m_fireMultiplier.damageType = CoreDamageTypes.Fire;
            target.healthHaver.damageTypeModifiers.Add(this.m_fireMultiplier);
            /*this.m_poisonMultiplier = new DamageTypeModifier();
            this.m_poisonMultiplier.damageMultiplier = 300f;
            this.m_poisonMultiplier.damageType = CoreDamageTypes.;
            target.healthHaver.damageTypeModifiers.Add(this.m_poisonMultiplier);*/
        }
        private void OnFires(Projectile bullet, float chanceshit)
        {
            if (fireyGuns.Contains(Owner.CurrentGun.PickupObjectId))
            {
                bullet.baseData.damage *= 2;
            }
        }
        public static List<int> fireyGuns = new List<int>()
        {
            382, //Phoenix
            125, //Flame Hand
            336, //Pitchfork
        };
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += this.OnFires;
            ETGMod.AIActor.OnPreStart += AIActorMods;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.PostProcessProjectile -= this.OnFires;
            ETGMod.AIActor.OnPreStart -= AIActorMods;
            return debrisObject;
        }
        public override void OnDestroy()
        {
            ETGMod.AIActor.OnPreStart -= AIActorMods;
            base.OnDestroy();
        }
    }
}
