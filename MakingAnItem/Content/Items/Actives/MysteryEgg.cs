using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Dungeonator;
using Alexandria.ItemAPI;
using Alexandria.Misc;
using UnityEngine;
using Alexandria.Assetbundle;

namespace NevernamedsItems
{
    public class MysteryEgg : PlayerItem
    {
        public static void Init()
        {
            PlayerItem item = ItemSetup.NewItem<MysteryEgg>(
              "Mystery Egg",
              "What Will It Be?",
              "Has a random effect when used. Magical eggs like these are one of the few items totally illegal within the Gungeon.\n\nNot for children under 5. Small parts may present a choking hazard.",
              "mysteryegg_icon") as PlayerItem;
            //ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 300);
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.None, 300);
            item.quality = ItemQuality.D;

            MysteryEgg.ID = item.PickupObjectId;
        }

        public static int ID;
        public static float leadSkinDuration = 10;
        public static float bulletTimeDuration = 6;
        public static float fortunesFavourDuration = 8;


        public override void DoEffect(PlayerController user)
        {
            int effect = UnityEngine.Random.Range(1, 11);
            switch (effect)
            {
                case 1: //Potion of Lead Skin
                    Bloop(user, 64);
                    user.StartCoroutine(this.HandleShield(user));
                    AkSoundEngine.PostEvent("Play_OBJ_metalskin_activate_01", base.gameObject);
                    break;
                case 2: //Knife Shield
                    Bloop(user, 65);
                    KnifeShieldEffect knifeShieldEffect = new GameObject("knife shield effect") { transform = { position = user.LockedApproximateSpriteCenter, parent = user.transform } }.AddComponent<KnifeShieldEffect>();
                    knifeShieldEffect.numKnives = 8;
                    knifeShieldEffect.remainingHealth = 0.5f;
                    knifeShieldEffect.knifeDamage = 30;
                    knifeShieldEffect.circleRadius = 2;
                    knifeShieldEffect.rotationDegreesPerSecond = 270;
                    knifeShieldEffect.throwSpeed = 15;
                    knifeShieldEffect.throwRange = 25;
                    knifeShieldEffect.throwRadius = 1.5f;
                    knifeShieldEffect.radiusChangeDistance = 5;
                    knifeShieldEffect.deathVFX = (PickupObjectDatabase.GetById(65) as KnifeShieldItem).knifeDeathVFX;
                    knifeShieldEffect.Initialize(user, (PickupObjectDatabase.GetById(65) as KnifeShieldItem).knifePrefab);
                    AkSoundEngine.PostEvent("Play_OBJ_daggershield_start_01", base.gameObject);
                    break;
                case 3: //Proximity Mine
                    Bloop(user, 66);
                    SpawnObject((PickupObjectDatabase.GetById(66) as SpawnObjectPlayerItem).objectToSpawn, user);
                    break;
                case 4: //Bullet Time
                    Bloop(user, 69);
                    user.StartCoroutine(this.DoBulletTime(user));
                    break;
                case 5: //Decoy
                    Bloop(user, 71);
                    SpawnObject((PickupObjectDatabase.GetById(71) as SpawnObjectPlayerItem).objectToSpawn, user);
                    break;
                case 6: //Supply Drop
                    Bloop(user, 77);
                    IntVector2 key = user.SpawnEmergencyCrate((PickupObjectDatabase.GetById(77) as SupplyDropItem).itemTableToUse);
                    AkSoundEngine.PostEvent("Play_OBJ_supplydrop_activate_01", base.gameObject);
                    break;
                case 7: //Fortunes Favour
                    Bloop(user, 105);
                    base.StartCoroutine(this.DoFortunesFavour(user));
                    AkSoundEngine.PostEvent("Play_OBJ_fortune_shield_01", base.gameObject);
                    break;
                case 8: //Bomb
                    Bloop(user, 108);
                    SpawnObject((PickupObjectDatabase.GetById(108) as SpawnObjectPlayerItem).objectToSpawn, user, 6f);
                    AkSoundEngine.PostEvent("Play_OBJ_bomb_fuse_01", base.gameObject);
                    break;
                case 9: //Ice Bomb
                    Bloop(user, 109);
                    SpawnObject((PickupObjectDatabase.GetById(109) as SpawnObjectPlayerItem).objectToSpawn, user, 6f);
                    AkSoundEngine.PostEvent("Play_OBJ_bomb_fuse_01", base.gameObject);
                    break;
                case 10: //Singularity
                    Bloop(user, 155);
                    SpawnObject((PickupObjectDatabase.GetById(155) as SpawnObjectPlayerItem).objectToSpawn, user, 8f);
                    break;
            }
        }
        public void Bloop(PlayerController user, int id)
        {
            user.BloopItemAboveHead(PickupObjectDatabase.GetById(id).sprite, "", Color.white, true);
        }

