using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using static MonoMod.Cil.RuntimeILReferenceBag.FastDelegateInvokers;
using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.Misc;
using MonoMod.RuntimeDetour;
using System.Reflection;

namespace NevernamedsItems
{
    class ChemGrenade : PassiveItem
    {
        public static void Init()
        {
            PassiveItem item = ItemSetup.NewItem<ChemGrenade>(
              "Chem Grenade",
              "Toxic Explosions",
              "Explosions leave pools of poison. Gives poison immunity. " + "\n\nThis probably breaks the Guneva Conventions, but this is the Gungeon, who's gonna stop you?",
              "chemgrenade_icon") as PassiveItem;        
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
            CustomActions.OnExplosionComplex += Explosion;
            base.Pickup(player);
        }
        public void Explosion(Vector3 position, ExplosionData data, Vector2 dir, Action onbegin, bool ignoreQueues, CoreDamageTypes damagetypes, bool ignoreDamageCaps)
        {
            float radius = 5;
            if (GameManager.Instance.AnyPlayerHasActiveSynergy("Toxic Shock")) radius = 8;
            DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(EasyGoopDefinitions.PoisonDef).TimedAddGoopCircle(position, radius, 1, false);
        }
        public override void DisableEffect(PlayerController player)
        {
            CustomActions.OnExplosionComplex -= Explosion;
            player.healthHaver.damageTypeModifiers.Remove(this.m_poisonImmunity);
            base.DisableEffect(player);
        }

    }
}
