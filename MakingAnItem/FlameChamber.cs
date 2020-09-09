using System;
using System.Collections.Generic;
using System.Linq;
using Gungeon;
using System.Text;

using UnityEngine;
using ItemAPI;

namespace NevernamedsItems
{
    public class FlameChamber : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Flame Chamber";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/flamechamber_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<FlameChamber>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Hotshot";
            string longDesc = "Reloading on an empty clip creates a pool of fire beneath the bearer. Use with caution."+"\n\nThis artefact seems strangely familiar to you, but you've never seen anything like it... yet.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.D;

            item.AddToSubShop(ItemBuilder.ShopType.Cursula);

            //LIST OF SYNERGIES
            // 'Burning With Passion' - Flamechamber + Charmed Bow, Charmed Horn, or Charming Rounds --> Item spawns charm creep instead.
            List<string> mandatorySynergyItems = new List<string>() { "nn:flame_chamber" };
            List<string> optionalSynergyItems = new List<string>() { "charmed_bow", "charm_horn", "charming_rounds" };
            CustomSynergies.Add("Burning With Passion", mandatorySynergyItems, optionalSynergyItems);          

            AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
            FlameChamber.goopDefs = new List<GoopDefinition>();
            foreach (string text in FlameChamber.goops)
            {
                GoopDefinition goopDefinition;
                try
                {
                    GameObject gameObject = assetBundle.LoadAsset(text) as GameObject;
                    goopDefinition = gameObject.GetComponent<GoopDefinition>();
                }
                catch
                {
                    goopDefinition = (assetBundle.LoadAsset(text) as GoopDefinition);
                }
                goopDefinition.name = text.Replace("assets/data/goops/", "").Replace(".asset", "");
                FlameChamber.goopDefs.Add(goopDefinition);
            }
            List<GoopDefinition> list = FlameChamber.goopDefs;
        }
        public GoopDefinition Goop;
        public float Radius = 3f;

        private void GiveFireImmunity()
        {
            if (fireImmune == true)
            {
                Owner.healthHaver.damageTypeModifiers.Remove(this.m_fireImmunity);
                fireImmune = false;
            }
            fireImmune = true;
            this.m_fireImmunity = new DamageTypeModifier();
            this.m_fireImmunity.damageMultiplier = 0f;
            this.m_fireImmunity.damageType = CoreDamageTypes.Fire;
            Owner.healthHaver.damageTypeModifiers.Add(this.m_fireImmunity);
            //ETGModConsole.Log("Fire immunity was given");
        }
        private void RemoveFireImmunity()
        {
            Owner.healthHaver.damageTypeModifiers.Remove(this.m_fireImmunity);
            fireImmune = false;
            //ETGModConsole.Log("Fire immunity was removed");
        }
        public bool fireImmune;
        private DamageTypeModifier m_fireImmunity;
        private void HandleGunReloaded(PlayerController player, Gun playerGun)
        {
            if (playerGun.ClipShotsRemaining == 0)
            {
                //ETGModConsole.Log("Empty Clip Reload detected");
                var charmGoop = PickupObjectDatabase.GetById(310)?.GetComponent<WingsItem>()?.RollGoop;
                float duration = 0.75f;
                GiveFireImmunity();
                if (Owner.HasPickupID(527) || Owner.HasPickupID(200) || Owner.HasPickupID(206))
                {
                    DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(charmGoop).TimedAddGoopCircle(Owner.specRigidbody.UnitCenter, 3.75f, duration, false);
                }
                else
                {
                    DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(FlameChamber.goopDefs[0]).TimedAddGoopCircle(Owner.specRigidbody.UnitCenter, 3.75f, duration, false);
                }
                //ETGModConsole.Log("Handled spawning goop");
                //player.healthHaver.damageTypeModifiers.Add();
                Invoke("RemoveFireImmunity", 2.0f);
            }
        }

        public void ForceGoop(PlayerController player)
        {
            DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(this.Goop).TimedAddGoopCircle(player.CenterPosition, this.Radius, 0.5f, false);
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnReloadedGun += this.HandleGunReloaded;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            player.OnReloadedGun -= this.HandleGunReloaded;
            if (fireImmune == true) RemoveFireImmunity();
            DebrisObject debrisObject = base.Drop(player);
            return debrisObject;
        }
        protected override void OnDestroy()
        {
            Owner.OnReloadedGun -= this.HandleGunReloaded;
            if (fireImmune == true) RemoveFireImmunity();
            base.OnDestroy();
        }

        private static List<GoopDefinition> goopDefs;

        private static string[] goops = new string[]
        {
            "assets/data/goops/napalmgoopthatworks.asset"
        };
    }

}