        public override void OnPreDrop(PlayerController user)
        {
            base.StopAllCoroutines();
            if (user)
            {
                if (isLeadSkinActive) { EndLeadSkin(user); }
            }
            if (this)
            {
                if (isBulletTimeActive) { AkSoundEngine.PostEvent("State_Bullet_Time_off", base.gameObject); BraveTime.ClearMultiplier(base.gameObject); }
            }
            base.OnPreDrop(user);
        }
        public override bool CanBeUsed(PlayerController user)
        {
            if (user.CurrentGun != null) return true;
            return base.CanBeUsed(user);
        }

        public void SpawnObject(GameObject objectToSpawn, PlayerController user, float tossForce = 0f)
        {
            GameObject spawnedObject;
            if (tossForce == 0)
            {
                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(objectToSpawn, user.specRigidbody.UnitCenter, Quaternion.identity);
                spawnedObject = gameObject;
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
            else
            {
                Vector3 vector = user.unadjustedAimPoint - user.LockedApproximateSpriteCenter;
                Vector3 vector2 = user.specRigidbody.UnitCenter;
                if (vector.y > 0f)
                {
                    vector2 += Vector3.up * 0.25f;
                }
                GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(objectToSpawn, vector2, Quaternion.identity);
                tk2dBaseSprite component4 = gameObject2.GetComponent<tk2dBaseSprite>();
                if (component4)
                {
                    component4.PlaceAtPositionByAnchor(vector2, tk2dBaseSprite.Anchor.MiddleCenter);
                }
                spawnedObject = gameObject2;
                Vector2 vector3 = user.unadjustedAimPoint - user.LockedApproximateSpriteCenter;
                vector3 = Quaternion.Euler(0f, 0f, 0f) * vector3;
                DebrisObject debrisObject = LootEngine.DropItemWithoutInstantiating(gameObject2, gameObject2.transform.position, vector3, tossForce, false, false, true, false);
                if (gameObject2.GetComponent<BlackHoleDoer>())
                {
                    debrisObject.PreventFallingInPits = true;
                    debrisObject.PreventAbsorption = true;
                }
                if (vector.y > 0f && debrisObject)
                {
                    debrisObject.additionalHeightBoost = -1f;
                    if (debrisObject.sprite)
                    {
                        debrisObject.sprite.UpdateZDepth();
                    }
                }
                debrisObject.IsAccurateDebris = true;
                debrisObject.Priority = EphemeralObject.EphemeralPriority.Critical;

            }
            if (spawnedObject)
            {
                PortableTurretController component5 = spawnedObject.GetComponent<PortableTurretController>();
                if (component5)
                {
                    component5.sourcePlayer = this.LastOwner;
                }
                Projectile componentInChildren = spawnedObject.GetComponentInChildren<Projectile>();
                if (componentInChildren)
                {
                    componentInChildren.Owner = this.LastOwner;
                    componentInChildren.TreatedAsNonProjectileForChallenge = true;
                }
                SpawnObjectItem componentInChildren2 = spawnedObject.GetComponentInChildren<SpawnObjectItem>();
                if (componentInChildren2)
                {
                    componentInChildren2.SpawningPlayer = this.LastOwner;
                }
            }
        }

        #region Lead Skin
        private bool leadSkinCachedPlayerMaterial;
        private bool isLeadSkinActive = false;
        private IEnumerator HandleShield(PlayerController user)
        {
            float instanceDuration = leadSkinDuration;
            if (user.PlayerHasActiveSynergy("Items Activated")) { instanceDuration *= 2f; }

            isLeadSkinActive = true;

            IsCurrentlyActive = true;
            m_activeElapsed = 0f;
            m_activeDuration = instanceDuration;
            leadSkinCachedPlayerMaterial = user.sprite.usesOverrideMaterial;
            user.sprite.usesOverrideMaterial = true;
            user.SetOverrideShader(ShaderCache.Acquire("Brave/ItemSpecific/MetalSkinShader"));
            user.specRigidbody.OnPreRigidbodyCollision += OnPreCollision;
            user.healthHaver.IsVulnerable = false;

            float elapsed = 0f;
            while (elapsed < instanceDuration)
            {
                elapsed += BraveTime.DeltaTime;
                user.healthHaver.IsVulnerable = false;
                yield return null;
            }
            EndLeadSkin(user);
            yield break;
        }
        private void OnPreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherCollider)
        {
            Projectile component = otherRigidbody.GetComponent<Projectile>();
            if (component != null && !(component.Owner is PlayerController) && this.LastOwner != null)
            {
                PassiveReflectItem.ReflectBullet(component, true, LastOwner, 10f, 1f, 1f, 0f);
                PhysicsEngine.SkipCollision = true;
            }
        }
        private void EndLeadSkin(PlayerController user)
        {
            isLeadSkinActive = false;
            if (user)
            {
                user.healthHaver.IsVulnerable = true;
                user.ClearOverrideShader();
                user.sprite.usesOverrideMaterial = leadSkinCachedPlayerMaterial;
                user.specRigidbody.OnPreRigidbodyCollision -= OnPreCollision;
                IsCurrentlyActive = false;
            }
            if (this)
            {
                AkSoundEngine.PostEvent("Play_OBJ_metalskin_end_01", base.gameObject);
            }
        }
        #endregion

