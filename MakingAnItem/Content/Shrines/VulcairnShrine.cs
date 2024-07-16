
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
using Alexandria.Misc;

namespace NevernamedsItems
{
    public class VulcairnShrine : GenericShrine
    {
        public static GameObject Setup(GameObject pedestal)
        {
            var shrineobj = ItemBuilder.SpriteFromBundle("shrine_vulcairn", Initialisation.NPCCollection.GetSpriteIdByName("shrine_vulcairn"), Initialisation.NPCCollection, new GameObject("Shrine Vulcairn Statue"));
            shrineobj.GetComponent<tk2dSprite>().HeightOffGround = 1.25f;
            shrineobj.GetComponent<tk2dSprite>().renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTiltedCutout");
            shrineobj.GetComponent<tk2dSprite>().usesOverrideMaterial = true;
            pedestal.AddComponent<VulcairnShrine>();
            GameObject talkpoint = new GameObject("talkpoint");
            talkpoint.transform.SetParent(pedestal.transform);
            talkpoint.transform.localPosition = new Vector3(1f, 36f / 16f, 0f);
            return shrineobj;
        }
        public List<int> ids = new List<int>();
        public override void OnPlacement()
        {
            ids = AlexandriaTags.GetAllItemsIdsWithTag("guon_stone");
            ids.Remove(565);
            base.OnPlacement();
        }
        public override bool CanAccept(PlayerController interactor)
        {
            if (timesAccepted > 0) { return false; }
            if (interactor.characterIdentity == OMITBChars.Shade && interactor.carriedConsumables.Currency >= 40) return true;
           else if (interactor.ForceZeroHealthState && interactor.healthHaver.Armor > 2) { return true; }
            else if (interactor.healthHaver.GetMaxHealth() > 1) { return true; }
            return false;
        }
        public override void OnAccept(PlayerController Interactor)
        {
            if (Interactor.ForceZeroHealthState)
            {
                if (Interactor.characterIdentity == OMITBChars.Shade) { Interactor.carriedConsumables.Currency -= 40; }
                else { Interactor.healthHaver.Armor -= 2; }
            }
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

            List<int> instanceids = new List<int>(ids);
            instanceids.RemoveInvalidIDListEntries();

            GameUIRoot.Instance.notificationController.DoCustomNotification(
                   "Rock and Stone!",
                   "Bounty of the earth",
                   Initialisation.NPCCollection,
                   Initialisation.NPCCollection.GetSpriteIdByName("vulcairn_icon"),
                   UINotificationController.NotificationColor.SILVER,
                   true,
                   false
                   );

            PickupObject byId = PickupObjectDatabase.GetById(BraveUtility.RandomElement(instanceids));
            Interactor.AcquirePassiveItemPrefabDirectly(byId as PassiveItem);

            DeregisterMapIcon();
     
            AkSoundEngine.PostEvent("Play_OBJ_shrine_accept_01", base.gameObject);
        }
        public override string AcceptText(PlayerController interactor)
        {
            if (interactor.characterIdentity == OMITBChars.Shade) { return "Rock and Stone! <Lose 40[sprite \"ui_coin\"]>"; }
            if (interactor.ForceZeroHealthState) { return $"Rock and Stone! <Lose 2 [sprite \"armor_money_icon_001\"]>"; }
            return $"Rock and Stone! <Lose 1 [sprite \"heart_big_idle_001\"] Container>";
        }
        public override string DeclineText(PlayerController Interactor)
        {
            return "Leave";
        }
        public override string PanelText(PlayerController Interactor)
        {
            return timesAccepted == 0 ? "A shrine to the stoic rock god Vulcairn. His most devout followers are said to recieve gifts of precious stone..." : "The spirits inhabiting this shrine have departed...";
        }
    }
}

