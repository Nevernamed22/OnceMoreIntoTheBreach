using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.Assetbundle;
using Alexandria.Misc;

namespace NevernamedsItems
{

    public class Dimensionaliser : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Dimensionaliser", "dimensionaliser");
            Game.Items.Rename("outdated_gun_mods:dimensionaliser", "nn:dimensionaliser");
            gun.gameObject.AddComponent<Dimensionaliser>();
            gun.SetShortDescription("The Multiverse!");
            gun.SetLongDescription("Opens portals to random segments of the multiverse, letting kaliber-knows-what through."+"\n\n");

            gun.SetGunSprites("dimensionaliser");

            gun.SetAnimationFPS(gun.shootAnimation, 14);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(56) as Gun, true, false);

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1f;
            gun.DefaultModule.cooldownTime = 2f;
            gun.DefaultModule.angleVariance = 7;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(89) as Gun).muzzleFlashEffects;
            gun.DefaultModule.numberOfShotsInClip = 2;
            gun.barrelOffset.transform.localPosition = new Vector3(0.81f, 0.43f, 0f);
            gun.SetBaseMaxAmmo(100);
            gun.ammo = 100;
            gun.gunClass = GunClass.SILLY;
            Projectile portalSubProj = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            portalSubProj.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(portalSubProj.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(portalSubProj);
            portalSubProj.baseData.damage = 5f;
            portalSubProj.baseData.speed *= 1f;
            portalSubProj.hitEffects.overrideMidairDeathVFX = SharedVFX.GreenLaserCircleVFX;
            portalSubProj.hitEffects.alwaysUseMidair = true;
            portalSubProj.SetProjectileSprite("dimensionaliser_projectile", 12, 7, true, tk2dBaseSprite.Anchor.MiddleCenter, 10, 5);

            Projectile portal = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0]);
            portal.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(portal.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(portal);
            portal.baseData.damage = 0f;
            portal.baseData.speed = 0f;
            portal.specRigidbody.CollideWithTileMap = false;
            portal.pierceMinorBreakables = true;
            DimensionaliserPortal portalComp = portal.gameObject.GetOrAddComponent<DimensionaliserPortal>();
            portalComp.subProj = portalSubProj;
            NoCollideBehaviour noCollide = portal.gameObject.GetOrAddComponent<NoCollideBehaviour>();
            noCollide.worksOnEnemies = true;
            noCollide.worksOnProjectiles = true;
            portal.hitEffects.overrideMidairDeathVFX = SharedVFX.GreenLaserCircleVFX;
            portal.hitEffects.alwaysUseMidair = true;

            portal.AnimateProjectileBundle("DimensionaliserPortalIdle", 
                Initialisation.ProjectileCollection, 
                Initialisation.projectileAnimationCollection,
                "DimensionaliserPortalIdle",
            MiscTools.DupeList(new IntVector2(55, 55), 90), 
            MiscTools.DupeList(true, 90),
            MiscTools.DupeList(tk2dBaseSprite.Anchor.MiddleCenter, 90),
            MiscTools.DupeList(true, 90),
            MiscTools.DupeList(false, 90),
            MiscTools.DupeList<Vector3?>(null, 90),
            MiscTools.DupeList<IntVector2?>(new IntVector2(30, 30), 90), //Override colliders
            MiscTools.DupeList<IntVector2?>(null, 90),
            MiscTools.DupeList<Projectile>(null, 90));




            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            projectile.baseData.damage = 1f;
            projectile.baseData.speed *= 3f;
            projectile.hitEffects.overrideMidairDeathVFX = SharedVFX.GreenLaserCircleVFX;
            projectile.hitEffects.alwaysUseMidair = true;
            DimensionaliserProjectile dimensionaliserProj = projectile.gameObject.GetOrAddComponent<DimensionaliserProjectile>();
            dimensionaliserProj.portalPrefab = portal.gameObject;
            projectile.SetProjectileSprite("dimensionaliser_projectile", 12, 7, true, tk2dBaseSprite.Anchor.MiddleCenter, 10, 5);
            gun.DefaultModule.projectiles[0] = projectile;

            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = CustomClipAmmoTypeToolbox.AddCustomAmmoType("Dimensionaliser Bullets", "NevernamedsItems/Resources/CustomGunAmmoTypes/dimensionaliser_clipfull", "NevernamedsItems/Resources/CustomGunAmmoTypes/dimensionaliser_clipempty");

            gun.quality = PickupObject.ItemQuality.S;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            DimensionaliserID = gun.PickupObjectId;

        }
        public static int DimensionaliserID;
        public Dimensionaliser()
        {

        }
    }
    public class DimensionaliserProjectile : MonoBehaviour
    {
        public DimensionaliserProjectile()
        {

        }
        private void Start()
        {
            self = base.GetComponent<Projectile>();
            if (self)
            {
                self.OnDestruction += this.OnDeath;
            }
        }
        private void OnDeath(Projectile me)
        {
            GameObject portal = SpawnManager.SpawnProjectile(portalPrefab, (self.specRigidbody.UnitCenter), Quaternion.identity);
            Projectile portalProjectile = portal.GetComponent<Projectile>();
            if (portalProjectile)
            {
                CorrectForWalls(portalProjectile);
                portalProjectile.Owner = me.Owner;
                portalProjectile.Shooter = me.Shooter;
                if (me.ProjectilePlayerOwner()) me.ProjectilePlayerOwner().DoPostProcessProjectile(portalProjectile);
            }
        }
        private void CorrectForWalls(Projectile portal)
        {
            bool flag = PhysicsEngine.Instance.OverlapCast(portal.specRigidbody, null, true, false, null, null, false, null, null, new SpeculativeRigidbody[0]);
            if (flag)
            {
                Vector2 vector = base.transform.position.XY();
                IntVector2[] cardinalsAndOrdinals = IntVector2.CardinalsAndOrdinals;
                int num = 0;
                int num2 = 1;
                for (; ; )
                {
                    for (int i = 0; i < cardinalsAndOrdinals.Length; i++)
                    {
                        base.transform.position = vector + PhysicsEngine.PixelToUnit(cardinalsAndOrdinals[i] * num2);
                        portal.specRigidbody.Reinitialize();
                        if (!PhysicsEngine.Instance.OverlapCast(portal.specRigidbody, null, true, false, null, null, false, null, null, new SpeculativeRigidbody[0]))
                        {
                            return;
                        }
                    }
                    num2++;
                    num++;
                    if (num > 200)
                    {
                        goto Block_4;
                    }
                }
                //return;
            Block_4:
                Debug.LogError("FREEZE AVERTED!  TELL RUBEL!  (you're welcome) 147");
                return;
            }
        }
        private Projectile self;
        public GameObject portalPrefab;
    }
    public class DimensionaliserPortal : MonoBehaviour
    {
        public DimensionaliserPortal()
        {

        }
        private void Start()
        {
            self = base.GetComponent<Projectile>();
            if (self && self.ProjectilePlayerOwner())
            {
                int selection = UnityEngine.Random.Range(1, 8);
                switch (selection)
                {
                    case 1:
                        StartCoroutine(HegemonyPlatoon());
                        break;
                    case 2:
                        StartCoroutine(RingBullets());
                        break;
                    case 3:
                        StartCoroutine(DelayedBlackHole());
                        break;
                    case 4:
                        StartCoroutine(HentaiTime());
                        break;
                    case 5:
                        StartCoroutine(FireStorm());
                        break;
                    case 6:
                        StartCoroutine(BlankyBlanky());
                        break;
                    case 7:
                        StartCoroutine(BatAttack());
                        break;
                }
            }
        }
        private IEnumerator HegemonyPlatoon()
        {
            yield return new WaitForSeconds(1);
            for (int i = 0; i < 3; i++)
            {
                if (self.ProjectilePlayerOwner().IsInCombat) CompanionisedEnemyUtility.SpawnCompanionisedEnemy(self.ProjectilePlayerOwner(), "556e9f2a10f9411cb9dbfd61e0e0f1e1", self.specRigidbody.UnitCenter.ToIntVector2(), false, Color.red, 5, 2, false, true);
                yield return new WaitForSeconds(0.8f);
            }
            Die();
            yield break;
        }
        private IEnumerator RingBullets()
        {
            yield return new WaitForSeconds(1);
            for (int i = 0; i < 7; i++)
            {
                float degrees = 0;
                for (int i2 = 0; i2 < 15; i2++)
                {
                    SpawnBullets(self.specRigidbody.UnitCenter, degrees);
                    degrees += 24;
                }
                yield return new WaitForSeconds(1);
            }
            Die();
            yield break;
        }
        private IEnumerator DelayedBlackHole()
        {
            yield return new WaitForSeconds(12);
            Projectile projectile2 = ((Gun)ETGMod.Databases.Items["black_hole_gun"]).DefaultModule.projectiles[0];
            GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, self.sprite.WorldCenter, Quaternion.Euler(0, 0, UnityEngine.Random.Range(1, 360)), true);
            Projectile component = gameObject.GetComponent<Projectile>();
            if (component != null)
            {
                component.Owner = self.Owner;
                component.Shooter = self.Owner.specRigidbody;
                component.baseData.speed *= 0.5f;
            }
            Die();
            yield break;
        }
        private IEnumerator HentaiTime()
        {
            yield return new WaitForSeconds(1);
            BeamAPI.FreeFireBeamFromAnywhere((PickupObjectDatabase.GetById(474) as Gun).DefaultModule.projectiles[0], self.ProjectilePlayerOwner(), self.gameObject, Vector2.zero,  90, 5, true);
            BeamAPI.FreeFireBeamFromAnywhere((PickupObjectDatabase.GetById(474) as Gun).DefaultModule.projectiles[0], self.ProjectilePlayerOwner(), self.gameObject, Vector2.zero,  -45, 5, true);
            BeamAPI.FreeFireBeamFromAnywhere((PickupObjectDatabase.GetById(474) as Gun).DefaultModule.projectiles[0], self.ProjectilePlayerOwner(), self.gameObject, Vector2.zero,  -135, 5, true);
            yield return new WaitForSeconds(6);
            Die();
            yield break;
        }
        private IEnumerator FireStorm()
        {
            yield return new WaitForSeconds(1);
            var ddgm = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(EasyGoopDefinitions.FireDef);
            ddgm.AddGoopCircle(self.sprite.WorldCenter, 5);
            for (int i = 0; i < 40; i++)
            {
                yield return new WaitForSeconds(0.1f);
                GameObject gameObject = SpawnManager.SpawnProjectile((PickupObjectDatabase.GetById(336) as Gun).DefaultModule.projectiles[0].gameObject, self.specRigidbody.UnitCenter, Quaternion.Euler(0, 0, UnityEngine.Random.Range(1, 360)), true);
                Projectile component = gameObject.GetComponent<Projectile>();
                if (component != null)
                {
                    component.Owner = self.Owner;
                    component.Shooter = self.Owner.specRigidbody;
                }
            }
            Die();
            yield break;
        }
        private IEnumerator BlankyBlanky()
        {
            yield return new WaitForSeconds(1);
            for (int i = 0; i < 3; i++)
            {
                self.ProjectilePlayerOwner().DoEasyBlank(self.specRigidbody.UnitCenter, EasyBlankType.MINI);
                yield return new WaitForSeconds(3);
            }
            Die();
            yield break;
        }
        private IEnumerator BatAttack()
        {
            yield return new WaitForSeconds(1);
            for (int i = 0; i < 10; i++)
            {
                if (self.ProjectilePlayerOwner().IsInCombat) CompanionisedEnemyUtility.SpawnCompanionisedEnemy(self.ProjectilePlayerOwner(), BraveUtility.RandomElement(AlexandriaTags.GetAllEnemyGuidsWithTag("small_bullat")), self.specRigidbody.UnitCenter.ToIntVector2(), false, Color.red, 7, 2, false, false);
                yield return new WaitForSeconds(0.1f);
            }
            Die();
            yield break;
        }
        private void SpawnBullets(Vector2 pos, float rot)
        {
            GameObject gameObject = SpawnManager.SpawnProjectile(subProj.gameObject, pos, Quaternion.Euler(0, 0, rot), true);
            Projectile component = gameObject.GetComponent<Projectile>();
            if (component != null)
            {
                component.Owner = self.Owner;
                component.Shooter = self.Owner.specRigidbody;
            }
        }
        private void Die()
        {
            self.DieInAir(false, true, true, false);
        }
        private Projectile self;
        public Projectile subProj;
    }
}