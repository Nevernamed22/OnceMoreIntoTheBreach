using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dungeonator;
using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.Misc;
using System.Collections;

namespace NevernamedsItems
{
    class SetOfAllSets : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<SetOfAllSets>(
            "Set of All Sets",
            "Contains Itself",
            "On damage, triggers a random 'on hit' effect." + "\n\nA physical manifestation of the question which has been hassling mathematicians and philosophers for centuries. The answer is yes.",
            "setofallsets_icon");
            item.quality = PickupObject.ItemQuality.C;
        }
        private void OnHit(PlayerController user)
        {
            int times = 1;
            if (user.PlayerHasActiveSynergy("Recursive Recursions")) times = 2;
            for (int i = 0; i < times; i++)
            {
            int effect = UnityEngine.Random.Range(0, 17);
            switch (effect)
            {
                case 0: // Enraging Photo
                    user.GetExtComp().Enrage(3f, true);
                    if (user.CurrentGun != null) { user.CurrentGun.ForceImmediateReload(false); }
                    break;
                case 1: // Honeycomb
                    SpawnProjectileOnDamagedItem honeycomb = PickupObjectDatabase.GetById(138).GetComponent<SpawnProjectileOnDamagedItem>();
                    SpawnProjectiles(honeycomb.minNumToSpawn, honeycomb.maxNumToSpawn, honeycomb.projectileToSpawn, honeycomb.randomAngle, user);
                    break;
                case 2: // Heart of Ice
                    SpawnProjectileOnDamagedItem heartOfIce = PickupObjectDatabase.GetById(364).GetComponent<SpawnProjectileOnDamagedItem>();
                    SpawnProjectiles(heartOfIce.minNumToSpawn, heartOfIce.maxNumToSpawn, heartOfIce.projectileToSpawn, heartOfIce.randomAngle, user);
                    break;
                case 3: // Green Guon Stone
                    user.healthHaver.ApplyHealing(0.5f);
                    break;
                case 4: //Blue Guon Stone
                    if (!m_isSlowingBullets) user.StartCoroutine(HandleSlowBullets());
                    else m_slowDurationRemaining = SlowBulletsDuration;
                    break;
                case 5: //Holey Grail
                    user.ForceBlank(25f, 0.5f, false, true, null, true, -1f);
                    if (user.inventory != null && user.inventory.AllGuns != null)
                    {
                        for (int j = 0; j < user.inventory.AllGuns.Count; j++)
                        {
                            Gun gun = user.inventory.AllGuns[j];
                            if (!gun.InfiniteAmmo && gun.CanGainAmmo) { gun.GainAmmo(Mathf.CeilToInt((float)gun.AdjustedMaxAmmo * 0.01f * 5)); }
                        }
                    }
                    break;
                case 6: // Monster Blood
                    DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(EasyGoopDefinitions.PlayerFriendlyPoisonGoop).TimedAddGoopCircle(user.specRigidbody.UnitCenter, PickupObjectDatabase.GetById(313).GetComponent<PassiveGooperItem>().goopRadius, 0.5f, false);
                    break;
                case 7: // GUNNER
                    SpawnGunnerSkull(user);
                    break;
                case 8: // Heart of Gold
                    LootEngine.SpawnCurrency(user.sprite.WorldCenter, 10);
                    break;
                case 9: // Cheese Heart
                    DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(EasyGoopDefinitions.CheeseDef).TimedAddGoopCircle(user.sprite.WorldCenter, 10, 1, false);
                    break;
                case 10: //Panic Pendant
                    friendlyifyTimer = 2;
                    foreach (Projectile proj in StaticReferenceManager.AllProjectiles)
                    {
                        if (proj.Owner == null || !(proj.Owner is PlayerController)) { ConvertBullet(proj); }
                    }
                    break;
                case 11: // Shadow Ring
                    DoShadowRing(user);
                    break;
                case 12: // Liquid-Metal Body
                    StartCoroutine(HandleShield(user));
                    break;
                case 13: // Dragun Scale
                    FullRoomStatusEffect(user, StaticStatusEffects.hotLeadEffect);
                    break;
                case 14: // Purple Prose
                    FullRoomStatusEffect(user, StaticStatusEffects.charmingRoundsEffect);
                    break;
                case 15: // Clay Sculpture
                    AIActor randomEnemy = user.CurrentRoom.GetRandomActiveEnemy();
                    if (randomEnemy != null && !randomEnemy.healthHaver.IsBoss) randomEnemy.ForceFall();
                    break;
                case 16: //Death Mask
                    DeathCurse(user);
                    break;
            }
            }
        }
        private void DeathCurse(PlayerController playa)
        {
            List<AIActor> activeEnemies = playa.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            GameManager.Instance.MainCameraController.DoScreenShake(StaticExplosionDatas.genericLargeExplosion.ss, null, false);
            Pixelator.Instance.FadeToColor(0.1f, Color.white, true, 0.1f);
            Exploder.DoDistortionWave(playa.CenterPosition, 0.4f, 0.15f, 10f, 0.4f);
            if (playa.CurrentRoom != null) playa.CurrentRoom.ClearReinforcementLayers();

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
        private void FullRoomStatusEffect(PlayerController user, GameActorEffect effect)
        {
            List<AIActor> activeEnemies = user.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            if (activeEnemies != null)
            {
                for (int i = 0; i < activeEnemies.Count; i++)
                {
                    AIActor aiactor = activeEnemies[i];
                    if (aiactor.IsNormalEnemy) { aiactor.gameActor.ApplyEffect(effect, 1f, null); }
                }
            }
        }
        private void SpawnProjectiles(int minNum, int maxnum, Projectile prefab, bool randomAngle, PlayerController player)
        {
            int num = UnityEngine.Random.Range(minNum, maxnum + 1);
            float num2 = 360f / (float)num;
            float num3 = UnityEngine.Random.Range(0f, num2);

            for (int i = 0; i < num; i++)
            {
                float num4 = (!randomAngle) ? (num3 + num2 * (float)i) : ((float)UnityEngine.Random.Range(0, 360));
                GameObject gameObject = SpawnManager.SpawnProjectile(prefab.gameObject, player.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, num4), true);
                Projectile component = gameObject.GetComponent<Projectile>();
                component.Owner = player;
                component.Shooter = player.specRigidbody;
            }
        }
        private IEnumerator HandleSlowBullets()
        {
            yield return new WaitForEndOfFrame();
            m_isSlowingBullets = true;
            float slowMultiplier = SlowBulletsMultiplier;
            Projectile.BaseEnemyBulletSpeedMultiplier *= slowMultiplier;
            m_slowDurationRemaining = SlowBulletsDuration;
            while (m_slowDurationRemaining > 0f)
            {
                yield return null;
                m_slowDurationRemaining -= BraveTime.DeltaTime;
                Projectile.BaseEnemyBulletSpeedMultiplier /= slowMultiplier;
                slowMultiplier = Mathf.Lerp(SlowBulletsMultiplier, 1f, 1f - m_slowDurationRemaining);
                Projectile.BaseEnemyBulletSpeedMultiplier *= slowMultiplier;
            }
            Projectile.BaseEnemyBulletSpeedMultiplier /= slowMultiplier;
            m_isSlowingBullets = false;
            yield break;
        }
        private void SpawnGunnerSkull(PlayerController p)
        {
            GameObject m_extantSkull = SpawnManager.SpawnDebris(PickupObjectDatabase.GetById(602).GetComponent<GunnerGunController>().SkullPrefab, p.CenterPosition.ToVector3ZisY(0f), Quaternion.identity);
            DebrisObject component = m_extantSkull.GetComponent<DebrisObject>();
            component.FlagAsPickup();
            component.Trigger((UnityEngine.Random.insideUnitCircle.normalized * 20f).ToVector3ZUp(3f), 1f, 0f);
            SpeculativeRigidbody component2 = m_extantSkull.GetComponent<SpeculativeRigidbody>();
            PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(component2, null, false);
            component2.RegisterTemporaryCollisionException(p.specRigidbody, 0.25f, null);
            SpeculativeRigidbody speculativeRigidbody = component2;
            speculativeRigidbody.OnEnterTrigger += HandleSkullTrigger;
            component2.StartCoroutine(HandleGunnerSkullLifespan(m_extantSkull));
        }
        private IEnumerator HandleGunnerSkullLifespan(GameObject source)
        {
            yield return new WaitForSeconds(4f);
            LootEngine.DoDefaultPurplePoof(source.transform.position + new Vector3(0.75f, 0.5f, 0f), false);
            UnityEngine.Object.Destroy(source);
            yield break;
        }
        private void HandleSkullTrigger(SpeculativeRigidbody specRigidbody, SpeculativeRigidbody sourceSpecRigidbody, CollisionData collisionData)
        {
            if (specRigidbody)
            {
                PlayerController component = specRigidbody.GetComponent<PlayerController>();
                if (component)
                {
                    sourceSpecRigidbody.OnEnterTrigger -= HandleSkullTrigger;
                    tk2dSpriteAnimator component2 = sourceSpecRigidbody.gameObject.GetComponent<tk2dSpriteAnimator>();
                    component2.PlayAndDestroyObject("gonner_skull_pickup_vfx", null);

                    if (component.characterIdentity == PlayableCharacters.Robot) component.healthHaver.Armor += 1;
                    else component.healthHaver.ApplyHealing(0.5f);
                    AkSoundEngine.PostEvent("Play_OBJ_heart_heal_01", base.gameObject);
                    component.PlayEffectOnActor(EasyVFXDatabase.HealingSparkles, Vector3.zero, true, false, false);
                }
            }
        }
        private void NewBulletAppeared(Projectile proj)
        {
            if (friendlyifyTimer > 0)
            {
                if (proj.Owner == null || !(proj.Owner is PlayerController))
                {
                    ConvertBullet(proj);
                }
            }
        }
        public override void Update()
        {
            if (friendlyifyTimer >= 0)
            {
                friendlyifyTimer -= BraveTime.DeltaTime;
            }
            base.Update();
        }
        private void DoShadowRing(PlayerController user)
        {
            GameObject shadowPrefab = PickupObjectDatabase.GetById(820).GetComponent<SpawnObjectPlayerItem>().objectToSpawn;
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(shadowPrefab, user.specRigidbody.UnitCenter, Quaternion.identity);
            tk2dBaseSprite component2 = gameObject.GetComponent<tk2dBaseSprite>();
            if (component2 != null)
            {
                component2.PlaceAtPositionByAnchor(user.specRigidbody.UnitCenter.ToVector3ZUp(component2.transform.position.z), tk2dBaseSprite.Anchor.MiddleCenter);
                if (component2.specRigidbody != null)
                {
                    component2.specRigidbody.RegisterGhostCollisionException(user.specRigidbody);
                }
            }
            KageBunshinController component3 = gameObject.GetComponent<KageBunshinController>();
            if (component3)
            {
                component3.InitializeOwner(user);
            }
            gameObject.transform.position = gameObject.transform.position.Quantize(0.0625f);
        }
        private void ConvertBullet(Projectile proj)
        {
            Vector2 dir = proj.Direction;
            if (proj.Owner && proj.Owner.specRigidbody) proj.specRigidbody.DeregisterSpecificCollisionException(proj.Owner.specRigidbody);

            proj.Owner = Owner;
            proj.SetNewShooter(Owner.specRigidbody);
            proj.allowSelfShooting = false;
            proj.collidesWithPlayer = false;
            proj.collidesWithEnemies = true;
            proj.baseData.damage = 15;
            if (proj.IsBlackBullet) proj.baseData.damage *= 2;
            PlayerController player = (Owner as PlayerController);
            if (player != null)
            {
                proj.baseData.damage *= player.stats.GetStatValue(PlayerStats.StatType.Damage);
                proj.baseData.speed *= player.stats.GetStatValue(PlayerStats.StatType.ProjectileSpeed);
                proj.UpdateSpeed();
                proj.baseData.force *= player.stats.GetStatValue(PlayerStats.StatType.KnockbackMultiplier);
                proj.baseData.range *= player.stats.GetStatValue(PlayerStats.StatType.RangeMultiplier);
                proj.BossDamageMultiplier *= player.stats.GetStatValue(PlayerStats.StatType.DamageToBosses);
                proj.RuntimeUpdateScale(player.stats.GetStatValue(PlayerStats.StatType.PlayerBulletScale));
                if (player.stats.GetStatValue(PlayerStats.StatType.AdditionalShotBounces) > 0)
                {
                    bool hadComp = proj.gameObject.GetComponent<BounceProjModifier>();
                    BounceProjModifier bounce = proj.gameObject.GetOrAddComponent<BounceProjModifier>();

                    if (hadComp) bounce.numberOfBounces += (int)player.stats.GetStatValue(PlayerStats.StatType.AdditionalShotBounces);
                    else bounce.numberOfBounces = (int)player.stats.GetStatValue(PlayerStats.StatType.AdditionalShotBounces);
                }
                player.DoPostProcessProjectile(proj);
            }
            if (proj.GetComponent<BeamController>() != null)
            {
                proj.GetComponent<BeamController>().HitsPlayers = false;
                proj.GetComponent<BeamController>().HitsEnemies = true;
            }
            else if (proj.GetComponent<BasicBeamController>() != null)
            {
                proj.GetComponent<BasicBeamController>().HitsPlayers = false;
                proj.GetComponent<BasicBeamController>().HitsEnemies = true;
            }
            proj.AdjustPlayerProjectileTint(ExtendedColours.honeyYellow, 1);
            proj.UpdateCollisionMask();
            proj.RemoveFromPool();
            proj.Reflected();
            proj.SendInDirection(dir, false);
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            StaticReferenceManager.ProjectileAdded += NewBulletAppeared;
            player.OnReceivedDamage += OnHit;
        }
        public override void DisableEffect(PlayerController player)
        {
            if (player) { player.OnReceivedDamage -= OnHit; }
            StaticReferenceManager.ProjectileAdded -= NewBulletAppeared;
            base.DisableEffect(player);
        }
        private IEnumerator HandleShield(PlayerController user)
        {
          bool  m_usedOverrideMaterial = user.sprite.usesOverrideMaterial;
            user.sprite.usesOverrideMaterial = true;
            user.SetOverrideShader(ShaderCache.Acquire("Brave/ItemSpecific/MetalSkinShader"));
            SpeculativeRigidbody specRigidbody = user.specRigidbody;
            specRigidbody.OnPreRigidbodyCollision  += OnLeadSkinPreCollision;
            user.healthHaver.IsVulnerable = false;
            float elapsed = 0f;
            while (elapsed < 3.5f)
            {
                elapsed += BraveTime.DeltaTime;
                user.healthHaver.IsVulnerable = false;
                yield return null;
            }
            if (user)
            {
                user.healthHaver.IsVulnerable = true;
                user.ClearOverrideShader();
                user.sprite.usesOverrideMaterial = m_usedOverrideMaterial;
                SpeculativeRigidbody specRigidbody2 = user.specRigidbody;
                specRigidbody2.OnPreRigidbodyCollision -= OnLeadSkinPreCollision;
                AkSoundEngine.PostEvent("Play_OBJ_metalskin_end_01", user.gameObject);
            }
            yield break;
        }
        private void OnLeadSkinPreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherCollider)
        {
            Projectile component = otherRigidbody.GetComponent<Projectile>();
            if (component != null && !(component.Owner is PlayerController))
            {
                PassiveReflectItem.ReflectBullet(component, true, Owner.specRigidbody.gameActor, 10f, 1f, 1f, 0f);
                PhysicsEngine.SkipCollision = true;
            }
        }

        public float friendlyifyTimer;

        public float SlowBulletsDuration = 15f;
        public float SlowBulletsMultiplier = 0.5f;
        private bool m_isSlowingBullets;
        private float m_slowDurationRemaining;
    }
}
