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
    public class NNGundertale : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Gundertale", "gundertale");
            Game.Items.Rename("outdated_gun_mods:gundertale", "nn:gundertale");
            var behav = gun.gameObject.AddComponent<NNGundertale>();
            behav.preventNormalFireAudio = true;
            behav.preventNormalReloadAudio = true;
            gun.SetShortDescription("Boom Boom Boom Boom");
            gun.SetLongDescription("It takes a lunatic to be a legend." + "\n\nThis powerful explosive weapon has one major drawback; it is capable of damaging it's bearer. You'd think more bombs would do that, but the Gungeon forgives.");

            gun.SetupSprite(null, "gundertale_idle_001", 8);

            gun.SetAnimationFPS(gun.shootAnimation, 12);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);

            //GUN STATS
            gun.DefaultModule.ammoCost = 0;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1f;
            gun.DefaultModule.cooldownTime = 10000000000f;
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.DefaultModule.numberOfShotsInClip = 10;
            gun.barrelOffset.transform.localPosition = new Vector3(2.5f, 0.68f, 0f);
            gun.SetBaseMaxAmmo(30);

            //BULLET STATS
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage *= 0f;
            projectile.baseData.speed *= 0f;
            projectile.sprite.renderer.enabled = false;


            gun.quality = PickupObject.ItemQuality.A;
            ETGMod.Databases.Items.Add(gun, null, "ANY");

        }

        protected override void Update()
        {
            if (!this.gun.RuntimeModuleData[this.gun.DefaultModule].onCooldown)
            {
                this.gun.RuntimeModuleData[this.gun.DefaultModule].onCooldown = true;
            }
            base.Update();
        }
        private void onDodgeRolledOverBullet(Projectile bullet)
        {
            if (this.gun.CurrentOwner.CurrentGun == this.gun && bullet.Owner && bullet.Owner is AIActor)
            {
                MakeEnemyNPC(bullet.Owner.aiActor);
            }
        }
        private void onDodgeRolledIntoEnemy(PlayerController player, AIActor enemy)
        {
            if (this.gun.CurrentOwner.CurrentGun == this.gun && enemy && enemy is AIActor)
            {
                enemy.healthHaver.flashesOnDamage = false;
                enemy.healthHaver.RegenerateCache();
                MakeEnemyNPC(enemy.aiActor);
            }
        }
        private void MakeEnemyNPC(AIActor enemy)
        {
            var CurrentRoom = enemy.transform.position.GetAbsoluteRoom();
            CurrentRoom.DeregisterEnemy(enemy);
            if (enemy.specRigidbody)
            {
                UnityEngine.Object.Destroy(enemy.specRigidbody);
            }
            if (enemy.behaviorSpeculator)
            {
                UnityEngine.Object.Destroy(enemy.behaviorSpeculator);
            }
            if (enemy.healthHaver)
            {
                enemy.healthHaver.IsVulnerable = false;
                enemy.healthHaver.bossHealthBar = HealthHaver.BossBarType.None;
                enemy.healthHaver.EndBossState(false);
            }
            if (enemy.aiAnimator)
            {
                enemy.aiAnimator.PlayUntilCancelled("idle", false, null, -1f, false);
            }
            //GameUIBossHealthController bossUI = enemy.GetComponent<GameUIBossHealthController>()
        }
        protected override void OnPickedUpByPlayer(PlayerController player)
        {
            player.OnDodgedProjectile += this.onDodgeRolledOverBullet;
            player.OnRolledIntoEnemy += this.onDodgeRolledIntoEnemy;
            base.OnPickedUpByPlayer(player);
        }
        protected override void OnPostDroppedByPlayer(PlayerController player)
        {
            player.OnDodgedProjectile -= this.onDodgeRolledOverBullet;
            player.OnRolledIntoEnemy -= this.onDodgeRolledIntoEnemy;
            base.OnPostDroppedByPlayer(player);
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}