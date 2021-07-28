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
    public class KineticBlaster : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Kinetic Blaster", "kineticblaster");
            Game.Items.Rename("outdated_gun_mods:kinetic_blaster", "nn:kinetic_blaster");
            gun.gameObject.AddComponent<KineticBlaster>();
            gun.SetShortDescription("Knock Knock Knockin");
            gun.SetLongDescription("Converts chemical potential energy into potent kinetic energy."+"\n\nOlder than most guns in the Gungeon. In it's hayday, it was even powerful enough to grant flight!");

            gun.SetupSprite(null, "kineticblaster_idle_001", 8);

            gun.SetAnimationFPS(gun.shootAnimation, 12);

            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(402) as Gun).gunSwitchGroup;
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.2f;
            gun.DefaultModule.cooldownTime = 0.2f;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(228) as Gun).muzzleFlashEffects;
            gun.DefaultModule.numberOfShotsInClip = 10;
            gun.barrelOffset.transform.localPosition = new Vector3(2.18f, 0.5f, 0f);
            gun.SetBaseMaxAmmo(300);
            gun.ammo = 300;

            //BULLET STATS
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage = 7f;
            projectile.baseData.force *= 6f;

            projectile.SetProjectileSpriteRight("kineticblaster_projectile", 10, 10, true, tk2dBaseSprite.Anchor.MiddleCenter, 8, 8);
            projectile.hitEffects.alwaysUseMidair = true;
            projectile.hitEffects.overrideMidairDeathVFX = EasyVFXDatabase.BlueFrostBlastVFX;

            projectile.transform.parent = gun.barrelOffset;

            gun.quality = PickupObject.ItemQuality.C;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
        }
        public override void OnPostFired(PlayerController player, Gun gun)
        {
            Vector2 vec = gun.CurrentAngle.DegreeToVector2().Rotate(180);
            player.knockbackDoer.ApplyKnockback(vec, 30);
            base.OnPostFired(player, gun);
        }
        public KineticBlaster()
        {

        }
    }
}
