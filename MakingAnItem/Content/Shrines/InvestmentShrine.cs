

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
    public class InvestmentShrine : GenericShrine
    {
        public static GameObject Setup(GameObject pedestal)
        {
            var shrineobj = ItemBuilder.SpriteFromBundle("shrine_investment", Initialisation.NPCCollection.GetSpriteIdByName("shrine_investment"), Initialisation.NPCCollection, new GameObject("Shrine Investment Statue"));
            shrineobj.GetComponent<tk2dSprite>().HeightOffGround = 1.25f;
            shrineobj.GetComponent<tk2dSprite>().renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTiltedCutout");
            shrineobj.GetComponent<tk2dSprite>().usesOverrideMaterial = true;
            pedestal.AddComponent<InvestmentShrine>();
            GameObject talkpoint = new GameObject("talkpoint");
            talkpoint.transform.SetParent(pedestal.transform);
            talkpoint.transform.localPosition = new Vector3(1f, 36f / 16f, 0f);
            return shrineobj;
        }
        public int numUses = 0;
        public override bool CanAccept(PlayerController interactor) { return interactor.carriedConsumables.Currency >= (10 * (numUses + 1)); }
        public override void OnAccept(PlayerController Interactor)
        {
            Interactor.carriedConsumables.Currency -= (10 * (numUses + 1));

            StatModifier PriceReduction = new StatModifier { statToBoost = PlayerStats.StatType.GlobalPriceMultiplier, amount = .9f, modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE };
            Interactor.ownerlessStatModifiers.Add(PriceReduction);
            Interactor.stats.RecalculateStats(Interactor);
            GameUIRoot.Instance.notificationController.DoCustomNotification(
                   "Invested!",
                    "Shop Prices Reduced",
                    Initialisation.NPCCollection,
                    Initialisation.NPCCollection.GetSpriteIdByName("investment_icon"),
                    UINotificationController.NotificationColor.SILVER,
                    true,
                    false
                    );
            AkSoundEngine.PostEvent("Play_OBJ_shrine_accept_01", base.gameObject);
            numUses++;
        }
        public override string AcceptText(PlayerController interactor)
        {
            return $"Invest! <Spend {(10 * (numUses + 1))}[sprite \"ui_coin\"]>";
        }
        public override string DeclineText(PlayerController Interactor)
        {
            return "Leave";
        }
        public override string PanelText(PlayerController Interactor)
        {
            return "A shrine to the darkest of demonic hordes- the board of investors.\nGiving up your money seems like a great investment opportunity.";
        }
    }
}

