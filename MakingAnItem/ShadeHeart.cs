using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dungeonator;
using UnityEngine;
using ItemAPI;
using System.Collections;

namespace NevernamedsItems
{
    class ShadeHeart : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Shade Heart";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "NevernamedsItems/Resources/shadeheart_icon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<ShadeHeart>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Heart of Darkness";
            string longDesc = "The ventricles of this shadowy organ are paper-thin, and ripple with a strange otherworldly energy." + "\n\nThough fragile, it holds fantastic power.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            //Adds the actual passive effect to the item
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.AdditionalShotPiercing, 10, StatModifier.ModifyMethod.ADDITIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.EnemyProjectileSpeedMultiplier, 0.95f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.GlobalPriceMultiplier, 0.7f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.AdditionalBlanksPerFloor, 1, StatModifier.ModifyMethod.ADDITIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Coolness, 4, StatModifier.ModifyMethod.ADDITIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.AdditionalItemCapacity, 1, StatModifier.ModifyMethod.ADDITIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Damage, 1.10f, StatModifier.ModifyMethod.MULTIPLICATIVE);

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.EXCLUDED;
            item.CanBeDropped = false;
        }
        private float currentArmour, lastArmour;
        private string currentRoom, lastRoom;
        private bool currentCombatState, lastCombatState;
        protected override void Update()
        {
            if (Owner)
            {
                CalculateHealth(Owner);
                ShouldFlyCheck(Owner);
                if (Owner.OverridePlayerSwitchState != PlayableCharacters.Pilot.ToString())
                {
                    Owner.OverridePlayerSwitchState = PlayableCharacters.Pilot.ToString();
                }
            }

            else { return; }
        }
        private bool hasDoneFirstArmourResetThisRun = false;
        private void CalculateHealth(PlayerController player)
        {
            currentArmour = player.healthHaver.Armor;
            if (currentArmour != lastArmour)
            {
                if (player.healthHaver.Armor > 1f)
                {
                    if (hasDoneFirstArmourResetThisRun)
                    {
                        int amountOfSurplus = (int)player.healthHaver.Armor - 1;
                        float percentPerArmour = 0.025f;
                        if (player.HasPickupID(FullArmourJacket.FullArmourJacketID)) percentPerArmour = 0.05f;

                        StatModifier statModifier = new StatModifier();
                        statModifier.amount = (percentPerArmour * amountOfSurplus) + 1;
                        statModifier.modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE;
                        statModifier.statToBoost = PlayerStats.StatType.Damage;
                        Owner.ownerlessStatModifiers.Add(statModifier);
                        Owner.stats.RecalculateStats(Owner, false, false);

                        LootEngine.SpawnCurrency(player.sprite.WorldCenter, (15 * amountOfSurplus)); 
                    }
                    hasDoneFirstArmourResetThisRun = true;
                    player.healthHaver.Armor = 1f;
                }
                lastArmour = currentArmour;
            }
        }
        private void ShouldFlyCheck(PlayerController player)
        {
            currentRoom = Owner.CurrentRoom.GetRoomName();
            currentCombatState = Owner.IsInCombat;
            bool roomFlag = !string.IsNullOrEmpty(currentRoom) && currentRoom != lastRoom;
            bool combatCheck = currentCombatState != lastCombatState;
            if (roomFlag || combatCheck)
            {
                FlyCheck(player);
                lastRoom = currentRoom;
                lastCombatState = currentCombatState;
            }
        }

        private void FlyCheck(PlayerController player)
        {
            ResourcefulRatMinesHiddenTrapdoor TrapDoorInstance = null;

            if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.MINEGEON)
            {
                TrapDoorInstance = FindObjectOfType<ResourcefulRatMinesHiddenTrapdoor>();
            }
            if ((!string.IsNullOrEmpty(Owner.CurrentRoom.GetRoomName()) && m_BannedRoomNames.Contains(Owner.CurrentRoom.GetRoomName())) | Owner.CurrentRoom.ForcePitfallForFliers)
            {
                SetCanFall(true);
            }
            else if (TrapDoorInstance != null && TrapDoorInstance.transform.position.GetAbsoluteRoom() == Owner.CurrentRoom)
            {
                SetCanFall(true);
            }
            else if (!string.IsNullOrEmpty(Owner.CurrentRoom.GetRoomName()) && Owner.CurrentRoom.GetRoomName() == "HelicopterRoom01" && Owner.IsInCombat == false)
            {
                SetCanFall(true);
            }
            else
            {
                SetCanFall(false);
            }
        }
        private void SetCanFall(bool state)
        {
            if (state == false)
            {
                Owner.FallingProhibited = true;
                DisableVFX(Owner);
                lastRoom = currentRoom;
            }
            else if (state == true)
            {
                Owner.FallingProhibited = false;
                EnableVFX(Owner);
                lastRoom = currentRoom;
            }
        }
        private void EnableVFX(PlayerController user)
        {
            Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(user.sprite);
            outlineMaterial.SetColor("_OverrideColor", new Color(100f, 1f, 1f));
        }
        private void onAnyEnemyTakeAnyDamage(float damageamount, bool fatal, HealthHaver enemy)
        {

        }
        private void DisableVFX(PlayerController user)
        {
            Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(user.sprite);
            outlineMaterial.SetColor("_OverrideColor", new Color(0f, 0f, 0f));
        }
        public static List<string> m_BannedRoomNames = new List<string>()
        {
            "Castle_Special_SewersEntrance_01",
            "Keep_TreeRoom",
            "Keep_TreeRoom2",
            "Exit_Room_Basic",
            "Floor_02_Gungeon_Entrance",
            "Elevator Entrance",
            "SubShop_SellCreep_CatacombsSpecial",
            "ResourcefulRat_SecondSecretRoom_01"
        };
        private DamageTypeModifier m_poisonImmunity;
        private DamageTypeModifier m_fireImmunity;
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnAnyEnemyReceivedDamage += this.onAnyEnemyTakeAnyDamage;
            this.m_poisonImmunity = new DamageTypeModifier();
            this.m_poisonImmunity.damageMultiplier = 0f;
            this.m_poisonImmunity.damageType = CoreDamageTypes.Poison;
            player.healthHaver.damageTypeModifiers.Add(this.m_poisonImmunity);
            this.m_fireImmunity = new DamageTypeModifier();
            this.m_fireImmunity.damageMultiplier = 0f;
            this.m_fireImmunity.damageType = CoreDamageTypes.Fire;
            player.healthHaver.damageTypeModifiers.Add(this.m_fireImmunity);
            player.ImmuneToPits.SetOverride("ShadeHeart", true, null);
            //player.SetIsFlying(true, "shade", true, false);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            player.OnAnyEnemyReceivedDamage -= this.onAnyEnemyTakeAnyDamage;
            //player.SetIsFlying(false, "shade", true, false);
            DebrisObject debrisObject = base.Drop(player);
            player.healthHaver.damageTypeModifiers.Remove(this.m_poisonImmunity);
            player.healthHaver.damageTypeModifiers.Remove(this.m_fireImmunity);
            player.ImmuneToPits.SetOverride("ShadeHeart", false, null);
            return debrisObject;
        }
        protected override void OnDestroy()
        {
            //Owner.SetIsFlying(false, "shade", true, false);
            Owner.OnAnyEnemyReceivedDamage -= this.onAnyEnemyTakeAnyDamage;
            Owner.healthHaver.damageTypeModifiers.Remove(this.m_poisonImmunity);
            Owner.ImmuneToPits.SetOverride("ShadeHeart", false, null);
            base.OnDestroy();
        }
    }
}