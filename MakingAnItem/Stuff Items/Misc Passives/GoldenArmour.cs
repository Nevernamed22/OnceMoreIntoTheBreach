﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dungeonator;
using UnityEngine;
using ItemAPI;

namespace NevernamedsItems
{
    class GoldenArmour : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Golden Armour";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/goldenarmour_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<GoldenArmour>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "You Can't Take It With You";
            string longDesc = "If the bearer has money, and takes damage that would kill them, the cash has a chance to take the hit instead." + "\n\nGold is a soft metal, and doesn't make for very good protection. Whoever decided to make gold armour probably isn't very smart."+"\n\nActivation chance is equal to cash amount. More than 100 cash guarantees a safety net.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.D;

            //SYNERGY --> If you have the items, it only takes a random amount of cash. (Old Goldie, Gold Ammolet, Gold Junk, AU Gun, Gilded Hydra)
            //SYNERGY --> Gives armour when it procs (Silver Bullets)
        }
        private void ModifyIncomingDamage(HealthHaver source, HealthHaver.ModifyDamageEventArgs args)
        {
            PlayableCharacters characterIdentity = Owner.characterIdentity;

            if (characterIdentity != PlayableCharacters.Robot)
            {
                if (source.GetCurrentHealth() <= 0.5 && source.Armor <= 0)
                {
                    if (Owner.carriedConsumables.Currency > 0)
                    {
                        int procVariable = UnityEngine.Random.Range(1, 101);

                        if (Owner.carriedConsumables.Currency > 100)
                        {
                            if (Owner.HasPickupID(93) || Owner.HasPickupID(321) || Owner.HasPickupID(641) || Owner.HasPickupID(53) || Owner.HasPickupID(231)) Owner.carriedConsumables.Currency = UnityEngine.Random.Range(1, Owner.carriedConsumables.Currency);
                            else Owner.carriedConsumables.Currency = 0;
                            args.ModifiedDamage = 0f;
                            Owner.healthHaver.ApplyHealing(0.5f);
                            Owner.healthHaver.ApplyDamage(0.5f, Vector2.zero, "GoldenArmourBug(PlsReport)", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                            if (Owner.HasPickupID(538))
                            {
                                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(120).gameObject, Owner);
                                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(120).gameObject, Owner);
                            }
                        }
                        else
                        {
                            if (Owner.carriedConsumables.Currency >= procVariable)
                            {
                                //Synergy
                                if (Owner.HasPickupID(93) || Owner.HasPickupID(321) || Owner.HasPickupID(641) || Owner.HasPickupID(53) || Owner.HasPickupID(231)) Owner.carriedConsumables.Currency = procVariable;
                                else Owner.carriedConsumables.Currency = 0;
                                args.ModifiedDamage = 0f;
                                Owner.healthHaver.ApplyHealing(0.5f);
                                Owner.healthHaver.ApplyDamage(0.5f, Vector2.zero, "GoldenArmourBug(PlsReport)", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                                if (Owner.HasPickupID(538))
                                {
                                    LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(120).gameObject, Owner);
                                    LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(120).gameObject, Owner);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (source.Armor <= 1)
                {
                    if (Owner.carriedConsumables.Currency > 0)
                    {
                        int procVariable = UnityEngine.Random.Range(1, 101);
                        if (Owner.carriedConsumables.Currency > 100)
                        {
                            if (Owner.HasPickupID(93) || Owner.HasPickupID(321) || Owner.HasPickupID(641) || Owner.HasPickupID(53) || Owner.HasPickupID(231)) Owner.carriedConsumables.Currency = UnityEngine.Random.Range(1, Owner.carriedConsumables.Currency);
                            else Owner.carriedConsumables.Currency = 0;
                            args.ModifiedDamage = 0f;
                            Owner.healthHaver.Armor += 1;
                            Owner.healthHaver.ApplyDamage(0.5f, Vector2.zero, "GoldenArmourBug(PlsReport)", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                            if (Owner.HasPickupID(538))
                            {
                                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(120).gameObject, Owner);
                                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(120).gameObject, Owner);
                            }
                        }
                        else
                        {
                            if (Owner.carriedConsumables.Currency >= procVariable)
                            {
                                //Synergy with Old Goldie, Gold Ammolet, Gold Junk, AU Gun, Gilded Hydra
                                if (Owner.HasPickupID(93) || Owner.HasPickupID(321) || Owner.HasPickupID(641) || Owner.HasPickupID(53) || Owner.HasPickupID(231)) Owner.carriedConsumables.Currency = procVariable;
                                else Owner.carriedConsumables.Currency = 0;
                                args.ModifiedDamage = 0f;
                                Owner.healthHaver.Armor += 1;
                                Owner.healthHaver.ApplyDamage(0.5f, Vector2.zero, "GoldenArmourBug(PlsReport)", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                                if (Owner.HasPickupID(538))
                                {
                                    LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(120).gameObject, Owner);
                                    LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(120).gameObject, Owner);
                                }
                            }
                        }
                    }
                }
            }
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            HealthHaver healthHaver = player.healthHaver;
            healthHaver.ModifyDamage = (Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>)Delegate.Combine(healthHaver.ModifyDamage, new Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>(this.ModifyIncomingDamage));
            bool hasntAlreadyBeenCollected = !this.m_pickedUpThisRun;
            if (hasntAlreadyBeenCollected)
            {
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(120).gameObject, player);
            }
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            HealthHaver healthHaver = player.healthHaver;
            healthHaver.ModifyDamage = (Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>)Delegate.Remove(healthHaver.ModifyDamage, new Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>(this.ModifyIncomingDamage));
            return debrisObject;
        }
        protected override void OnDestroy()
        {
            if (Owner)
            {
                Owner.healthHaver.ModifyDamage -= ModifyIncomingDamage;
            }
            base.OnDestroy();
        }
    }
}
