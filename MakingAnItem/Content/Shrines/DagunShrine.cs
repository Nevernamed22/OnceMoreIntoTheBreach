
using GungeonAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Gungeon;
using Alexandria.ItemAPI;
using Dungeonator;
using System.Reflection;
using MonoMod.RuntimeDetour;


namespace NevernamedsItems
{
    public class DagunShrine : GenericShrine
    {
        public static GameObject Setup(GameObject pedestal)
        {
            var shrineobj = ItemBuilder.SpriteFromBundle("shrine_dagun", Initialisation.NPCCollection.GetSpriteIdByName("shrine_dagun"), Initialisation.NPCCollection, new GameObject("Shrine Dagun Statue"));
            shrineobj.GetComponent<tk2dSprite>().HeightOffGround = 1.25f;
            shrineobj.GetComponent<tk2dSprite>().renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTiltedCutout");
            shrineobj.GetComponent<tk2dSprite>().usesOverrideMaterial = true;
            pedestal.AddComponent<DagunShrine>();
            GameObject talkpoint = new GameObject("talkpoint");
            talkpoint.transform.SetParent(pedestal.transform);
            talkpoint.transform.localPosition = new Vector3(1f, 36f / 16f, 0f);
            return shrineobj;
        }
        public override void OnAccept(PlayerController Interactor)
        {
            if (Interactor.characterIdentity == OMITBChars.Shade) { Interactor.carriedConsumables.Currency -= 40; }
            else if (Interactor.ForceZeroHealthState) { Interactor.healthHaver.Armor -= 2; }
            else
            {
                StatModifier HP = new StatModifier
                {
                    statToBoost = PlayerStats.StatType.Health,
                    amount = -1f,
                    modifyType = StatModifier.ModifyMethod.ADDITIVE
                };
                Interactor.ownerlessStatModifiers.Add(HP);
                Interactor.stats.RecalculateStats(Interactor);
            }

            int amt = UnityEngine.Random.Range(5, 11);
            for (int i = 0; i < amt; i++)
            {
                IntVector2 pos = m_room.GetRandomVisibleClearSpot(2, 2);
                LootEngine.SpawnItem(PickupObjectDatabase.GetById(BraveUtility.RandomElement(BabyGoodChanceKin.lootIDlist)).gameObject, pos.ToVector2(), Vector2.zero, 0);
            }

            GameUIRoot.Instance.notificationController.DoCustomNotification(
                    "Bounty",
                    "Dungeon of Plenty",
                    Initialisation.NPCCollection,
                    Initialisation.NPCCollection.GetSpriteIdByName("dagun_popup"),
                    UINotificationController.NotificationColor.SILVER,
                    true,
                    false
                    );
            AkSoundEngine.PostEvent("Play_OBJ_shrine_accept_01", base.gameObject);
        }
        public override bool CanAccept(PlayerController interactor)
        {
            if (interactor.characterIdentity == OMITBChars.Shade) { return interactor.carriedConsumables.Currency >= 40; }
            else if (interactor.ForceZeroHealthState) { return interactor.healthHaver.Armor > 2; }
            else { return interactor.healthHaver.GetMaxHealth() > 1; }
        }
        public override string AcceptText(PlayerController interactor)
        {
            if (interactor.characterIdentity == OMITBChars.Shade) { return "Pray <Lose 40[sprite \"ui_coin\"]>"; }
            if (interactor.ForceZeroHealthState) { return $"Pray <Lose 2 [sprite \"armor_money_icon_001\"]>"; }
            return $"Pray <Lose 1 [sprite \"heart_big_idle_001\"] Container>";
        }
        public override string DeclineText(PlayerController Interactor)
        {
            return "Leave";
        }
        public override string PanelText(PlayerController Interactor)
        {
            return "A shrine to Dagun, god of plenty. His amphora of shells running never dry.";
        }
    }
}

