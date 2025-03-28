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
    class Payback : PassiveItem
    {
        public static void Init()
        {
            PassiveItem item = ItemSetup.NewItem<Payback>(
              "Payback",
              "The Golden Touch",
              "Upon taking damage, closeby enemies become gilded. Enemies that shoot you also become gilded.\n\nThey can hurt you, but ultimately they'll be the ones paying for it.",
              "payback_icon") as PassiveItem;
            item.quality = PickupObject.ItemQuality.C;
        }
        public override void Pickup(PlayerController player)
        {
            player.healthHaver.OnDamaged += OnDamage;
            player.OnHitByProjectile += OnShot;
            base.Pickup(player);
        }
        public override void DisableEffect(PlayerController player)
        {
            if (player)
            {
                player.healthHaver.OnDamaged -= OnDamage;
                player.OnHitByProjectile -= OnShot;
            }
            base.DisableEffect(player);
        }
        private float Radius
        {
            get
            {
                float num = 2f;
                if (Owner && Owner.PlayerHasActiveSynergy("Paid In Full")) { num = 4f; }
                return num;
            }
        }
        private float Duration
        {
            get
            {
                float num = 10f;
                if (Owner && Owner.PlayerHasActiveSynergy("Paid In Full")) { num = 1000f; }
                return num;
            }
        }
        public void OnDamage(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
        {
            if (Owner)
            {
                Exploder.DoDistortionWave(Owner.CenterPosition, 0.5f, 0.04f, Radius, 0.3f);
                Exploder.DoRadialKnockback(Owner.CenterPosition, 5, Radius);
                AkSoundEngine.PostEvent("Play_ITM_Macho_Brace_Active_01", base.gameObject);
                List<AIActor> activeEnemies = Owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
                if (activeEnemies != null)
                {
                    for (int i = 0; i < activeEnemies.Count; i++)
                    {
                        AIActor aiactor = activeEnemies[i];
                        if (aiactor != null && aiactor.IsNormalEnemy && aiactor.transform)
                        {
                            float num = Vector2.Distance(Owner.CenterPosition, aiactor.CenterPosition);
                            if (num <= Radius)
                            {
                                aiactor.ApplyEffect(new GameActorGildedEffect()
                                {
                                    duration = 10,
                                    stackMode = GameActorEffect.EffectStackingMode.Refresh,
                                });
                            }
                        }
                    }
                }
            }

            
        }
        public void OnShot(Projectile hitter, PlayerController tro)
        {
            if (hitter != null && hitter.Owner != null)
            {
                hitter.Owner.ApplyEffect(new GameActorGildedEffect()
                {
                    duration = Duration,
                    stackMode = GameActorEffect.EffectStackingMode.Refresh,
                });
            }
        }
    }
}