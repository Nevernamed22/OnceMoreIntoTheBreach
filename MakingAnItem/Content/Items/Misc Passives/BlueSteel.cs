using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;
using MonoMod.RuntimeDetour;
using System.Reflection;
using Dungeonator;

namespace NevernamedsItems
{
    public class BlueSteel : AffectEnemiesInProximityTickItem
    {
        public static void Init()
        {
            AffectEnemiesInProximityTickItem item = ItemSetup.NewItem<BlueSteel>(
               "Blue Steel",
               ":o",
               "Freeze enemies in place with your captivating trademark stare!",
               "bluesteel_icon") as AffectEnemiesInProximityTickItem;
            item.range = 5f;
            item.CanBeDropped = true;
            item.quality = PickupObject.ItemQuality.B;

            lockdownToApply = new GameActorSpeedEffect
            {
                duration = 0.5f,
                TintColor = Color.grey,
                DeathTintColor = Color.grey,
                effectIdentifier = "Lockdown",
                AppliesTint = true,
                AppliesDeathTint = false,
                resistanceType = EffectResistanceType.None,
                SpeedMultiplier = 0f,

                //Eh
                OverheadVFX = SharedVFX.LockdownOverhead,
                AffectsEnemies = true,
                AffectsPlayers = false,
                AppliesOutlineTint = false,
                OutlineTintColor = Color.grey,
                PlaysVFXOnActor = false,

                stackMode = GameActorEffect.EffectStackingMode.Refresh,
            };
        }
        public static GameActorSpeedEffect lockdownToApply;
        public override void AffectEnemy(AIActor aiactor)
        {
            aiactor.ApplyEffect(lockdownToApply);
            base.AffectEnemy(aiactor);
        }
    }
}