        #region Bullet Time
        public bool isBulletTimeActive;
        private IEnumerator DoBulletTime(PlayerController user)
        {
            isBulletTimeActive = true;
            AkSoundEngine.PostEvent("State_Bullet_Time_on", base.gameObject);

            float instanceDuration = bulletTimeDuration;
            if (user.PlayerHasActiveSynergy("Items Activated")) { instanceDuration *= 2f; }

            IsCurrentlyActive = true;
            m_activeElapsed = 0f;
            m_activeDuration = instanceDuration;

            RadialSlowInterface newSlowterface = new RadialSlowInterface();
            newSlowterface.audioEvent = "";
            newSlowterface.DoesCirclePass = false;
            newSlowterface.DoesSepia = false;
            newSlowterface.EffectRadius = 100;
            newSlowterface.RadialSlowHoldTime = instanceDuration;
            newSlowterface.RadialSlowInTime = 0;
            newSlowterface.RadialSlowOutTime = 0.5f;
            newSlowterface.RadialSlowTimeModifier = 0.3f;
            newSlowterface.UpdatesForNewEnemies = true;

            newSlowterface.DoRadialSlow(user.CenterPosition, user.CurrentRoom);
            float ela = 0f;
            while (ela < instanceDuration)
            {
                ela += GameManager.INVARIANT_DELTA_TIME;
                this.m_activeElapsed = ela;
                yield return null;
            }
            if (this)
            {
                AkSoundEngine.PostEvent("State_Bullet_Time_off", base.gameObject);
            }
            base.IsCurrentlyActive = false;
            isBulletTimeActive = false;
            yield break;
        }
        #endregion

