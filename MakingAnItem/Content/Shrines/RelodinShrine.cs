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
	public class RelodinShrine : GenericShrine
	{
		public static GameObject Setup(GameObject pedestal)
		{
			var shrineobj = ItemBuilder.SpriteFromBundle("shrine_relodin", Initialisation.NPCCollection.GetSpriteIdByName("shrine_relodin"), Initialisation.NPCCollection, new GameObject("Shrine Relodin Statue"));
			shrineobj.GetComponent<tk2dSprite>().HeightOffGround = 1.25f;
			shrineobj.GetComponent<tk2dSprite>().renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTiltedCutout");
			shrineobj.GetComponent<tk2dSprite>().usesOverrideMaterial = true;
			pedestal.AddComponent<RelodinShrine>();
			GameObject talkpoint = new GameObject("talkpoint");
			talkpoint.transform.SetParent(pedestal.transform);
			talkpoint.transform.localPosition = new Vector3(1f, 36f / 16f, 0f);

			Alexandria.Misc.TextHelper.RegisterCustomTokenInsert("NevernamedsItems/Resources/UISprites/accuracy_ui.png", "accuracy_ui");

			return shrineobj;
		}
		public override bool CanAccept(PlayerController interactor) { return timesAccepted == 0; }
		public override void OnAccept(PlayerController Interactor)
		{
			StatModifier SpreadUp = new StatModifier
			{
				statToBoost = PlayerStats.StatType.Accuracy,
				amount = 2f,
				modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE
			};
			StatModifier DamageUp = new StatModifier
			{
				statToBoost = PlayerStats.StatType.Damage,
				amount = 1.3f,
				modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE
			};
			Interactor.ownerlessStatModifiers.Add(SpreadUp);
			Interactor.ownerlessStatModifiers.Add(DamageUp);
			Interactor.stats.RecalculateStats(Interactor);

			GameUIRoot.Instance.notificationController.DoCustomNotification(
				    "Blinded",
					"Wisdom of the Gun",
					Initialisation.NPCCollection,
					Initialisation.NPCCollection.GetSpriteIdByName("relodin_popup"),
					UINotificationController.NotificationColor.SILVER,
					true,
					false
					);
			AkSoundEngine.PostEvent("Play_OBJ_shrine_accept_01", base.gameObject);
		}
		public override string AcceptText(PlayerController interactor)
		{
			return "Give an eye for knowledge <Accuracy Down [sprite \"accuracy_ui\"]>";
		}
		public override string DeclineText(PlayerController Interactor)
		{
			return "Leave";
		}
		public override string PanelText(PlayerController Interactor)
		{
			return timesAccepted == 0 ? "A shrine to the lord of chambered rounds Relodin. According to legend, he plucked out his eye as a tribute to Kaliber, and in return recieved great wisdom." : "The spirits inhabiting this shrine have departed...";
		}
	}
}

