using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.Misc;
using SaveAPI;
using Dungeonator;

namespace NevernamedsItems
{
    class DeliveryBox : PlayerItem, ILabelItem
    {
        public static void Init()
        {
            PlayerItem item = ItemSetup.NewItem<DeliveryBox>(
            "Delivery Box",
            "Ready To Order",
            "Allows for the high-speed delivery of goods purchased straight from the manufacturer!" + "\n\nCut out the middle man!",
            "deliverybox_closed") as PlayerItem;
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 0.2f);

            DeliveryBox.spriteIDs = new int[9];

            DeliveryBox.spriteIDs[0] = item.sprite.spriteId; //empty
            DeliveryBox.spriteIDs[1] = Initialisation.itemCollection.GetSpriteIdByName("deliverybox_ammo"); //Ammo
            DeliveryBox.spriteIDs[2] = Initialisation.itemCollection.GetSpriteIdByName("deliverybox_spreadammo"); //SpreadAmmo
            DeliveryBox.spriteIDs[3] = Initialisation.itemCollection.GetSpriteIdByName("deliverybox_armour"); //Armour
            DeliveryBox.spriteIDs[4] = Initialisation.itemCollection.GetSpriteIdByName("deliverybox_halfheart"); //Half Heart
            DeliveryBox.spriteIDs[5] = Initialisation.itemCollection.GetSpriteIdByName("deliverybox_heart"); //full heart
            DeliveryBox.spriteIDs[6] = Initialisation.itemCollection.GetSpriteIdByName("deliverybox_key"); //Key
            DeliveryBox.spriteIDs[7] = Initialisation.itemCollection.GetSpriteIdByName("deliverybox_glassguon"); //Glass Guon
            DeliveryBox.spriteIDs[8] = Initialisation.itemCollection.GetSpriteIdByName("deliverybox_blank"); //blank

            item.consumable = false;
            item.quality = ItemQuality.B;
        }
        private Dictionary<ConsumableType, int> spriteDefDictionary = new Dictionary<ConsumableType, int>()
        {
            {ConsumableType.BLANK,  8},
            {ConsumableType.AMMO,  1},
            {ConsumableType.SPREAD_AMMO,  2},
            {ConsumableType.ARMOR,  3},
            {ConsumableType.HALF_HEART,  4},
            {ConsumableType.HEART,  5},
            {ConsumableType.KEY,  6},
            {ConsumableType.GLASS_GUON_STONE,  7},
        };
        private Dictionary<ConsumableType, int> consumableIDDictionary = new Dictionary<ConsumableType, int>()
        {
            {ConsumableType.BLANK,  224},
            {ConsumableType.AMMO,  78},
            {ConsumableType.SPREAD_AMMO,  600},
            {ConsumableType.ARMOR,  120},
            {ConsumableType.HALF_HEART,  73},
            {ConsumableType.HEART,  85},
            {ConsumableType.KEY,  67},
            {ConsumableType.GLASS_GUON_STONE,  565},
        };
        public string GetLabel()
        {
            return currentLabel;
        }
        public override void Pickup(PlayerController player)
        {
            currentLabel = "-1";
            if (!this.m_pickedUpThisRun)
            {
                SetConsumableTo(RandomConsumable());
            }
            else
            {
                SetConsumableTo(CurrentConsumable);
            }
            base.Pickup(player);
        }
        public override void OnPreDrop(PlayerController user)
        {
            base.sprite.SetSprite(DeliveryBox.spriteIDs[0]);
            base.OnPreDrop(user);
        }
        public override void DoEffect(PlayerController user)
        {
            user.carriedConsumables.Currency -= CalculatePrice(CurrentConsumable);
            SpawnCrate(consumableIDDictionary[CurrentConsumable]);
            SetConsumableTo(RandomConsumable());
        }
        private string currentLabel;
        public override void Update()
        {
            if (!string.IsNullOrEmpty(this.currentLabel) && (int.Parse(this.currentLabel) != CalculatePrice(CurrentConsumable)))
            {
                this.currentLabel = CalculatePrice(CurrentConsumable).ToString();
            }
            base.Update();
        }
        private int CalculatePrice(ConsumableType consumable)
        {
            if (consumableIDDictionary.ContainsKey(consumable))
            {
                PickupObject databaseObject = PickupObjectDatabase.GetById(consumableIDDictionary[consumable]);
                if (databaseObject != null)
                {
                    float price = databaseObject.PurchasePrice;
                    if (LastOwner)
                    {
                        price = price * LastOwner.stats.GetStatValue(PlayerStats.StatType.GlobalPriceMultiplier);
                        return Mathf.RoundToInt(price);
                    }
                    else
                    {
                        return Mathf.RoundToInt(price);
                    }
                }
                else return 420;
            }
            else return 69;
        }
        private void SpawnCrate(int item)
        {
            IntVector2 bestRewardLocation = LastOwner.CurrentRoom.GetBestRewardLocation(new IntVector2(1, 1), RoomHandler.RewardLocationStyle.CameraCenter, true);
            SupplyDropDoer.SpawnSupplyDrop(bestRewardLocation.ToVector2(), item);
        }
        private ConsumableType RandomConsumable()
        {
            return RandomEnum<ConsumableType>.Get();
        }
        private void SetConsumableTo(ConsumableType consumable)
        {
            if (spriteDefDictionary.ContainsKey(consumable))
            {
                base.sprite.SetSprite(DeliveryBox.spriteIDs[spriteDefDictionary[consumable]]);
            }
            else
            {
                base.sprite.SetSprite(DeliveryBox.spriteIDs[0]);
            }

            CurrentConsumable = consumable;
        }
        public ConsumableType CurrentConsumable;
        public enum ConsumableType
        {
            KEY,
            BLANK,
            AMMO,
            SPREAD_AMMO,
            ARMOR,
            HEART,
            HALF_HEART,
            GLASS_GUON_STONE,
        }

        public override bool CanBeUsed(PlayerController user)
        {
            if (user && user.carriedConsumables.Currency >= CalculatePrice(CurrentConsumable))
            {
                return true;
            }
            return false;
        }
        private static int[] spriteIDs;
    }
}