        #region Fortunes Favour
        private IEnumerator DoFortunesFavour(PlayerController user)
        {
            base.IsCurrentlyActive = true;

            float instanceDuration = fortunesFavourDuration;
            if (user.PlayerHasActiveSynergy("Items Activated")) { instanceDuration *= 2f; }


            this.m_activeElapsed = 0f;
            this.m_activeDuration = instanceDuration;
            float innerRadiusSqrDistance = 2f * 2f;
            float outerRadiusSqrDistance = 2.25f * 2.25f;
            float finalRadiusSqrDistance = 3f * 3f;
            float pushStrengthRadians = 720f * 0.0174532924f;
            GameObject sparkOctant = (PickupObjectDatabase.GetById(105) as FortuneFavorItem).sparkOctantVFX;
            List<Projectile> ensnaredProjectiles = new List<Projectile>();
            List<Vector2> initialDirections = new List<Vector2>();
            GameObject[] octantVFX = new GameObject[8];
            while (this.m_activeElapsed < this.m_activeDuration)
            {
                Vector2 playerCenter = user.CenterPosition;
                ReadOnlyCollection<Projectile> allProjectiles = StaticReferenceManager.AllProjectiles;
                for (int i = 0; i < allProjectiles.Count; i++)
                {
                    Projectile projectile = allProjectiles[i];
                    if (projectile.Owner != user && !(projectile.Owner is PlayerController))
                    {
                        Vector2 worldCenter = projectile.sprite.WorldCenter;
                        Vector2 vector = worldCenter - playerCenter;
                        float num = Vector2.SqrMagnitude(vector);
                        if (num < innerRadiusSqrDistance && !ensnaredProjectiles.Contains(projectile))
                        {
                            projectile.RemoveBulletScriptControl();
                            ensnaredProjectiles.Add(projectile);
                            initialDirections.Add(projectile.Direction);
                            int num2 = BraveMathCollege.VectorToOctant(vector);
                            if (octantVFX[num2] == null)
                            {
                                octantVFX[num2] = user.PlayEffectOnActor(sparkOctant, Vector3.zero, true, true, false);
                                octantVFX[num2].transform.rotation = Quaternion.Euler(0f, 0f, (float)(-45 + -45 * num2));
                            }
                        }
                    }
                }
                for (int j = 0; j < ensnaredProjectiles.Count; j++)
                {
                    Projectile projectile2 = ensnaredProjectiles[j];
                    if (!projectile2)
                    {
                        ensnaredProjectiles.RemoveAt(j);
                        initialDirections.RemoveAt(j);
                        j--;
                    }
                    else
                    {
                        Vector2 worldCenter2 = projectile2.sprite.WorldCenter;
                        Vector2 vector2 = playerCenter - worldCenter2;
                        float num3 = Vector2.SqrMagnitude(vector2);
                        if (num3 > finalRadiusSqrDistance)
                        {
                            ensnaredProjectiles.RemoveAt(j);
                            initialDirections.RemoveAt(j);
                            j--;
                        }
                        else if (num3 > outerRadiusSqrDistance)
                        {
                            projectile2.Direction = Vector3.RotateTowards(projectile2.Direction, initialDirections[j], pushStrengthRadians * BraveTime.DeltaTime * 0.5f, 0f).XY().normalized;
                        }
                        else
                        {
                            Vector2 vector3 = vector2 * -1f;
                            float num4 = 1f;
                            if (num3 / innerRadiusSqrDistance < 0.75f)
                            {
                                num4 = 3f;
                            }
                            vector3 = ((vector3.normalized + initialDirections[j].normalized) / 2f).normalized;
                            projectile2.Direction = Vector3.RotateTowards(projectile2.Direction, vector3, pushStrengthRadians * BraveTime.DeltaTime * num4, 0f).XY().normalized;
                        }
                    }
                }
                for (int k = 0; k < 8; k++)
                {
                    if (octantVFX[k] != null && !octantVFX[k])
                    {
                        octantVFX[k] = null;
                    }
                }
                yield return null;
            }
            base.IsCurrentlyActive = false;
            yield break;
        }
        #endregion
    }
}
