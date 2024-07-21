using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Alexandria.ItemAPI;
using Alexandria.Misc;
using UnityEngine;
using MonoMod.RuntimeDetour;
using System.Reflection;
using HarmonyLib;

namespace NevernamedsItems
{
    public class Dreamcatcher : PassiveItem
    {
        public static void Init()
        {
            PassiveItem item = ItemSetup.NewItem<Dreamcatcher>(
              "Dreamcatcher",
              "Close Your Eyes",
              "Catches spent shells ejected from the owners guns and flings them at nearby foes. Chance to also catch nearby bullets.\n\nYoung Bullet Kin sometimes hang these spiritual ornaments above their sleeping chambers to catch bad dreams and stray bullets.",
              "dreamcatcher_icon") as PassiveItem;
            item.quality = PickupObject.ItemQuality.B;
            ID = item.PickupObjectId;

            vfx = VFXToolbox.CreateVFXBundle("TinyShellImpact", new IntVector2(9, 11), tk2dBaseSprite.Anchor.MiddleCenter, true, 5f);

        }

        public override void Update()
        {
            if (Owner != null)
            {
                if (timer <= 0)
                {
                    List<Projectile> valid = new List<Projectile>();
                    for (int i = 0; i < StaticReferenceManager.AllProjectiles.Count; i++)
                    {
                        Projectile projectile = StaticReferenceManager.AllProjectiles[i];
                        if (projectile && (projectile.Owner == null || !(projectile.Owner is PlayerController)))
                        {
                            if (Vector2.Distance(projectile.LastPosition, Owner.CenterPosition) <= 3f) { CatchProjectile(projectile); }
                        }
                    }

                    timer = UnityEngine.Random.Range(0.25f, 5f);
                }
                else { timer -= BraveTime.DeltaTime; }
            }
            base.Update();
        }
        public void CatchProjectile(Projectile p)
        {
            if (!base.Owner) { return; }

            p.RemoveBulletScriptControl();
            if (p.Owner && p.Owner.specRigidbody) p.specRigidbody.DeregisterSpecificCollisionException(p.Owner.specRigidbody);

            p.Owner = base.Owner;
            p.SetNewShooter(base.Owner.specRigidbody);
            p.allowSelfShooting = false;
            p.collidesWithPlayer = false;
            p.collidesWithEnemies = true;

            p.UpdateCollisionMask();
            p.Reflected();

            SlowDownOverTimeModifier slower = p.gameObject.AddComponent<SlowDownOverTimeModifier>();
            slower.timeToSlowOver = 0.5f;
            slower.targetSpeed = 0;
            slower.doRandomTimeMultiplier = true;
            p.gameObject.AddComponent<DreamcatcherProjectile>();
            p.RemoveFromPool();
            p.ChangeColor(0.5f, ExtendedColours.honeyYellow);
        }
        float timer = 0;
        public static int ID;
        public static GameObject vfx;
        [HarmonyPatch(typeof(Gun))]
        [HarmonyPatch("SpawnShellCasingAtPosition", MethodType.Normal)]
        public class CasingSpawner
        {
            [HarmonyPrefix]
            public static bool HarmonyPostfix(Gun __instance, Vector3 position)
            {
                if (__instance != null && __instance.GunPlayerOwner() != null && __instance.GunPlayerOwner().IsInCombat && __instance.GunPlayerOwner().HasPickupID(Dreamcatcher.ID) && __instance.shellCasing !=  null)
                {
                    GameObject gameObject = SpawnManager.SpawnDebris(__instance.shellCasing, position.WithZ(__instance.m_transform.position.z), Quaternion.Euler(0f, 0f, __instance.gunAngle));

                    DebrisObject component2 = gameObject.GetComponent<DebrisObject>();
                    if (component2 != null) { UnityEngine.Object.Destroy(component2); }

                    SpeculativeRigidbody orAddComponent = gameObject.GetOrAddComponent<SpeculativeRigidbody>();
                    PixelCollider pixelCollider = new PixelCollider();
                    pixelCollider.ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual;
                    pixelCollider.CollisionLayer = CollisionLayer.EnemyCollider;
                    pixelCollider.ManualWidth = 3;
                    pixelCollider.ManualHeight = 3;
                    pixelCollider.ManualOffsetX = 0;
                    pixelCollider.ManualOffsetY = 0;
                    orAddComponent.PixelColliders = new List<PixelCollider> { pixelCollider };

                    Projectile projectile = gameObject.GetOrAddComponent<Projectile>();
                    projectile.Shooter = __instance.GunPlayerOwner().specRigidbody;
                    projectile.Owner = __instance.GunPlayerOwner().gameActor;
                    projectile.baseData.damage = 2.5f;
                    projectile.baseData.range = 1000f;
                    projectile.baseData.speed = 5f;
                    projectile.collidesWithProjectiles = false;
                    projectile.shouldRotate = false;
                    projectile.baseData.force = 10f;
                    projectile.specRigidbody.CollideWithTileMap = true;
                    projectile.specRigidbody.Reinitialize();
                    projectile.specRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.Projectile;
                    projectile.Start();
                    projectile.UpdateCollisionMask();

                    if (projectile.hitEffects == null) { projectile.hitEffects = new ProjectileImpactVFXPool(); }
                    projectile.hitEffects.overrideMidairDeathVFX = Dreamcatcher.vfx;
                    projectile.hitEffects.alwaysUseMidair = true;


                    SlowDownOverTimeModifier slower = gameObject.AddComponent<SlowDownOverTimeModifier>();
                    slower.timeToSlowOver = 0.5f;
                    slower.targetSpeed = 0;
                    slower.doRandomTimeMultiplier = true;

                    gameObject.AddComponent<ProjectileSpinner>();
                    gameObject.AddComponent<DreamcatcherProjectile>();

                    if ((__instance.PickupObjectId == 145 || __instance.PickupObjectId == 385) && __instance.GunPlayerOwner().PlayerHasActiveSynergy("Spellcatcher"))
                    {
                        AdvancedTransmogrifyBehaviour transmog = projectile.gameObject.GetOrAddComponent<AdvancedTransmogrifyBehaviour>();
                        transmog.TransmogDataList.Add(new AdvancedTransmogrifyBehaviour.TransmogData()
                        {
                            identifier = "Dreamcatcher",
                            TargetGuids = new List<string>() { "4254a93fc3c84c0dbe0a8f0dddf48a5a" },
                            maintainHPPercent = false,
                            TransmogChance = __instance.PickupObjectId == 145 ? 0.2f : 1f,
                        }) ;
                    }

                    projectile.SendInDirection((__instance.gunAngle + 180f).DegreeToVector2(), false, false);

                    return false;
                }
                else
                {
                    return true;
                }
            }


        }
        public class DreamcatcherProjectile : MonoBehaviour
        {
            private void Start()
            {
                base.gameObject.GetOrAddComponent<SlowDownOverTimeModifier>().OnCompleteStop += OnStop;
            }
            public void OnStop(Projectile proj)
            {
                base.StartCoroutine(HandleCasingProjectile(proj));
            }
            public IEnumerator HandleCasingProjectile(Projectile proj)
            {
                float time = 0.5f;
                time *= (UnityEngine.Random.value * 2);
                yield return new WaitForSeconds(time);
                Vector2 dirVec = proj.GetVectorToNearestEnemy();
                AkSoundEngine.PostEvent("Play_ENM_pop_shot_01", proj.gameObject);
                proj.baseData.speed = 23f;
                proj.UpdateSpeed();
                if (dirVec != Vector2.zero)
                {
                    proj.SendInDirection(dirVec, false, false);
                }
                else
                {
                    proj.DieInAir();
                }
                yield break;
            }
        }

    }
}

