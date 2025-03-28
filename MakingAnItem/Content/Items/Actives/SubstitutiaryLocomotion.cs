using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dungeonator;
using UnityEngine;
using Alexandria.ItemAPI;
using SaveAPI;
using System.Collections;

namespace NevernamedsItems
{
    class SubstitutiaryLocomotion : PlayerItem
    {
        public static void Init()
        {
            PlayerItem item = ItemSetup.NewItem<SubstitutiaryLocomotion>(
            "Substitutiary Locomotion",
            "Star of Astoroth",
            "Breathes ancient life into lifeless objects.\n\nAn ancient emblem of witchcraft. The words \"Treguna mekoides trecorum satis dee\" are inscribed around the rim.",
            "substitutiarylocomotion_icon") as PlayerItem;
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 400);
            item.consumable = false;
            item.quality = ItemQuality.C;
            item.AddToSubShop(ItemBuilder.ShopType.Cursula);
        }
        public override void DoEffect(PlayerController user)
        {
            user.StartCoroutine(SubstitutiaryLocomotion.HandleEffect(user));
        }
        public static bool incanting = false;
        private static IEnumerator HandleEffect(PlayerController user)
        {
            incanting = true;
            AkSoundEngine.PostEvent("TregunaMekoidesTrecorumSatisDee", user.gameObject);

            List<SpeculativeRigidbody> trg1 = new List<SpeculativeRigidbody>();
            List<SpeculativeRigidbody> trg2 = new List<SpeculativeRigidbody>();
            List<SpeculativeRigidbody> trg3 = new List<SpeculativeRigidbody>();
            List<SpeculativeRigidbody> trg4 = new List<SpeculativeRigidbody>();

            int iterator = 1;
            foreach (MinorBreakable br in StaticReferenceManager.AllMinorBreakables)
            {
                if (br && !br.IsBroken && br.specRigidbody != null && br.GetComponent<PathfindingProjectile>() == null)
                {
                    RoomHandler rH = br.specRigidbody.GetUnitCenter(ColliderType.HitBox).GetAbsoluteRoom();
                    if (rH != null && rH == user.CurrentRoom)
                    {
                        br.OnlyBrokenByCode = true;
                        br.isInvulnerableToGameActors = true;
                        br.resistsExplosions = true;
                        br.specRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.Projectile;
                        br.specRigidbody.Reinitialize();
                        switch (iterator)
                        {
                            case 1: trg1.Add(br.specRigidbody); iterator++; break;
                            case 2: trg2.Add(br.specRigidbody); iterator++; break;
                            case 3: trg3.Add(br.specRigidbody); iterator++; break;
                            case 4: trg4.Add(br.specRigidbody); iterator = 1; break;
                        }
                    }
                }
            }

            user.StartCoroutine(LifeWave(trg1, user.CenterPosition, user));
            yield return new WaitForSeconds(2f);
            user.StartCoroutine(LifeWave(trg2, user.CenterPosition, user));
            yield return new WaitForSeconds(2f);
            user.StartCoroutine(LifeWave(trg3, user.CenterPosition, user));
            yield return new WaitForSeconds(1.5f);
            user.StartCoroutine(LifeWave(trg4, user.CenterPosition, user));
            yield return new WaitForSeconds(2f);
            incanting = false;
            yield break;
        }
        private static IEnumerator LifeWave(List<SpeculativeRigidbody> waveTargets, Vector2 waveCenter, PlayerController player)
        {
            float m_timer = 1.5f - BraveTime.DeltaTime;
            float m_prevWaveDist = 0f;

            Exploder.DoDistortionWave(waveCenter, 0.5f, 0.04f, 20f, 1.5f);

            while (m_timer > 0)
            {
                m_timer -= BraveTime.DeltaTime;
                float num = BraveMathCollege.LinearToSmoothStepInterpolate(0f, 20f, 1f - m_timer / 1.5f);

                if (waveTargets != null)
                {
                    for (int i = 0; i < waveTargets.Count; i++)
                    {
                        SpeculativeRigidbody indivTarget = waveTargets[i];

                        Vector2 unitCenter = indivTarget.GetUnitCenter(ColliderType.HitBox);

                        float num2 = Vector2.Distance(unitCenter, waveCenter);
                        if (num2 >= m_prevWaveDist - 0.25f && num2 <= num + 0.25f)
                        {
                            if (indivTarget != null)
                            {
                                if (indivTarget.minorBreakable && !indivTarget.minorBreakable.IsBroken)
                                {
                                    BringToLife(indivTarget.gameObject, player);
                                }
                            }
                        }
                    }
                }
                for (int i = waveTargets.Count - 1; i >= 0; i--)
                {
                    if (waveTargets[i] && waveTargets[i].gameObject && waveTargets[i].gameObject.GetComponent<PathfindingProjectile>() == null)
                    {
                        if (waveTargets[i].minorBreakable && !waveTargets[i].minorBreakable.IsBroken)
                        {
                            BringToLife(waveTargets[i].gameObject, player);
                        }
                    }
                }
                m_prevWaveDist = num;
                yield return null;
            }

            yield break;
        }
        public static void BringToLife(GameObject thingy, PlayerController owner)
        {
            if (thingy.GetComponent<PathfindingProjectile>() != null) { return; }

            IPlayerInteractable @interface = thingy.GetInterface<IPlayerInteractable>();
            if (@interface != null)
            {
                RoomHandler roomFromPosition = GameManager.Instance.Dungeon.GetRoomFromPosition(thingy.transform.position.IntXY(VectorConversions.Round));
                if (roomFromPosition.IsRegistered(@interface))
                {
                    roomFromPosition.DeregisterInteractable(@interface);
                }
            }

            MinorBreakable minor = thingy.GetComponent<MinorBreakable>();
            if (minor)
            {
            thingy.GetComponent<MinorBreakable>().OnlyBrokenByCode = true;
            thingy.GetComponent<MinorBreakable>().isInvulnerableToGameActors = true;
            thingy.GetComponent<MinorBreakable>().resistsExplosions = true;
            }

            PathfindingProjectile projectile = thingy.GetOrAddComponent<PathfindingProjectile>();
            projectile.shouldRotate = false;
            //projectile.shouldFlipHorizontally = true;
            projectile.Shooter = owner.specRigidbody;
            projectile.Owner = owner;
            projectile.baseData.damage = 8 * owner.stats.GetStatValue(PlayerStats.StatType.Damage);
            projectile.baseData.range = 200f;
            projectile.baseData.speed = UnityEngine.Random.Range(5, 8);
            projectile.collidesWithProjectiles = false;
            projectile.gameObject.GetOrAddComponent<HitEffectHandler>();
            projectile.gameObject.AddComponent<PierceProjModifier>();
            projectile.gameObject.AddComponent<PierceDeadActors>();
            projectile.pierceMinorBreakables = true;
            projectile.baseData.force = 30f;
            projectile.hitEffects = (PickupObjectDatabase.GetById(541) as Gun).DefaultModule.chargeProjectiles[0].Projectile.hitEffects;
            projectile.enemyImpactEventName = "";
            projectile.specRigidbody.CollideWithTileMap = false;
            projectile.specRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.Projectile;
            projectile.UpdateCollisionMask();
            projectile.specRigidbody.Reinitialize();
            projectile.Start();
            projectile.gameObject.AddComponent<GravityGun.GravityGunObjectDeathHandler>();
        }
        public override bool CanBeUsed(PlayerController user)
        {
            return !incanting;
        }
    }
}

