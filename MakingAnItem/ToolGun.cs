using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using ItemAPI;
using MonoMod.RuntimeDetour;
using System.Reflection;

namespace NevernamedsItems
{
    public class ToolGun : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Tool Gun", "toolgun");
            Game.Items.Rename("outdated_gun_mods:tool_gun", "nn:tool_gun");
            gun.gameObject.AddComponent<ToolGun>();
            gun.SetShortDescription("Fires Ammunition");
            gun.SetLongDescription("Becomes more powerful the more times it's ammo is refilled." + "\n\nThis gun is comically stuffed with whole ammo boxes.");

            gun.SetupSprite(null, "toolgun_idle_001", 8);

            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(153) as Gun).gunSwitchGroup;
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0.8f;
            gun.DefaultModule.cooldownTime = 0.11f;
            gun.DefaultModule.numberOfShotsInClip = 10;
            gun.barrelOffset.transform.localPosition = new Vector3(2.43f, 0.75f, 0f);
            gun.SetBaseMaxAmmo(50);
            gun.ammo = 50;


            //BULLET STATS
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage *= 500f;
            projectile.baseData.speed *= 1f;
            projectile.baseData.range *= 2f;
            projectile.AdditionalScaleMultiplier *= 0.3f;

            gun.quality = PickupObject.ItemQuality.EXCLUDED; //S
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            ToolGunID = gun.PickupObjectId;
        }

        public static int ToolGunID;
        public ToolGun()
        {

        }
    }
    public class ToolGunHandler : MonoBehaviour
    {
        public ToolGunHandler()
        {
            this.projectileToSpawn = null;
        }

        // Token: 0x06007294 RID: 29332 RVA: 0x002CA328 File Offset: 0x002C8528
        private void Awake()
        {
            this.m_projectile = base.GetComponent<Projectile>();
            this.speculativeRigidBoy = base.GetComponent<SpeculativeRigidbody>();
        }
        private void Start()
        {
            int modifierToGive = UnityEngine.Random.Range(1, 6);
            switch (modifierToGive)
            {
                case 0: //Colour
                    break;
                case 1: //Increase Size
                    break; 
                case 2: //Decrease Size
                    break;
                case 3: //Apply Status Effect
                    break;
                case 4: //Spawn Enemy
                    break;
                case 5: //Delete Enemy
                    m_projectile.OnHitEnemy += this.DeleteEnemy;
                    break;
            }
        }
        private void DeleteEnemy(Projectile bullet, SpeculativeRigidbody enemy, bool fatal)
        {
            if (enemy != null && enemy.aiActor != null && enemy.healthHaver != null && enemy.gameObject != null && enemy.aiActor.EnemyGuid != null)
            {
                if (!enemy.healthHaver.IsBoss || EasyEnemyTypeLists.MiniBosses.Contains(enemy.aiActor.EnemyGuid))
                {
                    enemy.aiActor.EraseFromExistenceWithRewards(false);
                }
            }
        }

        private Projectile m_projectile;
        private SpeculativeRigidbody speculativeRigidBoy;
        public Projectile projectileToSpawn;
        private float elapsed;
    }
}
