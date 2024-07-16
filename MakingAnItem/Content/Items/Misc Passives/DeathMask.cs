using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dungeonator;
using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.Misc;
using SaveAPI;

namespace NevernamedsItems
{
    class DeathMask : PassiveItem
    {
        public static void Init()
        {
            PassiveItem item = ItemSetup.NewItem<DeathMask>(
              "Death Mask",
              "Face Off",
              "Chance to clear the room when damage is taken or a blank is used. \n\nA cracked burial mask worn by a high class noble at their first funeral.",
              "deathmask_icon") as PassiveItem;
            item.quality = PickupObject.ItemQuality.B;
            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.RAT_KILLED_SHADE, true);
        }
        private void OnUseBlank(PlayerController cont, int idk)
        {
            if (UnityEngine.Random.value <= 0.1f) { DeathCurse(cont); }
        }
        private void OnRecievedDamage(PlayerController player)
        {
            if (UnityEngine.Random.value <= 0.1f) { DeathCurse(player); }
        }
        public override void Pickup(PlayerController player)
        {
            player.OnUsedBlank += OnUseBlank;
            player.OnReceivedDamage += OnRecievedDamage;
            base.Pickup(player);
        }
        public override void DisableEffect(PlayerController player)
        {
            player.OnUsedBlank -= OnUseBlank;
            player.OnReceivedDamage -= OnRecievedDamage;
            base.DisableEffect(player);
        }
        private void DeathCurse(PlayerController playa)
        {
            List<AIActor> activeEnemies = playa.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            GameManager.Instance.MainCameraController.DoScreenShake(StaticExplosionDatas.genericLargeExplosion.ss, null, false);
            Pixelator.Instance.FadeToColor(0.1f, Color.white, true, 0.1f);
            Exploder.DoDistortionWave(playa.CenterPosition, 0.4f, 0.15f, 10f, 0.4f);
         if (playa.CurrentRoom != null)   playa.CurrentRoom.ClearReinforcementLayers();

            if (activeEnemies != null)
            {
                for (int i = 0; i < activeEnemies.Count; i++)
                {
                    AIActor aiactor = activeEnemies[i];
                    if (aiactor.IsNormalEnemy && aiactor.healthHaver)
                    {
                        aiactor.healthHaver.ApplyDamage(aiactor.healthHaver.IsBoss ? 100 : 1E+07f, Vector2.zero, string.Empty, CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
                    }
                }
            }

        }
    }
}