﻿using System;
using System.Collections.Generic;
using Gungeon;
using Alexandria.ItemAPI;
using UnityEngine;

namespace NevernamedsItems
{
    public class BabyGoodChanceKin : PassiveItem
    {
        public static void Init()
        {
            CompanionItem item = ItemSetup.NewItem<CompanionItem>(
            "Baby Good Chance Kin",
            "Confused Friend",
            "Can spawn supplies whenever the person they follow suffers damage." + "\n\nThis cute little lad is unable to look directly in front of himself, but in spite of this predicament he is eager to help you on your journey.",
            "babygoodchancekin_icon") as CompanionItem;
            item.quality = PickupObject.ItemQuality.C;
            item.CompanionGuid = BabyGoodChanceKin.guid;
            BabyGoodChanceKin.BuildPrefab();
        }

        // Token: 0x0600009E RID: 158 RVA: 0x00006660 File Offset: 0x00004860
        public static void BuildPrefab()
        {
            bool flag = BabyGoodChanceKin.prefab != null || CompanionBuilder.companionDictionary.ContainsKey(BabyGoodChanceKin.guid);
            if (!flag)
            {
                BabyGoodChanceKin.prefab = CompanionBuilder.BuildPrefab("Baby Good Chance Kin", BabyGoodChanceKin.guid, "NevernamedsItems/Resources/BabyGoodChanceKinSprites/babygoodchancekin_idleleft_001", new IntVector2(8, 0), new IntVector2(6, 11));
                var companionController = BabyGoodChanceKin.prefab.AddComponent<ChanceKinBehavior>();
                companionController.aiActor.MovementSpeed = 5f;
                BabyGoodChanceKin.prefab.AddAnimation("idle_right", "NevernamedsItems/Resources/BabyGoodChanceKinSprites/babygoodchancekin_idleright", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.TwoWayHorizontal, DirectionalAnimation.FlipType.None);
                BabyGoodChanceKin.prefab.AddAnimation("idle_left", "NevernamedsItems/Resources/BabyGoodChanceKinSprites/babygoodchancekin_idleleft", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.TwoWayHorizontal, DirectionalAnimation.FlipType.None);
                BabyGoodChanceKin.prefab.AddAnimation("run_right", "NevernamedsItems/Resources/BabyGoodChanceKinSprites/babygoodchancekin_moveright", 10, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.TwoWayHorizontal, DirectionalAnimation.FlipType.None);
                BabyGoodChanceKin.prefab.AddAnimation("run_left", "NevernamedsItems/Resources/BabyGoodChanceKinSprites/babygoodchancekin_moveleft", 10, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.TwoWayHorizontal, DirectionalAnimation.FlipType.None);
                BehaviorSpeculator component = BabyGoodChanceKin.prefab.GetComponent<BehaviorSpeculator>();
                component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior
                {
                    IdleAnimations = new string[]
                    {
                        "idle"
                    },
                    CatchUpRadius = 6,
                    CatchUpMaxSpeed = 10,
                    CatchUpAccelTime = 1,
                    CatchUpSpeed = 7,
                });
            }
        }       
        public class ChanceKinBehavior : CompanionController
        {
            private void Start()
            {
                this.Owner = this.m_owner;
                Owner.OnReceivedDamage += OnDamaged;
            }
            public override void OnDestroy()
            {
                if (this.Owner)
                {
                    Owner.OnReceivedDamage -= OnDamaged;
                }
                base.OnDestroy();
            }
            private void OnDamaged(PlayerController player)
            {        
                    float procChance;
                    if (Owner.PlayerHasActiveSynergy("Good Lads")) procChance = 0.6f;
                    else procChance = 0.4f;
                    if (UnityEngine.Random.value < procChance)
                    {
                        int amountOfStuffToSpawn = 1;
                        if (Owner.PlayerHasActiveSynergy("Worship")) amountOfStuffToSpawn += 1;
                        for (int i = 0; i < amountOfStuffToSpawn; i++)
                        {
                            int lootID = BraveUtility.RandomElement(lootIDlist);
                            LootEngine.SpawnItem(PickupObjectDatabase.GetById(lootID).gameObject, base.aiActor.sprite.WorldCenter, Vector2.zero, 1f, false, true, false);
                        }
                    }
                
            }


            public PlayerController Owner;
        }
        public static List<int> lootIDlist = new List<int>()
        {
            78, //Ammo
            600, //Spread Ammo
            565, //Glass Guon Stone
            73, //Half Heart
            85, //Heart
            120, //Armor
            224, //Blank
            67, //Key
        };        
        public static GameObject prefab;
        private static readonly string guid = "baby_good_chance_kin180492309438";
    }
}