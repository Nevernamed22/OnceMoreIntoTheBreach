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

    public class Pencil : GunBehaviour
    {

        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Pencil", "pencil");
            Game.Items.Rename("outdated_gun_mods:pencil", "nn:pencil");
            gun.gameObject.AddComponent<Pencil>();
            gun.SetShortDescription("Me Hoy Minoy");
            gun.SetLongDescription("Sketches out stationary bullets in the air. Reload to send your drawings flying!" + "\n\nAbandoned in the Gungeon by a grieving artist with really big hands.");

            gun.SetupSprite(null, "pencil_idle_001", 8);

            gun.SetAnimationFPS(gun.shootAnimation, 17);
            gun.doesScreenShake = false;
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(56) as Gun, true, false);
            gun.PreventNormalFireAudio = true;
            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0.1f;
            gun.muzzleFlashEffects.type = VFXPoolType.None;

            gun.DefaultModule.cooldownTime = 0.0001f;
            gun.DefaultModule.numberOfShotsInClip = 50000;
            gun.barrelOffset.transform.localPosition = new Vector3(2.25f, 0.31f, 0f);
            gun.SetBaseMaxAmmo(50000);
            gun.ammo = 50000;

            //BULLET STATS
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage *= 0.4f;
            projectile.baseData.speed *= 0.0001f;
            projectile.SetProjectileSpriteRight("pencil_projectile", 4, 4, false, tk2dBaseSprite.Anchor.MiddleCenter, 4, 4);

            projectile.transform.parent = gun.barrelOffset;

            // Here we just set the quality of the gun and the "EncounterGuid", which is used by Gungeon to identify the gun.
            gun.quality = PickupObject.ItemQuality.B;
            gun.encounterTrackable.EncounterGuid = "this is the Pencil";
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            pencilID = gun.PickupObjectId;
        }
        public static int pencilID;
        public override void PostProcessProjectile(Projectile projectile)
        {
            PlayerController player = projectile.Owner as PlayerController;
            if (player != null && player.PlayerHasActiveSynergy("Freehand"))
            {
                InstantTeleportToPlayerCursorBehav tp = projectile.gameObject.GetOrAddComponent<InstantTeleportToPlayerCursorBehav>();
            }
            ActiveBullets.Add(projectile);
            projectile.specRigidbody.OnPreTileCollision += this.onhit;
            if (player.PlayerHasActiveSynergy("Stationary")) projectile.OnHitEnemy += this.OnHitEnemy;
            base.PostProcessProjectile(projectile);
        }
        private void onhit(SpeculativeRigidbody myrigidbody, PixelCollider mypixelcollider, PhysicsEngine.Tile tile, PixelCollider tilepixelcollider)
        {
            // myrigidbody.projectile.ForceDestruction();
        }
        private void OnHitEnemy(Projectile self, SpeculativeRigidbody enemy, bool fatal)
        {
            if (ActiveBullets.Contains(self))
            {
                if (enemy && enemy.gameActor)
                {
                    float chance = 1f;
                    if (enemy.healthHaver && enemy.healthHaver.IsBoss) chance = 0.33f;
                    enemy.gameActor.DelelteOwnedBullets(chance);
                }
            }
        }
        public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
        {
            if (ActiveBullets.Count > 0)
            {
                foreach (Projectile bullet in ActiveBullets)
                {
                    if (bullet)
                    {
                        Vector2 vector = player.CenterPosition;
                        Vector2 normalized = (player.unadjustedAimPoint.XY() - vector).normalized;
                        bullet.SendInDirection(normalized, false, true);
                        bullet.baseData.speed *= 10000;
                        //bullet.Speed *= 1000;
                        bullet.UpdateSpeed();
                        BulletsToRemoveFromActiveBullets.Add(bullet);
                    }
                }
                foreach (Projectile bullet in BulletsToRemoveFromActiveBullets)
                {
                    ActiveBullets.Remove(bullet);
                }
                BulletsToRemoveFromActiveBullets.Clear();
            }
            base.OnReloadPressed(player, gun, bSOMETHING);
        }
        public static List<Projectile> ActiveBullets = new List<Projectile>() { };
        public static List<Projectile> BulletsToRemoveFromActiveBullets = new List<Projectile>() { };


        public Pencil()
        {
        }
    }
}

