using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;
using Gungeon;
using MonoMod;
using UnityEngine;
using ItemAPI;

namespace NevernamedsItems
{

    public class Blowgun : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Blowgun", "blowgun");
            Game.Items.Rename("outdated_gun_mods:blowgun", "nn:blowgun");
            gun.gameObject.AddComponent<Blowgun>();
            gun.SetShortDescription("Huff and Puff");
            gun.SetLongDescription("Relies on lung strength to propel poisonous darts."+"\n\nRobots may need to hold it up to a cooling vent or something.");

            gun.SetupSprite(null, "blowgun_idle_001", 8);

            gun.SetAnimationFPS(gun.shootAnimation, 15);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(56) as Gun, true, false);

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0.5f;
            gun.DefaultModule.cooldownTime = 0.2f;
            gun.DefaultModule.numberOfShotsInClip = 1;
            gun.barrelOffset.transform.localPosition = new Vector3(1.12f, 0.18f, 0f);
            gun.SetBaseMaxAmmo(200);

            //BULLET STATS
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.transform.parent = gun.barrelOffset;
            projectile.baseData.speed *= 2f;
            projectile.damageTypes |= CoreDamageTypes.Poison;
            ExtremelySimplePoisonBulletBehaviour poisoning = projectile.gameObject.AddComponent<ExtremelySimplePoisonBulletBehaviour>();
            poisoning.procChance = 1;
            poisoning.useSpecialTint = false;
            projectile.SetProjectileSpriteRight("blowgun_projectile", 16, 7, false, tk2dBaseSprite.Anchor.MiddleCenter, 15, 8);

            gun.quality = PickupObject.ItemQuality.D;
            gun.encounterTrackable.EncounterGuid = "this is the Blowgun";
            ETGMod.Databases.Items.Add(gun, null, "ANY");

            foreach (tk2dSpriteAnimationClip clip in AdjustGunPosition.GetGunAnimationClips(gun))
            {
                foreach (tk2dSpriteAnimationFrame frame in clip.frames)
                {
                    frame.spriteCollection.spriteDefinitions[frame.spriteId].ApplyOffset(new Vector2(0, 0f));
                }
            }

            BlowgunID = gun.PickupObjectId;
        }
        public override void PostProcessProjectile(Projectile projectile)
        {
            PlayerController player = projectile.Owner as PlayerController;
            base.PostProcessProjectile(projectile);
            if (player.PlayerHasActiveSynergy("Old and New"))
            {
                projectile.AppliesStun = true;
                projectile.AppliedStunDuration += 10f;
                projectile.StunApplyChance = 1f;
            }
        }
        public static int BlowgunID;
        public Blowgun()
        {

        }
    }
}