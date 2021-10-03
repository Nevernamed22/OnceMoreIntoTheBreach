using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using static MonoMod.Cil.RuntimeILReferenceBag.FastDelegateInvokers;
using UnityEngine;
using ItemAPI;
using MonoMod.RuntimeDetour;
using System.Reflection;

namespace NevernamedsItems
{
    class ChemGrenade : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Chem Grenade";
            string resourceName = "NevernamedsItems/Resources/chemgrenade_icon";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<ChemGrenade>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Toxic Explosions";
            string longDesc = "Explosions leave pools of poison. Gives poison immunity. " + "\n\nThis probably breaks the Guneva Conventions, but this is the Gungeon, who's gonna stop you?";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");
            item.quality = PickupObject.ItemQuality.C;
            item.AddToSubShop(ItemBuilder.ShopType.Trorc);
            item.AddToSubShop(ItemBuilder.ShopType.Goopton);

            ChemGrenadeID = item.PickupObjectId;
        }
        public static int ChemGrenadeID;
        private DamageTypeModifier m_poisonImmunity;
        public override void Pickup(PlayerController player)
        {
            if (m_poisonImmunity == null)
            {
                this.m_poisonImmunity = new DamageTypeModifier();
                this.m_poisonImmunity.damageMultiplier = 0f;
                this.m_poisonImmunity.damageType = CoreDamageTypes.Poison;
            }
            player.healthHaver.damageTypeModifiers.Add(this.m_poisonImmunity);
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject result = base.Drop(player);
            player.healthHaver.damageTypeModifiers.Remove(this.m_poisonImmunity);
            return result;
        }
        protected override void OnDestroy()
        {
            if (Owner != null)
            {
                Owner.healthHaver.damageTypeModifiers.Remove(this.m_poisonImmunity);
            }
            base.OnDestroy();
        }
    }
}
