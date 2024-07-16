using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.Misc;

namespace NevernamedsItems
{

    public class MamaGun : GunBehaviour
    {


        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Mama", "mama2");
            Game.Items.Rename("outdated_gun_mods:mama", "nn:mama");
            gun.gameObject.AddComponent<MamaGun>();
            gun.SetShortDescription("Just killed a man...");
            gun.SetLongDescription("Heavy with symbolism, this gun was brought to the Gungeon by a poor boy with many regrets." + "\n\nDoes significantly more damage when placed right against an enemy, and fired at point-blank range.");

            Alexandria.Assetbundle.GunInt.SetupSprite(gun, Initialisation.gunCollection, "mama2_idle_001", 8, "mama2_ammonomicon_001");

            gun.SetAnimationFPS(gun.shootAnimation, 15);
            gun.SetAnimationFPS(gun.reloadAnimation, 0);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(56) as Gun, true, false);
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(38) as Gun).gunSwitchGroup;

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1f;
            gun.DefaultModule.cooldownTime = 0.1f;
            gun.DefaultModule.numberOfShotsInClip = 15;
            gun.barrelOffset.transform.localPosition = new Vector3(22f / 16f, 13f / 16f, 0f);
            gun.SetBaseMaxAmmo(200);
            gun.gunClass = GunClass.PISTOL;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(80) as Gun).muzzleFlashEffects;

            //BULLET STATS
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage *= 1.5f;
            projectile.baseData.speed *= 1f;

            PutAGunAgainstHisHeadPulledMyTriggerNowHesDeadBehaviour WillYouDoTheFandango = projectile.gameObject.AddComponent<PutAGunAgainstHisHeadPulledMyTriggerNowHesDeadBehaviour>();

            gun.AddShellCasing(1, 1, 0, 0, "shell_bigbeige");
            gun.AddClipDebris( 0, 1, "clipdebris_mama");


            gun.quality = PickupObject.ItemQuality.B;
            ETGMod.Databases.Items.Add(gun, null, "ANY");

        }
    }
    public class PutAGunAgainstHisHeadPulledMyTriggerNowHesDeadBehaviour : MonoBehaviour
    {
        public PutAGunAgainstHisHeadPulledMyTriggerNowHesDeadBehaviour()
        {
        }
        private void Start()
        {
            this.m_projectile = base.GetComponent<Projectile>();
            m_projectile.OnPostUpdate += this.HandlePostUpdate;
            m_projectile.AdditionalScaleMultiplier *= 2;
            m_projectile.baseData.damage *= 5;
            m_projectile.OnHitEnemy += OnHitEnemy;
        }
        private Projectile m_projectile;
        private void HandlePostUpdate(Projectile proj)
        {
            if (proj && proj.GetElapsedDistance() > 1)
            {
                proj.RuntimeUpdateScale(0.5f);
                proj.baseData.damage /= 5;
                proj.OnPostUpdate -= this.HandlePostUpdate;
            }
        }
        private void OnHitEnemy(Projectile proj, SpeculativeRigidbody enemy, bool fatal)
        {
            if (enemy && enemy.healthHaver && fatal && proj && proj.GetElapsedDistance() < 1 && proj.ProjectilePlayerOwner() && proj.ProjectilePlayerOwner().PlayerHasActiveSynergy("Any Way The Wind Blows"))
            {
                for (int i = 0; i < 5; i++)
                {
                    GameObject spawned = (PickupObjectDatabase.GetById(520) as Gun).DefaultModule.projectiles[0].InstantiateAndFireInDirection(proj.LastPosition, UnityEngine.Random.Range(0, 360));
                    Projectile proj2 = spawned.GetComponent<Projectile>();
                    proj2.AssignToPlayer(proj.ProjectilePlayerOwner());
                    proj2.gameObject.AddComponent<PierceDeadActors>();
                }
            }
        }
    }
}
