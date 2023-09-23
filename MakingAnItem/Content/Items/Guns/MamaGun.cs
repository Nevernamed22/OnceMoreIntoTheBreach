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
            Gun gun = ETGMod.Databases.Items.NewGun("Mama", "mama");
            Game.Items.Rename("outdated_gun_mods:mama", "nn:mama");
            gun.gameObject.AddComponent<MamaGun>();
            gun.SetShortDescription("Just killed a man...");
            gun.SetLongDescription("Heavy with symbolism, this gun was brought to the Gungeon by a poor boy with many regrets."+"\n\nDoes significantly more damage when placed right against an enemy, and fired at point-blank range.");

            gun.SetupSprite(null, "mama_idle_001", 8);

            gun.SetAnimationFPS(gun.shootAnimation, 15);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(56) as Gun, true, false);

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1f;
            gun.DefaultModule.cooldownTime = 0.1f;
            gun.DefaultModule.numberOfShotsInClip = 15;
            gun.barrelOffset.transform.localPosition = new Vector3(1.37f, 0.56f, 0f);
            gun.SetBaseMaxAmmo(200);
            gun.gunClass = GunClass.PISTOL;
            //BULLET STATS
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage *= 1.5f;
            projectile.baseData.speed *= 1f;
            //projectile.shouldRotate = true;
            PutAGunAgainstHisHeadPulledMyTriggerNowHesDeadBehaviour WillYouDoTheFandango = projectile.gameObject.AddComponent<PutAGunAgainstHisHeadPulledMyTriggerNowHesDeadBehaviour>();
            projectile.transform.parent = gun.barrelOffset;

            gun.quality = PickupObject.ItemQuality.B;
            gun.encounterTrackable.EncounterGuid = "this is Mama";
            ETGMod.Databases.Items.Add(gun, null, "ANY");

        }        
        public override void OnPostFired(PlayerController player, Gun gun)
        {
        }
        public MamaGun()
        {

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
            if (enemy && enemy.healthHaver && fatal && proj && proj.GetElapsedDistance() < 1 &&proj.ProjectilePlayerOwner() && proj.ProjectilePlayerOwner().PlayerHasActiveSynergy("Any Way The Wind Blows"))
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
