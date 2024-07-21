using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;
using SaveAPI;
using Alexandria.Misc;

namespace NevernamedsItems
{

    public class FlayedRevolver : AdvancedGunBehavior
    {


        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Flayed Revolver", "flayedrevolver2");
            Game.Items.Rename("outdated_gun_mods:flayed_revolver", "nn:flayed_revolver");
            gun.gameObject.AddComponent<FlayedRevolver>();
            gun.SetShortDescription("Sinister Bells");
            gun.SetLongDescription("The favoured weapon of the cruel Mine Flayer, Planar lord of rings.\n\n" + "Reloading a full clip allows the bearer to slip beyond the curtain, if only briefly.");

            Alexandria.Assetbundle.GunInt.SetupSprite(gun, Initialisation.gunCollection, "flayedrevolver2_idle_001", 13, "flayedrevolver2_ammonomicon_001");

            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(35) as Gun).gunSwitchGroup;

            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.SetAnimationFPS(gun.reloadAnimation, 12);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(35) as Gun, true, false);

            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.1f;
            gun.SetBarrel(28, 15);
            gun.SetBaseMaxAmmo(250);
            gun.gunClass = GunClass.PISTOL;

            gun.DefaultModule.numberOfShotsInClip = 6;
            gun.DefaultModule.cooldownTime = 0.1f;
            gun.DefaultModule.projectiles[0] = ProjectileSetupUtility.MakeProjectile(35, 9.9f);
            gun.AddShellCasing(0, 0, 6, 1);

            gun.quality = PickupObject.ItemQuality.B;
            ETGMod.Databases.Items.Add(gun, false, "ANY");
            FlayedRevolverID = gun.PickupObjectId;

            gun.SetupUnlockOnCustomStat(CustomTrackedStats.MINEFLAYER_KILLS, 9, DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN);

            dummyCenter = new GameObject();
            dummyCenter.MakeFakePrefab();
            SpeculativeRigidbody orAddComponent = dummyCenter.GetOrAddComponent<SpeculativeRigidbody>();
            PixelCollider pixelCollider = new PixelCollider();
            pixelCollider.ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual;
            pixelCollider.CollisionLayer = CollisionLayer.EnemyCollider;
            pixelCollider.ManualWidth = 1;
            pixelCollider.ManualHeight = 1;
            pixelCollider.ManualOffsetX = 0;
            pixelCollider.ManualOffsetY = 0;
            orAddComponent.PixelColliders = new List<PixelCollider>
            {
                pixelCollider
            };
            orAddComponent.CollideWithOthers = false;
        }
        public static GameObject dummyCenter;
        public static int FlayedRevolverID;
        public override void OnReloadPressedSafe(PlayerController player, Gun gun, bool manualReload)
        {
            if ((gun.ClipCapacity == gun.ClipShotsRemaining) || (gun.CurrentAmmo == gun.ClipShotsRemaining))
            {
                if (gun.CurrentAmmo >= 5)
                {
                    if (this.m_extantReticleQuad) //If the cursor is there, then do the teleport
                    {
                        gun.CurrentAmmo -= 5;
                        Vector2 worldCenter = this.m_extantReticleQuad.WorldCenter;
                        UnityEngine.Object.Destroy(this.m_extantReticleQuad.gameObject);
                        worldCenter += new Vector2(1.5f, 1.5f);
                        //worldCenter = new Vector2(Mathf.FloorToInt(worldCenter.x), Mathf.FloorToInt(worldCenter.y));
                        // UnityEngine.Object.Instantiate<GameObject>((PickupObjectDatabase.GetById(Pencil.pencilID) as Gun).DefaultModule.projectiles[0].gameObject, new Vector3(worldCenter.x, worldCenter.y), Quaternion.identity);
                        if (player.PlayerHasActiveSynergy("Lord of Rings"))
                        {
                            DoRing(player.CenterPosition, 6, player);
                        }

                        TeleportPlayerToCursorPosition.StartTeleport(player, worldCenter);
                    }
                    else //If the cursor is not there, make it
                    {
                        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.reticleQuad);
                        this.m_extantReticleQuad = gameObject.GetComponent<tk2dBaseSprite>();
                        this.m_currentAngle = BraveMathCollege.Atan2Degrees(player.unadjustedAimPoint.XY() - player.CenterPosition);
                        this.m_currentDistance = 5f;
                        this.UpdateReticlePosition();
                    }
                }
            }
        }
        public void DoRing(Vector2 v, float radius, PlayerController owner)
        {
            GameObject center = UnityEngine.Object.Instantiate(dummyCenter, v, Quaternion.identity);
            SpeculativeRigidbody centerBody = center.GetComponent<SpeculativeRigidbody>();

            int localRotation = 0;
            for (int i = 0; i < 6; i++)
            {
                GameObject orbital = gun.DefaultModule.projectiles[0].InstantiateAndFireInDirection(v, 0);
                Projectile component = orbital.GetComponent<Projectile>();
                if (component != null)
                {
                    component.Owner = Owner;
                    component.Shooter = Owner.specRigidbody;
                    NoCollideBehaviour nocol = orbital.AddComponent<NoCollideBehaviour>();
                    nocol.worksOnEnemies = false;
                    component.specRigidbody.CollideWithTileMap = false;
                    component.pierceMinorBreakables = true;
                    component.baseData.range *= 2f;
                    component.transform.localRotation = Quaternion.Euler(0f, 0f, component.transform.localRotation.z + localRotation);
                    localRotation += 60;

                    component.ScaleByPlayerStats(owner);
                    owner.DoPostProcessProjectile(component);
                    component.shouldRotate = true;

                    OrbitProjectileMotionModule orbitProjectileMotionModule = new OrbitProjectileMotionModule();

                    orbitProjectileMotionModule.lifespan = 50;
                    orbitProjectileMotionModule.MinRadius = 0.1f;
                    orbitProjectileMotionModule.MaxRadius = 0.1f;
                    orbitProjectileMotionModule.usesAlternateOrbitTarget = true;
                    orbitProjectileMotionModule.OrbitGroup = -6;
                    orbitProjectileMotionModule.alternateOrbitTarget = centerBody;
                    component.OverrideMotionModule = orbitProjectileMotionModule;

                    component.StartCoroutine(LerpToMaxRadius(component, radius));
                }
            }
        }
        private IEnumerator LerpToMaxRadius(Projectile proj, float radius)
        {
            if (!proj || proj.OverrideMotionModule == null) yield break;
            if (proj.OverrideMotionModule is OrbitProjectileMotionModule)
            {
                OrbitProjectileMotionModule motionMod = proj.OverrideMotionModule as OrbitProjectileMotionModule;

                float elapsed = 0f;
                float duration = 1f;
                while (elapsed < duration)
                {
                    elapsed += proj.LocalDeltaTime;
                    float t = elapsed / duration;
                    float currentRadius = Mathf.Lerp(0.1f, radius, t);

                    motionMod.m_radius = currentRadius;
                    yield return null;
                }
            }
            yield break;
        }
        protected override void Update()
        {
            if (this.m_extantReticleQuad)
            {
                this.UpdateReticlePosition();
            }
            base.Update();
        }
        public override void OnSwitchedAwayFromThisGun()
        {
            if (this.m_extantReticleQuad)
            {
                UnityEngine.Object.Destroy(this.m_extantReticleQuad.gameObject);
            }
            base.OnSwitchedAwayFromThisGun();
        }
        public override void OnPostFired(PlayerController player, Gun gun)
        {
            if (this.m_extantReticleQuad)
            {
                UnityEngine.Object.Destroy(this.m_extantReticleQuad.gameObject);
            }
            base.OnPostFired(player, gun);
        }
        private void UpdateReticlePosition()
        {
            PlayerController user = this.gun.GunPlayerOwner();
            if (user)
            {
                if (BraveInput.GetInstanceForPlayer(user.PlayerIDX).IsKeyboardAndMouse(false))
                {
                    Vector2 vector = user.unadjustedAimPoint.XY();
                    Vector2 vector2 = vector - this.m_extantReticleQuad.GetBounds().extents.XY();
                    this.m_extantReticleQuad.transform.position = vector2;
                }
                else
                {
                    BraveInput instanceForPlayer = BraveInput.GetInstanceForPlayer(user.PlayerIDX);
                    Vector2 vector3 = user.CenterPosition + (Quaternion.Euler(0f, 0f, this.m_currentAngle) * Vector2.right).XY() * this.m_currentDistance;
                    vector3 += instanceForPlayer.ActiveActions.Aim.Vector * 12f * BraveTime.DeltaTime;
                    this.m_currentAngle = BraveMathCollege.Atan2Degrees(vector3 - user.CenterPosition);
                    this.m_currentDistance = Vector2.Distance(vector3, user.CenterPosition);
                    this.m_currentDistance = Mathf.Min(this.m_currentDistance, 100);
                    vector3 = user.CenterPosition + (Quaternion.Euler(0f, 0f, this.m_currentAngle) * Vector2.right).XY() * this.m_currentDistance;
                    Vector2 vector4 = vector3 - this.m_extantReticleQuad.GetBounds().extents.XY();
                    this.m_extantReticleQuad.transform.position = vector4;
                }
            }
        }
        public override void OnDestroy()
        {
            if (this.m_extantReticleQuad)
            {
                UnityEngine.Object.Destroy(this.m_extantReticleQuad.gameObject);
            }
            base.OnDestroy();
        }
        public override void OnDropped()
        {
            if (this.m_extantReticleQuad)
            {
                UnityEngine.Object.Destroy(this.m_extantReticleQuad.gameObject);
            }
            base.OnDropped();
        }
        public FlayedRevolver()
        {
            reticleQuad = (PickupObjectDatabase.GetById(443) as TargetedAttackPlayerItem).reticleQuad;
        }
        public GameObject reticleQuad;
        private tk2dBaseSprite m_extantReticleQuad;
        private float m_currentAngle;
        private float m_currentDistance = 5f;
    }
}
