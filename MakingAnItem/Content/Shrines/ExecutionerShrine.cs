

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
    public class ExecutionerShrine : GenericShrine
    {
        public static GameObject Setup(GameObject pedestal)
        {
            var shrineobj = ItemBuilder.SpriteFromBundle("shrine_execution", Initialisation.NPCCollection.GetSpriteIdByName("shrine_execution"), Initialisation.NPCCollection, new GameObject("Shrine Execution Statue"));
            shrineobj.GetComponent<tk2dSprite>().HeightOffGround = 1.25f;
            shrineobj.GetComponent<tk2dSprite>().renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTiltedCutout");
            shrineobj.GetComponent<tk2dSprite>().usesOverrideMaterial = true;
            pedestal.AddComponent<ExecutionerShrine>();
            GameObject talkpoint = new GameObject("talkpoint");
            talkpoint.transform.SetParent(pedestal.transform);
            talkpoint.transform.localPosition = new Vector3(1f, 36f / 16f, 0f);
            return shrineobj;
        }
        public override bool CanAccept(PlayerController interactor) { return !hasFailed; }
        public override void OnAccept(PlayerController Interactor)
        {
            float chance = 0.5f;
            if (Interactor.HasPickupID(LuckyCoin.LuckyCoinID)) { chance = 0.75f; }
            if (Interactor.HasPickupID(289)) { chance = 0.9f; }

            if (UnityEngine.Random.value <= chance)
            {
                if (Interactor.ForceZeroHealthState) { Interactor.healthHaver.Armor += 4; }
                else { Interactor.healthHaver.ApplyHealing(1000); }

                GameUIRoot.Instance.notificationController.DoCustomNotification(
                       "Salvation",
                        "Executioner's Wager",
                        Initialisation.NPCCollection,
                        Initialisation.NPCCollection.GetSpriteIdByName("executioner_icon"),
                        UINotificationController.NotificationColor.SILVER,
                        true,
                        false
                        );
                AkSoundEngine.PostEvent("Play_OBJ_shrine_accept_01", base.gameObject);
            }
            else
            {
                hasFailed = true;
                if (Interactor.ForceZeroHealthState) { Interactor.healthHaver.Armor = 1; }
                else { Interactor.healthHaver.ForceSetCurrentHealth(0.5f); }

                GameUIRoot.Instance.notificationController.DoCustomNotification(
                        "Damnation",
                        "Executioner's Wager",
                        Initialisation.NPCCollection,
                        Initialisation.NPCCollection.GetSpriteIdByName("executioner_icon"),
                        UINotificationController.NotificationColor.SILVER,
                        true,
                        false
                        );
                AkSoundEngine.PostEvent("Play_VO_lichA_cackle_01", base.gameObject);
            }
        }
        public override string AcceptText(PlayerController interactor)
        {
            return "Conjure The Spirit of Execution";
        }
        public override string DeclineText(PlayerController Interactor)
        {
            return "Leave";
        }
        public override string PanelText(PlayerController Interactor)
        {
            return hasFailed ? "No second chances..." : "A shrine to an over-zealous Executioner, fond of gambling and wagers. Those who conjure his spirit are either saved or damned depending on a roll of the dice.";
        }
        public bool hasFailed = false;
    }
}
