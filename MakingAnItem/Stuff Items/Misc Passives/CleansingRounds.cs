using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using ItemAPI;

namespace NevernamedsItems
{
    public class CleansingRounds : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Cleansing Rounds";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/cleansingrounds_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<CleansingRounds>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Undo What Is Done";
            string longDesc = "These holy shells are capable of saving the Gundead from eternal Jamnation.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item

            List<string> mandatorySynergyItems = new List<string>() { "nn:cleansing_rounds", "silver_bullets" };
            CustomSynergies.Add("Holy Smackerel", mandatorySynergyItems);

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.B;


        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += this.PostProcessProjectile;
            player.PostProcessBeam += this.PostProcessBeam;
        }
        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {
            try
            {
                //ETGModConsole.Log("posting the process to the projectile in the mail");
                //ETGModConsole.Log($"Proj Null: {projectile == null} | OnHitEnemy null: {projectile?.OnHitEnemy == null}");
                sourceProjectile.OnHitEnemy += this.OnHitEnemy;
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
            }
        }
        private void PostProcessBeam(BeamController sourceBeam)
        {
            try
            {
                sourceBeam.projectile.OnHitEnemy += this.OnHitEnemy;
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
            }
        }

        private void OnHitEnemy(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
        {
            if (arg2 != null && arg2.aiActor != null && Owner != null)
            {
                if (!arg2.aiActor.IsBlackPhantom) return;
                else if (!arg2.healthHaver.IsDead)
                {
                    float procChance = UnityEngine.Random.value;
                    if (Owner.HasPickupID(538))
                    {
                        if (procChance > 0.5)
                        {
                            arg2.aiActor.UnbecomeBlackPhantom();
                        }
                    }
                    else
                    {
                        if (procChance < 0.1)
                        {
                            arg2.aiActor.UnbecomeBlackPhantom();
                        }
                    }
                }
            }
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.PostProcessProjectile -= this.PostProcessProjectile;
            player.PostProcessBeam -= this.PostProcessBeam;
            return debrisObject;
        }
        public override void OnDestroy()
        {
            if (Owner)
            {
                Owner.PostProcessProjectile -= this.PostProcessProjectile;
                Owner.PostProcessBeam -= this.PostProcessBeam;
            }
            base.OnDestroy();
        }

    }
}
