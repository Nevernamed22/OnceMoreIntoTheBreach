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
            //The name of the item
            string itemName = "Chem Grenade";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/chemgrenade_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<ChemGrenade>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Toxic Explosions";
            string longDesc = "Explosions leave pools of poison. Gives poison immunity. " + "\n\nThis probably breaks the Guneva Conventions, but this is the Gungeon, who's gonna stop you?";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.C;
            item.AddToSubShop(ItemBuilder.ShopType.Trorc);
            item.AddToSubShop(ItemBuilder.ShopType.Goopton);

            ChemGrenadeID = item.PickupObjectId;
        }
        public static int ChemGrenadeID;

        private static Hook BombHook = new Hook(
    typeof(Exploder).GetMethod("Explode", BindingFlags.Static | BindingFlags.Public),
    typeof(ChemGrenade).GetMethod("ExplosionHook", BindingFlags.Instance | BindingFlags.Public),
    typeof(Exploder)
);
        public void ExplosionHook(Action<Vector3, ExplosionData, Vector2, Action, bool, CoreDamageTypes, bool> orig, Vector3 position, ExplosionData data, Vector2 sourceNormal, Action onExplosionBegin = null, bool ignoreQueues = false, CoreDamageTypes damageTypes = CoreDamageTypes.None, bool ignoreDamageCaps = false)
        {
            orig(position, data, sourceNormal, onExplosionBegin, ignoreQueues, damageTypes, ignoreDamageCaps);
            try
            {
                if (GameManager.Instance.PrimaryPlayer.HasPickupID(ChemGrenadeID))
                {
                    float radius = 5;
                    if (GameManager.Instance.PrimaryPlayer.PlayerHasActiveSynergy("Toxic Shock")) radius = 8;
                    DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(EasyGoopDefinitions.PoisonDef).TimedAddGoopCircle(position, radius, 1, false);

                }
                else if (GameManager.Instance.SecondaryPlayer != null && GameManager.Instance.SecondaryPlayer.HasPickupID(ChemGrenadeID))
                {
                    float radius = 5;
                    if (GameManager.Instance.SecondaryPlayer.PlayerHasActiveSynergy("Toxic Shock")) radius = 8;
                    DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(EasyGoopDefinitions.PoisonDef).TimedAddGoopCircle(position, radius, 1, false);
                }
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
                ETGModConsole.Log(e.StackTrace);
            }
        }
        private DamageTypeModifier m_poisonImmunity;

        public delegate void Action<T1, T2, T3, T4, T5, T6, T7>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);
        public override void Pickup(PlayerController player)
        {
            this.m_poisonImmunity = new DamageTypeModifier();
            this.m_poisonImmunity.damageMultiplier = 0f;
            this.m_poisonImmunity.damageType = CoreDamageTypes.Poison;
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
