using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dungeonator;
using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.Misc;

namespace NevernamedsItems
{
    class GuruMeditation : PassiveItem
    {
        public static void Init()
        {
            PassiveItem item = ItemSetup.NewItem<GuruMeditation>(
			  "Guru Meditation",
              "Peace Fire",
              "Standing still increases the firerate and accuracy of the practitioners weapons."+"\n\nOnly the most ancient and revered gunslingers remember the ancient art of putting one foot in front of the other, and squaring their shoulders before they fire.",
              "gurumeditation_icon") as PassiveItem;
            item.quality = PickupObject.ItemQuality.C;
		}
        public override void Update()
        {
			if (Owner)
			{
				if (Owner.specRigidbody != null)
                {
					if (Owner.specRigidbody.Velocity == Vector2.zero) { timeStandingStill += BraveTime.DeltaTime; }
					else { timeStandingStill = 0; }
                }
				if (timeStandingStill >= 0.7f) { if (!effectEnabled) { Enable(Owner); } }
				else  { if (effectEnabled) { Disable(Owner); } }
			}
			base.Update();
        }
		public float timeStandingStill = 0;
        public override void DisableEffect(PlayerController player)
        {
			if (player) { Disable(player); }
            base.DisableEffect(player);
        }

        public bool effectEnabled = false;
		public void Enable(PlayerController target)
		{
			if (!effectEnabled)
			{
				AkSoundEngine.PostEvent("Play_ITM_Macho_Brace_Active_01", base.gameObject);
				this.RemovePassiveStatModifier(PlayerStats.StatType.ChargeAmountMultiplier);
				this.AddPassiveStatModifier(new StatModifier() { statToBoost = PlayerStats.StatType.ChargeAmountMultiplier, amount = 1.5f, modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE });
				this.RemovePassiveStatModifier(PlayerStats.StatType.RateOfFire);
				this.AddPassiveStatModifier(new StatModifier() { statToBoost = PlayerStats.StatType.RateOfFire, amount = 1.5f, modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE });
				this.RemovePassiveStatModifier(PlayerStats.StatType.Accuracy);
				this.AddPassiveStatModifier(new StatModifier() { statToBoost = PlayerStats.StatType.Accuracy, amount = 0.6f, modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE });
				target.stats.RecalculateStatsWithoutRebuildingGunVolleys(target);
				Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(target.sprite);
				if (outlineMaterial != null) outlineMaterial.SetColor("_OverrideColor", new Color(100f, 0f, 0f));
				effectEnabled = true;
			}
		}

		public void Disable(PlayerController target)
		{
			if (effectEnabled)
			{
				AkSoundEngine.PostEvent("Play_ITM_Macho_Brace_Fade_01", base.gameObject);
				this.RemovePassiveStatModifier(PlayerStats.StatType.ChargeAmountMultiplier);
				this.RemovePassiveStatModifier(PlayerStats.StatType.Accuracy);
				this.RemovePassiveStatModifier(PlayerStats.StatType.RateOfFire);
				target.stats.RecalculateStatsWithoutRebuildingGunVolleys(target);
				Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(target.sprite);
				if (outlineMaterial != null) outlineMaterial.SetColor("_OverrideColor", new Color(0f, 0f, 0f));
				effectEnabled = false;
			}
		}
	}
}