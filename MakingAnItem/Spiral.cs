using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using ItemAPI;

namespace NevernamedsItems
{

    public class Spiral : GunBehaviour
    {


        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Spiral", "spiral");
            Game.Items.Rename("outdated_gun_mods:spiral", "nn:spiral");
            gun.gameObject.AddComponent<Spiral>();
            gun.SetShortDescription("Come Join Me...");
            gun.SetLongDescription("Forever. Inescapable. Beautiful."+"\n\nAll will become a part of the Spiral.");

            gun.SetupSprite(null, "spiral_idle_001", 16);

            gun.SetAnimationFPS(gun.shootAnimation, 30);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 2f;
            gun.DefaultModule.cooldownTime = 1f;
            gun.DefaultModule.numberOfShotsInClip = 1;
            gun.barrelOffset.transform.localPosition = new Vector3(1.12f, 0.62f, 0f);
            gun.SetBaseMaxAmmo(150);

            //BULLET STATS
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage *= 5f;
            projectile.baseData.speed *= 0.1f;
            projectile.pierceMinorBreakables = true;
            SpiralHandler spiralBehaviour = projectile.gameObject.AddComponent<SpiralHandler>();
            spiralBehaviour.projectileToSpawn = ((Gun)ETGMod.Databases.Items["38_special"]).DefaultModule.projectiles[0];
            projectile.SetProjectileSpriteRight("spiral_projectile", 15, 15, true, tk2dBaseSprite.Anchor.MiddleCenter, 14, 14);


            //projectile.shouldRotate = true;
            projectile.transform.parent = gun.barrelOffset;

            gun.quality = PickupObject.ItemQuality.A; //
            gun.encounterTrackable.EncounterGuid = "this is the Spiral";
            ETGMod.Databases.Items.Add(gun, null, "ANY");

            gun.AddToSubShop(ItemBuilder.ShopType.Cursula);
            SpiralID = gun.PickupObjectId;
        }
        public static int SpiralID;
        public Spiral()
        {

        }
    }
    public class SpiralHandler : MonoBehaviour
    {
        public SpiralHandler()
        {
            this.projectileToSpawn = null;
        }

        // Token: 0x06007294 RID: 29332 RVA: 0x002CA328 File Offset: 0x002C8528
        private void Awake()
        {
            this.m_projectile = base.GetComponent<Projectile>();
            this.speculativeRigidBoy = base.GetComponent<SpeculativeRigidbody>();
        }

        private float spawnAngle = 90f;
        // Token: 0x06007295 RID: 29333 RVA: 0x002CA3A0 File Offset: 0x002C85A0
        private void Update()
        {
            if (this.m_projectile == null)
            {
                this.m_projectile = base.GetComponent<Projectile>();
            }
            if (this.speculativeRigidBoy == null)
            {
                this.speculativeRigidBoy = base.GetComponent<SpeculativeRigidbody>();
            }
            this.elapsed += BraveTime.DeltaTime;
            if (this.elapsed > 0.02f)
            {                
                    this.SpawnProjectile(this.projectileToSpawn, m_projectile.sprite.WorldCenter, m_projectile.transform.eulerAngles.z + spawnAngle, null);
                    spawnAngle += 10;
                this.elapsed = 0;
            }
        }
        private void SpawnProjectile(Projectile proj, Vector3 spawnPosition, float zRotation, SpeculativeRigidbody collidedRigidbody = null)
        {
            GameObject gameObject = SpawnManager.SpawnProjectile(proj.gameObject, spawnPosition, Quaternion.Euler(0f, 0f, zRotation), true);
            Projectile component = gameObject.GetComponent<Projectile>();
            if (component)
            {
                component.SpawnedFromOtherPlayerProjectile = true;
                component.TreatedAsNonProjectileForChallenge = true;
                PlayerController playerController = this.m_projectile.Owner as PlayerController;
                component.baseData.damage *= playerController.stats.GetStatValue(PlayerStats.StatType.Damage);
                component.baseData.speed *= playerController.stats.GetStatValue(PlayerStats.StatType.ProjectileSpeed);
                playerController.DoPostProcessProjectile(component);
            }
        }

        private Projectile m_projectile;
        private SpeculativeRigidbody speculativeRigidBoy;
        public Projectile projectileToSpawn;
        private float elapsed;
    }
}